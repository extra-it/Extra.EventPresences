using Extra.EventPresences.Middleware.Managers.Interfaces;
using Extra.EventPresences.Middleware.Managers;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore;
using Extra.EventPresences.Model;
using Extra.EventPresences.Middleware.Services.Interfaces;
using Extra.EventPresences.Middleware.Services;
using System.Reflection;
using Microsoft.OpenApi.Models;
using Extra.EventPresences.DTO.Dto;
using Extra.EventPresences.DTO;

var builder = WebApplication.CreateBuilder(args);
var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

IConfiguration Configuration = new ConfigurationBuilder()
   .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
   .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true)
   .AddEnvironmentVariables()
   .AddCommandLine(args)
   .Build();

var connectionString = Configuration.GetValue<string>("DatabaseSettings:DBConnectinString");
builder.Services.AddDbContext<DBDataContext>(options =>
{
    options.UseSqlServer(connectionString);
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "ExtraEventPresences", Version = "v1" });
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddScoped<iConfigurationManager, Extra.EventPresences.Middleware.Managers.ConfigurationManager>();
builder.Services.AddScoped<iLogManager, LogManager>();
builder.Services.AddScoped<iUserManager, UserManager>();
builder.Services.AddScoped<iFunctionalityManager, FunctionalityManager>();
builder.Services.AddScoped<iUserService, UserService>();
builder.Services.AddScoped<iFunctionalityService, FunctionalityService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
//{
app.UseSwagger();
app.UseSwaggerUI();
// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
// specifying the Swagger JSON endpoint.
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MVCCallWebAPI");
});
//}

app.UseHttpsRedirection();

//app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
