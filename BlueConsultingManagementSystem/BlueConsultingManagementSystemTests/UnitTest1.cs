﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlueConsultingManagementSystemLogic;
using System.Data;
using System.Transactions;
using System.Data.SqlClient;




namespace BlueConsultingManagementSystemTests
{
    [TestClass]
    public class UnitTest1
    {
        //[TestCategory("staff")]
        //[TestMethod]
        //public void AllApprovedReportsTest()
        //{
        //    DatabaseHandler dh = new DatabaseHandler();
        //    DataSet ds = dh.ReturnAllApprovedReports();
        //    for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
        //    {
        //        String testify = ds.Tables[0].Rows[j]["StatusReport"].ToString();
        //        Assert.AreEqual("Approved", testify);
        //    }
        //}
        ////[TestCategory("staff")]
        ////[TestMethod]
        ////public void DenyReportForSupervisor()
        ////{
        ////    DatabaseHandler dh = new DatabaseHandler();
        ////    using (TransactionScope TestTransaction = new TransactionScope())
        ////    {
        ////        dh.DenyReportForSupervisor("supervisor", "JamesLoves");
        ////        DataSet ds = dh.ReturnRejectedReportNamesForSupervisor();
        ////        Assert.AreEqual("JamesLoves", ds.Tables[0].Rows[""].Contains);
        ////fell asleep at keyboard commiting and going to sleep.
        ////        TestTransaction.Dispose();
        ////    }
        ////}
        ////[TestCategory("staff")]
        ////[TestMethod]
        //[TestCategory("staff")]
        //[TestMethod]
        //public void ReturnDepExpensesMadeTest()
        //{
        //    DatabaseHandler dh = new DatabaseHandler();
        //    double result = dh.ReturnDepartmentExpensesMade("LogisticServices");
        //    Assert.AreEqual(0, result);
        //}
        //[TestCategory("staff")]
        //[TestMethod]
        //public void LoadRejectedInfoTest()
        //{
        //    DatabaseHandler dh = new DatabaseHandler();
        //    DataSet ds = dh.LoadStaffUnapprovedReportInfo("The Big Meeting");
        //    Assert.AreEqual("dsfdsfds", ds.Tables[0].Rows[0]["Location"].ToString());
        //}
        //[TestCategory("staff")]
        //[TestMethod]
        //public void ReturnRejectedReportNamesForSupervisor()
        //{
        //    DatabaseHandler dh = new DatabaseHandler();
        //    DataSet ds = dh.ReturnUnapprovedReportNamesForStaff();
        //    string result = ds.Tables[0].Rows[0]["ReportName"].ToString();
        //    Assert.AreEqual("The Big Meeting", result);
        //}
        //[TestCategory("staff")]
        //[TestMethod]
        ////this test I am unsure about.
        //public void UpdateDeptBudgetTest()
        //{
        //    DatabaseHandler dh = new DatabaseHandler();
        //     using (TransactionScope TestTransaction = new TransactionScope())
        //    {
        //        dh.UpdateDepartmentBudget("LogisticServices", 1.2);
        //        double result = dh.ReturnDepartmentExpensesMade("HigherEducation");
        //        Assert.AreEqual(424537.31, result);
               
        //        TestTransaction.Dispose();
        //    }
        //}
        //[TestCategory("staff")]
        //[TestMethod]
        //public void UnapprovedReportNames()
        //{
        //    DatabaseHandler dh = new DatabaseHandler();
        //    DataSet ds = dh.ReturnUnapprovedReportNamesForStaff();
        //    for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
        //    {
        //        String teststatus = ds.Tables[0].Rows[j]["StatusReport"].ToString();
        //        Assert.AreEqual("Approved", teststatus);
   
        //    }
        //}
        //[TestCategory("staff")]
        //[TestMethod]
        //public void LoadStaffUnapprovedInfoTest()
        //{
        //    DatabaseHandler dh = new DatabaseHandler();
        //    DataSet ds = dh.LoadStaffUnapprovedReportInfo("bacon");
        //    string result = ds.Tables[0].Rows[0]["name"].ToString();
        //    Assert.AreEqual("james",result);


