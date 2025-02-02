using hub.Models;
using hub.Services;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options=>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddScoped<FileService>();
builder.Services.AddScoped<LinkExtractorService>();
builder.Services.AddCors(op =>
{
    op.AddPolicy("angularApp", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().Build();
    });
});
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated();
}

app.MapControllers();
app.UseCors("angularApp");
app.Run();