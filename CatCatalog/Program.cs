using CatCatalog.Abstractions;
using CatCatalog.Contexts;
using CatCatalog.Options;
using CatCatalog.Services;
using CatCatalog.Workers;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Cat API",
        Version = "v1",
        Description = "An API to manage cats.",
        Contact = new OpenApiContact
        {
            Name = "Your Name",
            Email = "jsdrolias@gmail.com",
            Url = new Uri("https://yourwebsite.com")
        }
    });
    c.EnableAnnotations();
});

builder.Services.AddDbContext<CatDbContext>(
                       options =>
                       {
                           var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
                           var password = Environment.GetEnvironmentVariable("SA_PASSWORD");
                           connectionString = string.Format(connectionString!, password);
                           options.UseSqlServer(connectionString);

                       });

builder.Services.Configure<BlobStorageOptions>(
    builder.Configuration.GetSection(BlobStorageOptions.Section));

// Register services
builder.Services.AddScoped<ICatProcessingService, CatProcessingService>();
builder.Services.AddScoped<ICatWebClientService, CatWebClientService>();
builder.Services.AddHttpClient<ICatWebClientService, CatWebClientService>();
builder.Services.AddScoped<IJobService, JobService>();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddHttpClient<BlobStorageService>();

builder.Services.AddHostedService<JobProcessorService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cat API v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("AllowAllOrigins");

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CatDbContext>();
    db.Database.Migrate();
}

app.Run();
