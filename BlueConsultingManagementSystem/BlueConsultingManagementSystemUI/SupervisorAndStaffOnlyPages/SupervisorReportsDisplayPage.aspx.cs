﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using BlueConsultingManagementSystemLogic;


namespace BlueConsultingManagementSystemUI.SupervisorAndStaffOnlyPages
{
    public partial class SupervisorReportsDisplayPage : System.Web.UI.Page
    {
        public string reportName;
        public string userGroupMember = "";
        public string department = "";
        

        protected void Page_Load(object sender, EventArgs e)
        {
            Label2.Text = User.Identity.AuthenticationType.ToString();
            Label1.Text = "null";
            reportName = Session["reportName"].ToString();
            Label1.Text = reportName;
            fillExpenseTable();
            CurrentAmount.Text = "The total is: $" + getTotalNumber().ToString()+" AUD";

            if (User.IsInRole("Higher Education Services"))
                userGroupMember = "HigherEducation";
            else if (User.IsInRole("Logistic Services"))
                userGroupMember = "LogisticServices";
            else
                userGroupMember = "StateServices";

            if(User.IsInRole("Staff"))
            {
                department = "Staff";
            }
            else if(User.IsInRole("Department Supervisor"))
            {
                department = "DepartmentSupervisor";
            }

        }

        protected void ApproveButton_Click(object sender, EventArgs e)
        {


            if (isUnder())
            {
                approveReport();
                deductBudget();
                Response.Redirect("SupervisorAndStaffMain.aspx");
            }
            else
            {
                ConfirmLabel.Text = "Are you sure you want to proceed? The current department budget is: $" + returnCurrentDeptMoney()+" AUD";
                ConfirmLabel.Visible = true;
                ConfirmButton.Visible = true;
            }
        }

        protected void DenyButton_Click(object sender, EventArgs e)
        {
            denyReport();
            Response.Redirect("SupervisorAndStaffMain.aspx");
        }

        protected bool isUnder()
        {
            if (getTotalNumber() < returnCurrentDeptMoney())
            {                              
                return true;
            }
            else
            {

                return false;
            }
        }

        public double returnCurrentDeptMoney()
        {
            return new DatabaseHandler().ReturnCurrentDepartmentMoney(userGroupMember);
        }

        public double getTotalNumber()
        {
            double totalNumber = 0;
            string colNumb = "";
            string currency = "";

            foreach (GridViewRow row in DisplayResultsGridSQLConnection.Rows)
            {
                //totalNumber += Convert.ToDouble(row.Cells[4].Text.ToString());
                colNumb = row.Cells[4].Text.ToString();
                currency = row.Cells[5].Text.ToString();
                totalNumber += new CurrencyConverter().ConvertCurrencyToAUD(currency, Convert.ToDouble(colNumb));
            }

            return totalNumber;
        }

        public void deductBudget()
        {
            new DatabaseHandler().UpdateDepartmentBudget(userGroupMember, getTotalNumber());
        }

        public void approveReport()
        {
            if (department == "DepartmentSupervisor")
                approveSupervisor();
            else if (department == "Staff")
                approveReportStaff();
        }



        public void denyReport()
        {
            if(department == "DepartmentSupervisor")
                denySupervisor();
            else if (department == "Staff")
                denyReportStaff();
        }

        public void denyReportStaff()
        {

            new DatabaseHandler().DenyReportStaff(reportName);

        }

        public void approveReportStaff()
        {
            new DatabaseHandler().ApproveReportStaff(reportName);
        }

        public void denySupervisor()
        {
            new DatabaseHandler().DenyReportSupervisor(User.Identity.Name, reportName);
        }

        public void approveSupervisor()
        {
            new DatabaseHandler().ApproveReportSupervisor(User.Identity.Name, reportName);
        }

        protected void ConfirmButton_Click(object sender, EventArgs e)
        {
            approveReport();
            deductBudget();
            Response.Redirect("SupervisorAndStaffMain.aspx");
        }

        public void fillExpenseTable()
        {


            DisplayResultsGridSQLConnection.DataSource = new DatabaseHandler().LoadExpenseTable(reportName);
            DisplayResultsGridSQLConnection.DataBind();

        }

        protected void DisplayResultsGridSQLConnection_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {

            string currentCommand = e.CommandName;
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow selectedRow = DisplayResultsGridSQLConnection.Rows[index];

            string name  = selectedRow.Cells[1].Text.ToString();
            string location = selectedRow.Cells[2].Text.ToString();
            string description = selectedRow.Cells[3].Text.ToString();
            double amount = Convert.ToDouble(selectedRow.Cells[4].Text.ToString());
            string currency = selectedRow.Cells[5].Text.ToString();
            



            var connectionString = ConfigurationManager.ConnectionStrings["BlueConsultingDBString"].ConnectionString;
            var con = new SqlConnection(connectionString);
            con.Open();
            using (var sqlQuery = new SqlCommand(@"SELECT [PDF_File] FROM [dbo].[ExpenseDB] WHERE [reportName] = @reportName AND [ConsultantName] = @consultantName AND [Location] = @location AND [Description] = @description AND [Amount] = @amount AND [Currency] = @currency AND PDF_File is not NULL", con))
            {
                sqlQuery.Parameters.AddWithValue("@reportName", reportName);
                sqlQuery.Parameters.AddWithValue("@consultantName", name);
                sqlQuery.Parameters.AddWithValue("@location", location);
                sqlQuery.Parameters.AddWithValue("@description", description);
                sqlQuery.Parameters.AddWithValue("@amount", amount);
                sqlQuery.Parameters.AddWithValue("@currency", currency);

                using (var sqlQueryResult = sqlQuery.ExecuteReader())
                    if (sqlQueryResult != null)
                    {
                        sqlQueryResult.Read();
                        try
                        {
                            var blob = new Byte[(sqlQueryResult.GetBytes(0, 0, null, 0, int.MaxValue))];
                            sqlQueryResult.GetBytes(0, 0, blob, 0, blob.Length);
                            //using (var fs = new FileStream(Server.MapPath("~\\Desktop\\Report.pdf"), FileMode.Create, FileAccess.Write))
                            //    fs.Write(blob, 0, blob.Length);

                            HttpContext.Current.Response.ContentType = "application/pdf";
                            HttpContext.Current.Response.AddHeader("Content-Disposition", "inline;filename =test.pdf");
                            HttpContext.Current.Response.BinaryWrite((byte[])blob);//get data in variable in binary format
                            HttpContext.Current.Response.End();

                        }
                        catch
                        {
                            Response.Write("No PDF File for expense has been added");
                        }

                    }
            }

            
        }

    }
}