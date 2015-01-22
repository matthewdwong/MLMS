<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MemberSearch.aspx.cs" MasterPageFile="~/MasterPage/TemplateDefault.Master" Inherits="MLMS.User.MemberSearch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph1" runat="server">
    <div id="searchPanel" runat="server">
        <div style="float: left; width: 50%;">
            <asp:Label runat="server" ID="lblFirstNameSearch" Text="First Name"></asp:Label>
            <asp:TextBox runat="server" ID="txtBxFirstNameSearch"></asp:TextBox>
        </div>
        <div style="float: left;">
            <asp:Label runat="server" ID="lblLastNameSearch" Text="Last Name"></asp:Label>
            <asp:TextBox runat="server" ID="txtBxLastNameSearch"></asp:TextBox>
        </div>
        <div style="clear: both; float: left; width: 50%;">
            <asp:Label runat="server" ID="lblPhoneNumberSearch" Text="Phone Number"></asp:Label>
            <asp:TextBox runat="server" ID="txtBxPhoneNumberSearch"></asp:TextBox>
        </div>
        <div style="float: left;">
            <asp:Label runat="server" ID="lblPreferredCallBackSearch" Text="Preferred Call Back"></asp:Label>
            <asp:DropDownList runat="server" ID="ddlPreferredCallBackSearch"></asp:DropDownList>
        </div>
        <div style="clear: both; float: left; width: 50%"></div>
        <div style="float: left; width: 50%;">
            <asp:Button runat="server" ID="btnSearch" OnClick="btnSearch_Click" Text="Search" />
        </div>
    </div>
    <div style="clear: both;">
        <asp:GridView runat="server" ID="grdMembers" GridLines="Both" AutoGenerateColumns="false" AllowPaging="true" OnPageIndexChanging="grdMembers_PageIndexChanging" PageSize="25"
            AllowSorting="false" OnRowCommand="grdMembers_RowCommand" DataKeyNames="MemberID" OnRowDeleting="grdMembers_RowDeleting">
            <Columns>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <label>Details</label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Button runat="server" ID="btnDetails" Text="Details" CommandName="Select" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <label>Name</label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblName" Text='<%#Eval("FullName")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <label>Parent/Guardian Name</label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblPGFullName" Text='<%#Eval("PGFullName")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <label>Number</label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblPrimNumber" Text='<%#Eval("PrimaryNumber")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <label>Preferred Call Back Time</label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblPCB" Text='<%#Eval("PreferredCallBack")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <label>Meeting Date</label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%--<asp:Label runat="server" ID="lblIntroMeeting" Text='<%#Eval("IntroMeetDate", "{0:M-dd-yyyy}")%>'></asp:Label>--%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <label>Delete</label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Button runat="server" ID="btnDelete" Text="Delete" CommandName="Delete" OnClientClick="return confirm('Do you want to delete this lead ? ');" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
