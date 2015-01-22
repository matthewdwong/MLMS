using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MLMS.Service;
using MLMS.Objects;

namespace MLMS.User
{
    public partial class MemberSearch : System.Web.UI.Page
    {
        #region Properties
        private List<Member> LeadList
        {
            get { return (List<Member>)ViewState["LeadList"]; }
            set { ViewState["LeadList"] = value; }
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
                ddlPreferredCallBackSearch.DataSource =  MemberService.GetPreferredCallBack();
                ddlPreferredCallBackSearch.DataTextField = "PreferredValue";
                ddlPreferredCallBackSearch.DataValueField = "PreferredCallBackID";
                ddlPreferredCallBackSearch.DataBind();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string FirstName = null, LastName = null, PhoneNumber = null;
            if (txtBxFirstNameSearch.Text.Trim().Length > 0) FirstName = txtBxFirstNameSearch.Text;
            if (txtBxLastNameSearch.Text.Trim().Length > 0) LastName = txtBxLastNameSearch.Text;
            if (txtBxPhoneNumberSearch.Text.Trim().Length > 0) PhoneNumber = txtBxPhoneNumberSearch.Text;

            LeadList = MemberService.GetFilteredLeads(Page.User.Identity.Name, FirstName, LastName, PhoneNumber, ddlPreferredCallBackSearch.SelectedValue, 0);
            grdMembers.DataSource = LeadList;
            grdMembers.DataBind();
            searchPanel.Attributes.Remove("style");
        }

        protected void grdMembers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdMembers.PageIndex = e.NewPageIndex;
            grdMembers.DataSource = this.LeadList;
            grdMembers.DataBind();
        }

        protected void grdMembers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Page")
            {
                GridViewRow gvr = (GridViewRow)(((Button)e.CommandSource).NamingContainer);


                int RowIndex = gvr.RowIndex;
                int MemberID = Convert.ToInt32(grdMembers.DataKeys[RowIndex].Values["MemberID"]);
                if (e.CommandName == "Select")
                {
                    Context.Items["MemberID"] = MemberID;
                    Server.Transfer("Members.aspx");
                }
            }
        }

        protected void grdMembers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int MemberID = Convert.ToInt32(grdMembers.DataKeys[e.RowIndex].Values["MemberID"]);
            MemberService.DeleteMember(MemberID);
        }
    }
}