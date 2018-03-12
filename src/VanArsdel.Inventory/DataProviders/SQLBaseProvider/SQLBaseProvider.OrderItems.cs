using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using VanArsdel.Data;
using VanArsdel.Inventory.Models;

namespace VanArsdel.Inventory.Providers
{
    partial class SQLBaseProvider
    {
        public async Task<PageResult<OrderItemModel>> GetOrderItemsAsync(PageRequest<OrderItem> request)
        {
            var models = new List<OrderItemModel>();
            var page = await DataService.GetOrderItemsAsync(request);
            foreach (var item in page.Items)
            {
                models.Add(CreateOrderItemModel(item, includeAllFields: false));
            }
            return new PageResult<OrderItemModel>(page.PageIndex, page.PageSize, page.Count, models);
        }

        public async Task<OrderItemModel> GetOrderItemAsync(long orderID, int lineID)
        {
            var item = await DataService.GetOrderItemAsync(orderID, lineID);
            return CreateOrderItemModel(item, includeAllFields: true);
        }

        public async Task<int> DeleteOrderItemAsync(OrderItemModel model)
        {
            return await DataService.DeleteOrderItemAsync(model.OrderID, model.OrderLine);
        }

        public async Task<int> UpdateOrderItemAsync(OrderItemModel model)
        {
            long orderID = model.OrderID;
            var orderItem = orderID > 0 ? await DataService.GetOrderItemAsync(model.OrderID, model.OrderLine) : new OrderItem();
            if (orderItem != null)
            {
                UpdateOrderItemFromModel(orderItem, model);
                await DataService.UpdateOrderItemAsync(orderItem);
                model.Merge(await GetOrderItemAsync(orderItem.OrderID, orderItem.OrderLine));
            }
            return 0;
        }

        private OrderItemModel CreateOrderItemModel(OrderItem source, bool includeAllFields)
        {
            var model = new OrderItemModel()
            {
                OrderID = source.OrderID,
                OrderLine = source.OrderLine,
                ProductID = source.ProductID,
                Quantity = source.Quantity,
                UnitPrice = source.UnitPrice,
                Discount = source.Discount,
                TaxType = source.TaxType,
            };

            if (includeAllFields)
            {
                // TODO: Include Picture?
            }
            return model;
        }

        private void UpdateOrderItemFromModel(OrderItem target, OrderItemModel source)
        {
            target.OrderLine = source.OrderLine;
            target.ProductID = source.ProductID;
            target.Quantity = source.Quantity;
            target.UnitPrice = source.UnitPrice;
            target.Discount = source.Discount;
            target.TaxType = source.TaxType;
        }
    }
}
