namespace Kenny.Services.Email.Messaging.Interfaces
{
    public interface IAzureServiceBusConsumerEmail
    {
        Task Start();
        Task Stop();
    }
}
