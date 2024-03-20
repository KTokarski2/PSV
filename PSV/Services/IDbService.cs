using PSV.Models.DTOs;
using PSV.Models;

namespace PSV.Services
{
    public interface IDbService
    {
        public Task AddOrder(OrderPost request);
        public Task <List<OrderList>> GetAllOrders();
        public Task<OrderDetails?> GetOrderDetails(int orderId);
        public Task UpdateCutStartTime(int orderId);
        public Task UpdateCutEndTime(int orderId);
        public Task UpdateMillingStartTime(int orderId);
        public Task UpdateMillingEndTime(int orderId);
        public Task UpdateWrappingStartTime(int orderId);
        public Task UpdateWrappingEndTime(int orderId);
        public Task<OrderControl> GetCutControlData(int orderId);
        public Task<OrderControl> GetMillingControlData(int orderId);
        public Task<OrderControl> GetWrappingControlData(int orderId);
    }
}