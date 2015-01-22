using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MLMS.Common;
using MLMS.Objects;
using MLMS.Service;

namespace MLMS.User
{
    public partial class Members : System.Web.UI.Page
    {
        private List<Member> LeadList
        {
            get { return (List<Member>)ViewState["LeadList"]; }
            set { ViewState["LeadList"] = value; }
        }

        private string PageSelection
        {
            get { return (string)ViewState["PageSelection"]; }
            set { ViewState["PageSelection"] = value; }
        }

        protected void Page_PreInit(object sender, EventArgs e)
        {
            this.Page.Theme = "bdsu";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

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

                Dictionary<string, string> timeRange = TimeRange._dict;
                ddlStart.DataSource = timeRange;
                ddlStart.DataTextField = "Key";
                ddlStart.DataValueField = "Value";
                ddlStart.DataBind();

                ddlEnd.DataSource = timeRange;
                ddlEnd.DataTextField = "Key";
                ddlEnd.DataValueField = "Value";
                ddlEnd.DataBind();

                //ddlMeetingType.DataSource = CalendarService.GetMeetingType();
                //ddlMeetingType.DataTextField = "Type";
                //ddlMeetingType.DataValueField = "EventTypeID";
                //ddlMeetingType.DataBind();

                cblAchieve.DataSource = MemberService.GetUserAchieveList(Page.User.Identity.Name);
                cblAchieve.DataTextField = "AchieveName";
                cblAchieve.DataValueField = "AchieveID";
                cblAchieve.DataBind();

                ddlCl.DataSource = AdminService.GetUserCheckList(Page.User.Identity.Name);
                ddlCl.DataTextField = "ChecklistName";
                ddlCl.DataValueField = "UserChecklistID";
                ddlCl.DataBind();

                ddlCl.Items.Insert(0, new ListItem("-- Select a Checklist --", "0"));

                //BindDummyRow();

                var MemberID = Request.QueryString["MemberID"];
                if (MemberID != null)
                {
                    hdMemberID.Value = MemberID;
                    Page.ClientScript.RegisterStartupScript(GetType(), "LoadCallLog", "LoadMember();", true);
                }
                else if (Request.QueryString["MemberType"] != null)
                {
                    hdMemberTypeID.Value = Request.QueryString["MemberType"];
                    tblSearch.Attributes.Add("style", "display:none");
                }
            }

            foreach (ListItem li in cblAchieve.Items)
            {
                li.Attributes.Add("mainValue", li.Value);
            }
        }

        //private void BindDummyRow()
        //{
        //    DataTable dummy = new DataTable();
        //    dummy.Columns.Add("StartDate");
        //    dummy.Columns.Add("EndDate");
        //    dummy.Columns.Add("EventName");
        //    dummy.Columns.Add("EventDescription");
        //    dummy.Columns.Add("AllDayEvent");
        //    dummy.Columns.Add("EventType");
        //    dummy.Rows.Add();
        //    grdEvents.DataSource = dummy;
        //    grdEvents.DataBind();
        //}

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearItems();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (hdMemberID.Value.Trim().Length > 0)
            {
                MemberService.DeleteMember(Convert.ToInt32(hdMemberID.Value));
                ClearItems();
            }
        }

        protected void ClearItems()
        {
            txtBxFirstName.Text = string.Empty;
            txtBxLastName.Text = string.Empty;
            chkBxAdult.Checked = false;
            txtBxPGFName.Text = string.Empty;
            txtBxPGLName.Text = string.Empty;
            txtBxMDOB.Text = string.Empty;
            txtBxMPrimNumb.Text = string.Empty;
            txtBxMSecNumb.Text = string.Empty;
            ddlPreferCallBack.SelectedIndex = -1;
            txtBxEmailAdd.Text = string.Empty;
            ddlHowDidYouHear.SelectedIndex = -1;
            txtBxNotes.Text = string.Empty;
            txtBxSearchStartDate.Text = string.Empty;
            txtBxSearchEndDate.Text = string.Empty;
            hdMemberID.Value = string.Empty;
            hdCEID.Value = string.Empty;

            foreach (ListItem li in cblAchieve.Items) li.Selected = false;

            ddlCl.SelectedIndex = -1;
            cblChecklist.Items.Clear();
        }

        protected void ddlStart_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlEnd.SelectedIndex = ddlStart.SelectedIndex + 1;
            MPE.Show();
        }

        protected void ddlCL_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (hdMemberID.Value.Trim().Length > 0)
            {
                List<UserChecklistOption> uclo = new List<UserChecklistOption>();

                foreach (ListItem li in cblChecklist.Items)
                {
                    UserChecklistOption option = new UserChecklistOption();
                    option.UserChecklistOptionID = Convert.ToInt32(li.Value);
                    option.CheckedOption = li.Selected;
                    uclo.Add(option);
                }

                if (uclo.Count > 0)
                    MemberService.UpdateUserCheckListOption(Convert.ToInt32(hdMemberID.Value), uclo);
            }

            cblChecklist.DataSource = MemberService.GetUserChecklistOptions(Convert.ToInt32(ddlCl.SelectedValue));
            cblChecklist.DataTextField = "OptionName";
            cblChecklist.DataValueField = "UserChecklistOptionID";
            cblChecklist.DataBind();

            DataTable memberChecklistOptions = new DataTable();
            if(hdMemberID.Value.Trim().Length > 0)
                memberChecklistOptions = MemberService.GetMemberChecklist(Convert.ToInt32(hdMemberID.Value));

            foreach (ListItem li in cblChecklist.Items)
            {
                li.Attributes.Add("mainValue", li.Value);
            }

            if (memberChecklistOptions != null)
            {
                foreach (DataRow dr in memberChecklistOptions.Rows)
                {
                    string UserCheckListOptionID = dr["UserCheckListOptionID"].ToString();
                    ListItem li = cblChecklist.Items.FindByValue(UserCheckListOptionID);
                    if (li != null)
                        li.Selected = true;
                }
            }

            upCbl.Update();
        }
    }
}