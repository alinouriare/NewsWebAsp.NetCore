using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace SecurityNews.PublicClass
{
    public class DateAndTimeShamsi
    {

       
        

        public static string DateShamsi()
        {
            var currentDate = DateTime.Now;
            PersianCalendar pcCalender = new PersianCalendar();
            int year = pcCalender.GetYear(currentDate);
            int month = pcCalender.GetMonth(currentDate);
            int day = pcCalender.GetDayOfMonth(currentDate);

            //string shamsiDate = string.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(day + "/" + month + "/" + year));
            string Shamsidate = string.Format("{0:0000}/{1:00}/{2:00}", year, month, day);
            return Shamsidate;
        }

        public static string MyTime()
        {
            var currentDate = DateTime.Now;
            PersianCalendar pcCalender = new PersianCalendar();
            int year = pcCalender.GetYear(currentDate);
            int month = pcCalender.GetMonth(currentDate);
            int day = pcCalender.GetDayOfMonth(currentDate);

            string NowTime = string.Format("{0:hh:mm}", Convert.ToDateTime(pcCalender.GetHour(currentDate) + ":" + pcCalender.GetMinute(currentDate)));
            return NowTime;
        }
    }
}
