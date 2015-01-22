using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Linq;
using System.Web;
using System.Web.Services;
using MLMS.Service;

namespace MLMS.Services
{
    /// <summary>
    /// Summary description for MemberWebService
    /// </summary>
    [WebService(Namespace = "MLMS")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class MemberWebService : System.Web.Services.WebService
    {

        [WebMethod(EnableSession = true)]
        public bool DeleteLead(string MemberID)
        {
            bool check = MemberService.DeleteMember(Convert.ToInt32(MemberID));
            return true;
        }

        [WebMethod(EnableSession = true)]
        public string AddCallHistory(string LeadID, string Date, string Time, string Notes)
        {
            DateTime CallLogDate = Convert.ToDateTime(Date);
            DateTime FullDateTime = Convert.ToDateTime(CallLogDate.Month + "/" + CallLogDate.Day + "/" + CallLogDate.Year + " " + Time);
            MemberService.InsertCallLog(Convert.ToInt32(LeadID), FullDateTime, HttpUtility.HtmlDecode(Notes));

            DataTable CallLog = MemberService.GetCallLog(Convert.ToInt32(LeadID));

            string convertToHTML = string.Empty;

            if (CallLog != null)
                if (CallLog.Rows.Count != 0)
                {
                    for (int i = 0; i < CallLog.Rows.Count; i++)
                    {
                        HtmlTableRow tblRow = new HtmlTableRow();

                        HtmlTableCell tblEditCell = new HtmlTableCell();
                        HtmlInputButton btnEdit = new HtmlInputButton();
                        btnEdit.Value = "Edit";
                        btnEdit.ID = "btnEdit" + i;
                        btnEdit.Attributes.Add("onclick", "EditCallHistory(this.id);return false;");
                        btnEdit.Attributes.Add("class", "editCallHistory");
                        tblEditCell.Controls.Add(btnEdit);
                        HtmlInputButton btnCancel = new HtmlInputButton();
                        btnCancel.Value = "Cancel";
                        btnCancel.ID = "btnCancel" + i;
                        btnCancel.Attributes.Add("class", "cancelCallHistory");
                        btnCancel.Attributes.Add("style", "display:none");
                        btnCancel.Attributes.Add("onclick", "CancelCallHistory();return false;");
                        tblEditCell.Controls.Add(btnCancel);
                        HiddenField hdCallLogID = new HiddenField();
                        hdCallLogID.Value = CallLog.Rows[i]["MemberCallLog"] == DBNull.Value ? "" : CallLog.Rows[i]["MemberCallLog"].ToString();
                        hdCallLogID.ID = "hdMemberCallLogID" + i;
                        tblEditCell.Controls.Add(hdCallLogID);
                        tblRow.Controls.Add(tblEditCell);

                        //Date Called
                        HtmlTableCell tblCell = new HtmlTableCell();
                        tblCell.BorderColor = "black";
                        Label lblDateCalled = new Label();
                        lblDateCalled.Text = CallLog.Rows[i]["DateCalled"] == DBNull.Value ? "" : CallLog.Rows[i]["DateCalled"].ToString();
                        lblDateCalled.ID = "lblDateCalled" + i;
                        tblCell.Controls.Add(lblDateCalled);
                        tblRow.Controls.Add(tblCell);

                        //Notes
                        HtmlTableCell tblCellNotes = new HtmlTableCell();
                        tblCellNotes.BorderColor = "black";
                        Label lblNotes = new Label();
                        lblNotes.Text = CallLog.Rows[i]["Notes"] == DBNull.Value ? "" : CallLog.Rows[i]["Notes"].ToString();
                        lblNotes.ID = "lblNotes" + i;
                        tblCellNotes.Controls.Add(lblNotes);
                        TextBox txtBxNotesUpdate = new TextBox();
                        txtBxNotesUpdate.ID = "txtBxNotesUpdate" + i;
                        txtBxNotesUpdate.TextMode = TextBoxMode.MultiLine;
                        txtBxNotesUpdate.Attributes.Add("style", "max-width: 300px; max-height: 200px; min-height: 200px; min-width: 300px; display:none;");
                        tblCellNotes.Controls.Add(txtBxNotesUpdate);
                        tblRow.Controls.Add(tblCellNotes);

                        HtmlTableCell tblDeleteCell = new HtmlTableCell();
                        HtmlInputButton btnDelete = new HtmlInputButton();
                        btnDelete.Value = "Delete";
                        btnDelete.ID = "btnDelete" + i;
                        btnDelete.Attributes.Add("onclick", "DeleteCallHistory(this.id);return false;");
                        tblDeleteCell.Controls.Add(btnDelete);
                        tblRow.Controls.Add(tblDeleteCell);

                        using (StringWriter sw = new StringWriter())
                        {
                            tblRow.RenderControl(new HtmlTextWriter(sw));
                            convertToHTML = convertToHTML + sw.ToString();
                        }
                    }
                }

            return convertToHTML;
        }

        [WebMethod(EnableSession = true)]
        public string GetCallHistory(string LeadID)
        {
            DataTable CallLog = MemberService.GetCallLog(Convert.ToInt32(LeadID));

            string convertToHTML = string.Empty;

            if (CallLog != null)
                if (CallLog.Rows.Count != 0)
                {
                    for (int i = 0; i < CallLog.Rows.Count; i++)
                    {
                        HtmlTableRow tblRow = new HtmlTableRow();

                        HtmlTableCell tblEditCell = new HtmlTableCell();
                        HtmlInputButton btnEdit = new HtmlInputButton();
                        btnEdit.Value = "Edit";
                        btnEdit.ID = "btnEdit" + i;
                        btnEdit.Attributes.Add("onclick", "EditCallHistory(this.id);return false;");
                        btnEdit.Attributes.Add("class", "editCallHistory");
                        tblEditCell.Controls.Add(btnEdit);
                        HtmlInputButton btnCancel = new HtmlInputButton();
                        btnCancel.Value = "Cancel";
                        btnCancel.ID = "btnCancel" + i;
                        btnCancel.Attributes.Add("class", "cancelCallHistory");
                        btnCancel.Attributes.Add("style", "display:none");
                        btnCancel.Attributes.Add("onclick", "CancelCallHistory();return false;");
                        tblEditCell.Controls.Add(btnCancel);
                        HiddenField hdCallLogID = new HiddenField();
                        hdCallLogID.Value = CallLog.Rows[i]["MemberCallLog"] == DBNull.Value ? "" : CallLog.Rows[i]["MemberCallLog"].ToString();
                        hdCallLogID.ID = "hdMemberCallLogID" + i;
                        tblEditCell.Controls.Add(hdCallLogID);
                        tblRow.Controls.Add(tblEditCell);

                        //Date Called
                        HtmlTableCell tblCell = new HtmlTableCell();
                        tblCell.BorderColor = "black";
                        Label lblDateCalled = new Label();
                        lblDateCalled.Text = CallLog.Rows[i]["DateCalled"] == DBNull.Value ? "" : CallLog.Rows[i]["DateCalled"].ToString();
                        lblDateCalled.ID = "lblDateCalled" + i;
                        tblCell.Controls.Add(lblDateCalled);
                        tblRow.Controls.Add(tblCell);

                        //Notes
                        HtmlTableCell tblCellNotes = new HtmlTableCell();
                        tblCellNotes.BorderColor = "black";
                        Label lblNotes = new Label();
                        lblNotes.Text = CallLog.Rows[i]["Notes"] == DBNull.Value ? "" : CallLog.Rows[i]["Notes"].ToString();
                        lblNotes.ID = "lblNotes" + i;
                        tblCellNotes.Controls.Add(lblNotes);
                        TextBox txtBxNotesUpdate = new TextBox();
                        txtBxNotesUpdate.ID = "txtBxNotesUpdate" + i;
                        txtBxNotesUpdate.TextMode = TextBoxMode.MultiLine;
                        txtBxNotesUpdate.Attributes.Add("style", "max-width: 300px; max-height: 200px; min-height: 200px; min-width: 300px; display:none;");
                        tblCellNotes.Controls.Add(txtBxNotesUpdate);
                        tblRow.Controls.Add(tblCellNotes);

                        HtmlTableCell tblDeleteCell = new HtmlTableCell();
                        HtmlInputButton btnDelete = new HtmlInputButton();
                        btnDelete.Value = "Delete";
                        btnDelete.ID = "btnDelete" + i;
                        btnDelete.Attributes.Add("onclick", "DeleteCallHistory(this.id);return false;");
                        tblDeleteCell.Controls.Add(btnDelete);
                        tblRow.Controls.Add(tblDeleteCell);

                        using (StringWriter sw = new StringWriter())
                        {
                            tblRow.RenderControl(new HtmlTextWriter(sw));
                            convertToHTML = convertToHTML + sw.ToString();
                        }
                    }
                }

            return convertToHTML;
        }

        [WebMethod(EnableSession = true)]
        public void UpdateCallHistory(string MemberCallLog, string Notes)
        {
            MemberService.UpdateCallLog(Convert.ToInt32(MemberCallLog), HttpUtility.HtmlDecode(Notes));
        }

        [WebMethod(EnableSession = true)]
        public void DeleteCallHistory(string MemberCallLog)
        {
            MemberService.DeleteCallHistory(Convert.ToInt32(MemberCallLog));
        }
    }
}
