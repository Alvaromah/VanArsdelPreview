using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace VanArsdel.Data.Services
{
    partial class DataServiceBase
    {
        public async Task<PageResult<OrderItem>> GetOrderItemsAsync(PageRequest<OrderItem> request)
        {
            // Where
            IQueryable<OrderItem> items = _dataSource.OrderItems;
            if (request.Where != null)
            {
                items = items.Where(request.Where);
            }

            // Query
            // Not supported
            //if (!String.IsNullOrEmpty(request.Query))
            //{
            //    items = items.Where(r => r.SearchTerms.Contains(request.Query));
            //}

            // Count
            int count = items.Count();
            if (count > 0)
            {
                int pageSize = Math.Min(count, request.PageSize);
                int index = Math.Min(Math.Max(0, count - 1) / pageSize, request.PageIndex);

                // Order By
                if (request.OrderBy != null)
                {
                    items = request.Descending ? items.OrderByDescending(request.OrderBy) : items.OrderBy(request.OrderBy);
                }

                // Execute
                var records = await items.Skip(index * pageSize).Take(pageSize)
                    .Include(r => r.Product)
                    .AsNoTracking().ToListAsync();

                return new PageResult<OrderItem>(index, pageSize, count, records);
            }
            return PageResult<OrderItem>.Empty();
        }

        public async Task<OrderItem> GetOrderItemAsync(long orderID, int orderLine)
        {
            return await _dataSource.OrderItems
                .Where(r => r.OrderID == orderID && r.OrderLine == orderLine)
                .Include(r => r.Product)
                .FirstOrDefaultAsync();
        }

        public async Task<int> UpdateOrderItemAsync(OrderItem orderItem)
        {
            if (orderItem.OrderLine > 0)
            {
                _dataSource.Entry(orderItem).State = EntityState.Modified;
            }
            else
            {
                orderItem.OrderLine = _dataSource.OrderItems.Where(r => r.OrderID == orderItem.OrderID).Select(r => r.OrderLine).DefaultIfEmpty(0).Max() + 1;
                // TODO: 
                //orderItem.CreateOn = DateTime.UtcNow;
                _dataSource.Entry(orderItem).State = EntityState.Added;
            }
            // TODO: 
            //orderItem.LastModifiedOn = DateTime.UtcNow;
            //orderItem.SearchTerms = orderItem.BuildSearchTerms();
            return await _dataSource.SaveChangesAsync();
        }

        public async Task<int> DeleteOrderItemAsync(long orderID, int orderLine)
        {
            var item = _dataSource.OrderItems.Where(r => r.OrderID == orderID && r.OrderLine == orderLine).Select(r => new OrderItem { OrderID = r.OrderID, OrderLine = r.OrderLine }).FirstOrDefault();
            if (item != null)
            {
                _dataSource.OrderItems.Remove(item);
                return await _dataSource.SaveChangesAsync();
            }
            return 0;
        }
    }
}
