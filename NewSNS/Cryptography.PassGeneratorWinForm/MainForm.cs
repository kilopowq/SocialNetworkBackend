using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cryptography;

namespace Cryptography.PassGeneratorWinForm
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            cmbMethod.SelectedIndex = 0;
        }

        private void chkDisplayPassword_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDisplayPassword.Checked)
            {
                txtPassword.PasswordChar = '\0';
                txtPasswordConfirm.Enabled = false;
            }
            else
            {
                txtPassword.PasswordChar = '*';
                txtPasswordConfirm.Enabled = true;
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if ((!chkDisplayPassword.Checked) && (txtPassword.Text != txtPasswordConfirm.Text))
            {
                MessageBox.Show("Password and confirm don't match.");
                return;
            }

            //txtHash.Text = SimpleHash.ComputeHash(txtPassword.Text, cmbMethod.SelectedValue.ToString(), null);
            txtHash.Text = SimpleHash.ComputeHash(txtPassword.Text, cmbMethod.SelectedItem.ToString(), null);
        }

        private void btnVerify_Click(object sender, EventArgs e)
        {
            if (SimpleHash.VerifyHash(txtPassword.Text, cmbMethod.SelectedItem.ToString(), txtHash.Text))
            {
                MessageBox.Show("Hash and password match.");
            }
            else
            {
                MessageBox.Show("Hash and password do not match.");
            }
        }
    }
}
