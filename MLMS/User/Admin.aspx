<%@ Page Language="C#" MasterPageFile="~/MasterPage/TemplateDefault.Master" AutoEventWireup="true" CodeBehind="Admin.aspx.cs" Inherits="MLMS.User.Admin" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtoolkit" %>
<asp:Content ID="AdminContent" ContentPlaceHolderID="cph1" runat="server">
    <script type="text/javascript" src="../Scripts/jquery-1.9.1.js"></script>
    <script type="text/javascript">
        function Delete(value) {
            var textBox;
            if (value == 'HowDidYouHear')
                textBox = $('#<%=txtBxHowDidYouHear.ClientID %>');
            else if (value == 'Achieve')
                textBox = $('#<%=txtBxAchieve.ClientID %>');
            else if (value == 'Objection')
                textBox = $('#<%=txtBxObjection.ClientID %>');
            else if (value == 'CLOpt')
                textBox = $('#<%=txtBxCLOption.ClientID %>');
    if (textBox.val().length < 1)
        return false;
    var bool;
    bool = confirm('Are you sure you want to delete ' + textBox[0].value + '?');
    if (bool)
        return true;
    else
        return false;
}
        
        function validateTextBox() {
            var textBox = $('#<%= txtBoxAddNew.ClientID %>');
            if ($.trim(textBox.val()).length > 0) {
                textBox[0].style.border = "";
                return true;
            }
            else {
                showMessageBox("Item can not be blank", 2);
                textBox[0].style.border = "2px solid red";
                return false;
            }
        }
    </script>
    <div style="text-transform: uppercase; height: 25px; font-size: 18px; color: #808080; font-weight: bold;">
        Configure Settings 
    </div>
    <div id="tabs">
        <span class="calendarMenuSelected" id="formSection">
            Form
        </span>
        <span class="calendarMenu" id="checklistSection">
            Checklist
        </span>
    </div>

    <div style="float: left; width: 33.3%;">
        <div>
            <asp:Label runat="server" ID="lblHowDidYouHear" Text="How did you Hear: "></asp:Label>
            <asp:TextBox runat="server" ID="txtBxHowDidYouHear"></asp:TextBox>
        </div>

        <div>
            <asp:ListBox runat="server" ID="lstBxHowDidYouHear" Style="min-width: 200px;" AutoPostBack="true" OnSelectedIndexChanged="lstBxHowDidYouHear_SelectedIndexChanged"></asp:ListBox>
        </div>
        <div>
            <asp:Button runat="server" ID="btnAddHDYH" Text="Add New How Did You Hear" OnClick="LoadModal" />
            <asp:Button runat="server" ID="btnUpdateHDYH" Text="Update" OnClick="btnUpdateHDYH_Click" />
            <asp:Button runat="server" ID="btnDeleteHDYH" Text="Delete" OnClick="btnDeleteHDYH_Click" OnClientClick="return Delete('HowDidYouHear');" />
        </div>
    </div>
    <div style="float: left; width:  33.3%;">
        <div>
            <asp:Label runat="server" ID="lblAchieve" Text="What do they want to Achieve: "></asp:Label>
            <asp:TextBox runat="server" ID="txtBxAchieve"></asp:TextBox>
        </div>
        <div>
            <asp:ListBox runat="server" ID="lstBxAchieve" Style="min-width: 200px;" AutoPostBack="true" OnSelectedIndexChanged="lstBxAchieve_SelectedIndexChanged"></asp:ListBox>
        </div>
        <div>
            <asp:Button runat="server" ID="btnAddAchieve" Text="Add New Achieve Item" OnClick="LoadModal" />
            <asp:Button runat="server" ID="btnUpdateAchieve" Text="Update" OnClick="btnUpdateAchieve_Click" />
            <asp:Button runat="server" ID="btnDeleteAchieve" Text="Delete" OnClick="btnDeleteAchieve_Click" OnClientClick="return Delete('Achieve');" />
        </div>
    </div>
    <div style="float: left; width: 33.3%;">
        <div>
            <asp:Label runat="server" ID="lblObjection" Text="Objections: "></asp:Label>
            <asp:TextBox runat="server" ID="txtBxObjection"></asp:TextBox>
        </div>

        <div>
            <asp:ListBox runat="server" ID="lstBxObj" Style="min-width: 200px;" AutoPostBack="true" OnSelectedIndexChanged="lstBxObj_SelectedIndexChanged"></asp:ListBox>
        </div>
        <div>
            <asp:Button runat="server" ID="btnAddObjection" Text="Add New Objection" OnClick="LoadModal" />
            <asp:Button runat="server" ID="btnUpdateObjection" Text="Update" OnClick="btnUpdateObjection_Click" />
            <asp:Button runat="server" ID="btnDeleteObjection" Text="Delete" OnClick="btnDeleteObjection_Click" OnClientClick="return Delete('Objection');" />
        </div>
    </div>
    <div>
        <hr />
        <h2>Add New Options</h2>
        <div>
            <span>Select from detail:</span>
            <span>Add new option</span>
        </div>
        <div>
            <div style="float:left; width:30%">
            <asp:DropDownList runat="server">
                <asp:ListItem Text="text1" />
                <asp:ListItem Text="text2" />
            </asp:DropDownList>
            </div>
            <div style="float:left; max-width:200px;">
            <asp:TextBox runat="server" ID="txtBoxOption_1"></asp:TextBox>
            <asp:TextBox runat="server" ID="txtBoxOption_2"></asp:TextBox>
            <asp:TextBox runat="server" ID="txtBoxOption_3"></asp:TextBox>
            </div>

        </div>
    </div>
    <div style="clear: both;">
        <br />
    </div>
    <div style="float: left; width:50%;">
        <div>
            <asp:Label runat="server" ID="lblChecklist" Text="Select Checklist:"></asp:Label>
            <asp:DropDownList runat="server" ID="ddlCl" OnSelectedIndexChanged="ddlCl_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
        </div>
        <div>
            <asp:Button runat="server" ID="btnAddCl" Text="Add New Checklist" OnClick="LoadModal" />
            <asp:Button runat="server" ID="btnUpdateCl" Text="Edit" OnClick="LoadModal" />
            <asp:Button runat="server" ID="btnDeleteCl" Text="Delete" OnClick="btnDeleteCl_Click" />
        </div>
        <div>
            <asp:Label runat="server" ID="lblChecklistOption" Text="Checklist Option:"></asp:Label>
            <asp:TextBox runat="server" ID="txtBxCLOption"></asp:TextBox>
        </div>

        <div>
            <asp:ListBox runat="server" ID="lstBxCLOpt" Style="min-width: 400px;" AutoPostBack="true" OnSelectedIndexChanged="lstBxCLOpt_SelectedIndexChanged"></asp:ListBox>
        </div>
        <div>
            <asp:Button runat="server" ID="btnAddCLOpt" Text="Add New Checklist Option" OnClick="LoadModal" />
            <asp:Button runat="server" ID="btnUpdateCLOpt" Text="Update" OnClick="btnUpdateCLOpt_Click" />
            <asp:Button runat="server" ID="btnDeleteClOpt" Text="Delete" OnClick="btnDeleteClOpt_Click" OnClientClick="return Delete('CLOpt');" />
        </div>
    </div>
    <asp:Button runat="server" ID="btnPopUp" Style="display: none;" />
    <asp:Panel runat="server" ID="pnlPopUp" CssClass="modalPopupAdmin" DefaultButton="btnAddNew">
        <div style="float: left;">
            <asp:Label runat="server" ID="lblAddNew"></asp:Label>
        </div>
        <div style="clear: both;">
            <asp:TextBox runat="server" ID="txtBoxAddNew"></asp:TextBox>
            <asp:HiddenField runat="server" ID="hdType" />
            <asp:HiddenField runat="server" ID="hdID" />
            <asp:Button ID="btnAddNew" runat="server" OnClick="btnAddNew_Click" Text="Add" OnClientClick="return validateTextBox();" />
            <asp:Button runat="server" ID="btnCancel" Text="Cancel" />
        </div>
    </asp:Panel>
    <ajaxtoolkit:ModalPopupExtender ID="MPE" runat="server"
        BehaviorID="PE"
        TargetControlID="btnPopUp"
        PopupControlID="pnlPopUp"
        BackgroundCssClass="modalBackground"
        CancelControlID="btnCancel">
    </ajaxtoolkit:ModalPopupExtender>
</asp:Content>
