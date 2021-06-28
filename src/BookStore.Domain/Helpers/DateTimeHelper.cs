using System;

namespace BookStore.Domain.Helpers
{
    public static class DateTimeHelper
    {
        public static string ToUnixEpochDate(this DateTime dateTime)
        {
            return Math.Round((dateTime.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds).ToString();
        }
    }
}
