# Navigation Service
The Navigation Service is a one of the most common services you may find in any mobile platform. This abstraction is found in one way or another due to the need to switch views depending on the flow of the application. UWP takes the same concept and provides the mechanisms to implement navigation with ease.

Since we are using MVVM, we don't want to make any explicit reference to any type that related with the presentation technology. Instead, we will use the abstraction that is intented to be used from ViewModels, to facilitate the ViewModel-based navigation. 

## ViewModel-navigation
This kind of navigation, oposed to View-based navigation, is the navigation that uses a ViewModel as the subject that determines the navigation. The View isn't specified explicitly. Instead, there is a mechanism to associate each ViewModel with its corresponding View. This is where our Navigation Service comes in. It will perform the navigation itself, but also the will glue the ViewModel with its View.

## Our Service

A basic Navigation Service like the one that is presented with this application consists of one or more variations of a `Navigate` method.

Let's take a look to its interface:

```csharp
public interface INavigationService
{
    bool IsMainView { get; }

    bool CanGoBack { get; }

    void Initialize(Frame frame);

    bool Navigate<TViewModel>(object parameter = null);
    bool Navigate(Type viewModelType, object parameter = null);

    Task<int> CreateNewViewAsync<TViewModel>(object parameter = null);
    Task<int> CreateNewViewAsync(Type viewModelType, object parameter = null);

    void GoBack();

    Task CloseViewAsync();
}
```

Notice that there are 2 overloads of the `Navigate` method. 

They are basically the same. The only different is that one takes a `Type` and the other takes the `Type` as a generic argument. The former is provided for flexibility, because the type can be determined at run-time. The latter fits better when you know the type of the ViewModel at compile-time.

The way it works is very simple: The caller invokes the `Navigate` method specifying the `Type` of the ViewModel to be navigated to and optionally, an object that the ViewModel will receive that will act as a parameter. This is useful when the target ViewModel needs to receive **some context** its operations. For example, `CustomerViewModel` would receive a a `Customer` for a detail view.

The Navigation Service sits between the View and the ViewModel. As the Navigate method takes the type of the ViewModel, our service will have to find the View that is associated with it. Then it will have to connect both to enable MVVM.

### View Lookup
The Navigation Service will need a mechanism to associate Views to ViewModels. In our implementation, this is done using a dictionary called _viewModelMap.

Whenever the Navigate method is called, this dictionary will be queried for the Type of the View that corresponds to the ViewModel.

Let's take a look to the implementation of the `Navigate` method:

```csharp
public bool Navigate(Type viewModelType, object parameter = null)
{
    if (Frame == null)
    {
        throw new InvalidOperationException("Navigation frame not initialized.");
    }

    return Frame.Navigate(GetView(viewModelType), parameter);
}
```

In the first place, we're checking whether the `Frame` is null. The Frame property is the object that will perform the navigation at the UI side. It's usually set an the very beginning of the execution. In our application, inside the `ShellView.InitializeNavigation()` method, where the service is resolved.

In the second place, where are telling the `Frame` to navigate to the View associated to `viewModelType`. The lookup is done in `GetView`

```csharp
static public Type GetView(Type viewModel)
{
    if (_viewModelMap.TryGetValue(viewModel, out Type view))
    {
        return view;
    }
    throw new InvalidOperationException($"View not registered for ViewModel '{viewModel.FullName}'");
}
```

There you can see the _viewModelMap being queried to get the Type of the View.

But how is the dictionary initialized?

### View Registration

In order to know the association between a ViewModel and its View, some kind of registration is needed. For this effect, we will expose a method in our implementation: The Register method.

```csharp
static public void Register<TViewModel, TView>() where TView : Page
{
    if (!_viewModelMap.TryAdd(typeof(TViewModel), typeof(TView)))
    {
        throw new InvalidOperationException($"ViewModel already registered '{typeof(TViewModel).FullName}'");
    }
}
```

It just adds both a new entry in the _viewModelMap dictionary relating both `Types` (The ViewModel and the View).

The registration is usually done at the beginning of the execution, so all entries are available as soon as possible. 

In our application, you can locate the registration in the `Setup` class, inside the `ConfigureNavigation` method.

```csharp
private static void ConfigureNavigation()
{
    NavigationService.Register<ShellViewModel, ShellView>();
    NavigationService.Register<MainShellViewModel, MainShellView>();

    NavigationService.Register<DashboardViewModel, DashboardView>();
	...
}