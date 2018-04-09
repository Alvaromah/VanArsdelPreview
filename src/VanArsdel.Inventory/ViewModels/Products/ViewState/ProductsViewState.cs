using System;
using System.Linq.Expressions;

using VanArsdel.Data;

namespace VanArsdel.Inventory.ViewModels
{
    public class ProductsViewState : ListViewState
    {
        static public ProductsViewState CreateDefault() => new ProductsViewState();

        public ProductsViewState()
        {
            OrderBy = r => r.Name;
        }

        public Expression<Func<Product, object>> OrderBy { get; set; }
        public Expression<Func<Product, object>> OrderByDesc { get; set; }
    }
}
