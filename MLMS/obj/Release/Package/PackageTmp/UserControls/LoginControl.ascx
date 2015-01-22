<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LoginControl.ascx.cs" Inherits="MLMS.UserControls.LoginControl" %>
    <div id="divLoginControl">
        <asp:Login ID="lgnControl" runat="server" 
            TitleText="" 
            PasswordRecoveryText="Forgot Password?" 
            PasswordRecoveryUrl="~/PublicPages/PasswordRecovery.aspx" 
            OnLoggedIn="lgnControl_LoggedIn"
            Width="100%" 
            FailureAction="Refresh" 
            FailureText="Invalid Email Address or Password."
            Height="100%"
            TextLayout="TextOnTop"
            LoginButtonText="Sign In"
            DisplayRememberMe="false"
            OnLoginError="lgnControl_LoginError"            
            OnInit="lgnControl_OnInit"
             CssClas="loginControl"        
            >           
            <LayoutTemplate>
                <table border="0" cellpadding="0" cellspacing="0" 
                    style="border-collapse:collapse;height:100%;width:100%;">
                    <tr>
                        <td>
                            <table border="0" cellpadding="0" cellspacing="0" style="height:100%;width:100%;">
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName" Text="Email"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:TextBox ID="UserName" runat="server" CssClass="loginControlTextBox" Width="94%" TabIndex="1"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" 
                                            ControlToValidate="UserName" ErrorMessage="User Name is required." 
                                            ToolTip="User Name is required." ValidationGroup="ctl00$lgnControl">*</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password" Text="Password"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:TextBox ID="Password" runat="server" CssClass="loginControlTextBox" TextMode="Password" Width="94%" TabIndex="2"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" 
                                            ControlToValidate="Password" ErrorMessage="Password is required." 
                                            ToolTip="Password is required." ValidationGroup="ctl00$lgnControl">*</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" style="color:Red;" colspan="2">
                                        &nbsp;<asp:Label ID="FailureText" runat="server" ForeColor="Red" Width="94%"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:HyperLink ID="PasswordRecoveryLink" runat="server" 
                                            NavigateUrl="~/Public/PasswordRecovery.aspx" Text="Forgot Password"></asp:HyperLink>
                                    </td>
                                    <td align="right">
                                        <asp:Button ID="LoginButton" runat="server" CssClass="loginControlSignIn"
                                         CommandName="Login" ValidationGroup="ctl00$lgnControl" TabIndex="3" Text="Sign In" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </LayoutTemplate>
        </asp:Login>    
    <div style="height:auto; padding-top:5px; padding-bottom:0px; font-weight:bold; vertical-align:bottom; text-align:right;">
        <br />
        <asp:Literal ID="litHelpText" runat="server"></asp:Literal>
        <asp:CheckBox runat="server" ID="chkRememberMe" Text="Remember Me" Checked="true" TextAlign="Left" />
    </div>

    <div style="font-size:15px; height:auto; padding:0px; font-weight:bold; vertical-align:bottom">
        <br  />
            <div style="text-align:right;padding:0px; font-weight:normal">
        <asp:HyperLink ID="hlRegister" runat="server" Font-Underline="true" Text="Register" NavigateUrl="~/Public/Register.aspx"></asp:HyperLink>
    </div>
   
    
    </div>
</div>
