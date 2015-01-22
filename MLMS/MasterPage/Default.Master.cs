using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MLMS.Common;
using MLMS.Service;
using MLMS.Objects;

namespace MLMS.MasterPage
{
    public partial class Default : System.Web.UI.MasterPage
    {
        public UserProfile User
        {
            get { return (UserProfile)ViewState["User"]; }
            set { ViewState["User"] = value; }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (Request.UserAgent.IndexOf("AppleWebKit") > 0)
            {
                Request.Browser.Adapters.Clear();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.IsAuthenticated)
                {
                    User = UserService.GetUser(Page.User.Identity.Name, SessionHelper.HostID);
                    if (User != null)
                    {
                        lnkName.Text = User.FirstName + ' ' + User.LastName;
                        lnkName.Enabled = true;
                        lnkLogin.Visible = false;
                        mnuMain.Visible = true;
                    }
                }
            }
        }

        protected void LoginStatus1_LoggedOut(Object sender, EventArgs e)
        {
            lnkLogin.Visible = true;
            SessionHelper.ClearSession();

            Session.Abandon();
            System.Web.Security.FormsAuthentication.SignOut();
            Response.Redirect("~/Default.aspx");
            Response.End();
        }

        protected void lnkLogin_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Default.aspx");
        }
    }
}