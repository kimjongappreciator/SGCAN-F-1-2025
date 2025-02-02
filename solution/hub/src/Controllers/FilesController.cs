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
    private readonly string _scrappinUrl;
    private readonly LinkExtractorService _linkExtractorService;
    private HttpClient client = new HttpClient();
    
    public FilesController(AppDbContext context, IConfiguration configuration, LinkExtractorService linkExtractorService)
    {
        _context = context;
        _fileStoragePath = configuration["FileStoragePath"];
        _scrappinUrl = configuration["ScrappingUrl"];
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
        var url = _scrappinUrl;
        Console.WriteLine(url);
        
        var path = Path.Combine(_fileStoragePath, file.filename);
        var finalPath = Path.GetFullPath(path);
        file.filename = finalPath;
        
        string payload = JsonSerializer.Serialize(file);
        var content = new StringContent(payload,Encoding.UTF8, "application/json");
        
        HttpResponseMessage response = await client.PostAsync(url, content);
        if (!response.IsSuccessStatusCode) {
            
            return BadRequest("Error while processing file.");
        }
        return Ok();
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
        var responseEntity = await _context.scraping_results.Where(f => f.fileId == fileId.ToString()).Select(
            f=> new
            {
               f.url,
               f.content,
               f.date,
               f.task_id,
               f.title
            }).ToListAsync();
        if (responseEntity.Count < 1)
        {
            return NotFound("Archivo no encontrado.");
        }

        return Ok(responseEntity);
    }
    
    
    
}