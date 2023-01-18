using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kenny.MessageBus
{
	public class AzureServiceBusMessageBus : IMessageBus
	{
		private string connectionString = "Endpoint=sb://kennyrestaurant.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=lq5Rg1a91f1qavtou8mrbCTlGfhcyRRNMQ4SYhOc4MQ=";

		public async Task PublicMessage(BaseMessage message, string topicName)
		{
			ISenderClient senderClient = new TopicClient(connectionString, topicName);

			var jsonMessage = JsonConvert.SerializeObject(message);
			var finalMessage = new Message(Encoding.UTF8.GetBytes(jsonMessage))
			{ 
				CorrelationId = Guid.NewGuid().ToString()
			};
			await senderClient.SendAsync(finalMessage);
			await senderClient.CloseAsync();
		}
	}
}
