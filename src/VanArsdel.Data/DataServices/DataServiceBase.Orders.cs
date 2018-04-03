using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace VanArsdel.Data.Services
{
    partial class DataServiceBase
    {
        public async Task<PageResult<Order>> GetOrdersAsync(PageRequest<Order> request)
        {
            // Where
            IQueryable<Order> items = _dataSource.Orders;
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
                int pageSize = Math.Min(count, request.PageSize);
                int index = Math.Min(Math.Max(0, count - 1) / pageSize, request.PageIndex);

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
                var records = await items.Skip(index * pageSize).Take(pageSize).AsNoTracking().ToListAsync();

                return new PageResult<Order>(index, pageSize, count, records);
            }
            return PageResult<Order>.Empty();
        }

        public async Task<Order> GetOrderAsync(long id)
        {
            return await _dataSource.Orders.Where(r => r.OrderID == id)
                .Include(r => r.Customer)
                .FirstOrDefaultAsync();
        }

        public async Task<int> UpdateOrderAsync(Order order)
        {
            if (order.OrderID > 0)
            {
                _dataSource.Entry(order).State = EntityState.Modified;
            }
            else
            {
                order.OrderID = UIDGenerator.Next(4);
                order.OrderDate = DateTime.UtcNow;
                _dataSource.Entry(order).State = EntityState.Added;
            }
            // TODO: 
            //order.LastModifiedOn = DateTime.UtcNow;
            order.SearchTerms = order.BuildSearchTerms();
            return await _dataSource.SaveChangesAsync();
        }

        public async Task<int> DeleteOrderAsync(long id)
        {
            // TODO: Delete children
            var item = _dataSource.Orders.Where(r => r.OrderID == id).Select(r => new Order { OrderID = r.OrderID }).FirstOrDefault();
            if (item != null)
            {
                _dataSource.Orders.Remove(item);
                return await _dataSource.SaveChangesAsync();
            }
            return 0;
        }
    }
}
