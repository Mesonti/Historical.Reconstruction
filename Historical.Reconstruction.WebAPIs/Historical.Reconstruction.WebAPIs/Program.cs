using Historical.Reconstruction.WebAPIs.DbContexts;
using Historical.Reconstruction.WebAPIs.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Create the public file root before Build configures its file provider.
builder.Environment.WebRootPath ??= Path.Combine(builder.Environment.ContentRootPath, "wwwroot");
Directory.CreateDirectory(Path.Combine(builder.Environment.WebRootPath, "uploads"));


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddDbContext<FindsDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("SourcesConnection")));

builder.Services.AddScoped<IFindService, FindService>();
builder.Services.AddScoped<IFindImageService, FindImageService>();
builder.Services.AddScoped<IRegionService, RegionService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.MapOpenApi();
}

//app.UseHttpsRedirection();

//app.UseAuthorization();

app.UseStaticFiles();
app.MapControllers();

app.Run();


