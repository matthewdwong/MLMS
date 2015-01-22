using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MLMS.Service;

namespace MLMS.User
{
    public partial class Admin : System.Web.UI.Page
    {       
        #region Events
        protected void Page_PreInit(object sender, EventArgs e)
        {
            this.Page.Theme = "bdsu";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadHowDidYouHear();
                LoadAchieve();
                LoadObjection();
                LoadCL();
            }
        }

        protected void btnUpdateAchieve_Click(object sender, EventArgs e)
        {
            if (lstBxAchieve.SelectedIndex > -1)
            {
                int AchieveID = Convert.ToInt32(lstBxAchieve.SelectedValue);

                if (AchieveID > 0)
                {
                    AdminService.UpdateAchieve(txtBxAchieve.Text.Trim(), AchieveID);
                    lstBxAchieve.SelectedItem.Text = txtBxAchieve.Text;
                }
            }
        }

        protected void btnUpdateObjection_Click(object sender, EventArgs e)
        {
            if (lstBxObj.SelectedIndex > -1)
            {
                int ObjectionID = Convert.ToInt32(lstBxObj.SelectedValue);

                if (ObjectionID > 0)
                {
                    AdminService.UpdateObjection(txtBxObjection.Text.Trim(), ObjectionID);
                    lstBxObj.SelectedItem.Text = txtBxObjection.Text;
                }
            }
        }

        protected void btnUpdateHDYH_Click(object sender, EventArgs e)
        {
            if (lstBxHowDidYouHear.SelectedIndex > -1)
            {
                int HowDidYouHearID = Convert.ToInt32(lstBxHowDidYouHear.SelectedValue);

                if (HowDidYouHearID > 0)
                {
                    AdminService.UpdateHowDidYouHear(txtBxHowDidYouHear.Text.Trim(), HowDidYouHearID);
                    lstBxHowDidYouHear.SelectedItem.Text = txtBxHowDidYouHear.Text;
                }
            }
        }

        protected void btnUpdateCLOpt_Click(object sender, EventArgs e)
        {
            if (lstBxCLOpt.SelectedIndex > -1)
            {
                int CLOptID = Convert.ToInt32(lstBxCLOpt.SelectedValue);

                if (CLOptID > 0)
                {
                    AdminService.UpdateUserChecklistOption(txtBxCLOption.Text.Trim(), CLOptID);
                    lstBxCLOpt.SelectedItem.Text = txtBxCLOption.Text;
                }
            }
        }

