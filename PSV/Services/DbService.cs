using Microsoft.EntityFrameworkCore;
using PSV.Models;
using PSV.Models.DTOs;
using PSV.Utils;

namespace PSV.Services;

public class DbService : IDbService
{
    private readonly Repository _context;
    private readonly OrderDataService _dataService;

    public DbService(Repository context)
    {
        _context = context;
        _dataService = new OrderDataService();
    }
    
    public async Task AddOrder(OrderPost request)
    {
        var client = new Client
        {
            Name = request.Client
        };

        var order = new Order
        {
            OrderNumber = request.OrderNumber,
            CreatedAt = DateTime.Now,
            Client = client,
            Format = request.FormatCode,
            Comments = request.Comments,
            Photos = await _dataService.SavePhotos(request),
            Milling = new Milling { IsPresent = request.Milling },
            Wrapping = new Wrapping { IsPresent = request.Wrapping },
            Cut = new Cut { IsPresent = request.Cut },
            QrCode = await _dataService.GenerateQrCode(request.OrderNumber)
        };

        await _context.AddAsync(order);
        await _context.SaveChangesAsync();
        
    }

    public async Task<List<OrderList>> GetAllOrders()
    {
        return await _context.Orders.Select(o => new OrderList
        {
            Id = o.Id,
            OrderNumber = o.OrderNumber,
            CreatedAt = o.CreatedAt,
            Client = o.Client.Name,
            Cut = o.Cut.IsPresent,
            Milling = o.Milling.IsPresent,
            Wrapping = o.Wrapping.IsPresent
        }).ToListAsync();
    }

    public async Task<OrderDetails?> GetOrderDetails(int orderId)
    {
        return await _context.Orders.Where(o => o.Id == orderId)
            .Select(o => new OrderDetails
            {
                Id = o.Id,
                OrderNumber = o.OrderNumber,
                Cut = o.Cut.IsPresent,
                Milling = o.Milling.IsPresent,
                Wrapping = o.Wrapping.IsPresent,
                FormatCode = o.Format,
                Client = o.Client.Name,
                Comments = o.Comments,
                Photos = _dataService.GetPhotosFromDirectory(o.Photos)
            }).FirstOrDefaultAsync();
    }
}
