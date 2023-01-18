using Azure.Messaging.ServiceBus;
using Kenny.Services.OrderAPI.Messages.Dto;
using Kenny.Services.OrderAPI.Messaging.Interfaces;
using Kenny.Services.OrderAPI.Models;
using Kenny.Services.OrderAPI.Repository;
using Newtonsoft.Json;
using System.Text;

namespace Kenny.Services.OrderAPI.Messaging
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
	{
		private readonly OrderRepository _orderRepository;
		private readonly string serviceBusConnectionString;
		private readonly string checkoutMessageTopic;
		private readonly string subscriptionCheckOut;

		private ServiceBusProcessor checkOutProcessor;

		private readonly IConfiguration _configuration;

		public AzureServiceBusConsumer(OrderRepository orderRepository, IConfiguration configuration)
		{
			_orderRepository = orderRepository;
			_configuration = configuration;

			serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");
			checkoutMessageTopic = _configuration.GetValue<string>("CheckoutMessageTopic");
			subscriptionCheckOut = _configuration.GetValue<string>("SubscriptionCheckOut");

			var client = new ServiceBusClient(serviceBusConnectionString);
			checkOutProcessor = client.CreateProcessor(checkoutMessageTopic, subscriptionCheckOut);
		}

		public async Task Start()
		{
			checkOutProcessor.ProcessMessageAsync += OnCheckoutMessageReceived;
			checkOutProcessor.ProcessErrorAsync += ErrorHandler;
			await checkOutProcessor.StartProcessingAsync();
		}

		public async Task Stop()
		{
			await checkOutProcessor.StopProcessingAsync();
			await checkOutProcessor.DisposeAsync();
		}

		Task ErrorHandler(ProcessErrorEventArgs args)
		{
			Console.WriteLine(args.Exception.ToString());
			return Task.CompletedTask;
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
		}
	}
}
