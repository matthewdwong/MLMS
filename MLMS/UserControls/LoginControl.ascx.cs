using System;
using System.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web;
using System.Web.UI.WebControls;

using MLMS.Common;
using MLMS.Objects;
using MLMS.Service;

namespace MLMS.UserControls
{
    public partial class LoginControl : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "showPopup", "function pageLoad(){$find('popUp').showPopup();}", true);
            }
        }

        protected void lgnControl_LoggedIn(object sender, EventArgs e)
        {
            double expiresIn = double.Parse(ConfigurationManager.AppSettings["CookieExpiresIn"]);
            if (chkRememberMe.Checked)
                expiresIn = 262974;
            // Create the authentication ticket
            var authTicket = new FormsAuthenticationTicket(
                                             1,                                 // version                     
                                             lgnControl.UserName,               // User name
                                             DateTime.Now,                      // creation
                                             DateTime.Now.AddMinutes(expiresIn),//Expiration
                                             true,                              // Persistent
                                             SessionHelper.HostID.ToString()    // User data
                                 );            

            // Now encrypt the ticket.
            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
            // Create a cookie and add the encrypted ticket to the cookie as data.
            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

            // Add the cookie to the outgoing cookies collection. 
            Response.Cookies.Add(authCookie);

            UserProfile user = UserService.GetUser(lgnControl.UserName, SessionHelper.HostID);
            SessionHelper.UserID = user.UserID;

            Response.Redirect("~/User/Leads.aspx");

        }

        protected void lgnControl_LoginError(object sender, EventArgs e)
        {
            try
            {
                MembershipUser user = Membership.GetUser(lgnControl.UserName);

                if (user == null) return;
                if (user.IsLockedOut)
                {
                    lgnControl.FailureText =
                        "Your account has been locked.  Please use the 'Forgot Password' link to email your password.";
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.Write(ex, @"LoginControl.ascx.cs");
            }
        }

        protected void lgnControl_OnInit(object sender, EventArgs e)
        {
            base.OnInit(e);
            

        }

    }
}