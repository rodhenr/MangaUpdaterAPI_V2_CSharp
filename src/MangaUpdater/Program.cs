using System.Text.Json.Serialization;
using MangaUpdater.Exceptions;
using MangaUpdater.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Built-in Services
builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddHttpClient();
builder.Services.AddCors();

// Custom Services
builder.Services.AddApiServices().AddDataService().AddCoreServices(builder.Configuration);
builder.Services.AddJwtAuthenticationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

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