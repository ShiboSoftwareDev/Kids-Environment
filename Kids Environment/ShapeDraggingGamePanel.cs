using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace KidsGameEnvironment
{
    public class ShapeDraggingGamePanel : Panel
    {
        private enum ShapeType { Square, Circle, Triangle, Hexagon }

        private ShapeType[] shapeTypes;
        private Rectangle[] shapes;
        private Rectangle[] targets;
        private bool[] isShapePlaced;
        private int selectedShapeIndex = -1;
        private Point offset;
        private Timer gameTimer;
        private int timeLeft;
        private Button startButton;
        private Button restartButton;
        private Label timerLabel;
        private bool gameStarted = false;

        private readonly Color shapeColor = Color.CornflowerBlue;
        private readonly Color targetColor = Color.LightGray;

        public ShapeDraggingGamePanel()
        {
            this.DoubleBuffered = true; // Smooth UI rendering
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.White;
            this.MouseDown += ShapeDraggingGamePanel_MouseDown;
            this.MouseMove += ShapeDraggingGamePanel_MouseMove;
            this.MouseUp += ShapeDraggingGamePanel_MouseUp;
            this.Resize += ShapeDraggingGamePanel_Resize;

            InitializeShapes();
            InitializeGame();
        }

        private void InitializeShapes()
        {
            int shapeSize = Math.Min(this.Width, this.Height) / 10; // Base size
            int targetSize = shapeSize + 10; // Targets are slightly larger
            int margin = shapeSize / 2;

            int totalWidth = 4 * (shapeSize + 20) - 20;
            int startX = (this.Width - totalWidth) / 2;
            int startY = (this.Height - shapeSize - targetSize - 40) / 2;

            Random rand = new Random();
            shapes = new Rectangle[]
            {
                new Rectangle(rand.Next(margin, this.Width - shapeSize - margin), rand.Next(margin, this.Height / 2 - shapeSize - margin), shapeSize, shapeSize),
                new Rectangle(rand.Next(margin, this.Width - shapeSize - margin), rand.Next(margin, this.Height / 2 - shapeSize - margin), shapeSize, shapeSize),
                new Rectangle(rand.Next(margin, this.Width - shapeSize - margin), rand.Next(margin, this.Height / 2 - shapeSize - margin), shapeSize, shapeSize),
                new Rectangle(rand.Next(margin, this.Width - shapeSize - margin), rand.Next(margin, this.Height / 2 - shapeSize - margin), shapeSize, shapeSize)
            };

            targets = new Rectangle[]
            {
                new Rectangle(startX, startY + shapeSize + 20, targetSize, targetSize),
                new Rectangle(startX + targetSize + 20, startY + shapeSize + 20, targetSize, targetSize),
                new Rectangle(startX + 2 * (targetSize + 20), startY + shapeSize + 20, targetSize, targetSize),
                new Rectangle(startX + 3 * (targetSize + 20), startY + shapeSize + 20, targetSize, targetSize)
            };

            shapeTypes = new ShapeType[4];
            for (int i = 0; i < shapeTypes.Length; i++)
            {
                shapeTypes[i] = (ShapeType)rand.Next(0, 4);
            }

            isShapePlaced = new bool[shapes.Length];
            Invalidate();
        }

        private void InitializeGame()
        {
            startButton = new Button
            {
                Text = "Start",
                Size = new Size(200, 100),
                Location = new Point((this.Width - 200) / 2, (this.Height - 100) / 2)
            };
            startButton.Click += StartButton_Click;
            this.Controls.Add(startButton);

            timerLabel = new Label
            {
                Text = "Time: 5",
                Size = new Size(200, 50),
                Location = new Point((this.Width - 200) / 2, this.Height - 150),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Arial", 16)
            };
            this.Controls.Add(timerLabel);
            timerLabel.Visible = false;

            restartButton = new Button
            {
                Text = "Restart",
                Size = new Size(150, 50),
                Location = new Point((this.Width - 150) / 2, this.Height - 100)
            };
            restartButton.Click += RestartButton_Click;
            this.Controls.Add(restartButton);
            restartButton.Visible = false;

            gameTimer = new Timer();
            gameTimer.Interval = 1000; // 1 second
            gameTimer.Tick += GameTimer_Tick;
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            startButton.Visible = false;
            timerLabel.Visible = true;
            restartButton.Visible = true;
            timeLeft = 10;
            timerLabel.Text = "Time: " + timeLeft;
            gameStarted = true;
            gameTimer.Start();
            Invalidate(); // Redraw to show shapes
        }

        private void RestartButton_Click(object sender, EventArgs e)
        {
            ResetGame();
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            timeLeft--;
            timerLabel.Text = "Time: " + timeLeft;
            if (timeLeft <= 0)
            {
                gameTimer.Stop();
                CheckGameOver();
            }
        }

        private void CheckGameOver()
        {
            if (Array.TrueForAll(isShapePlaced, placed => placed))
            {
                MessageBox.Show("Congratulations! All shapes placed correctly!", "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Time's up! You lost.", "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            ResetGame();
        }

        private void ResetGame()
        {
            InitializeShapes();
            startButton.Visible = true;
            timerLabel.Visible = false;
            restartButton.Visible = false;
            gameStarted = false;
        }

        private void ShapeDraggingGamePanel_Resize(object sender, EventArgs e)
        {
            InitializeShapes(); // Recalculate positions and sizes on resize
            startButton.Location = new Point((this.Width - 200) / 2, (this.Height - 100) / 2);
            timerLabel.Location = new Point((this.Width - 200) / 2, this.Height - 150);
            restartButton.Location = new Point((this.Width - 150) / 2, this.Height - 100);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;

            // Enable anti-aliasing for smoother shapes
            g.SmoothingMode = SmoothingMode.AntiAlias;

            if (gameStarted)
            {
                // Draw targets
                using (Brush targetBrush = new SolidBrush(targetColor))
                using (Pen targetPen = new Pen(Color.Black, 2))
                {
                    for (int i = 0; i < targets.Length; i++)
                    {
                        DrawShape(g, targetBrush, targetPen, shapeTypes[i], targets[i]);
                    }
                }

                // Draw shapes
                using (Brush shapeBrush = new SolidBrush(shapeColor))
                {
                    for (int i = 0; i < shapes.Length; i++)
                    {
                        if (!isShapePlaced[i])
                        {
                            DrawShape(g, shapeBrush, Pens.Black, shapeTypes[i], shapes[i]);
                        }
                    }
                }
            }
        }

        private void DrawShape(Graphics g, Brush brush, Pen pen, ShapeType shapeType, Rectangle bounds)
        {
            switch (shapeType)
            {
                case ShapeType.Square:
                    g.FillRectangle(brush, bounds);
                    g.DrawRectangle(pen, bounds);
                    break;
                case ShapeType.Circle:
                    g.FillEllipse(brush, bounds);
                    g.DrawEllipse(pen, bounds);
                    break;
                case ShapeType.Triangle:
                    PointF[] trianglePoints = GetTrianglePoints(bounds);
                    g.FillPolygon(brush, trianglePoints);
                    g.DrawPolygon(pen, trianglePoints);
                    break;
                case ShapeType.Hexagon:
                    PointF[] hexagonPoints = GetHexagonPoints(bounds);
                    g.FillPolygon(brush, hexagonPoints);
                    g.DrawPolygon(pen, hexagonPoints);
                    break;
            }
        }

        private PointF[] GetTrianglePoints(Rectangle bounds)
        {
            return new PointF[]
            {
                new PointF(bounds.Left + bounds.Width / 2, bounds.Top), // Top vertex
                new PointF(bounds.Left, bounds.Bottom), // Bottom-left vertex
                new PointF(bounds.Right, bounds.Bottom) // Bottom-right vertex
            };
        }

        private PointF[] GetHexagonPoints(Rectangle bounds)
        {
            float width = bounds.Width;
            float height = bounds.Height;
            float x = bounds.Left;
            float y = bounds.Top;

            return new PointF[]
            {
                new PointF(x + width * 0.25f, y),
                new PointF(x + width * 0.75f, y),
                new PointF(x + width, y + height * 0.5f),
                new PointF(x + width * 0.75f, y + height),
                new PointF(x + width * 0.25f, y + height),
                new PointF(x, y + height * 0.5f)
            };
        }

        private void ShapeDraggingGamePanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (!gameStarted) return;

            for (int i = 0; i < shapes.Length; i++)
            {
                if (shapes[i].Contains(e.Location) && !isShapePlaced[i])
                {
                    selectedShapeIndex = i;
                    offset = new Point(e.X - shapes[i].X, e.Y - shapes[i].Y);
                    this.Cursor = Cursors.Hand; // Visual feedback for dragging
                    break;
                }
            }
        }

        private void ShapeDraggingGamePanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (selectedShapeIndex != -1)
            {
                shapes[selectedShapeIndex] = new Rectangle(e.X - offset.X, e.Y - offset.Y, shapes[selectedShapeIndex].Width, shapes[selectedShapeIndex].Height);
                Invalidate();
            }
        }

        private void ShapeDraggingGamePanel_MouseUp(object sender, MouseEventArgs e)
        {
            if (selectedShapeIndex != -1)
            {
                for (int i = 0; i < targets.Length; i++)
                {
                    if (targets[i].Contains(shapes[selectedShapeIndex].Location))
                    {
                        shapes[selectedShapeIndex] = targets[i];
                        isShapePlaced[selectedShapeIndex] = true;
                        break;
                    }
                }

                selectedShapeIndex = -1;
                this.Cursor = Cursors.Default; // Reset cursor
                Invalidate();

                // Check for game completion
                if (Array.TrueForAll(isShapePlaced, placed => placed))
                {
                    gameTimer.Stop();
                    MessageBox.Show("Congratulations! All shapes placed correctly!", "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ResetGame();
                }
            }
        }
    }
}
