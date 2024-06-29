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
        var location = await _context.Locations.FirstOrDefaultAsync(l => l.Id == Int32.Parse(request.Location));
    
        var order = new Order
        {
            OrderNumber = request.OrderNumber,
            CreatedAt = DateTime.Now,
            Client = client,
            Location = location,
            Milling = new Milling { IsPresent = request.Milling },
            Wrapping = new Wrapping { IsPresent = request.Wrapping },
            Cut = new Cut { IsPresent = request.Cut },
            EdgeCodeProvided = request.EdgeCodeProvided,
        };

        await _context.AddAsync(order);
        await _context.SaveChangesAsync();
        
        if (!string.IsNullOrEmpty(request.Comments))
        {
            var comments = new Comment
            {
                Content = request.Comments,
                Source = "Wprowadzenie",
                Time = DateTime.Now,
                Order = order
            };

            await _context.AddAsync(comments);
            await _context.SaveChangesAsync();
        }

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
            Location = o.Location.Name,
            Cut = o.Cut.IsPresent,
            Milling = o.Milling.IsPresent,
            Wrapping = o.Wrapping.IsPresent,
            EdgeCodeProvided = o.EdgeCodeProvided
        }).ToListAsync();
    }

    public async Task<OrderDetails?> GetOrderDetails(int? orderId)
    {
        var order = await _context.Orders
            .Include(o => o.Client)
            .Include(o => o.Cut)
                .ThenInclude(c => c.Operator)
            .Include(o => o.Milling)
                .ThenInclude(m => m.Operator)
            .Include(o => o.Wrapping)
                .ThenInclude(w => w.Operator)
            .Include(o => o.Location)
            .FirstOrDefaultAsync(o => o.Id == orderId);
        
        var allClients = await _context.Clients.Select(c => new ClientInfo
        {
            Id = c.Id,
            Name = c.Name
        }).ToListAsync();

        if (order == null)
            return null;

        var cutOperator = order.Cut?.Operator != null ? order.Cut.Operator.FirstName + " " + order.Cut.Operator.LastName : "";
        var millingOperator = order.Milling?.Operator != null ? order.Milling.Operator.FirstName + " " + order.Milling.Operator.LastName : "";
        var wrappingOperator = order.Wrapping?.Operator != null ? order.Wrapping.Operator.FirstName + " " + order.Wrapping.Operator.LastName : "";

        var orderDetails = new OrderDetails
        {
            Id = order.Id,
            OrderNumber = order.OrderNumber,
            Cut = order.Cut?.IsPresent ?? false,
            Milling = order.Milling?.IsPresent ?? false,
            Wrapping = order.Wrapping?.IsPresent ?? false,
            ClientId = order.Client.Id,
            AllClients = allClients,
            //Comments = order.Comments,
            Photos = _dataService.GetPhotosFromDirectory(order.Photos),
            EdgeCodeProvided = order.EdgeCodeProvided,
            EdgeCodeUsed = order.EdgeCodeUsed,
            Location = order.Location.Name,
            CutOperator = cutOperator,
            MillingOperator = millingOperator,
            WrappingOperator = wrappingOperator
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

        var client = await _context.Clients.FirstOrDefaultAsync(c => c.Id == dto.ClientId);
        var location = await _context.Locations.FirstOrDefaultAsync(l => l.Name == dto.Location);

        if (order != null)
        {
            order.OrderNumber = dto.OrderNumber;
            order.Client = client;
            //order.Comments = dto.Comments;
            order.Cut.IsPresent = dto.Cut;
            order.Milling.IsPresent = dto.Milling;
            order.Wrapping.IsPresent = dto.Wrapping;
            order.EdgeCodeProvided = dto.EdgeCodeProvided;
            order.EdgeCodeUsed = dto.EdgeCodeUsed;
            order.Location = location;
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
    
    public async Task UpdateCutStartTime(int orderId, int operatorId)
    {
        var order = await _context.Orders
            .Include(o => o.Cut)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        var opr = await _context.Operators.FirstOrDefaultAsync(o => o.Id == operatorId);

        if (order != null)
        { 
            order.Cut.From = DateTime.Now;
            order.Cut.Operator = opr;
            await _context.SaveChangesAsync();
        }
    }
    
    public async Task UpdateCutEndTime(int orderId, int operatorId)
    {
        var order = await _context.Orders
            .Include(o => o.Cut)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        var opr = await _context.Operators.FirstOrDefaultAsync(o => o.Id == operatorId);
        
        if (order != null)
        { 
            order.Cut.To = DateTime.Now;
            order.Cut.Operator = opr;
            await _context.SaveChangesAsync();
        }
    }
    
    public async Task UpdateMillingStartTime(int orderId, int operatorId)
    {
        var order = await _context.Orders
            .Include(o => o.Milling)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        var opr = await _context.Operators.FirstOrDefaultAsync(o => o.Id == operatorId);

        if (order != null)
        { 
            order.Milling.From = DateTime.Now;
            order.Milling.Operator = opr;
            await _context.SaveChangesAsync();
        }
    }
    
    public async Task UpdateMillingEndTime(int orderId, int operatorId)
    {
        var order = await _context.Orders
            .Include(o => o.Milling)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        var opr = await _context.Operators.FirstOrDefaultAsync(o => o.Id == operatorId);
        
        if (order != null)
        { 
            order.Milling.To = DateTime.Now;
            order.Milling.Operator = opr;
            await _context.SaveChangesAsync();
        }
    }
    
    public async Task UpdateWrappingStartTime(int orderId, int operatorId)
    {
        var order = await _context.Orders
            .Include(o => o.Wrapping)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        var opr = await _context.Operators.FirstOrDefaultAsync(o => o.Id == operatorId);

        if (order != null)
        { 
            order.Wrapping.From = DateTime.Now;
            order.Wrapping.Operator = opr;
            await _context.SaveChangesAsync();
        }
    }
    
    public async Task UpdateWrappingEndTime(int orderId, int operatorId)
    {
        var order = await _context.Orders
            .Include(o => o.Wrapping)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        var opr = await _context.Operators.FirstOrDefaultAsync(o => o.Id == operatorId);

        if (order != null)
        { 
            order.Wrapping.To = DateTime.Now;
            order.Wrapping.Operator = opr;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<OrderControl> GetCutControlData(int? orderId)
    {
        var order = await _context.Orders
            .Include(o => o.Cut)
            .ThenInclude(c => c.Operator)
            .FirstOrDefaultAsync(o => o.Id == orderId);
        
        var dto = new OrderControl
        {
            Id = order.Id,
            OrderNumber = order.OrderNumber,
            From = order.Cut.From,
            To = order.Cut.To
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
            //Comments = order.Comments

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
            EdgeCode = order.EdgeCodeProvided
            //Comments = order.Comments

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

    public async Task CommentOrder(OrderControl dto, string source)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == dto.Id);
        var opr = await _context.Operators.FirstOrDefaultAsync(o => o.Id == dto.OperatorId);
        var comment = new Comment
        {
            Content = dto.Comments,
            Operator = opr,
            Order = order,
            Source = source,
            Time = DateTime.Now
        };
        await _context.AddAsync(comment);
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
            PhoneNumber = request.PhoneNumber,
            NIP = request.NIP,
            Email = request.Email
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
            PhoneNumber = c.PhoneNumber,
            Email = c.Email,
            NIP = c.NIP
        }).ToListAsync();

        return clients;
    }

    public async Task<List<LocationInfo>> GetAllLocations()
    {
        var locations = await _context.Locations.Select(l => new LocationInfo
        {
            Id = l.Id,
            Name = l.Name
        }).ToListAsync();

        return locations;
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

        if (client.NIP != "")
        {
            existingClient.NIP = client.NIP;
        }

        if (client.Email != "")
        {
            existingClient.Email = client.Email;
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
            PhoneNumber = c.PhoneNumber,
            Email = c.Email,
            NIP = c.NIP
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
    public async Task AddOperator(OperatorPost newOperator)
    {
        var location = await _context.Locations.FirstOrDefaultAsync(l => l.Id == Int32.Parse(newOperator.Location));
        var op = new Operator
        {
            FirstName = newOperator.FirstName,
            LastName = newOperator.LastName,
            PhoneNumber = newOperator.PhoneNumber,
            Location = location
        };
        await _context.AddAsync(op);
        await _context.SaveChangesAsync();
    }

    public async Task EditOperator(int id, OperatorDetails opr)
    {
        var oprDb = await _context.Operators.FirstOrDefaultAsync(o => o.Id == id);
        var loc = await _context.Locations.FirstOrDefaultAsync(l => l.Name == opr.Location);
        oprDb.FirstName = opr.FristName;
        oprDb.LastName = opr.LastName;
        oprDb.PhoneNumber = opr.PhoneNumber;
        oprDb.Location = loc;
        await _context.SaveChangesAsync();
    }

    public async Task<List<OperatorDetails>> GetOperators()
    {
        var dto = await _context.Operators.Select(op => new OperatorDetails
        {
            Id = op.Id,
            FristName = op.FirstName,
            LastName = op.LastName,
            PhoneNumber = op.PhoneNumber,
            Location = op.Location.Name
        }).ToListAsync();
        return dto;
    }

    public async Task<OperatorDetails> GetOperatorDetails(int id)
    {
        var dto = await _context.Operators.Where(o => o.Id == id).Select(o => new OperatorDetails
        {
            Id = o.Id,
            FristName = o.FirstName,
            LastName = o.LastName,
            PhoneNumber = o.PhoneNumber,
            Location = o.Location.Name
        }).FirstOrDefaultAsync();
        return dto;
    }

    public async Task DeleteOperator(int id)
    {
        var opr = await _context.Operators.FindAsync(id);

        if (opr == null)
        {
            throw new ArgumentException($"Client with ID {opr} not found.");
        }

        _context.Operators.Remove(opr);
        await _context.SaveChangesAsync();
    }

    public async Task<List<OperatorInfo>> GetAllOperators()
    {
        var operators = await _context.Operators.Select(o => new OperatorInfo
        {
            Id = o.Id,
            FirstName = o.FirstName,
            LastName = o.LastName,
            Location = o.Location.Name
        }).ToListAsync();
        return operators;
    }

    public async Task<bool> CheckIfUsedDifferentProvided(int orderId, string edgeCode)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
        if (order.EdgeCodeProvided == edgeCode)
            return false;
        return true;
    }

    public async Task SetUsedEdgeCode(int orderId, string edgeCode)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
        order.EdgeCodeUsed = edgeCode;
        await _context.SaveChangesAsync();
    }

    public async Task<int> GetCutOperatorId(int? orderId)
    {
        var order = await _context.Orders
            .Include(o => o.Cut)
            .ThenInclude(c => c.Operator)
            .FirstOrDefaultAsync(o => o.Id == orderId);
        return order.Cut.Operator.Id;
    }

    public async Task<int> GetMillingOperatorId(int? orderId)
    {
        var order = await _context.Orders
            .Include(o => o.Milling)
            .ThenInclude(m => m.Operator)
            .FirstOrDefaultAsync(o => o.Id == orderId);
        return order.Milling.Operator.Id;
    }

    public async Task<int> GetWrappingOperatorId(int? orderId)
    {
        var order = await _context.Orders
            .Include(o => o.Wrapping)
            .ThenInclude(w => w.Operator)
            .FirstOrDefaultAsync(o => o.Id == orderId);
        return order.Wrapping.Operator.Id;
    }

    public async Task<List<CommentDto>> GetOrderComments(int orderId)
    {
        var comments = await _context.Orders
            .Include(o => o.Comments)
            .ThenInclude(c => c.Operator)
            .Where(o => o.Id == orderId)
            .SelectMany(o => o.Comments)
            .Select(c => new CommentDto
            {
                Content = c.Content,
                Source = c.Source,
                Time = c.Time.ToString("dd.MM.yyyy HH:mm"),
                Operator = c.Operator != null
                    ? c.Operator.FirstName + " " + c.Operator.LastName
                    : "-"
            }).ToListAsync();

        return comments;
    }
}
