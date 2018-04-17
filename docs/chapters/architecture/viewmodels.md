# ViewModels
*(To see more information about what's a ViewModel, you can see the [MVVM section](../mvvm))*

# How ViewModels are defined
 In our application, there are two separate sets of ViewModels. As ViewModels, both kinds are thought to be representations of an abstract view. However, they are slighly different in the nature of the model they are linked to.

To understand it better, we will take one ViewModel of each kind:

**CustomersViewModel** from `VanArsdel.Inventory\ViewModels` and
**OrderModel** from `VanArsdel.INventure\Models`

In the first ViewModel (**CustomersViewModel**), we will soon appreciate that it contains a number of properties that are very high level. This ViewModel is joining services, with other child ViewModels to compose everything that the ConsumersView will need. In other words, this is a View-focused ViewModel, because it will use other ViewModels to provide the compelling properties to its View.

In the second ViewModel (**OrderModel**). We are talking about a ViewModel although we aren't prefixing the class name with "ViewModel", but with "Model", instead.

Notice that this ViewModel is almost a copy of the entity **Order** entity that is defined in **VanArsdel.Data**. Actually, this ViewModel is a representation of the Order entity, but with the special features that we need when using MVVM. The main feature is the change notification. Hence, a big difference with is "Data" counterpart is that it will implement the INotifyPropertyChanged.

These entity-oriented ViewModels are usually created to be used in higher level ViewModels (view-oriented), like the **CustomersViewModel** we mentioned above.

For example, a View-oriented ViewModel won't expose a property of type `List<Order>`, but an ObservableList<OrderViewModel>. This makes it possible to observe when new items are added, deleted o replaced, and when individual properties of those items are modified as well, something that is absolutely desirable when using MVVM.
