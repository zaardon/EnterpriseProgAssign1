﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BlueConsultingManagementSystemLogic
{
    public class DatabaseHandler
    {
        // naming convention
        /*
         * [Person][Command][Name][Query] 
         * 
         * ALLWAYS FOLLOW
         */
        public SqlConnection SQLConnection;
        public DatabaseHandler()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["BlueConsultingDBString"].ConnectionString;
            var con = new SqlConnection(connectionString);
            this.SQLConnection = con;
        }

        public DataSet AllApprovedReports()
        {
            var selectCommand = new SqlCommand("SELECT DISTINCT ReportName, ConsultantName, StatusReport, Dept_type FROM ExpenseDB WHERE StatusReport = 'Approved'", SQLConnection);
            var adapter = new SqlDataAdapter(selectCommand);
            var resultSet = new DataSet();
            adapter.Fill(resultSet);
            SQLConnection.Close();
            return resultSet;
        }

        public void ConsultantsInsertExpenseQuery(string repName, string user, string location, string description, double amount, string currency, string deptType, DateTime dateExp)
        {
            SQLConnection.Open();

            string cmd = "INSERT INTO [dbo].[ExpenseDB] ( [ReportName], [ConsultantName], [StatusReport], [Location], [Description], [Amount], [Currency], [Dept_type], [DateExpense], [TotalAUD]) VALUES(@repName, @user, @status, @location, @description, @amount, @currency, @dept_type, @dateExp, @totalAUD)";
            SqlCommand sqlcmd = new SqlCommand(cmd, SQLConnection);
            sqlcmd.Parameters.AddWithValue("@repName", repName);
            sqlcmd.Parameters.AddWithValue("@user", user);
            sqlcmd.Parameters.AddWithValue("@status", "Submitted");
            sqlcmd.Parameters.AddWithValue("@location", location);
            sqlcmd.Parameters.AddWithValue("@description", description);
            sqlcmd.Parameters.AddWithValue("@amount", amount);
            sqlcmd.Parameters.AddWithValue("@currency", currency);
            sqlcmd.Parameters.AddWithValue("@dept_type", deptType);
            sqlcmd.Parameters.AddWithValue("@dateExp", dateExp);
            sqlcmd.Parameters.AddWithValue("@totalAUD", new CurrencyConverter().ConvertCurrencyToAUD(currency, amount));

            sqlcmd.ExecuteNonQuery();

            SQLConnection.Close();
        }

        public void ConsultantsInsertExpenseQueryWithPDF(string repName, string user, string location, string description, double amount, string currency, string deptType, DateTime dateExp, byte[] file)
        {
            SQLConnection.Open();

            string cmd = "INSERT INTO [dbo].[ExpenseDB] ( [ReportName], [ConsultantName], [StatusReport], [Location], [Description], [Amount], [Currency], [Dept_type], [DateExpense], [PDF_File], [TotalAUD]) VALUES(@repName, @user, @status, @location, @description, @amount, @currency, @dept_type, @dateExp, @PDF_File, @totalAUD)";
            SqlCommand sqlcmd = new SqlCommand(cmd, SQLConnection);
            sqlcmd.Parameters.AddWithValue("@repName", repName);
            sqlcmd.Parameters.AddWithValue("@user", user);
            sqlcmd.Parameters.AddWithValue("@status", "Submitted");
            sqlcmd.Parameters.AddWithValue("@location", location);
            sqlcmd.Parameters.AddWithValue("@description", description);
            sqlcmd.Parameters.AddWithValue("@amount", amount);
            sqlcmd.Parameters.AddWithValue("@currency", currency);
            sqlcmd.Parameters.AddWithValue("@dept_type", deptType);
            sqlcmd.Parameters.AddWithValue("@dateExp", dateExp);
            sqlcmd.Parameters.AddWithValue("@totalAUD", new CurrencyConverter().ConvertCurrencyToAUD(currency, amount));
 
            sqlcmd.Parameters.Add("@PDF_File", SqlDbType.VarBinary, file.Length).Value = file;

            sqlcmd.ExecuteNonQuery();

            SQLConnection.Close();
        }



        public DataSet LoadDepartmentSupervisorData(string userGroupMember)
        {
            SQLConnection.Open();
            var selectCommand = new SqlCommand("SELECT distinct ReportName, ConsultantName FROM ExpenseDB WHERE Dept_Type = '" + userGroupMember + "' AND StatusReport <> 'Approved' AND StatusReport <> 'Declined'", SQLConnection);
            var adapter = new SqlDataAdapter(selectCommand);
            var resultSet = new DataSet();
            adapter.Fill(resultSet);
            SQLConnection.Close();
            return resultSet;
        }

        public double ReturnCurrentDepartmentMoney(string userGroupMember)
        {
            double numb = 0.0;
            var selectCommand = new SqlCommand("SELECT Budget FROM DepartmentDB WHERE Dept_Name = '" + userGroupMember + "'", SQLConnection);
            //ONLY SHOW REPORTNAMES - DONT LET IT REPEAT ITSELF WITH THE OTHER INFO
            var adapter = new SqlDataAdapter(selectCommand);

            var resultSet = new DataSet();
            adapter.Fill(resultSet);

            numb = Convert.ToDouble(resultSet.Tables[0].Rows[0].ItemArray[0]);
            SQLConnection.Close();
            return numb;
        }

         public double ReturnDepartmentExpensesMade(string userGroupMember)
        {
            double tempNumb = 0.0;
            double numb = 0;
            int i = 0;
            string currency = "";

            var selectCommand = new SqlCommand("SELECT distinct SUM(TotalAUD) FROM ExpenseDB WHERE Dept_type = '" + userGroupMember + "' AND StatusReport = 'Approved' AND (StaffApproved = 'YES' OR StaffApproved IS NULL)", SQLConnection);
            //ONLY SHOW REPORTNAMES - DONT LET IT REPEAT ITSELF WITH THE OTHER INFO
            var adapter = new SqlDataAdapter(selectCommand);

            var resultSet = new DataSet();
            adapter.Fill(resultSet);

            SQLConnection.Close();
            return (Convert.ToDouble(resultSet.Tables[0].Rows[i].ItemArray[0]));

        }



        public void UpdateDepartmentBudget(string userGroupMember, double total)
        {
            SQLConnection.Open();
            double result = (ReturnCurrentDepartmentMoney(userGroupMember) - total);
            var selectCommand = new SqlCommand("UPDATE [dbo].[DepartmentDB] SET Budget = " + result + " WHERE Dept_Name = '" + userGroupMember + "'", SQLConnection);
            var adapter = new SqlDataAdapter(selectCommand);

            var resultSet = new DataSet();
            adapter.Fill(resultSet);
            SQLConnection.Close();

        }

        //public void DenyReportStaff(string reportName)
        //{
        //    SQLConnection.Open();
        //    var selectCommand = new SqlCommand("UPDATE [dbo].[ExpenseDB] SET StaffApproved = 'NO' WHERE ReportName = '" + reportName + "'", SQLConnection);
        //    var adapter = new SqlDataAdapter(selectCommand);
        //    var resultSet = new DataSet();
        //    adapter.Fill(resultSet);
        //    SQLConnection.Close();
        //}

        public void DenyReportStaff(string reportName, double returnedMoney)
        {
            SQLConnection.Open();
            var selectCommand = new SqlCommand("UPDATE [dbo].[ExpenseDB] SET StaffApproved = 'NO' WHERE ReportName = '" + reportName + "'", SQLConnection);
            var adapter = new SqlDataAdapter(selectCommand);
            var resultSet = new DataSet();
            adapter.Fill(resultSet);
            SQLConnection.Close();

            SQLConnection.Open();
            selectCommand = new SqlCommand("SELECT distinct reportName, Dept_type FROM ExpenseDB WHERE ReportName = '" + reportName + "'", SQLConnection);
            adapter = new SqlDataAdapter(selectCommand);
            resultSet = new DataSet();
            adapter.Fill(resultSet);
            SQLConnection.Close();

            string dept;
            try
            {
                dept = (resultSet.Tables[0].Rows[0].ItemArray[1]).ToString();

            }
            catch
            {
                dept = "";

            }

            SQLConnection.Open();
            selectCommand = new SqlCommand("UPDATE [dbo].[DepartmentDB] SET Budget = Budget + "+returnedMoney+" WHERE Dept_Name = '" + dept + "'", SQLConnection);
            adapter = new SqlDataAdapter(selectCommand);
            resultSet = new DataSet();
            adapter.Fill(resultSet);
            SQLConnection.Close();
        }

        public void ApproveReportStaff(string reportName)
        {
            SQLConnection.Open();
            var selectCommand = new SqlCommand("UPDATE [dbo].[ExpenseDB] SET StaffApproved = 'YES' WHERE ReportName = '" + reportName + "'", SQLConnection);
            var adapter = new SqlDataAdapter(selectCommand);

            var resultSet = new DataSet();
            adapter.Fill(resultSet);

            SQLConnection.Close();
        }

        public void DenyReportSupervisor(string name, string reportName)
        {
            SQLConnection.Open();
            var selectCommand = new SqlCommand("UPDATE [dbo].[ExpenseDB] SET StatusReport = 'Declined', ProcessedBy ='" + name + "' WHERE ReportName = '" + reportName + "'", SQLConnection);
            var adapter = new SqlDataAdapter(selectCommand);

            var resultSet = new DataSet();
            adapter.Fill(resultSet);

            SQLConnection.Close();
        }

        public void ApproveReportSupervisor(string name, string reportName)
        {
            SQLConnection.Open();
            var selectCommand = new SqlCommand("UPDATE [dbo].[ExpenseDB] SET StatusReport = 'Approved', ProcessedBy ='" + name + "', DateApproved = @dateApproved WHERE ReportName = '" + reportName + "'", SQLConnection);
            selectCommand.Parameters.AddWithValue("@dateApproved", DateTime.Now.Date);
            var adapter = new SqlDataAdapter(selectCommand);
            

            var resultSet = new DataSet();
            adapter.Fill(resultSet);

            SQLConnection.Close();

        }

        public DataSet LoadExpenseTable(string reportName)
        {
            SQLConnection.Open();
            var selectCommand = new SqlCommand("SELECT ConsultantName as 'Name', Location, Description, Amount, Currency, DateExpense as 'Date' FROM ExpenseDB WHERE ReportName = '" + reportName + "'", SQLConnection);
            var adapter = new SqlDataAdapter(selectCommand);

            var resultSet = new DataSet();
            adapter.Fill(resultSet);
            SQLConnection.Close();
            return resultSet;          
         
        }

        public DataSet LoadExpenseTableNonRejectedOrApproved(string reportName)
        {
            SQLConnection.Open();
            var selectCommand = new SqlCommand("SELECT ConsultantName as 'Name', Location, Description, Amount, Currency, DateExpense as 'Date' FROM ExpenseDB WHERE ReportName = '" + reportName + "' AND (StatusReport = 'Submitted' OR StaffApproved is NULL)", SQLConnection);
            var adapter = new SqlDataAdapter(selectCommand);

            var resultSet = new DataSet();
            adapter.Fill(resultSet);
            SQLConnection.Close();
            return resultSet;

        }


        //public DataSet LoadStaffDataExpenses()
        //{
        //    SQLConnection.Open();
        //    var connectionString = ConfigurationManager.ConnectionStrings["BlueConsultingDBString"].ConnectionString;
        //    var connection = new SqlConnection(connectionString);
        //    var selectCommand = new SqlCommand("SELECT distinct ProcessedBy, SUM(Amount) FROM ExpenseDB WHERE StatusReport ='Approved' AND StaffApproved = 'YES' GROUP BY ProcessedBy", connection);
        //    //ONLY SHOW REPORTNAMES - DONT LET IT REPEAT ITSELF WITH THE OTHER INFO
        //    var adapter = new SqlDataAdapter(selectCommand);

        //    var resultSet = new DataSet();
        //    adapter.Fill(resultSet);
        //    SQLConnection.Close();
        //    return resultSet;
        //}

        public DataSet LoadStaffDataExpenses()
        {
            SQLConnection.Open();
            var connectionString = ConfigurationManager.ConnectionStrings["BlueConsultingDBString"].ConnectionString;
            var connection = new SqlConnection(connectionString);
            var selectCommand = new SqlCommand("SELECT distinct ProcessedBy, SUM(TotalAUD) FROM ExpenseDB WHERE StatusReport ='Approved' AND StaffApproved = 'YES' GROUP BY ProcessedBy", connection);
            //ONLY SHOW REPORTNAMES - DONT LET IT REPEAT ITSELF WITH THE OTHER INFO
            var adapter = new SqlDataAdapter(selectCommand);

            var resultSet = new DataSet();
            adapter.Fill(resultSet);
            SQLConnection.Close();
            return resultSet;

        }

        public double ReturnTotalBudgetRemaining()
        {
            SQLConnection.Open();

            double numb;



            var selectCommand = new SqlCommand("SELECT SUM(Budget) FROM DepartmentDB", SQLConnection);
            //ONLY SHOW REPORTNAMES - DONT LET IT REPEAT ITSELF WITH THE OTHER INFO
            var adapter = new SqlDataAdapter(selectCommand);

            var resultSet = new DataSet();
            adapter.Fill(resultSet);

            try
            {
                numb = Convert.ToDouble(resultSet.Tables[0].Rows[0].ItemArray[0]);
            }
            catch
            {
                numb = 0;
            }

            SQLConnection.Close();
            return numb + ReturnTotalBudgetSpentUnapproved();
        }

        public double ReturnTotalBudgetSpentUnapproved()
        {
            SQLConnection.Open();
            double numb = 0.0;

            var selectCommand = new SqlCommand("SELECT SUM(TotalAUD) FROM ExpenseDB WHERE (StaffApproved is NULL) AND StatusReport = 'Approved'", SQLConnection);
            //ONLY SHOW REPORTNAMES - DONT LET IT REPEAT ITSELF WITH THE OTHER INFO
            var adapter = new SqlDataAdapter(selectCommand);

            var resultSet = new DataSet();
            adapter.Fill(resultSet);

            try
            {
                numb = Convert.ToDouble(resultSet.Tables[0].Rows[0].ItemArray[0]);
            }
            catch
            {
                numb = 0;
            }
            SQLConnection.Close();
            return numb;
        }

        public double ReturnTotalExpensesApproved()
        {
            SQLConnection.Open();
            double numb = 0.0;
            var selectCommand = new SqlCommand("SELECT distinct SUM(TotalAUD) FROM ExpenseDB WHERE StatusReport = 'Approved' AND StaffApproved = 'YES' ", SQLConnection);
            //ONLY SHOW REPORTNAMES - DONT LET IT REPEAT ITSELF WITH THE OTHER INFO
            var adapter = new SqlDataAdapter(selectCommand);

            var resultSet = new DataSet();
            adapter.Fill(resultSet);

            try
            {
                numb = Convert.ToDouble(resultSet.Tables[0].Rows[0].ItemArray[0]);
            }
            catch
            {
                numb = 0;
            }
            SQLConnection.Close();
            return numb;
        }

        public DataSet LoadRejectedReportInfo(string reportName)
        {
            SQLConnection.Open();
            var selectCommand = new SqlCommand("SELECT ConsultantName as 'Name', Location, Description, Amount, Currency, DateExpense as 'Date' FROM ExpenseDB WHERE ReportName = '" + reportName + "'", SQLConnection);
            var adapter = new SqlDataAdapter(selectCommand);

            var resultSet = new DataSet();
            adapter.Fill(resultSet);
            SQLConnection.Close();
            return resultSet;

        }
        public DataSet LoadRejectedReportNames()
        {
            SQLConnection.Open();
            var connectionString = ConfigurationManager.ConnectionStrings["BlueConsultingDBString"].ConnectionString;
            var connection = new SqlConnection(connectionString);
            var selectCommand = new SqlCommand("SELECT distinct ReportName, ConsultantName FROM ExpenseDB WHERE StaffApproved = 'NO'", SQLConnection);
            var adapter = new SqlDataAdapter(selectCommand);
            var resultSet = new DataSet();
            adapter.Fill(resultSet);          
            SQLConnection.Close();
            return resultSet;
        }

        public DataSet LoadStaffUnapprovedReportInfo(string reportName)
        {
            SQLConnection.Open();
            var selectCommand = new SqlCommand("SELECT ConsultantName as 'Name', Location, Description, Amount, Currency, DateExpense as 'Date' FROM ExpenseDB WHERE ReportName = '" + reportName + "'", SQLConnection);
            var adapter = new SqlDataAdapter(selectCommand);

            var resultSet = new DataSet();
            adapter.Fill(resultSet);
            SQLConnection.Close();
            return resultSet;
        }

        public DataSet LoadStaffUnapprovedReportNames()
        {
            SQLConnection.Open();
           var selectCommand = new SqlCommand("SELECT distinct ReportName, ConsultantName, StatusReport, Dept_type FROM ExpenseDB WHERE StatusReport = 'Approved' AND StaffApproved is NULL", SQLConnection);
           var adapter = new SqlDataAdapter(selectCommand);

            var resultSet = new DataSet();
            adapter.Fill(resultSet);
            SQLConnection.Close();
            return resultSet;
        }

        public double ReturnStaffReportTotalAmountForSupervisorName(string name)
        {
            SQLConnection.Open();
            double numb = 0.0;

            var selectCommand = new SqlCommand("SELECT SUM(TotalAUD) FROM ExpenseDB WHERE ReportName = '" + name + "'", SQLConnection);
            //ONLY SHOW REPORTNAMES - DONT LET IT REPEAT ITSELF WITH THE OTHER INFO
            var adapter = new SqlDataAdapter(selectCommand);

            var resultSet = new DataSet();
            adapter.Fill(resultSet);

            SQLConnection.Close();
            try
            {
                numb = Convert.ToDouble(resultSet.Tables[0].Rows[0].ItemArray[0]);
            }
            catch
            {
                numb = 0;
            }
            return numb;
        }

        public double ReturnSingleDepartmentBudgetRemaining(string dept)
        {
            double numb = 0.0;
            SQLConnection.Open();

            var connectionString = ConfigurationManager.ConnectionStrings["BlueConsultingDBString"].ConnectionString;
            var connection = new SqlConnection(connectionString);
            var selectCommand = new SqlCommand("SELECT Budget FROM DepartmentDB WHERE Dept_name = '" + dept + "'", connection);
            //ONLY SHOW REPORTNAMES - DONT LET IT REPEAT ITSELF WITH THE OTHER INFO
            var adapter = new SqlDataAdapter(selectCommand);

            var resultSet = new DataSet();
            adapter.Fill(resultSet);
            SQLConnection.Close();
            try
            {
                numb = Convert.ToDouble(resultSet.Tables[0].Rows[0].ItemArray[0]);
            }
            catch
            {

                numb = 0;
            }

            return numb;
        }

        public DataSet ConsultantLoadSubmittedReports(string name)
        {
            SQLConnection.Open();
            var selectCommand = new SqlCommand("SELECT distinct ReportName, StatusReport as 'Supvervisor Approval', StaffApproved as 'Account Staff Approval' FROM ExpenseDB WHERE ConsultantName = '" + name + "'", SQLConnection);
            var adapter = new SqlDataAdapter(selectCommand);

            var resultSet = new DataSet();
            adapter.Fill(resultSet);
            SQLConnection.Close();
            return resultSet;
        }

        public DataSet ConsultantLoadApprovedReports(string name)
        {
            SQLConnection.Open();
            var selectCommand = new SqlCommand("SELECT distinct ReportName, StatusReport as 'Supvervisor Approval', StaffApproved as 'Account Staff Approval' FROM ExpenseDB WHERE ConsultantName = '" + name + "' AND StatusReport = 'Approved' AND StaffApproved <> 'NO'", SQLConnection);
            var adapter = new SqlDataAdapter(selectCommand);

            var resultSet = new DataSet();
            adapter.Fill(resultSet);
            SQLConnection.Close();
            return resultSet;
        }

        public DataSet ConsultantLoadInProgressReports(string name)
        {
            SQLConnection.Open();
            var selectCommand = new SqlCommand("SELECT distinct ReportName FROM ExpenseDB WHERE ConsultantName = '" + name + "' AND StatusReport = 'Submitted'", SQLConnection);
            var adapter = new SqlDataAdapter(selectCommand);

            var resultSet = new DataSet();
            adapter.Fill(resultSet);
            SQLConnection.Close();
            return resultSet;
        }

        public byte[] RetrievePDFPage(string reportName, string consultantName, string location, string description, double amount, string currency)
        {
            byte[] pdfFile;
            SQLConnection.Open();

            var selectCommand = new SqlCommand("SELECT [PDF_File] FROM ExpenseDB WHERE reportName = '"+reportName+"' AND ConsultantName = '"+consultantName+"' AND Location = '"+location+"' AND Description = '"+description+"' AND Amount = '"+amount+"' AND Currency = '"+currency+"' AND PDF_File is not NULL", SQLConnection);
            var adapter = new SqlDataAdapter(selectCommand);

            var resultSet = new DataSet();
            adapter.Fill(resultSet);
            SQLConnection.Close();

            try
            {
                pdfFile = (byte[])(resultSet.Tables[0].Rows[0].ItemArray[0]);
            }
            catch
            {

                pdfFile = null;
            }

            return pdfFile;
        }
    }
}
