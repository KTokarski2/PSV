using PSV.Domain.Interfaces;

namespace Application.Services;

public class OrderService : IOrderService
{ 
    private readonly IDbOrderRepository _dbOrderRepository;

    public OrderService(IDbOrderRepository repository) 
    { 
        _dbOrderRepository = repository;
    }
    
    public async Task AddOrder(PSV.Domain.Entities.DTOs.OrderPost request)
    {
        await _dbOrderRepository.AddOrder(request);
    }

    public async Task GetAllOrders()
    {
        await _dbOrderRepository.GetAllOrders();
    }
    
}