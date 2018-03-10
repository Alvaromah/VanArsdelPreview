using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using VanArsdel.Data;
using VanArsdel.Inventory.Models;
using VanArsdel.Inventory.Providers;

namespace VanArsdel.Inventory.ViewModels
{
    public class OrderListViewModel : ListViewModel<OrderModel>
    {
        public OrderListViewModel(IDataProviderFactory providerFactory) : base(providerFactory)
        {
        }

        public OrdersViewState ViewState { get; private set; }

        public async Task LoadAsync(OrdersViewState state)
        {
            ViewState = state ?? OrdersViewState.CreateDefault();
            await base.RefreshAsync();
        }

        override public async Task<PageResult<OrderModel>> GetItemsAsync(IDataProvider dataProvider)
        {
            var request = new PageRequest<Order>(PageIndex, 10)
            {
                Query = Query,
                OrderBy = r => r.OrderID
            };
            if (ViewState.CustomerID > 0)
            {
                request.Where = (r) => r.CustomerID == ViewState.CustomerID;
            }
            return await dataProvider.GetOrdersAsync(request);
        }

        protected override async Task DeleteItemsAsync(IDataProvider dataProvider, IEnumerable<OrderModel> models)
        {
            foreach (var model in models)
            {
                await dataProvider.DeleteOrderAsync(model);
            }
        }
    }
}
