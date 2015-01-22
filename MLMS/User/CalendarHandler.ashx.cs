using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.SessionState;
using System.Collections.Generic;
using MLMS.Common;
using MLMS.Service;
using MLMS.Objects;

namespace MLMS.User
{
    /// <summary>
    /// Summary description for CalendarHandler
    /// </summary>
    public class CalendarHandler : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";

            DateTime start = new DateTime(1970, 1, 1);
            DateTime end = new DateTime(1970, 1, 1);

            start = start.AddSeconds(double.Parse(context.Request.QueryString["start"]));
            end = end.AddSeconds(double.Parse(context.Request.QueryString["end"]));

            String result = String.Empty;

            result += "[";

            List<int> idList = new List<int>();
            foreach (CalendarEvent cevent in CalendarService.GetEvents(start.ToString(), end.ToString(), HttpContext.Current.User.Identity.Name))
            {
                result += convertCalendarEventIntoString(cevent);
                idList.Add(cevent.CalendarEventID);
            }

            if (result.EndsWith(","))
            {
                result = result.Substring(0, result.Length - 1);
            }

            result += "]";

            //store list of event ids in Session, so that it can be accessed in web methods
            context.Session["idList"] = idList;

            context.Response.Write(result);
        }

        private String convertCalendarEventIntoString(CalendarEvent cevent)
        {
            string eventColor = string.Empty;
            switch (cevent.EventType)
            {
                case (int) EventType.Intro:
                    if (cevent.Confirmed == true)
                        eventColor = "eventColor1";
                    else
                        eventColor = "eventColor2";
                    break;
                case (int) EventType.FollowUp:
                    eventColor = "eventColor3";
                    break;
            }

            return "{" +
                      "\"id\": \"" + cevent.CalendarEventID + "\"," +
                      "\"title\": \"" + HttpContext.Current.Server.HtmlDecode(cevent.EventName) + "\"," +
                      "\"start\":  \"" + ConvertToTimestamp(cevent.StartDate).ToString() + "\"," +
                      "\"end\": \"" + ConvertToTimestamp(cevent.EndDate).ToString() + "\"," +
                      "\"allDay\": " + cevent.AllDayEvent.ToString().ToLower() +"," +
                      "\"description\": \"" + HttpContext.Current.Server.HtmlDecode(cevent.EventDescription) + "\"," +
                      "\"className\": \"" + eventColor + "\"" +
                      "},";
        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        private long ConvertToTimestamp(DateTime value)
        {
            long epoch = (value.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
            return epoch;
        }
    }
}