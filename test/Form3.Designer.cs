namespace test
{
    partial class Form3
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
            this.uc_ListBox1 = new YC_COMMON.uc_ListBox();
            this.SuspendLayout();
            // 
            // uc_ListBox1
            // 
            this.uc_ListBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.uc_ListBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uc_ListBox1.Location = new System.Drawing.Point(0, 0);
            this.uc_ListBox1.Name = "uc_ListBox1";
            this.uc_ListBox1.Size = new System.Drawing.Size(284, 261);
            this.uc_ListBox1.TabIndex = 0;
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.uc_ListBox1);
            this.Name = "Form3";
            this.Text = "Form3";
            this.ResumeLayout(false);

        }

        #endregion

        private YC_COMMON.uc_ListBox uc_ListBox1;


    }
}