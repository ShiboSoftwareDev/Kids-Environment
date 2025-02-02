using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace KidsGameEnvironment
{
    public class MazeGame : Control
    {
        private int mazeRows = 10;
        private int mazeCols = 10;
        private MazeCell[,] maze;
        private int cellSize;
        private Point playerPosition;
        private Point exitPosition;
        private Random random = new Random();

        private Label resultLabel;
        private Button restartButton;

        public MazeGame()
        {
            this.Dock = DockStyle.Fill;
            this.DoubleBuffered = true;
            this.BackColor = Color.White;
            this.Resize += (s, e) => { Invalidate(); };

            GenerateMaze();
            InitializeUI();



            playerPosition = new Point(0, 0);
            exitPosition = new Point(mazeCols - 1, mazeRows - 1);

            this.KeyDown += Game3Control_KeyDown;
            this.SetStyle(ControlStyles.Selectable, true);
            this.TabStop = true;
        }

        private void InitializeUI()
        {
            resultLabel = new Label
            {
                Size = new Size(500, 50),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Arial", 14, FontStyle.Bold),
                BackColor = Color.Transparent,
                Visible = false
            };
            Controls.Add(resultLabel);

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
        }

        protected override bool IsInputKey(Keys keyData)
        {
            if (keyData == Keys.Left || keyData == Keys.Right ||
                keyData == Keys.Up || keyData == Keys.Down)
            {
                return true;
            }
            return base.IsInputKey(keyData);
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (this.Visible)
            {
                this.Focus();
            }
        }

        private void GenerateMaze()
        {
            maze = new MazeCell[mazeRows, mazeCols];
            for (int r = 0; r < mazeRows; r++)
            {
                for (int c = 0; c < mazeCols; c++)
                {
                    maze[r, c] = new MazeCell();
                }
            }
            // Generate maze using DFS recursive backtracker
            Stack<Point> stack = new Stack<Point>();
            bool[,] visited = new bool[mazeRows, mazeCols];
            Point current = new Point(0, 0);
            visited[0, 0] = true;
            do
            {
                var neighbors = new List<(Point pt, string dir)>();
                // Check up/down/left/right
                if (current.Y > 0 && !visited[current.Y - 1, current.X])
                    neighbors.Add((new Point(current.X, current.Y - 1), "N"));
                if (current.Y < mazeRows - 1 && !visited[current.Y + 1, current.X])
                    neighbors.Add((new Point(current.X, current.Y + 1), "S"));
                if (current.X > 0 && !visited[current.Y, current.X - 1])
                    neighbors.Add((new Point(current.X - 1, current.Y), "W"));
                if (current.X < mazeCols - 1 && !visited[current.Y, current.X + 1])
                    neighbors.Add((new Point(current.X + 1, current.Y), "E"));

                if (neighbors.Any())
                {
                    var next = neighbors[random.Next(neighbors.Count)];
                    // Remove wall between current and next
                    if (next.dir == "N")
                    {
                        maze[current.Y, current.X].North = false;
                        maze[next.pt.Y, next.pt.X].South = false;
                    }
                    else if (next.dir == "S")
                    {
                        maze[current.Y, current.X].South = false;
                        maze[next.pt.Y, next.pt.X].North = false;
                    }
                    else if (next.dir == "W")
                    {
                        maze[current.Y, current.X].West = false;
                        maze[next.pt.Y, next.pt.X].East = false;
                    }
                    else if (next.dir == "E")
                    {
                        maze[current.Y, current.X].East = false;
                        maze[next.pt.Y, next.pt.X].West = false;
                    }
                    stack.Push(current);
                    current = next.pt;
                    visited[current.Y, current.X] = true;
                }
                else if (stack.Count > 0)
                {
                    current = stack.Pop();
                }
                else
                {
                    break;
                }
            } while (true);
        }

        private void Game3Control_KeyDown(object sender, KeyEventArgs e)
        {
            int newX = playerPosition.X;
            int newY = playerPosition.Y;

            if (e.KeyCode == Keys.Up)
            {
                if (!maze[playerPosition.Y, playerPosition.X].North)
                    newY--;
            }
            else if (e.KeyCode == Keys.Down)
            {
                if (!maze[playerPosition.Y, playerPosition.X].South)
                    newY++;
            }
            else if (e.KeyCode == Keys.Left)
            {
                if (!maze[playerPosition.Y, playerPosition.X].West)
                    newX--;
            }
            else if (e.KeyCode == Keys.Right)
            {
                if (!maze[playerPosition.Y, playerPosition.X].East)
                    newX++;
            }

            if (newX >= 0 && newX < mazeCols && newY >= 0 && newY < mazeRows)
            {
                playerPosition = new Point(newX, newY);
                Invalidate();

                if (playerPosition == exitPosition)
                {
                    ShowWinningMessage();

                }
            }
        }
        private void ShowWinningMessage()
        {
            resultLabel.Text = "Congratulations! You've escaped the maze!";
            resultLabel.ForeColor = Color.Green;
            resultLabel.BackColor = Color.White;
            resultLabel.Location = new Point((Width - resultLabel.Width) / 2, (Height - resultLabel.Height) / 2);
            resultLabel.Visible = true;

            restartButton.Location = new Point((Width - restartButton.Width) / 2, resultLabel.Bottom + 10);
            restartButton.Visible = true;
        }

        private void RestartButton_Click(object sender, EventArgs e)
        {
            GenerateMaze();
            playerPosition = new Point(0, 0);
            resultLabel.Visible = false;
            restartButton.Visible = false;
            Invalidate();
        }

protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (maze == null) return;

            cellSize = Math.Min(this.Width / mazeCols, this.Height / mazeRows);
            int offsetX = (this.Width - (mazeCols * cellSize)) / 2;
            int offsetY = (this.Height - (mazeRows * cellSize)) / 2;

            using (Pen wallPen = new Pen(Color.Black, 2))
            {
                // Draw maze cells
                for (int r = 0; r < mazeRows; r++)
                {
                    for (int c = 0; c < mazeCols; c++)
                    {
                        int x = offsetX + c * cellSize;
                        int y = offsetY + r * cellSize;

                        // If wall exists, draw it.
                        if (maze[r, c].North)
                            e.Graphics.DrawLine(wallPen, x, y, x + cellSize, y);
                        if (maze[r, c].West)
                            e.Graphics.DrawLine(wallPen, x, y, x, y + cellSize);
                        if (maze[r, c].South)
                            e.Graphics.DrawLine(wallPen, x, y + cellSize, x + cellSize, y + cellSize);
                        if (maze[r, c].East)
                            e.Graphics.DrawLine(wallPen, x + cellSize, y, x + cellSize, y + cellSize);
                    }
                }
            }

            // Highlight exit cell
            int exitX = offsetX + exitPosition.X * cellSize;
            int exitY = offsetY + exitPosition.Y * cellSize;
            e.Graphics.FillRectangle(Brushes.LightGreen, exitX + 4, exitY + 4,
                cellSize - 8, cellSize - 8);

            // Draw player (circle)
            int playerX = offsetX + playerPosition.X * cellSize;
            int playerY = offsetY + playerPosition.Y * cellSize;
            e.Graphics.FillEllipse(Brushes.Blue, playerX + 4, playerY + 4,
                cellSize - 8, cellSize - 8);
        }

        private class MazeCell
        {
            public bool North { get; set; } = true;
            public bool South { get; set; } = true;
            public bool East { get; set; } = true;
            public bool West { get; set; } = true;
        }
    }
}
