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
        private int gridSize = 4; // 4x4 grid

        public ColorMatchingGame()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.DarkGreen;
            InitializeGame();
        }

        private void InitializeGame()
        {
            // Prepare colors (8 pairs for a 4x4 grid)
            colors = new List<Color>
            {
                Color.Red, Color.Blue, Color.Yellow, Color.Purple,
                Color.Orange, Color.Cyan, Color.Magenta, Color.Lime
            };

            // Duplicate colors and shuffle
            List<Color> cardList = colors.Concat(colors).ToList();
            Shuffle(cardList);

            // Create a grid of buttons
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
                // Save the card's hidden color
                cardColors[card] = cardList[i];
                card.Click += Card_Click;
                grid.Controls.Add(card);
            }

            this.Controls.Add(grid);
        }

        private void Card_Click(object sender, EventArgs e)
        {
            Button clicked = sender as Button;
            if (clicked == null || clicked == firstClicked ||
                clicked.BackColor != Color.Gray)
            {
                return;
            }

            // Reveal the card - show its hidden color
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

            // Disable all buttons temporarily
            foreach (Button button in grid.Controls.OfType<Button>())
            {
                button.Enabled = false;
            }

            // Check match after a short delay.
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
                    // Optionally disable buttons
                    firstClicked.Enabled = false;
                    secondClicked.Enabled = false;
                }

                firstClicked = null;
                secondClicked = null;

                // Re-enable all buttons that are still gray
                foreach (Button button in grid.Controls.OfType<Button>().Where(b => b.BackColor == Color.Gray))
                {
                    button.Enabled = true;
                }

                if (grid.Controls.OfType<Button>().All(b => b.BackColor != Color.Gray))
                {
                    MessageBox.Show("You win!");
                }
            };
            delayTimer.Start();
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
