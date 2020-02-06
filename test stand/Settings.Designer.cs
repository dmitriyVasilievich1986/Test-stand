namespace test_stand
{
    partial class Settings
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
            this.On = new System.Windows.Forms.Button();
            this.Off = new System.Windows.Forms.Button();
            this.Dout_Din16 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Dout_Control = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.TBMTU5 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.TBPSC = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.CBCurrent_Check = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.Dout_Din32 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // On
            // 
            this.On.BackColor = System.Drawing.Color.LightGray;
            this.On.FlatAppearance.BorderSize = 0;
            this.On.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.On.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.On.Location = new System.Drawing.Point(86, 77);
            this.On.Name = "On";
            this.On.Size = new System.Drawing.Size(150, 40);
            this.On.TabIndex = 0;
            this.On.Text = "Power On";
            this.On.UseVisualStyleBackColor = false;
            // 
            // Off
            // 
            this.Off.BackColor = System.Drawing.Color.LightGray;
            this.Off.FlatAppearance.BorderSize = 0;
            this.Off.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Off.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Off.Location = new System.Drawing.Point(86, 117);
            this.Off.Name = "Off";
            this.Off.Size = new System.Drawing.Size(150, 40);
            this.Off.TabIndex = 1;
            this.Off.Text = "Power Off";
            this.Off.UseVisualStyleBackColor = false;
            // 
            // Dout_Din16
            // 
            this.Dout_Din16.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.Dout_Din16.Location = new System.Drawing.Point(415, 223);
            this.Dout_Din16.Name = "Dout_Din16";
            this.Dout_Din16.Size = new System.Drawing.Size(139, 23);
            this.Dout_Din16.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.ForeColor = System.Drawing.Color.LightGray;
            this.label1.Location = new System.Drawing.Point(83, 225);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(301, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Адрес модуля Dout управления  Din16:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.ForeColor = System.Drawing.Color.LightGray;
            this.label2.Location = new System.Drawing.Point(83, 305);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(321, 17);
            this.label2.TabIndex = 5;
            this.label2.Text = "Адрес модуля Dout управления  питания:";
            // 
            // Dout_Control
            // 
            this.Dout_Control.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.Dout_Control.Location = new System.Drawing.Point(415, 303);
            this.Dout_Control.Name = "Dout_Control";
            this.Dout_Control.Size = new System.Drawing.Size(139, 23);
            this.Dout_Control.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.label3.ForeColor = System.Drawing.Color.LightGray;
            this.label3.Location = new System.Drawing.Point(83, 529);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(105, 17);
            this.label3.TabIndex = 6;
            this.label3.Text = "Version 1.6.0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.ForeColor = System.Drawing.Color.LightGray;
            this.label4.Location = new System.Drawing.Point(83, 349);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(164, 17);
            this.label4.TabIndex = 8;
            this.label4.Text = "Адрес модуля MTU5:";
            // 
            // TBMTU5
            // 
            this.TBMTU5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.TBMTU5.Location = new System.Drawing.Point(415, 347);
            this.TBMTU5.Name = "TBMTU5";
            this.TBMTU5.Size = new System.Drawing.Size(139, 23);
            this.TBMTU5.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.ForeColor = System.Drawing.Color.LightGray;
            this.label5.Location = new System.Drawing.Point(83, 391);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(218, 17);
            this.label5.TabIndex = 10;
            this.label5.Text = "Адрес модуля PSC 24V 10A:";
            // 
            // TBPSC
            // 
            this.TBPSC.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.TBPSC.Location = new System.Drawing.Point(415, 389);
            this.TBPSC.Name = "TBPSC";
            this.TBPSC.Size = new System.Drawing.Size(139, 23);
            this.TBPSC.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.ForeColor = System.Drawing.Color.LightGray;
            this.label6.Location = new System.Drawing.Point(83, 428);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(124, 17);
            this.label6.TabIndex = 11;
            this.label6.Text = "Проверка тока:";
            // 
            // CBCurrent_Check
            // 
            this.CBCurrent_Check.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.CBCurrent_Check.FormattingEnabled = true;
            this.CBCurrent_Check.Items.AddRange(new object[] {
            "Включена",
            "Выключена"});
            this.CBCurrent_Check.Location = new System.Drawing.Point(415, 428);
            this.CBCurrent_Check.Name = "CBCurrent_Check";
            this.CBCurrent_Check.Size = new System.Drawing.Size(139, 24);
            this.CBCurrent_Check.TabIndex = 12;
            this.CBCurrent_Check.Text = "Выключена";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label7.ForeColor = System.Drawing.Color.LightGray;
            this.label7.Location = new System.Drawing.Point(83, 265);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(306, 17);
            this.label7.TabIndex = 14;
            this.label7.Text = "Адрес модуля Dout управления  Din 32:";
            // 
            // Dout_Din32
            // 
            this.Dout_Din32.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.Dout_Din32.Location = new System.Drawing.Point(415, 263);
            this.Dout_Din32.Name = "Dout_Din32";
            this.Dout_Din32.Size = new System.Drawing.Size(139, 23);
            this.Dout_Din32.TabIndex = 13;
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(162)))), ((int)(((byte)(31)))), ((int)(((byte)(123)))));
            this.ClientSize = new System.Drawing.Size(800, 568);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.Dout_Din32);
            this.Controls.Add(this.CBCurrent_Check);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.TBPSC);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.TBMTU5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Dout_Control);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Dout_Din16);
            this.Controls.Add(this.Off);
            this.Controls.Add(this.On);
            this.Name = "Settings";
            this.Text = "Form4";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button On;
        private System.Windows.Forms.Button Off;
        private System.Windows.Forms.TextBox Dout_Din16;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox Dout_Control;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox TBMTU5;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox TBPSC;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox CBCurrent_Check;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox Dout_Din32;
    }
}