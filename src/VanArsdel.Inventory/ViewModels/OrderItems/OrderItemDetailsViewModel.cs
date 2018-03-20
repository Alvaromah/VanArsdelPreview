using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

using VanArsdel.Inventory.Models;
using VanArsdel.Inventory.Providers;

namespace VanArsdel.Inventory.ViewModels
{
    public class OrderItemDetailsViewModel : DetailsViewModel<OrderItemModel>
    {
        public OrderItemDetailsViewModel(IDataProviderFactory providerFactory) : base(providerFactory)
        {
        }

        override public string Title => (Item?.IsNew ?? true) ? TitleNew : TitleEdit;
        public string TitleNew => $"New Order Item, Order #{OrderID}";
        public string TitleEdit => $"Order Item #{Item?.OrderID} - {Item?.OrderLine}" ?? String.Empty;

        public override bool IsNewItem => Item?.IsNew ?? false;

        public long OrderID { get; set; }

        public ICommand ProductSelectedCommand => new RelayCommand<ProductModel>(ProductSelected);
        private void ProductSelected(ProductModel product)
        {
            Item.ProductID = product.ProductID;
            Item.UnitPrice = product.ListPrice;
            Item.Product = product;

            Item.NotifyChanges();
        }

        protected override void ItemUpdated()
        {
            NotifyPropertyChanged(nameof(Title));
        }

        public async Task LoadAsync(OrderItemViewState state)
        {
            OrderID = state.OrderID;
            if (state.OrderLine > 0)
            {
                using (var dp = ProviderFactory.CreateDataProvider())
                {
                    Item = await dp.GetOrderItemAsync(OrderID, state.OrderLine);
                }
            }
            else
            {
                Item = new OrderItemModel { OrderID = OrderID };
                IsEditMode = true;
            }
        }

        protected override async Task SaveItemAsync(OrderItemModel model)
        {
            using (var dataProvider = ProviderFactory.CreateDataProvider())
            {
                await Task.Delay(100);
                await dataProvider.UpdateOrderItemAsync(model);
                NotifyPropertyChanged(nameof(Title));
            }
        }

        protected override async Task DeleteItemAsync(OrderItemModel model)
        {
            using (var dataProvider = ProviderFactory.CreateDataProvider())
            {
                await dataProvider.DeleteOrderItemAsync(model);
            }
        }

        protected override async Task<bool> ConfirmDeleteAsync()
        {
            return await DialogBox.ShowAsync("Confirm Delete", "Are you sure you want to delete current order item?", "Ok", "Cancel");
        }

        override protected IEnumerable<IValidationConstraint<OrderItemModel>> ValidationConstraints
        {
            get
            {
                // TODO: 
                yield break;
            }
        }
    }
}
