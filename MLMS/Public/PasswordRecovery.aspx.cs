namespace MLMS.Public
{
    using System;
    using System.Configuration;
    using System.Drawing;
    using System.Web;
    using System.Web.Security;
    using System.Web.UI;

    using Common;

    using Objects;

    using Service;

    /// <summary>
    /// The code behind for the PasswordRecovery page
    /// </summary>
    public partial class PasswordRecovery : System.Web.UI.Page
    {
        /// <summary>
        /// Handles the page load event
        /// </summary>
        /// <param name="sender">Invoking object</param>
        /// <param name="e">Event arguments</param>
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Gets the password from the membership database
        /// and emails it to the User
        /// </summary>
        /// <param name="sender">Invoking object</param>
        /// <param name="e">Event arguments</param>
        protected void btnResetPassword_Click(object sender, EventArgs e)
        {
            try
            {
                MembershipUser user = null;
                string userName = string.Empty;

                userName = Membership.GetUserNameByEmail(txtUserName.Text.Trim());

                if (userName != null)
                    user = Membership.GetUser(userName);

                if (user == null)
                {
                    //// display error message User does not exist
                    lblConfirmation.Text = (string)GetLocalResourceObject("locNotFound.text");
                    lblConfirmation.Visible = true;
                }
                else
                {
                    if (user.IsLockedOut)
                    {
                        user.UnlockUser();
                    }

                    string newPassword = user.GetPassword();
                    var userProfile = UserService.GetUser(userName,1);

                    //// Prepare the email message
                    //string emailFromAddress = host.DummyFromEmail;
                    //string smtpPassword = ConfigurationManager.AppSettings["SmtpPassword"];
                    //string smtpUserName = ConfigurationManager.AppSettings["SmtpUserName"];

                    
                    //string[] toAddress = { user.Email };
                   

                    //////EmailMessage msg = new EmailMessage();
                    //EmailMessage msg = EmailService.GetEmailContent(EmailEventTitles.PasswordReset, SessionHelper.HostID);
                    //msg.AttachmentLink = @"<a href='http://bdsmktg.com'>Visit BDS MKTG!</a>";
                    //msg.FromAddress = emailFromAddress;
                    //msg.IsHtml = true;
                    //msg.IsSslEnabled = false;
                    //msg.ToAddress = toAddress;

                    //msg.SmtpHost = ConfigurationManager.AppSettings["SmtpHost"];
                    //msg.SmtpPassword = smtpPassword;
                    //msg.SmtpUserName = smtpUserName;


                    ////// TODO: extremely duct-tape-ish solution
                    ////// Have to find a solid way to enter all these emails in
                    ////// the database and replace the links with real values 
                    //msg.EmailBody = msg.EmailBody.Replace("contact_us", Helper.BaseSiteUrl(HttpContext.Current) + @"PublicPages/ContactUs.aspx")
                    //    .Replace("first_name", userProfile.FirstName)
                    //    .Replace("user_name", userName)
                    //    .Replace("reset_password", newPassword);
                    //EmailService.SendEmailNotification(msg);

                    lblConfirmation.Text = (string)GetLocalResourceObject("locConfirm.text");
                    lblConfirmation.ForeColor = Color.Blue;
                    lblConfirmation.Visible = true;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.Write(ex, @"UserManagement.aspx.cs");
                throw;
            }
        }
    }
}
