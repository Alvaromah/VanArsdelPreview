using System;
using System.Threading.Tasks;

using VanArsdel.Inventory.Services;

namespace VanArsdel.Inventory.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        public SettingsViewModel(IContext context) : base(context)
        {
        }

        public string Version => $"v{AppSettings.Current.Version}";

        public SettingsViewState ViewState { get; private set; }

        public Task LoadAsync(SettingsViewState viewState)
        {
            ViewState = viewState;
            return Task.CompletedTask;
        }
    }
}
