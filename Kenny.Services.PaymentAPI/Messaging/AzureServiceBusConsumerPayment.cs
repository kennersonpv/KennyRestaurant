using Azure.Messaging.ServiceBus;
using Kenny.MessageBus;
using Kenny.Services.PaymentAPI.Messages;
using Kenny.Services.PaymentAPI.Messaging.Interfaces;
using Newtonsoft.Json;
using PaymentProcessor.Interfaces;
using System.Text;

namespace Kenny.Services.PaymentAPI.Messaging
{
	public class AzureServiceBusConsumerPayment : IAzureServiceBusConsumerPayment
	{
		private readonly string serviceBusConnectionString;
		private readonly string orderPaymentProcessSubscription;
		private readonly string orderPaymentProcessTopic;
		private readonly string orderupdatepaymentsresulttopic;

		private ServiceBusProcessor orderPaymentProcessor;

		private readonly IConfiguration _configuration;
		private readonly IMessageBus _messageBus;
		private readonly IProcessPayment _processPayment;

		public AzureServiceBusConsumerPayment(IConfiguration configuration, IMessageBus messageBus, IProcessPayment processPayment)
		{
			_configuration = configuration;
			_messageBus = messageBus;
			_processPayment = processPayment;

			serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");
			orderPaymentProcessSubscription = _configuration.GetValue<string>("OrderPaymentProcessSubscription");
			orderPaymentProcessTopic = _configuration.GetValue<string>("OrderPaymentProcessTopic");
			orderupdatepaymentsresulttopic = _configuration.GetValue<string>("OrderUpdatePaymentsResultTopic");

			var client = new ServiceBusClient(serviceBusConnectionString);
			orderPaymentProcessor = client.CreateProcessor(orderPaymentProcessTopic, orderPaymentProcessSubscription);
		}

		public async Task Start()
		{
			orderPaymentProcessor.ProcessMessageAsync += ProcessPayments;
			orderPaymentProcessor.ProcessErrorAsync += ErrorHandler;
			await orderPaymentProcessor.StartProcessingAsync();
		}

		public async Task Stop()
		{
			await orderPaymentProcessor.StopProcessingAsync();
			await orderPaymentProcessor.DisposeAsync();
		}

		Task ErrorHandler(ProcessErrorEventArgs args)
		{
			Console.WriteLine(args.Exception.ToString());
			return Task.CompletedTask;
		}

		private async Task ProcessPayments(ProcessMessageEventArgs args)
		{
			var message = args.Message;
			var body = Encoding.UTF8.GetString(message.Body);

			var paymentRequestMessage = JsonConvert.DeserializeObject<PaymentRequestMessage>(body);

			var result = _processPayment.PaymentProcessor();

			var updatePaymentResultMessage = new UpdatePaymentResultMessage()
			{
				OrderId = paymentRequestMessage.OrderId,
				Status = result,
				Email = paymentRequestMessage.Email
			};

			try
			{
				await _messageBus.PublicMessage(updatePaymentResultMessage, orderupdatepaymentsresulttopic);
				await args.CompleteMessageAsync(args.Message);
			}
			catch
			{
				throw;
			}
		}
	}
}
