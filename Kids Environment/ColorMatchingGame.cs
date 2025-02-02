using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace KidsGameEnvironment
{
    public class ColorMatchingGame : Panel
    {
        private TableLayoutPanel grid;
        private Button firstClicked;
        private Button secondClicked;
        private Dictionary<Button, Color> cardColors;
        private List<Color> colors;
        private int gridSize = 4;

        private Label resultLabel;
        private Button restartButton;

        public ColorMatchingGame()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.DarkGreen;
            this.Padding = new Padding(20, 70, 20, 20);
            InitializeGame();
            InitializeUI();
        }

        private void InitializeGame()
        {

            colors = new List<Color>
            {
                Color.Red, Color.Blue, Color.Yellow, Color.Purple,
                Color.Orange, Color.Cyan, Color.Magenta, Color.Lime
            };

            List<Color> cardList = colors.Concat(colors).ToList();
            Shuffle(cardList);

            grid = new TableLayoutPanel
            {
                RowCount = gridSize,
                ColumnCount = gridSize,
                Dock = DockStyle.Fill,
                BackColor = Color.DarkGreen,
            };

            for (int i = 0; i < gridSize; i++)
            {
                grid.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / gridSize));
                grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / gridSize));
            }

            cardColors = new Dictionary<Button, Color>();

            for (int i = 0; i < gridSize * gridSize; i++)
            {
                Button card = new Button
                {
                    Dock = DockStyle.Fill,
                    BackColor = Color.Gray,
                    Tag = i,
                    Font = new Font(FontFamily.GenericSansSerif, 24, FontStyle.Bold)
                };

                cardColors[card] = cardList[i];
                card.Click += Card_Click;
                grid.Controls.Add(card);
            }

            this.Controls.Add(grid);
        }

        private void InitializeUI()
        {
            resultLabel = new Label
            {
                Size = new Size(500, 50),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Arial", 14, FontStyle.Bold),
                BackColor = Color.Transparent,
                ForeColor = Color.White,
                Visible = false
            };
            Controls.Add(resultLabel);
            resultLabel.BringToFront();

            restartButton = new Button
            {
                Text = "Play Again",
                Size = new Size(150, 40),
                Font = new Font("Arial", 10),
                BackColor = Color.LightGreen,
                FlatStyle = FlatStyle.Flat,
                Visible = false
            };
            restartButton.Click += RestartButton_Click;
            Controls.Add(restartButton);
            restartButton.BringToFront();
        }

        private void Card_Click(object sender, EventArgs e)
        {
            Button clicked = sender as Button;
            if (clicked == null || clicked == firstClicked ||
                clicked.BackColor != Color.Gray)
            {
                return;
            }

            clicked.BackColor = cardColors[clicked];
            clicked.Refresh();

            if (firstClicked == null)
            {
                firstClicked = clicked;
                return;
            }
            else
            {
                secondClicked = clicked;
            }

            foreach (Button button in grid.Controls.OfType<Button>())
            {
                button.Enabled = false;
            }

            Timer delayTimer = new Timer { Interval = 500 };
            delayTimer.Tick += (s, args) =>
            {
                delayTimer.Stop();
                if (firstClicked != null && secondClicked != null && cardColors[firstClicked] != cardColors[secondClicked])
                {
                    firstClicked.BackColor = Color.Gray;
                    secondClicked.BackColor = Color.Gray;
                }
                else if (firstClicked != null && secondClicked != null)
                {

                    firstClicked.Enabled = false;
                    secondClicked.Enabled = false;
                }

                firstClicked = null;
                secondClicked = null;

                foreach (Button button in grid.Controls.OfType<Button>().Where(b => b.BackColor == Color.Gray))
                {
                    button.Enabled = true;
                }

                if (grid.Controls.OfType<Button>().All(b => b.BackColor != Color.Gray))
                {
                    ShowWinningMessage();
                }
            };
            delayTimer.Start();
        }

        private void ShowWinningMessage()
        {
            resultLabel.Text = "Congratulations! You've matched all the colors!";
            resultLabel.ForeColor = Color.Green;
            resultLabel.BackColor = Color.White;
            resultLabel.Location = new Point((Width - resultLabel.Width) / 2, (Height - resultLabel.Height) / 2);
            resultLabel.Visible = true;
            resultLabel.BringToFront();

            restartButton.Location = new Point((Width - restartButton.Width) / 2, resultLabel.Bottom + 10);
            restartButton.Visible = true;
            restartButton.BringToFront();
        }

        private void RestartButton_Click(object sender, EventArgs e)
        {
            this.Controls.Remove(grid);
            InitializeGame();
            resultLabel.Visible = false;
            restartButton.Visible = false;
        }

        private void Shuffle<T>(IList<T> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
