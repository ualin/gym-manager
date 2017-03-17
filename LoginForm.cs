using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gym_Manager
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string user = textBox2.Text.Trim();
            string pass = textBox3.Text.Trim();

            if (user == "admin" && pass == "fitnesschef")
            {
                this.Hide();
                MainForm mainForm = new MainForm();
                mainForm.ShowDialog(this);
                this.Show();
                textBox3.Text = "";
            }
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                button1_Click(null, null);
        }
    }
}
