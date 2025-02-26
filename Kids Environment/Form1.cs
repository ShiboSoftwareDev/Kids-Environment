﻿using System;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Windows.Forms;

namespace KidsGameEnvironment
{
    public partial class MainForm : Form
    {
        private Panel mainPanel;
        private Panel game1Panel;
        private Panel game2Panel;
        private Panel game3Panel;
        private TableLayoutPanel buttonLayoutPanel;


        public MainForm()
        {
            this.Icon = new System.Drawing.Icon(".\\joystick_game_3819.ico");

            Init();
        }

        private void Init()
        {
            this.mainPanel = new System.Windows.Forms.Panel();
            this.game1Panel = CreateGamePanel("game1Panel");
            this.game2Panel = CreateGamePanel("game2Panel");
            this.game3Panel = CreateGamePanel("game3Panel");
            this.SuspendLayout();

            
            this.buttonLayoutPanel = new TableLayoutPanel();

            this.buttonLayoutPanel.ColumnCount = 4;
            this.buttonLayoutPanel.RowCount = 1;
            this.buttonLayoutPanel.Dock = DockStyle.Fill;
            this.buttonLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            this.buttonLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            this.buttonLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            this.buttonLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            this.buttonLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            this.buttonLayoutPanel.Controls.Add(CreateButton("btnGame1", "Memory Color Matching", new EventHandler(this.btnGame1_Click), null, ".\\match.png"), 0, 0);
            this.buttonLayoutPanel.Controls.Add(CreateButton("btnGame2", "Maze", new EventHandler(this.btnGame2_Click), null, ".\\maze.png"), 1, 0);
            this.buttonLayoutPanel.Controls.Add(CreateButton("btnGame3", "Shape Dragging", new EventHandler(this.btnGame3_Click), null, ".\\drag.png"), 2, 0);
            this.buttonLayoutPanel.Controls.Add(CreateButton("btnExit", "X Exit", new EventHandler(this.btnExit_Click), System.Drawing.Color.Red), 3, 0);

            this.mainPanel.Controls.Add(this.buttonLayoutPanel); 


            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(0, 0);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(1000, 600);
            this.mainPanel.TabIndex = 4;

            this.ClientSize = new System.Drawing.Size(1000, 600);
            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this.game1Panel);
            this.Controls.Add(this.game2Panel);
            this.Controls.Add(this.game3Panel);
            this.Name = "MainForm";
            this.Text = "Kids Game Environment";
            this.ResumeLayout(false);

            this.mainPanel.Resize += (s, e) =>
            {
                this.buttonLayoutPanel.Location = new System.Drawing.Point((this.mainPanel.Width - this.buttonLayoutPanel.Width) / 2, (this.mainPanel.Height - this.buttonLayoutPanel.Height) / 2);
            };

        }

        private void btnGame1_Click(object sender, EventArgs e)
        {
            ShowPanel(game1Panel);
            AddGameControl(game1Panel, new ColorMatchingGame());
        }

        private void btnGame2_Click(object sender, EventArgs e)
        {
            ShowPanel(game2Panel);
            AddGameControl(game2Panel, new MazeGame());
        }

        private void btnGame3_Click(object sender, EventArgs e)
        {
            ShowPanel(game3Panel);
            AddGameControl(game3Panel, new ShapeDraggingGame());
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            ShowPanel(mainPanel);
            RemoveGameControls();
        }

        private void ShowPanel(Panel panel)
        {
            mainPanel.Visible = false;
            game1Panel.Visible = false;
            game2Panel.Visible = false;
            game3Panel.Visible = false;

            panel.Visible = true;
            panel.Focus();
        }

        private void AddGameControl(Panel panel, Control gameControl)
        {
            gameControl.Dock = DockStyle.Fill;
            panel.Controls.Add(gameControl);
            gameControl.Focus();
        }

        private void RemoveGameControls()
        {
            game1Panel.Controls.OfType<Control>().Where(c => c is ColorMatchingGame).ToList().ForEach(c => game1Panel.Controls.Remove(c));
            game2Panel.Controls.OfType<Control>().Where(c => c is MazeGame).ToList().ForEach(c => game2Panel.Controls.Remove(c));
            game3Panel.Controls.OfType<Control>().Where(c => c is ShapeDraggingGame).ToList().ForEach(c => game3Panel.Controls.Remove(c));
        }

        private Panel CreateGamePanel(string panelName, Panel gamePanel = null)
        {
            Panel panel = new Panel
            {
                Dock = DockStyle.Fill,
                Location = new System.Drawing.Point(0, 0),
                Name = panelName,
                Size = new System.Drawing.Size(1000, 600),
                TabIndex = 5,
                Visible = false
            };

            if (gamePanel != null)
            {
                gamePanel.Dock = DockStyle.Fill;
                panel.Controls.Add(gamePanel);
            }

            Button returnButton = new Button
            {
                Location = new System.Drawing.Point(12, 12),
                Name = "btnReturn",
                Size = new System.Drawing.Size(100, 50),
                Text = "< Return",
                UseVisualStyleBackColor = false,
                BackColor = System.Drawing.Color.Red
            };
            returnButton.Click += new System.EventHandler(this.btnReturn_Click);

            panel.Controls.Add(returnButton);
            returnButton.BringToFront(); 

            return panel;
        }

        private Button CreateButton(string name, string text, EventHandler clickEvent, System.Drawing.Color? backColor = null, string imagePath = "")
        {
            Button button = new Button
            {
                Name = name,
                Size = new System.Drawing.Size(200, 150),
                Text = text,
                UseVisualStyleBackColor = true,
                Anchor = AnchorStyles.None,
                Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))) 
            };

            if (backColor.HasValue)
            {
                button.BackColor = backColor.Value;
            }

            if (!string.IsNullOrEmpty(imagePath))
            {
                button.BackgroundImage = System.Drawing.Image.FromFile(imagePath);
                button.BackgroundImageLayout = ImageLayout.Stretch;
            }

            button.Click += clickEvent;

            return button;
        }

    }
}
