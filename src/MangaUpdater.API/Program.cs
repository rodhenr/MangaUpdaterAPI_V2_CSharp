using System.Text.Json.Serialization;
using MangaUpdater.API;
using MangaUpdater.Core;
using MangaUpdater.Core.Common.Exceptions;
using MangaUpdater.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddHttpClient();
builder.Services.AddCors();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Injections
builder.Services.AddApiServices()
    .AddCoreServices()
    .AddDataService();
builder.Services.AddJwtAuthenticationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<ValidationExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(b => b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseHttpsRedirection();
app.UseAuthorization();

// app.UseHangfireDashboard("/hangfire", new DashboardOptions
// {
//     Authorization = new[]
//     {
//         new BasicAuthAuthorizationFilter(
//             new BasicAuthAuthorizationFilterOptions()
//             {
//                 RequireSsl = false,
//                 SslRedirect = false,
//                 LoginCaseSensitive = true,
//                 Users = new[]
//                 {
//                     new BasicAuthAuthorizationUser
//                     {
//                         Login = "Admin",
//                         PasswordClear = "123"
//                     }
//                 }
//             })
//     }
// });

// var monitoringApi = JobStorage.Current.GetMonitoringApi();

// var scheduledJobs = monitoringApi.ScheduledJobs(0, int.MaxValue);

// foreach (var job in scheduledJobs)
// {
//     BackgroundJob.Delete(job.Key);
// }

// BackgroundJob.Enqueue<IHangfireService>(task => task.AddHangfireJobs());

app.MapControllers();

app.Run();