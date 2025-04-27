using Microsoft.EntityFrameworkCore;
using ReportService.Data;
using ReportService.Kafka;
using ReportService.Services;
using ReportService.Consumers;
using ReportService.Data.ReportService.Data;
using ReportService.BackgroundServices;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ReportDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ReportRequestProducer>();
builder.Services.AddScoped<IReportService, ReportService.Services.ReportService>();


builder.Services.AddSingleton<ReportRequestConsumer>();
builder.Services.AddSingleton<IHostedService, ConsumerHostedService>();
builder.Services.AddHostedService<ScheduledReportService>();



var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ReportService v1");
    c.RoutePrefix = "swagger";
});

app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
