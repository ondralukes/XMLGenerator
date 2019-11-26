namespace xmlGenerator
{
    partial class ValidationResultForm
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
            this.exceptionText = new System.Windows.Forms.TextBox();
            this.continueBtn = new System.Windows.Forms.Button();
            this.stopBtn = new System.Windows.Forms.Button();
            this.exceptionList = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // exceptionText
            // 
            this.exceptionText.Location = new System.Drawing.Point(12, 203);
            this.exceptionText.Multiline = true;
            this.exceptionText.Name = "exceptionText";
            this.exceptionText.ReadOnly = true;
            this.exceptionText.Size = new System.Drawing.Size(776, 168);
            this.exceptionText.TabIndex = 1;
            // 
            // continueBtn
            // 
            this.continueBtn.Location = new System.Drawing.Point(13, 377);
            this.continueBtn.Name = "continueBtn";
            this.continueBtn.Size = new System.Drawing.Size(247, 61);
            this.continueBtn.TabIndex = 2;
            this.continueBtn.Text = "Continue";
            this.continueBtn.UseVisualStyleBackColor = true;
            this.continueBtn.Click += new System.EventHandler(this.ContinueBtn_Click);
            // 
            // stopBtn
            // 
            this.stopBtn.Location = new System.Drawing.Point(541, 377);
            this.stopBtn.Name = "stopBtn";
            this.stopBtn.Size = new System.Drawing.Size(247, 61);
            this.stopBtn.TabIndex = 3;
            this.stopBtn.Text = "Stop";
            this.stopBtn.UseVisualStyleBackColor = true;
            this.stopBtn.Click += new System.EventHandler(this.StopBtn_Click);
            // 
            // exceptionList
            // 
            this.exceptionList.HideSelection = false;
            this.exceptionList.Location = new System.Drawing.Point(12, 12);
            this.exceptionList.MultiSelect = false;
            this.exceptionList.Name = "exceptionList";
            this.exceptionList.Size = new System.Drawing.Size(776, 185);
            this.exceptionList.TabIndex = 4;
            this.exceptionList.UseCompatibleStateImageBehavior = false;
            this.exceptionList.View = System.Windows.Forms.View.Details;
            this.exceptionList.SelectedIndexChanged += new System.EventHandler(this.ExceptionList_SelectedIndexChanged);
            // 
            // ValidationResultForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.exceptionList);
            this.Controls.Add(this.stopBtn);
            this.Controls.Add(this.continueBtn);
            this.Controls.Add(this.exceptionText);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ValidationResultForm";
            this.Text = "Validation result";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ValidationResultForm_FormClosing);
            this.Load += new System.EventHandler(this.ValidationResultForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox exceptionText;
        private System.Windows.Forms.Button continueBtn;
        private System.Windows.Forms.Button stopBtn;
        private System.Windows.Forms.ListView exceptionList;
    }
}