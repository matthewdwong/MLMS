using System;
using System.Collections.Generic;
using System.Data;
using MLMS.Objects;
using MLMS.Data;

namespace MLMS.Service
{
    public static class MemberService
    {
        #region Insert Methods
        /// <summary>
        /// Inserts lead
        /// </summary>
        public static int InsertMember(Member Lead, string UserName)
        {
            return MemberDataAccess.InsertMember(Lead, UserName);
        }

        /// <summary>
        /// Inserts call log for a member
        /// </summary>
        public static void InsertCallLog(int LeadID, DateTime StartDate, string Notes)
        {
            MemberDataAccess.InsertCallLog(LeadID, StartDate, Notes);
        }
        #endregion

        #region Get Methods
        /// <summary>
        /// Grabs preferred call back time of days
        /// </summary>
        public static DataTable GetPreferredCallBack()
        {
            return MemberDataAccess.GetPreferredCallBack();
        }

        /// <summary>
        /// Grabs how did you hear about 
        /// </summary>
        public static DataTable GetHearAbout(string UserName)
        {
            return MemberDataAccess.GetHearAbout(UserName);
        }

        /// <summary>
        /// Grabs users achieve list 
        /// </summary>
        public static DataTable GetUserAchieveList(string UserName)
        {
            return MemberDataAccess.GetUserAchieveList(UserName);
        }

        /// <summary>
        /// Grabs users achieve list 
        /// </summary>
        public static DataTable GetObjections(string UserName)
        {
            return MemberDataAccess.GetObjections(UserName);
        }

        /// <summary>
        /// Grabs preferred call back time of days
        /// </summary>
        public static DataTable GetUserChecklistOptions(int UserChecklistID)
        {
            return MemberDataAccess.GetUserCheckListOptions(UserChecklistID);
        }

        /// <summary>
        /// Gets a list of leads based on search parameters
        /// </summary>
        public static List<Member> GetFilteredLeads(string UserName, string FirstName, string LastName, string PhoneNumber, string PreferredCallBack, int LeadType)
        {
            return MemberDataAccess.GetFilteredLeads(UserName, FirstName, LastName, PhoneNumber, PreferredCallBack, LeadType);
        }

        /// <summary>
        /// Gets a list of ppl to call based on user
        /// </summary>
        public static List<Member> GetLeadsToCall(string UserName)
        {
            return MemberDataAccess.GetLeadsToCall(UserName);
        }

        /// <summary>
        /// Gets a list of ppl to call based on user
        /// </summary>
        public static List<Member> GetUpcomingMeetingsToConfirm(string UserName)
        {
            return MemberDataAccess.GetUpcomingMeetingsToConfirm(UserName);
        }

        /// <summary>
        /// Gets a list of ppl to call based on user
        /// </summary>
        public static List<Member> GetFollowUps(string UserName)
        {
            return MemberDataAccess.GetFollowUps(UserName);
        }

        /// <summary>
        /// Gets a list of ppl to call based on user
        /// </summary>
        public static Member GetLead(int MemberID)
        {
            return MemberDataAccess.GetLead(MemberID);
        }

        /// <summary>
        /// Gets a list of members by user
        /// </summary>
        public static List<Member> GetMembersByUser(string UserName, int MemberType)
        {
            return MemberDataAccess.GetMembersByUser(UserName, MemberType);
        }

        /// <summary>
        /// Gets a specific member by id
        /// </summary>
        public static Member GetMember(int MemberID)
        {
            return MemberDataAccess.GetMember(MemberID);
        }

        /// <summary>
        /// Gets a member Calendar events
        /// </summary>
        public static DataSet GetCalendarEventsForMember(int MemberID)
        {
            return MemberDataAccess.GetCalendarEventsForMember(MemberID);
        }

        /// <summary>
        /// Gets a member Calendar events
        /// </summary>
        public static DataSet GetCalendarEventsByDateRange(int MemberID, string StartDate, string EndDate)
        {
            return MemberDataAccess.GetCalendarEventsByDateRange(MemberID, StartDate, EndDate);
        }

        /// <summary>
        /// Gets the call log of a specific member
        /// </summary>
        public static DataTable GetCallLog(int MemberID)
        {
            return MemberDataAccess.GetCallLog(MemberID);
        }

        /// <summary>
        /// Gets a list of what a member wants to achieve
        /// </summary>
        public static List<Achieve> GetMemberAchieve(int MemberID)
        {
            return MemberDataAccess.GetMemberAchieve(MemberID);
        }

        /// <summary>
        /// Gets the members checklist
        /// </summary>
        public static DataTable GetMemberChecklist(int MemberID)
        {
            return MemberDataAccess.GetMemberChecklist(MemberID);
        }
        #endregion

        #region Update Methods
        /// <summary>
        /// Update lead
        /// </summary>
        public static void UpdateMember(Member Lead)
        {
            MemberDataAccess.UpdateMember(Lead);
        }

        /// <summary>
        /// Update lead
        /// </summary>
        public static void InactivateLead(int MemberID, int? Objection, string AdditionalNotes, bool IsActive)
        {
            MemberDataAccess.InactivateLead(MemberID, Objection, AdditionalNotes, IsActive);
        }

        /// <summary>
        /// Update Call Log
        /// </summary>
        public static void UpdateCallLog(int MemberCallLog, string Notes)
        {
            MemberDataAccess.UpdateCallLog(MemberCallLog, Notes);
        }

        /// <summary>
        /// Update Call Log
        /// </summary>
        public static void UpdateUserCheckListOption(int MemberID, List<UserChecklistOption> UserCheckListOptions)
        {
            MemberDataAccess.UpdateUserCheckListOption(MemberID, UserCheckListOptions);
        }
        #endregion

        #region DeleteMethods
        public static bool DeleteMember(int MemberID)
        {
            return MemberDataAccess.DeleteMember(MemberID);
        }

        public static void DeleteCallHistory(int MemberCallLog)
        {
            MemberDataAccess.DeleteCallHistory(MemberCallLog);
        }
        #endregion
    }
}
