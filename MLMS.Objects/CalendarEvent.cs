using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MLMS.Objects
{
    [Serializable]
    public class CalendarEvent
    {
        public int CalendarEventID { get; set; }
        public string EventName { get; set; }
        public string EventDescription { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool AllDayEvent { get; set; }
        public int? PriorityID { get; set; }
        public int? EventType { get; set; }
        public  bool? Confirmed { get; set; }
        public int MemberID { get; set; }


    }
}
