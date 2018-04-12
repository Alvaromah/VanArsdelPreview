# Solution Structure

The VanArsdel Inventory application consists of 2 projects
1. VanArsdel.Inventory
2. VanArsdel.Data

## VanArsdel.Inventory
The first project is the application isself. It's a Universal Windows Application (to be run inside the Universal Windows Platform). It's the one that will be our startup project. In order to run it, we have to deploy it using either the x86 or the x64 platform configurations.

This is a snapshot of the structure of the solution:

![Solution Tree](../img/solution-tree.png)

Please, notice that we'are using a mixed approach to organize the folders in the projects.

- The root folders are organized following the tech-folders paradigm.
Folders like, *Common*, *Configuration*, *Controls*, *Converts*...  
- Second and subsequent levels follow a feature-folder approach. Examples are the Views and ViewModels folders.

Inside them, we have folders with names that denote the feature they're related to. You can see it in the following snapshot.

![Feature Folders](../img/feature-folders.png)

Both *Views* and *ViewModels* are almost a mirror, with little differences. 

Now, let's analyze the contents of each folder:

### Assets
It's a default folder that contains the non-programmatical resources of the application. A well-known example is images (bitmaps). There you can find the logos and images like the ones the application will use for the Microsoft Store, or the Splash Screen, will all of their different sizes.

### Common
We're using the folder to keep common abstractions like the *RelayCommand* or the *ValidationConstraint*. These classes aren't directly related to our domain and could be used in other projects, so the name "common"

### Configuration

We're storing here the configuration of the application. It's the natural folder to keep the *AppSettings* class, where we are placing constants and providing a way to communicate with the built-in `ApplicationData.Current.LocalSettings`, the mechanism that UWP provides to load/store user-specific configuration.

In this folder we can also find classes that determine the configuration (the behavior) of all the application, like the ServiceLocator and the Startup classes. They are part of the configuration because switching some implementations inside them, we can easily extend our app, since where are using Dependency Injection. Let's say that the configuration is the part of the application that we expect to change or would like to be able to change that is not part of the design.

# Controls

In this folder we will find custom controls, abstractions that are directly related with the User Interface. *IconButtons*, *IconLabels*, *Search* controls.

It's the place to put anything that inherits from the *Control* class, like *UserControls*.

# Converters

It's the folder that contains all the converters in the solution. Converters are specific classes to convert between 2 types of data. 

You may already know converters from other technologies like *WPF* or *Silverlight*. 

The are used mainly in the XAML definition of the views, as resources. They implement the `IValueConverter` interface and consist of 2 methods, one for converting from type A to to B, and another for converting back B to A.

For instance:

```csharp
public sealed class BoolNegationConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        return !(value is bool && (bool)value);
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        return !(value is bool && (bool)value);
    }
}
```

This converter Converts a boolean to a negated boolean. That's way both methods are the same.

Another sample is this converter that converts a DateTimeOffset to a formatted string.

```csharp
public sealed class DateTimeFormatConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is DateTimeOffset dateTime)
        {
            string format = parameter as String ?? "shortdate";
            var userLanguages = GlobalizationPreferences.Languages;
            var dateFormatter = new DateTimeFormatter(format, userLanguages);
            return dateFormatter.Format(dateTime);
        }
        return "N/A";
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotSupportedException();
    }
}
```

Please, notice that the ConvertBack isn't possible in this case, so the ConvertBack method will raise an exception when called. 