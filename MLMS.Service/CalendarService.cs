using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MLMS.Data;
using MLMS.Objects;

namespace MLMS.Service
{
    public class CalendarService
    {
        /// <summary>
        /// Gets time ranges for the calendar
        /// </summary>
        public static DataTable GetTimeRange()
        {
            return CalendarDataAccess.GetTimeRange();
        }

        /// <summary>
        /// Gets events
        /// </summary>
        public static CalendarEvent GetEvent(int CalendarEventID)
        {
            return CalendarDataAccess.GetEvent(CalendarEventID);
        }

        /// <summary>
        /// Gets events
        /// </summary>
        public static List<CalendarEvent> GetEvents(string StartDate, string EndDate, string UserName)
        {
            return CalendarDataAccess.GetEvents(StartDate, EndDate, UserName);
        }

        /// <summary>
        /// Gets events
        /// </summary>
        public static Member GetMemberEvent(int CalendarEventID)
        {
            return CalendarDataAccess.GetMemberEvent(CalendarEventID);
        }

        public static DataTable GetMeetingType()
        {
            return CalendarDataAccess.GetMeetingType();
        }

        public static int InsertCalendarEvent(CalendarEvent Event)
        {
            return CalendarDataAccess.InsertCalendarEvent(Event);
        }

        public static void UpdateCalendarEvent(CalendarEvent Event)
        {
            CalendarDataAccess.UpdateCalendarEvent(Event);
        }

        public static void DeleteCalendarEvent(int CalendarEventID)
        {
            CalendarDataAccess.DeleteCalendarEvent(CalendarEventID);
        }
    }
}
