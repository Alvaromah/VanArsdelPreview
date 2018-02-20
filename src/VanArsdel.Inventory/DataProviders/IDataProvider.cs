using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VanArsdel.Inventory.Data
{
    public interface IDataProvider : IDisposable
    {
        Task<IList<CountryCode>> GetCountryCodesAsync();

        Task<PageResult<Customer>> GetCustomersAsync(int pageIndex, int pageSize, string query = null);
        Task DeleteCustomer(long id);

        Task<PageResult<Product>> GetProductsAsync(int pageIndex, int pageSize, string query = null);
        Task DeleteProduct(string id);

        Task<PageResult<Order>> GetOrdersAsync(int pageIndex, int pageSize, string query = null, long customerID = -1);
        Task DeleteOrder(long id);
    }
}
