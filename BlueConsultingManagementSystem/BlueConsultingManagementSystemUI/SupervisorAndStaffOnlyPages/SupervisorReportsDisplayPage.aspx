﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SupervisorReportsDisplayPage.aspx.cs" Inherits="BlueConsultingManagementSystemUI.SupervisorAndStaffOnlyPages.SupervisorReportsDisplayPage" %>

<!DOCTYPE html>
<link rel="stylesheet" href="../css/css/bootstrap-theme.css" type="text/css">
<link rel="stylesheet" href="../css/css/bootstrap.min.css" type="text/css" />

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <div class="navbar navbar-inverse">

    <div class="container">
        <div class="navbar-header">
            <button class="navbar-toggle" data-target=".navbar-collapse" data-toggle="collapse" type="button">
                <span class="sr-only">

                    Toggle navigation

                </span>
                <span class="icon-bar"></span>
                <span class="icon-bar"></span>
                <span class="icon-bar"></span>
            </button>
            <a class="navbar-brand" href="../index.aspx">

                Blue Management Expense reporting

            </a>
        </div>
        <div class="navbar-collapse collapse">
            <ul class="nav navbar-nav">
                <li class="dropdown">
                    <a class="dropdown-toggle" data-toggle="dropdown" href="#">

                        Supervisor 

                        <b class="caret"></b>
                    </a>
                    <ul class="dropdown-menu">
                        <li>
                            <a href="SupervisorUnapprovedResults.aspx">

                                Unapproved expenses 

                            </a>
                        </li>
                        <li>
                            <a href="SupervisorExpenseTotalPage.aspx">

                                Department Budget

                            </a>
                        </li>
                        <li>
                            <a href="SupervisorOnlyPages/SupervisorRejectedReportsPage.aspx">

                                Rejected expenses

                            </a>
                        </li>
                                        
                    </ul>
                </li>
            </ul>
        </div>
        </div>
</div>
     <div class="modal-dialog">
        <div class ="panel panel-primary"  >  
            <div class="panel-heading">
                <h1 class="panel-title"> Blue management Consultant expense -
        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>

                </h1>
                </div>
             <div id="padSpacer" align="center">
    <form id="form1" runat="server">
      <div>
    </div>
        <asp:Label ID="Label2" runat="server" Text="Label"></asp:Label>

            <br />

        <br />

        <asp:GridView ID="DisplayResultsGridSQLConnection" runat="server" onrowcommand="DisplayResultsGridSQLConnection_RowCommand" CssClass="table-responsive table-condensed">
            <Columns>
                <asp:ButtonField ButtonType="Button" CommandName="Select" HeaderText="PDFFile" ShowHeader="True" Text="Receipt" ControlStyle-CssClass="btn btn-primary"  />
            </Columns>
        </asp:GridView>
    </div>

        <asp:Label ID="CurrentAmount" runat="server" Text=""></asp:Label>
        <br />
        <br />
        <asp:Button ID="ApproveButton" runat="server" Text="Approve" OnClick="ApproveButton_Click" CssClass="btn-success" />
        <asp:Button ID="DenyButton" runat="server" Text="Deny" OnClick="DenyButton_Click" CssClass="btn-danger"/>
             <br />
        
             <br />
             <asp:Label ID="ConfirmLabel" runat="server" Text="Are you sure you want to confirm?" Visible="false"></asp:Label>
             <br />
             <asp:Button ID="ConfirmButton" runat="server" Text="Confirm" Visible=" false" OnClick="ConfirmButton_Click" CssClass="btn-info" />
             <br />
    </form>
                     </div>
         </div>
    </div>
</body>
            <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.0/jquery.min.js"></script>
    <script src="../css/js/bootstrap.min.js"></script>
    <script src="../css/js/docs.min.js"></script>
</html>
