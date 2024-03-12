using Microsoft.EntityFrameworkCore;
using PSV.Domain.Entities;
using PSV.Domain.Interfaces;

namespace PSV.Infrastructure.Repositories;

internal class DbOrderRepository : IDbOrderRepository
{
    private readonly Repository _context;

    public DbOrderRepository(Repository context)
    {
        _context = context;
    }
    
    public async Task AddOrder(Domain.Entities.DTOs.OrderPost request)
    {

        var client = await _context.Clients.FirstOrDefaultAsync(c => c.Name == request.Client);
        
        var order = new Order
        {
            
            QrCode = request.QrCode,
            Client = client,
            Format = request.Format,
            Comments = request.Comments,
            Photos = request.Photos,
            Cut = new Cut {IsPresent = request.IsCutPresent},  
            Milling = new Milling {IsPresent = request.IsMillingPresent},
            Wrapping = new Wrapping {IsPresent = request.IsWrappingPresent}, 

        };
        await _context.AddAsync(order);
        await _context.SaveChangesAsync();
    }
    
    public async Task<List<Order>> GetAllOrders()
    {
        return await _context.Orders.ToListAsync();
    }
}