using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Racquet_Ball
{
    public partial class Form1 : Form
    {

        Rectangle player1 = new Rectangle(10, 170, 10, 60);
        Rectangle player2 = new Rectangle(10, 170, 10, 60); // Start both players on the left side
        Rectangle ball = new Rectangle(295, 195, 10, 10);

        int player1Score = 0;
        int player2Score = 0;

        int playerSpeed = 4;
        int ballXSpeed = -8; // Increase ball speed
        int ballYSpeed = -8;

        bool wPressed = false;
        bool sPressed = false;
        bool upPressed = false;
        bool downPressed = false;
        bool aDown = false;
        bool dDown = false;
        bool leftDown = false;
        bool rightDown = false;

        int playerTurn = 1; // Track player's turn
        Pen highlightPen = new Pen(Color.White, 2); // Pen for highlighting active player

        SolidBrush blueBrush = new SolidBrush(Color.DodgerBlue);
        SolidBrush whiteBrush = new SolidBrush(Color.White);

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wPressed = true;
                    break;
                case Keys.S:
                    sPressed = true;
                    break;
                case Keys.Up:
                    upPressed = true;
                    break;
                case Keys.Down:
                    downPressed = true;
                    break;
                case Keys.A:
                    aDown = true;
                    break;
                case Keys.D:
                    dDown = true;
                    break;
                case Keys.Left:
                    leftDown = true;
                    break;
                case Keys.Right:
                    rightDown = true;
                    break;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wPressed = false;
                    break;
                case Keys.S:
                    sPressed = false;
                    break;
                case Keys.Up:
                    upPressed = false;
                    break;
                case Keys.Down:
                    downPressed = false;
                    break;
                case Keys.A:
                    aDown = false;
                    break;
                case Keys.D:
                    dDown = false;
                    break;
                case Keys.Left:
                    leftDown = false;
                    break;
                case Keys.Right:
                    rightDown = false;
                    break;
            }
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            // Move ball
            ball.X += ballXSpeed;
            ball.Y += ballYSpeed;

            // Check if ball hits any side and reverse direction
            if (ball.Y <= 0 || ball.Y >= this.Height - ball.Height)
            {
                ballYSpeed = -ballYSpeed;
            }
            if (ball.X <= 0 || ball.X >= this.Width - ball.Width)
            {
                ballXSpeed = -ballXSpeed;
            }

            // Check for scoring or restart ball when it touches left or right side
            if (ball.X <= 0)
            {
                if (playerTurn == 1)
                {
                    player2Score++;
                    p2ScoreLabel.Text = player2Score.ToString(); // Update player 2 score label
                    playerTurn = 2; // Switch turn to player 2
                }
                else
                {
                    player1Score++;
                    p1ScoreLabel.Text = player1Score.ToString(); // Update player 1 score label
                    playerTurn = 1; // Switch turn to player 1
                }

                // Reset ball and players
                ResetBallAndPlayers();
            }
            else if (ball.X >= this.Width - ball.Width)
            {
                if (playerTurn == 1)
                {
                    player1Score++;
                    p1ScoreLabel.Text = player1Score.ToString(); // Update player 1 score label
                    playerTurn = 1; // Switch turn to player 1
                }
                else
                {
                    player2Score++;
                    p2ScoreLabel.Text = player2Score.ToString(); // Update player 2 score label
                    playerTurn = 2; // Switch turn to player 2
                }

                // Reset ball and players
                ResetBallAndPlayers();
            }

            // Move player 1
            if (wPressed && player1.Y > 0)
            {
                player1.Y -= playerSpeed;
            }
            if (sPressed && player1.Y < this.Height - player1.Height)
            {
                player1.Y += playerSpeed;
            }
            if (aDown && player1.X > 0)
            {
                player1.X -= playerSpeed;
            }
            if (dDown && player1.X < this.Width - player1.Width)
            {
                player1.X += playerSpeed;
            }

            // Move player 2
            if (upPressed && player2.Y > 0)
            {
                player2.Y -= playerSpeed;
            }
            if (downPressed && player2.Y < this.Height - player2.Height)
            {
                player2.Y += playerSpeed;
            }
            if (leftDown && player2.X > 0)
            {
                player2.X -= playerSpeed;
            }
            if (rightDown && player2.X < this.Width - player2.Width)
            {
                player2.X += playerSpeed;
            }

            // Check collision with players based on ball direction and player's turn
            if (ballXSpeed < 0 && player1.IntersectsWith(ball))
            {
                // Handle collision with player 1
                ballXSpeed = -ballXSpeed;
                ball.X = player1.X + player1.Width;
                playerTurn = 1; // Switch turn to player 1
            }
            else if (ballXSpeed > 0 && player2.IntersectsWith(ball))
            {
                // Handle collision with player 2
                ballXSpeed = -ballXSpeed;
                ball.X = player2.X - ball.Width;
                playerTurn = 2; // Switch turn to player 2
            }

            // Check for winner
            if (player1Score == 3)
            {
                winLabel.Text = "Player 1 Wins";
                gameTimer.Stop();
            }
            else if (player2Score == 3)
            {
                winLabel.Text = "Player 2 Wins";
                gameTimer.Stop();
            }

            Refresh();
        }

        // Method to reset ball and players
        private void ResetBallAndPlayers()
        {
            ball.X = this.Width / 2;
            ball.Y = this.Height / 2;
            ballXSpeed = -Math.Abs(ballXSpeed); // Set ball speed towards the left
            ballYSpeed = -Math.Abs(ballYSpeed); // Set ball speed towards the top
            player1.Y = this.Height / 2 - player1.Height / 2;
            player2.Y = this.Height / 2 - player2.Height / 2;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(blueBrush, player1);
            e.Graphics.FillRectangle(blueBrush, player2);
            e.Graphics.FillRectangle(whiteBrush, ball);

            // Highlight active player
            if (playerTurn == 1)
            {
                e.Graphics.DrawRectangle(highlightPen, player1);
            }
            else
            {
                e.Graphics.DrawRectangle(highlightPen, player2);
            }
        }

    }
}
