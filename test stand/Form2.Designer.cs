namespace test_stand
{
    partial class Form2
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
            this.CBoxPort = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TBoxBaudRate = new System.Windows.Forms.TextBox();
            this.BtnOpen = new System.Windows.Forms.Button();
            this.BtnClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(87, 78);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Com Port:";
            // 
            // CBoxPort
            // 
            this.CBoxPort.BackColor = System.Drawing.Color.LightGray;
            this.CBoxPort.FormattingEnabled = true;
            this.CBoxPort.Location = new System.Drawing.Point(188, 78);
            this.CBoxPort.Name = "CBoxPort";
            this.CBoxPort.Size = new System.Drawing.Size(121, 21);
            this.CBoxPort.TabIndex = 1;
            this.CBoxPort.SelectedIndexChanged += new System.EventHandler(this.CBoxPort_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(75, 123);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Baud Rate:";
            // 
            // TBoxBaudRate
            // 
            this.TBoxBaudRate.BackColor = System.Drawing.Color.LightGray;
            this.TBoxBaudRate.Location = new System.Drawing.Point(188, 123);
            this.TBoxBaudRate.Name = "TBoxBaudRate";
            this.TBoxBaudRate.Size = new System.Drawing.Size(121, 20);
            this.TBoxBaudRate.TabIndex = 3;
            this.TBoxBaudRate.TextChanged += new System.EventHandler(this.TBoxBaudRate_TextChanged);
            // 
            // BtnOpen
            // 
            this.BtnOpen.BackColor = System.Drawing.Color.LightGray;
            this.BtnOpen.Location = new System.Drawing.Point(127, 163);
            this.BtnOpen.Name = "BtnOpen";
            this.BtnOpen.Size = new System.Drawing.Size(121, 23);
            this.BtnOpen.TabIndex = 4;
            this.BtnOpen.Text = "Open";
            this.BtnOpen.UseVisualStyleBackColor = false;
            // 
            // BtnClose
            // 
            this.BtnClose.BackColor = System.Drawing.Color.LightGray;
            this.BtnClose.Location = new System.Drawing.Point(127, 163);
            this.BtnClose.Name = "BtnClose";
            this.BtnClose.Size = new System.Drawing.Size(121, 23);
            this.BtnClose.TabIndex = 5;
            this.BtnClose.Text = "Close";
            this.BtnClose.UseVisualStyleBackColor = false;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(162)))), ((int)(((byte)(31)))), ((int)(((byte)(123)))));
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.BtnClose);
            this.Controls.Add(this.BtnOpen);
            this.Controls.Add(this.TBoxBaudRate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.CBoxPort);
            this.Controls.Add(this.label1);
            this.Name = "Form2";
            this.Text = "Form2";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox CBoxPort;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TBoxBaudRate;
        private System.Windows.Forms.Button BtnOpen;
        private System.Windows.Forms.Button BtnClose;
    }
}