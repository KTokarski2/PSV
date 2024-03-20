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
        var order = await _context.Orders
            .Include(o => o.Client)
            .Include(o => o.Cut)
            .Include(o => o.Milling)
            .Include(o => o.Wrapping)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if (order == null)
            return null;

        var orderDetails = new OrderDetails
        {
            Id = order.Id,
            OrderNumber = order.OrderNumber,
            Cut = order.Cut?.IsPresent ?? false,
            Milling = order.Milling?.IsPresent ?? false,
            Wrapping = order.Wrapping?.IsPresent ?? false,
            FormatCode = order.Format,
            Client = order.Client.Name,
            Comments = order.Comments,
            Photos = _dataService.GetPhotosFromDirectory(order.Photos)
        };

        if (order.Cut != null && order.Cut.IsPresent && order.Cut.From.HasValue && order.Cut.To.HasValue)
        {
            orderDetails.CutStart = order.Cut.From.Value;
            orderDetails.CutEnd = order.Cut.To.Value;
            orderDetails.CutTime = (orderDetails.CutEnd - orderDetails.CutStart).ToString("hh\\:mm\\:ss");
        }

        if (order.Milling != null && order.Milling.IsPresent && order.Milling.From.HasValue && order.Milling.To.HasValue)
        {
            orderDetails.MillingStart = order.Milling.From.Value;
            orderDetails.MillingEnd = order.Milling.To.Value;
            orderDetails.MillingTime = (orderDetails.MillingEnd - orderDetails.MillingStart).ToString("hh\\:mm\\:ss");
        }

        if (order.Wrapping != null && order.Wrapping.IsPresent && order.Wrapping.From.HasValue && order.Wrapping.To.HasValue)
        {
            orderDetails.WrappingStart = order.Wrapping.From.Value;
            orderDetails.WrappingEnd = order.Wrapping.To.Value;
            orderDetails.WrappingTime = (orderDetails.WrappingEnd - orderDetails.WrappingStart).ToString("hh\\:mm\\:ss");
        }

        return orderDetails;
    }
    
    public async Task UpdateCutStartTime(int orderId)
    {
        var order = await _context.Orders
            .Include(o => o.Cut)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if (order != null)
        { 
            order.Cut.From = DateTime.Now;
            await _context.SaveChangesAsync();
        }
    }
    
    public async Task UpdateCutEndTime(int orderId)
    {
        var order = await _context.Orders
            .Include(o => o.Cut)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if (order != null)
        { 
            order.Cut.To = DateTime.Now;
            await _context.SaveChangesAsync();
        }
    }
    
    public async Task UpdateMillingStartTime(int orderId)
    {
        var order = await _context.Orders
            .Include(o => o.Milling)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if (order != null)
        { 
            order.Milling.From = DateTime.Now;
            await _context.SaveChangesAsync();
        }
    }
    
    public async Task UpdateMillingEndTime(int orderId)
    {
        var order = await _context.Orders
            .Include(o => o.Milling)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if (order != null)
        { 
            order.Milling.To = DateTime.Now;
            await _context.SaveChangesAsync();
        }
    }
    
    public async Task UpdateWrappingStartTime(int orderId)
    {
        var order = await _context.Orders
            .Include(o => o.Wrapping)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if (order != null)
        { 
            order.Wrapping.From = DateTime.Now;
            await _context.SaveChangesAsync();
        }
    }
    
    public async Task UpdateWrappingEndTime(int orderId)
    {
        var order = await _context.Orders
            .Include(o => o.Wrapping)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if (order != null)
        { 
            order.Wrapping.To = DateTime.Now;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<OrderControl> GetCutControlData(int orderId)
    {
        var order = await _context.Orders
            .Include(o => o.Cut)
            .FirstOrDefaultAsync(o => o.Id == orderId);
        
        var dto = new OrderControl
        {
            Id = order.Id,
            OrderNumber = order.OrderNumber,
            From = order.Cut.From,
            To = order.Cut.To,
            Comments = order.Comments

        };
        return dto;
    }

    public async Task<OrderControl> GetMillingControlData(int orderId)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
        var dto = new OrderControl
        {
            Id = order.Id,
            OrderNumber = order.OrderNumber,
            From = order.Milling.From,
            To = order.Milling.To,
            Comments = order.Comments

        };
        return dto;
    }

    public async Task<OrderControl> GetWrappingControlData(int orderId)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
        var dto = new OrderControl
        {
            Id = order.Id,
            OrderNumber = order.OrderNumber,
            From = order.Wrapping.From,
            To = order.Wrapping.To,
            Comments = order.Comments

        };
        return dto;
    }
}
