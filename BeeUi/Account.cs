using BeeCore;
using BeeGlobal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeeUi
{
    public partial class Account : Form
    {
        public Account()
        {
            InitializeComponent();
            cbUser.DataSource=(Users[])Enum.GetValues(typeof(Users));
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {Users users = (Users)Enum.Parse(typeof(Users), cbUser.SelectedValue.ToString());
            switch (users)
            {   case Users.Admin:
                    if (txtPass.Text.Trim().ToLower() != "393939") {
                        MessageBox.Show("You have wrong password!");
                        this.Close();
                    } 
                        
                     
                    break;
                case Users.Leader:
                    if (txtPass.Text.Trim().ToLower() != "797979")
                    {
                        MessageBox.Show("You have wrong password!");
                        this.Close();

                    }    
                    
                    break;
                case Users.User:
                    break;
            }    
            Global.Config.Users = users;
            SaveData.Config(Global.Config);
            Global.EditTool.Acccess(Global.IsRun);
            this.Close();

        }

        private void txtPass_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Account_Load(object sender, EventArgs e)
        {
            this.Location = new Point(Screen.PrimaryScreen.Bounds.Width / 2 - this.Width / 2, Screen.PrimaryScreen.Bounds.Height / 2 - this.Height / 2);
            
        }

        private void tableLayoutPanel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtPass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin.PerformClick();
            }
        }

        private void cbUser_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cbUser_SelectionChangeCommitted(object sender, EventArgs e)
        {
            
        }
    }
}
