<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserControlBase.ascx.cs" Inherits="MLMS.UserControls.UserControlBase" className="UserControlBase" %>
<div id="divUCContent">
    <div class="divUCHeader">
        <asp:Label ID="lblHeader" runat="server" onload="lblHeader_Load"></asp:Label>
    </div>
    <div id="divUCBody" runat="server" class="ucBody">
        <asp:PlaceHolder ID="phBody" runat="server"></asp:PlaceHolder>
    </div>
</div>