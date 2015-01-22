using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using MLMS.Objects;
using MLMS.Common;

namespace MLMS.Data
{
    public static class AdminDataAccess
    {
        #region Insert
        /// <summary>
        /// Inserts How did you hear options
        /// </summary>
        public static int InsertHowDidYouHear(string HowDidYouHear, string UserName)
        {
            int HowDidYouHearID = 0;
            Database database = DatabaseFactory.CreateDatabase();
            using (DbConnection connection = database.CreateConnection())
            {
                connection.Open();

                try
                {
                    IDataReader controlReader = database.ExecuteReader(
                           "uspInsertHowDidYouHear",
                            HowDidYouHear,
                            UserName);

                    while (controlReader.Read()) HowDidYouHearID = DatabaseHelper.GetInt(controlReader["HowDidYouHearID"]);
                }
                catch (Exception ex)
                {
                    ExceptionHandlerDataAccess.Write(ex, @"AdminDataAccess.cs", SessionHelper.HostID);
                }
                finally
                {
                    connection.Close();
                }
            }
            return HowDidYouHearID;
        }

        /// <summary>
        /// Inserts Achieve options
        /// </summary>
        public static int InsertAchieve(string Achieve, string UserName)
        {
            int AchieveID = 0;
            Database database = DatabaseFactory.CreateDatabase();
            using (DbConnection connection = database.CreateConnection())
            {
                connection.Open();

                try
                {
                    IDataReader controlReader = database.ExecuteReader(
                           "uspInsertAchieve",
                            Achieve,
                            UserName);

                    while (controlReader.Read()) AchieveID = DatabaseHelper.GetInt(controlReader["AchieveID"]);
                }
                catch (Exception ex)
                {
                    ExceptionHandlerDataAccess.Write(ex, @"AdminDataAccess.cs", SessionHelper.HostID);
                }
                finally
                {
                    connection.Close();
                }
            }
            return AchieveID;
        }

        /// <summary>
        /// Inserts Objection options
        /// </summary>
        public static int InsertObjection(string Objection, string UserName)
        {
            int ObjectionID = 0;
            Database database = DatabaseFactory.CreateDatabase();
            using (DbConnection connection = database.CreateConnection())
            {
                connection.Open();

                try
                {
                    IDataReader controlReader = database.ExecuteReader(
                           "uspInsertObjection",
                            Objection,
                            UserName);

                    while (controlReader.Read()) ObjectionID = DatabaseHelper.GetInt(controlReader["ObjectionID"]);
                }
                catch (Exception ex)
                {
                    ExceptionHandlerDataAccess.Write(ex, @"AdminDataAccess.cs", SessionHelper.HostID);
                }
                finally
                {
                    connection.Close();
                }
            }
            return ObjectionID;
        }

        /// <summary>
        /// Inserts How did you hear options
        /// </summary>
        public static int InsertUserChecklist(string ChecklistName, string UserName)
        {
            int userChecklistID = 0;
            Database database = DatabaseFactory.CreateDatabase();
            using (DbConnection connection = database.CreateConnection())
            {
                connection.Open();

                try
                {
                    userChecklistID = Convert.ToInt32(database.ExecuteScalar(
                            "uspInsertUserChecklist",
                            ChecklistName
                            , UserName));
                }
                catch (Exception ex)
                {
                    ExceptionHandlerDataAccess.Write(ex, @"AdminDataAccess.cs", SessionHelper.HostID);
                }
                finally
                {
                    connection.Close();
                }
            }
            return userChecklistID;
        }

        /// <summary>
        /// Inserts How did you hear options
        /// </summary>
        public static int InsertUserChecklistOption(string OptionName, int UserChecklistID)
        {
            int userChecklistOptionID = 0;
            Database database = DatabaseFactory.CreateDatabase();
            using (DbConnection connection = database.CreateConnection())
            {
                connection.Open();

                try
                {
                    userChecklistOptionID = Convert.ToInt32(database.ExecuteScalar(
                            "uspInsertUserChecklistOption",
                            OptionName
                            , UserChecklistID));
                }
                catch (Exception ex)
                {
                    ExceptionHandlerDataAccess.Write(ex, @"AdminDataAccess.cs", SessionHelper.HostID);
                }
                finally
                {
                    connection.Close();
                }
            }
            return userChecklistOptionID;
        }
        #endregion

        #region Get
        /// <summary>
        /// Gets user achieve list
        /// </summary>
        public static DataTable GetUserCheckList(string UserName)
        {
            DataTable userCheckList = new DataTable();
            Database Database = DatabaseFactory.CreateDatabase();
            using (DbConnection connection = Database.CreateConnection())
            {
                try
                {
                    connection.Open();
                    userCheckList = Database.ExecuteDataSet("uspGetUserCheckList", UserName).Tables[0];
                }
                catch (Exception ex)
                {
                    ExceptionHandlerDataAccess.Write(ex, @"AdminDataAccess.cs", SessionHelper.HostID);
                }

                finally
                {
                    connection.Close();
                }
            }
            return userCheckList;
        }
        #endregion

