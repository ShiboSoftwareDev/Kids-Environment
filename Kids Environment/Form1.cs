using System;
using System.Windows.Forms;

namespace KidsGameEnvironment
{
    public partial class MainForm : Form
    {
        //private Panel mainPanel;
        //private Panel game1Panel;
        //private Panel game2Panel;
        //private Panel game3Panel;
        //private Button btnReturn;

        public MainForm()
        {
            Init();
        }

        private void Init()
        {
            this.btnGame1 = new System.Windows.Forms.Button();
            this.btnGame2 = new System.Windows.Forms.Button();
            this.btnGame3 = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.game1Panel = new System.Windows.Forms.Panel();
            this.game2Panel = new System.Windows.Forms.Panel();
            this.game3Panel = new System.Windows.Forms.Panel();
            this.btnReturn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnGame1
            // 
            this.btnGame1.Location = new System.Drawing.Point(175, 262);
            this.btnGame1.Name = "btnGame1";
            this.btnGame1.Size = new System.Drawing.Size(150, 75);
            this.btnGame1.TabIndex = 0;
            this.btnGame1.Text = "Game 1";
            this.btnGame1.UseVisualStyleBackColor = true;
            this.btnGame1.Click += new System.EventHandler(this.btnGame1_Click);
            // 
            // btnGame2
            // 
            this.btnGame2.Location = new System.Drawing.Point(335, 262);
            this.btnGame2.Name = "btnGame2";
            this.btnGame2.Size = new System.Drawing.Size(150, 75);
            this.btnGame2.TabIndex = 1;
            this.btnGame2.Text = "Game 2";
            this.btnGame2.UseVisualStyleBackColor = true;
            this.btnGame2.Click += new System.EventHandler(this.btnGame2_Click);
            // 
            // btnGame3
            // 
            this.btnGame3.Location = new System.Drawing.Point(495, 262);
            this.btnGame3.Name = "btnGame3";
            this.btnGame3.Size = new System.Drawing.Size(150, 75);
            this.btnGame3.TabIndex = 2;
            this.btnGame3.Text = "Game 3";
            this.btnGame3.UseVisualStyleBackColor = true;
            this.btnGame3.Click += new System.EventHandler(this.btnGame3_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(655, 262);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(150, 75);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.BackColor = System.Drawing.Color.Red;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // mainPanel
            // 
            this.mainPanel.Controls.Add(this.btnGame1);
            this.mainPanel.Controls.Add(this.btnGame2);
            this.mainPanel.Controls.Add(this.btnGame3);
            this.mainPanel.Controls.Add(this.btnExit);
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(0, 0);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(1000, 600);
            this.mainPanel.TabIndex = 4;
            // 
            // game1Panel
            // 
            this.game1Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.game1Panel.Location = new System.Drawing.Point(0, 0);
            this.game1Panel.Name = "game1Panel";
            this.game1Panel.Size = new System.Drawing.Size(1000, 600);
            this.game1Panel.TabIndex = 5;
            this.game1Panel.Visible = false;
            // 
            // game2Panel
            // 
            this.game2Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.game2Panel.Location = new System.Drawing.Point(0, 0);
            this.game2Panel.Name = "game2Panel";
            this.game2Panel.Size = new System.Drawing.Size(1000, 600);
            this.game2Panel.TabIndex = 6;
            this.game2Panel.Visible = false;
            // 
            // game3Panel
            // 
            this.game3Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.game3Panel.Location = new System.Drawing.Point(0, 0);
            this.game3Panel.Name = "game3Panel";
            this.game3Panel.Size = new System.Drawing.Size(1000, 600);
            this.game3Panel.TabIndex = 7;
            this.game3Panel.Visible = false;
            // 
            // btnReturn
            // 
            this.btnReturn.Location = new System.Drawing.Point(12, 12);
            this.btnReturn.Name = "btnReturn";
            this.btnReturn.Size = new System.Drawing.Size(75, 23);
            this.btnReturn.TabIndex = 8;
            this.btnReturn.Text = "Return";
            this.btnReturn.UseVisualStyleBackColor = false;
            this.btnReturn.BackColor = System.Drawing.Color.Red;
            this.btnReturn.Click += new System.EventHandler(this.btnReturn_Click);
            this.btnReturn.Visible = false;
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(1000, 600);
            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this.game1Panel);
            this.Controls.Add(this.game2Panel);
            this.Controls.Add(this.game3Panel);
            this.Controls.Add(this.btnReturn);
            this.Name = "MainForm";
            this.Text = "Kids Game Environment";
            this.ResumeLayout(false);

        }

        private void btnGame1_Click(object sender, EventArgs e)
        {
            ShowPanel(game1Panel);
        }

        private void btnGame2_Click(object sender, EventArgs e)
        {
            ShowPanel(game2Panel);
        }

        private void btnGame3_Click(object sender, EventArgs e)
        {
            ShowPanel(game3Panel);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            ShowPanel(mainPanel);
        }

        private void ShowPanel(Panel panel)
        {
            mainPanel.Visible = false;
            game1Panel.Visible = false;
            game2Panel.Visible = false;
            game3Panel.Visible = false;
            btnReturn.Visible = true;

            panel.Visible = true;
        }

        private System.Windows.Forms.Button btnGame1;
        private System.Windows.Forms.Button btnGame2;
        private System.Windows.Forms.Button btnGame3;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.Panel game1Panel;
        private System.Windows.Forms.Panel game2Panel;
        private System.Windows.Forms.Panel game3Panel;
        private System.Windows.Forms.Button btnReturn;
    }
}
