using Historical.Reconstruction.WebAPIs.DbContexts;
using Historical.Reconstruction.WebAPIs.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();

builder.Services.AddDbContext<FindsDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("SourcesConnection")));

builder.Services.AddScoped<IFindService, FindService>();
builder.Services.AddScoped<IRegionService, RegionService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
}

app.UseHttpsRedirection();

//app.UseAuthorization();


app.MapControllers();

app.Run();

