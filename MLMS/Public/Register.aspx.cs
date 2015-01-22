
namespace MLMS.Public
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Globalization;
    using System.Threading;
    using System.Web;
    using System.Web.Security;
    using System.Web.UI.WebControls;
    using System.Web.UI;

    using MLMS.Common;
    using MLMS.Objects;
    using MLMS.Service;

    /// <summary>
    /// Register a new User class
    /// </summary>
    public partial class Register : System.Web.UI.Page
    {
        #region Properties
        private string password;

        #endregion

        #region Events
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (Page.IsPostBack) ///re-select the state because options are built client-side
            {

                //CreateUserWizardStep wizAccountInfo = (CreateUserWizardStep)ctrlCreateUser.FindControl("wizAccountInfo");
                //var hidState = (HiddenField)wizAccountInfo.ContentTemplateContainer.FindControl("hidState");

                //SelectState(hidState.Value, null);

            }
        }

        /// <summary>
        /// Handles the page load event
        /// </summary>
        /// <param name="sender">the sender</param>
        /// <param name="e">Event arguments</param>
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// The event of loading the bcNewUser control
        /// </summary>
        /// <param name="sender">the sender</param>
        /// <param name="e">Event arguments</param>
        protected void bcNewUser_OnLoad(object sender, EventArgs e)
        {
            try
            {
                Host host = (Host)SessionHelper.Host;

                if (host.HostID == 0)
                    Response.Redirect("~/PublicPages/Error.aspx");

                SetRequiredFields(host.Fields);

                //CreateUserWizard ctrlCreateUser = (CreateUserWizard)bcNewUser.FindControl("ctrlCreateUser");

                CreateUserWizardStep wizAccountInfo = (CreateUserWizardStep)ctrlCreateUser.FindControl("wizAccountInfo");

                Literal litShareText = (Literal)wizAccountInfo.ContentTemplateContainer.FindControl("litShareText");
                string sharedata = host.ShareDataWithPartnerText;
                litShareText.Text = (String)GetLocalResourceObject("locShareDataPart.text");

                Literal litShareHelpText = (Literal)wizAccountInfo.ContentTemplateContainer.FindControl("litShareHelpText");
                string sharedataHelp = host.ShareDataWithPartnerHelpText;
                litShareHelpText.Text = (String)GetLocalResourceObject("locShareDataPartHelp.text");

                Label lblPartnerEmail = (Label)wizAccountInfo.ContentTemplateContainer.FindControl("lblPartnerEmail");
                string addEmail = host.AdditionalEmailText;
                lblPartnerEmail.Text = (String)GetLocalResourceObject("locAddEmail.text");

                Literal litPromoEmail = (Literal)wizAccountInfo.ContentTemplateContainer.FindControl("litPromoEmail");
                string recEmail = host.ReceivePromoEmailText;
                litPromoEmail.Text = (String)GetLocalResourceObject("locRecEmail.text");

                DropDownList ddlCountry = (DropDownList)wizAccountInfo.ContentTemplateContainer.FindControl("ddlCountry");
                ClientScript.RegisterStartupScript(this.GetType(), "initCountry", string.Format(@"initCountry('{0}', '{1}');", ddlCountry.ClientID, null), true);

                var nemail = Request.QueryString["nemail"];
                if (nemail != null)
                {
                    TextBox txtPartnerEmail = (TextBox)wizAccountInfo.ContentTemplateContainer.FindControl("txtPartnerEmail");
                    txtPartnerEmail.Text = nemail;
                    txtPartnerEmail.Enabled = false;
                    TextBox Email = (TextBox)wizAccountInfo.ContentTemplateContainer.FindControl("Email");
                    Email.Text = nemail;
                    Email.Enabled = false;
                    TextBox UserName = (TextBox)wizAccountInfo.ContentTemplateContainer.FindControl("UserName");
                    UserName.Text = nemail;
                    UserName.Enabled = false;
                    ctrlCreateUser.UserName = nemail;
                    ctrlCreateUser.Email = nemail;

                }

                //ContentManagementData cmdData = ContentManagementService.GetContent("PrivacyPolicy", "Register.aspx", SessionHelper.CultureCode, SessionHelper.HostID);
                Literal litPrivacy = (Literal)wizAccountInfo.ContentTemplateContainer.FindControl("litPrivacy");
                //if (cmdData != null)
                //    litPrivacy.Text = cmdData.PageContent;
            }
            catch (Exception ex)
            {
                ExceptionHandler.Write(ex, @"Register.aspx.cs");
                throw;
            }
        }

        protected void bcManagement_OnLoad(object sender, EventArgs e)
        {
            //ContentManagementData cmdRight = ContentManagementService.GetContent("RegistrationHelpText", "Register.aspx", SessionHelper.CultureCode, SessionHelper.HostID);
            //if (cmdRight != null)
            //{
            //    bcManagement.ControlTitle = cmdRight.Title;
            //    var literalControlRight = new LiteralControl();
            //    literalControlRight.Text = cmdRight.PageContent;
            //    bcManagement.AddPlaceHolderControl(literalControlRight);
            //}
            //else
            //{
            //    bcManagement.Visible = false;
            //}
        }

        /// <summary>
        /// Handles changes in User's selection to choose 
        /// to share data with partners or not.
        /// </summary>
        /// <param name="sender">the sender</param>
        /// <param name="e">Event arguments</param>
        protected void rlShareData_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var wizAccountInfo = (CreateUserWizardStep)ctrlCreateUser.FindControl("wizAccountInfo");
                RadioButtonList rlShareData = (RadioButtonList)wizAccountInfo.ContentTemplateContainer.FindControl("rlShareData");
                Label lblPartnerEmail = (Label)wizAccountInfo.ContentTemplateContainer.FindControl("lblPartnerEmail");
                TextBox txtPartnerEmail = (TextBox)wizAccountInfo.ContentTemplateContainer.FindControl("txtPartnerEmail");
                RequiredFieldValidator regFldValPartnerEmail =
                        (RequiredFieldValidator)wizAccountInfo.ContentTemplateContainer.FindControl("regFldValPartnerEmail");

                if (rlShareData.SelectedIndex == 0)
                {
                    lblPartnerEmail.Visible = true;
                    txtPartnerEmail.Visible = true;
                    regFldValPartnerEmail.Enabled = true;
                }
                else
                {
                    lblPartnerEmail.Visible = false;
                    txtPartnerEmail.Visible = false;
                    regFldValPartnerEmail.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.Write(ex, @"Register.aspx.cs");
                throw;
            }
        }

        /// <summary>
        /// Validates if User chooses to share data with business partner
        /// that they provide their email at the business partner
        /// </summary>
        /// <param name="source">Invoking object</param>
        /// <param name="args">Server Validation Event arguments</param>
        protected void vldPartnerEmail_ServerValidate(object source, ServerValidateEventArgs args)
        {

            var wizAccountInfo = (CreateUserWizardStep)ctrlCreateUser.FindControl("wizAccountInfo");
            RadioButtonList rlShareData = (RadioButtonList)wizAccountInfo.ContentTemplateContainer.FindControl("rlShareData");
            TextBox txtPartnerEmail = (TextBox)wizAccountInfo.ContentTemplateContainer.FindControl("txtPartnerEmail");

            if (rlShareData.SelectedValue == "true")
            {
                if (txtPartnerEmail.Text == string.Empty)
                {
                    args.IsValid = false;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ctrlCreateUser_CreatingUser(object sender, EventArgs e)
        {
            if (ctrlCreateUser.UserName == User.Identity.Name)
            {
                // the User is already logged in
                UserProfile profile = GetUserProfile();
                UserService.UpdateUser(profile);

                // update password
                MembershipUser loggedInUser = Membership.GetUser(ctrlCreateUser.UserName);
                //MembershipUser loggedInUser = Membership.GetUser();
                string oldPassword = loggedInUser.GetPassword();
                loggedInUser.ChangePassword(oldPassword, password);

                RedirectToLearnerDashboard();
            }
        }

        private void RedirectToLearnerDashboard()
        {
            Response.Redirect("~/Default.aspx");
        }

        private void CreateCookie(string username)
        {
            double expiresIn = double.Parse(ConfigurationManager.AppSettings["CookieExpiresIn"]);

            // Create the authentication ticket
            FormsAuthenticationTicket authTicket = new
                                        FormsAuthenticationTicket(1,        // version                     
                                             username,           // User  name                                              
                                             DateTime.Now,                  // creation
                                             DateTime.Now.AddMinutes(expiresIn),//Expiration                                                
                                             true,                         // Persistent                                               
                                             SessionHelper.HostID.ToString());           // User   data


            // Now encrypt the ticket.
            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
            // Create a cookie and add the encrypted ticket to the cookie as data.
            HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

            // Add the cookie to the outgoing cookies collection. 
            Response.Cookies.Add(authCookie);
        }

        /// <summary>
        /// Handles the event of creating a new User
        /// This method will associate the newly created User
        /// with the Learner role as a default. The role can be changed
        /// in another admin screen.
        /// </summary>
        /// <param name="sender">the sender object</param>
        /// <param name="e">the event arguments</param>
        protected void ctrlCreateUser_CreatedUser(object sender, EventArgs e)
        {

            UserProfile profile = GetUserProfile();
            UserService.UpdateUser(profile);

            //SecurityService.InsertUserRole(ctrlCreateUser.UserName, "LNR");  //Defalt insert as Learner

            //// At the end of the User
            //// creation send an email.

            //// Prepare the email message
            string emailFromAddress = ((Host)(SessionHelper.Host)).DummyFromEmail;
            string smtpPassword = ConfigurationManager.AppSettings["SmtpPassword"];
            string smtpUserName = ConfigurationManager.AppSettings["SmtpUserName"];

            string[] toAddress = { profile.Email };

            ////EmailMessage msg = new EmailMessage();
            //EmailMessage msg = EmailService.GetEmailContent(EmailEventTitles.SiteUserRegistration, SessionHelper.HostID);
            //if (msg.EmailBody != null)
            //{
            //    msg.AttachmentLink = @"<a href='http://bdsmktg.com'>Visit BDS MKTG!</a>";
            //    msg.FromAddress = emailFromAddress;
            //    msg.IsHtml = true;
            //    msg.IsSslEnabled = false;
            //    msg.SmtpHost = ConfigurationManager.AppSettings["SmtpHost"];
            //    msg.SmtpPassword = smtpPassword;
            //    msg.SmtpUserName = smtpUserName;
            //    msg.ToAddress = toAddress;

            //    //// TODO: extremely duct-tape-ish solution
            //    //// Have to find a solid way to enter all these emails in
            //    //// the database and replace the links with real values 
            //    msg.EmailBody = msg.EmailBody.Replace("contact_us", Helper.BaseSiteUrl(HttpContext.Current) + @"PublicPages/ContactUs.aspx");
            //    EmailService.SendEmailNotification(msg);
            //}

            CreateCookie(profile.UserName);

            Page.ClientScript.RegisterStartupScript(GetType(), "hidePopup", "function pageLoad(){$find('popUp').hidePopup();}", true);
        }

        /// <summary>
        /// Validates whether the User agrees to the privacy agreement
        /// </summary>
        /// <param name="source">Invoking object</param>
        /// <param name="args">Server Validation Event arguments</param>
        protected void vldPrivacyAgreement_ServerValidate(object source, ServerValidateEventArgs args)
        {
            //bool isStateRequired = false;

            //RequiredField state = ((Host)SessionHelper.Host).Fields.Find(delegate(RequiredField f) { return f.FieldName == "State"; });
            //if (state != null && state.IsRequired)
            //    isStateRequired = true;

            //HiddenField hidState = (HiddenField)wizAccountInfo.ContentTemplateContainer.FindControl("hidState");
            //CheckBox chkPrivacyAgreement = (CheckBox)wizAccountInfo.ContentTemplateContainer.FindControl("chkPrivacyAgreement");
            //if (!chkPrivacyAgreement.Checked || (isStateRequired && hidState.Value == ""))
            //{
            //    args.IsValid = false;
            //}
        }

        protected void valPhone_ServerValidation(object source, ServerValidateEventArgs args)
        {

            var txtPhoneArea = (TextBox)wizAccountInfo.ContentTemplateContainer.FindControl("txtPhoneArea");
            var txtPhoneNum1 = (TextBox)wizAccountInfo.ContentTemplateContainer.FindControl("txtPhoneNum1");
            var txtPhoneNum2 = (TextBox)wizAccountInfo.ContentTemplateContainer.FindControl("txtPhoneNum2");

            string value = txtPhoneArea.Text + txtPhoneNum1.Text + txtPhoneNum2.Text;

            args.IsValid = ValidatePhone("Phone", value);
        }

        protected void valCellPhone_ServerValidation(object source, ServerValidateEventArgs args)
        {
            var txtCellArea = (TextBox)wizAccountInfo.ContentTemplateContainer.FindControl("txtCellPhoneArea");
            var txtCellNum1 = (TextBox)wizAccountInfo.ContentTemplateContainer.FindControl("txtCellPhoneNum1");
            var txtCellNum2 = (TextBox)wizAccountInfo.ContentTemplateContainer.FindControl("txtCellPhoneNum2");
            string value = txtCellArea.Text + txtCellNum1.Text + txtCellNum2.Text;

            args.IsValid = ValidatePhone("CellPhone", value);
        }

        /// <summary>
        /// Validates whether the User selected a state
        /// </summary>
        /// <param name="source">Invoking object</param>
        /// <param name="args">Server Validation Event arguments</param>
        protected void vldState_ServerValidate(object source, ServerValidateEventArgs args)
        {
            HiddenField hidState = (HiddenField)wizAccountInfo.ContentTemplateContainer.FindControl("hidState");
            if (hidState.Value == "")
            {
                args.IsValid = false;
            }
        }

        protected void ctrlCreateUser_ContinueButtonClick(object sender, EventArgs e)
        {
            RedirectToLearnerDashboard();
        }

        #endregion

        #region Methods

        private UserProfile GetUserProfile()
        {
            Host host = (Host)SessionHelper.Host;

            //// Get all the controls from the UC

            var wizAccountInfo = (CreateUserWizardStep)ctrlCreateUser.FindControl("wizAccountInfo");
            var wizDemographics = (CompleteWizardStep)ctrlCreateUser.FindControl("wizDemographics");

            var txtFirstName = (TextBox)wizAccountInfo.ContentTemplateContainer.FindControl("txtFirstName");
            var txtLastName = (TextBox)wizAccountInfo.ContentTemplateContainer.FindControl("txtLastName");
            ////TextBox txtUserName = (TextBox)wizAccountInfo.ContentTemplateContainer.FindControl("UserName");
            var txtEmail = (TextBox)wizAccountInfo.ContentTemplateContainer.FindControl("Email");
            password = ((TextBox)wizAccountInfo.ContentTemplateContainer.FindControl("Password")).Text;
            ////TextBox txtConfirmPassword = (TextBox)wizAccountInfo.ContentTemplateContainer.FindControl("ConfirmPassword");
            var txtCity = (TextBox)wizAccountInfo.ContentTemplateContainer.FindControl("txtCity");
            var chkEmailMarketing = (CheckBox)wizAccountInfo.ContentTemplateContainer.FindControl("chkEmailMarketing");
            var txtPhoneArea = (TextBox)wizAccountInfo.ContentTemplateContainer.FindControl("txtPhoneArea");
            var txtPhoneNum1 = (TextBox)wizAccountInfo.ContentTemplateContainer.FindControl("txtPhoneNum1");
            var txtPhoneNum2 = (TextBox)wizAccountInfo.ContentTemplateContainer.FindControl("txtPhoneNum2");
            var txtCellArea = (TextBox)wizAccountInfo.ContentTemplateContainer.FindControl("txtCellPhoneArea");
            var txtCellNum1 = (TextBox)wizAccountInfo.ContentTemplateContainer.FindControl("txtCellPhoneNum1");
            var txtCellNum2 = (TextBox)wizAccountInfo.ContentTemplateContainer.FindControl("txtCellPhoneNum2");
            var txtPartnerEmail = (TextBox)wizAccountInfo.ContentTemplateContainer.FindControl("txtPartnerEmail");
            var cxBxTxtMsg = (CheckBox)wizAccountInfo.ContentTemplateContainer.FindControl("ckBxTxtMsg");
            var rlShareData = (RadioButtonList)wizAccountInfo.ContentTemplateContainer.FindControl("rlShareData");
            var ddlCountry = (DropDownList)wizAccountInfo.ContentTemplateContainer.FindControl("ddlCountry");
            //string[] languages = host.Languages.Split(',');

            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;

            //// Get the User object for the User that was just created
            MembershipUser newUser = Membership.GetUser(ctrlCreateUser.UserName);

            //// Hydrate the UserProfile object to be sent to the service
            var profile = new UserProfile();
            profile.UserID = Convert.ToInt32(newUser.ProviderUserKey);

            profile.Age = string.Empty;
            profile.City = textInfo.ToTitleCase(txtCity.Text);
            profile.Email = txtEmail.Text;
            profile.FirstName = textInfo.ToTitleCase(txtFirstName.Text);
            profile.Gender = "";
            profile.LastName = textInfo.ToTitleCase(txtLastName.Text);
            profile.ModifiedBy = newUser.UserName;
            string positionsHeld = string.Empty;

            if (ddlCountry.SelectedValue != string.Empty)
                profile.Country = ddlCountry.SelectedValue;
            profile.UserName = newUser.UserName;

           
            profile.CultureCode = "en-Us";
            profile.HostID = host.HostID;
            return profile;
        }

        private void SetRequiredFields(List<RequiredField> fields)
        {

            CreateUserWizardStep wizAccountInfo = (CreateUserWizardStep)ctrlCreateUser.FindControl("wizAccountInfo");
            CompleteWizardStep wizDemographics = (CompleteWizardStep)ctrlCreateUser.FindControl("wizDemographics");

            foreach (RequiredField field in fields)
            {
                string rowID = "tr" + field.FieldName;  //all rows are in labels w/ ID= 'tr' + FieldName
                TableRow row = (TableRow)wizAccountInfo.ContentTemplateContainer.FindControl(rowID);

                if (row == null) //if field not in Account Info, then check in Demographics
                    row = (TableRow)wizDemographics.ContentTemplateContainer.FindControl(rowID);

                if (row != null)
                {
                    row.Visible = field.IsActive;

                    if (!field.IsRequired)
                    {
                        //disable all validation contols for the row
                        List<BaseValidator> objValList = new List<BaseValidator>();
                        Helper.FindControl<BaseValidator>(row.Controls, ref objValList);
                        foreach (BaseValidator val in objValList)
                        {
                            if ((field.FieldName == "State" || field.FieldName == "Positions")
                                && val.GetType() == typeof(System.Web.UI.WebControls.CustomValidator))
                            {
                                val.Enabled = false;
                            }
                            else
                            {
                                if (val.GetType() == typeof(System.Web.UI.WebControls.RequiredFieldValidator))
                                    val.Enabled = false;
                            }
                        }

                        //hide asterisk
                        Label asterisk = (Label)row.FindControl("ast" + field.FieldName);  //all asterisks are in labels w/ ID= 'ast' + FieldName
                        if (asterisk != null)
                            asterisk.Visible = false;
                    }
                }
            }
        }

        private bool ValidatePhone(string fieldName, string value)
        {
            bool isValid = false;

            Host host = (Host)SessionHelper.Host;

            List<RequiredField> fields = host.Fields;
            RequiredField reqField = fields.Find(
                                   delegate(RequiredField requiredField)
                                   { return requiredField.FieldName == fieldName; }

                               );

            if (value.Length == 0 && !reqField.IsRequired)
            {
                isValid = true;
            }
            else
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(value,
                                                   @"^[2-9]\d{9}$"))
                {
                    isValid = true;
                }

            }

            return isValid;
        }

        private void SelectState(string state, string country)
        {

            CreateUserWizardStep wizAccountInfo = (CreateUserWizardStep)ctrlCreateUser.FindControl("wizAccountInfo");

            var ddlCountry = (DropDownList)wizAccountInfo.ContentTemplateContainer.FindControl("ddlCountry");

            if (country == string.Empty)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "country", string.Format(@"getCountryForState('{0}', '{1}');", ddlCountry.ClientID, state), true);
            }
            ClientScript.RegisterStartupScript(this.GetType(), "state", string.Format(@"updateState('{0}', '{1}');", ddlCountry.ClientID, state), true);
        }

        #endregion

    }
}