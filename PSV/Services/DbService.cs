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

        /*
        
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
        */
    }
}