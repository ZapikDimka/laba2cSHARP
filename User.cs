using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfApp1
{
    /// <summary>
    /// Class representing a user.
    /// </summary>
    public class User : BaseBindable
    {
        // Fields for storing user data
        private DateTime _birthDate;
        private int _age;
        private bool _isBirthdayToday = false;
        private ChineseZodiac _chineseZodiac;
        private WesternZodiac _westernZodiac;
        private string _zodiacInfo;
        private string _firstName;
        private string _lastName;
        private string _emailAddress;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public User()
        {
            // Initializes all properties to default values
        }

        // User properties with additional documentation:

        /// <summary>
        /// User's first name.
        /// </summary>
        public string FirstName
        {
            get => _firstName;
            set => SetProperty(ref _firstName, value);
        }

        /// <summary>
        /// User's last name.
        /// </summary>
        public string LastName
        {
            get => _lastName;
            set => SetProperty(ref _lastName, value);
        }

        /// <summary>
        /// User's email address.
        /// </summary>
        public string EmailAddress
        {
            get => _emailAddress;
            set => SetProperty(ref _emailAddress, value);
        }

        /// <summary>
        /// Information about the user's zodiac sign.
        /// </summary>
        public string ZodiacInfo
        {
            get => _zodiacInfo;
            set => UpdateProperty(ref _zodiacInfo, value, nameof(ZodiacInfo));
        }

        /// <summary>
        /// Checks if the user is an adult.
        /// </summary>
        public bool IsAdult => Age >= 18;

        /// <summary>
        /// User's sun sign.
        /// </summary>
        public string SunSign => WesternZodiac.ToString();

        /// <summary>
        /// User's Chinese zodiac sign.
        /// </summary>
        public string ChineseSign => ChineseZodiac.ToString();

        /// <summary>
        /// Checks if today is the user's birthday.
        /// </summary>
        public bool IsBirthday => IsBirthdayToday;

        // Constructors with additional documentation:

        /// <summary>
        /// Constructor accepting all user data.
        /// </summary>
        public User(string firstName, string lastName, string emailAddress, DateTime birthDate)
        {
            FirstName = firstName;
            LastName = lastName;
            EmailAddress = emailAddress;
            BirthDate = birthDate;
        }

        /// <summary>
        /// Constructor accepting user's name and email address.
        /// </summary>
        public User(string firstName, string lastName, string emailAddress)
        {
            FirstName = firstName;
            LastName = lastName;
            EmailAddress = emailAddress;
            BirthDate = DateTime.Today;
        }

        /// <summary>
        /// Constructor accepting user's name and birth date.
        /// </summary>
        public User(string firstName, string lastName, DateTime birthDate)
        {
            FirstName = firstName;
            LastName = lastName;
            EmailAddress = string.Empty;
            BirthDate = birthDate;
        }

        /// <summary>
        /// User's birth date.
        /// </summary>
        public DateTime BirthDate
        {
            get => _birthDate;
            set
            {
                if (UpdateProperty(ref _birthDate, value, nameof(BirthDate)))
                {
                    Age = CalculateAge(BirthDate);
                    IsBirthdayToday = CalculateIsBirthdayToday(BirthDate);
                    ChineseZodiac = BirthDate.GetChineseZodiacSign();
                    WesternZodiac = BirthDate.GetWesternZodiacSign();
                    OnPropertyChanged(nameof(FormattedAge));
                    UpdateZodiacInfo();
                }
            }
        }

        /// <summary>
        /// User's age.
        /// </summary>
        public int Age
        {
            get => _age;
            private set => UpdateProperty(ref _age, value, nameof(Age));
        }

        /// <summary>
        /// Checks if today is the user's birthday.
        /// </summary>
        public bool IsBirthdayToday
        {
            get => _isBirthdayToday;
            private set => UpdateProperty(ref _isBirthdayToday, value, nameof(IsBirthdayToday));
        }

        /// <summary>
        /// User's Chinese zodiac sign.
        /// </summary>
        public ChineseZodiac ChineseZodiac
        {
            get => _chineseZodiac;
            private set => UpdateProperty(ref _chineseZodiac, value, nameof(ChineseZodiac));
        }

        /// <summary>
        /// User's Western zodiac sign.
        /// </summary>
        public WesternZodiac WesternZodiac
        {
            get => _westernZodiac;
            private set => UpdateProperty(ref _westernZodiac, value, nameof(WesternZodiac));
        }

        /// <summary>
        /// Formatted user age.
        /// </summary>
        public string FormattedAge
        {
            get
            {
                var today = DateTime.Today;
                var age = today.Year - BirthDate.Year;

                if (BirthDate.Date > today.AddYears(-age))
                {
                    age--;
                }

                var lastBirthday = BirthDate.AddYears(age);
                var days = (today - lastBirthday).Days;

                var daysInMonth = new int[]
                {
                    31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31
                };

                var months = 0;

                if (today.Month < BirthDate.Month || (today.Month == BirthDate.Month && today.Day < BirthDate.Day))
                {
                    months = 12 - (BirthDate.Month - today.Month);
                }
                else
                {
                    months = today.Month - BirthDate.Month;
                }

                if (today.Day < BirthDate.Day)
                {
                    months--;
                    var lastMonth = today.AddMonths(-1);
                    var daysInLastMonth = daysInMonth[lastMonth.Month - 1];
                    days = daysInLastMonth - (BirthDate.Day - today.Day) + 1;
                }

                var years = age;

                return $"{years} years,\n{months} months,\n{days} days";
            }
        }

        /// <summary>
        /// Calculates the user's age based on the birth date.
        /// </summary>
        private static int CalculateAge(DateTime birthDate)
        {
            var today = DateTime.Today;
            int age = today.Year - birthDate.Year;

            if (birthDate.Date > today.AddYears(-age))
            {
                age--;
            }

            return age;
        }

        /// <summary>
        /// Checks if today is the user's birthday.
        /// </summary>
        public static bool CalculateIsBirthdayToday(DateTime birthDate)
        {
            var today = DateTime.Today;

            if (birthDate.Month == today.Month && birthDate.Day == today.Day)
            {
                if (birthDate.Year <= today.Year)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Updates the zodiac information of the user.
        /// </summary>
        public void UpdateZodiacInfo()
        {
            ZodiacInfo = GetZodiacInfo(WesternZodiac.ToString(), ChineseZodiac.ToString());
        }

        /// <summary>
        /// Retrieves and formats zodiac information based on Western and Chinese zodiac signs.
        /// </summary>
        public static string GetZodiacInfo(string westernZodiac, string chineseZodiac)
        {
            string westernInfo = ZodiacUtils.GetWesternZodiacInfo(westernZodiac);
            string chineseInfo = ZodiacUtils.GetChineseZodiacInfo(chineseZodiac);

            return $"{westernInfo}\n{chineseInfo}";
        }

        /// <summary>
        /// Method to set property value and raise property changed event if value has changed.
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
