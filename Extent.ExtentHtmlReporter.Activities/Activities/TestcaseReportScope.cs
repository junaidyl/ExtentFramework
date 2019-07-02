using System;
using System.Activities;
using System.ComponentModel;
using System.Activities.Statements;
using Extent.ExtentHtmlReporter.Activities.Properties;

namespace Extent.ExtentHtmlReporter.Activities
{

    [LocalizedDescription(nameof(Resources.TestcaseReportScopeDescription))]
    [LocalizedDisplayName(nameof(Resources.TestcaseReportScope))]
    public class TestcaseReportScope : NativeActivity
    {
        #region Properties

        [Browsable(false)]
        public ActivityAction<TestcaseReport> Body { get; set; }

        [LocalizedCategory(nameof(Resources.Input))]
        [LocalizedDisplayName(nameof(Resources.TestcaseReportScopePathDisplayName))]
        [LocalizedDescription(nameof(Resources.TestcaseReportScopePathDescription))]
        public InArgument<string> ReportPath { get; set; }

        internal static string TestcaseReportContainerPropertyTag => "TestcaseReportScope";

        #endregion

        #region Constructors

        public TestcaseReportScope()
        {

            Body = new ActivityAction<TestcaseReport>
            {
                Argument = new DelegateInArgument<TestcaseReport>(TestcaseReportContainerPropertyTag),
                Handler = new Sequence { DisplayName = "Do" }
            };
        }

        #endregion


        #region Private Methods

        protected override void CacheMetadata(NativeActivityMetadata metadata)
        {
            base.CacheMetadata(metadata);

            if (ReportPath == null)
            {
                metadata.AddValidationError(string.Format(Resources.MetadataValidationError, nameof(ReportPath)));
            }
        }

        protected override void Execute(NativeActivityContext context)
        {
            var path = ReportPath.Get(context);
            var testreport = new TestcaseReport(path);

            if (Body != null)
            {
                context.ScheduleAction<TestcaseReport>(Body, testreport, OnCompleted, OnFaulted);
            }
        }

        private void OnFaulted(NativeActivityFaultContext faultContext, Exception propagatedException, ActivityInstance propagatedFrom)
        {
            //TODO
        }

        private void OnCompleted(NativeActivityContext context, ActivityInstance completedInstance)
        {
            //TODO
        }

        #endregion


        #region Helpers

        #endregion
    }
}
