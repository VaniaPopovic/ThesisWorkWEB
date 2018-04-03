<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Results.aspx.cs" Inherits="NQL_Thesis.Results" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="https://code.highcharts.com/highcharts.js"></script>
    <script src="https://code.highcharts.com/modules/exporting.js"></script>
    <asp:TextBox ID="multitxt" runat="server" TextMode="MultiLine" CssClass="form-control" Rows="10"></asp:TextBox>
    
    <br/>
    <br/>
    
    <asp:GridView ID="gridView" runat="server" Visible="False" CssClass="table table-hover table-striped" GridLines="None">
        <RowStyle CssClass="cursor-pointer" />

    </asp:GridView>
  
        <asp:Literal Visible="False" ID="ltrChart" runat="server"></asp:Literal>
    
    <asp:Literal ID="barChart" runat="server"></asp:Literal>

</asp:Content>
