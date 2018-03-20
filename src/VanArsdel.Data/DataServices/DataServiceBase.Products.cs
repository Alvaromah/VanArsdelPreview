using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace VanArsdel.Data.Services
{
    partial class DataServiceBase
    {
        public async Task<PageResult<Product>> GetProductsAsync(PageRequest<Product> request)
        {
            // Where
            IQueryable<Product> items = _dataSource.Products;
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
            int count = items.Count();
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

                return new PageResult<Product>(index, pageSize, count, records);
            }
            return PageResult<Product>.Empty();
        }

        public async Task<Product> GetProductAsync(string id)
        {
            return await _dataSource.Products.Where(r => r.ProductID == id).FirstOrDefaultAsync();
        }

        public async Task<int> UpdateProductAsync(Product product)
        {
            if (!String.IsNullOrEmpty(product.ProductID))
            {
                _dataSource.Entry(product).State = EntityState.Modified;
            }
            else
            {
                // TODO: Generate friendly ID
                product.ProductID = Guid.NewGuid().ToString();
                product.CreatedOn = DateTime.UtcNow;
                _dataSource.Entry(product).State = EntityState.Added;
            }
            product.LastModifiedOn = DateTime.UtcNow;
            product.SearchTerms = product.BuildSearchTerms();
            return await _dataSource.SaveChangesAsync();
        }

        public async Task<int> DeleteProductAsync(string id)
        {
            // TODO: Delete children
            var item = _dataSource.Products.Where(r => r.ProductID == id).Select(r => new Product { ProductID = r.ProductID }).FirstOrDefault();
            if (item != null)
            {
                _dataSource.Products.Remove(item);
                return await _dataSource.SaveChangesAsync();
            }
            return 0;
        }
    }
}
