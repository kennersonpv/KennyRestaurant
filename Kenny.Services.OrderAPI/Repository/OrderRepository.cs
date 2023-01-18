using Kenny.Services.OrderAPI.Models;

namespace Kenny.Services.OrderAPI.Repository
{
	public class OrderRepository : IOrderRepository
	{
		public Task<bool> AddOrderAsync(OrderHeader orderHeader)
		{
			throw new NotImplementedException();
		}

		public Task UpdateOrderPaymentStatus(int orderHeaderId, bool paid)
		{
			throw new NotImplementedException();
		}
	}
}
