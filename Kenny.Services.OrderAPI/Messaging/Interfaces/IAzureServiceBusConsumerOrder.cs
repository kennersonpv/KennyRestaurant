namespace Kenny.Services.OrderAPI.Messaging.Interfaces
{
    public interface IAzureServiceBusConsumerOrder
    {
        Task Start();
        Task Stop();
    }
}
