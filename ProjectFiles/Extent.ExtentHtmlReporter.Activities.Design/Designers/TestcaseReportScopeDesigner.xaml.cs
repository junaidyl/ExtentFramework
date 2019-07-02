namespace Extent.ExtentHtmlReporter.Activities.Design
{
    using System.Windows.Forms;

    /// <summary>
    /// Interaction logic for TestcaseReportScopeDesigner.xaml
    /// </summary>
    public partial class TestcaseReportScopeDesigner
    {
        public TestcaseReportScopeDesigner()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the Click event of the FileDialogButton control to launch an OpenFileDialog instance.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        /*
        private void FileDialogButton_Click(object sender, System.EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    this.ModelItem.Properties["ReportPath"].SetValue(fbd.SelectedPath);
                }
            }
        }
        */
    }
}
