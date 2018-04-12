using System;
using System.Linq.Expressions;

using VanArsdel.Data;

namespace VanArsdel.Inventory.ViewModels
{
    public class CustomersViewState : ListViewState
    {
        static public CustomersViewState CreateEmpty() => new CustomersViewState { IsEmpty = true };

        public CustomersViewState()
        {
            OrderBy = r => r.FirstName;
        }

        public Expression<Func<Customer, object>> OrderBy { get; set; }
        public Expression<Func<Customer, object>> OrderByDesc { get; set; }
    }
}
