using Microsoft.EntityFrameworkCore;
using PSV.Models;
using PSV.Models.DTOs;

namespace PSV.Services;

public class DbService : IDbService
{
    private readonly Repository _context;

    public DbService(Repository context)
    {
        _context = context;
    }
    
    public async Task AddOrder(OrderPost request)
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
}