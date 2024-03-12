namespace Application.Services;

public interface IOrderService
{
    Task AddOrder(PSV.Domain.Entities.DTOs.OrderPost request);
    Task GetAllOrders();
}