        //}
        //[TestCategory("staff")]
        //[TestMethod]
        //public void CurrentDeptBudgetTest()
        //{
        //    DatabaseHandler dh = new DatabaseHandler();
        //    double result = dh.ReturnCurrentDepartmentMoney("HigherEducation");
        //    //Assert.AreEqual(10000.000, result);
        //    result = dh.ReturnCurrentDepartmentMoney("LogisticServices");
        //    Assert.AreEqual(10000.000, result);
        //}
        //[TestCategory("staff")]
        //[TestMethod]
        //public void DenyReportTest()
        //{
        //    DatabaseHandler dh = new DatabaseHandler();
        //    using (TransactionScope TestTransaction = new TransactionScope())
        //    {

        //        dh.InsertConsultantExpenseQuery("testReport", "test", "test", "test", 12.04, "AUD", "TestDept", DateTime.Today);
        //       // dh.DenyReportForStaffMember("testReport");
        //        var selectCommand = new SqlCommand("select ReportName,StaffApproved from [dbo].[ExpenseDB] where ReportName='testReport';", dh.SQLConnection);
        //        var adapter = new SqlDataAdapter(selectCommand);
        //        var resultSet = new DataSet();
        //        adapter.Fill(resultSet);
        //        string testify = resultSet.Tables[0].Rows[0]["StaffApproved"].ToString();
        //        Assert.AreEqual("NO", testify);
        //        TestTransaction.Dispose();
        //    }
        //}
        // [TestCategory("staff")]
        //[TestMethod]
        //public void ApproveReportTest()
        //{
        //    DatabaseHandler dh = new DatabaseHandler();
        //    using (TransactionScope TestTransaction = new TransactionScope())
        //    {

