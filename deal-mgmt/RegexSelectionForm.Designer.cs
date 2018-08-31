namespace Trinity.DealManagementUtils
{
    partial class RegexSelectionForm
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
            this.comboBoxRegexTypes = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxRegexFileNameSave = new System.Windows.Forms.TextBox();
            this.buttonSave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // comboBoxRegexTypes
            // 
            this.comboBoxRegexTypes.FormattingEnabled = true;
            this.comboBoxRegexTypes.Location = new System.Drawing.Point(22, 26);
            this.comboBoxRegexTypes.Name = "comboBoxRegexTypes";
            this.comboBoxRegexTypes.Size = new System.Drawing.Size(238, 24);
            this.comboBoxRegexTypes.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 79);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(242, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "What would you like to name the file?";
            // 
            // textBoxRegexFileNameSave
            // 
            this.textBoxRegexFileNameSave.Location = new System.Drawing.Point(25, 116);
            this.textBoxRegexFileNameSave.Name = "textBoxRegexFileNameSave";
            this.textBoxRegexFileNameSave.Size = new System.Drawing.Size(181, 22);
            this.textBoxRegexFileNameSave.TabIndex = 2;
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(25, 164);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(94, 34);
            this.buttonSave.TabIndex = 3;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // RegexSelectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(331, 235);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.textBoxRegexFileNameSave);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxRegexTypes);
            this.Name = "RegexSelectionForm";
            this.Text = "Choose Regex";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxRegexTypes;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxRegexFileNameSave;
        private System.Windows.Forms.Button buttonSave;
    }
}