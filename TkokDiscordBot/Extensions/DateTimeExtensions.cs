using System;

namespace TkokDiscordBot.Extensions
{
    public static class DateTimeExtensions
    {
        public static TimeSpan ElapsedFromNow(this DateTime dateTime)
        {
            return dateTime - DateTime.Now;
        }
    }
}
