namespace MLMS.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using MLMS.Common;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using MLMS.Objects;

    public static class MemberDataAccess
    {
        #region Insert Methods
        /// <summary>
        /// Inserts new lead
        /// </summary>
        public static int InsertMember(Member Member, string UserName)
        {
            int leadID = 0;
            Database database = DatabaseFactory.CreateDatabase();
            using (DbConnection connection = database.CreateConnection())
            {
                connection.Open();

                try
                {
                    IDataReader controlReader = database.ExecuteReader(
                           "uspInsertMember",
                           DatabaseHelper.FromNull(Member.FirstName),
                           DatabaseHelper.FromNull(Member.LastName),
                           Member.Adult,
                           DatabaseHelper.FromNull(Member.PGFirstName),
                           DatabaseHelper.FromNull(Member.PGLastName),
                           Member.DOB,
                           Member.PrimaryNumber,
                           Member.SecondaryNumber,
                           Member.PreferredCallBackID,
                           DatabaseHelper.FromNull(Member.EmailAddress),
                           Member.HowDidYouHearID,
                           DatabaseHelper.FromNull(Member.Notes),
                           Member.MemberTypeID,
                           Member.Quality,
                           UserName);

                    while (controlReader.Read()) leadID = DatabaseHelper.GetInt(controlReader["MemberID"]);

                    foreach (Achieve ach in Member.Achieve)
                    {
                        database.ExecuteNonQuery(
                            "uspInsertMemberAchieve",
                            leadID,
                            ach.AchieveID);
                    }
                }
                catch (Exception ex)
                {
                    ExceptionHandlerDataAccess.Write(ex, @"MemberDataAccess.cs", SessionHelper.HostID);
                }
                finally
                {
                    connection.Close();
                }
            }
            return leadID;
        }

        /// <summary>
        /// Inserts new Call Log for a member
        /// </summary>
        public static void InsertCallLog(int LeadID, DateTime StartDate, string Notes)
        {
            Database database = DatabaseFactory.CreateDatabase();
            using (DbConnection connection = database.CreateConnection())
            {
                connection.Open();

                try
                {
                    database.ExecuteNonQuery(
                           "uspInsertCallLog",
                           LeadID,
                           StartDate,
                           Notes);
                }
                catch (Exception ex)
                {
                    ExceptionHandlerDataAccess.Write(ex, @"MemberDataAccess.cs", SessionHelper.HostID);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        #endregion

        #region Get Methods
        /// <summary>
        /// Gets preferred call back
        /// </summary>
        public static DataTable GetPreferredCallBack()
        {
            DataTable referrals = new DataTable();
            Database database = DatabaseFactory.CreateDatabase();
            using (DbConnection connection = database.CreateConnection())
            {
                connection.Open();

                try
                {

                    referrals = database.ExecuteDataSet("uspGetPreferredCallBack").Tables[0];
                }
                catch (Exception ex)
                {
                    ExceptionHandlerDataAccess.Write(ex, @"MemberDataAccess.cs", SessionHelper.HostID);
                }
                finally
                {
                    connection.Close();
                }
            }
            return referrals;
        }

        /// <summary>
        /// Gets how did you hear
        /// </summary>
        public static DataTable GetHearAbout(string UserName)
        {
            DataTable referrals = new DataTable();
            Database database = DatabaseFactory.CreateDatabase();
            using (DbConnection connection = database.CreateConnection())
            {
                connection.Open();

                try
                {

                    referrals = database.ExecuteDataSet("uspGetHowDidYouHear", UserName).Tables[0];
                }
                catch (Exception ex)
                {
                    ExceptionHandlerDataAccess.Write(ex, @"MemberDataAccess.cs", SessionHelper.HostID);
                }
                finally
                {
                    connection.Close();
                }
            }
            return referrals;
        }

        /// <summary>
        /// Gets preferred call back
        /// </summary>
        public static DataTable GetUserCheckListOptions(int UserChecklistID)
        {
            DataTable UserChecklistOptions = new DataTable();
            Database database = DatabaseFactory.CreateDatabase();
            using (DbConnection connection = database.CreateConnection())
            {
                connection.Open();

                try
                {

                    UserChecklistOptions = database.ExecuteDataSet("uspGetUserChecklistOptions", UserChecklistID).Tables[0];
                }
                catch (Exception ex)
                {
                    ExceptionHandlerDataAccess.Write(ex, @"MemberDataAccess.cs", SessionHelper.HostID);
                }
                finally
                {
                    connection.Close();
                }
            }
            return UserChecklistOptions;
        }

        /// <summary>
        /// Gets user achieve list
        /// </summary>
        public static DataTable GetUserAchieveList(string UserName)
        {
            DataTable achieveList = new DataTable();
            Database Database = DatabaseFactory.CreateDatabase();
            using (DbConnection connection = Database.CreateConnection())
            {
                try
                {
                    connection.Open();
                    achieveList = Database.ExecuteDataSet("uspGetUserAchieveList", UserName).Tables[0];
                }
                catch (Exception ex)
                {
                    ExceptionHandlerDataAccess.Write(ex, @"MemberDataAccess.cs", SessionHelper.HostID);
                }

                finally
                {
                    connection.Close();
                }
            }
            return achieveList;
        }

        /// <summary>
        /// Gets user objection list
        /// </summary>
        public static DataTable GetObjections(string UserName)
        {
            DataTable objections = new DataTable();
            Database Database = DatabaseFactory.CreateDatabase();
            using (DbConnection connection = Database.CreateConnection())
            {
                try
                {
                    connection.Open();
                    objections = Database.ExecuteDataSet("uspGetObjection", UserName).Tables[0];
                }
                catch (Exception ex)
                {
                    ExceptionHandlerDataAccess.Write(ex, @"MemberDataAccess.cs", SessionHelper.HostID);
                }

                finally
                {
                    connection.Close();
                }
            }
            return objections;
        }

        /// <summary>
        /// Gets leads based on filter
        /// </summary>
        public static List<Member> GetFilteredLeads(string UserName, string FirstName, string LastName, string PhoneNumber, string PreferredCallBack, int LeadType)
        {
            List<Member> members = new List<Member>();
            Database lmsDatabase = DatabaseFactory.CreateDatabase();
            using (DbConnection connection = lmsDatabase.CreateConnection())
            {
                try
                {
                    connection.Open();
                    IDataReader controlReader = lmsDatabase.ExecuteReader("uspGetLeads", UserName, FirstName, LastName, PhoneNumber, PreferredCallBack, LeadType);

                    while (controlReader.Read())
                    {
                        Member member = new Member();
                        member = FormatMember(controlReader);

                        members.Add(member);
                    }

                    controlReader.Close();
                }
                catch (Exception ex) { }

                finally
                {
                    connection.Close();
                }
                return members;
            }
        }

        /// <summary>
        /// Gets leads to call based on user
        /// </summary>
        public static List<Member> GetLeadsToCall(string UserName)
        {
            List<Member> members = new List<Member>();
            Database lmsDatabase = DatabaseFactory.CreateDatabase();
            using (DbConnection connection = lmsDatabase.CreateConnection())
            {
                try
                {
                    connection.Open();
                    IDataReader controlReader = lmsDatabase.ExecuteReader("uspGetLeadsToCall", UserName);

                    while (controlReader.Read())
                    {
                        Member member = new Member();
                        member = FormatMember(controlReader);

                        members.Add(member);
                    }

                    controlReader.Close();
                }
                catch (Exception ex) { }

                finally
                {
                    connection.Close();
                }
                return members;
            }
        }

        /// <summary>
        /// Gets leads to call with upcoming meetings
        /// </summary>
        public static List<Member> GetUpcomingMeetingsToConfirm(string UserName)
        {
            List<Member> members = new List<Member>();
            Database lmsDatabase = DatabaseFactory.CreateDatabase();
            using (DbConnection connection = lmsDatabase.CreateConnection())
            {
                try
                {
                    connection.Open();
                    IDataReader controlReader = lmsDatabase.ExecuteReader("uspUpcomingMeetingsToConfirm", UserName);

                    while (controlReader.Read())
                    {
                        Member member = new Member();
                        member = FormatMember(controlReader);

                        members.Add(member);
                    }

                    controlReader.Close();
                }
                catch (Exception ex) { }

                finally
                {
                    connection.Close();
                }
                return members;
            }
        }

        /// <summary>
        /// Gets all follow ups
        /// </summary>
        public static List<Member> GetFollowUps(string UserName)
        {
            List<Member> members = new List<Member>();
            Database lmsDatabase = DatabaseFactory.CreateDatabase();
            using (DbConnection connection = lmsDatabase.CreateConnection())
            {
                try
                {
                    connection.Open();
                    IDataReader controlReader = lmsDatabase.ExecuteReader("uspGetAllFollowUps", UserName);

                    while (controlReader.Read())
                    {
                        Member member = new Member();
                        member = FormatMember(controlReader);

                        members.Add(member);
                    }

                    controlReader.Close();
                }
                catch (Exception ex) { }

                finally
                {
                    connection.Close();
                }
                return members;
            }
        }

        /// <summary>
        /// Get specific Lead
        /// </summary>
        public static Member GetLead(int MemberID)
        {
            Member member = new Member();
            Database lmsDatabase = DatabaseFactory.CreateDatabase();
            using (DbConnection connection = lmsDatabase.CreateConnection())
            {
                try
                {
                    connection.Open();
                    IDataReader controlReader = lmsDatabase.ExecuteReader("uspGetLead", MemberID);

                    while (controlReader.Read())
                    {
                        member = FormatMember(controlReader);

                    }
                    controlReader.Close();
                }
                catch (Exception ex) { }

                finally
                {
                    connection.Close();
                }
                return member;
            }
        }

        /// <summary>
        /// Get list of members by user
        /// </summary>
        public static List<Member> GetMembersByUser(string UserName, int MemberType)
        {
            List<Member> memberList = new List<Member>();
            Database lmsDatabase = DatabaseFactory.CreateDatabase();
            using (DbConnection connection = lmsDatabase.CreateConnection())
            {
                try
                {
                    connection.Open();
                    IDataReader controlReader = lmsDatabase.ExecuteReader("uspGetMembersByUserName", UserName, MemberType);

                    while (controlReader.Read())
                    {
                        Member member = new Member();
                        member.MemberID = DatabaseHelper.GetInt(controlReader["MemberID"]);
                        member.FirstName = controlReader["FirstName"].ToString();
                        member.LastName = controlReader["LastName"].ToString();
                        member.Adult = DatabaseHelper.GetBoolean(controlReader["Adult"]);
                        member.PGFirstName = DatabaseHelper.GetString(controlReader["PGFirstName"]);
                        member.PGLastName = DatabaseHelper.GetString(controlReader["PGLastName"]);
                        member.DOB = DatabaseHelper.GetNullableDateTime(controlReader["DOB"]);
                        member.PrimaryNumber = DatabaseHelper.GetString(controlReader["PrimaryNumber"]);
                        member.SecondaryNumber = DatabaseHelper.GetString(controlReader["SecondaryNumber"]);
                        member.PreferredCallBackID = DatabaseHelper.GetNullableInt(controlReader["PreferredCallBackID"]);
                        member.HowDidYouHearID = DatabaseHelper.GetNullableInt(controlReader["HowDidYouHearID"]);
                        member.EmailAddress = DatabaseHelper.GetString(controlReader["EmailAddress"]);
                        member.MemberTypeID = DatabaseHelper.GetInt(controlReader["MemberTypeID"]);

                        memberList.Add(member);
                    }

                    controlReader.Close();
                }
                catch (Exception ex) { }

                finally
                {
                    connection.Close();
                }
                return memberList;
            }
        }

        /// <summary>
        /// Get member by id
        /// </summary>
        public static Member GetMember(int MemberID)
        {
            Member member = new Member();
            Database lmsDatabase = DatabaseFactory.CreateDatabase();
            using (DbConnection connection = lmsDatabase.CreateConnection())
            {
                try
                {
                    connection.Open();
                    IDataReader controlReader = lmsDatabase.ExecuteReader("uspGetMember", MemberID);

                    while (controlReader.Read())
                    {
                        member = FormatMember(controlReader);
                    }

                    controlReader.Close();
                }
                catch (Exception ex) { }

                finally
                {
                    connection.Close();
                }
                return member;
            }
        }

        /// <summary>
        /// Get member by id
        /// </summary>
        public static DataSet GetCalendarEventsForMember(int MemberID)
        {
            DataSet ds = new DataSet();
            Database lmsDatabase = DatabaseFactory.CreateDatabase();
            using (DbConnection connection = lmsDatabase.CreateConnection())
            {
                try
                {
                    connection.Open();
                    ds = lmsDatabase.ExecuteDataSet("uspGetMemberCalendarEvents", MemberID, null, null);
                }
                catch (Exception ex) { }

                finally
                {
                    connection.Close();
                }
                return ds;
            }
        }

        /// <summary>
        /// Get member by id
        /// </summary>
        public static DataSet GetCalendarEventsByDateRange(int MemberID, string StartDate, string EndDate)
        {
            DataSet ds = new DataSet();
            Database lmsDatabase = DatabaseFactory.CreateDatabase();
            using (DbConnection connection = lmsDatabase.CreateConnection())
            {
                try
                {
                    connection.Open();
                    ds = lmsDatabase.ExecuteDataSet("uspGetMemberCalendarEvents", MemberID, StartDate, EndDate);
                }
                catch (Exception ex) { }

                finally
                {
                    connection.Close();
                }
                return ds;
            }
        }

        /// <summary>
        /// Get call log for a member
        /// </summary>
        public static DataTable GetCallLog(int MemberID)
        {
            DataTable dt = new DataTable();
            Database lmsDatabase = DatabaseFactory.CreateDatabase();
            using (DbConnection connection = lmsDatabase.CreateConnection())
            {
                try
                {
                    connection.Open();
                    dt = lmsDatabase.ExecuteDataSet("uspGetCallLog", MemberID).Tables[0];
                }
                catch (Exception ex) { }

                finally
                {
                    connection.Close();
                }
                return dt;
            }
        }

        /// <summary>
        /// Get member achieve list
        /// </summary>
        public static List<Achieve> GetMemberAchieve(int MemberID)
        {
            List<Achieve> achieveList = new List<Achieve>();
            Database lmsDatabase = DatabaseFactory.CreateDatabase();
            using (DbConnection connection = lmsDatabase.CreateConnection())
            {
                try
                {
                    connection.Open();
                    IDataReader controlReader = lmsDatabase.ExecuteReader("uspGetMemberAchieve", MemberID);

                    while (controlReader.Read())
                    {
                        Achieve achieve = new Achieve();
                        achieve.AchieveID = DatabaseHelper.GetInt(controlReader["AchieveID"]);
                        achieve.AchieveName = DatabaseHelper.GetString(controlReader["AchieveName"]);

                        achieveList.Add(achieve);
                    }
                }
                catch (Exception ex) { }

                finally
                {
                    connection.Close();
                }
                return achieveList;
            }
        }

        /// <summary>
        /// Get member checklist
        /// </summary>
        public static DataTable GetMemberChecklist(int MemberID)
        {
            DataTable dt = new DataTable();
            Database lmsDatabase = DatabaseFactory.CreateDatabase();
            using (DbConnection connection = lmsDatabase.CreateConnection())
            {
                try
                {
                    connection.Open();
                    dt = lmsDatabase.ExecuteDataSet("uspGetMemberChecklist", MemberID).Tables[0];
                }
                catch (Exception ex) { }

                finally
                {
                    connection.Close();
                }
                return dt;
            }
        }

        private static Member FormatMember(IDataReader controlReader)
        {
            Member member = new Member();
            member.MemberID = DatabaseHelper.GetInt(controlReader["MemberID"]);
            member.FirstName = controlReader["FirstName"].ToString();
            member.LastName = controlReader["LastName"].ToString();
            member.Adult = DatabaseHelper.GetBoolean(controlReader["Adult"]);
            member.PGFirstName = DatabaseHelper.GetString(controlReader["PGFirstName"]);
            member.PGLastName = DatabaseHelper.GetString(controlReader["PGLastName"]);
            member.DOB = DatabaseHelper.GetNullableDateTime(controlReader["DOB"]);
            member.PrimaryNumber = DatabaseHelper.GetString(controlReader["PrimaryNumber"]);
            member.SecondaryNumber = DatabaseHelper.GetString(controlReader["SecondaryNumber"]);
            member.PreferredCallBackID = DatabaseHelper.GetNullableInt(controlReader["PreferredCallBackID"]);
            member.PreferredCallBack = DatabaseHelper.GetString(controlReader["PreferredValue"]);
            member.HowDidYouHearID = DatabaseHelper.GetNullableInt(controlReader["HowDidYouHearID"]);
            member.EmailAddress = DatabaseHelper.GetString(controlReader["EmailAddress"]);
            member.Quality = DatabaseHelper.GetNullableInt(controlReader["Quality"]);
            member.CalendarEvent.CalendarEventID = DatabaseHelper.GetInt(controlReader["CalendarEventID"]);
            member.CalendarEvent.EventName = DatabaseHelper.GetString(controlReader["EventName"]);
            member.CalendarEvent.AllDayEvent = DatabaseHelper.GetBoolean(controlReader["AllDayEvent"]);
            member.CalendarEvent.StartDate = DatabaseHelper.GetDateTime(controlReader["StartDate"]);
            member.CalendarEvent.EndDate = DatabaseHelper.GetDateTime(controlReader["EndDate"]);
            member.CalendarEvent.EventDescription = DatabaseHelper.GetString(controlReader["EventDescription"]);
            member.CalendarEvent.Confirmed = DatabaseHelper.GetNullableBoolean(controlReader["Confirmed"]);
            member.Notes = DatabaseHelper.GetString(controlReader["Notes"]);
            member.MemberTypeID = DatabaseHelper.GetInt(controlReader["MemberTypeID"]);

            return member;
        }

        #endregion

        #region Update Methods
        /// <summary>
        /// Inserts new lead
        /// </summary>
        public static void UpdateMember(Member Member)
        {
            Database database = DatabaseFactory.CreateDatabase();
            using (DbConnection connection = database.CreateConnection())
            {
                connection.Open();
                var transaction = connection.BeginTransaction();

                try
                {
                    database.ExecuteNonQuery(
                           transaction,
                           "uspUpdateMember",
                           DatabaseHelper.FromNull(Member.FirstName),
                           DatabaseHelper.FromNull(Member.LastName),
                           Member.Adult,
                           DatabaseHelper.FromNull(Member.PGFirstName),
                           DatabaseHelper.FromNull(Member.PGLastName),
                           Member.DOB,
                           Member.PrimaryNumber,
                           Member.SecondaryNumber,
                           Member.PreferredCallBackID,
                           DatabaseHelper.FromNull(Member.EmailAddress),
                           Member.HowDidYouHearID,
                           DatabaseHelper.FromNull(Member.Notes),
                           Member.MemberTypeID,
                           Member.Quality,
                           Member.MemberID);

                    database.ExecuteNonQuery(
                                transaction,
                               "uspDeleteMemberAchieve",
                               Member.MemberID);

                    foreach (Achieve ach in Member.Achieve)
                    {
                        database.ExecuteNonQuery(
                            transaction,
                            "uspInsertMemberAchieve",
                            Member.MemberID,
                            ach.AchieveID);
                    }

                    foreach (UserChecklist UserCheckList in Member.UserChecklist)
                    {
                        foreach (UserChecklistOption option in UserCheckList.UserChecklistOption)
                        {
                            if (option.CheckedOption)
                            {
                                database.ExecuteNonQuery(
                                        transaction,
                                       "uspInsertMemberChecklistOption",
                                        Member.MemberID,
                                        option.UserChecklistOptionID);
                            }
                            else
                            {
                                database.ExecuteNonQuery(
                                        transaction,
                                        "uspDeleteMemberChecklistOption",
                                        Member.MemberID,
                                        option.UserChecklistOptionID);
                            }
                        }
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ExceptionHandlerDataAccess.Write(ex, @"MemberDataAccess.cs", SessionHelper.HostID);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// Inactivate lead
        /// </summary>
        public static void InactivateLead(int MemberID, int? Objection, string AdditionalNotes, bool IsActive)
        {
            Database database = DatabaseFactory.CreateDatabase();
            using (DbConnection connection = database.CreateConnection())
            {
                connection.Open();
                var transaction = connection.BeginTransaction();

                try
                {
                    database.ExecuteNonQuery(
                           transaction,
                           "uspInactivateMember",
                           MemberID,
                           Objection,
                           AdditionalNotes,
                           IsActive);

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ExceptionHandlerDataAccess.Write(ex, @"MemberDataAccess.cs", SessionHelper.HostID);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// Update call log 
        /// </summary>
        public static void UpdateCallLog(int MemberCallLog, string Notes)
        {
            Database database = DatabaseFactory.CreateDatabase();
            using (DbConnection connection = database.CreateConnection())
            {
                connection.Open();

                try
                {
                    database.ExecuteNonQuery(
                           "uspUpdateMemberCallLog",
                            MemberCallLog,
                            Notes);
                }
                catch (Exception ex)
                {
                    ExceptionHandlerDataAccess.Write(ex, @"MemberDataAccess.cs", SessionHelper.HostID);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// Update call log 
        /// </summary>
        public static void UpdateUserCheckListOption(int MemberID, List<UserChecklistOption> UserChecklistOptions)
        {
            Database database = DatabaseFactory.CreateDatabase();
            using (DbConnection connection = database.CreateConnection())
            {
                connection.Open();

                try
                {
                    foreach (UserChecklistOption option in UserChecklistOptions)
                    {
                        if (option.CheckedOption)
                        {
                            database.ExecuteNonQuery(
                                   "uspInsertMemberChecklistOption",
                                    MemberID,
                                    option.UserChecklistOptionID);
                        }
                        else
                        {
                            database.ExecuteNonQuery(
                                   "uspDeleteMemberChecklistOption",
                                   MemberID,
                                   option.UserChecklistOptionID);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ExceptionHandlerDataAccess.Write(ex, @"MemberDataAccess.cs", SessionHelper.HostID);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        #endregion

        #region Delete Methods
        /// <summary>
        /// Inserts new lead
        /// </summary>
        public static bool DeleteMember(int MemberID)
        {
            bool success = false;
            Database database = DatabaseFactory.CreateDatabase();
            using (DbConnection connection = database.CreateConnection())
            {
                connection.Open();
                var transaction = connection.BeginTransaction();

                try
                {
                    int rowsAffected = database.ExecuteNonQuery(
                           transaction,
                           "uspDeleteMember",
                           MemberID);

                    transaction.Commit();

                    if (rowsAffected > 0) success = true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ExceptionHandlerDataAccess.Write(ex, @"MemberDataAccess.cs", SessionHelper.HostID);
                }
                finally
                {
                    connection.Close();
                }
            }
            return success;
        }

        /// <summary>
        /// Delete Call History
        /// </summary>
        public static void DeleteCallHistory(int MemberCallLog)
        {
            Database database = DatabaseFactory.CreateDatabase();
            using (DbConnection connection = database.CreateConnection())
            {
                connection.Open();
                var transaction = connection.BeginTransaction();

                try
                {
                    database.ExecuteNonQuery(
                           "uspDeleteCallLogHistory",
                            MemberCallLog);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ExceptionHandlerDataAccess.Write(ex, @"MemberDataAccess.cs", SessionHelper.HostID);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        #endregion
    }
}
