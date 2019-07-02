using System.Activities.Presentation.Metadata;
using System.ComponentModel;
using System.ComponentModel.Design;
using Extent.ExtentHtmlReporter.Activities.Design.Properties;

namespace Extent.ExtentHtmlReporter.Activities.Design
{
    public class DesignerMetadata : IRegisterMetadata
    {
        public void Register()
        {
            var builder = new AttributeTableBuilder();
            builder.ValidateTable();

            var categoryAttribute =  new CategoryAttribute($"{Resources.Category}");

            builder.AddCustomAttributes(typeof(TestcaseReportScope), categoryAttribute);
            builder.AddCustomAttributes(typeof(TestcaseReportScope), new DesignerAttribute(typeof(TestcaseReportScopeDesigner)));
            builder.AddCustomAttributes(typeof(TestcaseReportScope), new HelpKeywordAttribute("https://go.uipath.com"));

            builder.AddCustomAttributes(typeof(TestcaseScope), categoryAttribute);
            builder.AddCustomAttributes(typeof(TestcaseScope), new DesignerAttribute(typeof(TestcaseScopeDesigner)));
            builder.AddCustomAttributes(typeof(TestcaseScope), new HelpKeywordAttribute("https://go.uipath.com"));

            builder.AddCustomAttributes(typeof(LogTestStatus), categoryAttribute);
            builder.AddCustomAttributes(typeof(LogTestStatus), new DesignerAttribute(typeof(LogTestStatusDesigner)));
            builder.AddCustomAttributes(typeof(LogTestStatus), new HelpKeywordAttribute("https://go.uipath.com"));

            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}
