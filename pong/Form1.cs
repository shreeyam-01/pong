namespace pong
{
    public partial class pongGame : Form
    {
        // Location variables
        int cpuDirection = 4;
        int ballXCoordinate = 5;
        int ballYCoordinate = 5;

        // Score variables
        int playerScore = 0;
        int cpuScore = 0;

        // Size variables
        int bottomBoundary;
        int XMidPoint;
        int YMidPoint;

        // Detection variables
        bool playerDetectedUp;
        bool playerDetectedDown;

        // Special keys
        int spaceBarClicked = 0;

        // To keep track of ball
        int lastBallX;
        int lastBallY;
        public pongGame()
        {
            InitializeComponent();
            bottomBoundary = ClientSize.Height - player.Height;
            XMidPoint = ClientSize.Width / 2;
            YMidPoint = ClientSize.Height / 2;
        }

        private void pongGame_Load(object sender, EventArgs e)
        {

        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            Random newBallSpot = new Random();
            int newSpot = newBallSpot.Next(100, ClientSize.Height - 100);

            cpu.Top += cpuDirection;

            // Adjust where the ball is
            pongBall.Top -= ballYCoordinate;
            pongBall.Left -= ballXCoordinate;

            // Position of the ball
            lastBallX = pongBall.Left;
            lastBallY = pongBall.Top;

            // Make Cpu move
            if (ballXCoordinate < 0 && lastBallX < XMidPoint)
            {
                if (cpu.Top + cpu.Height / 2 < lastBallY)
                {
                    cpu.Top += 5; // move down
                }
                else
                {
                    cpu.Top -= 5; // move up
                }
            }

            // Check if cpu has reached the top or bottom
            if (cpu.Top<0 || cpu.Top > bottomBoundary)
            {
                cpuDirection = -cpuDirection;
            }

            // Check if the ball has exited left part of the screen
            if (pongBall.Left < 0)
            {
                pongBall.Left = XMidPoint;
                pongBall.Top = newSpot;
                ballXCoordinate = -ballXCoordinate;
                cpuScore++;
                cpuScoreLabel.Text = cpuScore.ToString();
            }

            // Check if the ball has exited right part of the screen
            if (pongBall.Left + pongBall.Width > ClientSize.Width)
            {
                pongBall.Left = XMidPoint;
                pongBall.Top = newSpot;
                ballXCoordinate = -ballXCoordinate;
                playerScore++;
                playerScoreLabel.Text = playerScore.ToString();
            }

            // Ensure that ball is within the boundary of the screen
            if(pongBall.Top<0 || pongBall.Top + pongBall.Height > ClientSize.Height)
            {
                ballYCoordinate = -ballYCoordinate;
            }

            // Check if the ball hits player or cpu
            if(pongBall.Bounds.IntersectsWith(cpu.Bounds) || pongBall.Bounds.IntersectsWith(player.Bounds))
            {
                ballXCoordinate = -ballXCoordinate;
            }

            // Move player up
            if(playerDetectedUp==true && player.Top > 0)
            {
                player.Top -= 10;
            }

            // Move player down
            if(playerDetectedDown==true && player.Top < bottomBoundary)
            {
                player.Top += 10;
            }

            // Check for winner
            if (playerScore >= 10)
            {
                gameTimer.Stop();
                reset();
                MessageBox.Show("The player has won the game");
            }
        }

        public void reset()
        {
            playerScore = 0;
            cpuScore = 0;
            playerScoreLabel.Text = playerScore.ToString();
            cpuScoreLabel.Text = cpuScore.ToString();
        }

        private void pongGame_KeyDown(object sender, KeyEventArgs e)
        {
            // If player presses the up arrow, the player paddle moves up
            if (e.KeyCode == Keys.Up)
            {
                playerDetectedUp = true;
            }

            // If player presses the down arrow, the cpu paddle moves down
            if (e.KeyCode == Keys.Down)
            {
                playerDetectedDown = true;
            }

            // If player enters spacebar, the game pauses
            if (e.KeyCode == Keys.Space)
            {
                if (spaceBarClicked % 2 == 0)
                {
                    gameTimer.Stop();
                }
                else
                {
                    gameTimer.Start();
                }
                spaceBarClicked++;
            }
        }

        private void pongGame_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                playerDetectedUp = false;
            }

            if (e.KeyCode == Keys.Down)
            {
                playerDetectedDown = false;
            }
        }
    }
}