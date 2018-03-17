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
                models.Add(await CreateOrderModelAsync(item, includeAllFields: false));
            }
            return new PageResult<OrderModel>(page.PageIndex, page.PageSize, page.Count, models);
        }

        public async Task<OrderModel> GetOrderAsync(long id)
        {
            var item = await DataService.GetOrderAsync(id);
            return await CreateOrderModelAsync(item, includeAllFields: true);
        }

        public async Task<OrderModel> CreateNewOrderAsync(long customerID)
        {
            var model = new OrderModel
            {
                CustomerID = customerID,
                OrderDate = DateTime.UtcNow,
                Status = 1
            };
            if (customerID > 0)
            {
                var parent = await DataService.GetCustomerAsync(customerID);
                if (parent != null)
                {
                    model.CustomerID = customerID;
                    model.ShipAddress = parent.AddressLine1;
                    model.ShipCity = parent.City;
                    model.ShipRegion = parent.Region;
                    model.ShipCountryCode = parent.CountryCode;
                    model.ShipPostalCode = parent.PostalCode;
                    model.Customer = await CreateCustomerModelAsync(parent, includeAllFields: true);
                }
            }
            return model;
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

        public async Task<int> DeleteOrderAsync(OrderModel model)
        {
            return await DataService.DeleteOrderAsync(model.OrderID);
        }

        private async Task<OrderModel> CreateOrderModelAsync(Order source, bool includeAllFields)
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
            if (source.Customer != null)
            {
                model.Customer = await CreateCustomerModelAsync(source.Customer, includeAllFields);
            }
            return model;
        }

        private void UpdateOrderFromModel(Order target, OrderModel source)
        {
            target.CustomerID = source.CustomerID;
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
