using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System.Text;

namespace Kenny.MessageBus
{
	public class AzureServiceBusMessageBus : IMessageBus
	{
		private string connectionString = "Endpoint=sb://kennyrestaurant.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=lq5Rg1a91f1qavtou8mrbCTlGfhcyRRNMQ4SYhOc4MQ=";

		public async Task PublicMessage(BaseMessage message, string topicName)
		{
			await using var client = new ServiceBusClient(connectionString);
			var sender = client.CreateSender(topicName);

			var jsonMessage = JsonConvert.SerializeObject(message);
			var finalMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(jsonMessage))
			{ 
				CorrelationId = Guid.NewGuid().ToString()
			};
			await sender.SendMessageAsync(finalMessage);
			await client.DisposeAsync();
		}
	}
}
