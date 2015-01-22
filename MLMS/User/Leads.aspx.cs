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
    public partial class Leads : System.Web.UI.Page
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
                LoadCallList();

                DataTable dtPreferredCallBack = MemberService.GetPreferredCallBack();
                ddlPreferCallBack.DataSource = dtPreferredCallBack;
                ddlPreferCallBack.DataTextField = "PreferredValue";
                ddlPreferCallBack.DataValueField = "PreferredCallBackID";
                ddlPreferCallBack.DataBind();

                ddlPreferCallBack.Items.Insert(0, new ListItem("", null));

                ddlPreferredCallBackSearch.DataSource = dtPreferredCallBack;
                ddlPreferredCallBackSearch.DataTextField = "PreferredValue";
                ddlPreferredCallBackSearch.DataValueField = "PreferredCallBackID";
                ddlPreferredCallBackSearch.DataBind();

                ddlPreferredCallBackSearch.Items.Insert(0, new ListItem("", null));

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

                cblAchieve.DataSource = MemberService.GetUserAchieveList(Page.User.Identity.Name);
                cblAchieve.DataTextField = "AchieveName";
                cblAchieve.DataValueField = "AchieveID";
                cblAchieve.DataBind();

                PageSelection = "ScheduleIntro";
            }
        }

        protected void LoadCallList()
        {
            LeadList = MemberService.GetLeadsToCall(Page.User.Identity.Name);
            grdCallList.DataSource = LeadList;
            grdCallList.DataBind();

            Page.ClientScript.RegisterStartupScript(GetType(), "HideLoading", "hideLoading();", true);
        }

        protected void hlSchIntro_Click(object sender, EventArgs e)
        {
            PageSelection = "ScheduleIntro";
            LeadList.Clear();
            LoadCallList();
        }

        protected void hlNeedConfirm_Click(object sender, EventArgs e)
        {
            PageSelection = "NeedConfirm";
            LeadList.Clear();
            LeadList = MemberService.GetUpcomingMeetingsToConfirm(Page.User.Identity.Name);
            grdCallList.DataSource = LeadList;
            grdCallList.DataBind();

            Page.ClientScript.RegisterStartupScript(GetType(), "HideLoading", "hideLoading();", true);
        }

        protected void grdCallList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Page")
            {
                GridViewRow gvr = (GridViewRow)(((Button)e.CommandSource).NamingContainer);


                int RowIndex = gvr.RowIndex;
                int MemberID = Convert.ToInt32(grdCallList.DataKeys[RowIndex].Values["MemberID"]);
                if (e.CommandName == "Select")
                {
                    Member member = new Member();
                    member = MemberService.GetLead(MemberID);

                    hdMemberID.Value = member.MemberID.ToString();
                    txtBxFirstName.Text = member.FirstName;
                    txtBxLastName.Text = member.LastName;
                    txtBxPGFName.Text = member.PGFirstName;
                    txtBxPGLName.Text = member.PGLastName;
                    chkBxAdult.Checked = member.Adult;
                    if (member.DOB != null)
                        if (member.DOB.Value.Date.ToShortDateString() != "1/1/0001")
                            txtBxMDOB.Text = member.DOB.Value.Month + "/" + member.DOB.Value.Day + "/" + member.DOB.Value.Year;
                    var splitPrimNumbArea = member.PrimaryNumber.Split(')');
                    if (splitPrimNumbArea.Length > 1)
                    {
                        var splitPrimNumb = splitPrimNumbArea[1].Split('-');
                        txtBxMPrimNumbArea.Text = splitPrimNumbArea[0].Substring(1);
                        txtBxMPrimNumb.Text = splitPrimNumb[0];
                        if (splitPrimNumb.Length > 0) txtBxMPrimNumbLast4.Text = splitPrimNumb[1];
                    }
                    var splitSecNumbArea = member.SecondaryNumber.Split(')');
                    if (splitSecNumbArea.Length > 1)
                    {
                        var splitSecNumb = splitSecNumbArea[1].Split('-');
                        txtBxMSecNumbArea.Text = splitSecNumbArea[0].Substring(1);
                        txtBxMSecNumb.Text = splitSecNumb[0];
                        if (splitSecNumb.Length > 0) txtBxMSecNumbLast4.Text = splitSecNumb[1];
                    }
                    if (member.PreferredCallBackID != 0) ddlPreferCallBack.SelectedValue = member.PreferredCallBackID.ToString();
                    if (member.HowDidYouHearID != 0) ddlHowDidYouHear.SelectedValue = member.HowDidYouHearID.ToString();
                    txtBxEmailAdd.Text = member.EmailAddress;
                    txtBxNotes.Text = member.Notes;
                    hdHearAbout.Value = member.HowDidYouHearID.ToString();
                    leadQuality.Value = member.Quality.ToString();

                    foreach (Achieve ach in member.Achieve)
                    {
                        cblAchieve.Items.FindByValue(ach.AchieveID.ToString()).Selected = true;
                    }

                    if (PageSelection == "NeedConfirm")
                    {
                        tdConfirmLbl.Visible = true;
                        tdConfirmChk.Visible = true;
                    }
                    else
                    {
                        tdConfirmLbl.Visible = false;
                        tdConfirmChk.Visible = false;
                    }

                    string SetStartDate = string.Empty;
                    if (member.CalendarEvent.StartDate.Month + "/" + member.CalendarEvent.StartDate.Day + "/" + member.CalendarEvent.StartDate.Year != "1/1/1")
                        SetStartDate = member.CalendarEvent.StartDate.Month + "/" + member.CalendarEvent.StartDate.Day + "/" + member.CalendarEvent.StartDate.Year;

                    string SetEndDate = string.Empty;
                    if (member.CalendarEvent.StartDate.Month + "/" + member.CalendarEvent.StartDate.Day + "/" + member.CalendarEvent.StartDate.Year != "1/1/1")
                        SetEndDate = member.CalendarEvent.EndDate.Month + "/" + member.CalendarEvent.EndDate.Day + "/" + member.CalendarEvent.EndDate.Year;

                    txtBxIntroStartDate.Text = SetStartDate;
                    txtBxIntroEndDate.Text = SetEndDate;

                    hdCalendarEventID.Value = member.CalendarEvent.CalendarEventID.ToString();

                    ddlStart.SelectedValue = member.CalendarEvent.StartDate.TimeOfDay.ToString();
                    ddlEnd.SelectedValue = member.CalendarEvent.EndDate.TimeOfDay.ToString(); ;

                    btnUpdate.Text = "Update";
                    hdUpdate.Value = "Update";

                    HiddenField hdLeadID = (HiddenField)ucCallHistory.FindControl("hdLeadID");
                    hdLeadID.Value = member.MemberID.ToString();
                    Page.ClientScript.RegisterStartupScript(GetType(), "LoadCallLog", "LoadCallLog();", true);

                    MPE.Show();
                    Page.ClientScript.RegisterStartupScript(GetType(), "HideLoading", "hideLoading();", true);
                }
            }
        }


        protected void grdCallList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int MemberID = Convert.ToInt32(grdCallList.DataKeys[e.RowIndex].Values["MemberID"]);
            MemberService.DeleteMember(MemberID);
            LoadCallList();
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            Member Lead = new Member();
            CalendarEvent Event = new CalendarEvent();

            //Set Lead Info
            Lead.FirstName = txtBxFirstName.Text;
            Lead.LastName = txtBxLastName.Text;
            Lead.Adult = chkBxAdult.Checked;
            Lead.PGFirstName = txtBxPGFName.Text;
            Lead.PGLastName = txtBxPGLName.Text;
            if (txtBxMDOB.Text.Trim().Length > 0) Lead.DOB = Convert.ToDateTime(txtBxMDOB.Text);
            var primNumber = "(" + txtBxMPrimNumbArea.Text + ")" + txtBxMPrimNumb.Text + "-" + txtBxMPrimNumbLast4.Text;
            Lead.PrimaryNumber = primNumber;
            var secondaryNumber = "(" + txtBxMSecNumbArea.Text + ")" + txtBxMSecNumb.Text + "-" + txtBxMSecNumbLast4.Text;
            Lead.SecondaryNumber = secondaryNumber;
            Lead.PreferredCallBackID = null;
            if (ddlPreferCallBack.SelectedValue.Trim().Length > 0) Lead.PreferredCallBackID = Convert.ToInt32(ddlPreferCallBack.SelectedValue);
            Lead.EmailAddress = txtBxEmailAdd.Text;
            Lead.HowDidYouHearID = null;
            if (ddlHowDidYouHear.SelectedValue.Trim().Length > 0) Lead.HowDidYouHearID = Convert.ToInt32(ddlHowDidYouHear.SelectedValue);
            Lead.Notes = txtBxNotes.Text;
            Lead.MemberTypeID = Convert.ToInt32(MemberType.Lead);
            if (leadQuality.SelectedIndex > 0) Lead.Quality = Convert.ToInt32(leadQuality.Value);

            Lead.Achieve.Clear();
            foreach (ListItem cb in cblAchieve.Items)
            {
                if (cb.Selected == true)
                {
                    Achieve achieve = new Achieve();
                    achieve.AchieveID = Convert.ToInt32(cb.Value);
                    Lead.Achieve.Add(achieve);
                }
            }

            //Set Calendar Event
            if (txtBxIntroStartDate.Text.Trim().Length > 0 && txtBxIntroEndDate.Text.Trim().Length > 0)
            {
                Event.EventName = "Intro meeting for " + Lead.FullName;
                DateTime StartDate = Convert.ToDateTime(txtBxIntroStartDate.Text);
                Event.StartDate = Convert.ToDateTime(StartDate.Month + "/" + StartDate.Day + "/" + StartDate.Year + " " + ddlStart.SelectedValue);
                DateTime EndDate = Convert.ToDateTime(txtBxIntroEndDate.Text);
                Event.EndDate = Convert.ToDateTime(EndDate.Month + "/" + EndDate.Day + "/" + EndDate.Year + " " + ddlEnd.SelectedValue);
                Event.AllDayEvent = false;
                Event.EventType = 1;
                Event.Confirmed = chkBxConfirmMeeting.Checked;
            }

            if (hdUpdate.Value == "Update")
            {
                Lead.MemberID = Convert.ToInt32(hdMemberID.Value);

                if (txtBxIntroStartDate.Text.Trim().Length > 0 && txtBxIntroEndDate.Text.Trim().Length > 0)
                {
                    Event.MemberID = Lead.MemberID;

                    //Update Calendar event
                    if (Convert.ToInt32(hdCalendarEventID.Value) > 0)
                    {
                        Event.CalendarEventID = Convert.ToInt32(hdCalendarEventID.Value);
                        CalendarService.UpdateCalendarEvent(Event);
                    }
                    else
                        CalendarService.InsertCalendarEvent(Event);
                }

                MemberService.UpdateMember(Lead);
            }
            else if (hdUpdate.Value == "Save")
            {
                //Insert Lead
                int MemberID = MemberService.InsertMember(Lead, Page.User.Identity.Name);

                if (MemberID > 0)
                {
                    if (txtBxIntroStartDate.Text.Trim().Length > 0 && txtBxIntroEndDate.Text.Trim().Length > 0)
                    {
                        //Insert Calendar event
                        Event.MemberID = MemberID;

                        CalendarService.InsertCalendarEvent(Event);
                    }
                }
            }
            LoadCallList();
        }

        protected void ddlStart_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlEnd.SelectedIndex = ddlStart.SelectedIndex + 1;
            MPE.Show();
        }

        protected void grdCallList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdCallList.PageIndex = e.NewPageIndex;
            grdCallList.DataSource = this.LeadList;
            grdCallList.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string FirstName = null, LastName = null, PhoneNumber = null, PreferCallBack = null;
            if (txtBxFirstNameSearch.Text.Trim().Length > 0) FirstName = txtBxFirstNameSearch.Text;
            if (txtBxLastNameSearch.Text.Trim().Length > 0) LastName = txtBxLastNameSearch.Text;
            if (txtBxPhoneNumberSearch.Text.Trim().Length > 0) PhoneNumber = txtBxPhoneNumberSearch.Text;
            if (ddlPreferredCallBackSearch.SelectedValue.Trim().Length > 0) PreferCallBack = ddlPreferredCallBackSearch.SelectedValue;

            LeadList = MemberService.GetFilteredLeads(Page.User.Identity.Name, FirstName, LastName, PhoneNumber, PreferCallBack, Convert.ToInt32(MemberType.Lead));
            grdCallList.DataSource = LeadList;
            grdCallList.DataBind();
            searchPanel.Attributes.Remove("style");
            Page.ClientScript.RegisterStartupScript(GetType(), "HideLoading", "hideLoading();", true);
        }
    }
}