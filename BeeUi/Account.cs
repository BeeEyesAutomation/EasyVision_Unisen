﻿using BeeCore;
using BeeGlobal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if(cbUser.Text.Trim()=="Admin")
                {
                if(txtPass.Text.Trim().ToLower()=="393939")
                {
                   Global.Config.nameUser = cbUser.Text.Trim();
                    this.Hide();
                   // G.EditTool.btnUser.Text =Global.Config.nameUser;
                    if (File.Exists("Default.config"))
                        File.Delete("Default.config");
                    Access.SaveConfig("Default.config",Global.Config);
                    G.Header.Acccess(Global.IsRun);
                }
                else
                {
                    MessageBox.Show("You have wrong password!");
                }

            }
            else if (cbUser.Text.Trim() == "Leader")
            {
                if (txtPass.Text.Trim().ToLower() == "797979")
                {
                   Global.Config.nameUser = cbUser.Text.Trim();
                    this.Hide();
                   // G.EditTool.btnUser.Text =Global.Config.nameUser;
                    if (File.Exists("Default.config"))
                        File.Delete("Default.config");
                    Access.SaveConfig("Default.config",Global.Config);
                    G.Header.Acccess(Global.IsRun);
                }
                else
                {
                    MessageBox.Show("You have wrong password!");
                }

            }
            else if (cbUser.Text.Trim() == "Maintenance")
            {
                if (txtPass.Text.Trim().ToLower() == "1234@8765")
                {
                   Global.Config.nameUser = cbUser.Text.Trim();
                    this.Hide();
                    if (File.Exists("Default.config"))
                        File.Delete("Default.config");
                    Access.SaveConfig("Default.config",Global.Config);
                    G.Header.Acccess(Global.IsRun);
                }
                else
                {
                    MessageBox.Show("You have wrong password!");
                }

            }
            else
            {
               Global.Config.nameUser ="User";
                this.Hide();
                 if (File.Exists("Default.config"))
                    File.Delete("Default.config");
                Access.SaveConfig("Default.config",Global.Config);
                G.Header.Acccess(Global.IsRun);
            }    


        }

        private void txtPass_TextChanged(object sender, EventArgs e)
        {

        }

        private void cbUser_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
