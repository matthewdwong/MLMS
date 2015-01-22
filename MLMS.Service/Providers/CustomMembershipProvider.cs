using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Data;
using System.Web.Security;

using MLMS.Common;
using MLMS.Objects;

namespace MLMS.Service.Providers
{  
    /// <summary>
    /// This class inherits and extends MembershipProvider class
    /// </summary>
    public class CustomMembershipProvider : MembershipProvider
    {
        #region Fields
        private string pApplicationName;
        private bool pEnablePasswordReset;
        private bool pEnablePasswordRetrieval;
        private bool pRequiresQuestionAndAnswer;
        private bool pRequiresUniqueEmail;
        private int pMaxInvalidPasswordAttempts;
        private int pPasswordAttemptWindow;
        private MembershipPasswordFormat pPasswordFormat;
        private int pMinRequiredNonAlphanumericCharacters;
        private int pMinRequiredPasswordLength;
        private string pPasswordStrengthRegularExpression;
        private bool pWriteExceptionsToEventLog;
        private string connectionString;


        #endregion

        #region Properties
        public override int MinRequiredPasswordLength
        {
            get { return pMinRequiredPasswordLength; }
        }
        public override bool RequiresUniqueEmail
        {
            get { return pRequiresUniqueEmail; }
        }
        public override bool EnablePasswordReset
        {
            get { return pEnablePasswordReset; }
        }
        public override bool EnablePasswordRetrieval
        {
            get { return pEnablePasswordRetrieval; }
        }
        public override bool RequiresQuestionAndAnswer
        {
            get { return pRequiresQuestionAndAnswer; }
        }
        public override string ApplicationName
        {
            get { return pApplicationName; }
            set { pApplicationName = value; }
        }
        public override int MaxInvalidPasswordAttempts
        {
            get { return pMaxInvalidPasswordAttempts; }
        }
        public override int PasswordAttemptWindow
        {
            get { return pPasswordAttemptWindow; }
        }
        public override MembershipPasswordFormat PasswordFormat
        {
            get { return pPasswordFormat; }
        }
        public override int MinRequiredNonAlphanumericCharacters
        {
            get { return pMinRequiredNonAlphanumericCharacters; }
        }
        public override string PasswordStrengthRegularExpression
        {
          get { return pPasswordStrengthRegularExpression; }
        }
        public bool WriteExceptionsToEventLog
        {
            get { return pWriteExceptionsToEventLog; }
            set { pWriteExceptionsToEventLog = value; }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Creates a new user to the LMS.
        /// </summary>
        /// <param name="username">The username</param>
        /// <param name="password">The password</param>
        /// <param name="email">The email address</param>
        /// <param name="passwordQuestion">The password question</param>
        /// <param name="passwordAnswer">The password Answer</param>
        /// <param name="isApproved">Bool is approved</param>
        /// <param name="providerUserKey">the User Key</param>
        /// <param name="status">Output status</param>
        /// <returns>The user created</returns>
        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            try
            {
                ValidatePasswordEventArgs args =
                    new ValidatePasswordEventArgs(username, password, true);
                OnValidatingPassword(args);
                
                if (args.Cancel)
                {
                    status = MembershipCreateStatus.InvalidPassword;
                    return null;
                }

                if (RequiresUniqueEmail && GetUserNameByEmail(email) != null)
                {
                    status = MembershipCreateStatus.DuplicateEmail;
                    return null;
                }

                //if (!ValidateUser(username, password))
                //{
                //    status = MembershipCreateStatus.UserRejected;
                //    return null;
                //}

                var newUser = new UserProfile {UserName = email, HostID = SessionHelper.HostID}; //create UserProfile with required fields only; we'll update the other profile fields later

                int userID = UserService.InsertUserProfile(newUser, password);

                MembershipUser user = GetUser(username, false);
                if (user == null || userID == 0)
                {
                    status = MembershipCreateStatus.UserRejected;
                }
                else
                {
                    status = MembershipCreateStatus.Success;
                }
                return user;
            }
            catch (Exception e)
            {
                ExceptionHandler.Write(e, "CustomMembershipProvider.CreateUser()");
                status = MembershipCreateStatus.ProviderError;

                return null;
            }            
        }

