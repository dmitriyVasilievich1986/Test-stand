namespace test_stand
{
    partial class Form6
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
            this.PortCondition = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // PortCondition
            // 
            this.PortCondition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PortCondition.Location = new System.Drawing.Point(0, 0);
            this.PortCondition.Multiline = true;
            this.PortCondition.Name = "PortCondition";
            this.PortCondition.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.PortCondition.Size = new System.Drawing.Size(888, 313);
            this.PortCondition.TabIndex = 0;
            // 
            // Form6
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(888, 313);
            this.Controls.Add(this.PortCondition);
            this.Name = "Form6";
            this.Text = "Form6";
            this.Load += new System.EventHandler(this.Form6_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox PortCondition;
    }
}