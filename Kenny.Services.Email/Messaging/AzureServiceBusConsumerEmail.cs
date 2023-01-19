using Azure.Messaging.ServiceBus;
using Kenny.Services.Email.Messages;
using Kenny.Services.Email.Messaging.Interfaces;
using Kenny.Services.Email.Repository;
using Newtonsoft.Json;
using System.Text;

namespace Kenny.Services.Email.Messaging
{
    public class AzureServiceBusConsumerEmail : IAzureServiceBusConsumerEmail
	{
		private readonly EmailRepository _emailRepository;
		private readonly string serviceBusConnectionString;
		private readonly string subscriptionEmail;
		private readonly string orderUpdatePaymentsResultTopic;		

		private ServiceBusProcessor checkOutProcessor;
		private ServiceBusProcessor orderUpdatePaymetStatusProcessor;

		private readonly IConfiguration _configuration;

		public AzureServiceBusConsumerEmail(EmailRepository emailRepository, IConfiguration configuration)
		{
			_emailRepository = emailRepository;
			_configuration = configuration;

			serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");
			subscriptionEmail = _configuration.GetValue<string>("SubscriptionName");
			orderUpdatePaymentsResultTopic = _configuration.GetValue<string>("OrderUpdatePaymentsResultTopic");

			var client = new ServiceBusClient(serviceBusConnectionString);
			orderUpdatePaymetStatusProcessor = client.CreateProcessor(orderUpdatePaymentsResultTopic, subscriptionEmail);
		}

		public async Task Start()
		{
			orderUpdatePaymetStatusProcessor.ProcessMessageAsync += OnOrderPaymentReceived;
			orderUpdatePaymetStatusProcessor.ProcessErrorAsync += ErrorHandler;
			await orderUpdatePaymetStatusProcessor.StartProcessingAsync();
		}

		public async Task Stop()
		{
			await orderUpdatePaymetStatusProcessor.StopProcessingAsync();
			await orderUpdatePaymetStatusProcessor.DisposeAsync();
		}

		Task ErrorHandler(ProcessErrorEventArgs args)
		{
			Console.WriteLine(args.Exception.ToString());
			return Task.CompletedTask;
		}

		private async Task OnOrderPaymentReceived (ProcessMessageEventArgs args)
		{
			var message = args.Message;
			var body = Encoding.UTF8.GetString(message.Body);

			var paymentResultMessage = JsonConvert.DeserializeObject<UpdatePaymentResultMessage>(body);

			try
			{
				await _emailRepository.SendAndLogEmailAsync(paymentResultMessage);
				await args.CompleteMessageAsync(args.Message);
			}
			catch
			{
				throw;
			}
		}
	}
}
