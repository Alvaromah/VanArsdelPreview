using System;
using System.Threading.Tasks;

using VanArsdel.Inventory.Controls;
using VanArsdel.Inventory.Providers;

namespace VanArsdel.Inventory.ViewModels
{
    abstract public class DetailsViewModel<TModel> : ViewModelBase<TModel> where TModel : ModelBase
    {
        public DetailsViewModel(IDataProviderFactory providerFactory)
        {
            ProviderFactory = providerFactory;
        }

        public IDataProviderFactory ProviderFactory { get; }

        public bool IsDataAvailable => _item != null;
        public bool IsDataUnavailable => !IsDataAvailable;

        private TModel _item = null;
        public TModel Item
        {
            get => _item;
            set
            {
                if (Set(ref _item, value))
                {
                    IsEnabled = true;
                    IsDeleted = false;
                }
            }
        }

        private DetailToolbarMode _toolbarMode = DetailToolbarMode.Default;
        public DetailToolbarMode ToolbarMode
        {
            get => _toolbarMode;
            set => Set(ref _toolbarMode, value);
        }

        private bool _isEditMode = false;
        public bool IsEditMode
        {
            get => _isEditMode;
            set => Set(ref _isEditMode, value);
        }

        private bool _isEnabled = true;
        public bool IsEnabled
        {
            get => _isEnabled;
            set => Set(ref _isEnabled, value);
        }

        private bool _isDeleted = false;
        public bool IsDeleted
        {
            get => _isDeleted;
            set => Set(ref _isDeleted, value);
        }

        private TModel _modelBackup = null;

        public void BeginEdit()
        {
            IsEditMode = true;
            ToolbarMode = DetailToolbarMode.CancelSave;
            _modelBackup = Item.Clone() as TModel;
            RaiseUpdateView();
        }

        public void CancelEdit()
        {
            if (IsEditMode)
            {
                IsEditMode = false;
                ToolbarMode = DetailToolbarMode.Default;
                var selected = Item;
                if (selected != null)
                {
                    selected.Merge(_modelBackup);
                    selected.NotifyChanges();
                }
                _modelBackup = null;
                RaiseUpdateView();
            }
        }

        public async Task SaveAsync()
        {
            IsEditMode = false;
            ToolbarMode = DetailToolbarMode.Default;
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
            RaiseUpdateView();
        }

        public async Task DeletetAsync()
        {
            var model = Item;
            if (model != null)
            {
                IsEnabled = false;
                await Task.Delay(100);
                await DeleteItemAsync(model);
                IsDeleted = true;
                IsEnabled = true;
            }
        }

        public Result Validate()
        {
            return base.Validate(Item);
        }

        abstract protected Task SaveItemAsync(TModel model);
        abstract protected Task DeleteItemAsync(TModel model);
    }
}
