/*
 * Created by SharpDevelop.
 * User: z002sajh
 * Date: 11/20/2012
 * Time: 2:52 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace TALogTool
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.cfgTextBox = new System.Windows.Forms.TextBox();
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.alyBtn = new System.Windows.Forms.Button();
			this.cfgFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.logFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.logTextBox = new System.Windows.Forms.TextBox();
			this.cfgBtn = new System.Windows.Forms.Button();
			this.logBtn = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.beginLogText = new System.Windows.Forms.TextBox();
			this.endLogText = new System.Windows.Forms.TextBox();
			this.pictureBox = new System.Windows.Forms.PictureBox();
			this.taLogCheckBox = new System.Windows.Forms.CheckBox();
			this.PreDeal = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
			this.SuspendLayout();
			// 
			// cfgTextBox
			// 
			this.cfgTextBox.Location = new System.Drawing.Point(28, 103);
			this.cfgTextBox.Name = "cfgTextBox";
			this.cfgTextBox.Size = new System.Drawing.Size(582, 20);
			this.cfgTextBox.TabIndex = 0;
			// 
			// progressBar1
			// 
			this.progressBar1.Location = new System.Drawing.Point(28, 203);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(582, 20);
			this.progressBar1.TabIndex = 1;
			
			this.progressBar1.Minimum = 0;
			this.progressBar1.Maximum = 100;
			
			// 
			// alyBtn
			// 
			this.alyBtn.Location = new System.Drawing.Point(637, 202);
			this.alyBtn.Name = "alyBtn";
			this.alyBtn.Size = new System.Drawing.Size(73, 22);
			this.alyBtn.TabIndex = 2;
			this.alyBtn.Text = "Analyse";
			this.alyBtn.UseVisualStyleBackColor = true;
			this.alyBtn.Click += new System.EventHandler(this.AlyBtnClick);
			// 
			// cfgFileDialog
			// 
			this.cfgFileDialog.FileName = "cfgFileDialog";
			// 
			// logFileDialog
			// 
			this.logFileDialog.FileName = "logFileDialog";
			// 
			// logTextBox
			// 
			this.logTextBox.Location = new System.Drawing.Point(28, 153);
			this.logTextBox.Name = "logTextBox";
			this.logTextBox.Size = new System.Drawing.Size(582, 20);
			this.logTextBox.TabIndex = 3;
			// 
			// cfgBtn
			// 
			this.cfgBtn.Location = new System.Drawing.Point(637, 103);
			this.cfgBtn.Name = "cfgBtn";
			this.cfgBtn.Size = new System.Drawing.Size(73, 21);
			this.cfgBtn.TabIndex = 4;
			this.cfgBtn.Text = "pattern file";
			this.cfgBtn.UseVisualStyleBackColor = true;
			this.cfgBtn.Click += new System.EventHandler(this.CfgBtnClick);
			// 
			// logBtn
			// 
			this.logBtn.Location = new System.Drawing.Point(637, 155);
			this.logBtn.Name = "logBtn";
			this.logBtn.Size = new System.Drawing.Size(73, 23);
			this.logBtn.TabIndex = 5;
			this.logBtn.Text = "log file";
			this.logBtn.UseVisualStyleBackColor = true;
			this.logBtn.Click += new System.EventHandler(this.LogBtnClick);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(28, 10);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(88, 23);
			this.label1.TabIndex = 6;
			this.label1.Text = "Begin Of Log:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(27, 54);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(89, 25);
			this.label2.TabIndex = 7;
			this.label2.Text = "End of Log:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// beginLogText
			// 
			this.beginLogText.Location = new System.Drawing.Point(122, 11);
			this.beginLogText.Name = "beginLogText";
			this.beginLogText.Size = new System.Drawing.Size(488, 20);
			this.beginLogText.TabIndex = 8;
			this.beginLogText.Text = "End of automated Test (DBID: xxxxx)";
			// 
			// endLogText
			// 
			this.endLogText.Location = new System.Drawing.Point(122, 57);
			this.endLogText.Name = "endLogText";
			this.endLogText.Size = new System.Drawing.Size(487, 20);
			this.endLogText.TabIndex = 9;
			this.endLogText.Text = "Silktest: Start of Test (DBID: xxxxx)";
			// 
			// pictureBox
			// 
			this.pictureBox.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox.Image")));
			this.pictureBox.Location = new System.Drawing.Point(637, 10);
			this.pictureBox.Name = "pictureBox";
			this.pictureBox.Size = new System.Drawing.Size(73, 69);
			this.pictureBox.TabIndex = 10;
			this.pictureBox.TabStop = false;
			// 
			// taLogCheckBox
			// 
			this.taLogCheckBox.Location = new System.Drawing.Point(122, 34);
			this.taLogCheckBox.Name = "taLogCheckBox";
			this.taLogCheckBox.Size = new System.Drawing.Size(123, 23);
			this.taLogCheckBox.TabIndex = 11;
			this.taLogCheckBox.Text = "Include TA Log";
			this.taLogCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.taLogCheckBox.UseVisualStyleBackColor = true;
			// 
			// PreDeal
			// 
			this.PreDeal.Location = new System.Drawing.Point(28, 126);
			this.PreDeal.Name = "PreDeal";
			this.PreDeal.Size = new System.Drawing.Size(102, 24);
			this.PreDeal.TabIndex = 12;
			this.PreDeal.Text = "pre deal pattern";
			this.PreDeal.UseVisualStyleBackColor = true;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(732, 239);
			this.Controls.Add(this.PreDeal);
			this.Controls.Add(this.taLogCheckBox);
			this.Controls.Add(this.pictureBox);
			this.Controls.Add(this.endLogText);
			this.Controls.Add(this.beginLogText);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.logBtn);
			this.Controls.Add(this.cfgBtn);
			this.Controls.Add(this.logTextBox);
			this.Controls.Add(this.alyBtn);
			this.Controls.Add(this.progressBar1);
			this.Controls.Add(this.cfgTextBox);
			this.Name = "MainForm";
			this.Text = "TALogTool";
			((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.CheckBox PreDeal;
		private System.Windows.Forms.CheckBox taLogCheckBox;
		private System.Windows.Forms.PictureBox pictureBox;
		private System.Windows.Forms.TextBox endLogText;
		private System.Windows.Forms.TextBox beginLogText;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button logBtn;
		private System.Windows.Forms.Button cfgBtn;
		private System.Windows.Forms.TextBox logTextBox;
		private System.Windows.Forms.OpenFileDialog logFileDialog;
		private System.Windows.Forms.OpenFileDialog cfgFileDialog;
		private System.Windows.Forms.Button alyBtn;
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.TextBox cfgTextBox;
	}
}
