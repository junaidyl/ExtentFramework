using System;
using System.Threading.Tasks;
using Extent.ExtentHtmlReporter.Properties;

using AventStack.ExtentReports;

namespace Extent.ExtentHtmlReporter
{
    /// <summary>
    /// The TestcaseReport class is used once to setup the report
    /// </summary>
    public class TestcaseReport
    {
        #region Properties

        public static ExtentReports ExtentReports { get; set; }

        private AventStack.ExtentReports.Reporter.ExtentHtmlReporter HtmlReporter { get; }
        private string ReportPath { get; }

        #endregion

        #region Constructors

        // Creates a new testcase using the provided input
        public TestcaseReport(string path)
        {
            //ReportPath = path + @"\" + System.DateTime.Now.ToString(@"yyyy-MM-dd HH.mm.ss") + @"\";
            ReportPath = path + @"\";

            System.IO.Directory.CreateDirectory(ReportPath);
            HtmlReporter = new AventStack.ExtentReports.Reporter.ExtentHtmlReporter(ReportPath);

            ExtentReports = new ExtentReports();
            ExtentReports.AttachReporter(HtmlReporter);
        }

        #endregion
    }

    /// <summary>
    /// The Testcase class marks the scope of a testcase.
    /// </summary>
    public class Testcase
    {
        #region Properties

        private string Name { get; }
        private string Description { get; }
        private ExtentTest ExtentTest { get; set; }

        #endregion

        #region Constructors

        // Creates a new testcase using the provided input
        public Testcase(string name, string description)
        {
            Name = name;
            Description = description;

            SetupReport();
        }

        // Setup Report
        private void SetupReport()
        {
            try
            {
                ExtentTest = TestcaseReport.ExtentReports.CreateTest(Name, Description);
            }
            catch (Exception e)
            {
                throw new CustomException(Resources.Testcase_InitializeAsync_UnknownException, e);
            }
        }

        #endregion


        #region Action Calls

        public Task<string> TestcasePass(string details, string attachment = "")
        {
            if (String.IsNullOrEmpty(attachment))
            {
                ExtentTest.Pass(details);
            }
            else
            {
                ExtentTest.Pass(details).AddScreenCaptureFromPath(attachment);
            }
            TestcaseReport.ExtentReports.Flush();
            return Task.FromResult(details);
        }

        public Task<string> TestcaseFail(string details, string attachment = "")
        {
            if (String.IsNullOrEmpty(attachment))
            {
                ExtentTest.Fail(details);
            }
            else
            {
                ExtentTest.Fail(details).AddScreenCaptureFromPath(attachment);
            }
            TestcaseReport.ExtentReports.Flush();
            return Task.FromResult(details);
        }
        #endregion
    }
}
