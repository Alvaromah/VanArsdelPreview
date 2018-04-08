﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using VanArsdel.Data;
using VanArsdel.Inventory.Models;

namespace VanArsdel.Inventory.Providers
{
    partial class SQLBaseProvider
    {
        public async Task<int> GetOrdersCountAsync(DataRequest<Order> request)
        {
            return await DataService.GetOrdersCountAsync(request);
        }

        public async Task<IList<OrderModel>> GetOrdersAsync(int skip, int take, DataRequest<Order> request)
        {
            var models = new List<OrderModel>();
            var items = await DataService.GetOrdersAsync(skip, take, request);
            foreach (var item in items)
            {
                models.Add(await CreateOrderModelAsync(item, includeAllFields: false));
            }
            return models;
        }

        public async Task<OrderModel> GetOrderAsync(long id)
        {
            var item = await DataService.GetOrderAsync(id);
            if (item != null)
            {
                return await CreateOrderModelAsync(item, includeAllFields: true);
            }
            return null;
        }

        public async Task<OrderModel> CreateNewOrderAsync(long customerID)
        {
            var model = new OrderModel
            {
                CustomerID = customerID,
                OrderDate = DateTime.UtcNow,
                Status = 0
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
            var order = new Order { OrderID = model.OrderID };
            return await DataService.DeleteOrdersAsync(order);
        }

        public async Task<int> DeleteOrderRangeAsync(int index, int length, DataRequest<Order> request)
        {
            var items = await DataService.GetOrderKeysAsync(index, length, request);
            return await DataService.DeleteOrdersAsync(items.ToArray());
        }

        private async Task<OrderModel> CreateOrderModelAsync(Order source, bool includeAllFields)
        {
            var model = new OrderModel()
            {
                OrderID = source.OrderID,
                CustomerID = source.CustomerID,
                OrderDate = source.OrderDate.AsDateTimeOffset(),
                ShippedDate = source.ShippedDate.AsNullableDateTimeOffset(),
                DeliveredDate = source.DeliveredDate.AsNullableDateTimeOffset(),
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
            target.OrderDate = source.OrderDate.AsDateTime();
            target.ShippedDate = source.ShippedDate.AsNullableDateTime();
            target.DeliveredDate = source.DeliveredDate.AsNullableDateTime();
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
