namespace Kenny.Services.PaymentAPI.Messaging.Interfaces
{
	public interface IAzureServiceBusConsumerPayment
	{
		Task Start();
		Task Stop();
	}
}
