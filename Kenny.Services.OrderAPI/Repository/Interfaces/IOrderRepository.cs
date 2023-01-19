using Kenny.Services.OrderAPI.Models;

namespace Kenny.Services.OrderAPI.Repository.Interfaces
{
    public interface IOrderRepository
    {
        Task<bool> AddOrderAsync(OrderHeader orderHeader);
        Task UpdateOrderPaymentStatus(int orderHeaderId, bool paid);
    }
}
