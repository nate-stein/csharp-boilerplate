using System;
using System.Windows.Forms;
using Trinity.SPDetailRipping;
using System.Collections.Generic;
using System.Linq;

namespace Trinity.DealManagementUtils
{
	public partial class RegexSelectionForm : Form
	{
		private Dictionary<EStructureDetailRegexType, string> _regexTypesAndDescriptions;

		public RegexSelectionForm ()
		{
			InitializeComponent ();
			updateComboBoxOptions ();
		}

		#region buttonSave_Click
		private void buttonSave_Click (object sender, EventArgs e)
		{
			Visible = false;
			try
			{
				string desiredRegexType = comboBoxRegexTypes.SelectedItem.ToString ();
				EStructureDetailRegexType type =
					_regexTypesAndDescriptions.FirstOrDefault (x => x.Value == desiredRegexType).Key;
				string fileName = textBoxRegexFileNameSave.Text;
				SPFileManagementUtils.SaveRegularExpressionToExperimentalRegexFolder (type, fileName);
				MessageBox.Show ("Finished saving the regular expression to file.", "Done");
			}
			catch (Exception exc)
			{
				MessageBox.Show ("Details: " + exc, "Error encountered");
			}
			finally
			{
				Application.Exit ();
			}
		}
		#endregion

		#region updateComboBoxOptions
		private void updateComboBoxOptions ()
		{
			_regexTypesAndDescriptions = RegexBuilderUtils.GetRegexTypesAndDescriptions ();
			string [] descriptions = _regexTypesAndDescriptions.Values.ToArray ();
			comboBoxRegexTypes.Items.AddRange (descriptions);
		}
		#endregion
	}
}
