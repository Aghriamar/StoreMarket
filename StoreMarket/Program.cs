using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using StoreMarket.Abstraction;
using StoreMarket.Models;
using StoreMarket.Repo;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var config = new ConfigurationBuilder();
config.AddJsonFile("appsettings.json");
var cfg = builder.Build();
builder.Services.AddAutoMapper(typeof(MappingProfiler));
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(cb =>
{
    cb.RegisterType<ProductRepository>().As<IProductRepository>();
});
builder.Services.AddSingleton<IProductRepository, ProductRepository>();
builder.Services.AddMemoryCache(x => x.TrackStatistics = true);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var staticFilesPath = Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles");
Directory.CreateDirectory(staticFilesPath);
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        staticFilesPath), RequestPath = "/static"
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
