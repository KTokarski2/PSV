using Microsoft.EntityFrameworkCore;
using PSV.Models;
using PSV.Models.DTOs;
using PSV.Utils;

namespace PSV.Services;

public class DbService : IDbService
{
    private readonly Repository _context;
    private readonly OrderFileService _fileService;
    private readonly ISmsService _smsservice;

    public DbService(Repository context, ISmsService smsservice)
    {
        _context = context;
        _fileService = new OrderFileService();
        _smsservice = smsservice;
    }
    
    public async Task AddOrder(OrderPost request)
    {
        var client = await _context.Clients.FirstOrDefaultAsync(c => c.Id == Int32.Parse(request.Client));
        var location = await _context.Locations.FirstOrDefaultAsync(l => l.Id == Int32.Parse(request.Location));
    
        var order = new Order
        {
            OrderNumber = request.OrderNumber,
            OrderName = request.OrderName,
            CreatedAt = DateTime.Now,
            Client = client,
            Location = location,
            StagesTotal = CountTotalStages(request),
            Milling = new Milling { IsPresent = request.Milling },
            Wrapping = new Wrapping { IsPresent = request.Wrapping },
            Cut = new Cut { IsPresent = request.Cut },
            EdgeCodeProvided = request.EdgeCodeProvided,
            IdOrderStatus = 1
        };

        await _context.AddAsync(order);
        await _context.SaveChangesAsync();
        
        if (!string.IsNullOrEmpty(request.Comments))
        {
            var comments = new Comment
            {
                Content = request.Comments,
                Source = "Biuro",
                Time = DateTime.Now,
                Order = order
            };

            await _context.AddAsync(comments);
            await _context.SaveChangesAsync();
        }

        order.Photos = await _fileService.SavePhotos(request, order.Id);
        order.QrCode = await _fileService.GenerateQrCode(order.Id);
        order.BarCode = await _fileService.GenerateBarcode(order.Id, order.OrderNumber);
        order.OrderFile = await _fileService.GetTemporaryFile(order.OrderName);

        await _context.SaveChangesAsync();
        //await _smsservice.SendMessage(client.PhoneNumber, "OrderCreated", order.OrderNumber);
    }
            
    public async Task<List<OrderList>> GetAllOrders()
    {
        return await _context.Orders
            .OrderByDescending(o => o.CreatedAt)
            .Select(o => new OrderList
        {
            Id = o.Id,
            OrderNumber = o.OrderNumber,
            OrderName = o.OrderName,
            CreatedAt = o.CreatedAt.ToString("dd.MM.yyyy HH:mm"),
            Client = o.Client.Name,
            Location = o.Location.Name,
            Cut = o.Cut.IsPresent,
            Milling = o.Milling.IsPresent,
            Wrapping = o.Wrapping.IsPresent,
            EdgeCodeProvided = o.EdgeCodeProvided,
            Status = o.OrderStatus.Name,
            CutDone = o.Cut.To != null,
            MillingDone = o.Milling.To != null,
            WrappingDone = o.Wrapping.To != null
            
        }).ToListAsync();
    }

    public async Task<OrderDetails?> GetOrderDetails(int? orderId)
    {
        var order = await _context.Orders
            .Include(o => o.Client)
            .Include(o => o.OrderStatus)
            .Include(o => o.Releaser)
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
            OrderName = order.OrderName,
            Cut = order.Cut?.IsPresent ?? false,
            Milling = order.Milling?.IsPresent ?? false,
            Wrapping = order.Wrapping?.IsPresent ?? false,
            ClientId = order.Client.Id,
            AllClients = allClients,
            Photos = _fileService.GetPhotosFromDirectory(order.Photos),
            EdgeCodeProvided = order.EdgeCodeProvided,
            EdgeCodeUsed = order.EdgeCodeUsed,
            Location = order.Location.Name,
            CutOperator = cutOperator,
            MillingOperator = millingOperator,
            WrappingOperator = wrappingOperator,
            CreatedAt = order.CreatedAt.ToString("dd.MM.yyyy HH:mm"),
            Status = order.OrderStatus.Name,
            ReleasedBy = order.Releaser != null ? $"{order.Releaser.FirstName} {order.Releaser.LastName}" : "",
            ReleasedAt = order.ReleaseDate != null && order.ReleaseDate.Value != DateTime.MinValue 
                ? order.ReleaseDate.Value.ToString("dd.MM.yyyy HH:mm") 
                : ""

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
            order.OrderName = dto.OrderName;
            order.Client = client;
            order.Cut.IsPresent = dto.Cut;
            order.Milling.IsPresent = dto.Milling;
            order.Wrapping.IsPresent = dto.Wrapping;
            order.EdgeCodeProvided = dto.EdgeCodeProvided;
            order.EdgeCodeUsed = dto.EdgeCodeUsed;
            order.Location = location;
        }

