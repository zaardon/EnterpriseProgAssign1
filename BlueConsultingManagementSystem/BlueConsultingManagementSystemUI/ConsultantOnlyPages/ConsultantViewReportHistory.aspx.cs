﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BlueConsultingManagementSystemLogic;

namespace BlueConsultingManagementSystemUI.ConsultantOnlyPages
{
    public partial class ConsultantViewReportHistory : System.Web.UI.Page
    {
        private string reportName;
        private string deptName;
        private readonly int REP_POS = 1;
        private readonly int DEPT_POS = 2;

        protected void Page_Load(object sender, EventArgs e)
        {
            string reportType = (string)Session["reportType"]; //Retrieves the type of report selected on the previous page...

            if (reportType == "AllSubmitted")
                LoadSubmittedReports();
            else if (reportType == "AllApproved")
                LoadApprovedReports();
            else if (reportType == "InProgress")
                LoadInProgressReports();            
        }

        private void LoadSubmittedReports()
        {
            ConsultantHistorySQLConnection.DataSource = new DatabaseHandler().ReturnConsultantSubmittedReports(User.Identity.Name);
            ConsultantHistorySQLConnection.DataBind();
        }

        private void LoadApprovedReports()
        {
            ConsultantHistorySQLConnection.DataSource = new DatabaseHandler().ReturnConsultantApprovedReports(User.Identity.Name);
            ConsultantHistorySQLConnection.DataBind();
        }

        private void LoadInProgressReports()
        {
            ConsultantHistorySQLConnection.DataSource = new DatabaseHandler().ReturnConsultantInProgressReports(User.Identity.Name);
            ConsultantHistorySQLConnection.DataBind();
        }

        /*
         * Allows the user to view a selected report through the gridview's row command
         */
        protected void ConsultantHistorySQLConnection_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            string currentCommand = e.CommandName;
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow selectedRow = ConsultantHistorySQLConnection.Rows[index];
            reportName = selectedRow.Cells[REP_POS].Text.ToString();
            Session["reportName"] = reportName;
            deptName = selectedRow.Cells[DEPT_POS].Text.ToString();
            Session["deptName"] = deptName;
            Response.Redirect("ConsultantViewReportHistoryExpenses.aspx");
        }

        protected void BackButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("ConsultantMain.aspx");
        }
    }
}