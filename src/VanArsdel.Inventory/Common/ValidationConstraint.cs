using System;

namespace VanArsdel.Inventory
{
    public interface IValidationConstraint<T>
    {
        Func<T, bool> Validate { get; }
        string Message { get; }
    }

    public class ValidationConstraint<T> : IValidationConstraint<T>
    {
        public Func<T, bool> Validate { get; set; }
        public string Message { get; set; }
    }

    public class RequiredConstraint<T> : IValidationConstraint<T>
    {
        public RequiredConstraint(string propertyName, Func<T, object> propertyValue)
        {
            PropertyName = propertyName;
            PropertyValue = propertyValue;
        }

        public string PropertyName { get; set; }
        public Func<T, object> PropertyValue { get; set; }

        Func<T, bool> IValidationConstraint<T>.Validate => ValidateProperty;

        private bool ValidateProperty(T model)
        {
            var value = PropertyValue(model);
            if (value != null)
            {
                return !String.IsNullOrEmpty(value.ToString());
            }
            return false;
        }

        string IValidationConstraint<T>.Message => $"Property '{PropertyName}' cannot be empty.";
    }

    public class NonZeroConstraint<T> : IValidationConstraint<T>
    {
        public NonZeroConstraint(string propertyName, Func<T, object> propertyValue)
        {
            PropertyName = propertyName;
            PropertyValue = propertyValue;
        }

        public string PropertyName { get; set; }
        public Func<T, object> PropertyValue { get; set; }

        Func<T, bool> IValidationConstraint<T>.Validate => ValidateProperty;

        private bool ValidateProperty(T model)
        {
            var value = PropertyValue(model);
            if (value != null)
            {
                if (Double.TryParse(value.ToString(), out double d))
                {
                    return d != 0;
                }
            }
            return true;
        }

        string IValidationConstraint<T>.Message => $"Property '{PropertyName}' cannot be zero.";
    }

    public class GreaterThanConstraint<T> : IValidationConstraint<T>
    {
        public GreaterThanConstraint(string propertyName, Func<T, object> propertyValue, double value)
        {
            PropertyName = propertyName;
            PropertyValue = propertyValue;
            Value = value;
        }

        public string PropertyName { get; set; }
        public Func<T, object> PropertyValue { get; set; }
        public double Value { get; set; }

        Func<T, bool> IValidationConstraint<T>.Validate => ValidateProperty;

        private bool ValidateProperty(T model)
        {
            var value = PropertyValue(model);
            if (value != null)
            {
                if (Double.TryParse(value.ToString(), out double d))
                {
                    return d > Value;
                }
            }
            return true;
        }

        string IValidationConstraint<T>.Message => $"Property '{PropertyName}' must be greater than {Value}.";
    }
}
