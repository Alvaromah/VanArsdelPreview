using System;

using Windows.UI.Xaml;

namespace VanArsdel.Inventory
{
    public class DependencyExpression
    {
        public DependencyExpression(string name, DependencyProperty[] dependencyProperties)
        {
            Name = name;
            DependencyProperties = dependencyProperties;
        }
        public DependencyExpression(string name, DependencyExpression[] dependencyExpressions)
        {
            Name = name;
            DependencyExpressions = dependencyExpressions;
        }

        public string Name { get; }

        public DependencyProperty[] DependencyProperties { get; }
        public DependencyExpression[] DependencyExpressions { get; }

        public override string ToString()
        {
            return $"{Name} {DependencyProperties} {DependencyExpressions}";
        }
    }
}
