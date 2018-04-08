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
        public async Task<int> GetCustomersCountAsync(DataRequest<Customer> request)
        {
            return await DataService.GetCustomersCountAsync(request);
        }

        public async Task<IList<CustomerModel>> GetCustomersAsync(int skip, int take, DataRequest<Customer> request)
        {
            var models = new List<CustomerModel>();
            var items = await DataService.GetCustomersAsync(skip, take, request);
            foreach (var item in items)
            {
                models.Add(await CreateCustomerModelAsync(item, includeAllFields: false));
            }
            return models;
        }

        public async Task<CustomerModel> GetCustomerAsync(long id)
        {
            var item = await DataService.GetCustomerAsync(id);
            if (item != null)
            {
                return await CreateCustomerModelAsync(item, includeAllFields: true);
            }
            return null;
        }

        public async Task<int> UpdateCustomerAsync(CustomerModel model)
        {
            long id = model.CustomerID;
            var customer = id > 0 ? await DataService.GetCustomerAsync(model.CustomerID) : new Customer();
            if (customer != null)
            {
                UpdateCustomerFromModel(customer, model);
                await DataService.UpdateCustomerAsync(customer);
                model.Merge(await GetCustomerAsync(customer.CustomerID));
            }
            return 0;
        }

        public async Task<int> DeleteCustomerAsync(CustomerModel model)
        {
            var customer = new Customer { CustomerID = model.CustomerID };
            return await DataService.DeleteCustomersAsync(customer);
        }

        public async Task<int> DeleteCustomerRangeAsync(int index, int length, DataRequest<Customer> request)
        {
            var items = await DataService.GetCustomerKeysAsync(index, length, request);
            return await DataService.DeleteCustomersAsync(items.ToArray());
        }

        private async Task<CustomerModel> CreateCustomerModelAsync(Customer source, bool includeAllFields)
        {
            var model = new CustomerModel()
            {
                CustomerID = source.CustomerID,
                Title = source.Title,
                FirstName = source.FirstName,
                MiddleName = source.MiddleName,
                LastName = source.LastName,
                Suffix = source.Suffix,
                Gender = source.Gender,
                EmailAddress = source.EmailAddress,
                AddressLine1 = source.AddressLine1,
                AddressLine2 = source.AddressLine2,
                City = source.City,
                Region = source.Region,
                CountryCode = source.CountryCode,
                PostalCode = source.PostalCode,
                Phone = source.Phone,
                CreatedOn = source.CreatedOn.AsDateTimeOffset(),
                LastModifiedOn = source.LastModifiedOn.AsNullableDateTimeOffset(),
                Thumbnail = source.Thumbnail,
                ThumbnailBitmap = await BitmapTools.LoadBitmapAsync(source.Thumbnail)
            };
            if (includeAllFields)
            {
                model.BirthDate = source.BirthDate.AsNullableDateTimeOffset();
                model.Education = source.Education;
                model.Occupation = source.Occupation;
                model.YearlyIncome = source.YearlyIncome;
                model.MaritalStatus = source.MaritalStatus;
                model.TotalChildren = source.TotalChildren;
                model.ChildrenAtHome = source.ChildrenAtHome;
                model.IsHouseOwner = source.IsHouseOwner;
                model.NumberCarsOwned = source.NumberCarsOwned;
                model.Picture = source.Picture;
                model.PictureBitmap = await BitmapTools.LoadBitmapAsync(source.Picture);
            }
            return model;
        }

        private void UpdateCustomerFromModel(Customer target, CustomerModel source)
        {
            target.Title = source.Title;
            target.FirstName = source.FirstName;
            target.MiddleName = source.MiddleName;
            target.LastName = source.LastName;
            target.Suffix = source.Suffix;
            target.Gender = source.Gender;
            target.EmailAddress = source.EmailAddress;
            target.AddressLine1 = source.AddressLine1;
            target.AddressLine2 = source.AddressLine2;
            target.City = source.City;
            target.Region = source.Region;
            target.CountryCode = source.CountryCode;
            target.PostalCode = source.PostalCode;
            target.Phone = source.Phone;
            target.BirthDate = source.BirthDate.AsNullableDateTime();
            target.Education = source.Education;
            target.Occupation = source.Occupation;
            target.YearlyIncome = source.YearlyIncome;
            target.MaritalStatus = source.MaritalStatus;
            target.TotalChildren = source.TotalChildren;
            target.ChildrenAtHome = source.ChildrenAtHome;
            target.IsHouseOwner = source.IsHouseOwner;
            target.NumberCarsOwned = source.NumberCarsOwned;
            target.CreatedOn = source.CreatedOn.AsDateTime();
            target.LastModifiedOn = source.LastModifiedOn.AsNullableDateTime();
            target.Picture = source.Picture;
            target.Thumbnail = source.Thumbnail;
        }
    }
}
