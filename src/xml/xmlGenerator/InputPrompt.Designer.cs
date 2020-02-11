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
            this.SIdentificationTextBox = new System.Windows.Forms.TextBox();
            this.RIdentificationTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.okBtn = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.outagesCountUpDown = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.outagesCountUpDown)).BeginInit();
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
            this.cTimeIntTextBox.Location = new System.Drawing.Point(180, 11);
            this.cTimeIntTextBox.Name = "cTimeIntTextBox";
            this.cTimeIntTextBox.Size = new System.Drawing.Size(375, 22);
            this.cTimeIntTextBox.TabIndex = 1;
            this.cTimeIntTextBox.Text = "2019-02-19T23:00Z/2019-02-20T23:00Z";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(133, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "SenderIdentification";
            this.label2.Click += new System.EventHandler(this.Label2_Click);
            // 
            // SIdentificationTextBox
            // 
            this.SIdentificationTextBox.Location = new System.Drawing.Point(180, 39);
            this.SIdentificationTextBox.Name = "SIdentificationTextBox";
            this.SIdentificationTextBox.Size = new System.Drawing.Size(375, 22);
            this.SIdentificationTextBox.TabIndex = 3;
            this.SIdentificationTextBox.Text = "17XTSO-CS------W";
            this.SIdentificationTextBox.TextChanged += new System.EventHandler(this.SIdentificationTextBox_TextChanged);
            // 
            // RIdentificationTextBox
            // 
            this.RIdentificationTextBox.Location = new System.Drawing.Point(180, 67);
            this.RIdentificationTextBox.Name = "RIdentificationTextBox";
            this.RIdentificationTextBox.Size = new System.Drawing.Size(375, 22);
            this.RIdentificationTextBox.TabIndex = 4;
            this.RIdentificationTextBox.Text = "17XTSO-CS------W";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(143, 17);
            this.label3.TabIndex = 5;
            this.label3.Text = "ReceiverIdentification";
            // 
            // okBtn
            // 
            this.okBtn.Location = new System.Drawing.Point(16, 123);
            this.okBtn.Name = "okBtn";
            this.okBtn.Size = new System.Drawing.Size(540, 29);
            this.okBtn.TabIndex = 6;
            this.okBtn.Text = "OK";
            this.okBtn.UseVisualStyleBackColor = true;
            this.okBtn.Click += new System.EventHandler(this.OkBtn_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 97);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(135, 17);
            this.label4.TabIndex = 7;
            this.label4.Text = "Outages per branch";
            // 
            // outagesCountUpDown
            // 
            this.outagesCountUpDown.Location = new System.Drawing.Point(180, 95);
            this.outagesCountUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.outagesCountUpDown.Name = "outagesCountUpDown";
            this.outagesCountUpDown.Size = new System.Drawing.Size(120, 22);
            this.outagesCountUpDown.TabIndex = 8;
            this.outagesCountUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // InputPrompt
            // 
            this.AcceptButton = this.okBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(567, 161);
            this.Controls.Add(this.outagesCountUpDown);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.okBtn);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.RIdentificationTextBox);
            this.Controls.Add(this.SIdentificationTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cTimeIntTextBox);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InputPrompt";
            this.Text = "InputPrompt";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.InputPrompt_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.outagesCountUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox cTimeIntTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox SIdentificationTextBox;
        private System.Windows.Forms.TextBox RIdentificationTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button okBtn;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown outagesCountUpDown;
    }
}