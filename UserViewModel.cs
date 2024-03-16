using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace WpfApp1
{
    /// <summary>
    /// View model for managing user data and interactions in the application.
    /// </summary>
    public class UserViewModel : BaseBindable
    {
        private User _user;
        private bool _isDataVisible = false;
        private bool _isProcessing = false; // Flag indicating whether processing is ongoing

        /// <summary>
        /// Default constructor.
        /// </summary>
        public UserViewModel()
        {
            User = new User();
            ProceedCommand = new RelayCommand(_ => OnProceedClicked(), () => !_isProcessing); // Disable command if processing is ongoing
        }

        /// <summary>
        /// The user associated with the view model.
        /// </summary>
        public User User
        {
            get => _user;
            set
            {
                if (UpdateProperty(ref _user, value, nameof(User)))
                {
                    User.PropertyChanged += OnUserPropertyChanged;
                }
            }
        }

        /// <summary>
        /// Indicates whether user data is visible in the UI.
        /// </summary>
        public bool IsDataVisible
        {
            get => _isDataVisible;
            set => SetProperty(ref _isDataVisible, value);
        }

        /// <summary>
        /// Command to proceed with the current user data.
        /// </summary>
        public ICommand ProceedCommand { get; }

        /// <summary>
        /// Handler for the proceed button click event.
        /// </summary>
        private async void OnProceedClicked()
        {
            _isProcessing = true; // Set processing flag

            if (User.Age <= 0)
            {
                MessageBox.Show("Invalid age. Age cannot be less than or equal to 0.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                _isProcessing = false; // Clear processing flag
                return; // Exit method to avoid further execution
            }

            if (User.Age >= 135)
            {
                MessageBox.Show("Invalid age. Age cannot be greater than or equal to 135.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                _isProcessing = false; // Clear processing flag
                return; // Exit method to avoid further execution
            }

            OnUpdate();
            IsDataVisible = true;

            _isProcessing = false; // Clear processing flag
        }

        /// <summary>
        /// Updates user information asynchronously.
        /// </summary>
        private async void OnUpdate()
        {
            if (User.GetPropertyErrors(nameof(User.BirthDate)) is var errors && errors != null)
            {
                MessageBox.Show(errors.ElementAt(0), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                await Task.Run(() =>
                {
                    if (User.CalculateIsBirthdayToday(User.BirthDate))
                    {
                        Application.Current.Dispatcher.Invoke(() => ShowBirthdayWindow());
                    }
                    else
                    {
                        User.UpdateZodiacInfo();
                    }
                });

                // Display values of all 8 User class fields
                MessageBox.Show(
                    $"First Name: {User.FirstName}\n" +
                    $"Last Name: {User.LastName}\n" +
                    $"Email Address: {User.EmailAddress}\n" +
                    $"Birth Date: {User.BirthDate.ToShortDateString()}\n" +
                    $"Age: {User.Age}\n" +
                    $"Is Adult: {User.IsAdult}\n" +
                    $"Sun Sign: {User.SunSign}\n" +
                    $"Chinese Sign: {User.ChineseSign}",
                    "User Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// Shows the birthday window.
        /// </summary>
        private void ShowBirthdayWindow()
        {
            var birthdayViewModel = new BirthdayViewModel(User);
            var birthdayWindow = new BirthdayWindow { DataContext = birthdayViewModel };
            birthdayWindow.Show();
        }

        /// <summary>
        /// Called before updating a property value.
        /// </summary>
        protected override void PrePropertyUpdated(string propertyName)
        {
            if (propertyName == nameof(User))
            {
                User.PropertyChanged -= OnUserPropertyChanged;
            }
        }

        /// <summary>
        /// Handles the PropertyChanged event of the User object.
        /// </summary>
        private void OnUserPropertyChanged(object? sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == nameof(User.BirthDate))
            {
                ((RelayCommand)ProceedCommand).RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Sets property value and raises PropertyChanged event if value has changed.
        /// </summary>
        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