        protected void lstBxAchieve_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtBxAchieve.Text = lstBxAchieve.SelectedItem.Text;
        }

        protected void lstBxHowDidYouHear_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtBxHowDidYouHear.Text = lstBxHowDidYouHear.SelectedItem.Text;
        }

        protected void lstBxObj_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtBxObjection.Text = lstBxObj.SelectedItem.Text;
        }

        protected void lstBxCLOpt_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtBxCLOption.Text = lstBxCLOpt.SelectedItem.Text;
        }

        protected void btnDeleteHDYH_Click(object sender, EventArgs e)
        {
            if (lstBxHowDidYouHear.SelectedIndex > -1)
            {
                int HowDidYouHearID = Convert.ToInt32(lstBxHowDidYouHear.SelectedValue);
                if (HowDidYouHearID > 0)
                {
                    AdminService.DeleteHowDidYouHear(HowDidYouHearID);
                    LoadHowDidYouHear();
                    txtBxHowDidYouHear.Text = string.Empty;
                }
            }
        }

        protected void btnDeleteAchieve_Click(object sender, EventArgs e)
        {
            if (lstBxAchieve.SelectedIndex > -1)
            {
                int AchieveID = Convert.ToInt32(lstBxAchieve.SelectedValue);
                if (AchieveID > 0)
                {
                    AdminService.DeleteAchieve(AchieveID);
                    LoadAchieve();
                    txtBxAchieve.Text = string.Empty;
                }
            }
        }

        protected void btnDeleteObjection_Click(object sender, EventArgs e)
        {
            if (lstBxObj.SelectedIndex > -1)
            {
                int ObjectionID = Convert.ToInt32(lstBxObj.SelectedValue);
                if (ObjectionID > 0)
                {
                    AdminService.DeleteObjection(ObjectionID);
                    LoadObjection();
                    txtBxObjection.Text = string.Empty;
                }
            }
        }

        protected void btnDeleteClOpt_Click(object sender, EventArgs e)
        {
            if (lstBxCLOpt.SelectedIndex > -1)
            {
                int CLOptID = Convert.ToInt32(lstBxCLOpt.SelectedValue);
                if (CLOptID > 0)
                {
                    AdminService.DeleteUserChecklistOption(CLOptID);
                    LoadCLOpt();
                    txtBxCLOption.Text = string.Empty;
                }
            }
        }

        protected void btnDeleteCl_Click(object sender, EventArgs e)
        {
            if (ddlCl.SelectedIndex > -1)
            {
                int ClID = Convert.ToInt32(ddlCl.SelectedValue);
                if (ClID > 0)
                {
                    AdminService.DeleteUserChecklist(ClID);
                    LoadCL();
                    lstBxCLOpt.Items.Clear();
                }
            }
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            string NewItem = txtBoxAddNew.Text;
            string hiddenID = hdID.Value;

            if (NewItem.Trim().Length > 0)
            {
                switch (hdType.Value.ToLower())
                {
                    case "achieve":
                        int AchieveID = AdminService.InsertAchieve(NewItem, Page.User.Identity.Name);
                        if (AchieveID > 0)
                            lstBxAchieve.Items.Add(new ListItem(NewItem, AchieveID.ToString()));
                        break;
                    case "hdyh":
                        int HowDidYouHearID = AdminService.InsertHowDidYouHear(NewItem, Page.User.Identity.Name);
                        if (HowDidYouHearID > 0)
                            lstBxHowDidYouHear.Items.Add(new ListItem(NewItem, HowDidYouHearID.ToString()));
                        break;
                    case "objection":
                        int ObjectionID = AdminService.InsertObjection(NewItem, Page.User.Identity.Name);
                        if (ObjectionID > 0)
                            lstBxObj.Items.Add(new ListItem(NewItem, ObjectionID.ToString()));
                        break;
                    case "clopt":
                        int CLOpt = AdminService.InsertUserChecklistOption(NewItem, Convert.ToInt32(hiddenID));
                        if (CLOpt > 0)
                            lstBxCLOpt.Items.Add(new ListItem(NewItem, CLOpt.ToString()));
                        break;
                    case "cl":
                        int Cl = AdminService.InsertUserChecklist(NewItem, Page.User.Identity.Name);
                        if (Cl > 0)
                        {
                            ddlCl.Items.Add(new ListItem(NewItem, Cl.ToString()));
                            ddlCl.SelectedValue = Cl.ToString();
                            lstBxCLOpt.Items.Clear();
                            EnableChecklist(true);
                        }
                        break;
                    case "clupdate":
                        AdminService.UpdateUserChecklist(NewItem, Convert.ToInt32(hiddenID));
                        LoadCL();
                        ddlCl.SelectedValue = hiddenID;
                        LoadCLOpt();
                        EnableChecklist(true);
                        break;
                }
            }
        }

        protected void ddlCl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCl.SelectedIndex > 0)
                EnableChecklist(true);
            else
                EnableChecklist(false);

            LoadCLOpt();
        }

        #endregion

        #region Methods

        protected void LoadHowDidYouHear()
        {
            lstBxHowDidYouHear.DataSource = MemberService.GetHearAbout(Page.User.Identity.Name);
            lstBxHowDidYouHear.DataTextField = "HowDidYouHear";
            lstBxHowDidYouHear.DataValueField = "HowDidYouHearId";
            lstBxHowDidYouHear.DataBind();
        }

        protected void LoadAchieve()
        {
            lstBxAchieve.DataSource = MemberService.GetUserAchieveList(Page.User.Identity.Name);
            lstBxAchieve.DataTextField = "AchieveName";
            lstBxAchieve.DataValueField = "AchieveID";
            lstBxAchieve.DataBind();
        }

        protected void LoadObjection()
        {
            lstBxObj.DataSource = MemberService.GetObjections(Page.User.Identity.Name);
            lstBxObj.DataTextField = "Objection";
            lstBxObj.DataValueField = "ObjectionId";
            lstBxObj.DataBind();
        }

        protected void LoadCLOpt()
        {
                lstBxCLOpt.DataSource = MemberService.GetUserChecklistOptions(Convert.ToInt32(ddlCl.SelectedValue));
                lstBxCLOpt.DataTextField = "OptionName";
                lstBxCLOpt.DataValueField = "UserChecklistOptionID";
                lstBxCLOpt.DataBind();
        }

        protected void LoadCL()
        {
            ddlCl.DataSource = AdminService.GetUserCheckList(Page.User.Identity.Name);
            ddlCl.DataTextField = "ChecklistName";
            ddlCl.DataValueField = "UserChecklistID";
            ddlCl.DataBind();

            ddlCl.Items.Insert(0, new ListItem("-- Select a Checklist --", "0"));

            EnableChecklist(false);
        }

        protected void EnableChecklist(bool enable)
        {
            btnAddCl.Enabled = true;
            if (enable)
            {
                btnUpdateCl.Enabled = true;
                btnDeleteCl.Enabled = true;
                txtBxCLOption.Text = string.Empty;
                btnAddCLOpt.Enabled = true;
                btnUpdateCLOpt.Enabled = true;
                btnDeleteClOpt.Enabled = true;
            }
            else
            {
                btnUpdateCl.Enabled = false;
                btnDeleteCl.Enabled = false;
                txtBxCLOption.Text = string.Empty;
                lstBxCLOpt.Items.Clear();
                btnAddCLOpt.Enabled = false;
                btnUpdateCLOpt.Enabled = false;
                btnDeleteClOpt.Enabled = false;
            }
        }

        protected void LoadModal(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            switch (btn.ID.ToLower())
            {
                case "btnaddclopt":
                    lblAddNew.Text = "Add new Check List Option";
                    hdType.Value = "CLOpt";
                    hdID.Value = ddlCl.SelectedValue;
                    btnAddNew.Text = "Add";
                    break;
                case "btnaddobjection":
                    lblAddNew.Text = "Add new objection";
                    hdType.Value = "Objection";
                    btnAddNew.Text = "Add";
                    break;
                case "btnaddhdyh":
                    lblAddNew.Text = "Add new how did you hear option";
                    hdType.Value = "HDYH";
                    btnAddNew.Text = "Add";
                    break;
                case "btnaddachieve":
                    lblAddNew.Text = "Add new achieve option";
                    hdType.Value = "Achieve";
                    btnAddNew.Text = "Add";
                    break;
                case "btnaddcl":
                    lblAddNew.Text = "Add new Checklist";
                    hdType.Value = "Cl";
                    btnAddNew.Text = "Add";
                    break;
                case "btnupdatecl":
                    lblAddNew.Text = "Update Checklist Name: " + ddlCl.SelectedItem;
                    hdType.Value = "ClUpdate";
                    hdID.Value = ddlCl.SelectedValue;
                    btnAddNew.Text = "Update";
                    break;
            }

            txtBoxAddNew.Text = string.Empty;
            MPE.Show();
        }

        #endregion

    }
}