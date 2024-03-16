using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace WpfApp1
{
    /// <summary>
    /// Base class implementing common functionality for data binding with validation support.
    /// </summary>
    public abstract class BaseBindable : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, List<string>> _propertyErrors = new();
        private readonly Dictionary<string, Func<bool>> _validationRules = new();

        /// <summary>
        /// Event raised when errors have changed.
        /// </summary>
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        /// <summary>
        /// Event raised when a property has changed.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Gets a value indicating whether the object is currently considered valid.
        /// </summary>
        public bool IsValid => !HasErrors;

        /// <summary>
        /// Gets a value indicating whether the object has validation errors.
        /// </summary>
        public bool HasErrors => _propertyErrors.Count > 0;

        /// <summary>
        /// Gets the validation errors for the specified property.
        /// </summary>
        /// <param name="propertyName">The name of the property to retrieve errors for.</param>
        /// <returns>The validation errors for the specified property.</returns>
        public IEnumerable GetErrors(string? propertyName)
        {
            return GetPropertyErrors(propertyName);
        }

        /// <summary>
        /// Gets the validation errors for the specified property asynchronously.
        /// </summary>
        /// <param name="propertyName">The name of the property to retrieve errors for.</param>
        /// <returns>The validation errors for the specified property.</returns>
        public async Task<IEnumerable> GetErrorsAsync(string propertyName)
        {
            if (_validationRules.TryGetValue(propertyName, out var validationRule))
            {
                bool isValid = await Task.Run(() => validationRule.Invoke());
                if (!isValid)
                    return GetPropertyErrors(propertyName);
            }

            return Enumerable.Empty<string>();
        }

        /// <summary>
        /// Gets the validation errors for the specified property.
        /// </summary>
        /// <param name="propertyName">The name of the property to retrieve errors for.</param>
        /// <returns>The validation errors for the specified property.</returns>
        public IEnumerable<string>? GetPropertyErrors(string? propertyName)
        {
            if (propertyName == null)
                return _propertyErrors.Values.SelectMany(x => x);

            return _propertyErrors.GetValueOrDefault(propertyName);
        }

        /// <summary>
        /// Adds a validation rule for the specified property.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="validationRule">The validation rule function.</param>
        protected void AddValidationRule(string propertyName, Func<bool> validationRule)
        {
            _validationRules[propertyName] = validationRule;
        }

        /// <summary>
        /// Updates the specified property and triggers validation.
        /// </summary>
        /// <typeparam name="T">The type of the property.</typeparam>
        /// <param name="property">Reference to the property.</param>
        /// <param name="value">The new value for the property.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="dependantPropertyNames">Optional dependent property names to trigger additional updates.</param>
        /// <returns>True if the property was updated; otherwise, false.</returns>
        protected bool UpdateProperty<T>(ref T property, T value, string propertyName, params string[] dependantPropertyNames)
        {
            return UpdatePropertyValidated(ref property, value, null, propertyName, dependantPropertyNames);
        }

        /// <summary>
        /// Updates the specified property with validation and triggers additional updates.
        /// </summary>
        /// <typeparam name="T">The type of the property.</typeparam>
        /// <param name="property">Reference to the property.</param>
        /// <param name="value">The new value for the property.</param>
        /// <param name="validate">Optional validation action.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="dependantPropertyNames">Optional dependent property names to trigger additional updates.</param>
        /// <returns>True if the property was updated; otherwise, false.</returns>
        protected bool UpdatePropertyValidated<T>(ref T property, T value, Action? validate, string propertyName, params string[] dependantPropertyNames)
        {
            if (EqualityComparer<T>.Default.Equals(property, value))
                return false;

            if (property != null)
                PrePropertyUpdated(propertyName);
            property = value;

            bool wasValid = IsValid;
            validate?.Invoke();
            if (wasValid != IsValid)
                OnPropertyChanged(nameof(IsValid));

            OnPropertyChanged(propertyName);
            foreach (var name in dependantPropertyNames)
                OnPropertyChanged(name);

            return true;
        }

        /// <summary>
        /// Adds a validation error for the specified property.
        /// </summary>
        /// <param name="propertyName">The name of the property with the error.</param>
        /// <param name="error">The validation error message.</param>
        protected void AddPropertyError(string propertyName, string error)
        {
            if (!_propertyErrors.TryGetValue(propertyName, out var errorList))
            {
                errorList = new List<string>();
                _propertyErrors.Add(propertyName, errorList);
            }

            errorList.Add(error);
            OnErrorsChanged(propertyName);
        }

        /// <summary>
        /// Clears all validation errors for the specified property.
        /// </summary>
        /// <param name="propertyName">The name of the property to clear errors for.</param>
        protected void ClearPropertyErrors(string propertyName)
        {
            if (_propertyErrors.Remove(propertyName))
                OnErrorsChanged(propertyName);
        }

        /// <summary>
        /// Method called before updating a property value.
        /// </summary>
        /// <param name="propertyName">The name of the property being updated.</param>
        protected virtual void PrePropertyUpdated(string propertyName) { }

        /// <summary>
        /// Raises the ErrorsChanged event for the specified property.
        /// </summary>
        /// <param name="propertyName">The name of the property with changed errors.</param>
        protected virtual void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Raises the PropertyChanged event for the specified property.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
