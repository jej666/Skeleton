using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Skeleton.Common
{
    public static class DateTimeExtensions
    {
        private static readonly int[] MoveByDays = {6, 7, 8, 9, 10, 4, 5};

        public static DateTime FirstMonthDay(this DateTime value)
        {
            return value.AddDays(-(value.Day - 1));
        }

        [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "WeekDay")]
        public static DateTime FirstWeekDay(this DateTime value)
        {
            var day = value.DayOfWeek == DayOfWeek.Sunday ? 7 : (int) value.DayOfWeek;

            return value.AddDays(-(day - 1));
        }

        public static void ForEachDay(this DateTime value, DateTime until, Action<DateTime> onNext)
        {
            onNext.ThrowIfNull(() => onNext);

            if (value.IsAfter(until))
                throw new InvalidOperationException("Source date must be anterior to until date");
            var d = value;

            while (!(d > until))
            {
                onNext(d);
                d = d.AddDays(1);
            }
        }

        public static double GetBusinessDaysUntil(this DateTime from, DateTime until)
        {
            var calcBusinessDays = 1 + ((until - from).TotalDays*5 -
                                        (from.DayOfWeek - until.DayOfWeek)*2)/7;

            if ((int) until.DayOfWeek == 6)
                calcBusinessDays--;

            if (until.DayOfWeek == 0)
                calcBusinessDays--;

            return calcBusinessDays;
        }

        public static IEnumerable<DateTime> GetDaysUntil(this DateTime from, DateTime until)
        {
            var list = new List<DateTime>();

            for (var d = from.Date; d <= until.Date; d = d.AddDays(1))
                list.Add(d);

            return list;
        }

        public static DateTime GetNext(this DateTime value, DayOfWeek dayOfWeek)
        {
            var daysToAdd = value.DayOfWeek < dayOfWeek
                ? dayOfWeek - value.DayOfWeek
                : 7 - (int) value.DayOfWeek + (int) dayOfWeek;

            return value.AddDays(daysToAdd);
        }

        public static bool IsAfter(this DateTime value, DateTime other)
        {
            return value.CompareTo(other) > 0;
        }

        public static bool IsBefore(this DateTime value, DateTime other)
        {
            return value.CompareTo(other) < 0;
        }

        public static bool IsDateBetween(this DateTime inBetweenDate, DateTime startDate, DateTime endDate)
        {
            return (inBetweenDate >= startDate) && (inBetweenDate <= endDate);
        }

        public static bool IsDateEqual(this DateTime value, DateTime dateToCompare)
        {
            return value.Date == dateToCompare.Date;
        }

        public static bool IsWeekend(this DateTime value)
        {
            return (value.DayOfWeek == DayOfWeek.Saturday) || (value.DayOfWeek == DayOfWeek.Sunday);
        }

        public static DateTime LastMonthDay(this DateTime value)
        {
            return new DateTime(value.Year, value.Month, DateTime.DaysInMonth(value.Year, value.Month));
        }

        [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "WeekDay")]
        public static DateTime LastWeekDay(this DateTime value)
        {
            var day = value.DayOfWeek == DayOfWeek.Sunday ? 7 : (int) value.DayOfWeek;

            return value.AddDays(7 - day);
        }

        [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "ToDay")]
        public static string ToDayName(this DateTime value)
        {
            return DateTimeFormatInfo.CurrentInfo != null
                ? DateTimeFormatInfo.CurrentInfo.GetAbbreviatedDayName(value.DayOfWeek)
                : null;
        }

        public static string ToShortDate(this DateTime value)
        {
            return value.ToString("dd/MM/yyyy", CultureInfo.GetCultureInfo("fr"));
        }

        public static int WeekNumber(this DateTime date)
        {
            var startOfYear = new DateTime(date.Year, 1, 1);
            var endOfYear = new DateTime(date.Year, 12, 31);

            var numberDays = date.Subtract(startOfYear).Days +
                             MoveByDays[(int) startOfYear.DayOfWeek];

            var weekNumber = numberDays/7;

            switch (weekNumber)
            {
                case 0:
                    weekNumber = WeekNumber(startOfYear.AddDays(-1));
                    break;

                case 53:
                    if (endOfYear.DayOfWeek < DayOfWeek.Thursday)
                        weekNumber = 1;
                    break;
            }
            return weekNumber;
        }
    }
}