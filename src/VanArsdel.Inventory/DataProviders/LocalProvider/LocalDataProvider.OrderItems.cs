using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace VanArsdel.Inventory.Data
{
    partial class LocalDataProvider
    {
        public async Task<IList<OrderItem>> GetOrderItemsAsync(long orderID)
        {
            var items = _datasource.OrderItems.Where(r => r.OrderID == orderID).OrderBy(r => r.OrderLine);
            return await items.Include(r => r.Product).ToListAsync();
        }

        public async Task DeleteOrderItem(long orderID, int orderLine)
        {
            var item = _datasource.OrderItems.Where(r => r.OrderID == orderID && r.OrderLine == orderLine).FirstOrDefault();
            if (item != null)
            {
                _datasource.OrderItems.Remove(item);
                await _datasource.SaveChangesAsync();
            }
            else
            {
                // TODO: Handle issue "Trying to delete unexisting item"
            }
        }
    }
}
