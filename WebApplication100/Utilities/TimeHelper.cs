using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEshop
{
    public static class TimeHelper
    {
        private static TimeSpan PassedTime(DateTime dateTime)
        {
            return DateTime.Now - dateTime;
        }

        public static TimeSpan RemainTime(DateTime dateTime)
        {
            return dateTime - DateTime.Now;
        }

        public static string Detail(DateTime date)
        {
            var dateTime = PassedTime(date);
            if (dateTime.Days >= 1)
            {
                if (dateTime.Days == 1)
                {
                    return "دیروز";
                }
                return date.ToString("yyyy/MM/dd");
            }
            else if(dateTime.Hours >= 1)
            {
                return $"{dateTime.Hours} ساعت پیش";
            }
            else
            {
                return $"{dateTime.Minutes} دقیقه پیش";
            }
        }
    }
}