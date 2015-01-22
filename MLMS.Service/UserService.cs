namespace MLMS.Service
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    using MLMS.Data;
    using MLMS.Objects;

    /// <summary>
    /// Handles user profile and management transactions
    /// </summary>
    public static class UserService
    {
        /// <summary>
        /// Inserts custom profile info into the database
        /// </summary>
        /// <param name="profile">The custom profile object</param>
        public static int InsertUserProfile(UserProfile profile, string password)
        {
            return UserDataAccess.InsertUser(profile, password);
        }

        /// <summary>
        /// Gets a list of users based on a username (search)
        /// </summary>
        /// <param name="userName">The username used for search</param>
        /// <param name="roleAliases">comma delimited list of role aliases to include</param>
        /// <returns>List of users</returns>
        public static List<UserProfile> GetUserList(string userName, string roleAliases)
        {
            if (userName == null)
            {
                userName = string.Empty;
            }

            List<UserProfile> users = UserDataAccess.GetUsers(userName, roleAliases);
            //users.Sort();
            return users;
        }

        /// <summary>
        /// Gets a data table of users based on a username (search)
        /// </summary>
        /// <param name="userName">The username used for search</param>
        /// <returns>List of users</returns>
        public static DataTable GetUserListTable(string userName)
        {
            if (userName == null)
            {
                userName = string.Empty;
            }

            DataTable users = UserDataAccess.GetUsersTable(userName);
            return users;
        }

        public static DataSet GetUsers(string userName)
        {
            if (userName == null)
            {
                userName = string.Empty;
            }

            DataSet users = UserDataAccess.GetUsersDS(userName);
            return users;
        }

        /// <summary>
        /// Gets a populated user profile object
        /// </summary>
        /// <param name="userName">The user name</param>
        /// <returns>Populated User Profile object</returns>
        public static UserProfile GetUser(string userName, int hostID)
        {
            UserProfile user = UserDataAccess.GetUser(userName, hostID);
            return user;
        }

        /// <summary>
        /// Gets a populated user profile object
        /// </summary>
        /// <param name="userID">userID</param>
        /// <returns>Populated User Profile object</returns>
        public static UserProfile GetUser(int userID)
        {
            UserProfile user = UserDataAccess.GetUser(userID);
            //user.Companys = GetUserCompanys(userName, hostID);
            return user;
        }

        /// <summary>
        /// Gets a populated user profile object
        /// </summary>
        /// <param name="userName">The user name</param>
        /// <returns>Populated User Profile object</returns>
        public static UserProfile GetUserByEmail(string email, int hostID)
        {
            UserProfile user = UserDataAccess.GetUserByEmail(email, hostID);
            return user;
        }

        /// <summary>
        /// Updates the user profile
        /// </summary>
        /// <param name="user">The user profile object</param>
        public static void UpdateUser(UserProfile user)
        {
            try
            {
                UserDataAccess.UpdateUser(user);
            }
            catch (Exception ex)
            {
                ExceptionHandler.Write(ex, @"UserManagementService");
                throw;
            }
        }

        /// <summary>
        /// Gets a list of site referrals from the DB
        /// </summary>
        /// <returns>A list of site referrals</returns>
        public static DataTable GetSiteReferrals(int hostID)
        {
            return UserDataAccess.GetSiteReferrals(hostID);
        }

        /// <summary>
        /// Validates username and password in the DB
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static DataTable AuthenticateUser(string username, string password)
        {
            try
            {
                return UserDataAccess.AuthenticateUser(username, password);
            }
            catch (Exception ex)
            {
                ExceptionHandler.Write(ex, @"UserManagementService.cs");
                throw;
            }
        }
        /// <summary>
        /// Unlocks user after too many failed login attempts
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static bool UnlockUser(string username)
        {
            try
            {
                return UserDataAccess.UnlockUser(username);
            }
            catch (Exception ex)
            {
                ExceptionHandler.Write(ex, @"UserManagementService.cs");
                throw;
            }
        }

        /// <summary>
        /// Updates the password in the UserProfile table 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public static bool ChangePassword(int userID, string newPassword)
        {
            try
            {
                return UserDataAccess.ChangePassword(userID, newPassword);
            }
            catch (Exception ex)
            {
                ExceptionHandler.Write(ex, @"UserManagementService.cs");
                throw;
            }
        }

        /// <summary>
        /// Gets the password from tblUserProfile
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static string GetPassword(int userID)
        {
            try
            {
                return UserDataAccess.GetPassword(userID);
            }
            catch (Exception ex)
            {
                ExceptionHandler.Write(ex, @"UserManagementService.cs");
                throw;
            }
        }
        /// <summary>
        /// Updates the FailedLoginAttempts column in tblUserProfile
        /// </summary>
        /// <param name="username"></param>
        /// <param name="failedLoginAttempts"></param>
        public static void FailedLoginAttemptsUpdate(string username, int failedLoginAttempts)
        {
            try
            {
                UserDataAccess.FailedLoginAttemptsUpdate(username, failedLoginAttempts);
            }
            catch (Exception ex)
            {
                ExceptionHandler.Write(ex, @"UserManagementService.cs");
                throw;
            }
        }

        public static void IsLockedOutUpdate(int userID, bool isLockedOut)
        {
            try
            {
                UserDataAccess.IsLockedOutUpdate(userID, isLockedOut);
            }
            catch (Exception ex)
            {
                ExceptionHandler.Write(ex, @"UserManagementService.cs");
                throw;
            }
        }
        
    }
}
