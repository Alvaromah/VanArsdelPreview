using System;
using System.Threading.Tasks;

using VanArsdel.Inventory.Providers;
using VanArsdel.Inventory.Views;

namespace VanArsdel.Inventory.ViewModels
{
    abstract public partial class DetailsViewModel<TModel> : ViewModelBase<TModel> where TModel : ModelBase
    {
        public event EventHandler ItemDeleted;

        public DetailsViewModel(IDataProviderFactory providerFactory)
        {
            ProviderFactory = providerFactory;
        }

        public IDataProviderFactory ProviderFactory { get; }

        private TModel _modelBackup = null;

        public void BeginEdit()
        {
            IsEditMode = true;
            _modelBackup = Item.Clone() as TModel;
        }

        public void CancelEdit()
        {
            if (IsEditMode)
            {
                IsEditMode = false;
                var selected = Item;
                if (selected != null)
                {
                    selected.Merge(_modelBackup);
                    selected.NotifyChanges();
                }
                _modelBackup = null;
                ItemUpdated();
            }
        }

        public async Task SaveAsync()
        {
            IsEditMode = false;
            var model = Item;
            if (model != null)
            {
                IsEnabled = false;
                await Task.Delay(100);
                await SaveItemAsync(model);
                model.NotifyChanges();
                IsEnabled = true;
            }
            _modelBackup = null;
            ItemUpdated();
        }

        public async Task DeletetAsync()
        {
            var model = Item;
            if (model != null)
            {
                IsEnabled = false;
                await Task.Delay(100);
                await DeleteItemAsync(model);
                ItemDeleted?.Invoke(this, EventArgs.Empty);
            }
            ItemUpdated();
        }

        private async Task TryExit()
        {
            if (!IsMainView)
            {
                if (NavigationService.Main.CanGoBack)
                {
                    NavigationService.Main.GoBack();
                }
                else
                {
                    await ViewManager.Current.Close();
                }
            }
        }

        public Result Validate()
        {
            return base.Validate(Item);
        }

        virtual protected void ItemUpdated() { }

        abstract public bool IsNewItem { get; }
        abstract protected Task SaveItemAsync(TModel model);
        abstract protected Task DeleteItemAsync(TModel model);
    }
}
