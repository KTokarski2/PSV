namespace PSV.Domain.Interfaces;

public interface IDbOrderRepository
{
    public Task AddOrder(Entities.DTOs.OrderPost request);
    public Task<List<Entities.Order>> GetAllOrders();

}