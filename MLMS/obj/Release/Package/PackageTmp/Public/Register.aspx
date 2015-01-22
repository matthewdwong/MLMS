<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="MLMS.Public.Register"
    MasterPageFile="~/MasterPage/Default.Master" EnableEventValidation="false" %>

<%@ Register TagPrefix="cc1" Namespace="MLMS.Common"
    Assembly="MLMS.Common" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtoolkit" %>


<asp:Content ID="Content1" ContentPlaceHolderID="cphBody" runat="server">


    <!-- The country default -->
    <input type="hidden" value="USA" name="countryDefault" id="countryDefault" />

    <asp:CreateUserWizard ID="ctrlCreateUser" EnableClientScript="false" runat="server"
        OnCreatingUser="ctrlCreateUser_CreatingUser"
        OnCreatedUser="ctrlCreateUser_CreatedUser"
        OnContinueButtonClick="ctrlCreateUser_ContinueButtonClick"
        FinishDestinationPageUrl="~/User/Leads.aspx"
        PasswordRegularExpression="(?=.*[a-z])(?=.*[\d])^([a-zA-Z0-9]{6,15})$"
        PasswordRegularExpressionErrorMessage="Password length minimum: 6. At least 1 digit and 1 character required."
        CreateUserButtonText="Next"
        StartNextButtonType="Button"
        StepPreviousButtonType="Button"
        StepPreviousButtonText="Previous"
        Width="100%"
        InvalidPasswordErrorMessage="Please enter in a case sensitive password that is at least 6 characters long with at least one number"
        DuplicateUserNameErrorMessage="The email address that you entered is already in use. Please enter a different e-mail address."
        UserNameRequiredErrorMessage="Email is required.">
        <StepNavigationTemplate>
            <asp:Button ID="StepNextButton" runat="server" CommandName="MoveNext" CausesValidation="true"
                Text="Next" ValidationGroup="ctrlCreateUser" />
        </StepNavigationTemplate>
        <WizardSteps>
            <asp:CreateUserWizardStep runat="server" ID="wizAccountInfo" Title="Account Information">
                <ContentTemplate>
                    <asp:Table runat="server" border="0" Style="border-collapse: collapse" Width="100%">
                        <asp:TableRow runat="server">
                            <asp:TableCell runat="server" align="center" colspan="3" Style="padding-bottom: 10px">
                                                <asp:Label id="lblSignUp" Text="Sign Up"></asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="trValidation" runat="server">
                            <asp:TableCell runat="server" align="center" colspan="3" Style="color: Red;">
                                <asp:ValidationSummary ID="vldSummary" runat="server"
                                    DisplayMode="BulletList"
                                    EnableViewState="false"
                                    HeaderText="The following errors occurred:"
                                    ShowSummary="true"
                                    ValidationGroup="ctrlCreateUser"
                                    meta:resourceKey="locVldSumm" />

                                <asp:Literal ID="ErrorMessage" runat="server" EnableViewState="False" Visible="false"></asp:Literal>
                                <asp:CompareValidator ID="PasswordCompare" runat="server"
                                    ControlToCompare="Password"
                                    ControlToValidate="ConfirmPassword"
                                    Display="Dynamic"
                                    ErrorMessage="The Password and Confirmation Password must match."
                                    ValidationGroup="ctrlCreateUser"
                                    meta:resourceKey="locPassCompare"></asp:CompareValidator>

                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow runat="server" ID="trFirstName">
                            <asp:TableCell runat="server" align="right" Width="30%">
                                <asp:Label ID="lblFirstName" runat="server" AssociatedControlID="txtFirstName" Text="First Name"></asp:Label>
                                <asp:Label ID="astFirstName" runat="server">*</asp:Label>
                            </asp:TableCell>
                            <asp:TableCell runat="server" Style="padding: 5px 10px 5px 15px;" Width="60%">
                                <asp:TextBox ID="txtFirstName" runat="server"
                                    CausesValidation="true"
                                    Width="100%"
                                    AutoCompleteType="FirstName"></asp:TextBox>
                            </asp:TableCell>
                            <asp:TableCell runat="server" Width="10%">
                                <asp:RequiredFieldValidator ID="vldFirstName" runat="server"
                                    Display="Dynamic"
                                    ControlToValidate="txtFirstName"
                                    ErrorMessage="First Name is required."
                                    ToolTip="First Name is required."
                                    ValidationGroup="ctrlCreateUser"
                                    meta:resourceKey="locDay">*</asp:RequiredFieldValidator>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow runat="server" ID="trLastName">
                            <asp:TableCell runat="server" align="right">
                                <asp:Label ID="Label2" runat="server" AssociatedControlID="txtLastName" Text="Last Name"></asp:Label>
                                <asp:Label ID="astLastName" runat="server"> *</asp:Label>
                            </asp:TableCell>
                            <asp:TableCell runat="server" Style="padding: 5px 10px 10px 15px;">
                                <asp:TextBox ID="txtLastName" runat="server"
                                    Width="100%"
                                    AutoCompleteType="LastName"></asp:TextBox>
                            </asp:TableCell>
                            <asp:TableCell runat="server">
                                <asp:RequiredFieldValidator ID="vldLastName" runat="server"
                                    Display="Dynamic"
                                    ControlToValidate="txtLastName"
                                    ErrorMessage="Last Name is required."
                                    ToolTip="Last Name is required."
                                    ValidationGroup="ctrlCreateUser"
                                    meta:resourceKey="locDay">*</asp:RequiredFieldValidator>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow runat="server">
                            <asp:TableCell runat="server" Width="30%" align="right" Style="border-top: solid 1px black; padding-bottom: 5px; padding-top: 5px; background-color: #FFFFBB; border-left: solid 1px black">
                                <asp:Label ID="lblEmail" runat="server" Text="Email Address"></asp:Label>
                                <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName"> *</asp:Label>
                            </asp:TableCell>
                            <asp:TableCell runat="server" Style="background-color: #FFFFBB; padding: 5px 10px 5px 15px; border-top: solid 1px black;" Width="60%">
                                <asp:TextBox ID="UserName" runat="server"
                                    Enabled="true"
                                    Width="100%"
                                    AutoCompleteType="Email"></asp:TextBox>
                            </asp:TableCell>
                            <asp:TableCell runat="server" Style="border-top: solid 1px black; background-color: #FFFFBB; border-right: solid 1px black;" Width="10%">
                                <asp:RequiredFieldValidator ID="UserNameRequired" runat="server"
                                    Display="Dynamic"
                                    ControlToValidate="UserName"
                                    ErrorMessage="Email is required."
                                    ToolTip="Email is required."
                                    ValidationGroup="ctrlCreateUser"
                                    meta:resourceKey="locDay">*</asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator runat="server" ID="vldUserNameRegEx"
                                    Display="Dynamic"
                                    ErrorMessage="Email address is invalid. Please provide a valid email address."
                                    ToolTip="Email address is required."
                                    Text="*"
                                    ControlToValidate="UserName"
                                    ValidationGroup="ctrlCreateUser"
                                    meta:resourceKey="locDay"
                                    ValidationExpression="^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$">
                                </asp:RegularExpressionValidator>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow runat="server">
                            <asp:TableCell runat="server" align="right" Style="padding-bottom: 5px; padding-top: 5px; background-color: #FFFFBB; border-left: solid 1px black">
                                <asp:Label ID="lblConfirmEmail" runat="server" Text="Confirm Email Address"></asp:Label>
                                <asp:Label ID="EmailLabel" runat="server" AssociatedControlID="Email"> *</asp:Label>
                            </asp:TableCell>
                            <asp:TableCell runat="server" Style="padding: 5px 10px 5px 15px; background-color: #FFFFBB;">
                                <asp:TextBox ID="Email" runat="server" Width="100%"></asp:TextBox>
                            </asp:TableCell>
                            <asp:TableCell runat="server" Style="border-right: solid 1px black; background-color: #FFFFBB;">
                                <asp:RequiredFieldValidator ID="EmailRequired" runat="server"
                                    ControlToValidate="Email"
                                    Display="Dynamic"
                                    ErrorMessage="Email is required."
                                    ToolTip="Email is required."
                                    ValidationGroup="ctrlCreateUser"
                                    meta:resourceKey="locDay">*</asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="vldEmailCompare" runat="server"
                                    Display="Dynamic"
                                    ControlToValidate="UserName"
                                    ControlToCompare="Email"
                                    ErrorMessage="Email address does not match."
                                    Text="*"
                                    ValidationGroup="ctrlCreateUser"
                                    meta:resourceKey="locDay" />
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow runat="server">
                            <asp:TableCell runat="server" align="right" Style="border-left: solid 1px black; background-color: #FFFFBB;">
                                <asp:Label ID="lblPass" runat="server" Text="Password"></asp:Label>
                                <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password"> *</asp:Label>
                            </asp:TableCell>
                            <asp:TableCell runat="server" Style="background-color: #FFFFBB; padding: 5px 10px 5px 15px;">
                                <asp:TextBox ID="Password" runat="server"
                                    TextMode="Password"
                                    Width="100%"></asp:TextBox>
                            </asp:TableCell>
                            <asp:TableCell runat="server" Style="border-right: solid 1px black; background-color: #FFFFBB;">
                                <asp:RequiredFieldValidator ID="PasswordRequired" runat="server"
                                    ControlToValidate="Password"
                                    ErrorMessage="Password is required."
                                    Display="Dynamic"
                                    ToolTip="Password is required."
                                    ValidationGroup="ctrlCreateUser"
                                    meta:resourceKey="locDay">*</asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator runat="server" ID="vldPasswordRegEx"
                                    Display="Dynamic"
                                    ControlToValidate="Password"
                                    ErrorMessage="Password must include at least one number, and should be 6 or more characters long. No special characters allowed"
                                    ValidationExpression="(?=.*[a-z])(?=.*[\d])^([a-zA-Z0-9]{6,15})$"
                                    Text="*"
                                    ValidationGroup="ctrlCreateUser"
                                    meta:resourceKey="locDay"
                                    EnableClientScript="false"></asp:RegularExpressionValidator>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow runat="server">
                            <asp:TableCell runat="server" align="right" Style="border-bottom: solid 1px black; padding-bottom: 5px; background-color: #FFFFBB; border-left: solid 1px black">
                                <asp:Label ID="lblConfirmPass" runat="server" Text="Confirm Password"></asp:Label>
                                <asp:Label ID="ConfirmPasswordLabel" runat="server" AssociatedControlID="ConfirmPassword" Text="Confirm Password"> *</asp:Label>
                            </asp:TableCell>
                            <asp:TableCell runat="server" Style="border-bottom: solid 1px black; background-color: #FFFFBB; padding: 5px 10px 5px 15px;">
                                <asp:TextBox ID="ConfirmPassword" runat="server"
                                    CausesValidation="true"
                                    TextMode="Password"
                                    Width="100%"></asp:TextBox>
                            </asp:TableCell>
                            <asp:TableCell runat="server" Style="border-bottom: solid 1px black; background-color: #FFFFBB; border-right: solid 1px black;">
                                <asp:RequiredFieldValidator ID="ConfirmPasswordRequired" runat="server"
                                    Display="Dynamic"
                                    ControlToValidate="ConfirmPassword"
                                    ErrorMessage="Confirm Password is required."
                                    ToolTip="Confirm Password is required."
                                    meta:resourceKey="locDay"
                                    ValidationGroup="ctrlCreateUser">*</asp:RequiredFieldValidator>

                            </asp:TableCell>
                        </asp:TableRow>

                        <asp:TableRow runat="server" ID="trCity">
                            <asp:TableCell runat="server" align="right">
                                <asp:Label ID="Label5" runat="server" AssociatedControlID="txtCity" Text="City"></asp:Label>
                                <asp:Label ID="astCity" runat="server">*</asp:Label>
                            </asp:TableCell>
                            <asp:TableCell runat="server" Style="padding: 5px 10px 5px 15px;">
                                <asp:TextBox ID="txtCity" runat="server"
                                    Width="100%"
                                    AutoCompleteType="HomeCity"></asp:TextBox>
                            </asp:TableCell>
                            <asp:TableCell runat="server">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                    ControlToValidate="txtCity"
                                    ErrorMessage="City is required."
                                    ToolTip="City is required."
                                    ValidationGroup="ctrlCreateUser"
                                    meta:resourceKey="locDay">*</asp:RequiredFieldValidator>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow runat="server" ID="trCountry">
                            <asp:TableCell runat="server" align="right">
                                <asp:Label ID="lblCountry" runat="server" AssociatedControlID="ddlCountry" Text="Country"></asp:Label>
                                <asp:Label ID="astCountry" runat="server">*</asp:Label>
                            </asp:TableCell>
                            <asp:TableCell runat="server" Style="padding: 5px 10px 5px 15px;">
                                <asp:DropDownList ID="ddlCountry" runat="server" CssClass="formpulldownwide"
                                    onchange="updateState(this.id);">
                                    <asp:ListItem></asp:ListItem>
                                    <asp:ListItem Value="CAN" meta:resourceKey="locCanada"></asp:ListItem>
                                    <asp:ListItem Value="MEX" meta:resourceKey="locMexico"></asp:ListItem>
                                    <asp:ListItem Value="USA" meta:resourceKey="locUnitedStates"></asp:ListItem>
                                </asp:DropDownList>
                            </asp:TableCell>
                            <asp:TableCell ID="TableCell1" runat="server">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server"
                                    ControlToValidate="ddlCountry"
                                    InitialValue=""
                                    ErrorMessage="Country is required."
                                    ToolTip="Country is required."
                                    Display="Dynamic"
                                    ValidationGroup="ctrlCreateUser"
                                    meta:resourceKey="locDay">*</asp:RequiredFieldValidator>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow runat="server">
                            <asp:TableCell runat="server" colspan="3" Style="padding: 10px 3px 10px 3px; border: solid 1px black">
                                <div id="divPolicy" style="HEIGHT: 150px; WIDTH: 100%; OVERFLOW: auto">
                                    <asp:Literal ID="litPrivacy" runat="server"></asp:Literal>
                                </div>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow runat="server">
                            <asp:TableCell runat="server" align="left" colspan="3" Style="padding: 10px 3px 10px 3px; font-size: small">
                                <asp:CheckBox runat="server" ID="chkPrivacyAgreement" Text="Agree to Terms of Use and Privacy Agreement" />
                                <asp:CustomValidator runat="server"
                                    ErrorMessage="You must agree to the Terms of Use & Privacy Agreement."
                                    Display="Dynamic"
                                    Text="*"
                                    OnServerValidate="vldPrivacyAgreement_ServerValidate"
                                    ValidationGroup="ctrlCreateUser"
                                    meta:resourceKey="locDay" />
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </ContentTemplate>
                <CustomNavigationTemplate>
                    <table border="0" cellspacing="5" style="width: 100%; height: 100%;">
                        <tr align="right">
                            <td align="right" colspan="0">
                                <asp:Button ID="StepNextButton" runat="server" CommandName="MoveNext"
                                    ValidationGroup="ctrlCreateUser" CausesValidation="true" Text="Next" />
                            </td>
                        </tr>
                    </table>
                </CustomNavigationTemplate>
            </asp:CreateUserWizardStep>
            <asp:CompleteWizardStep runat="server" Title="Registration Complete" ID="wizComplete">
                <ContentTemplate>
                    <asp:Table ID="Table1" runat="server" style="margin:0px auto;">
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:Label runat="server" ID="lblCompleteReg" Text="You have successfully registered"></asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:Button ID="ContinueButton" runat="server" CausesValidation="true" CommandName="Continue"
                                    ValidationGroup="ctrlDemographic" Text="Continue" />
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </ContentTemplate>
            </asp:CompleteWizardStep>
        </WizardSteps>
    </asp:CreateUserWizard>

</asp:Content>
