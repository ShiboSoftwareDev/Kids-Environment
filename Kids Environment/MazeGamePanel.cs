using System;
using System.Drawing;
using System.Windows.Forms;

namespace KidsGameEnvironment
{
    public class MazeGamePanel : Panel
    {
        private int[,] maze;
        private Point playerPosition;
        private const int cellSize = 20;

        public MazeGamePanel()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.White;
            this.KeyDown += MazeGamePanel_KeyDown;
            this.PreviewKeyDown += MazeGamePanel_PreviewKeyDown;
            InitializeMaze();
        }

        private void InitializeMaze()
        {
            // Simple maze for demonstration
            maze = new int[,]
            {
                { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                { 1, 0, 0, 0, 1, 0, 0, 0, 0, 1 },
                { 1, 0, 1, 0, 1, 0, 1, 1, 0, 1 },
                { 1, 0, 1, 0, 0, 0, 0, 1, 0, 1 },
                { 1, 0, 1, 1, 1, 1, 0, 1, 0, 1 },
                { 1, 0, 0, 0, 0, 1, 0, 0, 0, 1 },
                { 1, 1, 1, 1, 0, 1, 1, 1, 1, 1 },
                { 1, 0, 0, 1, 0, 0, 0, 0, 0, 1 },
                { 1, 0, 1, 1, 1, 1, 1, 1, 0, 1 },
                { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }
            };

            playerPosition = new Point(1, 1);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;

            for (int y = 0; y < maze.GetLength(0); y++)
            {
                for (int x = 0; x < maze.GetLength(1); x++)
                {
                    if (maze[y, x] == 1)
                    {
                        g.FillRectangle(Brushes.Black, x * cellSize, y * cellSize, cellSize, cellSize);
                    }
                }
            }

            g.FillRectangle(Brushes.Blue, playerPosition.X * cellSize, playerPosition.Y * cellSize, cellSize, cellSize);
        }

        private void MazeGamePanel_KeyDown(object sender, KeyEventArgs e)
        {
            Point newPosition = playerPosition;

            switch (e.KeyCode)
            {
                case Keys.Up:
                    newPosition.Y--;
                    break;
                case Keys.Down:
                    newPosition.Y++;
                    break;
                case Keys.Left:
                    newPosition.X--;
                    break;
                case Keys.Right:
                    newPosition.X++;
                    break;
            }

            if (maze[newPosition.Y, newPosition.X] == 0)
            {
                playerPosition = newPosition;
                Invalidate();
            }
        }

        private void MazeGamePanel_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
            {
                e.IsInputKey = true;
            }
        }
    }
}
