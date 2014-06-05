namespace Калькулятор_посчитайкин
{
    partial class FHelp
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FHelp));
            this.tb_manual = new System.Windows.Forms.TextBox();
            this.lbl_me = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tb_manual
            // 
            this.tb_manual.BackColor = System.Drawing.Color.White;
            this.tb_manual.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tb_manual.Location = new System.Drawing.Point(12, 12);
            this.tb_manual.Multiline = true;
            this.tb_manual.Name = "tb_manual";
            this.tb_manual.ReadOnly = true;
            this.tb_manual.Size = new System.Drawing.Size(344, 386);
            this.tb_manual.TabIndex = 14;
            this.tb_manual.TabStop = false;
            // 
            // lbl_me
            // 
            this.lbl_me.AutoSize = true;
            this.lbl_me.BackColor = System.Drawing.Color.Transparent;
            this.lbl_me.Font = new System.Drawing.Font("Matura MT Script Capitals", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_me.ForeColor = System.Drawing.Color.DodgerBlue;
            this.lbl_me.Location = new System.Drawing.Point(172, 400);
            this.lbl_me.Name = "lbl_me";
            this.lbl_me.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lbl_me.Size = new System.Drawing.Size(202, 28);
            this.lbl_me.TabIndex = 15;
            this.lbl_me.Text = "Sevenmi007 2014.";
            // 
            // F_Help
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(368, 427);
            this.Controls.Add(this.lbl_me);
            this.Controls.Add(this.tb_manual);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FHelp";
            this.Text = "Help";
            this.Load += new System.EventHandler(this.Help_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tb_manual;
        private System.Windows.Forms.Label lbl_me;
    }
}