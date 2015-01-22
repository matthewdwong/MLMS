<%@ Page Language="C#" MasterPageFile="~/MasterPage/TemplateDefault.Master" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="MLMS.User.Search" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph1" runat="server">
    <script type="text/javascript">
        function confirmDelete() {
            showLoading();
            var response = confirm('Do you want to delete this member/customer ? ');
            if (response)
                return true;
            else
            {
                hideLoading();
                return false;
            }
        }
    </script>
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
        <div style="clear:both; float:left; width:50%"></div>
        <div style="float:left; width:50%;">
            <asp:Button runat="server" ID="btnSearch" OnClick="btnSearch_Click" Text="Search" OnClientClick="showLoading();" />
            <asp:Button runat="server" ID="btnAddNew" OnClick="btnAddNew_Click" Text="Add New" OnClientClick="showLoading();" />
        </div>
    </div>
    <div style="clear:both;"></div>
    <asp:GridView runat="server" ID="grdMembersCustomer" GridLines="Both" AutoGenerateColumns="false" AllowPaging="true" OnPageIndexChanging="grdMembersCustomer_PageIndexChanging" PageSize="25"
            AllowSorting="false" OnRowCommand="grdMembersCustomer_RowCommand" DataKeyNames="MemberID" OnRowDeleting="grdMembersCustomer_RowDeleting">
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
                        <label>Delete</label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Button runat="server" ID="btnDelete" Text="Delete" CommandName="Delete" OnClientClick="return confirmDelete();" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
    </asp:GridView>
</asp:Content>
