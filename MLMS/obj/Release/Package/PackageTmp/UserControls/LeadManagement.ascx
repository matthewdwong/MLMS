<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LeadManagement.ascx.cs" Inherits="MLMS.UserControls.LeadManagement" %>

<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.4.2/jquery.min.js"></script>
<script type="text/javascript">
    function isNumberKey(evt) {
        var charCode = (evt.which) ? evt.which : event.keyCode;
        if (charCode != 46 && charCode > 31
          && (charCode < 48 || charCode > 57))
            return false;

        return true;
    }

    function CreateLead(btnID) {
        var txtBxFirstName = $('#<%=txtBxFirstName.ClientID%>').val();
        var txtBxPrimNumb = $('#<%=txtBxPrimNumb.ClientID%>').val();
        var txtCallBackDate = $('#<%=txtBxCallBackDate.ClientID%>').val();

        //Make sure values are not blank
        if (txtBxFirstName == "") {
            $('#<%=lblMsg.ClientID%>').html("First name can not be blank.");
            return false;
        }

        if (txtBxPrimNumb == "") {
            $('#<%=lblMsg.ClientID%>').html("Primary number can not be blank.");
            return false;
        }

        if (txtCallBackDate == "") {
            $('#<%=lblMsg.ClientID%>').html("Call back date can not be blank.");
            return false;
        }

        return true;
        }
</script>
<div style="width:1000px; overflow:auto;">
<div style="width:500px; float:left;">
    <asp:Label runat="server" ID="lblMsg"></asp:Label>
    <asp:Label runat="server" ID="lblFirstName" Text="First Name:" Width="200"></asp:Label>
    <asp:TextBox runat="server" ID="txtBxFirstName"></asp:TextBox>
    <br />
    <asp:Label runat="server" ID="lblLastName" Text="Last Name:" Width="200"></asp:Label>
    <asp:TextBox runat="server" ID="txtBxLastName"></asp:TextBox>
    <br />
    <asp:Label runat="server" ID="lblAdult" Text="Adult:" Width="200"></asp:Label>
    <asp:CheckBox runat="server" ID="chkBxAdult" />
    <br />
    <asp:Label runat="server" ID="lblPGFirstName" Text="Parent/Guardian First Name:" Width="200"></asp:Label>
    <asp:TextBox runat="server" ID="txtBxPGFirstName"></asp:TextBox>
    <br />
    <asp:Label runat="server" ID="lblPGLastName" Text="Parent/Guardian Last Name:" Width="200"></asp:Label>
    <asp:TextBox runat="server" ID="txtBxPGLastName"></asp:TextBox>
    <br />
    <asp:Label runat="server" ID="lblDOB" Text="Date of birth:" Width="200"></asp:Label>
    <asp:TextBox runat="server" ID="txtBxDOB"></asp:TextBox>
</div>
<div style="width:500px; float:right;">
    <asp:Label runat="server" ID="lblPrimNumb" Text="Primary Number:" Width="200"></asp:Label>
        <asp:TextBox runat="server" ID="txtBxPrimNumb" ></asp:TextBox>
    <br />
    <asp:Label runat="server" ID="lblSecNumb" Text="Secondary Number:" Width="200"></asp:Label>
    <asp:TextBox runat="server" ID="txtBxSecNumb"></asp:TextBox>
    <br />
    <asp:Label runat="server" ID="lblPrefCall" Text="Preferred Call Back:" Width="200"></asp:Label>
    <asp:DropDownList runat="server" ID="ddlCallBackTOD"></asp:DropDownList>
    <br />
    <asp:Label runat="server" ID="lblCallBackDate" Text="Call Back Date:" Width="200"></asp:Label>
    <asp:TextBox runat="server" ID="txtBxCallBackDate"></asp:TextBox>
    <br />
    <asp:Label runat="server" ID="lblEmailAddress" Text="Email Address:" Width="200"></asp:Label>
    <asp:TextBox runat="server" ID="txtBxEmailAddress"></asp:TextBox>
    <br />
    <asp:Label runat="server" ID="lblIntroDate" Text="Intro Metting Date:" Width="200"></asp:Label>
    <asp:TextBox runat="server" ID="txtBxIntroMeetDate"></asp:TextBox>
    <br />
    <asp:Label runat="server" ID="lblHearAbout" Text="How did you hear about us?:" Width="200"></asp:Label>
    <asp:DropDownList runat="server" ID="ddlHearAbout"></asp:DropDownList>
    <br />
    <asp:Label runat="server" ID="lblAchieve" Text="What do you want to achieve?:" Width="200"></asp:Label>
    <asp:CheckBoxList ID="chkBxList" AutoPostBack="false" TextAlign="Right" runat="server">
        <asp:ListItem Text="Test"></asp:ListItem>
    </asp:CheckBoxList>
    <br />
    <asp:Label runat="server" ID="lblNotes" Width="200" Text="Notes"></asp:Label>
    <asp:TextBox ID="txtBxNotes" runat="server" TextMode="MultiLine"></asp:TextBox>
    <br />
    <asp:Button runat="server" ID="btnCreateLead" text="Add Lead" OnClientClick="return CreateLead(this.id);" OnClick="btnCreateLead_Click" />
</div>
</div>
