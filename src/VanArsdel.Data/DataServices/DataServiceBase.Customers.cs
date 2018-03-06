using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace VanArsdel.Data.Services
{
    partial class DataServiceBase
    {
        public async Task<PageResult<Customer>> GetCustomersAsync(int pageIndex, int pageSize, string query = null, Expression<Func<Customer, object>> orderBy = null, bool descending = false)
        {
            // Query
            IQueryable<Customer> items = _dataSource.Customers;
            if (!String.IsNullOrEmpty(query))
            {
                items = items.Where(r => r.SearchTerms.Contains(query));
            }
            int count = items.Count();
            int index = Math.Min(Math.Max(0, count - 1) / pageSize, pageIndex);

            // Order By
            if (orderBy != null)
            {
                items = descending ? items.OrderByDescending(orderBy) : items.OrderBy(orderBy);
            }

            // Execute
            var records = await items.Skip(index * pageSize).Take(pageSize)
                .Select(r => new Customer
                {
                    CustomerID = r.CustomerID,
                    Title = r.Title,
                    FirstName = r.FirstName,
                    MiddleName = r.MiddleName,
                    LastName = r.LastName,
                    Suffix = r.Suffix,
                    Gender = r.Gender,
                    EmailAddress = r.EmailAddress,
                    AddressLine1 = r.AddressLine1,
                    AddressLine2 = r.AddressLine2,
                    City = r.City,
                    Region = r.Region,
                    CountryCode = r.CountryCode,
                    PostalCode = r.PostalCode,
                    Phone = r.Phone,
                    CreatedOn = r.CreatedOn,
                    LastModifiedOn = r.LastModifiedOn,
                    Thumbnail = r.Thumbnail
                })
                .AsNoTracking()
                .ToListAsync();

            return new PageResult<Customer>(index, pageSize, count, records);
        }

        public async Task<Customer> GetCustomerAsync(long id)
        {
            return await _dataSource.Customers.Where(r => r.CustomerID == id).FirstOrDefaultAsync();
        }

        public async Task<int> UpdateCustomerAsync(Customer customer)
        {
            if (customer.CustomerID > 0)
            {
                _dataSource.Entry(customer).State = EntityState.Modified;
            }
            else
            {
                customer.CustomerID = UIDGenerator.Next();
                customer.CreatedOn = DateTime.UtcNow;
                _dataSource.Entry(customer).State = EntityState.Added;
            }
            customer.LastModifiedOn = DateTime.UtcNow;
            customer.SearchTerms = customer.BuildSearchTerms();
            int res = await _dataSource.SaveChangesAsync();
            return res;
        }

        public async Task<int> DeleteCustomerAsync(long id)
        {
            var item = _dataSource.Customers.Where(r => r.CustomerID == id).Select(r => new Customer { CustomerID = r.CustomerID }).FirstOrDefault();
            if (item != null)
            {
                _dataSource.Customers.Remove(item);
                return await _dataSource.SaveChangesAsync();
            }
            return 0;
        }
    }
}
