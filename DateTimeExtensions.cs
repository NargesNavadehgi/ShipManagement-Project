using System;

namespace ShipManagement.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToPersianDate(this DateTime date)
        {
            try
            {
                var pc = new System.Globalization.PersianCalendar();
                return $"{pc.GetYear(date)}/{pc.GetMonth(date):00}/{pc.GetDayOfMonth(date):00}";
            }
            catch
            {
                return date.ToString("yyyy/MM/dd");
            }
        }

        // این متد برای DateTime? (Nullable) ضروری است
        public static string ToPersianDate(this DateTime? date)
        {
            if (!date.HasValue) return "-";
            return date.Value.ToPersianDate();
        }
    }
}