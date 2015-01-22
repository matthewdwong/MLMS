using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.IO;
using MLMS.Common;
using MLMS.Service;
using MLMS.Objects;

namespace MLMS.Services
{
    /// <summary>
    /// Summary description for CalendarWebService
    /// </summary>
    [WebService(Namespace = "MLMS")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class CalendarWebService : System.Web.Services.WebService
    {

        [WebMethod(EnableSession = true)]
        public string GetMemberEvent(string CalendarEventID)
        {
            string Member = string.Empty;
            Member = "[";
            Member += convertMemberInfoIntoString(CalendarService.GetMemberEvent(Convert.ToInt32(CalendarEventID)));

            if (Member.EndsWith(","))
            {
                Member = Member.Substring(0, Member.Length - 1);
            }

            Member += "]";

            return Member;
        }

        [WebMethod(EnableSession = true)]
        public string GetMemberInfo(string MemberID)
        {
            string Member = string.Empty;
            Member = "[";
            Member += convertMemberInfoIntoString(MemberService.GetMember(Convert.ToInt32(MemberID)));
            Member += convertAchieveListInfoIntoString(MemberService.GetMemberAchieve(Convert.ToInt32(MemberID)));
            Member += convertMemberChecklistInfoIntoString(MemberService.GetMemberChecklist(Convert.ToInt32(MemberID)));

            if (Member.EndsWith(","))
            {
                Member = Member.Substring(0, Member.Length - 1);
            }

            Member += "]";

            return Member;
        }

        [WebMethod(EnableSession = true)]
        public string GetCalendarEventsForMember(string MemberID)
        {
            DataSet member = MemberService.GetCalendarEventsForMember(Convert.ToInt32(MemberID));
            return member.GetXml();
        }

        [WebMethod(EnableSession = true)]
        public string GetCalendarEventsByDateRange(string MemberID, string StartDate, string EndDate)
        {
            if (StartDate.Trim().Length == 0) StartDate = null;
            if (EndDate.Trim().Length == 0) EndDate = null;
            DataSet member = MemberService.GetCalendarEventsByDateRange(Convert.ToInt32(MemberID), StartDate, EndDate);
            return member.GetXml();
        }

        [WebMethod(EnableSession = true)]
        public string InsertCalendarEvent(string MemberID, string FirstName, string LastName, string Adult, string PGFirstName, string PGLastName, string DOB, string PrimaryNumber, string SecondaryNumber, string PreferredCallBackID, string HowDidYouHearID, string Confirmed, string Quality, string EmailAddres, string EventName, string StartDate, string EndDate, string EventDescription, string AllDayEvent, string EventType, string Notes)
        {
            string returnMessage = "There was an error creating the event";
            CalendarEvent Event = new CalendarEvent();
            Event.EventName = HttpUtility.HtmlEncode(EventName);
            Event.EventDescription = HttpUtility.HtmlEncode(EventDescription);
            Event.StartDate = Convert.ToDateTime(StartDate);
            Event.EndDate = Convert.ToDateTime(EndDate);
            Event.AllDayEvent = Convert.ToBoolean(AllDayEvent);
            Event.EventType = Convert.ToInt32(EventType);
            Event.MemberID = Convert.ToInt32(MemberID);
            Event.Confirmed = Convert.ToBoolean(Confirmed);

            Member member = new Member();
            member.MemberID = Convert.ToInt32(MemberID);
            member.FirstName = FirstName;
            member.LastName = LastName;
            member.Adult = Convert.ToBoolean(Adult);
            member.PGFirstName = PGFirstName;
            member.PGLastName = PGLastName;
            if (DOB.Trim().Length > 0) member.DOB = Convert.ToDateTime(DOB);
            member.PrimaryNumber = PrimaryNumber;
            member.SecondaryNumber = SecondaryNumber;
            if (PreferredCallBackID.Trim().Length > 0) member.PreferredCallBackID = Convert.ToInt32(PreferredCallBackID);
            if (HowDidYouHearID.Trim().Length > 0) member.HowDidYouHearID = Convert.ToInt32(HowDidYouHearID);
            if (Quality.Trim().Length > 0) member.Quality = Convert.ToInt32(Quality);
            member.EmailAddress = EmailAddres;

            //Only update member info if memberid, first name, and primary number are not blank. Else leave
            if (MemberID.Trim().Length > 0 && FirstName.Trim().Length > 0 && PrimaryNumber.Trim().Length > 0)
                MemberService.UpdateMember(member);

            //Inserts calendar events
            if (Event.EventName.Trim().Length > 0 && StartDate.Trim().Length > 0 && EndDate.Trim().Length > 0)
            {
                int CalendarEventID = CalendarService.InsertCalendarEvent(Event);

                if (CalendarEventID > 0)
                    returnMessage = CalendarEventID + "|Event was Created";
            }

            return returnMessage;
        }

        [WebMethod(EnableSession = true)]
        public string InsertCalendarEventFromMemberPage(string MemberID, string EventName, string StartDate, string EndDate, string EventDescription, string AllDayEvent, string FollowUp)
        {
            string returnMessage = "There was an error creating the event";

            int EventType = 3;
            if (FollowUp.Trim().Length > 0)
                if (FollowUp == "true") EventType = 2;

            CalendarEvent Event = ConvertEvent("", MemberID, EventName, EventDescription, StartDate, EndDate, EventType, false, Convert.ToBoolean(AllDayEvent));

            //Inserts calendar events
            if (Event.EventName.Trim().Length > 0 && StartDate.Trim().Length > 0 && EndDate.Trim().Length > 0)
            {
                int CalendarEventID = CalendarService.InsertCalendarEvent(Event);

                if (CalendarEventID > 0)
                    returnMessage = CalendarEventID + "|Event was Created";
            }

            return returnMessage;
        }

        [WebMethod(EnableSession = true)]
        public string UpdateCalendarEventFromMemberPage(string CalendarEventID, string EventName, string StartDate, string EndDate, string EventDescription, string AllDayEvent, string Notes, string FollowUp)
        {
            string returnMessage = "There was an error updating the event";
            int EventType = 3;
            if (FollowUp.Trim().Length > 0)
                if (FollowUp == "true") EventType = 2;

            CalendarEvent Event = ConvertEvent(CalendarEventID, "", EventName, EventDescription, StartDate, EndDate, EventType, false, Convert.ToBoolean(AllDayEvent));

            //Updates calendar event
            if (CalendarEventID.Trim().Length > 0 && Event.EventName.Trim().Length > 0 && StartDate.Trim().Length > 0 && EndDate.Trim().Length > 0)
            {
                CalendarService.UpdateCalendarEvent(Event);
                returnMessage = "Event was updated";
            }

            return returnMessage;
        }

        [WebMethod(EnableSession = true)]
        public string UpdateMemberAndCalendarEvent(string CalendarEventID, string MemberID, string FirstName, string LastName, string Adult, string PGFirstName, string PGLastName, string DOB, string PrimaryNumber, string SecondaryNumber, string PreferredCallBackID, string HowDidYouHearID, string Confirmed, string Quality, string EmailAddres, string MemberTypeID, string EventName, string StartDate, string EndDate, string EventDescription, string AllDayEvent, string EventType, string Notes)
        {
            string returnMessage = "There was an error updating the event";
            CalendarEvent Event = new CalendarEvent();
            Event.CalendarEventID = Convert.ToInt32(CalendarEventID);
            Event.EventName = HttpUtility.HtmlEncode(EventName);
            Event.EventDescription = HttpUtility.HtmlEncode(EventDescription);
            Event.StartDate = Convert.ToDateTime(StartDate);
            Event.EndDate = Convert.ToDateTime(EndDate);
            Event.Confirmed = Convert.ToBoolean(Confirmed);
            Event.EventType = Convert.ToInt32(EventType);
            Event.AllDayEvent = Convert.ToBoolean(AllDayEvent);

            Member member = new Member();
            member.MemberID = Convert.ToInt32(MemberID);
            member.FirstName = FirstName;
            member.LastName = LastName;
            member.Adult = Convert.ToBoolean(Adult);
            member.PGFirstName = PGFirstName;
            member.PGLastName = PGLastName;
            if (DOB.Trim().Length > 0) member.DOB = Convert.ToDateTime(DOB);
            member.PrimaryNumber = PrimaryNumber;
            member.SecondaryNumber = SecondaryNumber;
            if (PreferredCallBackID.Trim().Length > 0) member.PreferredCallBackID = Convert.ToInt32(PreferredCallBackID);
            if (HowDidYouHearID.Trim().Length > 0) member.HowDidYouHearID = Convert.ToInt32(HowDidYouHearID);
            if (Quality.Trim().Length > 0) member.Quality = Convert.ToInt32(Quality);
            member.EmailAddress = EmailAddres;
            member.Notes = Notes;
            member.MemberTypeID = Convert.ToInt32(MemberTypeID);

            //Only update member info if memberid, first name, and primary number are not blank. Else leave
            if (MemberID.Trim().Length > 0 && FirstName.Trim().Length > 0 && PrimaryNumber.Trim().Length > 0)
                MemberService.UpdateMember(member);

            //Updates calendar event
            if (CalendarEventID.Trim().Length > 0 && Event.EventName.Trim().Length > 0 && StartDate.Trim().Length > 0 && EndDate.Trim().Length > 0)
            {
                CalendarService.UpdateCalendarEvent(Event);
                returnMessage = "Event was updated";
            }

            return returnMessage;
        }

        [WebMethod(EnableSession = true)]
        public string InsertMember(string FirstName, string LastName, string Adult, string PGFirstName, string PGLastName, string DOB, string PrimaryNumber, string SecondaryNumber, string PreferredCallBackID, string HowDidYouHearID, string Quality, string EmailAddres, string MemberTypeID, string Notes)
        {
            string returnMessage = "There was an error updating the event";

            Member member = new Member();
            member.FirstName = FirstName;
            member.LastName = LastName;
            member.Adult = Convert.ToBoolean(Adult);
            member.PGFirstName = PGFirstName;
            member.PGLastName = PGLastName;
            if (DOB.Trim().Length > 0) member.DOB = Convert.ToDateTime(DOB);
            member.PrimaryNumber = PrimaryNumber;
            member.SecondaryNumber = SecondaryNumber;
            if (PreferredCallBackID.Trim().Length > 0) member.PreferredCallBackID = Convert.ToInt32(PreferredCallBackID);
            if (HowDidYouHearID.Trim().Length > 0) member.HowDidYouHearID = Convert.ToInt32(HowDidYouHearID);
            if (Quality.Trim().Length > 0) member.Quality = Convert.ToInt32(Quality);
            member.EmailAddress = EmailAddres;
            member.Notes = Notes;
            member.MemberTypeID = Convert.ToInt32(MemberTypeID);

            int MemberID = 0;
            //Only update member info if memberid, first name, and primary number are not blank. Else leave
            if (FirstName.Trim().Length > 0 && PrimaryNumber.Trim().Length > 0)
                MemberID = MemberService.InsertMember(member, HttpContext.Current.User.Identity.Name);

            return MemberID.ToString();
        }

        [WebMethod(EnableSession = true)]
        public string UpdateMember(string MemberID, string FirstName, string LastName, string Adult, string PGFirstName, string PGLastName, string DOB, string PrimaryNumber, string SecondaryNumber, string PreferredCallBackID, string HowDidYouHearID, string Quality, string EmailAddres, string MemberTypeID, string Notes, string AchieveList, string Checklist)
        {
            string returnMessage = "There was an error updating the event";

            Member member = new Member();
            member.MemberID = Convert.ToInt32(MemberID);
            member.FirstName = FirstName;
            member.LastName = LastName;
            member.Adult = Convert.ToBoolean(Adult);
            member.PGFirstName = PGFirstName;
            member.PGLastName = PGLastName;
            if (DOB.Trim().Length > 0) member.DOB = Convert.ToDateTime(DOB);
            member.PrimaryNumber = PrimaryNumber;
            member.SecondaryNumber = SecondaryNumber;
            if (PreferredCallBackID.Trim().Length > 0) member.PreferredCallBackID = Convert.ToInt32(PreferredCallBackID);
            if (HowDidYouHearID.Trim().Length > 0) member.HowDidYouHearID = Convert.ToInt32(HowDidYouHearID);
            if (Quality.Trim().Length > 0) member.Quality = Convert.ToInt32(Quality);
            member.EmailAddress = EmailAddres;
            member.Notes = Notes;
            member.MemberTypeID = Convert.ToInt32(MemberTypeID);

            AchieveList = AchieveList.Substring(1, AchieveList.Length - 2);
            string[] achieveArray = AchieveList.Split(',');

            member.Achieve.Clear();
            if (achieveArray[0].ToString().Trim().Length > 0)
            {
                foreach (string achieve in achieveArray)
                {
                    Achieve achieveObj = new Achieve();
                    achieveObj.AchieveID = Convert.ToInt32(achieve);
                    member.Achieve.Add(achieveObj);
                }
            }

            Checklist = Checklist.Substring(1, Checklist.Length - 2);
            string[] checklistArray = Checklist.Split(',');

            member.UserChecklist.Clear();
            member.UserChecklist = new List<UserChecklist>();
            member.UserChecklist.Add(new UserChecklist());
            member.UserChecklist[0].UserChecklistOption.Clear();
            if (checklistArray[0].ToString().Trim().Length > 0)
            {
                foreach (string checklist in checklistArray)
                {
                    string[] optionSplit = checklist.Split('|');
                    UserChecklistOption memberChecklist = new UserChecklistOption();
                    memberChecklist.UserChecklistOptionID = Convert.ToInt32(optionSplit[0]);
                    memberChecklist.CheckedOption = Convert.ToBoolean(optionSplit[1]);
                    member.UserChecklist[0].UserChecklistOption.Add(memberChecklist);
                }
            }

            //Only update member info if memberid, first name, and primary number are not blank. Else leave
            if (MemberID.Trim().Length > 0 && FirstName.Trim().Length > 0 && PrimaryNumber.Trim().Length > 0)
                MemberService.UpdateMember(member);

            return returnMessage;
        }

        [WebMethod(EnableSession = true)]
        public string UpdateCalendarEvent(string CalendarEventID, string StartDate, string EndDate)
        {
            string returnMessage = "There was an error updating the event";
            CalendarEvent Event = CalendarService.GetEvent(Convert.ToInt32(CalendarEventID));
            Event.StartDate = DateTime.ParseExact(StartDate, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            Event.EndDate = DateTime.ParseExact(EndDate, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

            //Updates calendar event
            if (CalendarEventID.Trim().Length > 0 && Event.EventName.Trim().Length > 0 && StartDate.Trim().Length > 0 && EndDate.Trim().Length > 0)
            {
                CalendarService.UpdateCalendarEvent(Event);
                returnMessage = "Event was updated";
            }

            return returnMessage;
        }

        [WebMethod(EnableSession = true)]
        public string DeleteCalendarEvent(string CalendarEventID)
        {
            string returnMessage = "There was an error deleting the event";

            //Updates calendar event
            if (CalendarEventID.Trim().Length > 0)
            {
                CalendarService.DeleteCalendarEvent(Convert.ToInt32(CalendarEventID));
                returnMessage = "Event was deleted";
            }

            return returnMessage;
        }

        [WebMethod(EnableSession = true)]
        public string InactivateLead(string MemberID, string Objection, string AdditionalNotes)
        {
            string returnMessage = "There was an error updating the lead";

            int? ObjectionID = null;
            if (Objection != "null") ObjectionID = Convert.ToInt32(Objection);

            //Inactivate
            if (MemberID.Trim().Length > 0)
            {
                MemberService.InactivateLead(Convert.ToInt32(MemberID), ObjectionID, AdditionalNotes, false);
                returnMessage = "Lead is now inactive";
            }

            return returnMessage;
        }

        //Gets Lead info and intro meeting date
        [WebMethod(EnableSession = true)]
        public string GetLeadInfo(string MemberID)
        {
            string Member = string.Empty;
            Member = "[";
            Member += convertMemberInfoIntoString(MemberService.GetLead(Convert.ToInt32(MemberID)));

            if (Member.EndsWith(","))
            {
                Member = Member.Substring(0, Member.Length - 1);
            }

            Member += "]";

            return Member;
        }

        private String convertMemberInfoIntoString(Member Member)
        {
            string SetStartDate = string.Empty;
            if (Member.CalendarEvent.StartDate.Month + "/" + Member.CalendarEvent.StartDate.Day + "/" + Member.CalendarEvent.StartDate.Year != "1/1/1")
                SetStartDate = Member.CalendarEvent.StartDate.Month + "/" + Member.CalendarEvent.StartDate.Day + "/" + Member.CalendarEvent.StartDate.Year;

            string SetEndDate = string.Empty;
            if (Member.CalendarEvent.StartDate.Month + "/" + Member.CalendarEvent.StartDate.Day + "/" + Member.CalendarEvent.StartDate.Year != "1/1/1")
                SetEndDate = Member.CalendarEvent.EndDate.Month + "/" + Member.CalendarEvent.EndDate.Day + "/" + Member.CalendarEvent.EndDate.Year;

            string DOB = string.Empty;
            if (Member.DOB.ToString().Trim().Length > 0)
                DOB = Member.DOB.Value.Month + "/" + Member.DOB.Value.Day + "/" + Member.DOB.Value.Year;

            return "{" +
                      "\"MemberID\": \"" + Member.MemberID + "\"," +
                      "\"FirstName\": \"" + Member.FirstName + "\"," +
                      "\"LastName\": \"" + Member.LastName + "\"," +
                      "\"Adult\": \"" + Member.Adult + "\"," +
                      "\"PGFirstName\": \"" + Member.PGFirstName + "\"," +
                      "\"PGLastName\": \"" + Member.PGLastName + "\"," +
                      "\"DOB\": \"" + DOB + "\"," +
                      "\"PrimaryNumber\": \"" + Member.PrimaryNumber + "\"," +
                      "\"SecondaryNumber\":  \"" + Member.SecondaryNumber + "\"," +
                      "\"PrefCallBack\": \"" + Member.PreferredCallBackID + "\"," +
                      "\"HowDidYouHear\": \"" + Member.HowDidYouHearID + "\"," +
                      "\"EmailAddress\": \"" + Member.EmailAddress + "\"," +
                      "\"MemberTypeID\": \"" + Member.MemberTypeID + "\"," +
                      "\"Quality\": \"" + Member.Quality + "\"," +
                      "\"Confirmed\": \"" + Member.CalendarEvent.Confirmed + "\"," +
                      "\"EventDateStart\": \"" + SetStartDate + "\"," +
                      "\"EventDateEnd\": \"" + SetEndDate + "\"," +
                      "\"EventName\": \"" + HttpUtility.HtmlDecode(Member.CalendarEvent.EventName) + "\"," +
                      "\"EventDateStartTime\": \"" + Member.CalendarEvent.StartDate.TimeOfDay + "\"," +
                      "\"EventDateEndTime\": \"" + Member.CalendarEvent.EndDate.TimeOfDay + "\"," +
                      "\"AllDayEvent\": \"" + Member.CalendarEvent.AllDayEvent + "\"," +
                      "\"EventType\": \"" + Member.CalendarEvent.EventType + "\"," +
                      "\"EventDescription\": \"" + HttpUtility.HtmlDecode(Member.CalendarEvent.EventDescription) + "\"," +
                      "\"Notes\": \"" + Member.Notes + "\"" +
                      "},";
        }

        private String convertAchieveListInfoIntoString(List<Achieve> AchieveList)
        {
            string returnList = "[";
            foreach (Achieve Achieve in AchieveList)
            {
                returnList = returnList + "{\"AchieveID\":\"" + Achieve.AchieveID + "\", \"AchieveName\": \"" + Achieve.AchieveName + "\"},";
            }

            if (returnList.EndsWith(","))
            {
                returnList = returnList.Substring(0, returnList.Length - 1);
            }

            returnList = returnList + "],";

            if (returnList.Length == 0) returnList = "[{}],";

            return returnList;
        }

        private String convertMemberChecklistInfoIntoString(DataTable Memberlist)
        {
            string returnList = "[";
            foreach (DataRow option in Memberlist.Rows)
            {
                returnList = returnList + "{\"MemberChecklistOptionID\":\"" + option["MemberChecklistOptionID"] + "\", \"UserCheckListOptionID\": \"" + option["UserCheckListOptionID"] + "\"},";
            }

            if (returnList.EndsWith(","))
            {
                returnList = returnList.Substring(0, returnList.Length - 1);
            }

            returnList = returnList + "],";

            if (returnList.Length == 0) returnList = "[{}],";

            return returnList;
        }

        private long ConvertToTimestamp(DateTime value)
        {
            long epoch = (value.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
            return epoch;

        }

        private CalendarEvent ConvertEvent(string CalendarEventID, string MemberID, string EventName, string EventDescription, string StartDate, string EndDate, int EventType, bool Confirmed, bool AllDayEvent)
        {
            CalendarEvent Event = new CalendarEvent();
            if (CalendarEventID.Trim().Length > 0) Event.CalendarEventID = Convert.ToInt32(CalendarEventID);
            Event.EventName = HttpUtility.HtmlEncode(EventName);
            Event.EventDescription = HttpUtility.HtmlEncode(EventDescription);
            Event.StartDate = Convert.ToDateTime(StartDate);
            Event.EndDate = Convert.ToDateTime(EndDate);
            Event.Confirmed = Confirmed;
            Event.EventType = EventType;
            Event.AllDayEvent = AllDayEvent;
            if (MemberID.Trim().Length > 0) Event.MemberID = Convert.ToInt32(MemberID);

            return Event;
        }
    }
}
