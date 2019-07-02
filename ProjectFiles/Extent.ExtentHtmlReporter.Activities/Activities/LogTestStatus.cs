using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using Extent.ExtentHtmlReporter.Activities.Properties;

namespace Extent.ExtentHtmlReporter.Activities
{
    [LocalizedDisplayName(nameof(Resources.LogTestStatusDisplayName))]
    [LocalizedDescription(nameof(Resources.LogTestStatusDescription))]
    public class LogTestStatus : AsyncTaskCodeActivity<string>
    {
        #region Properties

        [LocalizedDisplayName(nameof(Resources.LogTestStatusPassFailDisplayName))]
        [LocalizedDescription(nameof(Resources.LogTestStatusPassFailDescription))]
        [LocalizedCategory(nameof(Resources.Input))]
        public TestcaseStatus Status { get; set; }

        [LocalizedDisplayName(nameof(Resources.LogTestStatusDetailsDisplayName))]
        [LocalizedDescription(nameof(Resources.LogTestStatusDetailsDescription))]
        [LocalizedCategory(nameof(Resources.Input))]
        public InArgument<string> Details { get; set; }

        [LocalizedDisplayName(nameof(Resources.LogTestStatusAttachmentDisplayName))]
        [LocalizedDescription(nameof(Resources.LogTestStatusAttachmentDescription))]
        [LocalizedCategory(nameof(Resources.Input))]
        public InArgument<string> Attachment { get; set; }

        #endregion

        #region Constructors

        public LogTestStatus()
        {
            this.Constraints.Add(ActivityConstraints.HasParentType<Activity, TestcaseScope>(Resources.ParentScope_ValidationMessage + " " + Resources.TestcaseScope));
        }

        #endregion

        public enum TestcaseStatus
        {
            Pass,
            Fail
        }

        /// <inheritdoc />
        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            base.CacheMetadata(metadata);

            if (Details == null)
            {
                metadata.AddValidationError(string.Format(Resources.MetadataValidationError, nameof(Details)));
            }
        }

        protected override Task<string> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken cancellationToken, Testcase client)
        {
            var status = Status;
            var details = Details.Get(context).Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");
            var attachment = Attachment.Get(context);
            if (status == TestcaseStatus.Pass)
            {
                return client.TestcasePass(details, attachment);
            }
            else
            {
                return client.TestcaseFail(details, attachment);
            }
        }
    }
}
