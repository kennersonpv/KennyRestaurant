using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kenny.MessageBus
{
	public interface IMessageBus
	{
		Task PublicMessage(BaseMessage message, string topicName);
	}
}
