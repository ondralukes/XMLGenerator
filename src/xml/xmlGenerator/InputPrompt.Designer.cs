namespace xmlGenerator
{
    partial class InputPrompt
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
            this.label1 = new System.Windows.Forms.Label();
            this.cTimeIntTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SIndentificationTextBox = new System.Windows.Forms.TextBox();
            this.RIndentificationTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.okBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(149, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "ConstraintTimeInterval";
            // 
            // cTimeIntTextBox
            // 
            this.cTimeIntTextBox.Location = new System.Drawing.Point(175, 10);
            this.cTimeIntTextBox.Name = "cTimeIntTextBox";
            this.cTimeIntTextBox.Size = new System.Drawing.Size(247, 22);
            this.cTimeIntTextBox.TabIndex = 1;
            this.cTimeIntTextBox.Text = "2019-02-19T23:00Z/2019-02-20T23:00Z";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(141, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "SenderIndentification";
            this.label2.Click += new System.EventHandler(this.Label2_Click);
            // 
            // SIndentificationTextBox
            // 
            this.SIndentificationTextBox.Location = new System.Drawing.Point(175, 39);
            this.SIndentificationTextBox.Name = "SIndentificationTextBox";
            this.SIndentificationTextBox.Size = new System.Drawing.Size(247, 22);
            this.SIndentificationTextBox.TabIndex = 3;
            this.SIndentificationTextBox.Text = "17XTSO-CS------W";
            this.SIndentificationTextBox.TextChanged += new System.EventHandler(this.SIndentificationTextBox_TextChanged);
            // 
            // RIndentificationTextBox
            // 
            this.RIndentificationTextBox.Location = new System.Drawing.Point(175, 67);
            this.RIndentificationTextBox.Name = "RIndentificationTextBox";
            this.RIndentificationTextBox.Size = new System.Drawing.Size(247, 22);
            this.RIndentificationTextBox.TabIndex = 4;
            this.RIndentificationTextBox.Text = "17XTSO-CS------W";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(151, 17);
            this.label3.TabIndex = 5;
            this.label3.Text = "ReceiverIndentification";
            // 
            // okBtn
            // 
            this.okBtn.Location = new System.Drawing.Point(15, 96);
            this.okBtn.Name = "okBtn";
            this.okBtn.Size = new System.Drawing.Size(407, 29);
            this.okBtn.TabIndex = 6;
            this.okBtn.Text = "OK";
            this.okBtn.UseVisualStyleBackColor = true;
            this.okBtn.Click += new System.EventHandler(this.OkBtn_Click);
            // 
            // InputPrompt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 138);
            this.Controls.Add(this.okBtn);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.RIndentificationTextBox);
            this.Controls.Add(this.SIndentificationTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cTimeIntTextBox);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InputPrompt";
            this.Text = "InputPrompt";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox cTimeIntTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox SIndentificationTextBox;
        private System.Windows.Forms.TextBox RIndentificationTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button okBtn;
    }
}