using System;

namespace WpfApp1
{
    /// <summary>
    /// View model for the BirthdayWindow, providing a birthday message.
    /// </summary>
    public class BirthdayViewModel
    {
        /// <summary>
        /// Gets the birthday message.
        /// </summary>
        public string BirthdayMessage { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BirthdayViewModel"/> class.
        /// </summary>
        /// <param name="user">The user for whom the birthday message is displayed.</param>
        public BirthdayViewModel(User user)
        {
            // Generate the birthday message with the user's name
            BirthdayMessage = GenerateBirthdayMessage(user);
        }

        /// <summary>
        /// Generates a birthday message for the user.
        /// </summary>
        /// <param name="user">The user for whom the birthday message is generated.</param>
        /// <returns>The birthday message.</returns>
        private string GenerateBirthdayMessage(User user)
        {
            // TODO: Customize the birthday message based on your preferences
            return $"Happy Birthday, {user}!";
        }
    }
}
