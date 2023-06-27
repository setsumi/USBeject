namespace USBeject
{
    partial class FormMain
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.tbInfo = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnEjectDialog = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.lblProcessName = new System.Windows.Forms.Label();
            this.btnKill = new System.Windows.Forms.Button();
            this.tbPID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbInfo
            // 
            this.tbInfo.BackColor = System.Drawing.SystemColors.Window;
            this.tbInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbInfo.Location = new System.Drawing.Point(0, 0);
            this.tbInfo.Multiline = true;
            this.tbInfo.Name = "tbInfo";
            this.tbInfo.ReadOnly = true;
            this.tbInfo.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbInfo.Size = new System.Drawing.Size(671, 371);
            this.tbInfo.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnEjectDialog);
            this.panel1.Controls.Add(this.btnRefresh);
            this.panel1.Controls.Add(this.lblProcessName);
            this.panel1.Controls.Add(this.btnKill);
            this.panel1.Controls.Add(this.tbPID);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 371);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(671, 43);
            this.panel1.TabIndex = 1;
            // 
            // btnEjectDialog
            // 
            this.btnEjectDialog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEjectDialog.Location = new System.Drawing.Point(501, 8);
            this.btnEjectDialog.Name = "btnEjectDialog";
            this.btnEjectDialog.Size = new System.Drawing.Size(73, 23);
            this.btnEjectDialog.TabIndex = 3;
            this.btnEjectDialog.Text = "Eject dialog";
            this.btnEjectDialog.UseVisualStyleBackColor = true;
            this.btnEjectDialog.Click += new System.EventHandler(this.btnEjectDialog_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefresh.Location = new System.Drawing.Point(580, 8);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 4;
            this.btnRefresh.Text = "Refresh (F5)";
            this.toolTip1.SetToolTip(this.btnRefresh, "Refresh event list (F5)");
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // lblProcessName
            // 
            this.lblProcessName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblProcessName.AutoSize = true;
            this.lblProcessName.Location = new System.Drawing.Point(238, 13);
            this.lblProcessName.Name = "lblProcessName";
            this.lblProcessName.Size = new System.Drawing.Size(83, 13);
            this.lblProcessName.TabIndex = 2;
            this.lblProcessName.Text = "lblProcessName";
            // 
            // btnKill
            // 
            this.btnKill.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnKill.Location = new System.Drawing.Point(157, 8);
            this.btnKill.Name = "btnKill";
            this.btnKill.Size = new System.Drawing.Size(75, 23);
            this.btnKill.TabIndex = 1;
            this.btnKill.Text = "Close/Kill";
            this.toolTip1.SetToolTip(this.btnKill, "Close offending process");
            this.btnKill.UseVisualStyleBackColor = true;
            this.btnKill.Click += new System.EventHandler(this.btnKill_Click);
            // 
            // tbPID
            // 
            this.tbPID.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tbPID.Location = new System.Drawing.Point(78, 10);
            this.tbPID.Name = "tbPID";
            this.tbPID.Size = new System.Drawing.Size(73, 20);
            this.tbPID.TabIndex = 0;
            this.tbPID.TextChanged += new System.EventHandler(this.tbPID_TextChanged);
            this.tbPID.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbPID_KeyDown);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Process ID";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 25);
            this.label2.TabIndex = 2;
            this.label2.Text = "Loading...";
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(671, 414);
            this.Controls.Add(this.tbInfo);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "FormMain";
            this.Text = "USBeject";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.Shown += new System.EventHandler(this.FormMain_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormMain_KeyDown);
            this.Resize += new System.EventHandler(this.FormMain_Resize);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbInfo;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblProcessName;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnKill;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbPID;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnEjectDialog;
    }
}

