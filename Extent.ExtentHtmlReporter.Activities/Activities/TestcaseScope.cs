using System;
using System.Activities;
using System.ComponentModel;
using System.Activities.Statements;
using Extent.ExtentHtmlReporter.Activities.Properties;

namespace Extent.ExtentHtmlReporter.Activities
{

    [LocalizedDescription(nameof(Resources.TestcaseScopeDescription))]
    [LocalizedDisplayName(nameof(Resources.TestcaseScope))]
    public class TestcaseScope : NativeActivity
    {
        #region Properties 

        [Browsable(false)]
        public ActivityAction<Testcase> Body { get; set; }

        [LocalizedCategory(nameof(Resources.Input))]
        [LocalizedDisplayName(nameof(Resources.TestcaseScopeNameDisplayName))]
        [LocalizedDescription(nameof(Resources.TestcaseScopeNameDescription))]
        public InArgument<string> TestcaseName { get; set; }

        [LocalizedCategory(nameof(Resources.Input))]
        [LocalizedDisplayName(nameof(Resources.TestcaseScopeDescDisplayName))]
        [LocalizedDescription(nameof(Resources.TestcaseScopeDescDescription))]
        public InArgument<string> TestcaseDescription { get; set; }

        internal static string TestcaseContainerPropertyTag => "TestcaseScope";

        #endregion

        #region Constructors

        public TestcaseScope()
        {
            this.Constraints.Add(ActivityConstraints.HasParentType<Activity, TestcaseReportScope>(Resources.ParentScope_ValidationMessage + " " + Resources.TestcaseReportScope));
            Body = new ActivityAction<Testcase>
            {
                Argument = new DelegateInArgument<Testcase>(TestcaseContainerPropertyTag),
                Handler = new Sequence { DisplayName = "Do" }
            };
        }

        #endregion


        #region Private Methods

        protected override void CacheMetadata(NativeActivityMetadata metadata)
        {
            base.CacheMetadata(metadata);

            if (TestcaseName == null)
            {
                metadata.AddValidationError(string.Format(Resources.MetadataValidationError, nameof(TestcaseName)));
            }
            if (TestcaseDescription == null)
            {
                metadata.AddValidationError(string.Format(Resources.MetadataValidationError, nameof(TestcaseDescription)));
            }
        }

        protected override void Execute(NativeActivityContext context)
        {
            var name = TestcaseName.Get(context);
            var desc = TestcaseDescription.Get(context);
            var testcase = new Testcase(name, desc);

            if (Body != null)
            {
                context.ScheduleAction<Testcase>(Body, testcase, OnCompleted, OnFaulted);
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
