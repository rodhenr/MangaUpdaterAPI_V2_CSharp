using System.Text.Json.Serialization;
using Hangfire;
using MangaUpdater.API;
using MangaUpdater.Core;
using MangaUpdater.Core.Common.Exceptions;
using MangaUpdater.Data;
using MangaUpdater.Core.Common.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Built-in Services
builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddHttpClient();
builder.Services.AddCors();

// Custom Services
builder.Services.AddApiServices().AddCoreServices().AddDataService();
builder.Services.AddJwtAuthenticationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

// Hangfire
builder.Services.AddHangfire(configuration => configuration.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseIgnoredAssemblyVersionTypeResolver()
        .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection"))
        .UseMediatR());

builder.Services.AddHangfireServer(options => options.WorkerCount = 2);

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Hangfire
app.AddHangfireBuilder();

// Custom Middleware
app.UseMiddleware<ValidationExceptionHandlingMiddleware>();

// Built-in 
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(b => b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();