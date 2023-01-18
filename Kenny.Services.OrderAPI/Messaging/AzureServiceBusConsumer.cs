using Azure.Messaging.ServiceBus;
using Kenny.Services.OrderAPI.Messages;
using Kenny.Services.OrderAPI.Models;
using Kenny.Services.OrderAPI.Repository;
using Newtonsoft.Json;
using System.Text;

namespace Kenny.Services.OrderAPI.Messaging
{
	public class AzureServiceBusConsumer
	{
		private readonly OrderRepository _orderRepository;
		public AzureServiceBusConsumer(OrderRepository orderRepository)
		{
			_orderRepository = orderRepository;
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
