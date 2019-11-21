namespace xmlGenerator
{
    partial class OverwriteDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.mainLabel = new System.Windows.Forms.Label();
            this.yesForAllBtn = new System.Windows.Forms.Button();
            this.noForAllBtn = new System.Windows.Forms.Button();
            this.noBtn = new System.Windows.Forms.Button();
            this.yesBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // mainLabel
            // 
            this.mainLabel.Location = new System.Drawing.Point(13, 9);
            this.mainLabel.Name = "mainLabel";
            this.mainLabel.Size = new System.Drawing.Size(345, 56);
            this.mainLabel.TabIndex = 0;
            this.mainLabel.Text = "file.xml already exists";
            this.mainLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // yesForAllBtn
            // 
            this.yesForAllBtn.Location = new System.Drawing.Point(12, 97);
            this.yesForAllBtn.Name = "yesForAllBtn";
            this.yesForAllBtn.Size = new System.Drawing.Size(151, 23);
            this.yesForAllBtn.TabIndex = 1;
            this.yesForAllBtn.Text = "Yes for all";
            this.yesForAllBtn.UseVisualStyleBackColor = true;
            this.yesForAllBtn.Click += new System.EventHandler(this.YesForAllBtn_Click);
            // 
            // noForAllBtn
            // 
            this.noForAllBtn.Location = new System.Drawing.Point(207, 97);
            this.noForAllBtn.Name = "noForAllBtn";
            this.noForAllBtn.Size = new System.Drawing.Size(151, 23);
            this.noForAllBtn.TabIndex = 2;
            this.noForAllBtn.Text = "No for all";
            this.noForAllBtn.UseVisualStyleBackColor = true;
            this.noForAllBtn.Click += new System.EventHandler(this.NoForAllBtn_Click);
            // 
            // noBtn
            // 
            this.noBtn.Location = new System.Drawing.Point(207, 68);
            this.noBtn.Name = "noBtn";
            this.noBtn.Size = new System.Drawing.Size(151, 23);
            this.noBtn.TabIndex = 3;
            this.noBtn.Text = "No";
            this.noBtn.UseVisualStyleBackColor = true;
            this.noBtn.Click += new System.EventHandler(this.NoBtn_Click);
            // 
            // yesBtn
            // 
            this.yesBtn.Location = new System.Drawing.Point(12, 68);
            this.yesBtn.Name = "yesBtn";
            this.yesBtn.Size = new System.Drawing.Size(151, 23);
            this.yesBtn.TabIndex = 4;
            this.yesBtn.Text = "Yes";
            this.yesBtn.UseVisualStyleBackColor = true;
            this.yesBtn.Click += new System.EventHandler(this.YesBtn_Click);
            // 
            // OverwriteDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(370, 128);
            this.Controls.Add(this.yesBtn);
            this.Controls.Add(this.noBtn);
            this.Controls.Add(this.noForAllBtn);
            this.Controls.Add(this.yesForAllBtn);
            this.Controls.Add(this.mainLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OverwriteDialog";
            this.Text = "OverwriteDialog";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label mainLabel;
        private System.Windows.Forms.Button yesForAllBtn;
        private System.Windows.Forms.Button noForAllBtn;
        private System.Windows.Forms.Button noBtn;
        private System.Windows.Forms.Button yesBtn;
    }
}