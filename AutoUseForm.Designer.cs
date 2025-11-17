namespace HiddenUniverse_WebClient
{
    partial class AutoUseForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AutoUseForm));
            this.tbA = new System.Windows.Forms.TableLayoutPanel();
            this.autoUseATime = new System.Windows.Forms.ComboBox();
            this.textA = new System.Windows.Forms.TextBox();
            this.tbB = new System.Windows.Forms.TableLayoutPanel();
            this.autoUseBTime = new System.Windows.Forms.ComboBox();
            this.textB = new System.Windows.Forms.TextBox();
            this.tbC = new System.Windows.Forms.TableLayoutPanel();
            this.autoUseCTime = new System.Windows.Forms.ComboBox();
            this.textC = new System.Windows.Forms.TextBox();
            this.dscText = new System.Windows.Forms.TextBox();
            this.tbA.SuspendLayout();
            this.tbB.SuspendLayout();
            this.tbC.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbA
            // 
            this.tbA.BackColor = System.Drawing.Color.LightSkyBlue;
            this.tbA.ColumnCount = 1;
            this.tbA.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tbA.Controls.Add(this.autoUseATime, 0, 1);
            this.tbA.Controls.Add(this.textA, 0, 0);
            this.tbA.Location = new System.Drawing.Point(12, 117);
            this.tbA.Name = "tbA";
            this.tbA.RowCount = 2;
            this.tbA.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tbA.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tbA.Size = new System.Drawing.Size(214, 111);
            this.tbA.TabIndex = 0;
            // 
            // autoUseATime
            // 
            this.autoUseATime.BackColor = System.Drawing.Color.LightSkyBlue;
            this.autoUseATime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.autoUseATime.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.autoUseATime.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.autoUseATime.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.autoUseATime.FormattingEnabled = true;
            this.autoUseATime.Items.AddRange(new object[] {
            "Every 2 seconds",
            "Every 3 seconds",
            "Every 4 seconds",
            "Every 5 seconds",
            "Every 10 seconds",
            "Every 30 seconds",
            "Every 60 seconds",
            "Every 3 minutes",
            "Every 5 minutes",
            "Every 7 minutes",
            "Every 10 minutes",
            "Every 15 minutes",
            "Every 20 minutes"});
            this.autoUseATime.Location = new System.Drawing.Point(3, 58);
            this.autoUseATime.Name = "autoUseATime";
            this.autoUseATime.Size = new System.Drawing.Size(208, 47);
            this.autoUseATime.TabIndex = 7;
            this.autoUseATime.SelectedIndexChanged += new System.EventHandler(this.autoUseATime_SelectedIndexChanged);
            // 
            // textA
            // 
            this.textA.BackColor = System.Drawing.Color.LightSkyBlue;
            this.textA.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textA.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textA.Enabled = false;
            this.textA.Font = new System.Drawing.Font("Franklin Gothic Medium", 15.85714F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textA.Location = new System.Drawing.Point(3, 3);
            this.textA.Name = "textA";
            this.textA.ReadOnly = true;
            this.textA.Size = new System.Drawing.Size(208, 42);
            this.textA.TabIndex = 3;
            this.textA.Text = "Auto Use A";
            this.textA.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textA.WordWrap = false;
            // 
            // tbB
            // 
            this.tbB.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbB.BackColor = System.Drawing.Color.LightSkyBlue;
            this.tbB.ColumnCount = 1;
            this.tbB.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tbB.Controls.Add(this.autoUseBTime, 0, 1);
            this.tbB.Controls.Add(this.textB, 0, 0);
            this.tbB.Location = new System.Drawing.Point(258, 117);
            this.tbB.Name = "tbB";
            this.tbB.RowCount = 2;
            this.tbB.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tbB.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tbB.Size = new System.Drawing.Size(214, 111);
            this.tbB.TabIndex = 1;
            // 
            // autoUseBTime
            // 
            this.autoUseBTime.BackColor = System.Drawing.Color.LightSkyBlue;
            this.autoUseBTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.autoUseBTime.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.autoUseBTime.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.autoUseBTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.autoUseBTime.FormattingEnabled = true;
            this.autoUseBTime.Items.AddRange(new object[] {
            "Every 2 seconds",
            "Every 3 seconds",
            "Every 4 seconds",
            "Every 5 seconds",
            "Every 10 seconds",
            "Every 30 seconds",
            "Every 60 seconds",
            "Every 3 minutes",
            "Every 5 minutes",
            "Every 7 minutes",
            "Every 10 minutes",
            "Every 15 minutes",
            "Every 20 minutes"});
            this.autoUseBTime.Location = new System.Drawing.Point(3, 58);
            this.autoUseBTime.Name = "autoUseBTime";
            this.autoUseBTime.Size = new System.Drawing.Size(208, 47);
            this.autoUseBTime.TabIndex = 8;
            this.autoUseBTime.SelectedIndexChanged += new System.EventHandler(this.autoUseBTime_SelectedIndexChanged);
            // 
            // textB
            // 
            this.textB.BackColor = System.Drawing.Color.LightSkyBlue;
            this.textB.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textB.Enabled = false;
            this.textB.Font = new System.Drawing.Font("Franklin Gothic Medium", 15.85714F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textB.Location = new System.Drawing.Point(3, 3);
            this.textB.Name = "textB";
            this.textB.ReadOnly = true;
            this.textB.Size = new System.Drawing.Size(208, 42);
            this.textB.TabIndex = 4;
            this.textB.Text = "Auto Use B";
            this.textB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textB.WordWrap = false;
            // 
            // tbC
            // 
            this.tbC.BackColor = System.Drawing.Color.LightSkyBlue;
            this.tbC.ColumnCount = 1;
            this.tbC.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tbC.Controls.Add(this.autoUseCTime, 0, 1);
            this.tbC.Controls.Add(this.textC, 0, 0);
            this.tbC.Location = new System.Drawing.Point(502, 117);
            this.tbC.Name = "tbC";
            this.tbC.RowCount = 2;
            this.tbC.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tbC.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tbC.Size = new System.Drawing.Size(214, 111);
            this.tbC.TabIndex = 2;
            // 
            // autoUseCTime
            // 
            this.autoUseCTime.BackColor = System.Drawing.Color.LightSkyBlue;
            this.autoUseCTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.autoUseCTime.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.autoUseCTime.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.autoUseCTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.autoUseCTime.FormattingEnabled = true;
            this.autoUseCTime.Items.AddRange(new object[] {
            "Every 2 seconds",
            "Every 3 seconds",
            "Every 4 seconds",
            "Every 5 seconds",
            "Every 10 seconds",
            "Every 30 seconds",
            "Every 60 seconds",
            "Every 3 minutes",
            "Every 5 minutes",
            "Every 7 minutes",
            "Every 10 minutes",
            "Every 15 minutes",
            "Every 20 minutes"});
            this.autoUseCTime.Location = new System.Drawing.Point(3, 58);
            this.autoUseCTime.Name = "autoUseCTime";
            this.autoUseCTime.Size = new System.Drawing.Size(208, 47);
            this.autoUseCTime.TabIndex = 8;
            this.autoUseCTime.SelectedIndexChanged += new System.EventHandler(this.autoUseCTime_SelectedIndexChanged);
            // 
            // textC
            // 
            this.textC.BackColor = System.Drawing.Color.LightSkyBlue;
            this.textC.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textC.Enabled = false;
            this.textC.Font = new System.Drawing.Font("Franklin Gothic Medium", 15.85714F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textC.Location = new System.Drawing.Point(3, 3);
            this.textC.Name = "textC";
            this.textC.ReadOnly = true;
            this.textC.Size = new System.Drawing.Size(208, 42);
            this.textC.TabIndex = 4;
            this.textC.Text = "Auto Use C";
            this.textC.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textC.WordWrap = false;
            // 
            // dscText
            // 
            this.dscText.BackColor = System.Drawing.Color.LightSteelBlue;
            this.dscText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dscText.Dock = System.Windows.Forms.DockStyle.Top;
            this.dscText.Font = new System.Drawing.Font("Franklin Gothic Medium", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dscText.ForeColor = System.Drawing.Color.Firebrick;
            this.dscText.Location = new System.Drawing.Point(0, 0);
            this.dscText.Multiline = true;
            this.dscText.Name = "dscText";
            this.dscText.ReadOnly = true;
            this.dscText.Size = new System.Drawing.Size(728, 79);
            this.dscText.TabIndex = 3;
            this.dscText.TabStop = false;
            this.dscText.Text = resources.GetString("dscText.Text");
            this.dscText.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // AutoUseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SteelBlue;
            this.ClientSize = new System.Drawing.Size(728, 271);
            this.Controls.Add(this.dscText);
            this.Controls.Add(this.tbC);
            this.Controls.Add(this.tbB);
            this.Controls.Add(this.tbA);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AutoUseForm";
            this.Opacity = 0.95D;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Hidden Universe Auto Use Settings";
            this.TopMost = true;
            this.tbA.ResumeLayout(false);
            this.tbA.PerformLayout();
            this.tbB.ResumeLayout(false);
            this.tbB.PerformLayout();
            this.tbC.ResumeLayout(false);
            this.tbC.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tbA;
        private System.Windows.Forms.TableLayoutPanel tbB;
        private System.Windows.Forms.TableLayoutPanel tbC;
        private System.Windows.Forms.TextBox textA;
        private System.Windows.Forms.TextBox textB;
        private System.Windows.Forms.TextBox textC;
        private System.Windows.Forms.ComboBox autoUseATime;
        private System.Windows.Forms.ComboBox autoUseBTime;
        private System.Windows.Forms.ComboBox autoUseCTime;
        private System.Windows.Forms.TextBox dscText;
    }
}