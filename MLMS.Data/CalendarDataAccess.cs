using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using MLMS.Objects;
using MLMS.Common;

namespace MLMS.Data
{
    public class CalendarDataAccess
    {
        /// <summary>
        /// Gets time ranges for the calendar pop up
        /// </summary>
        public static DataTable GetTimeRange()
        {
            DataTable timeRange = new DataTable();

            Database lmsDatabase = DatabaseFactory.CreateDatabase();

            using (DbConnection connection = lmsDatabase.CreateConnection())
            {
                try
                {
                    connection.Open();

                    timeRange = lmsDatabase.ExecuteDataSet("uspGetTimeRange").Tables[0];
                }
                catch (Exception ex)
                {
                    ExceptionHandlerDataAccess.Write(ex, @"CalendarDataAccess", SessionHelper.HostID);
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }

            return timeRange;
        }

        /// <summary>
        /// Gets event
        /// </summary>
        public static CalendarEvent GetEvent(int CalendarEventID)
        {
            CalendarEvent Event = new CalendarEvent();

            Database database = DatabaseFactory.CreateDatabase();

            using (DbConnection connection = database.CreateConnection())
            {
                try
                {
                    connection.Open();

                    IDataReader controlReader = database.ExecuteReader("uspGetCalendarEvent", CalendarEventID);

                    while (controlReader.Read())
                    {
                        Event.CalendarEventID = DatabaseHelper.GetInt(controlReader["CalendarEventID"]);
                        Event.StartDate = DatabaseHelper.GetDateTime(controlReader["StartDate"]);
                        Event.EndDate = DatabaseHelper.GetDateTime(controlReader["EndDate"]);
                        Event.EventName = DatabaseHelper.GetString(controlReader["EventName"]);
                        Event.EventDescription = DatabaseHelper.GetString(controlReader["EventDescription"]);
                        Event.EventType = DatabaseHelper.GetInt(controlReader["EventType"]);
                        Event.AllDayEvent = DatabaseHelper.GetBoolean(controlReader["AllDayEvent"]);
                        Event.Confirmed = DatabaseHelper.GetBoolean(controlReader["Confirmed"]);
                    }
                }
                catch (Exception ex)
                {
                    ExceptionHandlerDataAccess.Write(ex, @"CalendarDataAccess", SessionHelper.HostID);
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }

            return Event;
        }

        /// <summary>
        /// Gets events
        /// </summary>
        public static List<CalendarEvent> GetEvents(string StartDate, string EndDate, string UserName)
        {
            List<CalendarEvent> EventList = new List<CalendarEvent>();

            Database database = DatabaseFactory.CreateDatabase();

            using (DbConnection connection = database.CreateConnection())
            {
                try
                {
                    connection.Open();

                    IDataReader controlReader = database.ExecuteReader("uspGetCalendarEvents", StartDate, EndDate, UserName);

                    while (controlReader.Read())
                    {
                        CalendarEvent Event = new CalendarEvent();
                        Event.CalendarEventID = DatabaseHelper.GetInt(controlReader["CalendarEventID"]);
                        Event.StartDate = DatabaseHelper.GetDateTime(controlReader["StartDate"]);
                        Event.EndDate = DatabaseHelper.GetDateTime(controlReader["EndDate"]);
                        Event.EventName = DatabaseHelper.GetString(controlReader["EventName"]);
                        Event.EventDescription = DatabaseHelper.GetString(controlReader["EventDescription"]);
                        Event.EventType = DatabaseHelper.GetInt(controlReader["EventType"]);
                        Event.AllDayEvent = DatabaseHelper.GetBoolean(controlReader["AllDayEvent"]);
                        Event.Confirmed = DatabaseHelper.GetBoolean(controlReader["Confirmed"]);

                        EventList.Add(Event);
                    }
                }
                catch (Exception ex)
                {
                    ExceptionHandlerDataAccess.Write(ex, @"CalendarDataAccess", SessionHelper.HostID);
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }

            return EventList;
        }

        /// <summary>
        /// Gets event for user
        /// </summary>
        public static Member GetMemberEvent(int CalendarEventID)
        {
            Member Member = new Member();

            Database database = DatabaseFactory.CreateDatabase();

            using (DbConnection connection = database.CreateConnection())
            {
                try
                {
                    connection.Open();

                    IDataReader controlReader = database.ExecuteReader("uspGetMemberEvent", CalendarEventID);

                    while (controlReader.Read())
                    {
                        Member.MemberID = DatabaseHelper.GetInt(controlReader["MemberID"]);
                        Member.FirstName = controlReader["FirstName"].ToString();
                        Member.LastName = controlReader["LastName"].ToString();
                        Member.Adult = DatabaseHelper.GetBoolean(controlReader["Adult"]);
                        Member.PGFirstName = DatabaseHelper.GetString(controlReader["PGFirstName"]);
                        Member.PGLastName = DatabaseHelper.GetString(controlReader["PGLastName"]);
                        Member.DOB = DatabaseHelper.GetNullableDateTime(controlReader["DOB"]);
                        Member.PrimaryNumber = DatabaseHelper.GetString(controlReader["PrimaryNumber"]);
                        Member.SecondaryNumber = DatabaseHelper.GetString(controlReader["SecondaryNumber"]);
                        Member.PreferredCallBackID = DatabaseHelper.GetNullableInt(controlReader["PreferredCallBackID"]);
                        Member.HowDidYouHearID = DatabaseHelper.GetNullableInt(controlReader["HowDidYouHearID"]);
                        Member.Quality = DatabaseHelper.GetNullableInt(controlReader["Quality"]);
                        Member.EmailAddress = DatabaseHelper.GetString(controlReader["EmailAddress"]);
                        Member.MemberTypeID = DatabaseHelper.GetInt(controlReader["MemberTypeID"]);
                        Member.CalendarEvent.CalendarEventID = DatabaseHelper.GetInt(controlReader["CalendarEventID"]);
                        Member.CalendarEvent.EventName = DatabaseHelper.GetString(controlReader["EventName"]);
                        Member.CalendarEvent.AllDayEvent = DatabaseHelper.GetBoolean(controlReader["AllDayEvent"]);
                        Member.CalendarEvent.StartDate = DatabaseHelper.GetDateTime(controlReader["StartDate"]);
                        Member.CalendarEvent.EndDate = DatabaseHelper.GetDateTime(controlReader["EndDate"]);
                        Member.CalendarEvent.EventDescription = DatabaseHelper.GetString(controlReader["EventDescription"]);
                        Member.CalendarEvent.Confirmed = DatabaseHelper.GetNullableBoolean(controlReader["Confirmed"]);
                        Member.CalendarEvent.EventType = DatabaseHelper.GetNullableInt(controlReader["EventType"]);
                        Member.Notes = DatabaseHelper.GetString(controlReader["Notes"]);
                    }
                }
                catch (Exception ex)
                {
                    ExceptionHandlerDataAccess.Write(ex, @"CalendarDataAccess", SessionHelper.HostID);
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }

            return Member;
        }

        public static DataTable GetMeetingType()
        {
            DataTable MeetingTypes = new DataTable();
            Database database = DatabaseFactory.CreateDatabase();
            using (DbConnection connection = database.CreateConnection())
            {
                connection.Open();

                try
                {

                    MeetingTypes = database.ExecuteDataSet("uspGetMeetingType").Tables[0];
                }
                catch (Exception ex)
                {
                    ExceptionHandlerDataAccess.Write(ex, @"CalendarDataAccess.cs", SessionHelper.HostID);
                }
                finally
                {
                    connection.Close();
                }
            }
            return MeetingTypes;
        }

        public static int InsertCalendarEvent(CalendarEvent Event)
        {
            int CalendarEventID = 0;
            Database database = DatabaseFactory.CreateDatabase();
            using (DbConnection connection = database.CreateConnection())
            {
                connection.Open();

                try
                {
                    IDataReader controlReader = database.ExecuteReader(
                           "uspInsertCalendarEvent",
                           Event.EventName,
                           Event.EventDescription,
                           Event.StartDate,
                           Event.EndDate,
                           Event.AllDayEvent,
                           Event.EventType,
                           Event.MemberID,
                           Event.Confirmed);

                    while (controlReader.Read()) CalendarEventID = DatabaseHelper.GetInt(controlReader["CalendarEventID"]);
                }
                catch (Exception ex)
                {
                    ExceptionHandlerDataAccess.Write(ex, @"CalendarDataAccess.cs", SessionHelper.HostID);
                }
                finally
                {
                    connection.Close();
                }
            }
            return CalendarEventID;
        }

        public static void UpdateCalendarEvent(CalendarEvent Event)
        {
            Database database = DatabaseFactory.CreateDatabase();
            using (DbConnection connection = database.CreateConnection())
            {
                connection.Open();

                try
                {
                    database.ExecuteNonQuery(
                          "uspUpdateCalendarEvent",
                          Event.CalendarEventID,
                          Event.EventName,
                          Event.EventDescription,
                          Event.StartDate,
                          Event.EndDate,
                          Event.EventType,
                          Event.Confirmed,
                          Event.AllDayEvent);

                }
                catch (Exception ex)
                {
                    ExceptionHandlerDataAccess.Write(ex, @"CalendarDataAccess.cs", SessionHelper.HostID);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public static void DeleteCalendarEvent(int CalendarEventID)
        {
            Database database = DatabaseFactory.CreateDatabase();
            using (DbConnection connection = database.CreateConnection())
            {
                connection.Open();

                try
                {
                    database.ExecuteNonQuery(
                          "uspDeleteCalendarEvent",
                          CalendarEventID);

                }
                catch (Exception ex)
                {
                    ExceptionHandlerDataAccess.Write(ex, @"CalendarDataAccess.cs", SessionHelper.HostID);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

    }
}
