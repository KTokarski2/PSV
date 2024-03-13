using PSV.Models.DTOs;
using PSV.Models;

namespace PSV.Services
{
    public interface IDbService
    {
        public Task AddOrder(OrderPost request);
    }
}