        if (!string.IsNullOrEmpty(dto.Comments))
        {
            var comments = new Comment
            {
                Content = dto.Comments,
                Source = "Biuro",
                Time = DateTime.Now,
                Order = order
            };
            await _context.AddAsync(comments);
            await _context.SaveChangesAsync();
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
        _fileService.DeleteOrderDirectory(order.Id);
        _fileService.DeleteOrderFile(order.OrderFile);
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
            .Include(o => o.Client)
            .Include(o => o.OrderStatus)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        var opr = await _context.Operators.FirstOrDefaultAsync(o => o.Id == operatorId);
        
        if (order != null)
        { 
            order.Cut.To = DateTime.Now;
            order.Cut.Operator = opr;
            await _context.SaveChangesAsync();
        }

        if (!order.Cut.ClientNotified && order.Client.PhoneNumber != null)
        {
            if (order.StagesCompleted != order.StagesTotal)
            {
                order.StagesCompleted += 1;
                order.Cut.ClientNotified = true;
                await _context.SaveChangesAsync();
            }

            /*

            if (order.StagesCompleted < order.StagesTotal)
            {
                await _smsservice.SendMessage(order.Client.PhoneNumber, "StageFinished", order.OrderNumber, order.StagesCompleted.ToString(),order.StagesTotal.ToString());
                order.Cut.ClientNotified = true;
                await _context.SaveChangesAsync();
            }

            */

            if (order.StagesCompleted == order.StagesTotal)
            {
                await _smsservice.SendMessage(order.Client.PhoneNumber, "OrderFinished", order.OrderNumber);
                order.Cut.ClientNotified = true;

                if (order.OrderStatus.Id != 2)
                {
                    order.OrderStatus = await _context.OrderStatuses.FirstOrDefaultAsync(s => s.Id == 2);
                }
                
                await _context.SaveChangesAsync();
            }
            
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
            .Include(o => o.Client)
            .Include(o => o.OrderStatus)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        var opr = await _context.Operators.FirstOrDefaultAsync(o => o.Id == operatorId);
        
        if (order != null)
        { 
            order.Milling.To = DateTime.Now;
            order.Milling.Operator = opr;
            await _context.SaveChangesAsync();
        }

        if (!order.Milling.ClientNotified && order.Client.PhoneNumber != null)
        {
            if (order.StagesTotal != order.StagesCompleted)
            {
                order.StagesCompleted += 1;
                order.Milling.ClientNotified = true;
                await _context.SaveChangesAsync();
            }

            /*

            if (order.StagesCompleted < order.StagesTotal)
            {
                await _smsservice.SendMessage(order.Client.PhoneNumber, "StageFinished",order.OrderNumber, order.StagesCompleted.ToString(), order.StagesTotal.ToString());
                order.Milling.ClientNotified = true;
                await _context.SaveChangesAsync();
            }

            */

            if (order.StagesCompleted == order.StagesTotal)
            {
                await _smsservice.SendMessage(order.Client.PhoneNumber, "OrderFinished", order.OrderNumber);
                order.Milling.ClientNotified = true;
                
                if (order.OrderStatus.Id != 2)
                {
                    order.OrderStatus = await _context.OrderStatuses.FirstOrDefaultAsync(s => s.Id == 2);
                }
                
                await _context.SaveChangesAsync();
            }
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
            .Include(o => o.Client)
            .Include(o => o.OrderStatus)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        var opr = await _context.Operators.FirstOrDefaultAsync(o => o.Id == operatorId);

        if (order != null)
        { 
            order.Wrapping.To = DateTime.Now;
            order.Wrapping.Operator = opr;
            await _context.SaveChangesAsync();
        }

        if (!order.Wrapping.ClientNotified && order.Client.PhoneNumber != null)
        {
            if (order.StagesCompleted != order.StagesTotal)
            {
                order.StagesCompleted += 1;
                order.Wrapping.ClientNotified = true;
                await _context.SaveChangesAsync();
            }

            /*

            if (order.StagesCompleted < order.StagesTotal)
            {
                await _smsservice.SendMessage(order.Client.PhoneNumber, "StageFinished",order.OrderNumber, order.StagesCompleted.ToString(), order.StagesTotal.ToString());
                order.Wrapping.ClientNotified = true;
                await _context.SaveChangesAsync();
            }

            */

            if (order.StagesCompleted == order.StagesTotal)
            {
                await _smsservice.SendMessage(order.Client.PhoneNumber, "OrderFinished", order.OrderNumber);
                order.Wrapping.ClientNotified = true;
                
                if (order.OrderStatus.Id != 2)
                {
                    order.OrderStatus = await _context.OrderStatuses.FirstOrDefaultAsync(s => s.Id == 2);
                }
                
                await _context.SaveChangesAsync();
            }
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
    
    public async Task EditClient(int clientId, ClientDetails client)
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
        oprDb.FirstName = opr.FirstName;
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
            FirstName = op.FirstName,
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
            FirstName = o.FirstName,
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
                Id = c.Id,
                Content = c.Content,
                Source = c.Source,
                Time = c.Time.ToString("dd.MM.yyyy HH:mm"),
                Operator = c.Operator != null
                    ? c.Operator.FirstName + " " + c.Operator.LastName
                    : "-"
            }).ToListAsync();

        return comments;
    }

    public async Task<string> GetOrderFilePath(int id)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);
        return order.OrderFile;
    }

    public async Task<string> GetOrderPhotosPath(int id)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);
        return order.Photos;
    }

    public async Task<string> GenerateOrderNumber()
    {
        var datePrefix = DateTime.Now.ToString("yyyyMMdd");
        var todayOrdersCount = await _context.Orders
            .Where(o => o.CreatedAt.Date == DateTime.Now.Date)
            .CountAsync();
        var orderNumber = $"{datePrefix}{todayOrdersCount + 1:D4}";
        return orderNumber;
    }
    private int CountTotalStages(OrderPost request)
    {
        int stages = 0;

        if (request.Cut == true)
            stages += 1;
        if (request.Milling == true)
            stages += 1;
        if (request.Wrapping == true)
            stages += 1;
        
        return stages;
    }

    public async Task<bool> CheckIfEdgeCodeWasProvided(int orderId)
    {
        var order = await _context.Orders.FindAsync(orderId);
        if (order.EdgeCodeProvided == null) return false;
        return true;
    }

    public async Task AddReleaser(ReleaserPost newReleaser)
    {
        var location = await _context.Locations.FirstOrDefaultAsync(l => l.Id == Int32.Parse(newReleaser.Location));
        var releaser = new Releaser
        {
            FirstName = newReleaser.FirstName,
            LastName = newReleaser.LastName,
            Location = location
        };
        await _context.AddAsync(releaser);
        await _context.SaveChangesAsync();
    }

    public async Task<List<ReleaserDetails>> GetReleasers()
    {
        var dto = await _context.Releasers.Select(re => new ReleaserDetails 
        {
            Id = re.Id,
            FirstName = re.FirstName,
            LastName = re.LastName,
            Location = re.Location.Name
        }).ToListAsync();
        return dto;
    }

    public async Task<ReleaserDetails> GetReleaserDetails(int id)
    {
        var dto = await _context.Releasers.Where(r => r.Id == id).Select(r => new ReleaserDetails 
        {
            Id = r.Id,
            FirstName = r.FirstName,
            LastName = r.LastName,
            Location = r.Location.Name
        }).FirstOrDefaultAsync();
        return dto;
    }

    public async Task EditReleaser(int id, ReleaserDetails rel)
    {
        var relDb = await _context.Releasers.FirstOrDefaultAsync(r => r.Id == id);
        var loc = await _context.Locations.FirstOrDefaultAsync(l => l.Name == rel.Location);
        relDb.FirstName = rel.FirstName;
        relDb.LastName = rel.LastName;
        relDb.Location = loc;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteReleaser(int id)
    {
        var rel = await _context.Releasers.FindAsync(id);
        _context.Releasers.Remove(rel);
        await _context.SaveChangesAsync();
    }

    public async Task<List<OrderList>> GetFinishedOrders()
    {
        return await _context.Orders
            .OrderByDescending(o => o.CreatedAt)
            .Where(o => o.OrderStatus.Id == 2)
            .Select(o => new OrderList
            {
                Id = o.Id,
                OrderNumber = o.OrderNumber,
                OrderName = o.OrderName,
                CreatedAt = o.CreatedAt.ToString("dd.MM.yyyy HH:mm"),
                Client = o.Client.Name,
                Location = o.Location.Name,
                Cut = o.Cut.IsPresent,
                Milling = o.Milling.IsPresent,
                Wrapping = o.Wrapping.IsPresent,
                EdgeCodeProvided = o.EdgeCodeProvided,
                Status = o.OrderStatus.Name
            }).ToListAsync();
    }

    public async Task<OrderRelease?> GetReleaseOrderData(int? orderId)
    {
        var allReleasers = await GetReleasers();
        return await _context.Orders
            .Where(o => o.Id == orderId)
            .Select(o => new OrderRelease
            {
                OrderId = o.Id,
                OrderNumber = o.OrderNumber,
                Releasers = allReleasers
            }).FirstOrDefaultAsync();
    }

    public async Task ReleaseOrder(int orderId, int releaserId)
    {
        var order = await _context.Orders
            .Include(o => o.OrderStatus)
            .FirstOrDefaultAsync(o => o.Id == orderId);
        var releaser = await _context.Releasers.FindAsync(releaserId);
        order.Releaser = releaser;
        order.OrderStatus = await _context.OrderStatuses.FindAsync(3);;
        order.ReleaseDate = DateTime.Now;
        await _context.SaveChangesAsync();
    }

    public async Task<bool> CheckIfReadyForRelease(int? id)
    {
        var order = await _context.Orders.Include(o => o.OrderStatus).FirstOrDefaultAsync(o => o.Id == id);
        if (order.OrderStatus.Id == 2)
        {
            return true;
        }

        return false;
    }

    public async Task<bool> CheckIfAlreadyReleased(int? id)
    {
        var order = await _context.Orders.Include(o => o.OrderStatus).FirstOrDefaultAsync(o => o.Id == id);
        if (order.OrderStatus.Id == 3)
        {
            return true;
        }

        return false;
    }
}