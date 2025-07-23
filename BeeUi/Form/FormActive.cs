using BeeCore;
using BeeGlobal;
using BeeUi.Commons;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeeUi
{
    public partial class FormActive : Form
    {

        Access access = new Access();
        public FormActive()
        {
            InitializeComponent();
          
            tmShow.Interval = 1000;
            tmShow.Tick += TmShow_Tick;
        }

        private void TmShow_Tick(object sender, EventArgs e)
        {
            G.IsActive = true;
            
            tmShow.Enabled = false;
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
        public void CheckActive(String addMac)
        {
            try
            {
                G.Load.IsLockTrial = false;
                if (File.Exists("Keys.bee"))
                    G.Load.sKey = Crypto.DecryptString128Bit(access.LoadKeys("Keys.bee"), "");
                else return;
                //  MessageBox.Show(sKey);
                if (G.Load.sKey != null && G.Load.sKey != "")
                {
                    String[] sKeys = G.Load.sKey.Split('*'); // Decompile.Compile(sKey);
                    G.keys = new KeyAcitve(sKeys[0], sKeys[1], DateTime.Parse(sKeys[2]), Convert.ToInt32(sKeys[4]), 0, (typeKey)Enum.Parse(typeof(typeKey), sKeys[3]));

                  G.Licence = G.keys.typeKey.ToString();
                    if (G.keys.typeKey == typeKey.Trial)
                        G.Licence += " " + G.keys.DateKey + " ngày ("+G.keys.dateCreate.AddDays(G.keys.DateKey).ToString("yyyy/MM/dd")+")";
                    else if (G.keys.typeKey == typeKey.Machine)
                        G.Licence += " - " + G.keys.IDDev.Split('$')[0].Trim();
                    switch (G.keys.typeKey)
                    {

                        case typeKey.Trial:
                            if (addMac == G.keys.Key && G.keys.IDDev ==Global.Config.IDCamera)
                            {
                                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Sytems");
                                if (key != null)
                                {
                                  
                                    if ((String)key.GetValue("0x353543645741") == "True")
                                    {

                                        G.Load.IsLockTrial = true;
                                        G.Load.sActive = "Locked Trial";
                                        G.IsActive = false;
                                        return;
                                    }
                                }
                                if (Properties.Settings.Default.IsLockTrial)
                                {
                                     key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Sytems");
                                    key.SetValue("0x353543645741", "True");
                                    G.Load.IsLockTrial = true;
                                    G.Load.sActive = "Locked Trial";
                                    G.IsActive = false;
                                    return;
                                }
                                if (G.keys.valueDateKey <= G.keys.DateKey)
                                {
                                    if (DateTime.Now >= G.keys.dateCreate)
                                    {
                                        TimeSpan sp = DateTime.Now - G.keys.dateCreate;
                                        if (sp.TotalDays < G.keys.valueDateKey)
                                        {

                                            G.IsActive = false;
                                            G.keys.valueDateKey = 10000;
                                            G.keys.DateKey = 0;
                                            Properties.Settings.Default.macAdd = "";
                                            Properties.Settings.Default.Save();
                                        }
                                        else
                                        {

                                            Properties.Settings.Default.macAdd = addMac;
                                            Properties.Settings.Default.Save();
                                            G.keys.valueDateKey = (int)sp.TotalDays;
                                            G.keys.dateCreate = DateTime.Now;

                                        }
                                        Properties.Settings.Default.macAdd = "";
                                        Properties.Settings.Default.Save();
                                        G.keys.DateKey = G.keys.DateKey - G.keys.valueDateKey;

                                        if (G.keys.DateKey < 0)
                                        {
                                            G.IsActive = false;
                                            G.Load.sActive = "Hết hạn Trial";
                                            
                                        }
                                        else
                                        {
                                            G.IsActive = true;
                                            G.Load.sActive = "Trial(" + G.keys.DateKey + "day)";
                                        }
                                        G.Load.sKey = sKeys[0] + "*" + sKeys[1] + "*" + G.keys.dateCreate.ToString("yyyy/MM/dd") + "*" + sKeys[3] + "*" + G.keys.DateKey.ToString();

                                    }
                                    else
                                    {
                                         key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Sytems");
                                        key.SetValue("0x353543645741", "True");
                                        G.Load.sActive = "Hết hạn Trial";
                                        Properties.Settings.Default.IsLockTrial = true;
                                        Properties.Settings.Default.Save();
                                    }
                                }
                                else
                                {
                                     key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Sytems");
                                    key.SetValue("0x353543645741", "True");
                                    G.Load.sActive = "Hết hạn Trial";
                                    Properties.Settings.Default.IsLockTrial = true;
                                    Properties.Settings.Default.Save();
                                    G.IsActive = false;
                                    Properties.Settings.Default.macAdd = "";
                                    Properties.Settings.Default.Save();
                                }
                            }
                            break;
                        case typeKey.Machine:
                            if (addMac == G.keys.Key)
                            {
                                if (G.keys.IDDev ==Global.Config.IDCamera)
                                {
                                    G.Load.sActive = "Machine";
                                    G.Load.sActive = G.keys.typeKey.ToString();
                                    G.IsActive = true;
                                    Properties.Settings.Default.macAdd = addMac;
                                    Properties.Settings.Default.Save();
                                }
                                else
                                {
                                    G.IsActive = false;
                                    G.Load.IsLockTrial = true;
                                    G.Load.sActive = "License is incorrect for this device!";
                                }
                            }
                            break;
                        case typeKey.Pro:
                            if (addMac == G.keys.Key)
                            {
                                G.Load.sActive = G.keys.typeKey.ToString();
                                G.IsActive = true;
                                Properties.Settings.Default.macAdd = addMac;
                                Properties.Settings.Default.Save();
                            }
                            break;
                    }

                }
                else
                {
                    G.IsActive = false;
                    Properties.Settings.Default.macAdd = "";
                    Properties.Settings.Default.Save();
                }
            }
            catch(Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }
        private void FormLoad_Load(object sender, EventArgs e)
        {
            txtMac.Text = G.Load.addMac;
            G.keys = new KeyAcitve();
            lbVersion.Text = G.Load.lbVersion.Text;
            this.Location = new Point(Screen.PrimaryScreen.Bounds.Width / 2 - this.Width / 2, Screen.PrimaryScreen.Bounds.Height / 2 - this.Height / 2);

        }
        Timer tmShow = new Timer();
        private void btnActive_Click(object sender, EventArgs e)
        {
            if (G.keys.Key== "")
            {
                MessageBox.Show("Key Entry!");
                return;
            }    
               
            if (G.Load.addMac == G.keys.Key)

            {
                if (File.Exists("Keys.bee"))
                    File.Delete("Keys.bee");
                access.SaveKeys( G.Load.sKey, "Keys.bee");
                this.Hide();
                G.Load.Show();
                G.Load.wLoad.RunWorkerAsync();
             
            }
            else
            {
                MessageBox.Show("Key not Right !");
            }
        }
        public String KeyActive="";
        private void btnCoppy_Click(object sender, EventArgs e)
        {
            if(saveFile.ShowDialog()==DialogResult.OK)
               {
                access.SaveKeys(KeyActive.Trim(), saveFile.FileName);
                MessageBox.Show("Please send it to us so we can activate it for you");
            }
           
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("www.beeau.vn/sharing");
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
           
            if (G.Load != null)
                G.Load.Close();
            if (G.Main != null)
                G.Main.Close();
            this.Close();
            Process.GetCurrentProcess().Kill();
        }

        private void FormActive_FormClosing(object sender, FormClosingEventArgs e)
        {
            G.Load.Close();
        }
        Crypto Crypto = new Crypto();
        private void btnKeys_Click(object sender, EventArgs e)
        {
            if (openFile.ShowDialog() == DialogResult.OK)
            {
               
                        G.Load.sKey = access.LoadKeys(openFile.FileName);
            
                G.Load.sKeys = Crypto.DecryptString128Bit(G.Load.sKey, "").Split('*');
                G.keys = new KeyAcitve( G.Load.sKeys[0] ,G.Load.sKeys[1], DateTime.Parse(G.Load.sKeys[2]), Convert.ToInt32(G.Load.sKeys[4]), 0, (typeKey)Enum.Parse(typeof(typeKey), G.Load.sKeys[3]));
                if (G.keys.typeKey == typeKey.Trial)
                {
                    RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Sytems");
                    if (key != null)
                    {

                        if ((String)key.GetValue("0x353543645741") == "True")
                        {
                            txtLicence.Text = "Locked Trial";
                            return;
                        }
                    }
                }
                txtLicence.Text = G.keys.typeKey.ToString() ;
                if (G.keys.typeKey == typeKey.Trial)
                    txtLicence.Text += " " + G.keys.DateKey + " ngày ";
                else if (G.keys.typeKey == typeKey.Machine)
                    txtLicence.Text += " - " + G.keys.IDDev.Split('$')[0].Trim() ;


                //F903-F378-A971-C1A3-1D62-93D6-DCB8-D6AD

                btnKeys.BackColor = Color.SteelBlue;
            }
        }
    }
}
