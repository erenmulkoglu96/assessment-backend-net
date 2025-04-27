using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ReportService.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ReportService.BackgroundServices
{
    public class ScheduledReportService : BackgroundService
    {
        private readonly ILogger<ScheduledReportService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public ScheduledReportService(ILogger<ScheduledReportService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("⏰ ScheduledReportService başlatıldı.");

            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTime.UtcNow.AddHours(3); // Türkiye saati (UTC+3)
                var nextRunTime = now.Date.AddDays(1).AddHours(2); // Yani yarın 02:00

                var delay = nextRunTime - now;
                if (delay.TotalMilliseconds < 0)
                    delay = TimeSpan.FromHours(24);

                _logger.LogInformation($"🕑 Sonraki rapor oluşturulacak zaman: {nextRunTime}");

                await Task.Delay(delay, stoppingToken);

                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var reportService = scope.ServiceProvider.GetRequiredService<IReportService>();

                    await reportService.RequestReportAsync();

                    _logger.LogInformation("📦 Otomatik rapor isteği gönderildi.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "🚨 Otomatik rapor oluştururken hata oluştu.");
                }
            }
        }
    }
}
