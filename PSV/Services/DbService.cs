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
        var client = await _context.Clients.FirstOrDefaultAsync(c => c.Id == Int32.Parse(request.Client));

        var order = new Order
        {
            OrderNumber = request.OrderNumber,
            CreatedAt = DateTime.Now,
            Client = client,
            Format = request.FormatCode,
            Comments = request.Comments,
            Milling = new Milling { IsPresent = request.Milling },
            Wrapping = new Wrapping { IsPresent = request.Wrapping },
            Cut = new Cut { IsPresent = request.Cut }
        };

        await _context.AddAsync(order);
        await _context.SaveChangesAsync();

        order.Photos = await _dataService.SavePhotos(request, order.Id);
        order.QrCode = await _dataService.GenerateQrCode(order.Id);
        order.BarCode = await _dataService.GenerateBarcode(order.Id, order.OrderNumber);

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

    public async Task<OrderDetails?> GetOrderDetails(int? orderId)
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

        if (order.Cut is { IsPresent: true, From: not null, To: not null })
        {
            orderDetails.CutStart = order.Cut.From.Value.ToString("dd.MM.yyyy HH:mm");
            orderDetails.CutEnd = order.Cut.To.Value.ToString("dd.MM.yyyy HH:mm");
            var cutDuration = order.Cut.To.Value - order.Cut.From.Value;
            var hours = cutDuration.Hours;
            var minutes = cutDuration.Minutes;
            var seconds = cutDuration.Seconds;
            string cutTime = $"{hours:00}:{minutes:00}:{seconds:00}";
            orderDetails.CutTime = cutTime;
        }

        if (order.Milling is { IsPresent: true, From: not null, To: not null })
        {
            orderDetails.MillingStart = order.Milling.From.Value.ToString("dd.MM.yyyy HH:mm");
            orderDetails.MillingEnd = order.Milling.To.Value.ToString("dd.MM.yyyy HH:mm");
            var millingDuration = order.Milling.To.Value - order.Milling.From.Value;
            var hours = millingDuration.Hours;
            var minutes = millingDuration.Minutes;
            var seconds = millingDuration.Seconds;
            string millingTime = $"{hours:00}:{minutes:00}:{seconds:00}";
            orderDetails.MillingTime = millingTime;
        }

        if (order.Wrapping is { IsPresent: true, From: not null, To: not null })
        {
            orderDetails.WrappingStart = order.Wrapping.From.Value.ToString("dd.MM.yyyy HH:mm");
            orderDetails.WrappingEnd = order.Wrapping.To.Value.ToString("dd.MM.yyyy HH:mm");
            var wrappingDuration = order.Wrapping.To.Value - order.Wrapping.From.Value;
            var hours = wrappingDuration.Hours;
            var minutes = wrappingDuration.Minutes;
            var seconds = wrappingDuration.Seconds;
            string wrappingTime = $"{hours:00}:{minutes:00}:{seconds:00}";
            orderDetails.WrappingTime = wrappingTime;
        }

        return orderDetails;
    }

    public async Task EditOrder(OrderDetails dto)
    {

        var order = await _context.Orders
            .Include(o => o.Cut)
            .Include(o => o.Milling)
            .Include(o => o.Wrapping)
            .FirstOrDefaultAsync(o => o.Id == dto.Id);

        if (order != null)
        {
            order.OrderNumber = dto.OrderNumber;
            order.Format = dto.FormatCode;
            order.Comments = dto.Comments;
            order.Cut.IsPresent = dto.Cut;
            order.Milling.IsPresent = dto.Milling;
            order.Wrapping.IsPresent = dto.Wrapping;
        }

        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteOrder(int orderId)
    {
        var order = await _context.Orders.FindAsync(orderId);
        
        if (order == null)
        {
            throw new ArgumentException("Order not found");
        }
        _dataService.DeleteOrderDirectory(order.Id);
        var cut = await _context.Cuts.FirstOrDefaultAsync(c => c.Id == order.IdCut);
        var milling = await _context.Millings.FirstOrDefaultAsync(m => m.Id == order.IdMilling);
        var wrapping = await _context.Wrappings.FirstOrDefaultAsync(w => w.Id == order.IdWrapping);
        
        
        
        _context.Orders.Remove(order);
        if (cut != null) _context.Cuts.Remove(cut);
        if (milling != null) _context.Millings.Remove(milling);
        if (wrapping != null) _context.Wrappings.Remove(wrapping);

        await _context.SaveChangesAsync();
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

    public async Task<OrderControl> GetCutControlData(int? orderId)
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

        if (order.Cut is { IsPresent: true, From: not null, To: not null })
        {
            var timeDuration = order.Cut.To.Value - order.Cut.From.Value;
            var hours = timeDuration.Hours;
            var minutes = timeDuration.Minutes;
            var seconds = timeDuration.Seconds;
            string cutTime = $"{hours:00}:{minutes:00}:{seconds:00}";
            dto.TotalTime = cutTime;
        }
        
        return dto;
    }

    public async Task<OrderControl> GetMillingControlData(int? orderId)
    {
        var order = await _context.Orders
            .Include(o => o.Milling)
            .FirstOrDefaultAsync(o => o.Id == orderId);
        var dto = new OrderControl
        {
            Id = order.Id,
            OrderNumber = order.OrderNumber,
            From = order.Milling.From,
            To = order.Milling.To,
            Comments = order.Comments

        };

        if (order.Milling is { IsPresent: true, From: not null, To: not null })
        {
            var timeDuration = order.Milling.To.Value - order.Milling.From.Value;
            var hours = timeDuration.Hours;
            var minutes = timeDuration.Minutes;
            var seconds = timeDuration.Seconds;
            string cutTime = $"{hours:00}:{minutes:00}:{seconds:00}";
            dto.TotalTime = cutTime;
        }
        
        return dto;
    }

    public async Task<OrderControl> GetWrappingControlData(int? orderId)
    {
        var order = await _context.Orders
            .Include(o => o.Wrapping)
            .FirstOrDefaultAsync(o => o.Id == orderId);
        var dto = new OrderControl
        {
            Id = order.Id,
            OrderNumber = order.OrderNumber,
            From = order.Wrapping.From,
            To = order.Wrapping.To,
            Comments = order.Comments

        };

        if (order.Wrapping is { IsPresent: true, From: not null, To: not null })
        {
            var timeDuration = order.Wrapping.To.Value - order.Wrapping.From.Value;
            var hours = timeDuration.Hours;
            var minutes = timeDuration.Minutes;
            var seconds = timeDuration.Seconds;
            string cutTime = $"{hours:00}:{minutes:00}:{seconds:00}";
            dto.TotalTime = cutTime;
        }
        
        return dto;
    }

    public async Task CommentOrder(OrderControl dto)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == dto.Id);
        order.Comments = dto.Comments;
        await _context.SaveChangesAsync();
    }

    public async Task<string> GetQrCodePath(int id)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);
        return order.QrCode;
    }

    public async Task<string> GetBarcodePath(int id)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);
        return order.BarCode;
    }

    public async Task<int?> GetIdByOrderNumber(string orderNumber)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderNumber == orderNumber);
        return order?.Id;
    }

    public async Task AddClient(ClientPost request)
    {
        var newClient = new Client
        {
            Name = request.Name,
            Address = request.Address,
            PhoneNumber = request.PhoneNumber
        };

        _context.Clients.Add(newClient);
        await _context.SaveChangesAsync();
    }

    public async Task<List<ClientDetails>> GetAllClients()
    {
        var clients = await _context.Clients.Select(c => new ClientDetails
        {
            Id = c.Id,
            Name = c.Name,
            Address = c.Address,
            PhoneNumber = c.PhoneNumber
        }).ToListAsync();

        return clients;
    }
    public async Task EditClient(int clientId, ClientPost client)
    {
        var existingClient = await _context.Clients.FindAsync(clientId);

        if (existingClient == null)
        {
            throw new ArgumentException($"Client with ID {clientId} not found.");
        }

        if (client.Name != "")
        {
            existingClient.Name = client.Name;
        }

        if (client.Address != "")
        {
            existingClient.Address = client.Address;
        }

        if (client.PhoneNumber != "")
        {
            existingClient.PhoneNumber = client.PhoneNumber;
        }

        await _context.SaveChangesAsync();
    }
    public async Task DeleteClient(int clientId)
    {
        var client = await _context.Clients.FindAsync(clientId);

        if (client == null)
        {
            throw new ArgumentException($"Client with ID {clientId} not found.");
        }

        _context.Clients.Remove(client);
        await _context.SaveChangesAsync();
    }

    public async Task<ClientDetails> GetClientDetails(int id)
    {
        var client = await _context.Clients.Where(c => c.Id == id).Select(c => new ClientDetails
        {
            Id = c.Id,
            Name = c.Name,
            Address = c.Address,
            PhoneNumber = c.PhoneNumber
        }).FirstOrDefaultAsync();

        return client;
    }

    public async Task<List<ClientInfo>> GetClientsInfo()
    {
        var clients = await _context.Clients.Select(c => new ClientInfo
        {
            Id = c.Id,
            Name = c.Name
        }).ToListAsync();

        return clients;
    }

    public async Task<bool> IsCutPresent(int? id)
    {
        var order = await _context.Orders.Include(o => o.Cut)
            .FirstOrDefaultAsync(o => o.Id == id);
        return order.Cut.IsPresent;
    }

    public async Task<bool> IsMillingPresent(int? id)
    {
        var order = await _context.Orders.Include(o => o.Milling)
            .FirstOrDefaultAsync(o => o.Id == id);
        return order.Milling.IsPresent;
    }

    public async Task<bool> IsWrappingPresent(int? id)
    {
        var order = await _context.Orders.Include(o => o.Wrapping)
            .FirstOrDefaultAsync(o => o.Id == id);
        return order.Wrapping.IsPresent;
    }
}
