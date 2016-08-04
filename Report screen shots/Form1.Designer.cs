namespace Report_screen_shots
{
    partial class Form1
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
            this.run_btn = new System.Windows.Forms.Button();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.from_date = new System.Windows.Forms.Label();
            this.to_date = new System.Windows.Forms.Label();
            this.browse_btn = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.checkBox_banner = new System.Windows.Forms.CheckBox();
            this.checkBox_lightbox = new System.Windows.Forms.CheckBox();
            this.checkBox_newpop = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.listView1 = new System.Windows.Forms.ListView();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // run_btn
            // 
            this.run_btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F);
            this.run_btn.Location = new System.Drawing.Point(30, 688);
            this.run_btn.Name = "run_btn";
            this.run_btn.Size = new System.Drawing.Size(150, 53);
            this.run_btn.TabIndex = 0;
            this.run_btn.Text = "Run";
            this.run_btn.UseVisualStyleBackColor = true;
            this.run_btn.Click += new System.EventHandler(this.run_btn_Click);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(31, 215);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(210, 20);
            this.dateTimePicker1.TabIndex = 1;
            this.dateTimePicker1.ValueChanged += new System.EventHandler(this.dateTimePicker1_ValueChanged);
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Location = new System.Drawing.Point(31, 121);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(209, 20);
            this.dateTimePicker2.TabIndex = 2;
            this.dateTimePicker2.ValueChanged += new System.EventHandler(this.dateTimePicker2_ValueChanged);
            // 
            // from_date
            // 
            this.from_date.AutoSize = true;
            this.from_date.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.from_date.Location = new System.Drawing.Point(28, 78);
            this.from_date.Name = "from_date";
            this.from_date.Size = new System.Drawing.Size(89, 20);
            this.from_date.TabIndex = 5;
            this.from_date.Text = "From Date:";
            // 
            // to_date
            // 
            this.to_date.AutoSize = true;
            this.to_date.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.to_date.Location = new System.Drawing.Point(28, 173);
            this.to_date.Name = "to_date";
            this.to_date.Size = new System.Drawing.Size(70, 20);
            this.to_date.TabIndex = 6;
            this.to_date.Text = "To Date:";
            // 
            // browse_btn
            // 
            this.browse_btn.Location = new System.Drawing.Point(30, 309);
            this.browse_btn.Name = "browse_btn";
            this.browse_btn.Size = new System.Drawing.Size(112, 40);
            this.browse_btn.TabIndex = 8;
            this.browse_btn.Text = "Save to..";
            this.browse_btn.UseVisualStyleBackColor = true;
            this.browse_btn.Click += new System.EventHandler(this.browse_btn_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(30, 371);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(210, 20);
            this.textBox1.TabIndex = 9;
            // 
            // checkBox_banner
            // 
            this.checkBox_banner.AutoSize = true;
            this.checkBox_banner.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.checkBox_banner.Location = new System.Drawing.Point(30, 542);
            this.checkBox_banner.Name = "checkBox_banner";
            this.checkBox_banner.Size = new System.Drawing.Size(73, 21);
            this.checkBox_banner.TabIndex = 10;
            this.checkBox_banner.Text = "Banner";
            this.checkBox_banner.UseVisualStyleBackColor = true;
            this.checkBox_banner.CheckedChanged += new System.EventHandler(this.checkBox_banner_CheckedChanged);
            // 
            // checkBox_lightbox
            // 
            this.checkBox_lightbox.AutoSize = true;
            this.checkBox_lightbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.checkBox_lightbox.Location = new System.Drawing.Point(30, 566);
            this.checkBox_lightbox.Name = "checkBox_lightbox";
            this.checkBox_lightbox.Size = new System.Drawing.Size(167, 21);
            this.checkBox_lightbox.TabIndex = 11;
            this.checkBox_lightbox.Text = "Lighbox direct old Pop";
            this.checkBox_lightbox.UseVisualStyleBackColor = true;
            // 
            // checkBox_newpop
            // 
            this.checkBox_newpop.AutoSize = true;
            this.checkBox_newpop.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.checkBox_newpop.Location = new System.Drawing.Point(30, 589);
            this.checkBox_newpop.Name = "checkBox_newpop";
            this.checkBox_newpop.Size = new System.Drawing.Size(170, 21);
            this.checkBox_newpop.TabIndex = 12;
            this.checkBox_newpop.Text = "New Pop YIELD + RTB";
            this.checkBox_newpop.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label1.Location = new System.Drawing.Point(299, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 20);
            this.label1.TabIndex = 13;
            this.label1.Text = "Advertisers";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label2.Location = new System.Drawing.Point(27, 509);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(118, 20);
            this.label2.TabIndex = 14;
            this.label2.Text = "Data to retrieve";
            // 
            // listView1
            // 
            this.listView1.Location = new System.Drawing.Point(303, 121);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(150, 668);
            this.listView1.TabIndex = 7;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            this.listView1.Click += new System.EventHandler(this.listView1_Click);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(32, 439);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(209, 23);
            this.progressBar.TabIndex = 15;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(507, 850);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkBox_newpop);
            this.Controls.Add(this.checkBox_lightbox);
            this.Controls.Add(this.checkBox_banner);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.browse_btn);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.to_date);
            this.Controls.Add(this.from_date);
            this.Controls.Add(this.dateTimePicker2);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.run_btn);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button run_btn;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.Label to_date;
        private System.Windows.Forms.Button browse_btn;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.CheckBox checkBox_banner;
        private System.Windows.Forms.CheckBox checkBox_lightbox;
        private System.Windows.Forms.CheckBox checkBox_newpop;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListView listView1;
        protected System.Windows.Forms.Label from_date;
        private System.Windows.Forms.ProgressBar progressBar;
    }
}