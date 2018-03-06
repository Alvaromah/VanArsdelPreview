using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using VanArsdel.Data;
using VanArsdel.Inventory.Models;

namespace VanArsdel.Inventory.Providers
{
    partial class SQLBaseProvider
    {
        public async Task<PageResult<OrderModel>> GetOrdersAsync(PageRequest<Order> request)
        {
            var models = new List<OrderModel>();
            var page = await DataService.GetOrdersAsync(request);
            foreach (var item in page.Items)
            {
                models.Add(CreateOrderModel(item, includeAllFields: false));
            }
            return new PageResult<OrderModel>(page.PageIndex, page.PageSize, page.Count, models);
        }

        public async Task<OrderModel> GetOrderAsync(long id)
        {
            var item = await DataService.GetOrderAsync(id);
            return CreateOrderModel(item, includeAllFields: true);
        }

        public async Task<int> DeleteOrderAsync(OrderModel model)
        {
            return await DataService.DeleteOrderAsync(model.OrderID);
        }

        public async Task<int> UpdateOrderAsync(OrderModel model)
        {
            long id = model.OrderID;
            var order = id > 0 ? await DataService.GetOrderAsync(model.OrderID) : new Order();
            if (order != null)
            {
                UpdateOrderFromModel(order, model);
                await DataService.UpdateOrderAsync(order);
                model.Merge(await GetOrderAsync(order.OrderID));
            }
            return 0;
        }

        private OrderModel CreateOrderModel(Order source, bool includeAllFields)
        {
            var model = new OrderModel()
            {
                OrderID = source.OrderID,
                CustomerID = source.CustomerID,
                OrderDate = source.OrderDate,
                ShippedDate = source.ShippedDate,
                DeliveredDate = source.DeliveredDate,
                Status = source.Status,
                PaymentType = source.PaymentType,
                TrackingNumber = source.TrackingNumber,
                ShipVia = source.ShipVia,
                ShipAddress = source.ShipAddress,
                ShipCity = source.ShipCity,
                ShipRegion = source.ShipRegion,
                ShipCountryCode = source.ShipCountryCode,
                ShipPostalCode = source.ShipPostalCode,
                ShipPhone = source.ShipPhone,
            };

            if (includeAllFields)
            {
                // TODO: Include OrderItems?
            }
            return model;
        }

        private void UpdateOrderFromModel(Order target, OrderModel source)
        {
            target.OrderDate = source.OrderDate;
            target.ShippedDate = source.ShippedDate;
            target.DeliveredDate = source.DeliveredDate;
            target.Status = source.Status;
            target.PaymentType = source.PaymentType;
            target.TrackingNumber = source.TrackingNumber;
            target.ShipVia = source.ShipVia;
            target.ShipAddress = source.ShipAddress;
            target.ShipCity = source.ShipCity;
            target.ShipRegion = source.ShipRegion;
            target.ShipCountryCode = source.ShipCountryCode;
            target.ShipPostalCode = source.ShipPostalCode;
            target.ShipPhone = source.ShipPhone;
        }
    }
}
