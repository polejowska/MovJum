using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MovJum
{
    public partial class Form1 : Form
    {
        int playerNumber;
        int timerCounter;
        int scoreCounter = 0;

        bool moveLeft, moveRight, jump, gameOver;
        int jumpSpeed, force;
        int playerSpeed = 15;

        int horizontalSpeed = 5;
        int verticalSpeed = 5;

        int firstEnemySpeed = 5;
        int secondEnemySpeed = 3;

        public Form1(int playerNumber)
        {
            InitializeComponent();
            timer1.Start();
            this.playerNumber = playerNumber;
            menuStrip1.ForeColor = Color.BlanchedAlmond;
            statusLabel.Text = "";
        }

        private void MainTimerEvent(object sender, EventArgs e)
        {
            if (playerNumber == 1)
            {
                player.Image = Properties.Resources.player_stand;
            }
            if (playerNumber == 2)
            {
                player.Image = Properties.Resources.player_girl_stand1;
            }

            labelScore.Text = "| Score: " + scoreCounter + " | ";
            labelScore.BackColor = System.Drawing.Color.Transparent;
            player.BackColor = System.Drawing.Color.Transparent;

            // Make the player fall
            player.Top += jumpSpeed;

            // Move the player;
            if (moveLeft == true)
            {
                player.Left -= playerSpeed;
            }
            if (moveRight == true)
            {
                player.Left += playerSpeed;
            }
            if (jump == true && force < 0)
            {
                jump = false;
            }
            if (jump == true)
            {
                jumpSpeed = -10;
                force -= 1;
            }
            else
            {
                jumpSpeed = 8;
            }

           
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox)
                {
                    x.BackColor = System.Drawing.Color.Transparent;

                    if ((string)x.Tag == "platform")
                    {
                        if (player.Bounds.IntersectsWith(x.Bounds))
                        {
                            force = 8;
                            player.Top = x.Top - player.Height;

                            if ((string)x.Name == "horizontalPlatform" && moveLeft == false ||
                                (string)x.Name == "horizontalPlatform" && moveRight == false)
                            {
                                // Move the player with the platform
                                player.Left -= horizontalSpeed; 
                            }

                        }
                        x.BringToFront();
                    }

                    if ((string)x.Tag == "coin")
                    {
                        if (player.Bounds.IntersectsWith(x.Bounds) && x.Visible == true)
                        {
                            x.Visible = false;
                            scoreCounter++;
                        }
                    }

                    if ((string)x.Tag == "enemy")
                    {
                        if (player.Bounds.IntersectsWith(x.Bounds))
                        {
                            if (playerNumber == 1)
                            {
                                player.Image = Properties.Resources.player_enemy;
                            }
                            if (playerNumber == 2)
                            {
                                player.Image = Properties.Resources.player_girl_enemy;
                            }
                            player.BackgroundImageLayout = ImageLayout.Zoom;

                            statusLabel.ForeColor = Color.Red;
                            statusLabel.Text = "You failed. Press enter to try again.";

                            gameTimer.Stop();
                            timer1.Stop();
                            gameOver = true;
                   
                        }
                    }

                }
            }

            // Move the horizontal platform
            horizontalPlatform.Left -= horizontalSpeed;

            // Move the vertical platform
            verticalPlatform.Top += verticalSpeed;

            // Make the platforms bounce
            if (horizontalPlatform.Left < 0 || (horizontalPlatform.Left + horizontalPlatform.Width > this.ClientSize.Width))
            {
                horizontalSpeed = -horizontalSpeed;
            }

            if (verticalPlatform.Top < 200 || verticalPlatform.Top > 650)
            {
                verticalSpeed = -verticalSpeed;
            }

            // Move enemies
            firstEnemy.Left -= firstEnemySpeed;
            secondEnemy.Left += secondEnemySpeed;

            // Limit the enemies movement to platform borders
            if (firstEnemy.Left < pictureBox6.Left || firstEnemy.Left + firstEnemy.Width > pictureBox6.Left + pictureBox6.Width)
            {
                firstEnemySpeed = -firstEnemySpeed;
            }
            if (secondEnemy.Left < pictureBox3.Left || secondEnemy.Left + secondEnemy.Width > pictureBox3.Left + pictureBox3.Width)
            {
                secondEnemySpeed = -secondEnemySpeed;
            }

            // If the player fell
            if (player.Top + player.Height > this.ClientSize.Height + 70)
            {
                statusLabel.ForeColor = Color.Red;
                statusLabel.Text = "You failed. Press enter to try again.";
                gameTimer.Stop();
                gameOver = true;

            }

            // Win conditions
            if (player.Bounds.IntersectsWith(doorGame.Bounds) && scoreCounter == 22)
            {
                statusLabel.Visible = true;
                statusLabel.ForeColor = Color.Green;
                statusLabel.Text = "Congratulations! You are a winner!";
                gameTimer.Stop();
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void changeBack_Click(object sender, EventArgs e)
        {
            this.BackgroundImage = Properties.Resources.mountains_1;
            this.BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void TickEvent(object sender, EventArgs e)
        {
            timerCounter++;
            timerLabel.Text = "Time: " + timerCounter + " s |";
        }

        private void restartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RestartGame();
        }

        private void firstLandscapeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.BackgroundImage = Properties.Resources.mountains_1;
        }

        private void secondLandscapeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.BackgroundImage = Properties.Resources.mountain_2;
        }

        private void startMenuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This will close down the whole application. Confirm?",
              "Close Application", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                MessageBox.Show("The application has been closed successfully.",
                    "Application Closed!", MessageBoxButtons.OK);
                System.Windows.Forms.Application.Exit();
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                moveLeft = false;
            }
            else if (e.KeyCode == Keys.Right)
            {
                moveRight = false;
            }
            else if (jump == true)
            {
                jump = false;
            }

            // Click enter to restart if the game is over
            if (e.KeyCode == Keys.Enter && gameOver == true)
            {
                RestartGame();
            }
        }

        // Move if key is pressed
        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                moveLeft = true;
            }
            else if (e.KeyCode == Keys.Right)
            {
                moveRight = true;
            }
            else if (e.KeyCode == Keys.Space && jump == false) // Second condition - prevent double jumping
            {
                jump = true;
            }
        }

        private void RestartGame()
        {
            jump = false;
            moveLeft = false;
            moveRight = false;
            gameOver = false;
            scoreCounter = 0;

            labelScore.Text = "| Score: " + scoreCounter + "| ";

            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && x.Visible == false)
                {
                    x.Visible = true;
                }
            }

            statusLabel.Visible = false;

            // Reset positions
            player.Left = 31;
            player.Top = 584;
            firstEnemy.Left = 12;
            firstEnemy.Top = 207;
            secondEnemy.Left = 338;
            secondEnemy.Top = 484;
            horizontalPlatform.Left = 309;
            verticalPlatform.Top = 554;

            // Resume timers
            gameTimer.Start();
            timer1.Start();
        }

        private void CloseGame(object sender, FormClosedEventArgs e)
        {

        }
    }
}
