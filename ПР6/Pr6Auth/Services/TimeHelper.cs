using System;

namespace Pr6Auth.Services
{
    public static class TimeHelper
    {
        public static string GetTimeOfDay(DateTime time)
        {
            int hour = time.Hour;
            if (hour >= 10 && hour <= 12) return "Утро";
            if (hour >= 13 && hour <= 17) return "День";
            if (hour >= 18 && hour <= 19) return "Вечер";
            return "Ночь";
        }

        public static bool IsWorkingTime(DateTime time)
        {
            return time.Hour >= 10 && time.Hour <= 19;
        }

        public static string GetGreeting(string lastName, string firstName, string middleName)
        {
            string fullName = lastName + " " + firstName;
            if (!string.IsNullOrWhiteSpace(middleName))
                fullName += " " + middleName;
            string timeOfDay = GetTimeOfDay(DateTime.Now);
            return $"{timeOfDay}, {fullName}!";
        }
    }
}