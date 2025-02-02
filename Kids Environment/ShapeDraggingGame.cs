using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace KidsGameEnvironment
{
    public class ShapeDraggingGame : Panel
    {
        private enum ShapeType { Square, Circle, Triangle, Hexagon }
        private const int NumberOfShapes = 4;
        private const int InitialTime = 30;
        private const int BaseShapeSize = 60;

        private ShapeType[] shapeTypes;
        private Rectangle[] shapes;
        private Rectangle[] targets;
        private Rectangle[] originalShapes;
        private Color[] shapeColors;
        private bool[] isShapePlaced;
        private bool[] isTargetOccupied;
        private int selectedShapeIndex = -1;
        private Point offset;
        private Timer gameTimer;
        private int timeLeft;
        private Button startButton;
        private Button restartButton;
        private Label timerLabel;
        private Label titleLabel;
        private Label resultLabel;
        private bool gameStarted = false;
        private bool gameWon = false;

        private readonly Color targetColor = Color.White;
        private readonly Color placedShapeColor = Color.LightGreen;

        public ShapeDraggingGame()
        {
            DoubleBuffered = true;
            Dock = DockStyle.Fill;
            BackColor = Color.White;
            MouseDown += ShapeDraggingGamePanel_MouseDown;
            MouseMove += ShapeDraggingGamePanel_MouseMove;
            MouseUp += ShapeDraggingGamePanel_MouseUp;
            Resize += ShapeDraggingGamePanel_Resize;

            InitializeGame();
            InitializeShapes();
        }

        private void InitializeShapes()
        {
            int shapeSize = BaseShapeSize;
            int targetSize = shapeSize + 15;
            int margin = shapeSize / 2;

            Random rand = new Random();
            shapes = new Rectangle[NumberOfShapes];
            originalShapes = new Rectangle[NumberOfShapes];

            for (int i = 0; i < NumberOfShapes; i++)
            {
                bool positionValid;
                int attempts = 0;
                do
                {
                    positionValid = true;
                    int x = rand.Next(margin, Width - shapeSize - margin);
                    int minY = Math.Min(Height / 2 - shapeSize - margin, margin);
                    int maxY = Math.Max(Height / 2 - shapeSize - margin, margin);
                    int y = rand.Next(minY, maxY);
                    shapes[i] = new Rectangle(x, y, shapeSize, shapeSize);

                    for (int j = 0; j < i; j++)
                    {
                        if (shapes[i].IntersectsWith(shapes[j]))
                        {
                            positionValid = false;
                            break;
                        }
                    }

                    attempts++;
                } while (!positionValid && attempts < 100);

                originalShapes[i] = shapes[i];
            }

            int startX = (Width - (NumberOfShapes * targetSize + (NumberOfShapes - 1) * 20)) / 2;
            int startY = Height - 200;

            targets = new Rectangle[NumberOfShapes];
            for (int i = 0; i < NumberOfShapes; i++)
            {
                targets[i] = new Rectangle(
                    startX + i * (targetSize + 20),
                    startY,
                    targetSize,
                    targetSize
                );
            }

            shapeTypes = new ShapeType[NumberOfShapes];
            shapeColors = new Color[NumberOfShapes];
            for (int i = 0; i < NumberOfShapes; i++)
            {
                shapeTypes[i] = (ShapeType)rand.Next(0, 4);
                shapeColors[i] = Color.FromArgb(rand.Next(150, 256), rand.Next(150, 256), rand.Next(150, 256));
            }

            isShapePlaced = new bool[NumberOfShapes];
            isTargetOccupied = new bool[NumberOfShapes];
            Invalidate();
        }


        private void InitializeGame()
        {
            titleLabel = new Label
            {
                Text = "Drag and drop the shapes into the matching holes!",
                Size = new Size(500, 50),
                Location = new Point((Width - 500) / 2, 20),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Arial", 14, FontStyle.Bold),
                ForeColor = Color.DarkBlue
            };
            Controls.Add(titleLabel);

            startButton = new Button
            {
                Text = "Start Game",
                Size = new Size(200, 60),
                Location = new Point((Width - 200) / 2, 150),
                Font = new Font("Arial", 12),
                BackColor = Color.LightSkyBlue,
                FlatStyle = FlatStyle.Flat
            };
            startButton.Click += StartButton_Click;
            Controls.Add(startButton);

            timerLabel = new Label
            {
                Text = $"Time: {InitialTime}",
                Size = new Size(200, 40),
                Location = new Point((Width - 200) / 2, Height - 100),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Arial", 14),
                ForeColor = Color.DarkBlue
            };
            Controls.Add(timerLabel);
            timerLabel.Visible = false;

            restartButton = new Button
            {
                Text = "Play Again",
                Size = new Size(150, 40),
                Location = new Point((Width - 150) / 2, Height - 60),
                Font = new Font("Arial", 10),
                BackColor = Color.LightGreen,
                FlatStyle = FlatStyle.Flat
            };
            restartButton.Click += RestartButton_Click;
            Controls.Add(restartButton);
            restartButton.Visible = false;

            resultLabel = new Label
            {
                Size = new Size(500, 50),
                Location = new Point((Width - 500) / 2, 270),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Arial", 14, FontStyle.Bold)
            };
            Controls.Add(resultLabel);
            resultLabel.Visible = false;

            gameTimer = new Timer { Interval = 1000 };
            gameTimer.Tick += GameTimer_Tick;
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            titleLabel.Visible = false;
            startButton.Visible = false;
            timerLabel.Visible = true;
            restartButton.Visible = false;
            resultLabel.Visible = false;
            timeLeft = InitialTime;
            timerLabel.Text = $"Time: {timeLeft}";
            gameStarted = true;
            gameWon = false;
            gameTimer.Start();
            Invalidate();
        }

        private void RestartButton_Click(object sender, EventArgs e)
        {
            ResetGame();
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            timeLeft--;
            timerLabel.Text = $"Time: {timeLeft}";

            if (timeLeft <= 0)
            {
                gameTimer.Stop();
                CheckGameOver();
            }
        }

        private void CheckGameOver()
        {
            bool allPlaced = Array.TrueForAll(isShapePlaced, placed => placed);
            resultLabel.Text = allPlaced
                ? "Congratulations! All shapes placed correctly!"
                : "Time's up! Try again!";
            resultLabel.ForeColor = allPlaced ? Color.Green : Color.Red;
            resultLabel.Visible = true;
            restartButton.Visible = true;
            gameWon = allPlaced;
        }

        private void ResetGame()
        {
            InitializeShapes();
            Array.Clear(isShapePlaced, 0, isShapePlaced.Length);
            Array.Clear(isTargetOccupied, 0, isTargetOccupied.Length);
            titleLabel.Visible = true;
            startButton.Visible = true;
            timerLabel.Visible = false;
            restartButton.Visible = false;
            resultLabel.Visible = false;
            gameStarted = false;
            gameWon = false;
            Invalidate();
        }

        private void ShapeDraggingGamePanel_Resize(object sender, EventArgs e)
        {
            InitializeShapes();
            titleLabel.Location = new Point((Width - 500) / 2, 20);
            startButton.Location = new Point((Width - 200) / 2, 150);
            timerLabel.Location = new Point((Width - 200) / 2, Height - 100);
            restartButton.Location = new Point((Width - 150) / 2, Height - 60);
            resultLabel.Location = new Point((Width - 500) / 2, 270);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            if (gameStarted)
            {
                // Draw targets with matching color outlines
                using (Brush targetBrush = new SolidBrush(targetColor))
                {
                    for (int i = 0; i < NumberOfShapes; i++)
                    {
                        using (Pen targetPen = new Pen(shapeColors[i], 3))
                        {
                            DrawShape(g, targetBrush, targetPen, shapeTypes[i], targets[i]);
                        }
                    }
                }

                for (int i = 0; i < NumberOfShapes; i++)
                {
                    if (isShapePlaced[i])
                    {
                        using (Brush brush = new SolidBrush(placedShapeColor))
                        {
                            DrawShape(g, brush, Pens.Black, shapeTypes[i], shapes[i]);
                        }
                    }
                }

                List<int> unplaced = new List<int>();
                for (int i = 0; i < NumberOfShapes; i++)
                {
                    if (!isShapePlaced[i] && i != selectedShapeIndex)
                    {
                        unplaced.Add(i);
                    }
                }

                foreach (int i in unplaced)
                {
                    using (Brush brush = new SolidBrush(shapeColors[i]))
                    {
                        DrawShape(g, brush, Pens.Black, shapeTypes[i], shapes[i]);
                    }
                }

                if (selectedShapeIndex != -1 && !isShapePlaced[selectedShapeIndex])
                {
                    using (Brush brush = new SolidBrush(shapeColors[selectedShapeIndex]))
                    {
                        DrawShape(g, brush, Pens.Black, shapeTypes[selectedShapeIndex], shapes[selectedShapeIndex]);
                    }
                }
            }
        }

        private void DrawShape(Graphics g, Brush brush, Pen pen, ShapeType type, Rectangle bounds)
        {
            switch (type)
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
                    PointF[] triangle = {
                        new PointF(bounds.Left + bounds.Width/2, bounds.Top),
                        new PointF(bounds.Left, bounds.Bottom),
                        new PointF(bounds.Right, bounds.Bottom)
                    };
                    g.FillPolygon(brush, triangle);
                    g.DrawPolygon(pen, triangle);
                    break;
                case ShapeType.Hexagon:
                    PointF[] hexagon = {
                        new PointF(bounds.Left + bounds.Width*0.25f, bounds.Top),
                        new PointF(bounds.Left + bounds.Width*0.75f, bounds.Top),
                        new PointF(bounds.Right, bounds.Top + bounds.Height/2),
                        new PointF(bounds.Left + bounds.Width*0.75f, bounds.Bottom),
                        new PointF(bounds.Left + bounds.Width*0.25f, bounds.Bottom),
                        new PointF(bounds.Left, bounds.Top + bounds.Height/2)
                    };
                    g.FillPolygon(brush, hexagon);
                    g.DrawPolygon(pen, hexagon);
                    break;
            }
        }

        private void ShapeDraggingGamePanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (!gameStarted || gameWon || timeLeft <= 0) return;

            for (int i = 0; i < NumberOfShapes; i++)
            {
                if (shapes[i].Contains(e.Location) && !isShapePlaced[i])
                {
                    selectedShapeIndex = i;
                    offset = new Point(e.X - shapes[i].X, e.Y - shapes[i].Y);
                    Cursor = Cursors.Hand;
                    break;
                }
            }
        }

        private void ShapeDraggingGamePanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (selectedShapeIndex != -1)
            {
                shapes[selectedShapeIndex] = new Rectangle(
                    e.X - offset.X,
                    e.Y - offset.Y,
                    shapes[selectedShapeIndex].Width,
                    shapes[selectedShapeIndex].Height
                );
                Invalidate();
            }
        }

        private void ShapeDraggingGamePanel_MouseUp(object sender, MouseEventArgs e)
        {
            if (selectedShapeIndex != -1)
            {
                bool placed = false;
                int targetIndex = selectedShapeIndex;
                Point center = new Point(
                    shapes[selectedShapeIndex].X + shapes[selectedShapeIndex].Width / 2,
                    shapes[selectedShapeIndex].Y + shapes[selectedShapeIndex].Height / 2
                );

                if (targets[targetIndex].Contains(center) && !isTargetOccupied[targetIndex])
                {
                    shapes[selectedShapeIndex] = targets[targetIndex];
                    isShapePlaced[selectedShapeIndex] = true;
                    isTargetOccupied[targetIndex] = true;
                    placed = true;
                }

                if (!placed)
                {
                    shapes[selectedShapeIndex] = originalShapes[selectedShapeIndex];
                }

                selectedShapeIndex = -1;
                Cursor = Cursors.Default;
                Invalidate();

                bool allPlaced = Array.TrueForAll(isShapePlaced, p => p);
                if (allPlaced)
                {
                    gameTimer.Stop();
                    resultLabel.Text = "Congratulations! All shapes placed correctly!";
                    resultLabel.ForeColor = Color.Green;
                    resultLabel.Visible = true;
                    restartButton.Visible = true;
                    gameWon = true;
                }
                else if (timeLeft <= 0)
                {
                    CheckGameOver();
                }
            }
        }
    }
}