using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace VanArsdel.Inventory.Data
{
    partial class LocalDataProvider
    {
        public async Task<PageResult<Order>> GetOrdersAsync(int pageIndex, int pageSize, string query = null, long customerID = -1)
        {
            IQueryable<Order> items = _datasource.Orders;
            if (customerID > 0)
            {
                items = items.Where(r => r.CustomerID == customerID);
            }
            if (!String.IsNullOrEmpty(query))
            {
                items = items.Where(r => r.SearchTerms.Contains(query, StringComparison.OrdinalIgnoreCase));
            }
            int count = items.Count();
            int index = Math.Min(Math.Max(0, count - 1) / pageSize, pageIndex);
            var records = await items.Skip(index * pageSize).Take(pageSize).Include(r => r.Customer).ToListAsync();
            return new PageResult<Order>(index, pageSize, count, records);
        }

        public async Task DeleteOrder(long id)
        {
            var item = _datasource.Orders.Where(r => r.OrderID == id).FirstOrDefault();
            if (item != null)
            {
                _datasource.Orders.Remove(item);
                await _datasource.SaveChangesAsync();
            }
            else
            {
                // TODO: Handle issue "Trying to delete unexisting item"
            }
        }
    }
}
