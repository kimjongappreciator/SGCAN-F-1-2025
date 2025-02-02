using System.Text;
using hub.Models;
using hub.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Hub.Controllers;

public class FilesController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly string _fileStoragePath;
    private readonly LinkExtractorService _linkExtractorService;
    private HttpClient client = new HttpClient();
    
    public FilesController(AppDbContext context, IConfiguration configuration, LinkExtractorService linkExtractorService)
    {
        _context = context;
        _fileStoragePath = configuration["FileStoragePath"];
        _linkExtractorService = linkExtractorService;

        // Crear el directorio si no existe
        if (!Directory.Exists(_fileStoragePath))
        {
            Directory.CreateDirectory(_fileStoragePath);
        }
    }
    
    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile([FromForm] IFormFile file, [FromForm] Guid userId)
    {
        if (file == null)
        {
            return BadRequest("No file uploaded.");
        }

        if (file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        var fileId = Guid.NewGuid();
        var fileName = $"{file.FileName}";
        var filePath = Path.Combine(_fileStoragePath, fileName);
        
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }
        
        var fileEntity = new FileEntity
        {
            Id = fileId,
            FileName = file.FileName,
            UploadDate = DateTime.UtcNow,
            UserId = userId
        };

        _context.Files.Add(fileEntity);
        await _context.SaveChangesAsync();

        return Ok(new { FileId = fileId, FileName = file.FileName });
    }

    [HttpPost("process")]
    public async Task<IActionResult> ProcessFile([FromBody] ProcessFile file)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var url = "http://localhost:8000/process/";
        
        string payload = JsonSerializer.Serialize(file);
        Console.WriteLine(payload);
        var content = new StringContent(payload,Encoding.UTF8, "application/json");
        
        HttpResponseMessage response = await client.PostAsync(url, content);
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine(response.StatusCode);
            return BadRequest("error al contactar al api");
        }
        return Ok("Archivo processado correctamente.");
    }
    

    [HttpGet("files/{userId}")]
    public async Task<IActionResult> GetFilesByUser(Guid userId)
    {
        var user = await _context.User.FindAsync(userId);
        if (user == null)
        {
            return NotFound("Usuario no encontrado.");
        }

        var files = await _context.Files.Where(f => f.UserId == userId).Select(f => new
        {
            f.Id,
            f.FileName,
            f.UploadDate
        }).ToListAsync();
        return Ok(files);
    }
    
    [HttpGet("files/{fileId}/links")]
    public async Task<IActionResult> GetFileLinks(Guid fileId)
    {
        var fileEntity = await _context.Files.FindAsync(fileId);
        if (fileEntity == null)
        {
            return NotFound("Archivo no encontrado.");
        }

        string url = "http://localhost:8000/content/" + fileEntity.FileName;
        HttpResponseMessage response = await client.GetAsync(url);
        
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return Ok(content);
        }
        return NotFound("Archivo no encontrado.");
    }
    
}