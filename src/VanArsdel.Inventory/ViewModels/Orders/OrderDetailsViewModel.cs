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

        override public string Title => (Item?.IsNew ?? true) ? TitleNew : TitleEdit;
        public string TitleNew => Item?.Customer == null ? "New Order" : $"New Order, {Item?.Customer?.FullName}";
        public string TitleEdit => Item == null ? "Order" : $"Order #{Item?.OrderID}";

        public override bool IsNewItem => Item?.IsNew ?? false;

        protected override void ItemUpdated()
        {
            NotifyPropertyChanged(nameof(Title));
        }

        public async Task LoadAsync(OrderViewState state)
        {
            using (var dp = ProviderFactory.CreateDataProvider())
            {
                if (state.OrderID > 0)
                {
                    Item = await dp.GetOrderAsync(state.OrderID);
                }
                else
                {
                    Item = await dp.CreateNewOrderAsync(state.CustomerID);
                    IsEditMode = true;
                }
            }
        }

        protected override async Task SaveItemAsync(OrderModel model)
        {
            using (var dataProvider = ProviderFactory.CreateDataProvider())
            {
                await Task.Delay(250);
                await dataProvider.UpdateOrderAsync(model);
                NotifyPropertyChanged(nameof(Title));
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
