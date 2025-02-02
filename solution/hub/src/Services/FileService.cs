namespace hub.Services;

public class FileService
{
    private readonly string _fileStoragePath;

    public FileService(IConfiguration configuration)
    {
        _fileStoragePath = configuration["FileStoragePath"];

        if (!Directory.Exists(_fileStoragePath))
        {
            Directory.CreateDirectory(_fileStoragePath);
        }
    }
    
    public async Task<string> SaveFileAsync(IFormFile file, Guid fileId)
    {
        var fileName = $"{fileId}_{file.FileName}";
        var filePath = Path.Combine(_fileStoragePath, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return fileName;
    }

}