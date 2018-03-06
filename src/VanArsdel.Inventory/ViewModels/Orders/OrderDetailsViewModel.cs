using System;
using System.Threading.Tasks;

using VanArsdel.Inventory.Models;
using VanArsdel.Inventory.Controls;
using VanArsdel.Inventory.Providers;
using System.Collections.Generic;

namespace VanArsdel.Inventory.ViewModels
{
    public class OrderDetailsViewModel : DetailsViewModel<OrderModel>
    {
        public OrderDetailsViewModel(IDataProviderFactory providerFactory) : base(providerFactory)
        {
        }

        override public string Title => ((Item?.IsNew) ?? false) ? "New Order" : $"Order #{Item?.OrderID}" ?? String.Empty;

        protected override async Task SaveItemAsync(OrderModel model)
        {
            using (var dataProvider = ProviderFactory.CreateDataProvider())
            {
                await dataProvider.UpdateOrderAsync(model);
            }
        }

        protected override async Task DeleteItemAsync(OrderModel model)
        {
            using (var dataProvider = ProviderFactory.CreateDataProvider())
            {
                await dataProvider.DeleteOrderAsync(model);
            }
        }

        public async Task LoadAsync(OrderViewState state)
        {
            if (state.OrderID > 0)
            {
                using (var dp = ProviderFactory.CreateDataProvider())
                {
                    Item = await dp.GetOrderAsync(state.OrderID);
                }
            }
            else
            {
                Item = new OrderModel();
                IsEditMode = true;
                ToolbarMode = DetailToolbarMode.CancelSave;
            }
            RaiseUpdateView();
        }

        override protected IEnumerable<IValidationConstraint<OrderModel>> ValidationConstraints
        {
            get
            {
                // TODO: 
                yield break;
            }
        }
    }
}
