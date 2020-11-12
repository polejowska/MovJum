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
        int player_n;
        int timer_count;

        bool move_left, move_right, jump, game_over;
        int jump_speed, force;
        int score = 0;
        int player_speed = 15;

        int horizontal_speed = 5;
        int vertical_speed = 5;

        int first_enemy_speed = 5;
        int second_enemy_speed = 3;

        public Form1(int player_num)
        {
            InitializeComponent();
            timer1.Start();
            this.player_n = player_num;
            menuStrip1.ForeColor = Color.BlanchedAlmond;
            statusLabel.Text = "";
        }

        private void MainTimerEvent(object sender, EventArgs e)
        {
            if (player_n == 1)
            {
                player.Image = Properties.Resources.player_stand;
            }
            if (player_n == 2)
            {
                player.Image = Properties.Resources.player_girl_stand;
            }

            labelScore.Text = "| Score: " + score + " | ";
            labelScore.BackColor = System.Drawing.Color.Transparent;
            player.BackColor = System.Drawing.Color.Transparent;

            // Make the player fall
            player.Top += jump_speed;

            // Move the player;
            if (move_left == true)
            {
                player.Left -= player_speed;
            }
            if (move_right == true)
            {
                player.Left += player_speed;
            }
            if (jump == true && force < 0)
            {
                jump = false;
            }
            if (jump == true)
            {
                jump_speed = -10;
                force -= 1;
            }
            else
            {
                jump_speed = 8;
            }

            // Check collisions 
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

                            if ((string)x.Name == "horizontalPlatform" && move_left == false ||
                                (string)x.Name == "horizontalPlatform" && move_right == false)
                            {
                                // Move the player with the platform
                                player.Left -= horizontal_speed; 
                            }

                        }
                        x.BringToFront();
                    }

                    if ((string)x.Tag == "coin")
                    {
                        if (player.Bounds.IntersectsWith(x.Bounds) && x.Visible == true)
                        {
                            x.Visible = false;
                            score++;
                        }
                    }

                    if ((string)x.Tag == "enemy")
                    {
                        if (player.Bounds.IntersectsWith(x.Bounds))
                        {
                            if (player_n == 1)
                            {
                                player.Image = Properties.Resources.player_enemy;
                            }
                            if (player_n == 2)
                            {
                                player.Image = Properties.Resources.player_girl_enemy;
                            }
                            player.BackgroundImageLayout = ImageLayout.Zoom;

                            statusLabel.ForeColor = Color.Red;
                            statusLabel.Text = "You failed. Press enter to try again.";

                            gameTimer.Stop();
                            timer1.Stop();
                            game_over = true;
                   
                        }
                    }

                }
            }

            // Move the horizontal platform
            horizontalPlatform.Left -= horizontal_speed;

            // Move the vertical platform
            verticalPlatform.Top += vertical_speed;

            // Make the platforms bounce
            if (horizontalPlatform.Left < 0 || (horizontalPlatform.Left + horizontalPlatform.Width > this.ClientSize.Width))
            {
                horizontal_speed = -horizontal_speed;
            }

            if (verticalPlatform.Top < 200 || verticalPlatform.Top > 650)
            {
                vertical_speed = -vertical_speed;
            }

            // Move enemies
            firstEnemy.Left -= first_enemy_speed;
            secondEnemy.Left += second_enemy_speed;

            // Limit the enemies movement to platform borders
            if (firstEnemy.Left < pictureBox6.Left || firstEnemy.Left + firstEnemy.Width > pictureBox6.Left + pictureBox6.Width)
            {
                first_enemy_speed = -first_enemy_speed;
            }
            if (secondEnemy.Left < pictureBox3.Left || secondEnemy.Left + secondEnemy.Width > pictureBox3.Left + pictureBox3.Width)
            {
                second_enemy_speed = -second_enemy_speed;
            }

            // If the player fell
            if (player.Top + player.Height > this.ClientSize.Height + 70)
            {
                gameTimer.Stop();
                game_over = true;
                statusLabel.ForeColor = Color.Red;
                statusLabel.Text = "You failed. Press enter to try again.";

            }

            // Win conditions
            if (player.Bounds.IntersectsWith(doorGame.Bounds) && score == 22)
            {
                statusLabel.Visible = true;
                statusLabel.ForeColor = Color.Green;
                statusLabel.Text = "Congratulations! You are a winner!";
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
            timer_count++;
            timerLabel.Text = "Time: " + timer_count + " s |";
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
                move_left = false;
            }
            else if (e.KeyCode == Keys.Right)
            {
                move_right = false;
            }
            else if (jump == true)
            {
                jump = false;
            }

            // Click enter to restart if the game is over
            if (e.KeyCode == Keys.Enter && game_over == true)
            {
                RestartGame();
            }
        }

        // Move if key is pressed
        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                move_left = true;
            }
            else if (e.KeyCode == Keys.Right)
            {
                move_right = true;
            }
            else if (e.KeyCode == Keys.Space && jump == false) // Second condition - prevent double jumping
            {
                jump = true;
            }
        }

        private void RestartGame()
        {
            jump = false;
            move_left = false;
            move_right = false;
            game_over = false;
            score = 0;

            labelScore.Text = "| Score: " + score + "| ";

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
