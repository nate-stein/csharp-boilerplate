using System;
using System.Windows.Forms;
using Trinity.SPDetailRipping;
using Trinity.DealRipping;

namespace Trinity.DealManagementUtils
{
	public partial class MainForm : Form
	{
		private const string _URL_LIST_XML_FILE_PATH = @"";
        
		public MainForm ()
		{
			InitializeComponent ();
			updateComboBoxOptions ();
		}

		#region updateComboBoxOptions
		private void updateComboBoxOptions ()
		{
			comboBoxActionOptions.Items.Add ("Add all deals from folder to ZZ database");
			comboBoxActionOptions.Items.Add ("Add deal to ZZ database from text file");
			comboBoxActionOptions.Items.Add ("Delete Cusip from SP Test database");
			comboBoxActionOptions.Items.Add ("Delete Cusip from ZZ database");
			comboBoxActionOptions.Items.Add ("Empty ZZ database");
			comboBoxActionOptions.Items.Add ("Save single prospectus from URL");
			comboBoxActionOptions.Items.Add ("Save prospectuses ripped from URLs in xml file");
			comboBoxActionOptions.Items.Add ("Save regular expression to Experimental Regex folder");
		}
		#endregion

		#region buttonContinue_Click
		private void buttonContinue_Click (object sender, EventArgs e)
		{
			switch (comboBoxActionOptions.SelectedItem.ToString ())
			{
				case "Add all deals from folder to ZZ database":
					addAllDealsToZZDatabaseFromFolder ();
					Application.Exit ();
					break;
				case "Add deal to ZZ database from text file":
					addDealToZZDatabaseFromTextFile ();
					Application.Exit ();
					break;
				case "Delete Cusip from SP Test database":
					Visible = false;
					CusipDeleteForm cusipForm = new CusipDeleteForm
					{
						TargetDatabaseConnectionString =
						SPFileManagementUtils.ConnectionStringSPTestDatabase
					};
					cusipForm.Show ();
					break;
				case "Delete Cusip from ZZ database":
					Visible = false;
					CusipDeleteForm zzCusipForm = new CusipDeleteForm
					{
						TargetDatabaseConnectionString =
						SPFileManagementUtils.ConnectionStringZZDatabase
					};
					zzCusipForm.Show ();
					break;
				case "Empty ZZ database":
					emptyZZDatabase ();
					Application.Exit ();
					break;
				case "Save single prospectus from URL":
					Visible = false;
					URLEntryForm urlForm = new URLEntryForm ();
					urlForm.Show ();
					break;
				case "Save prospectuses ripped from URLs in xml file":
					Visible = false;
					saveProspectusesFromUrlsInXmlFile ();
					Application.Exit ();
					break;
				case "Save regular expression to Experimental Regex folder":
					Visible = false;
					RegexSelectionForm form = new RegexSelectionForm ();
					form.Show ();
					break;
				default:
					string errMsg = "Unhandled option selected in dropdown.";
					throw new NotImplementedException (errMsg);
			}
		}
		#endregion

		#region emptyZZDatabase
		private static void emptyZZDatabase ()
		{
			SPDatabaseUpdateTools.ClearAllDealDataFromZZDatabase ();
			MessageBox.Show ("Finished clearing all deal data from ZZ database.", "Done");
		}
		#endregion

		#region saveProspectusesFromUrlsInXmlFile
		private void saveProspectusesFromUrlsInXmlFile ()
		{
			folderBrowserDialogToSaveProspectuses.ShowDialog ();
			string folderSavePath = folderBrowserDialogToSaveProspectuses.SelectedPath + @"\";
			string [] urls =
				SPFileManagementUtils.GetListOfUrlsFromXmlFile (_URL_LIST_XML_FILE_PATH).ToArray ();
			ProspectusRipping.BulkProspectusRipper ripper =
				new ProspectusRipping.BulkProspectusRipper (folderSavePath, urls);
			ripper.SaveProspectusesFromUrls (true, false, true, false);
			ripper.SaveExecutionSummary ();
			ripper.SaveErrorLog ();
			MessageBox.Show ("Finished ripping prospectus documents.", "Done");
		}
		#endregion

		#region addDealToZZDatabaseFromTextFile
		private void addDealToZZDatabaseFromTextFile ()
		{
			openFileDialogProspectusTextFile.ShowDialog ();
			string filePath = openFileDialogProspectusTextFile.FileName;
			string zzDbConnString = SPFileManagementUtils.ConnectionStringZZDatabase;
			SPDatabaseUpdateTools.AddDealFromTextFileToDatabase (filePath, zzDbConnString);
			MessageBox.Show ("Finished adding deal to database.", "Done");
		}
		#endregion

		#region addAllDealsToZZDatabaseFromFolder
		private void addAllDealsToZZDatabaseFromFolder ()
		{
			folderBrowserDialogFolderWithTextFiles.ShowDialog ();
			string folderPath = folderBrowserDialogFolderWithTextFiles.SelectedPath;
			string zzDbConnString = SPFileManagementUtils.ConnectionStringZZDatabase;
			string errorLogPath = folderPath + @"\" + "Error Log.txt";
			SPDatabaseUpdateTools.AddAllDealsFromFolderToSPDatabase (
				folderPath, zzDbConnString, errorLogPath);
			MessageBox.Show ("Finished saving all deals from folder to ZZ Database. Error log " +
								  "was saved in same folder.", "Done");
		}
		#endregion
	}
}
