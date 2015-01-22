namespace MLMS.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    using Microsoft.Practices.EnterpriseLibrary.Data;
    using MLMS.Objects;
    using MLMS.Common;

    /// <summary>
    /// Handles the data access for operations 
    /// related to users
    /// </summary>
    public static class UserDataAccess
    {
        /// <summary>
        /// Gets a list of all users 
        /// that match the provided parameter
        /// </summary>
        /// <param name="userName">The Username for search</param>
        /// <returns>List of all users</returns>
        public static List<UserProfile> GetUsers(string userName, string roleAliases)
        {
            List<UserProfile> users = null;

            Database lmsDatabase = DatabaseFactory.CreateDatabase();

            using (DbConnection connection = lmsDatabase.CreateConnection())
            {
                try
                {
                    connection.Open();
                    IDataReader dataReader = lmsDatabase.ExecuteReader("uspGetUserList", userName, roleAliases, SessionHelper.HostID);

                    while (dataReader.Read())
                    {
                        UserProfile user = new UserProfile();
                        user.UserID = Convert.ToInt32(dataReader["UserID"].ToString());
                        user.HostID = Convert.ToInt32(dataReader["HostID"].ToString());
                        user.UserName = dataReader["UserName"].ToString();
                        user.FirstName = dataReader["FirstName"].ToString();
                        user.LastName = dataReader["LastName"].ToString();
                        user.Email = dataReader["Email"].ToString();
                        user.Role = dataReader["RoleAlias"].ToString();

                        if (users == null)
                        {
                            users = new List<UserProfile>();
                        }

                        users.Add(user);
                    }
                    dataReader.Close();
                }
                catch (Exception ex)
                {
                    ExceptionHandlerDataAccess.Write(ex, @"UserDataAccess", SessionHelper.HostID);
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }

            return users;
        }

        /// <summary>
        /// Gets a DataTable of all users 
        /// that match the provided parameter
        /// </summary>
        /// <param name="userName">The Username for search</param>
        /// <returns>a DataTable containing of all users</returns>
        public static DataTable GetUsersTable(string userName)
        {
            DataTable users = new DataTable();

            Database lmsDatabase = DatabaseFactory.CreateDatabase();

            using (DbConnection connection = lmsDatabase.CreateConnection())
            {
                try
                {
                    connection.Open();

                    users = lmsDatabase.ExecuteDataSet("uspGetUserList", userName).Tables[0];
                }
                catch (Exception ex)
                {
                    ExceptionHandlerDataAccess.Write(ex, @"UserDataAccess", SessionHelper.HostID);
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }

            return users;
        }

        public static DataSet GetUsersDS(string userName)
        {
            DataSet users = new DataSet();

            Database lmsDatabase = DatabaseFactory.CreateDatabase();

            using (DbConnection connection = lmsDatabase.CreateConnection())
            {
                try
                {
                    connection.Open();

                    users = lmsDatabase.ExecuteDataSet("uspGetUserList", userName);
                }
                catch (Exception ex)
                {
                    ExceptionHandlerDataAccess.Write(ex, @"UserDataAccess", SessionHelper.HostID);
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }

            return users;
        }

        /// <summary>
        /// Gets a populated user profile object from the DB
        /// </summary>
        /// <param name="userName">The user name</param>
        /// <param name="hostID">Host Identifier</param>
        /// <returns>Populated User Profile object</returns>
        public static UserProfile GetUser(string userName, int hostID)
        {
            UserProfile user = new UserProfile();

            Database lmsDatabase = DatabaseFactory.CreateDatabase();

            using (DbConnection connection = lmsDatabase.CreateConnection())
            {
                try
                {
                    connection.Open();
                    IDataReader dataReader = lmsDatabase.ExecuteReader("uspGetUserProfileByUsername", userName, hostID);

                    while (dataReader.Read())
                    {
                        user.UserID = Convert.ToInt32(dataReader["UserID"].ToString());
                        user.HostID = hostID;
                        user.UserName = dataReader["UserName"].ToString();
                        user.FirstName = dataReader["FirstName"].ToString();
                        user.LastName = dataReader["LastName"].ToString();
                        user.Email = dataReader["Email"].ToString();
                        user.AddressLine1 = dataReader["AddressLine1"].ToString();
                        user.AddressLine2 = dataReader["AddressLine2"].ToString();
                        user.City = dataReader["City"].ToString();
                        user.State = dataReader["State"].ToString();
                        user.Zip = dataReader["Zip"].ToString();
                        user.Phone = dataReader["Phone"].ToString();
                        user.CellPhone = dataReader["CellPhone"].ToString();
                        user.ReceivePromoEmail = DatabaseHelper.GetNullableBoolean(dataReader["ReceivePromoEmail"]);
                        user.ReceivePromoText = DatabaseHelper.GetNullableBoolean(dataReader["ReceivePromoText"]);
                        user.AdditionalEmail = dataReader["AdditionalEmail"].ToString();
                        user.ShareDataWithPartner = DatabaseHelper.GetNullableBoolean(dataReader["ShareDataWithPartner"]);
                        user.Country = dataReader["Country"].ToString();
                        user.Age = dataReader["Age"].ToString();
                        user.Gender = dataReader["Gender"].ToString();
                        user.Ethnicity = dataReader["Ethnicity"].ToString();
                        user.Experience = dataReader["Experience"].ToString();
                        user.PositionsHeld = dataReader["PositionsHeld"].ToString();
                        user.IsLockedOut = Convert.ToBoolean(dataReader["IsLockedOut"]);
                        user.ModifiedDate = DatabaseHelper.GetNullableDateTime(dataReader["ModifiedDate"]);
                        user.CreatedDate = Convert.ToDateTime(dataReader["CreatedDate"]);
                        user.Role = dataReader["RoleAlias"].ToString();
                        user.CultureCode = dataReader["CultureCode"].ToString();
                    }
                    dataReader.Close();
                }
                catch (Exception ex)
                {
                    ExceptionHandlerDataAccess.Write(ex, @"UserDataAccess", SessionHelper.HostID);
                    throw;
                }
                finally
                {
                    connection.Close();
                }
                return user;
            }
        }
                
        /// <summary>
        /// Gets a populated user profile object from the DB
        /// </summary>
        /// <param name="userName">The email address</param>
        /// <param name="hostID">Host Identifier</param>
        /// <returns>Populated User Profile object</returns>
        public static UserProfile GetUserByEmail(string email, int hostID)
        {
            UserProfile user = new UserProfile();

            Database lmsDatabase = DatabaseFactory.CreateDatabase();

            using (DbConnection connection = lmsDatabase.CreateConnection())
            {
                try
                {
                    connection.Open();
                    IDataReader dataReader = lmsDatabase.ExecuteReader("uspGetUserProfileByEmail", email, hostID);

                    while (dataReader.Read())
                    {
                        user.UserID = Convert.ToInt32(dataReader["UserID"].ToString());
                        user.HostID = Convert.ToInt32(dataReader["HostID"].ToString());
                        user.UserName = dataReader["UserName"].ToString();
                        user.FirstName = dataReader["FirstName"].ToString();
                        user.LastName = dataReader["LastName"].ToString();
                        user.Email = dataReader["Email"].ToString();
                        user.AddressLine1 = dataReader["AddressLine1"].ToString();
                        user.AddressLine2 = dataReader["AddressLine2"].ToString();
                        user.City = dataReader["City"].ToString();
                        user.State = dataReader["State"].ToString();
                        user.Zip = dataReader["Zip"].ToString();
                        user.Phone = dataReader["Phone"].ToString();
                        user.CellPhone = dataReader["CellPhone"].ToString();
                        user.ReceivePromoEmail = DatabaseHelper.GetNullableBoolean(dataReader["ReceivePromoEmail"]);
                        user.ReceivePromoText = DatabaseHelper.GetNullableBoolean(dataReader["ReceivePromoText"]);
                        user.AdditionalEmail = dataReader["AdditionalEmail"].ToString();
                        user.ShareDataWithPartner = DatabaseHelper.GetNullableBoolean(dataReader["ShareDataWithPartner"]);
                        user.Country = dataReader["Country"].ToString();
                        user.Age = dataReader["Age"].ToString();
                        user.Gender = dataReader["Gender"].ToString();
                        user.Ethnicity = dataReader["Ethnicity"].ToString();
                        user.Experience = dataReader["Experience"].ToString();
                        user.PositionsHeld = dataReader["PositionsHeld"].ToString();
                        user.IsLockedOut = Convert.ToBoolean(dataReader["IsLockedOut"]);
                        user.ModifiedDate = DatabaseHelper.GetNullableDateTime(dataReader["ModifiedDate"]);
                        user.CreatedDate = Convert.ToDateTime(dataReader["CreatedDate"]);
                        user.Role = dataReader["RoleAlias"].ToString();
                    }
                    dataReader.Close();
                }
                catch (Exception ex)
                {
                    ExceptionHandlerDataAccess.Write(ex, @"UserDataAccess", SessionHelper.HostID);
                    throw;
                }
                finally
                {
                    connection.Close();
                }
                return user;
            }
        }

        /// <summary>
        /// Gets a populated user profile object from the DB
        /// </summary>
        /// <param name="userID"></param>
        /// <returns>Populated User Profile object</returns>
        public static UserProfile GetUser(int userID)
        {
            UserProfile user = new UserProfile();

            Database lmsDatabase = DatabaseFactory.CreateDatabase();

            using (DbConnection connection = lmsDatabase.CreateConnection())
            {
                try
                {
                    connection.Open();
                    IDataReader dataReader = lmsDatabase.ExecuteReader("uspUserProfileSelect", userID);

                    while (dataReader.Read())
                    {
                        user.UserID = Convert.ToInt32(dataReader["UserID"].ToString());
                        user.HostID = Convert.ToInt32(dataReader["HostID"].ToString());
                        user.UserName = dataReader["UserName"].ToString();
                        user.FirstName = dataReader["FirstName"].ToString();
                        user.LastName = dataReader["LastName"].ToString();
                        user.Email = dataReader["Email"].ToString();
                        user.AddressLine1 = dataReader["AddressLine1"].ToString();
                        user.AddressLine2 = dataReader["AddressLine2"].ToString();
                        user.City = dataReader["City"].ToString();
                        user.State = dataReader["State"].ToString();
                        user.Zip = dataReader["Zip"].ToString();
                        user.Phone = dataReader["Phone"].ToString();
                        user.CellPhone = dataReader["CellPhone"].ToString();
                        user.ReceivePromoEmail = DatabaseHelper.GetNullableBoolean(dataReader["ReceivePromoEmail"]);
                        user.ReceivePromoText = DatabaseHelper.GetNullableBoolean(dataReader["ReceivePromoText"]);
                        user.AdditionalEmail = dataReader["AdditionalEmail"].ToString();
                        user.ShareDataWithPartner = DatabaseHelper.GetNullableBoolean(dataReader["ShareDataWithPartner"]);
                        user.Country = dataReader["Country"].ToString();
                        user.Age = dataReader["Age"].ToString();
                        user.Gender = dataReader["Gender"].ToString();
                        user.Ethnicity = dataReader["Ethnicity"].ToString();
                        user.Experience = dataReader["Experience"].ToString();
                        user.PositionsHeld = dataReader["PositionsHeld"].ToString();
                        user.IsLockedOut = Convert.ToBoolean(dataReader["IsLockedOut"]);
                        user.ModifiedDate = DatabaseHelper.GetNullableDateTime(dataReader["ModifiedDate"]);
                        user.CreatedDate = Convert.ToDateTime(dataReader["CreatedDate"]);
                        user.Role = dataReader["RoleAlias"].ToString();
                    }
                    dataReader.Close();
                }
                catch (Exception ex)
                {
                    ExceptionHandlerDataAccess.Write(ex, @"UserDataAccess", SessionHelper.HostID);
                    throw;
                }
                finally
                {
                    connection.Close();
                }
                return user;
            }
        }

        /// <summary>
        /// Updates the user profile data in the database
        /// </summary>
        /// <param name="user">The user profile object</param>
        public static void UpdateUser(UserProfile user)
        {
            ////var recPromoEmails = DatabaseHelper.FromNullable<bool>(user.ReceivePromoEmail.Value);
            ////var recPromoTexts = DatabaseHelper.FromNullable<bool>(user.ReceivePromoText.Value);
            ////var shareData = DatabaseHelper.FromNullable<bool>(user.ShareDataWithPartner.Value);

            Database database = DatabaseFactory.CreateDatabase();
            using (DbConnection connection = database.CreateConnection())
            {
                connection.Open();
                var transaction = connection.BeginTransaction();
                try
                {
                     int rowsaffected = database.ExecuteNonQuery(
                            transaction,
                            "uspUpdateUserProfile",                            
                            DatabaseHelper.FromNull(user.FirstName),
                            DatabaseHelper.FromNull(user.LastName),
                            DatabaseHelper.FromNull(user.UserName),
                            DatabaseHelper.FromNull(user.OriginalUserName),
                            DatabaseHelper.FromNull(user.AddressLine1),
                            DatabaseHelper.FromNull(user.AddressLine2),
                            DatabaseHelper.FromNull(user.City),
                            DatabaseHelper.FromNull(user.State),
                            DatabaseHelper.FromNull(user.Country),
                            DatabaseHelper.FromNull(user.Zip),
                            DatabaseHelper.FromNull(user.Phone),
                            DatabaseHelper.FromNull(user.CellPhone),
                            DatabaseHelper.FromNullable(user.ReceivePromoEmail),
                            DatabaseHelper.FromNullable(user.ReceivePromoText),
                            DatabaseHelper.FromNull(user.AdditionalEmail),
                            DatabaseHelper.FromNullable(user.ShareDataWithPartner),
                            DatabaseHelper.FromNull(user.Age),
                            DatabaseHelper.FromNull(user.Gender),
                            DatabaseHelper.FromNull(user.Ethnicity),
                            DatabaseHelper.FromNull(user.Experience),
                            DatabaseHelper.FromNull(user.PositionsHeld),
                            DatabaseHelper.FromNullable(user.SiteReferral),
                            DatabaseHelper.FromNull(user.ModifiedBy),
                            DatabaseHelper.FromNull(user.CultureCode),
                            user.HostID);

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ExceptionHandlerDataAccess.Write(ex, @"UserDataAccess.cs", SessionHelper.HostID);
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// Gets a list of site referrals from the DB
        /// </summary>
        /// <returns>A list of site referrals</returns>
        public static DataTable GetSiteReferrals(int hostID)
        {
            try
            {
                ////Database db = DatabaseFactory.CreateDatabase();
                ////DbCommand cmd = db.GetStoredProcCommand("uspGetSiteReferrals");
                ////DataSet ds = db.ExecuteDataSet(cmd);
                ////return ds.Tables[0];
                DataTable referrals = new DataTable();

                Database lmsDatabase = DatabaseFactory.CreateDatabase();

                using (DbConnection connection = lmsDatabase.CreateConnection())
                {
                    try
                    {
                        connection.Open();

                        referrals = lmsDatabase.ExecuteDataSet("uspGetSiteReferrals", hostID.ToString()).Tables[0];
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandlerDataAccess.Write(ex, @"UserDataAccess", SessionHelper.HostID);
                        throw;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }

                return referrals;
            }
            catch (Exception ex)
            {
                ExceptionHandlerDataAccess.Write(ex, @"UserDataAccess.cs", hostID);
                throw;
            }
        }

        /// <summary>
        /// Inserts custom profile data into the
        /// database to a custom table
        /// </summary>
        /// <param name="profile">The user profile object</param>
        public static int InsertUser(UserProfile profile, string password)
        {
            decimal userID = 0;

            Database database = DatabaseFactory.CreateDatabase();
            using (DbConnection connection = database.CreateConnection())
            {
                connection.Open();
                var transaction = connection.BeginTransaction();

                try
                {
                     userID = (decimal)database.ExecuteScalar(
                            transaction,
                            "uspInsertCustomUserProfileData",                           
                            DatabaseHelper.FromNull(profile.UserName),
                            DatabaseHelper.FromNull(profile.FirstName),
                            DatabaseHelper.FromNull(profile.LastName),
                            DatabaseHelper.FromNull(profile.Email),
                            DatabaseHelper.FromNull(profile.AddressLine1),
                            DatabaseHelper.FromNull(profile.AddressLine2),
                            DatabaseHelper.FromNull(profile.City),
                            DatabaseHelper.FromNull(profile.State),
                            DatabaseHelper.FromNull(profile.Country),
                            DatabaseHelper.FromNull(profile.Zip),
                            DatabaseHelper.FromNull(profile.Phone),
                            DatabaseHelper.FromNull(profile.CellPhone),
                            DatabaseHelper.FromNullable(profile.ReceivePromoEmail),
                            DatabaseHelper.FromNullable(profile.ReceivePromoText),
                            DatabaseHelper.FromNull(profile.AdditionalEmail),
                            DatabaseHelper.FromNullable(profile.ShareDataWithPartner),
                            DatabaseHelper.FromNull(profile.Age),
                            DatabaseHelper.FromNull(profile.Gender),
                            DatabaseHelper.FromNull(profile.Ethnicity),
                            DatabaseHelper.FromNull(profile.Experience),
                            DatabaseHelper.FromNull(profile.PositionsHeld),
                            DatabaseHelper.FromNullable(profile.SiteReferral),
                            DatabaseHelper.FromNull(profile.CultureCode),
                            DatabaseHelper.FromNull(profile.ModifiedBy),                            
                            profile.HostID,
                            password);
                    
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ExceptionHandlerDataAccess.Write(ex, @"UserDataAccess.cs", SessionHelper.HostID);
                }
                finally
                {
                    connection.Close();
                }
            }
            return Convert.ToInt32(userID);
        }
        
        /// <summary>
        /// Validates username and password in the DB
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static DataTable AuthenticateUser(string username, string password)
        {
            DataTable user = new DataTable();

            Database lmsDatabase = DatabaseFactory.CreateDatabase();

            using (DbConnection connection = lmsDatabase.CreateConnection())
            {
                try
                {
                    connection.Open();
                    user = lmsDatabase.ExecuteDataSet("sec.uspUserAuthenticate", username, SessionHelper.HostID).Tables[0];

                    //if (user.Rows.Count == 0)
                        //update 
                }
                catch (Exception ex)
                {
                    ExceptionHandlerDataAccess.Write(ex, @"UserDataAccess", SessionHelper.HostID);
                    throw;
                }
                finally
                {
                    connection.Close();
                }
                return user;
            }
        }
        
        /// <summary>
        ///  Unlocks user after too many failed login attempts
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static bool UnlockUser(string username)
        {
            Database database = DatabaseFactory.CreateDatabase();
            using (DbConnection connection = database.CreateConnection())
            {
                try
                {
                    connection.Open();
                    int rowsaffected = database.ExecuteNonQuery(
                            "uspUserUnlock",
                            username,
                            SessionHelper.HostID);
                   if (rowsaffected > 0)
                       return true;
                   else
                       return false;
                }
                catch (Exception ex)
                {
                    ExceptionHandlerDataAccess.Write(ex, @"UserDataAccess.cs", SessionHelper.HostID);
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        ///  Updates the password in the UserProfile table 
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static bool ChangePassword(int userID, string newPassword)
        {
            Database database = DatabaseFactory.CreateDatabase();
            using (DbConnection connection = database.CreateConnection())
            {
                try
                {
                    connection.Open();
                    object returnVal = database.ExecuteScalar(
                             "sec.uspUserPasswordChange",
                             userID,
                             newPassword,
                             newPassword);
                   
                    if (returnVal == null)
                        return true;
                    else
                        return false;
                }
                catch (Exception ex)
                {
                    ExceptionHandlerDataAccess.Write(ex, @"UserDataAccess.cs", SessionHelper.HostID);
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// Gets the password from tblUserProfile 
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static string GetPassword(int userID)
        {
            string password = string.Empty;
            Database lmsDatabase = DatabaseFactory.CreateDatabase();

            using (DbConnection connection = lmsDatabase.CreateConnection())
            {
                try
                {
                    connection.Open();
                    IDataReader dataReader = lmsDatabase.ExecuteReader("sec.uspUserPasswordSelect", userID);
                    
                    if (dataReader.Read())
                        password = DatabaseHelper.GetString(dataReader["Password"]);
                    dataReader.Close();
                }
                catch (Exception ex)
                {
                    ExceptionHandlerDataAccess.Write(ex, @"UserDataAccess.cs", SessionHelper.HostID);
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }

            return password;
        }

        /// <summary>
        /// Updates the FailedLoginAttempts column in tblUserProfile
        /// </summary>
        /// <param name="username"></param>
        /// <param name="failedLoginAttempts"></param>
        public static void FailedLoginAttemptsUpdate(string username, int failedLoginAttempts)
        {
            Database database = DatabaseFactory.CreateDatabase();
            using (DbConnection connection = database.CreateConnection())
            {
                try
                {
                    connection.Open();
                    database.ExecuteNonQuery("uspUserFailedLoginAttemptsUpdate", 
                                                username, 
                                                SessionHelper.HostID, 
                                                failedLoginAttempts);
                }
                catch (Exception ex)
                {
                    ExceptionHandlerDataAccess.Write(ex, @"UserDataAccess.cs", SessionHelper.HostID);
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public static void IsLockedOutUpdate(int userID, bool isLockedOut)
        {
            Database database = DatabaseFactory.CreateDatabase();
            using (DbConnection connection = database.CreateConnection())
            {
                try
                {
                    connection.Open();
                    database.ExecuteNonQuery("uspUserIsLockedOutUpdate", userID, isLockedOut);
                }
                catch (Exception ex)
                {
                    ExceptionHandlerDataAccess.Write(ex, @"UserDataAccess.cs", SessionHelper.HostID);
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}
