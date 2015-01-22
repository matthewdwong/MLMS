using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using MLMS.Service;

namespace MLMS.UserControls
{
    public partial class CallHistory : System.Web.UI.UserControl
    {
        private DataTable TimeRange
        {
            get { return (DataTable)ViewState["TimeRange"]; }
            set { ViewState["TimeRange"] = value; }
        }

        private DataTable CallLog
        {
            get { return (DataTable)ViewState["CallLog"]; }
            set { ViewState["CallLog"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                TimeRange = CalendarService.GetTimeRange();
                ddlCallStart.DataSource = TimeRange;
                ddlCallStart.DataTextField = "TimeName";
                ddlCallStart.DataValueField = "TimeValue";
                ddlCallStart.DataBind();
            }
        }

        protected void ddlStart_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlCallStart_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

    }
}