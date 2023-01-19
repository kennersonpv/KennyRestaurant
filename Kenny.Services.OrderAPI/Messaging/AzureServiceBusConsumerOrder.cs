using Azure.Messaging.ServiceBus;
using Kenny.MessageBus;
using Kenny.Services.OrderAPI.Messages;
using Kenny.Services.OrderAPI.Messages.Dto;
using Kenny.Services.OrderAPI.Messaging.Interfaces;
using Kenny.Services.OrderAPI.Models;
using Kenny.Services.OrderAPI.Repository;
using Newtonsoft.Json;
using System.Text;

namespace Kenny.Services.OrderAPI.Messaging
{
    public class AzureServiceBusConsumerOrder : IAzureServiceBusConsumerOrder
	{
		private readonly OrderRepository _orderRepository;
		private readonly string serviceBusConnectionString;
		private readonly string checkoutMessageTopic;
		private readonly string subscriptionCheckOut;
		private readonly string orderPaymentProcessTopic;
		private readonly string orderUpdatePaymentsResultTopic;		

		private ServiceBusProcessor checkOutProcessor;
		private ServiceBusProcessor orderUpdatePaymetStatusProcessor;

		private readonly IConfiguration _configuration;
		private readonly IMessageBus _messageBus;

		public AzureServiceBusConsumerOrder(OrderRepository orderRepository, IConfiguration configuration, IMessageBus messageBus)
		{
			_orderRepository = orderRepository;
			_configuration = configuration;
			_messageBus = messageBus;

			serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");
			checkoutMessageTopic = _configuration.GetValue<string>("CheckoutMessageTopic");
			subscriptionCheckOut = _configuration.GetValue<string>("SubscriptionCheckOut");
			orderPaymentProcessTopic = _configuration.GetValue<string>("OrderPaymentProcessTopic");
			orderUpdatePaymentsResultTopic = _configuration.GetValue<string>("OrderUpdatePaymentsResultTopic");

			var client = new ServiceBusClient(serviceBusConnectionString);
			//Solution for Topics
			//checkOutProcessor = client.CreateProcessor(checkoutMessageTopic, subscriptionCheckOut);

			//Solution for queues
			checkOutProcessor = client.CreateProcessor(checkoutMessageTopic);
			orderUpdatePaymetStatusProcessor = client.CreateProcessor(orderUpdatePaymentsResultTopic, subscriptionCheckOut);
		}

		public async Task Start()
		{
			checkOutProcessor.ProcessMessageAsync += OnCheckoutMessageReceived;
			checkOutProcessor.ProcessErrorAsync += ErrorHandler;
			await checkOutProcessor.StartProcessingAsync();

			orderUpdatePaymetStatusProcessor.ProcessMessageAsync += OnOrderPaymentUpdateReceived;
			orderUpdatePaymetStatusProcessor.ProcessErrorAsync += ErrorHandler;
			await orderUpdatePaymetStatusProcessor.StartProcessingAsync();
		}

		public async Task Stop()
		{
			await checkOutProcessor.StopProcessingAsync();
			await checkOutProcessor.DisposeAsync();

			await orderUpdatePaymetStatusProcessor.StopProcessingAsync();
			await orderUpdatePaymetStatusProcessor.DisposeAsync();
		}

		Task ErrorHandler(ProcessErrorEventArgs args)
		{
			Console.WriteLine(args.Exception.ToString());
			return Task.CompletedTask;
		}

		private async Task OnOrderPaymentUpdateReceived(ProcessMessageEventArgs args)
		{
			var message = args.Message;
			var body = Encoding.UTF8.GetString(message.Body);

			var paymentResultMessage = JsonConvert.DeserializeObject<UpdatePaymentResultMessage>(body);
			await _orderRepository.UpdateOrderPaymentStatus(paymentResultMessage.OrderId, paymentResultMessage.Status);
			await args.CompleteMessageAsync(args.Message);
		}

		private async Task OnCheckoutMessageReceived (ProcessMessageEventArgs args)
		{
			var message = args.Message;
			var body = Encoding.UTF8.GetString(message.Body);

			var checkoutHeaderDto = JsonConvert.DeserializeObject<CheckoutHeaderDto>(body);

			var orderHeader = new OrderHeader()
			{
				UserId = checkoutHeaderDto.UserId,
				FirstName = checkoutHeaderDto.FirstName,
				LastName = checkoutHeaderDto.LastName,
				OrderDetails = new List<OrderDetails>(),
				CardNumber = checkoutHeaderDto.CardNumber,
				CouponCode = checkoutHeaderDto.CouponCode,
				CVV = checkoutHeaderDto.CVV,
				DiscountTotal = checkoutHeaderDto.DiscountTotal,
				Email = checkoutHeaderDto.Email,
				ExpiryMonthYear = checkoutHeaderDto.ExpiryMonthYear,
				OrderTime = DateTime.Now,
				OrderTotal = checkoutHeaderDto.OrderTotal,
				PaymentStatus = false,
				Phone = checkoutHeaderDto.Phone,
				PickupDateTime = checkoutHeaderDto.PickupDateTime
			};

			foreach(var detailList in checkoutHeaderDto.CartDetails)
			{
				OrderDetails orderDetails = new()
				{
					ProductId = detailList.ProductId,
					ProductName = detailList.Product.Name,
					Price = detailList.Product.Price,
					Count = detailList.Count
				};
				orderHeader.CartTotalItems += detailList.Count;
				orderHeader.OrderDetails.Add(orderDetails);
			}

			await _orderRepository.AddOrderAsync(orderHeader);

			var paymentRequestMessage = new PaymentRequestMessage()
			{
				Name = orderHeader.FirstName + " " + orderHeader.LastName,
				CardNumber = orderHeader.CardNumber,
				CVV = orderHeader.CVV,
				ExpiryMonthYear = orderHeader.ExpiryMonthYear,
				OrderId = orderHeader.OrderHeaderId,
				OrderTotal = orderHeader.OrderTotal,
				Email = orderHeader.Email
			};

			try
			{
				await _messageBus.PublicMessage(paymentRequestMessage, orderPaymentProcessTopic);
				await args.CompleteMessageAsync(args.Message);
			}
			catch
			{
				throw;
			}
		}
	}
}
