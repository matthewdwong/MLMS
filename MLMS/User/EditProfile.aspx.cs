using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using MLMS.Common;
using MLMS.Data;
using MLMS.Objects;
using MLMS.Service;

namespace MLMS.User
{
    public partial class EditProfile : System.Web.UI.Page
    {
        public int UserID
        {
            get
            {
                int userID = 0;
                userID = (ViewState["UserID"] != null) ? (int)ViewState["UserID"] : 0;
                return userID;
            }
            set { ViewState["UserID"] = value; }
        }

        public UserProfile CurrentUserProfile
        {
            get;
            set;
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void bcUserProfile_LoadUserControl(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    Host host = (Host)SessionHelper.Host;

                    string userName = User.Identity.Name;

                    UserProfile user = UserService.GetUser(userName, SessionHelper.HostID);
                    CurrentUserProfile = user;

                    UserID = user.UserID;

                    txtFirstName.Text = user.FirstName;
                    txtLastName.Text = user.LastName;
                    UserName.Text = user.UserName;
                    txtCity.Text = user.City;
                    rlAge.SelectedValue = user.Age;
                    rlGender.SelectedValue = user.Gender;
                    rlExperience.SelectedValue = user.Experience;

                    if (user.PositionsHeld != null)
                    {
                        string[] positions = user.PositionsHeld.Split(',');
                        foreach (string position in positions)
                        {
                            foreach (ListItem held in clPositions.Items)
                            {
                                if (held.Text == position)
                                    held.Selected = true;
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.Write(ex, @"ProfileManagement.aspx.cs");
                throw;
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!Page.IsPostBack)
            {
                ddlCountry.SelectedValue = CurrentUserProfile.Country;
            }

        }

        /// <summary>
        /// Handles clicking the reset password button
        /// </summary>
        /// <param name="sender">The invoking object</param>
        /// <param name="e">Event Arguments</param>
        protected void btnChangePassword_ButtonClick(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Handles clicking the cancel password button
        /// </summary>
        /// <param name="sender">The invoking object</param>
        /// <param name="e">Event Arguments</param>
        protected void btnCancel_ButtonClick(object sender, EventArgs e)
        {
            Response.Redirect(@"LearnerDashboard.aspx");
        }

        protected void valUsername_ServerValidation(object source, ServerValidateEventArgs args)
        {
            bool isValid = false;
            string username = args.Value;
            UserProfile user = UserService.GetUser(username, SessionHelper.HostID);
            if (user.UserID > 0)
            {
                isValid = (user.UserID == UserID); //Same User
            }
            else //username does not exists
            {
                isValid = true;
            }
            args.IsValid = isValid;

        }

        /// <summary>
        /// Handles the save button click
        /// </summary>
        /// <param name="sender">The invoking object</param>
        /// <param name="e">Event Arguments</param>
        protected void btnSave_ButtonClick(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
                    TextInfo textInfo = cultureInfo.TextInfo;

                    string originalUserName = User.Identity.Name;
                    string warning = string.Empty;

                    UserProfile user = UserService.GetUser(originalUserName, SessionHelper.HostID);
                    user.HostID = SessionHelper.HostID;
                    user.FirstName = textInfo.ToTitleCase(txtFirstName.Text);
                    user.LastName = textInfo.ToTitleCase(txtLastName.Text);
                    user.UserName = UserName.Text;
                    user.City = textInfo.ToTitleCase(txtCity.Text);
                    user.OriginalUserName = originalUserName;
                    user.Age = string.Empty;
                    if (rlAge.SelectedValue != string.Empty)
                    {
                        user.Age = DatabaseHelper.GetString(rlAge.SelectedItem.Value);
                    }
                    user.Experience = rlExperience.SelectedValue;
                    user.Gender = rlGender.SelectedValue;

                    string positionsHeld = string.Empty;

                    foreach (ListItem position in clPositions.Items)
                    {
                        if (position.Selected)
                        {
                            positionsHeld += position.Text + ',';
                        }
                    }
                    if (positionsHeld != string.Empty)
                    {
                        positionsHeld = positionsHeld.Substring(0, positionsHeld.Length - 1);
                    }


                    user.PositionsHeld = positionsHeld;

                    user.Email = UserName.Text;

                    if (UserName.Text != originalUserName)
                    {
                        string password = Membership.Provider.GetPassword(originalUserName, String.Empty);

                        FormsAuthentication.SignOut();
                        FormsAuthentication.Authenticate(UserName.Text, password);

                        Membership.ValidateUser(UserName.Text, password);

                        warning = "Email was changed";
                    }

                    user.ProfilePicture = UploadPicture();

                    UserService.UpdateUser(user);

                    lblConfirmation.Text = "Save Successful" + warning;
                    lblConfirmation.ForeColor = Color.Blue;
                    lblConfirmation.Visible = true;
                }
                catch (Exception ex)
                {
                    ExceptionHandler.Write(ex, @"ProfileManagement.aspx.cs");
                    lblConfirmation.Text = "An error has occured while saving";
                    lblConfirmation.ForeColor = Color.Red;
                    lblConfirmation.Visible = true;

                    throw;
                }
            }
            else
            {
                lblConfirmation.Visible = false;
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

        private string UploadPicture()
        {
            string ProfilePictureImgURL;
            try
            {
                //get the file name of the posted image
                string imgName = filUpload.FileName;

                //sets directory
                string dir = "Images/" + UserID + "/Original/";
                if (!Directory.Exists(Server.MapPath(dir)))
                    Directory.CreateDirectory(Server.MapPath(dir));

                //sets the image path
                string imgPath = dir + imgName;

                //get the size in bytes that
                int imgSize = filUpload.PostedFile.ContentLength;

                //validates the posted file before saving
                if (filUpload.PostedFile != null && filUpload.PostedFile.FileName != "")
                {

                    //// 10240 KB means 10MB, You can change the value based on your requirement
                    //if (filUpload.PostedFile.ContentLength > 30960)
                    //    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Alert", "alert('File is too big.')", true);
                    //else
                    //{
                    //then save it to the Fo1lder

                    filUpload.SaveAs(Server.MapPath(imgPath));

                    //Create Thumbnail and save it - First checks dir 
                    string dirThumb = "Images/" + UserID + "/Thumbs/";
                    if (!Directory.Exists(Server.MapPath(dirThumb)))
                        Directory.CreateDirectory(Server.MapPath(dirThumb));

                    //sets image thumb path
                    string imgThumbPath = dirThumb + imgName;
                    Stream imgStream = filUpload.PostedFile.InputStream;
                    Bitmap bmThumb = new Bitmap(imgStream);
                    System.Drawing.Image im = bmThumb.GetThumbnailImage(100, 100, null, IntPtr.Zero);
                    im.Save(Server.MapPath(imgThumbPath));

                    imgProfilePic.ImageUrl = "~/User/" + imgThumbPath;
                    //hdThumbURL.Value = "User/" + dirThumb;
                    //hdPicURL.Value = "User/" + dir;
                    //hdFileName.Value = imgName;

                    ProfilePictureImgURL = "~/User/" + imgThumbPath;
                    return ProfilePictureImgURL;
                }
            }
            catch (Exception ex)
            {
                    ExceptionHandlerDataAccess.Write(ex, @"PostedPicture.aspx.cs", SessionHelper.HostID);
            }

            return "";
        }
    }
}