using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MLMS.Objects;
using MLMS.Service;

namespace MLMS.User
{
    public partial class Search : System.Web.UI.Page
    {
        private List<Member> MemberList
        {
            get { return (List<Member>)ViewState["MemberList"]; }
            set { ViewState["MemberList"] = value; }
        }

        protected void Page_PreInit(object sender, EventArgs e)
        {
            this.Page.Theme = "bdsu";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ddlPreferredCallBackSearch.DataSource = MemberService.GetPreferredCallBack();
                ddlPreferredCallBackSearch.DataTextField = "PreferredValue";
                ddlPreferredCallBackSearch.DataValueField = "PreferredCallBackID";
                ddlPreferredCallBackSearch.DataBind();

                ddlPreferredCallBackSearch.Items.Insert(0, new ListItem("", null));
            }
        }

        protected void grdMembersCustomer_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdMembersCustomer.PageIndex = e.NewPageIndex;
            grdMembersCustomer.DataSource = this.MemberList;
            grdMembersCustomer.DataBind();
        }

        protected void grdMembersCustomer_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Page")
            {
                GridViewRow gvr = (GridViewRow)(((Button)e.CommandSource).NamingContainer);


                int RowIndex = gvr.RowIndex;
                int MemberID = Convert.ToInt32(grdMembersCustomer.DataKeys[RowIndex].Values["MemberID"]);
                if (e.CommandName == "Select")
                {
                    if(MemberID > 0)
                        Response.Redirect("~/User/Members.aspx?MemberID=" + MemberID);
                }
            }
        }

        protected void grdMembersCustomer_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int MemberID = Convert.ToInt32(grdMembersCustomer.DataKeys[e.RowIndex].Values["MemberID"]);
            MemberService.InactivateLead(MemberID, null, "Inactivated/Deleted from search page", false);
            grdMembersCustomer.DataSource = null;
            grdMembersCustomer.DataBind();
            Page.ClientScript.RegisterStartupScript(GetType(), "HideLoading", "hideLoading();", true);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string FirstName = null, LastName = null, PhoneNumber = null, PreferCallBack = null;
            if (txtBxFirstNameSearch.Text.Trim().Length > 0) FirstName = txtBxFirstNameSearch.Text;
            if (txtBxLastNameSearch.Text.Trim().Length > 0) LastName = txtBxLastNameSearch.Text;
            if (txtBxPhoneNumberSearch.Text.Trim().Length > 0) PhoneNumber = txtBxPhoneNumberSearch.Text;
            if (ddlPreferredCallBackSearch.SelectedValue.Trim().Length > 0) PreferCallBack = ddlPreferredCallBackSearch.SelectedValue;

            var MemberTypePara = Request.QueryString["MemberType"];
            int MemberTypeID = Convert.ToInt32(MemberTypePara);

            MemberList = MemberService.GetFilteredLeads(Page.User.Identity.Name, FirstName, LastName, PhoneNumber, PreferCallBack, MemberTypeID);
            grdMembersCustomer.DataSource = MemberList;
            grdMembersCustomer.DataBind();
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/User/Members.aspx?MemberType=" + Request.QueryString["MemberType"]);
        }
    }
}