using System;
using System.Windows.Forms;

namespace KidsGameEnvironment
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.btnGame1 = new System.Windows.Forms.Button();
            this.btnGame2 = new System.Windows.Forms.Button();
            this.btnGame3 = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnGame1
            // 
            this.btnGame1.Location = new System.Drawing.Point(100, 200);
            this.btnGame1.Name = "btnGame1";
            this.btnGame1.Size = new System.Drawing.Size(150, 75);
            this.btnGame1.TabIndex = 0;
            this.btnGame1.Text = "Game 1";
            this.btnGame1.UseVisualStyleBackColor = true;
            // 
            // btnGame2
            // 
            this.btnGame2.Location = new System.Drawing.Point(260, 200);
            this.btnGame2.Name = "btnGame2";
            this.btnGame2.Size = new System.Drawing.Size(150, 75);
            this.btnGame2.TabIndex = 1;
            this.btnGame2.Text = "Game 2";
            this.btnGame2.UseVisualStyleBackColor = true;
            // 
            // btnGame3
            // 
            this.btnGame3.Location = new System.Drawing.Point(420, 200);
            this.btnGame3.Name = "btnGame3";
            this.btnGame3.Size = new System.Drawing.Size(150, 75);
            this.btnGame3.TabIndex = 2;
            this.btnGame3.Text = "Game 3";
            this.btnGame3.UseVisualStyleBackColor = true;
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(580, 200);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(150, 75);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.BackColor = System.Drawing.Color.Red;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnGame3);
            this.Controls.Add(this.btnGame2);
            this.Controls.Add(this.btnGame1);
            this.Name = "MainForm";
            this.Text = "Kids Game Environment";
            this.ResumeLayout(false);

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private System.Windows.Forms.Button btnGame1;
        private System.Windows.Forms.Button btnGame2;
        private System.Windows.Forms.Button btnGame3;
        private System.Windows.Forms.Button btnExit;
    }
}
