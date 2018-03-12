using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using VanArsdel.Data;
using VanArsdel.Inventory.Models;
using VanArsdel.Inventory.Providers;

namespace VanArsdel.Inventory.ViewModels
{
    public class OrderItemListViewModel : ListViewModel<OrderItemModel>
    {
        public OrderItemListViewModel(IDataProviderFactory providerFactory) : base(providerFactory)
        {
        }

        public OrderItemsViewState ViewState { get; private set; }

        public async Task LoadAsync(OrderItemsViewState state)
        {
            ViewState = state ?? OrderItemsViewState.CreateDefault();
            ApplyViewState(ViewState);
            await base.RefreshAsync();
        }

        public void Unload()
        {
            UpdateViewState(ViewState);
        }

        override public async Task<PageResult<OrderItemModel>> GetItemsAsync(IDataProvider dataProvider)
        {
            var request = new PageRequest<OrderItem>(PageIndex, PageSize)
            {
                Query = Query,
                OrderBy = r => r.OrderLine
            };
            if (ViewState.OrderID > 0)
            {
                request.Where = (r) => r.OrderID == ViewState.OrderID;
            }
            return await dataProvider.GetOrderItemsAsync(request);
        }

        protected override async Task DeleteItemsAsync(IDataProvider dataProvider, IEnumerable<OrderItemModel> models)
        {
            foreach (var model in models)
            {
                await dataProvider.DeleteOrderItemAsync(model);
            }
        }

        public CustomersViewState GetCurrentState()
        {
            var state = CustomersViewState.CreateDefault();
            UpdateViewState(state);
            return state;
        }
    }
}
