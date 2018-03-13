using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using VanArsdel.Inventory.Models;
using VanArsdel.Inventory.Providers;

namespace VanArsdel.Inventory.ViewModels
{
    public class OrderDetailsViewModel : DetailsViewModel<OrderModel>
    {
        public OrderDetailsViewModel(IDataProviderFactory providerFactory) : base(providerFactory)
        {
        }

        override public string Title => ((Item?.IsNew) ?? false) ? "New Order" : $"Order #{Item?.OrderID}" ?? String.Empty;

        public override bool IsNewItem => Item?.IsNew ?? false;

        protected override void ItemUpdated()
        {
            NotifyPropertyChanged(nameof(Title));
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
            }
        }

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

        protected override async Task<bool> ConfirmDeleteAsync()
        {
            return await DialogBox.ShowAsync("Confirm Delete", "Are you sure you want to delete current order?", "Ok", "Cancel");
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
