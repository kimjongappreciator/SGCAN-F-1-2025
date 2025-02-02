using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
namespace hub.Models;
public class AppDbContext :DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<FileEntity> Files { get; set; }
    public DbSet<User> User { get; set; }
    public DbSet<ScrappingResults> scraping_results { get; set; }
}