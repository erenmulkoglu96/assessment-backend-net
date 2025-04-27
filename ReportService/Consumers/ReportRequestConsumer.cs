using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ReportService.Data.ReportService.Data;
using ReportService.Models;
using ReportService.Services;
using System.Text.Json;
using System.Threading;

namespace ReportService.Consumers;

public class ReportRequestConsumer
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ReportRequestConsumer> _logger;
    private readonly IConfiguration _configuration;

    public ReportRequestConsumer(IServiceProvider serviceProvider, ILogger<ReportRequestConsumer> logger, IConfiguration configuration)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _configuration = configuration;
    }

    public async Task StartAsync(CancellationToken stoppingToken)
    {
        var config = new ConsumerConfig
        {
            GroupId = "report-consumer-group",
            BootstrapServers = _configuration["Kafka:BootstrapServers"],
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = true
        };

        var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        consumer.Subscribe("report-requests");

        _logger.LogInformation("Kafka consumer başlatıldı.");

        _ = Task.Run(async () =>
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var cr = consumer.Consume(TimeSpan.FromSeconds(1)); // Blocking değil
                    if (cr == null) continue;

                    var message = JsonSerializer.Deserialize<ReportMessage>(cr.Message.Value);

                    using var scope = _serviceProvider.CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<ReportDbContext>();
                    var reportService = scope.ServiceProvider.GetRequiredService<IReportService>();

                    var report = await dbContext.Reports.FindAsync(message.ReportId);
                    if (report != null)
                    {
                        var content = await reportService.GenerateReportContentAsync(report.Id);

                        var csvContent = await reportService.GenerateReportCsvContentAsync(report.Id);

                        // JSON dosyasını kaydet
                        var fileName = $"report-{report.Id}.json";
                        var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "reports");
                        Directory.CreateDirectory(folderPath);
                        var filePath = Path.Combine(folderPath, fileName);
                        await File.WriteAllTextAsync(filePath, content);

                        // CSV dosyasını kaydet
                        var csvFileName = $"report-{report.Id}.csv";
                        var csvFilePath = Path.Combine(folderPath, csvFileName);
                        await File.WriteAllTextAsync(csvFilePath, csvContent);

                        // Veritabanına kaydet
                        report.FilePath = $"/reports/{fileName}";
                        report.Content = content;
                        report.CsvPath = $"/reports/{csvFileName}";
                        report.CsvContent = csvContent; // ❗ Buraya ekliyoruz
                        report.Status = ReportStatus.Completed;
                        report.CompletedAt = DateTime.UtcNow;

                        await dbContext.SaveChangesAsync();
                        _logger.LogInformation($"✔ Rapor işlendi: {report.Id}");
                    }

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Kafka döngüsü hatası");
                }
            }
        });

        await Task.CompletedTask;
    }


    private class ReportMessage
    {
        public Guid ReportId { get; set; }
    }
}

