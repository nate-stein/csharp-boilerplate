namespace Trinity.DealManagementUtils
{
    partial class URLEntryForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxURL = new System.Windows.Forms.TextBox();
            this.buttonRipProspectusFromURL = new System.Windows.Forms.Button();
            this.checkBoxSaveCleanText = new System.Windows.Forms.CheckBox();
            this.checkBoxCreateXmlDoc = new System.Windows.Forms.CheckBox();
            this.checkBoxSaveHtml = new System.Windows.Forms.CheckBox();
            this.checkBoxSaveExecutionLogs = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(181, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Enter the URL for file to rip:";
            // 
            // textBoxURL
            // 
            this.textBoxURL.Location = new System.Drawing.Point(27, 60);
            this.textBoxURL.Name = "textBoxURL";
            this.textBoxURL.Size = new System.Drawing.Size(283, 22);
            this.textBoxURL.TabIndex = 1;
            // 
            // buttonRipProspectusFromURL
            // 
            this.buttonRipProspectusFromURL.Location = new System.Drawing.Point(27, 261);
            this.buttonRipProspectusFromURL.Name = "buttonRipProspectusFromURL";
            this.buttonRipProspectusFromURL.Size = new System.Drawing.Size(97, 46);
            this.buttonRipProspectusFromURL.TabIndex = 4;
            this.buttonRipProspectusFromURL.Text = "Rip from URL";
            this.buttonRipProspectusFromURL.UseVisualStyleBackColor = true;
            this.buttonRipProspectusFromURL.Click += new System.EventHandler(this.buttonRipProspectusFromURL_Click);
            // 
            // checkBoxSaveCleanText
            // 
            this.checkBoxSaveCleanText.AutoSize = true;
            this.checkBoxSaveCleanText.Location = new System.Drawing.Point(27, 106);
            this.checkBoxSaveCleanText.Name = "checkBoxSaveCleanText";
            this.checkBoxSaveCleanText.Size = new System.Drawing.Size(93, 21);
            this.checkBoxSaveCleanText.TabIndex = 5;
            this.checkBoxSaveCleanText.Text = "Save Text";
            this.checkBoxSaveCleanText.UseVisualStyleBackColor = true;
            // 
            // checkBoxCreateXmlDoc
            // 
            this.checkBoxCreateXmlDoc.AutoSize = true;
            this.checkBoxCreateXmlDoc.Location = new System.Drawing.Point(27, 144);
            this.checkBoxCreateXmlDoc.Name = "checkBoxCreateXmlDoc";
            this.checkBoxCreateXmlDoc.Size = new System.Drawing.Size(123, 21);
            this.checkBoxCreateXmlDoc.TabIndex = 6;
            this.checkBoxCreateXmlDoc.Text = "Save XML Doc";
            this.checkBoxCreateXmlDoc.UseVisualStyleBackColor = true;
            // 
            // checkBoxSaveHtml
            // 
            this.checkBoxSaveHtml.AutoSize = true;
            this.checkBoxSaveHtml.Location = new System.Drawing.Point(27, 180);
            this.checkBoxSaveHtml.Name = "checkBoxSaveHtml";
            this.checkBoxSaveHtml.Size = new System.Drawing.Size(104, 21);
            this.checkBoxSaveHtml.TabIndex = 7;
            this.checkBoxSaveHtml.Text = "Save HTML";
            this.checkBoxSaveHtml.UseVisualStyleBackColor = true;
            // 
            // checkBoxSaveExecutionLogs
            // 
            this.checkBoxSaveExecutionLogs.AutoSize = true;
            this.checkBoxSaveExecutionLogs.Location = new System.Drawing.Point(27, 217);
            this.checkBoxSaveExecutionLogs.Name = "checkBoxSaveExecutionLogs";
            this.checkBoxSaveExecutionLogs.Size = new System.Drawing.Size(226, 21);
            this.checkBoxSaveExecutionLogs.TabIndex = 8;
            this.checkBoxSaveExecutionLogs.Text = "Save Execution and Error Logs";
            this.checkBoxSaveExecutionLogs.UseVisualStyleBackColor = true;
            // 
            // URLEntryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(330, 337);
            this.Controls.Add(this.checkBoxSaveExecutionLogs);
            this.Controls.Add(this.checkBoxSaveHtml);
            this.Controls.Add(this.checkBoxCreateXmlDoc);
            this.Controls.Add(this.checkBoxSaveCleanText);
            this.Controls.Add(this.buttonRipProspectusFromURL);
            this.Controls.Add(this.textBoxURL);
            this.Controls.Add(this.label1);
            this.Name = "URLEntryForm";
            this.Text = "Enter URL to Rip";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxURL;
        private System.Windows.Forms.Button buttonRipProspectusFromURL;
        private System.Windows.Forms.CheckBox checkBoxSaveCleanText;
        private System.Windows.Forms.CheckBox checkBoxCreateXmlDoc;
        private System.Windows.Forms.CheckBox checkBoxSaveHtml;
        private System.Windows.Forms.CheckBox checkBoxSaveExecutionLogs;
    }
}