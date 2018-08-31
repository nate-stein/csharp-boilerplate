using System;
using System.Windows.Forms;

namespace Trinity.DealManagementUtils
{
	public partial class URLEntryForm : Form
	{
		private const string _PROSPECTUS_FILES_FOLDER = @"C:\Users\Nathaniel\Box Sync\TRINITY\RIPPER LABORATORY\PROSPECTUS TEXT FILES\";

		public URLEntryForm ()
		{
			InitializeComponent ();
		}

		#region buttonRipProspectusFromURL_Click
		private void buttonRipProspectusFromURL_Click (object sender, EventArgs e)
		{
			bool saveCleanText = checkBoxSaveCleanText.Checked;
			bool saveXmlDoc = checkBoxCreateXmlDoc.Checked;
			bool saveHtml = checkBoxSaveHtml.Checked;
			bool saveExecutionLogs = checkBoxSaveExecutionLogs.Checked;
			string [] urls = { textBoxURL.Text };

			ProspectusRipping.BulkProspectusRipper ripper =
				 new ProspectusRipping.BulkProspectusRipper (_PROSPECTUS_FILES_FOLDER, urls);

			ripper.SaveProspectusesFromUrls (saveCleanText, saveXmlDoc, true, saveHtml);

			if (saveExecutionLogs)
			{
				ripper.SaveExecutionSummary ();
				ripper.SaveErrorLog ();
			}
			MessageBox.Show ("Finished ripping URL to Trinity folder.", "Done");
			Application.Exit ();
		}
		#endregion
	}
}