        public override bool ValidateUser(string username, string password)
        {
            DataTable userDT = UserService.AuthenticateUser(username, password);

            if (userDT.Rows.Count == 0)
                return false; //invalid username

            DataRow user = userDT.Rows[0];

            int failedLoginAttempts = user["FailedLoginAttempts"].ToString() == String.Empty ? 0 : Convert.ToInt16(user["FailedLoginAttempts"].ToString());
            DateTime lastLoginAttempt = user["LastLoginAttempt"].ToString() == String.Empty ? System.DateTime.Now : Convert.ToDateTime(user["LastLoginAttempt"].ToString());
            int userID = Convert.ToInt32(user["UserID"].ToString());

            if (!Convert.ToBoolean(user["IsActive"]))
                return false;

            if (lastLoginAttempt < System.DateTime.Now.AddMinutes(-pPasswordAttemptWindow))
            {
                failedLoginAttempts = 0;
                UserService.FailedLoginAttemptsUpdate(username, failedLoginAttempts);
            }

            if (failedLoginAttempts >= pMaxInvalidPasswordAttempts && lastLoginAttempt >= System.DateTime.Now.AddMinutes(-pPasswordAttemptWindow))
            {
                UserService.FailedLoginAttemptsUpdate(username, failedLoginAttempts + 1);
                UserService.IsLockedOutUpdate(userID, true);
                return false;
            }

            //Is Password correct?
            if (password.Trim() != user["Password"].ToString())
            {
                UserService.FailedLoginAttemptsUpdate(username, failedLoginAttempts + 1);
                return false;
            }

            failedLoginAttempts = 0;
            UserService.FailedLoginAttemptsUpdate(username, failedLoginAttempts);

            return true;
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            UserProfile userRep = new UserProfile();
            userRep = UserService.GetUser(username, SessionHelper.HostID);

            if (userRep.UserID != 0)
            {
                MembershipUser memUser = new MembershipUser("CustomMembershipProvider", 
                                               username, userRep.UserID, userRep.Email,
                                               string.Empty, string.Empty,
                                               true, userRep.IsLockedOut, userRep.CreatedDate,
                                               DateTime.MinValue,
                                               DateTime.MinValue,
                                               DateTime.Now, DateTime.Now);
                return memUser;
            }
            return null;
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }
        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }
        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }
        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPwdQuestion, string newPwdAnswer)
        {
           throw new NotImplementedException(); 
        }
        public override string GetPassword(string username, string answer)
        {
            if (!EnablePasswordRetrieval)
            {
                throw new ProviderException("Password Retrieval Not Enabled.");
            }

            if (PasswordFormat == MembershipPasswordFormat.Hashed)
            {
                throw new ProviderException("Cannot retrieve Hashed passwords.");
            }
            
            MembershipUser user = GetUser(username, false);
            return UserService.GetPassword((int)user.ProviderUserKey);
        }
        public override bool ChangePassword(string username, string oldPwd, string newPwd)
        {
            if (!ValidateUser(username, oldPwd))
                return false;

            var args = new ValidatePasswordEventArgs(username, newPwd, true);

            OnValidatingPassword(args);

            if (args.Cancel)
                if (args.FailureInformation != null)
                    throw args.FailureInformation;
                else
                    throw new MembershipPasswordException("Change password canceled due to new password validation failure.");
            
            MembershipUser user = GetUser(username, true);
            return UserService.ChangePassword((int)user.ProviderUserKey, newPwd);
        }
        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }
        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }
        public override bool UnlockUser(string username)
        {
            MembershipUser user = GetUser(username, false);

            return user != null && UserService.UnlockUser(user.UserName);
        }
        public override string GetUserNameByEmail(string email)
        {
            return UserService.GetUserByEmail(email, 1).UserName;
        }
        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }
        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }
        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override void Initialize(string name, NameValueCollection config)
        {
            //
            // Initialize values from web.config.
            //

            if (config == null)
                throw new ArgumentNullException("config");

            if (name == null || name.Length == 0)
                name = "CustomMembershipProvider";

            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "LMS Custom Membership provider");
            }

            // Initialize the abstract base class.
            base.Initialize(name, config);

            pApplicationName = GetConfigValue(config["applicationName"],
                                            System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
            pMaxInvalidPasswordAttempts = Convert.ToInt32(GetConfigValue(config["maxInvalidPasswordAttempts"], "5"));
            pPasswordAttemptWindow = Convert.ToInt32(GetConfigValue(config["passwordAttemptWindow"], "10"));
            pMinRequiredNonAlphanumericCharacters = Convert.ToInt32(GetConfigValue(config["minRequiredNonAlphanumericCharacters"], "1"));
            pMinRequiredPasswordLength = Convert.ToInt32(GetConfigValue(config["minRequiredPasswordLength"], "7"));
            pPasswordStrengthRegularExpression = Convert.ToString(GetConfigValue(config["passwordStrengthRegularExpression"], ""));
            pEnablePasswordReset = Convert.ToBoolean(GetConfigValue(config["enablePasswordReset"], "true"));
            pEnablePasswordRetrieval = Convert.ToBoolean(GetConfigValue(config["enablePasswordRetrieval"], "true"));
            pRequiresQuestionAndAnswer = Convert.ToBoolean(GetConfigValue(config["requiresQuestionAndAnswer"], "false"));
            pRequiresUniqueEmail = Convert.ToBoolean(GetConfigValue(config["requiresUniqueEmail"], "true"));
            pWriteExceptionsToEventLog = Convert.ToBoolean(GetConfigValue(config["writeExceptionsToEventLog"], "true"));

            string temp_format = config["passwordFormat"] ?? "Encrypted";

            switch (temp_format)
            {
                case "Hashed":
                    pPasswordFormat = MembershipPasswordFormat.Hashed;
                    break;
                case "Encrypted":
                    pPasswordFormat = MembershipPasswordFormat.Encrypted;
                    break;
                case "Clear":
                    pPasswordFormat = MembershipPasswordFormat.Clear;
                    break;
                default:
                    throw new Exception("Password format not supported.");
            }
            
            ConnectionStringSettings ConnectionStringSettings =
                       ConfigurationManager.ConnectionStrings[config["connectionStringName"]];

            if (ConnectionStringSettings == null || ConnectionStringSettings.ConnectionString.Trim() == "")
            {
                throw new ProviderException("Connection string cannot be blank.");
            }

            connectionString = ConnectionStringSettings.ConnectionString;
        }

        /// <summary>
        /// A helper function to retrieve config values from the configuration file.
        /// </summary>
        /// <param name="configValue"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        private string GetConfigValue(string configValue, string defaultValue)
        {
            return String.IsNullOrEmpty(configValue) ? defaultValue : configValue;
        }

        #endregion

    }
}
