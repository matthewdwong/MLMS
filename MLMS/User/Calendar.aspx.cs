using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MLMS.Service;
using MLMS.Objects;

namespace MLMS.User
{
    public partial class Calendar : System.Web.UI.Page
    {
        #region Properties

        private DataTable TimeTable
        {
            get { return (DataTable)ViewState["TimeTable"]; }
            set { ViewState["TimeTable"] = value; }
        }


        #endregion

        protected void Page_PreInit(object sender, EventArgs e)
        {
            this.Page.Theme = "bdsu";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Dictionary<string, string> timeRange = TimeRange._dict;
                ddlStart.DataSource = timeRange;
                ddlStart.DataTextField = "Key";
                ddlStart.DataValueField = "Value";
                ddlStart.DataBind();

                ddlEnd.DataSource = timeRange;
                ddlEnd.DataTextField = "Key";
                ddlEnd.DataValueField = "Value";
                ddlEnd.DataBind();

                ddlFollowUpTimeStart.DataSource = timeRange;
                ddlFollowUpTimeStart.DataTextField = "Key";
                ddlFollowUpTimeStart.DataValueField = "Value";
                ddlFollowUpTimeStart.DataBind();

                ddlFollowUpEndTime.DataSource = timeRange;
                ddlFollowUpEndTime.DataTextField = "Key";
                ddlFollowUpEndTime.DataValueField = "Value";
                ddlFollowUpEndTime.DataBind();

                ddlPreferCallBack.DataSource = MemberService.GetPreferredCallBack();
                ddlPreferCallBack.DataTextField = "PreferredValue";
                ddlPreferCallBack.DataValueField = "PreferredCallBackID";
                ddlPreferCallBack.DataBind();

                ddlPreferCallBack.Items.Insert(0, new ListItem("", null));

                ddlHowDidYouHear.DataSource = MemberService.GetHearAbout(Page.User.Identity.Name);
                ddlHowDidYouHear.DataTextField = "HowDidYouHear";
                ddlHowDidYouHear.DataValueField = "HowDidYouHearId";
                ddlHowDidYouHear.DataBind();

                ddlHowDidYouHear.Items.Insert(0, new ListItem("", null));

                ddlObjection.DataSource = MemberService.GetObjections(Page.User.Identity.Name);
                ddlObjection.DataTextField = "Objection";
                ddlObjection.DataValueField = "ObjectionId";
                ddlObjection.DataBind();

                //Sets Members List to be used in search query
                List<Member> Members = MemberService.GetMembersByUser(HttpContext.Current.User.Identity.Name, Convert.ToInt32(MemberType.Member));
                Session["MemberList"] = Members;
            }
        }

        protected void ddlStart_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlEnd.SelectedIndex = ddlStart.SelectedIndex + 1;

            KeepModalAlive();
            upPanel.Update();
        }

        protected void ddlFollowUpTimeStart_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlFollowUpEndTime.SelectedIndex = ddlFollowUpTimeStart.SelectedIndex + 1;

            KeepModalAlive();
            upPanel.Update();
        }

        protected void KeepModalAlive()
        {
            tblEventDetails.Enabled = true;

            if (hdMemberTypeID.Value == MemberType.Customer.ToString())
            {
                lblBecomeMember.Attributes.Add("style", "display:none");
                spanYes.Attributes.Add("style", "display:none");
                spanNo.Attributes.Add("style", "display:none");
                yesChk.Attributes.Add("style", "display:none");
                noChk.Attributes.Add("style", "display:none");
                ddlObjection.Attributes.Add("style", "display:none");
                ddlMemberTypeID.Attributes.Add("style", "display:none");
                FollowUpNotesSection.Attributes.Add("style", "display:none");
            }
            else
            {
                lblBecomeMember.Attributes.Remove("style");
                spanYes.Attributes.Remove("style");
                spanNo.Attributes.Remove("style");
                yesChk.Attributes.Remove("style");
                noChk.Attributes.Remove("style");
                yesChk.Disabled = false;
                noChk.Disabled = false;

                if (yesChk.Checked == true)
                {
                    yesChk.Disabled = false;
                    noChk.Disabled = true;
                    FollowUpNotesSection.Attributes.Remove("style");
                    ddlMemberTypeID.Attributes.Remove("style");
                }
                else if (noChk.Checked == true)
                {
                    ddlObjection.Attributes.Remove("style");
                    FollowUpNotesSection.Attributes.Remove("style");
                    yesChk.Disabled = true;
                    noChk.Disabled = false;
                }
            }

        }
    }
}