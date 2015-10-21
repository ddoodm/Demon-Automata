namespace NetAppsAssignmentTwo
{
    partial class MainForm
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
                backBuffer.Dispose();
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
            this.panel_display = new System.Windows.Forms.Panel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.btn_reset = new System.Windows.Forms.Button();
            this.btn_start = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_seed = new System.Windows.Forms.TextBox();
            this.tb_gens = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.combo_rules = new System.Windows.Forms.ComboBox();
            this.combo_colours = new System.Windows.Forms.ComboBox();
            this.panel_controls = new System.Windows.Forms.Panel();
            this.status_generation = new System.Windows.Forms.ToolStripStatusLabel();
            this.status_hash = new System.Windows.Forms.ToolStripStatusLabel();
            this.progress_generations = new System.Windows.Forms.ToolStripProgressBar();
            this.statusStrip1.SuspendLayout();
            this.panel_controls.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel_display
            // 
            this.panel_display.Location = new System.Drawing.Point(1, 80);
            this.panel_display.Name = "panel_display";
            this.panel_display.Size = new System.Drawing.Size(640, 480);
            this.panel_display.TabIndex = 0;
            this.panel_display.Paint += new System.Windows.Forms.PaintEventHandler(this.panel_display_Paint);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.progress_generations,
            this.status_generation,
            this.status_hash});
            this.statusStrip1.Location = new System.Drawing.Point(0, 567);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(642, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // btn_reset
            // 
            this.btn_reset.Location = new System.Drawing.Point(553, 10);
            this.btn_reset.Name = "btn_reset";
            this.btn_reset.Size = new System.Drawing.Size(75, 23);
            this.btn_reset.TabIndex = 2;
            this.btn_reset.Text = "Reset";
            this.btn_reset.UseVisualStyleBackColor = true;
            this.btn_reset.Click += new System.EventHandler(this.btn_reset_Click);
            // 
            // btn_start
            // 
            this.btn_start.Location = new System.Drawing.Point(553, 39);
            this.btn_start.Name = "btn_start";
            this.btn_start.Size = new System.Drawing.Size(75, 23);
            this.btn_start.TabIndex = 3;
            this.btn_start.Text = "Start";
            this.btn_start.UseVisualStyleBackColor = true;
            this.btn_start.Click += new System.EventHandler(this.btn_start_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Seed";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Gens";
            // 
            // tb_seed
            // 
            this.tb_seed.Location = new System.Drawing.Point(48, 12);
            this.tb_seed.Name = "tb_seed";
            this.tb_seed.Size = new System.Drawing.Size(100, 20);
            this.tb_seed.TabIndex = 6;
            // 
            // tb_gens
            // 
            this.tb_gens.Location = new System.Drawing.Point(48, 41);
            this.tb_gens.Name = "tb_gens";
            this.tb_gens.Size = new System.Drawing.Size(100, 20);
            this.tb_gens.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(217, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Rules";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(217, 44);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Colors";
            // 
            // combo_rules
            // 
            this.combo_rules.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_rules.FormattingEnabled = true;
            this.combo_rules.Location = new System.Drawing.Point(259, 12);
            this.combo_rules.Name = "combo_rules";
            this.combo_rules.Size = new System.Drawing.Size(121, 21);
            this.combo_rules.TabIndex = 10;
            // 
            // combo_colours
            // 
            this.combo_colours.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_colours.FormattingEnabled = true;
            this.combo_colours.Location = new System.Drawing.Point(259, 41);
            this.combo_colours.Name = "combo_colours";
            this.combo_colours.Size = new System.Drawing.Size(121, 21);
            this.combo_colours.TabIndex = 11;
            this.combo_colours.SelectionChangeCommitted += new System.EventHandler(this.combo_colours_SelectionChangeCommitted);
            // 
            // panel_controls
            // 
            this.panel_controls.Controls.Add(this.tb_seed);
            this.panel_controls.Controls.Add(this.combo_colours);
            this.panel_controls.Controls.Add(this.btn_reset);
            this.panel_controls.Controls.Add(this.combo_rules);
            this.panel_controls.Controls.Add(this.btn_start);
            this.panel_controls.Controls.Add(this.label4);
            this.panel_controls.Controls.Add(this.label1);
            this.panel_controls.Controls.Add(this.label3);
            this.panel_controls.Controls.Add(this.label2);
            this.panel_controls.Controls.Add(this.tb_gens);
            this.panel_controls.Location = new System.Drawing.Point(0, 0);
            this.panel_controls.Name = "panel_controls";
            this.panel_controls.Size = new System.Drawing.Size(641, 74);
            this.panel_controls.TabIndex = 12;
            // 
            // status_generation
            // 
            this.status_generation.Name = "status_generation";
            this.status_generation.Size = new System.Drawing.Size(13, 17);
            this.status_generation.Text = "0";
            // 
            // status_hash
            // 
            this.status_hash.Name = "status_hash";
            this.status_hash.Size = new System.Drawing.Size(118, 17);
            this.status_hash.Text = "toolStripStatusLabel1";
            // 
            // progress_generations
            // 
            this.progress_generations.Name = "progress_generations";
            this.progress_generations.Size = new System.Drawing.Size(100, 16);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(642, 589);
            this.Controls.Add(this.panel_controls);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.panel_display);
            this.DoubleBuffered = true;
            this.Name = "MainForm";
            this.Text = "Demon Simulator - Deinyon L Davies (11688025) 2015";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel_controls.ResumeLayout(false);
            this.panel_controls.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel_display;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Button btn_reset;
        private System.Windows.Forms.Button btn_start;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb_seed;
        private System.Windows.Forms.TextBox tb_gens;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox combo_rules;
        private System.Windows.Forms.ComboBox combo_colours;
        private System.Windows.Forms.Panel panel_controls;
        private System.Windows.Forms.ToolStripProgressBar progress_generations;
        private System.Windows.Forms.ToolStripStatusLabel status_generation;
        private System.Windows.Forms.ToolStripStatusLabel status_hash;

    }
}

