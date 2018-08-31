using System;
using System.Windows.Forms;
using Trinity.DealRipping;

namespace Trinity.DealManagementUtils
{
	public partial class CusipDeleteForm : Form
	{
		public CusipDeleteForm ()
		{
			InitializeComponent ();
		}

		private void buttonDelete_Click (object sender, EventArgs e)
		{
			string cusip = tbCusip.Text.Trim ();
			System.Collections.Generic.List<string> tablesInDatabase =
				 SPDatabaseUtils.GetSPDatabaseListOfDealTables ();
			SPDatabaseUpdateTools.DeleteCusipRecordFromSPDatabase (
				 cusip, tablesInDatabase, TargetDatabaseConnectionString);
			MessageBox.Show ("Finished deleting Cusip from records.", "Done");
			Application.Exit ();
		}

		public string TargetDatabaseConnectionString { get; set; }
	}
}
