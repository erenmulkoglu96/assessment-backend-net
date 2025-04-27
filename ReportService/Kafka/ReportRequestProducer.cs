using Confluent.Kafka;
using System.Text.Json;

namespace ReportService.Kafka
{
 

    public class ReportRequestProducer
    {
        private readonly IConfiguration _config;

        public ReportRequestProducer(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendMessageAsync(Guid reportId)
        {
            var producerConfig = new ProducerConfig
            {
                BootstrapServers = _config["Kafka:BootstrapServers"]
            };

            using var producer = new ProducerBuilder<Null, string>(producerConfig).Build();

            var message = JsonSerializer.Serialize(new { ReportId = reportId });

            await producer.ProduceAsync("report-requests", new Message<Null, string> { Value = message });
        }
    }

}