        #region Update
        /// <summary>
        /// Update How did you hear options
        /// </summary>
        public static void UpdateHowDidYouHear(string HowDidYouHear, int HowDidYouHearID)
        {
            Database database = DatabaseFactory.CreateDatabase();
            using (DbConnection connection = database.CreateConnection())
            {
                connection.Open();

                try
                {
                    database.ExecuteNonQuery(
                           "uspUpdateHowDidYouHear",
                            HowDidYouHear,
                            HowDidYouHearID);
                }
                catch (Exception ex)
                {
                    ExceptionHandlerDataAccess.Write(ex, @"AdminDataAccess.cs", SessionHelper.HostID);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// Update Achieve options
        /// </summary>
        public static void UpdateAchieve(string Achieve, int AchieveID)
        {
            Database database = DatabaseFactory.CreateDatabase();
            using (DbConnection connection = database.CreateConnection())
            {
                connection.Open();

                try
                {
                    database.ExecuteNonQuery(
                           "uspUpdateAchieve",
                            Achieve,
                            AchieveID);
                }
                catch (Exception ex)
                {
                    ExceptionHandlerDataAccess.Write(ex, @"AdminDataAccess.cs", SessionHelper.HostID);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// Update How did you hear options
        /// </summary>
        public static void UpdateObjection(string Objection, int ObjectionID)
        {
            Database database = DatabaseFactory.CreateDatabase();
            using (DbConnection connection = database.CreateConnection())
            {
                connection.Open();

                try
                {
                    database.ExecuteNonQuery(
                           "uspUpdateObjection",
                            Objection,
                            ObjectionID);
                }
                catch (Exception ex)
                {
                    ExceptionHandlerDataAccess.Write(ex, @"AdminDataAccess.cs", SessionHelper.HostID);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// Update User Check List
        /// </summary>
        public static void UpdateUserChecklist(string ChecklistName, int UserChecklistID)
        {
            Database database = DatabaseFactory.CreateDatabase();
            using (DbConnection connection = database.CreateConnection())
            {
                connection.Open();

                try
                {
                    database.ExecuteNonQuery(
                           "uspUpdateUserChecklist",
                            ChecklistName,
                            UserChecklistID);
                }
                catch (Exception ex)
                {
                    ExceptionHandlerDataAccess.Write(ex, @"AdminDataAccess.cs", SessionHelper.HostID);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// Update User Check List
        /// </summary>
        public static void UpdateUserChecklistOption(string OptionName, int UserChecklistOptionID)
        {
            Database database = DatabaseFactory.CreateDatabase();
            using (DbConnection connection = database.CreateConnection())
            {
                connection.Open();

                try
                {
                    database.ExecuteNonQuery(
                           "uspUpdateUserChecklistOption",
                            OptionName,
                            UserChecklistOptionID);
                }
                catch (Exception ex)
                {
                    ExceptionHandlerDataAccess.Write(ex, @"AdminDataAccess.cs", SessionHelper.HostID);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        #endregion

        #region Delete

        /// <summary>
        /// Delete How did you hear options
        /// </summary>
        public static void DeleteHowDidYouHear(int HowDidYouHearID)
        {
            Database database = DatabaseFactory.CreateDatabase();
            using (DbConnection connection = database.CreateConnection())
            {
                connection.Open();

                try
                {
                    database.ExecuteNonQuery(
                           "uspDeleteHowDidYouHear",
                            HowDidYouHearID);
                }
                catch (Exception ex)
                {
                    ExceptionHandlerDataAccess.Write(ex, @"AdminDataAccess.cs", SessionHelper.HostID);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// Delete Achieve options
        /// </summary>
        public static void DeleteAchieve(int AchieveID)
        {
            Database database = DatabaseFactory.CreateDatabase();
            using (DbConnection connection = database.CreateConnection())
            {
                connection.Open();

                try
                {
                    database.ExecuteNonQuery(
                           "uspDeleteAchieve",
                            AchieveID);
                }
                catch (Exception ex)
                {
                    ExceptionHandlerDataAccess.Write(ex, @"AdminDataAccess.cs", SessionHelper.HostID);
                }
                finally
                {
                    connection.Close();
                }
            }
        }


        /// <summary>
        /// Delete Objection option
        /// </summary>
        public static void DeleteObjection(int ObjectionID)
        {
            Database database = DatabaseFactory.CreateDatabase();
            using (DbConnection connection = database.CreateConnection())
            {
                connection.Open();

                try
                {
                    database.ExecuteNonQuery(
                           "uspDeleteObjection",
                            ObjectionID);
                }
                catch (Exception ex)
                {
                    ExceptionHandlerDataAccess.Write(ex, @"AdminDataAccess.cs", SessionHelper.HostID);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// Delete User Check list and any members check list options that is associated to this check list
        /// </summary>
        public static void DeleteUserChecklist(int UserChecklistID)
        {
            Database database = DatabaseFactory.CreateDatabase();
            using (DbConnection connection = database.CreateConnection())
            {
                connection.Open();

                try
                {
                    database.ExecuteNonQuery(
                           "uspDeleteUserChecklist",
                            UserChecklistID);
                }
                catch (Exception ex)
                {
                    ExceptionHandlerDataAccess.Write(ex, @"AdminDataAccess.cs", SessionHelper.HostID);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// 	Delete User Check list Option and any members check list options that is associated
        /// </summary>
        public static void DeleteUserChecklistOption(int UserChecklistOptionID)
        {
            Database database = DatabaseFactory.CreateDatabase();
            using (DbConnection connection = database.CreateConnection())
            {
                connection.Open();

                try
                {
                    database.ExecuteNonQuery(
                           "uspDeleteUserChecklistOption",
                            UserChecklistOptionID);
                }
                catch (Exception ex)
                {
                    ExceptionHandlerDataAccess.Write(ex, @"AdminDataAccess.cs", SessionHelper.HostID);
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
