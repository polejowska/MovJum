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
    public partial class Start : Form
    {
        int playerNumber;

        public Start()
        {
            InitializeComponent();
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            // Set player number
            if (player1.Checked)
            {
                playerNumber = 1;
            }
            if (player2.Checked)
            {
                playerNumber = 2;
            }

            Form gameForm = new Form1(playerNumber);
            gameForm.ShowDialog();
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This will close down the whole application. Confirm?",
                "Close Application", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                MessageBox.Show("The application has been closed successfully.",
                    "Application Closed!", MessageBoxButtons.OK);
                System.Windows.Forms.Application.Exit();
            }
        }
    }
}