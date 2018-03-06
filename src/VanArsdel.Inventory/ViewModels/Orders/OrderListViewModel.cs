using System;
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

        private OrdersViewState _state = null;

        public async Task LoadAsync(OrdersViewState state)
        {
            _state = state ?? OrdersViewState.CreateDefault();
            await base.RefreshAsync();
        }

        override public async Task<PageResult<OrderModel>> GetItemsAsync(IDataProvider dataProvider)
        {
            var request = new PageRequest<Order>(PageIndex, 10)
            {
                Query = Query,
                OrderBy = r => r.OrderID
            };
            if (_state.CustomerID > 0)
            {
                request.Where = (r) => r.CustomerID == _state.CustomerID;
            }
            return await dataProvider.GetOrdersAsync(request);
        }
    }
}
