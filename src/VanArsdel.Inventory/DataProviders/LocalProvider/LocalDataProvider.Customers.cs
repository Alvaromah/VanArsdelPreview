using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace VanArsdel.Inventory.Data
{
    partial class LocalDataProvider
    {
        public async Task<PageResult<Customer>> GetCustomersAsync(int pageIndex, int pageSize, string query)
        {
            IQueryable<Customer> items = _datasource.Customers;
            if (!String.IsNullOrEmpty(query))
            {
                items = items.Where(r => r.SearchTerms.Contains(query, StringComparison.OrdinalIgnoreCase));
            }
            int count = items.Count();
            int index = Math.Min(Math.Max(0, count - 1) / pageSize, pageIndex);
            var records = await items.Skip(index * pageSize).Take(pageSize).ToListAsync();
            return new PageResult<Customer>(index, pageSize, count, records);
        }

        public async Task DeleteCustomer(long id)
        {
            var item = _datasource.Customers.Where(r => r.CustomerID == id).FirstOrDefault();
            if (item != null)
            {
                _datasource.Customers.Remove(item);
                await _datasource.SaveChangesAsync();
            }
            else
            {
                // TODO: Handle issue "Trying to delete unexisting item"
            }
        }
    }
}
