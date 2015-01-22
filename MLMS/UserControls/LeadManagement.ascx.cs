using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MLMS.Objects;
using MLMS.Service;

namespace MLMS.UserControls
{
    public partial class LeadManagement : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindInfo();
            }
        }

        protected void BindInfo()
        {
            ddlCallBackTOD.DataSource = MemberService.GetPreferredCallBack();
            ddlCallBackTOD.DataTextField = "PreferredValue";
            ddlCallBackTOD.DataValueField = "PreferredCallBackID";
            ddlCallBackTOD.DataBind();

            ddlHearAbout.DataSource = MemberService.GetHearAbout(Page.User.Identity.Name);
            ddlHearAbout.DataTextField = "HowDidYouHear";
            ddlHearAbout.DataValueField = "HowDidYouHearId";
            ddlHearAbout.DataBind();

            chkBxList.DataSource = MemberService.GetUserAchieveList(Page.User.Identity.Name);
            chkBxList.DataTextField = "AchieveName";
            chkBxList.DataValueField = "AchieveID";
            chkBxList.DataBind();

        }

        protected void btnCreateLead_Click(object sender, EventArgs e)
        {
            Member Lead = new Member();
            Lead.FirstName = txtBxFirstName.Text;
            Lead.LastName = txtBxLastName.Text;
            Lead.Adult = chkBxAdult.Checked;
            Lead.PGFirstName = txtBxPGFirstName.Text;
            Lead.PGLastName = txtBxPGLastName.Text;
            if (txtBxDOB.Text.Trim().Length > 0) Lead.DOB = Convert.ToDateTime(txtBxDOB.Text);
            else Lead.DOB = null;
            Lead.PrimaryNumber = txtBxPrimNumb.Text;
            Lead.SecondaryNumber = txtBxSecNumb.Text;
            Lead.PreferredCallBackID = Convert.ToInt32(ddlCallBackTOD.SelectedValue);
            Lead.EmailAddress = txtBxEmailAddress.Text;
            Lead.HowDidYouHearID = Convert.ToInt32(ddlHearAbout.SelectedValue);
            Lead.Notes = txtBxNotes.Text;
            Lead.MemberTypeID = Convert.ToInt32(MemberType.Lead);

            int LeadId = MemberService.InsertMember(Lead, Page.User.Identity.Name);


            lblMsg.Text = "Lead was added.";

            ResetAll();
        }

        protected void ResetAll()
        {
            txtBxFirstName.Text = string.Empty;
            txtBxLastName.Text = string.Empty;
            chkBxAdult.Checked = false;
            txtBxPGFirstName.Text = string.Empty;
            txtBxPGLastName.Text = string.Empty;
            txtBxDOB.Text = string.Empty;
            txtBxPrimNumb.Text = string.Empty;
            txtBxSecNumb.Text = string.Empty;
            ddlCallBackTOD.SelectedIndex = -1;
            txtBxCallBackDate.Text = string.Empty;
            txtBxEmailAddress.Text = string.Empty;
            txtBxIntroMeetDate.Text = string.Empty;
            ddlHearAbout.SelectedIndex = -1;
            txtBxNotes.Text = string.Empty;
        }
    }
}