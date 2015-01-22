<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditProfile.aspx.cs" Inherits="MLMS.User.EditProfile"
    MasterPageFile="~/MasterPage/Default.Master" EnableEventValidation="false" %>
<%@ Register Src="~/UserControls/UserControlBase.ascx" TagName="bC" TagPrefix="uc"  %>
<asp:Content runat="server" ID="contentArea" ContentPlaceHolderID="cphBody">

        <uc:bC ID="bcUserProfile" ControlTitle="Account Profile" runat="server" OnLoad="bcUserProfile_LoadUserControl">
        <TemplateItems>
            <table border="0" style="border-collapse: collapse" width="100%" id="tbl">
                <tr>
                    <td align="center" colspan="3" style="padding-bottom: 10px">
                        <asp:ValidationSummary ID="vldSummary" runat="server" 
                            HeaderText="The following errors occurred:" 
                            DisplayMode="BulletList" 
                            ValidationGroup="bcUserProfile" meta:resourceKey="locDay" />
                        <asp:Label runat="server" ID="lblConfirmation" Text="Confirmation Label" />
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="3" style="color: Red;">
                        <asp:Literal ID="ErrorMessage" runat="server" EnableViewState="False"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="lblProfilePicture" runat="server" Text="Profile Picture"></asp:Label>
                    </td>
                    <td width="60%" style="padding: 5px 10px 5px 15px;">
                        <asp:Image runat="server" id="imgProfilePic" />
                    </td>
                    <td width="10%">
                        
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="lblUploadProfilePic" runat="server" Text="Upload Profile Picture"></asp:Label>
                    </td>
                    <td width="60%" style="padding: 5px 10px 5px 15px;">
                        <asp:FileUpload runat="server" ID="filUpload" />
                    </td>
                    <td width="10%">
                        
                    </td>
                </tr>
                <tr id="trFirstName" runat="server">
                    <td align="right">
                        <asp:Label ID="lblFirstName" runat="server" AssociatedControlID="txtFirstName" Text="First Name"></asp:Label>
                        <asp:Label ID="astFirstName" runat="server" >*</asp:Label>
                    </td>
                    <td width="60%" style="padding: 5px 10px 5px 15px;">
                        <asp:TextBox ID="txtFirstName" CausesValidation="true" runat="server" Width="100%"></asp:TextBox>
                    </td>
                    <td width="10%">
                        &nbsp;
                        <asp:RequiredFieldValidator 
                            ID="vldFirstName" 
                            Display="Dynamic" 
                            runat="server" 
                            ControlToValidate="txtFirstName"
                            ErrorMessage="First Name is required." 
                            ToolTip="First Name is required." 
                            Text="*"
                            ValidationGroup="bcUserProfile" meta:resourceKey="locDay"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr id="trLastName" runat="server">
                    <td align="right">
                        <asp:Label ID="lblLastName" runat="server" AssociatedControlID="txtLastName" Text="Last Name"></asp:Label>
                        <asp:Label ID="astLastName" runat="server" > *</asp:Label>
                    </td>
                    <td width="60%" style="padding: 5px 10px 5px 15px;">
                        <asp:TextBox Width="100%" ID="txtLastName" runat="server"></asp:TextBox>
                        
                    </td>
                    <td width="10%">
                        <asp:RequiredFieldValidator ID="vldLastName" 
                            Display="Dynamic" 
                            runat="server" 
                            ControlToValidate="txtLastName"
                            ErrorMessage="Last Name is required." 
                            ToolTip="Last Name is required." 
                            Text="*"
                            ValidationGroup="bcUserProfile" meta:resourceKey="locDay"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr id="trEmail" runat="server">
                    <td align="right" style="border-top: solid 1px black; padding-bottom: 5px; padding-top: 5px;
                        background-color: #FFFFBB; border-left: solid 1px black">
                        <asp:Label ID="lblUserName" runat="server" AssociatedControlID="UserName" Text="Email Address"></asp:Label>
                        <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">*</asp:Label>
                    </td>
                    <td style="border-top: solid 1px black; padding: 5px 10px 5px 15px; background-color: #FFFFBB;">
                        <asp:TextBox Width="100%" ID="UserName" runat="server" Enabled="true"></asp:TextBox>
                    </td>
                    <td style="border-top: solid 1px black; padding-bottom: 5px; background-color: #FFFFBB;
                        padding-top: 5px; border-right: solid 1px black">
                        <asp:RequiredFieldValidator 
                            ID="UserNameRequired" 
                            runat="server" 
                            Display="Dynamic"
                            ControlToValidate="UserName" 
                            ErrorMessage="Email Address is required." 
                            ToolTip="Email Address is required."
                            ValidationGroup="bcUserProfile" meta:resourceKey="locDay">*</asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator 
                            runat="server" 
                            Display="Dynamic" 
                            ID="vldUserNameRegEx"
                            ErrorMessage="Email address is invalid. Please provide a valid email address." 
                            ToolTip="Email address is required."
                            Text="*" 
                            ControlToValidate="UserName" 
                            ValidationExpression="^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"
                            ValidationGroup="bcUserProfile" meta:resourceKey="locDay">
                        </asp:RegularExpressionValidator>
                        <asp:CustomValidator ID="valUsernameDuplicate" runat="server" 
                            ControlToValidate="UserName" 
                            OnServerValidate="valUsername_ServerValidation"
                            Display="Dynamic"
                            ErrorMessage="The email address that you entered is already in use. Please enter a different email address." 
                            Text="*" 
                            ValidationGroup="bcUserProfile" meta:resourceKey="locDay">
                         </asp:CustomValidator>
                    </td>
                </tr>
                <tr>
                    <td align="right" style="border-bottom: solid 1px black; padding-bottom: 5px; padding-top: 5px;
                        background-color: #FFFFBB; border-left: solid 1px black">
                    </td>
                    <td align="left" style="border-bottom: solid 1px black; padding: 5px 10px 5px 15px;
                        background-color: #FFFFBB;" >
                        <asp:Button ID="btnChangePassword" runat="server" OnClick="btnChangePassword_ButtonClick" PostBackUrl="~/Restricted/Learner/ChangePassword.aspx" CausesValidation="false" Text="Change Password"/>
                    </td>
                    <td align="left" style="border-bottom: solid 1px black; padding-bottom: 5px; padding-top: 5px;
                        background-color: #FFFFBB; border-right: solid 1px black"></td>
                </tr>
                <tr id="trCity" runat="server">
                    <td align="right" style="padding-top: 5px">
                        <asp:Label ID="Label5" runat="server" AssociatedControlID="txtCity" Text="City"></asp:Label>
                        <asp:Label ID="astCity" runat="server" >*</asp:Label>
                    </td>
                    <td style="padding: 5px 10px 5px 15px;">
                        <asp:TextBox Width="100%" ID="txtCity" runat="server"></asp:TextBox>
                    </td>
                    <td style="padding-top: 5px">
                        <asp:RequiredFieldValidator 
                            ID="vldCity" 
                            runat="server" 
                            ControlToValidate="txtCity"
                            ErrorMessage="City is required." 
                            ToolTip="City is required."
                            ValidationGroup="bcUserProfile" meta:resourceKey="locDay">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                 <tr id="trCountry" runat="server">
                    <td align="right">
                        <asp:Label ID="lblCountry" runat="server" AssociatedControlID="ddlCountry" meta:resourceKey="locCountry"></asp:Label>
                        <asp:Label ID="astCountry" runat="server" >*</asp:Label>
                    </td>
                    <td style="padding: 5px 10px 5px 15px;"> 
                    <asp:DropDownList ID="ddlCountry" runat="server"   CssClass="formpulldownwide" 
                       onchange="updateState(this.id,null);">
                                               <asp:ListItem Value=""></asp:ListItem>
                                                <asp:ListItem Value="CAN" meta:resourceKey="locCanada"></asp:ListItem>
                                                 <asp:ListItem Value="MEX" meta:resourceKey="locMexico"></asp:ListItem>
                                                  <asp:ListItem Value="USA" meta:resourceKey="locUnitedStates"></asp:ListItem>
                     </asp:DropDownList>
                     </td>
                     <td style="padding-top: 5px">
                         <asp:RequiredFieldValidator ID="vldCountry" runat="server" 
                                ControlToValidate="ddlCountry"
                                InitialValue=""
                                ErrorMessage="Country is required." 
                                ToolTip="Country is required."
                                Display="Dynamic"
                                ValidationGroup="bcUserProfile" meta:resourceKey="locDay">*</asp:RequiredFieldValidator>                
                    </td>
                </tr>       
                <tr id="trAge" runat="server">
                    <td style="padding-bottom:10px" align="right">
                        <asp:Label runat="server" ID="lblAge" Text="Age">
                        </asp:Label>
                    </td>
                    <td style="padding-bottom:10px" colspan="2">
                        <asp:RadioButtonList runat="server" RepeatColumns="2" RepeatDirection="Horizontal"
                            ID="rlAge" Width="100%">
                            <asp:ListItem>18-23</asp:ListItem>
                            <asp:ListItem>24-29</asp:ListItem>
                            <asp:ListItem>30-39</asp:ListItem>
                            <asp:ListItem>40-49</asp:ListItem>
                            <asp:ListItem>50+</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr id="trGender" runat="server">
                    <td style="padding-bottom:10px" align="right">
                        <asp:Label runat="server" ID="lblGender" Text="Gender">
                        </asp:Label>
                    </td>
                    <td style="padding-bottom:10px" colspan="2">
                        <asp:RadioButtonList runat="server" RepeatColumns="0" ID="rlGender" RepeatDirection="Horizontal"
                            Width="100%">
                            <asp:ListItem Text="Male"></asp:ListItem>
                            <asp:ListItem Text="Female"></asp:ListItem>
                            <asp:ListItem Text="Decline"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr id="trExperience" runat="server">
                    <td style="padding-bottom:10px" align="right">
                        <asp:Label runat="server" ID="lblExperience" Text="Experience">
                        </asp:Label>
                    </td>
                    <td style="padding-bottom:10px" colspan="2">
                        <asp:RadioButtonList runat="server" RepeatColumns="2" RepeatDirection="Horizontal"
                            Width="100%" ID="rlExperience">
                            <asp:ListItem meta:resourceKey="locNone"></asp:ListItem>
                            <asp:ListItem>< 1 year</asp:ListItem>
                            <asp:ListItem>1-3 years</asp:ListItem>
                            <asp:ListItem>4-8</asp:ListItem>
                            <asp:ListItem>9-12</asp:ListItem>
                            <asp:ListItem>13+</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr id="trPositions" runat="server">
                    <td style="padding-bottom:10px" align="right">
                        <asp:Label runat="server" ID="lblPositions" text="Occupation">
                        </asp:Label>
                    </td>
                    <td style="padding-bottom:10px" colspan="2">
                        <asp:CheckBoxList ID="clPositions" runat="server" RepeatColumns="2" RepeatDirection="Horizontal">
                            <asp:ListItem Value="Corporate Manager" meta:resourceKey="locCorpMan"></asp:ListItem>
                            <asp:ListItem Value="Field Manager" meta:resourceKey="locFieldMan"></asp:ListItem>
                            <asp:ListItem Value="Event Manager" meta:resourceKey="locEventMan"></asp:ListItem>
                            <asp:ListItem Value="Sales" meta:resourceKey="locSales"></asp:ListItem>
                            <asp:ListItem Value="Merchandising" meta:resourceKey="locMerch"></asp:ListItem>
                        </asp:CheckBoxList>
                    </td>
                </tr>
                <tr>
                    <td align="right" colspan="3">
                        <asp:Button runat="server" ID="btnCancel" OnClick="btnCancel_ButtonClick" CausesValidation="false" ValidationGroup="bcUserProfile" Text="Cancel"/>                    
                        <asp:Button runat="server" ID="btnSave" OnClick="btnSave_ButtonClick" CausesValidation="true" ValidationGroup="bcUserProfile" Text="Save"/>
                    </td>
                </tr>
            </table>
            </TemplateItems>
            </uc:bC>
</asp:Content>