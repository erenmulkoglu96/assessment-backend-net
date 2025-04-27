using ReportService.Consumers;

public class ConsumerHostedService : IHostedService
{
    private readonly ReportRequestConsumer _consumer;

    public ConsumerHostedService(ReportRequestConsumer consumer)
    {
        _consumer = consumer;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        return _consumer.StartAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