        //        dh.InsertConsultantExpenseQuery("testReport", "test", "test", "test", 12.04, "AUD", "TestDept", DateTime.Today);
        //        dh.ApproveReportForStaffMember("testReport");
        //        var selectCommand = new SqlCommand("select ReportName,StaffApproved from [dbo].[ExpenseDB] where ReportName='testReport';", dh.SQLConnection);
        //        var adapter = new SqlDataAdapter(selectCommand);
        //        var resultSet = new DataSet();
        //        adapter.Fill(resultSet);
        //        string testify = resultSet.Tables[0].Rows[0]["StaffApproved"].ToString();
        //        Assert.AreEqual("YES", testify);
        //        TestTransaction.Dispose();
        //    }
        //}
        //INSERTION TEST METHOD.
        [TestCategory("Consultant")]
        [TestMethod]
        public void ConsultantTest()
        {
            DatabaseHandler dh = new DatabaseHandler();
            using (TransactionScope TestTransaction = new TransactionScope())
            {

                dh.InsertConsultantExpenseQuery("CONSULTANTSUPERTEST", "Hugh", "Home", "Doing testing peasants", 1500.50, "AUD", "LogisticServices", DateTime.Today);
                DataSet dsInprogress = dh.ReturnConsultantInProgressReports("Hugh");
                Assert.AreEqual(1, dsInprogress.Tables[0].Rows.Count);
                dh.InsertConsultantExpenseQuery("CONSULTANTSUPERTEST", "Hugh", "work", "ICac Champagne, 1959 was a good year", 3000, "AUD", "LogisticServices", DateTime.Today);
                DataSet dsSubmitted = dh.ReturnConsultantSubmittedReports("hugh");
                Assert.AreEqual(1, dsSubmitted.Tables[0].Rows.Count);
                DataSet dsApproved = dh.ReturnConsultantApprovedReports("hugh");
                Assert.AreEqual(0, dsApproved.Tables[0].Rows.Count);
                TestTransaction.Dispose();
            }

        }
        [TestCategory("Supervisor")]
        [TestMethod]
        public void SupervisorTest()
        {

            DatabaseHandler dh = new DatabaseHandler();
            using (TransactionScope TestTransaction = new TransactionScope())
            {

                dh.InsertConsultantExpenseQuery("SUPERVISORSUPERTEST", "Hugh", "Home", "Doing testing peasants", 1500.50, "AUD", "LogisticServices", DateTime.Today);
                dh.InsertConsultantExpenseQuery("SUPERVISORSUPERTEST", "Hugh", "Home", "Doing testing peasants", 1500.50, "AUD", "LogisticServices", DateTime.Today);
               DataSet dsDepartmentSupervisor = dh.LoadSubmittedReportsForDepartmentSupervisor("LogisticServices");
               int i = 0;
               bool resultDepsup= false;
               while (i < dsDepartmentSupervisor.Tables[0].Rows.Count)
               {
                   if (dsDepartmentSupervisor.Tables[0].Rows[i]["ReportName"].ToString() == "SUPERVISORSUPERTEST" && dsDepartmentSupervisor.Tables[0].Rows[i]["ConsultantName"].ToString() == "Hugh")
                   {
                       resultDepsup = true;
                       break;
                   }
                   i++;
               }
               Assert.AreEqual(true, resultDepsup);
               DataSet dsExpenseSupervisor = dh.ReturnExpenseTable("SUPERVISORSUPERTEST");
               Assert.AreEqual("Hugh", dsExpenseSupervisor.Tables[0].Rows[0]["Name"].ToString());
               dh.ApproveReportForSupervisor("SuperSupervisor", "SUPERVISORSUPERTEST");
               dh.UpdateDepartmentBudget("LogisticServices", 3001.00);
               DataSet dsStatus = dh.ReturnConsultantSubmittedReports("Hugh");
               Assert.AreEqual("Approved", dsStatus.Tables[0].Rows[0]["Supvervisor Approval"].ToString());
                Assert.AreEqual(6999,dh.ReturnCurrentDepartmentMoney("LogisticServices"));
                TestTransaction.Dispose();
            }

            //deny route
            using (TransactionScope TestTransaction = new TransactionScope())
            {

                dh.InsertConsultantExpenseQuery("SUPERVISORSUPERTEST", "Hugh", "Home", "Doing testing peasants", 1500.50, "AUD", "LogisticServices", DateTime.Today);
                dh.InsertConsultantExpenseQuery("SUPERVISORSUPERTEST", "Hugh", "Home", "Doing testing peasants", 1500.50, "AUD", "LogisticServices", DateTime.Today);
                DataSet dsDepartmentSupervisor = dh.LoadSubmittedReportsForDepartmentSupervisor("LogisticServices");
                int i = 0;
                bool resultDepsup = false;
                while (i < dsDepartmentSupervisor.Tables[0].Rows.Count)
                {
                    if (dsDepartmentSupervisor.Tables[0].Rows[i]["ReportName"].ToString() == "SUPERVISORSUPERTEST" && dsDepartmentSupervisor.Tables[0].Rows[i]["ConsultantName"].ToString() == "Hugh")
                    {
                        resultDepsup = true;
                        break;
                    }
                    i++;
                }
                Assert.AreEqual(true, resultDepsup);
                DataSet dsExpenseSupervisor = dh.ReturnExpenseTable("SUPERVISORSUPERTEST");
                Assert.AreEqual("Hugh", dsExpenseSupervisor.Tables[0].Rows[0]["Name"].ToString());
                dh.DenyReportForSupervisor("SuperSupervisor", "SUPERVISORSUPERTEST");
                DataSet dsStatus = dh.ReturnConsultantSubmittedReports("Hugh");
                Assert.AreEqual("Declined", dsStatus.Tables[0].Rows[0]["Supvervisor Approval"].ToString());
                TestTransaction.Dispose();
            }
        }

        //currency tests below here
         [TestCategory("Currency")]
                [TestMethod]
        public void TestEUR()
        {
            double expected;
            CurrencyConverter cc = new CurrencyConverter();
            expected = cc.ConvertCurrencyToAUD("EUR", 1);
            Assert.AreEqual(0.680265,expected);
        }
         [TestCategory("Currency")]
        [TestMethod]
        public void TestCNY()
        {
            double expected;
            CurrencyConverter cc = new CurrencyConverter();
            expected = cc.ConvertCurrencyToAUD("CNY", 1);
            Assert.AreEqual(0.172175, expected);

        }
         [TestCategory("Currency")]
        [TestMethod]
        public void TestCurrencyConvertFails()
        {
            double expected;
            CurrencyConverter cc = new CurrencyConverter();
            expected = cc.ConvertCurrencyToAUD("fsd", 1);
            Assert.AreEqual(-1.0, expected);

        }

    }
}
