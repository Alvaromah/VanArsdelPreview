using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

using VanArsdel.Data;
using VanArsdel.Inventory.Models;

namespace VanArsdel.Inventory.Providers
{
    public interface IDataProvider : IDisposable
    {
        Task<IList<CountryCodeModel>> GetCountryCodesAsync();
        Task<IList<OrderStatusModel>> GetOrderStatusAsync();
        Task<IList<PaymentTypeModel>> GetPaymentTypesAsync();
        Task<IList<ShipperModel>> GetShippersAsync();
        Task<IList<TaxTypeModel>> GetTaxTypesAsync();

        Task<PageResult<CustomerModel>> GetCustomersAsync(int pageIndex, int pageSize, string query = null, Expression<Func<Customer, object>> orderBy = null, bool descending = false);
        Task<CustomerModel> GetCustomerAsync(long id);
        Task<int> UpdateCustomerAsync(CustomerModel model);
        Task<int> DeleteCustomerAsync(CustomerModel model);

        Task<PageResult<OrderModel>> GetOrdersAsync(PageRequest<Order> request);
        Task<OrderModel> GetOrderAsync(long id);
        Task<int> UpdateOrderAsync(OrderModel model);
        Task<int> DeleteOrderAsync(OrderModel model);
    }
}
