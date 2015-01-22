using System;
using System.Web;

namespace MLMS.Common
{
    public class SessionHelper
    {
        #region Host Sessions

        public static int HostID
        {
            get
            {
                int hostID;
                try
                {
                    hostID = (int)HttpContext.Current.Session["HostID"];
                }
                catch (Exception)
                {
                    hostID = 0;
                }

                return hostID;
            }
            set
            {
                HttpContext.Current.Session["HostID"] = value;
            }
        }

        public static string CultureCode
        {
            get
            {
                string cultureCode;
                try
                {
                    cultureCode = (string)HttpContext.Current.Session["CultureCode"];
                }
                catch (Exception)
                {
                    cultureCode = null;
                }

                return cultureCode;
            }
            set
            {
                HttpContext.Current.Session["CultureCode"] = value;
            }
        }

        public static Object Host
        {
            get
            {
                Object host;
                try
                {
                    host = HttpContext.Current.Session["Host"];
                }
                catch (Exception)
                {
                    host = null;
                }
                return host;
            }
            set
            {
                HttpContext.Current.Session["Host"] = value;
            }
        }
             
        private static string Theme
        {
            get
            {
                string theme;
                try
                {
                    theme = (string)HttpContext.Current.Session["Theme"];
                }
                catch (Exception)
                {
                    theme = String.Empty;
                }
                return theme;
            }
            set
            {
                HttpContext.Current.Session["Theme"] = value;
            }
        }
        /// <summary>
        /// [Public] Gets or sets SelectedUserName
        /// </summary>
        public static string SelectedUserName
        {
            get
            {
                string selectedUserName;
                try
                {
                    selectedUserName = (string)HttpContext.Current.Session["SelectedUserName"];
                }
                catch (Exception)
                {
                    selectedUserName = String.Empty;
                }
                return selectedUserName;
            }
            set
            {
                HttpContext.Current.Session["SelectedUserName"] = value;
            }
        }
        /// <summary>
        /// [Public] Gets or sets SelectedCourseID
        /// </summary>
        public static int SelectedCourseID
        {
            get
            {
                int selectedCourseID;
                try
                {
                    selectedCourseID = (int)HttpContext.Current.Session["SelectedCourseID"];

                }
                catch (Exception)
                {
                    selectedCourseID = 0;
                }

                return selectedCourseID;
            }
            set
            {
                HttpContext.Current.Session["SelectedCourseID"] = value;
            }
        }

        public static int UserID
        {
            get
            {
                int UserID;
                try
                {
                    UserID = (int)HttpContext.Current.Session["UserID"];
                }
                catch (Exception)
                {
                    UserID = 0;
                }

                return UserID;
            }
            set
            {
                HttpContext.Current.Session["UserID"] = value;
            }
        }

        #endregion Host Sessions

        #region Session Helper

        public static void ClearSession()
        {
            HttpContext.Current.Session["ShoppingCart"] = null;
        }

        #endregion Session Helper

    }
}
