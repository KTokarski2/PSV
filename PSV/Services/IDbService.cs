using PSV.Models.DTOs;
using PSV.Models;

namespace PSV.Services
{
    public interface IDbService
    {
        public Task AddOrder(OrderPost request);
        public Task <List<OrderList>> GetAllOrders();
        public Task<OrderDetails?> GetOrderDetails(int? orderId);
        public Task EditOrder(OrderDetails dto);
        public Task DeleteOrder(int orderId);
        public Task UpdateCutStartTime(int orderId);
        public Task UpdateCutEndTime(int orderId);
        public Task UpdateMillingStartTime(int orderId);
        public Task UpdateMillingEndTime(int orderId);
        public Task UpdateWrappingStartTime(int orderId);
        public Task UpdateWrappingEndTime(int orderId);
        public Task<OrderControl> GetCutControlData(int? orderId);
        public Task<OrderControl> GetMillingControlData(int? orderId);
        public Task<OrderControl> GetWrappingControlData(int? orderId);
        public Task CommentOrder(OrderControl dto);
        public Task<string> GetQrCodePath(int id);
        public Task<string> GetBarcodePath(int id);
        public Task<int?> GetIdByOrderNumber(string orderNumber);
        public Task AddClient(ClientPost request);
        public Task<List<ClientDetails>> GetAllClients();
        public Task<List<LocationInfo>> GetAllLocations();
        public Task<List<ClientInfo>> GetClientsInfo();
        public Task<ClientDetails> GetClientDetails(int id);
        public Task EditClient(int clientId, ClientPost client);
        public Task DeleteClient(int clientId);
        public Task<bool> IsCutPresent(int? id);
        public Task<bool> IsMillingPresent(int? id);
        public Task<bool> IsWrappingPresent(int? id);
        public Task<CodeRequest> GetCodeByID(int id);
        public Task AddOperator(OperatorPost newOperator);
        public Task EditOperator(Operator updatedOperator);
        public Task<List<OperatorDetails>> GetOperators();


    }
}