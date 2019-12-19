namespace test_stand
{
    partial class Form5
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
            this.Serial_Number = new System.Windows.Forms.TextBox();
            this.Test_Condition = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Serial_Number
            // 
            this.Serial_Number.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.Serial_Number.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Serial_Number.Location = new System.Drawing.Point(61, 77);
            this.Serial_Number.Name = "Serial_Number";
            this.Serial_Number.Size = new System.Drawing.Size(260, 23);
            this.Serial_Number.TabIndex = 1;
            this.Serial_Number.Text = "Введите серийный номер";
            this.Serial_Number.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Test_Condition
            // 
            this.Test_Condition.AutoSize = true;
            this.Test_Condition.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.Test_Condition.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Test_Condition.Location = new System.Drawing.Point(25, 33);
            this.Test_Condition.Name = "Test_Condition";
            this.Test_Condition.Size = new System.Drawing.Size(0, 17);
            this.Test_Condition.TabIndex = 3;
            // 
            // Form5
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(40)))), ((int)(((byte)(52)))));
            this.BackgroundImage = global::test_stand.Properties.Resources.загружено;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(494, 265);
            this.Controls.Add(this.Test_Condition);
            this.Controls.Add(this.Serial_Number);
            this.Name = "Form5";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form5";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox Serial_Number;
        private System.Windows.Forms.Label Test_Condition;
    }
}