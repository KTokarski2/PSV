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

    public async Task<List<Order>> GetAllOrders()
    {
        return await _context.Orders.ToListAsync();
    }

    public async Task<OrderDetails> GetOrderDetails(int orderId)
    {
        try
        {
            var order = await _context.Orders.FindAsync(orderId);

            if (order == null)
                return null;

            var orderDetails = new OrderDetails
            {
                OrderNumber = order.OrderNumber,
                Cut = order.Cut?.IsPresent ?? false,
                Milling = order.Milling?.IsPresent ?? false,
                Wrapping = order.Wrapping?.IsPresent ?? false,
                FormatCode = order.Format,
                Client = order.Client.Name,
                Comments = order.Comments,
                Photos = await _dataService.GetPhotosFromDirectory(order.Photos)
            };

            return orderDetails;
        }
        catch (ArgumentNullException ex)
        {
            Console.WriteLine($"Argument null exception occurred while retrieving order details: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while retrieving order details: {ex.Message}");
            throw;
        }
    }
}
