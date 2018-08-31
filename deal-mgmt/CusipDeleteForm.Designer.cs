namespace Trinity.DealManagementUtils
{
	partial class CusipDeleteForm
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
			this.tbCusip = new System.Windows.Forms.TextBox ();
			this.label1 = new System.Windows.Forms.Label ();
			this.buttonDelete = new System.Windows.Forms.Button ();
			this.SuspendLayout ();
			// 
			// tbCusip
			// 
			this.tbCusip.Location = new System.Drawing.Point (31, 43);
			this.tbCusip.Name = "tbCusip";
			this.tbCusip.Size = new System.Drawing.Size (100, 22);
			this.tbCusip.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point (138, 47);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size (81, 17);
			this.label1.TabIndex = 1;
			this.label1.Text = "Enter Cusip";
			// 
			// buttonDelete
			// 
			this.buttonDelete.Location = new System.Drawing.Point (31, 90);
			this.buttonDelete.Name = "buttonDelete";
			this.buttonDelete.Size = new System.Drawing.Size (100, 32);
			this.buttonDelete.TabIndex = 2;
			this.buttonDelete.Text = "Delete";
			this.buttonDelete.UseVisualStyleBackColor = true;
			this.buttonDelete.Click += new System.EventHandler (this.buttonDelete_Click);
			// 
			// CusipDeleteForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF (8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size (299, 142);
			this.Controls.Add (this.buttonDelete);
			this.Controls.Add (this.label1);
			this.Controls.Add (this.tbCusip);
			this.Name = "CusipDeleteForm";
			this.Text = "Cusip Deletion";
			this.ResumeLayout (false);
			this.PerformLayout ();

		}

		#endregion

		private System.Windows.Forms.TextBox tbCusip;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button buttonDelete;
	}
}