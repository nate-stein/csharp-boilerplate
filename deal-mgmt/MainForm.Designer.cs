namespace Trinity.DealManagementUtils
{
	partial class MainForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose (bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose ();
			}
			base.Dispose (disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent ()
		{
			this.comboBoxActionOptions = new System.Windows.Forms.ComboBox ();
			this.label1 = new System.Windows.Forms.Label ();
			this.buttonContinue = new System.Windows.Forms.Button ();
			this.openFileDialogProspectusTextFile = new System.Windows.Forms.OpenFileDialog ();
			this.folderBrowserDialogFolderWithTextFiles = new System.Windows.Forms.FolderBrowserDialog ();
			this.folderBrowserDialogToSaveProspectuses = new System.Windows.Forms.FolderBrowserDialog ();
			this.SuspendLayout ();
			// 
			// comboBoxActionOptions
			// 
			this.comboBoxActionOptions.FormattingEnabled = true;
			this.comboBoxActionOptions.Location = new System.Drawing.Point (27, 45);
			this.comboBoxActionOptions.Name = "comboBoxActionOptions";
			this.comboBoxActionOptions.Size = new System.Drawing.Size (324, 24);
			this.comboBoxActionOptions.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point (27, 22);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size (177, 17);
			this.label1.TabIndex = 1;
			this.label1.Text = "What would you like to do?";
			// 
			// buttonContinue
			// 
			this.buttonContinue.Location = new System.Drawing.Point (27, 104);
			this.buttonContinue.Name = "buttonContinue";
			this.buttonContinue.Size = new System.Drawing.Size (96, 32);
			this.buttonContinue.TabIndex = 2;
			this.buttonContinue.Text = "Continue";
			this.buttonContinue.UseVisualStyleBackColor = true;
			this.buttonContinue.Click += new System.EventHandler (this.buttonContinue_Click);
			// 
			// openFileDialogProspectusTextFile
			// 
			this.openFileDialogProspectusTextFile.Title = "Select text file to add.";
			// 
			// folderBrowserDialogFolderWithTextFiles
			// 
			this.folderBrowserDialogFolderWithTextFiles.Description = "Select Folder to Rip Text Files From";
			// 
			// folderBrowserDialogToSaveProspectuses
			// 
			this.folderBrowserDialogToSaveProspectuses.Description = "Select Folder to Save Prospectus Docs";
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF (8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size (375, 170);
			this.Controls.Add (this.buttonContinue);
			this.Controls.Add (this.label1);
			this.Controls.Add (this.comboBoxActionOptions);
			this.Name = "MainForm";
			this.Text = "Trinity";
			this.ResumeLayout (false);
			this.PerformLayout ();

		}

		#endregion

		private System.Windows.Forms.ComboBox comboBoxActionOptions;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button buttonContinue;
		private System.Windows.Forms.OpenFileDialog openFileDialogProspectusTextFile;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialogFolderWithTextFiles;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialogToSaveProspectuses;

	}
}

