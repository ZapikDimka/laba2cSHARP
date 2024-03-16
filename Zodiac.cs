using System;

namespace WpfApp1
{
    /// <summary>
    /// Represents the Western Zodiac signs.
    /// </summary>
    public enum WesternZodiac
    {
        Capricorn = 0,
        Aquarius,
        Pisces,
        Aries,
        Taurus,
        Gemini,
        Cancer,
        Leo,
        Virgo,
        Libra,
        Scorpio,
        Sagittarius
    }

    /// <summary>
    /// Represents the Chinese Zodiac signs.
    /// </summary>
    public enum ChineseZodiac
    {
        Rat = 0,
        Ox,
        Tiger,
        Rabbit,
        Dragon,
        Snake,
        Horse,
        Goat,
        Monkey,
        Rooster,
        Dog,
        Pig
    }

    /// <summary>
    /// Provides extension methods for working with Zodiac signs based on a given date.
    /// </summary>
    public static class ZodiacUtils
    {
        /// <summary>
        /// Gets the Western Zodiac sign based on the provided date.
        /// </summary>
        /// <param name="dateTime">The date to determine the Western Zodiac sign for.</param>
        /// <returns>The Western Zodiac sign for the given date.</returns>
        public static WesternZodiac GetWesternZodiacSign(this DateTime dateTime)
        {
            int[] pivotDays = { 21, 20, 22, 21, 22, 22, 24, 24, 24, 24, 23, 23 };

            int monthIndex = dateTime.Month - 1;
            int offset = dateTime.Day < pivotDays[monthIndex] ? 0 : 1;
            int index = (monthIndex + offset) % 12;

            return (WesternZodiac)index;
        }

        /// <summary>
        /// Gets the Chinese Zodiac sign based on the provided date.
        /// </summary>
        /// <param name="dateTime">The date to determine the Chinese Zodiac sign for.</param>
        /// <returns>The Chinese Zodiac sign for the given date.</returns>
        public static ChineseZodiac GetChineseZodiacSign(this DateTime dateTime)
        {
            int index = (dateTime.Year - 4) % 12;

            return (ChineseZodiac)index;
        }

        /// <summary>
        /// Gets information about the Western zodiac sign.
        /// </summary>
        /// <param name="zodiac">The Western zodiac sign.</param>
        /// <returns>Information about the Western zodiac sign.</returns>
        public static string GetWesternZodiacInfo(string zodiac)
        {
            switch (zodiac.ToLower())
            {
                case "aries":
                    return "Aries are known for their courage and determination.";
                case "taurus":
                    return "Taurus individuals are reliable and practical.";
                case "gemini":
                    return "Geminis are known for their adaptability and communication skills.";
                case "cancer":
                    return "Cancer individuals are nurturing and empathetic.";
                case "leo":
                    return "Leos are often confident and generous.";
                case "virgo":
                    return "Virgos are detail-oriented and practical.";
                case "libra":
                    return "Libras value harmony and cooperation.";
                case "scorpio":
                    return "Scorpios are passionate and resourceful.";
                case "sagittarius":
                    return "Sagittarians are adventurous and optimistic.";
                case "capricorn":
                    return "Capricorns are disciplined and responsible.";
                case "aquarius":
                    return "Aquarians are innovative and open-minded.";
                case "pisces":
                    return "Pisces individuals are compassionate and artistic.";
                default:
                    return "Unknown Western Zodiac sign.";
            }
        }

        /// <summary>
        /// Gets information about the Chinese zodiac sign.
        /// </summary>
        /// <param name="zodiac">The Chinese zodiac sign.</param>
        /// <returns>Information about the Chinese zodiac sign.</returns>
        public static string GetChineseZodiacInfo(string chineseZodiac)
        {
            switch (chineseZodiac.ToLower())
            {
                case "rat":
                    return "Rats are quick-witted and resourceful.";
                case "ox":
                    return "Oxen are diligent and reliable.";
                case "tiger":
                    return "Tigers are brave and confident.";
                case "rabbit":
                    return "Rabbits are gentle and compassionate.";
                case "dragon":
                    return "Dragons are ambitious and passionate.";
                case "snake":
                    return "Snakes are wise and intuitive.";
                case "horse":
                    return "Horses are energetic and free-spirited.";
                case "goat":
                    return "Goats are kind-hearted and artistic.";
                case "monkey":
                    return "Monkeys are clever and playful.";
                case "rooster":
                    return "Roosters are confident and honest.";
                case "dog":
                    return "Dogs are loyal and responsible.";
                case "pig":
                    return "Pigs are diligent and compassionate.";
                default:
                    return "Unknown Chinese Zodiac sign.";
            }
        }
    }
}