using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using VanArsdel.Inventory.Services;
using VanArsdel.Inventory.Providers;

namespace VanArsdel.Inventory.ViewModels
{
    abstract public partial class DetailsViewModel<TModel> : ViewModelBase where TModel : ModelBase
    {
        public event EventHandler ItemDeleted;

        public DetailsViewModel(IDataProviderFactory providerFactory, IServiceManager serviceManager) : base(serviceManager.Context)
        {
            ProviderFactory = providerFactory;
            NavigationService = serviceManager.NavigationService;
            MessageService = serviceManager.MessageService;
            DialogService = serviceManager.DialogService;
            LogService = serviceManager.LogService;
        }

        public IDataProviderFactory ProviderFactory { get; }
        public INavigationService NavigationService { get; }
        public IMessageService MessageService { get; }
        public IDialogService DialogService { get; }
        public ILogService LogService { get; }

        private TModel _modelBackup = null;

        public void StatusReady()
        {
            MessageService.Send(this, "StatusMessage", "Ready");
        }

        public void StatusMessage(string message)
        {
            MessageService.Send(this, "StatusMessage", message);
        }

        public void StatusError(string message)
        {
            MessageService.Send(this, "StatusError", message);
        }

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
                MessageService.Send(this, "ItemChanged", model);
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
                MessageService.Send(this, "ItemDeleted", model);
            }
            ItemUpdated();
        }

        private async Task TryExit()
        {
            if (!IsMainView)
            {
                if (NavigationService.CanGoBack)
                {
                    NavigationService.GoBack();
                }
                else
                {
                    await NavigationService.CloseViewAsync();
                }
            }
        }

        virtual protected IEnumerable<IValidationConstraint<TModel>> ValidationConstraints => Enumerable.Empty<IValidationConstraint<TModel>>();

        public Result Validate()
        {
            return Validate(Item);
        }
        public Result Validate(TModel model)
        {
            foreach (var constraint in ValidationConstraints)
            {
                if (!constraint.Validate(model))
                {
                    return Result.Error("Validation Error", constraint.Message);
                }
            }
            return Result.Ok();
        }

        virtual protected void ItemUpdated() { }

        abstract public bool IsNewItem { get; }
        abstract protected Task SaveItemAsync(TModel model);
        abstract protected Task DeleteItemAsync(TModel model);
    }
}
