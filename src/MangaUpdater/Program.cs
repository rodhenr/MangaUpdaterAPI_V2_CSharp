using System.Text.Json.Serialization;
using MangaUpdater;
using MangaUpdater.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Built-in Services
builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddHttpClient();
builder.Services.AddCors();

// Custom Services
builder.Services.AddCustomServices(builder.Configuration);

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Hangfire
app.AddHangfireBuilder(app.Configuration);

// Custom Middlewares
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

public partial class Program
{
}