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
        public async Task<int> GetProductsCountAsync(DataRequest<Product> request)
        {
            return await DataService.GetProductsCountAsync(request);
        }

        public async Task<IList<ProductModel>> GetProductsAsync(int skip, int take, DataRequest<Product> request)
        {
            var models = new List<ProductModel>();
            var items = await DataService.GetProductsAsync(skip, take, request);
            foreach (var item in items)
            {
                models.Add(await CreateProductModelAsync(item, includeAllFields: false));
            }
            return models;
        }

        public async Task<ProductModel> GetProductAsync(string id)
        {
            var item = await DataService.GetProductAsync(id);
            if (item != null)
            {
                return await CreateProductModelAsync(item, includeAllFields: true);
            }
            return null;
        }

        public async Task<int> UpdateProductAsync(ProductModel model)
        {
            string id = model.ProductID;
            var product = !String.IsNullOrEmpty(id) ? await DataService.GetProductAsync(model.ProductID) : new Product();
            if (product != null)
            {
                UpdateProductFromModel(product, model);
                await DataService.UpdateProductAsync(product);
                model.Merge(await GetProductAsync(product.ProductID));
            }
            return 0;
        }

        public async Task<int> DeleteProductAsync(ProductModel model)
        {
            var product = new Product { ProductID = model.ProductID };
            return await DataService.DeleteProductsAsync(product);
        }

        public async Task<int> DeleteProductRangeAsync(int index, int length, DataRequest<Product> request)
        {
            var items = await DataService.GetProductKeysAsync(index, length, request);
            return await DataService.DeleteProductsAsync(items.ToArray());
        }

        private async Task<ProductModel> CreateProductModelAsync(Product source, bool includeAllFields)
        {
            var model = new ProductModel()
            {
                ProductID = source.ProductID,
                CategoryID = source.CategoryID,
                SubCategoryID = source.SubCategoryID,
                Name = source.Name,
                Description = source.Description,
                Size = source.Size,
                Color = source.Color,
                Gender = source.Gender,
                ListPrice = source.ListPrice,
                DealerPrice = source.DealerPrice,
                TaxType = source.TaxType,
                Discount = source.Discount,
                DiscountStartDate = source.DiscountStartDate.AsNullableDateTimeOffset(),
                DiscountEndDate = source.DiscountEndDate.AsNullableDateTimeOffset(),
                StockUnits = source.StockUnits,
                SafetyStockLevel = source.SafetyStockLevel,
                StartDate = source.StartDate.AsNullableDateTimeOffset(),
                EndDate = source.EndDate.AsNullableDateTimeOffset(),
                CreatedOn = source.CreatedOn.AsDateTimeOffset(),
                LastModifiedOn = source.LastModifiedOn.AsDateTimeOffset(),
                Thumbnail = source.Thumbnail,
                ThumbnailBitmap = await BitmapTools.LoadBitmapAsync(source.Thumbnail)
            };

            if (includeAllFields)
            {
                model.Picture = source.Picture;
                model.PictureBitmap = await BitmapTools.LoadBitmapAsync(source.Picture);
            }
            return model;
        }

        private void UpdateProductFromModel(Product target, ProductModel source)
        {
            target.CategoryID = source.CategoryID;
            target.SubCategoryID = source.SubCategoryID;
            target.Name = source.Name;
            target.Description = source.Description;
            target.Size = source.Size;
            target.Color = source.Color;
            target.Gender = source.Gender;
            target.ListPrice = source.ListPrice;
            target.DealerPrice = source.DealerPrice;
            target.TaxType = source.TaxType;
            target.Discount = source.Discount;
            target.DiscountStartDate = source.DiscountStartDate.AsNullableDateTime();
            target.DiscountEndDate = source.DiscountEndDate.AsNullableDateTime();
            target.StockUnits = source.StockUnits;
            target.SafetyStockLevel = source.SafetyStockLevel;
            target.StartDate = source.StartDate.AsNullableDateTime();
            target.EndDate = source.EndDate.AsNullableDateTime();
            target.CreatedOn = source.CreatedOn.AsDateTime();
            target.LastModifiedOn = source.LastModifiedOn.AsDateTime();
            target.Picture = source.Picture;
            target.Thumbnail = source.Thumbnail;
        }
    }
}
