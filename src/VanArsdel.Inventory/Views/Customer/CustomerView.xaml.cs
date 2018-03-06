using System;
using System.Threading.Tasks;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using VanArsdel.Inventory.ViewModels;
using VanArsdel.Inventory.Providers;
using VanArsdel.Inventory.Controls;

namespace VanArsdel.Inventory.Views
{
    public sealed partial class CustomerView : Page
    {
        public CustomerView()
        {
            InitializeViewModel();
            InitializeComponent();
        }

        public CustomerDetailsViewModel ViewModel { get; private set; }

        private void InitializeViewModel()
        {
            ViewModel = new CustomerDetailsViewModel(new DataProviderFactory());
            ViewModel.UpdateView += OnUpdateView;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            NavigationService.Main.HideBackButton();

            var state = e.Parameter as CustomerViewState;
            state = state ?? CustomerViewState.CreateDefault();
            await ViewModel.LoadAsync(state);
            this.SetTitle(ViewModel.Title);

            if (state.IsNew)
            {
                await Task.Delay(100);
                details.SetFocus();
            }

            Bindings.Update();
        }

        private void OnUpdateView(object sender, EventArgs e)
        {
            this.SetTitle(ViewModel.Title);
            Bindings.Update();
        }

        private async void OnDetailToolbarClick(object sender, ToolbarButtonClickEventArgs e)
        {
            switch (e.ClickedButton)
            {
                case ToolbarButton.Back:
                    if (NavigationService.Main.CanGoBack)
                    {
                        NavigationService.Main.GoBack();
                    }
                    break;
                case ToolbarButton.Edit:
                    ViewModel.BeginEdit();
                    break;
                case ToolbarButton.Cancel:
                    this.Focus(FocusState.Programmatic);
                    if (ViewModel.Item.IsNew)
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
                    else
                    {
                        ViewModel.CancelEdit();
                    }
                    break;
                case ToolbarButton.Save:
                    var result = ViewModel.Validate();
                    if (result.IsOk)
                    {
                        this.Focus(FocusState.Programmatic);
                        await ViewModel.SaveAsync();
                    }
                    else
                    {
                        await DialogBox.ShowAsync(result);
                    }
                    break;
                case ToolbarButton.Delete:
                    if (await DialogBox.ShowAsync("Confirm Delete", "Are you sure you want to delete current customer?", "Ok", "Cancel"))
                    {
                        await ViewModel.DeletetAsync();
                    }
                    break;
            }
        }

        private void OnInputGotFocus(object sender, RoutedEventArgs e)
        {
            if (!ViewModel.IsEditMode)
            {
                ViewModel.BeginEdit();
            }
        }

        private async void OpenInNewView(object sender, RoutedEventArgs e)
        {
            ViewModel.IsEditMode = false;
            await ViewManager.Current.CreateNewView(typeof(CustomerView), new CustomerViewState { CustomerID = ViewModel.Item.CustomerID });
            NavigationService.Main.GoBack();
        }
    }
}
