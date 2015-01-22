<%@ Page Title="" Language="C#"
    MasterPageFile="~/MasterPage/TemplateDefault.Master" AutoEventWireup="true"
    CodeBehind="PasswordRecovery.aspx.cs" Inherits="MLMS.Public.PasswordRecovery" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph1" runat="server">
    <div class="mainColumnLeft">
        <table>
            <tr>
                <td>
                    <asp:Localize runat="server" ID="enterEmailText" meta:resourceKey="locEnterEmail" Text="Email Address"></asp:Localize>
                </td>
                <td width="60%">
                    <asp:TextBox runat="server" ID="txtUserName" CausesValidation="true"
                        Width="100%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="padding-top: 15px"></td>
                <td style="padding-top: 15px">
                    <asp:Button runat="server" ID="btnResetPassword" CausesValidation="true" Text="Reset Password"
                        OnClick="btnResetPassword_Click" meta:resourceKey="locBtnReset" />
                </td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <asp:Label runat="server" ID="lblConfirmation" Text="" Visible="false" ForeColor="Red" meta:resourceKey="locConfirm" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
