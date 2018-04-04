using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace VanArsdel.Data.Services
{
    partial class DataServiceBase
    {
        public async Task<int> GetCustomersCountAsync(string query = null, Expression<Func<Customer, bool>> where = null)
        {
            IQueryable<Customer> items = _dataSource.Customers;

            // Query
            if (!String.IsNullOrEmpty(query))
            {
                items = items.Where(r => r.SearchTerms.Contains(query.ToLower()));
            }

            // Where
            if (where != null)
            {
                items = items.Where(where);
            }

            return await items.CountAsync();
        }

        public async Task<PageResult<Customer>> GetCustomersAsync(PageRequest<Customer> request)
        {
            return await GetCustomersAsync(request.PageIndex * request.PageSize, request.PageSize, new DataRequest<Customer>());
        }

        public async Task<PageResult<Customer>> GetCustomersAsync(int skip, int take, DataRequest<Customer> request)
        {
            IQueryable<Customer> items = _dataSource.Customers;

            // Where
            if (request.Where != null)
            {
                items = items.Where(request.Where);
            }

            // Query
            if (!String.IsNullOrEmpty(request.Query))
            {
                items = items.Where(r => r.SearchTerms.Contains(request.Query.ToLower()));
            }

            // Count
            int count = await items.CountAsync();
            if (count > 0)
            {
                // Order By
                if (request.OrderBy != null)
                {
                    items = items.OrderBy(request.OrderBy);
                }
                if (request.OrderByDesc != null)
                {
                    items = items.OrderByDescending(request.OrderByDesc);
                }

                // Execute
                var records = await items.Skip(skip).Take(take)
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

                return new PageResult<Customer>(0, take, count, records);
            }
            return PageResult<Customer>.Empty();
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
