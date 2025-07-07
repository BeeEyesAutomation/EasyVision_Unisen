using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeeUi
{
    public partial class SettingDevice : Form
    {
        public SettingDevice()
        {
            InitializeComponent();
        }

        private void SettingDevice_Load(object sender, EventArgs e)
        {
            
            //if (G.ConfiGlobal.TypeCamera ==TypeCamera.TinyIV)
            //{
              //  G.DeviceConnectForm.ReadingPage.DeviceSettingPage.Init(true);
            //    btnAutoExposure.IsCLick = G.DeviceConnectForm.ReadingPage.DeviceSettingPage.GetAutoExposue();
            //    btnIamgeImgMirror.IsCLick = G.DeviceConnectForm.ReadingPage.DeviceSettingPage.GetImageMirror();
            //    btnInverse.IsCLick = G.DeviceConnectForm.ReadingPage.DeviceSettingPage.GetImageInverse();
            //    trackExpo.Value = G.DeviceConnectForm.ReadingPage.DeviceSettingPage.GetValueExpo();
            //    trackGain.Value = G.DeviceConnectForm.ReadingPage.DeviceSettingPage.GetGain();
            //}    
        
        }

        private void btnAutoExposure_Click(object sender, EventArgs e)
        {
            //trackExpo.Enabled = !btnAutoExposure.IsCLick;
            //G.DeviceConnectForm.ReadingPage.DeviceSettingPage.SetAutoExposue(btnAutoExposure.IsCLick);
            //if(!btnAutoExposure.IsCLick)
            //    G.DeviceConnectForm.ReadingPage.DeviceSettingPage.SetValueExpo(trackExpo.Value);
        }

        private void SettingDevice_FormClosing(object sender, FormClosingEventArgs e)
        {
           // G.DeviceConnectForm.ReadingPage.DeviceSettingPage.Init(false);
        }

        private void trackExpo_ValueChanged(float obj)
        {
           /// G.DeviceConnectForm.ReadingPage.DeviceSettingPage.SetValueExpo(trackExpo.Value);

        }
        
        private void trackGain_ValueChanged(float obj)
        {
           // G.DeviceConnectForm.ReadingPage.DeviceSettingPage.SetGain(trackGain.Value);
        }

        private void btnIamgeImgMirror_Click(object sender, EventArgs e)
        {
           // G.DeviceConnectForm.ReadingPage.DeviceSettingPage.SetImageMirror(btnIamgeImgMirror.IsCLick);
        }

        private void btnInverse_Click(object sender, EventArgs e)
        {
          //  G.DeviceConnectForm.ReadingPage.DeviceSettingPage.SetImageInverse(btnInverse.IsCLick);
        }

        private void cbReSolution_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            String[] sp = cbReSolution.Text.Trim().Split(' ')[0].Split('x');
            int w = Convert.ToInt32( sp[0]);
            int h = Convert.ToInt32(sp[1]);
            int x = 1280/2-w/2; int y = 1080/2-h/2;
            G.EditTool.View.imgView.Location = new Point(G.EditTool.View.pView.Width / 2 - w / 2, G.EditTool.View.pView.Height / 2 - h / 2);

          //  G.DeviceConnectForm.ReadingPage.DeviceSettingPage.SetROI(x, y, w, h);
             }

        private void btnSave_Click(object sender, EventArgs e)
        {
           // G.DeviceConnectForm.SavetoDevice();
        }
    }
}
