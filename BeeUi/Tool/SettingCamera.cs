using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;


namespace BeeUi
{
    public partial class SettingCamera : UserControl
    {
        public SettingCamera()
        {
            InitializeComponent();
        }
        public void SetValue()
        {
            //numBright.Value = G._modelCur._ParaCamera.Brightness;
            //numContrast.Value = G._modelCur._ParaCamera.Contrast;
            //numGain.Value = G._modelCur._ParaCamera.Gain;
            //numGamma.Value = G._modelCur._ParaCamera.Gamma;
            //numSaturation.Value = G._modelCur._ParaCamera.Saturation;
            //numExplosure.Value = G._modelCur._ParaCamera.Exposure;

            //trackBright.Value = G._modelCur._ParaCamera.Brightness;
            //trackContrast.Value = G._modelCur._ParaCamera.Contrast;
            //trackGain.Value = G._modelCur._ParaCamera.Gain;
            //trackGamma.Value = G._modelCur._ParaCamera.Gamma;
            //trackSaturation.Value = G._modelCur._ParaCamera.Saturation;
            //trackExplosure.Value = G._modelCur._ParaCamera.Exposure;
        }
        private void SettingCamera_Load(object sender, EventArgs e)
        {

         /*             // Brightness  1
            "" = Properties.Settings.Default.Browser;
            trackBright.Value = "";
            "" = Properties.Settings.Default.Browser;
            numBright.Value = "";
                       // Contrast  2
            "" = Properties.Settings.Default.Browser;
            trackContrast.Value = "";
            "" = Properties.Settings.Default.Browser;
            numContrast.Value = "";
                      // Hue 3
            "" = Properties.Settings.Default.Browser;
            trackHue.Value = "";
            "" = Properties.Settings.Default.Browser;
            numHue.Value = "";
                       // Saturation  4
            "" = Properties.Settings.Default.Browser;
            trackSaturation.Value = "";
            "" = Properties.Settings.Default.Browser;
            numSaturation.Value = "";
                        // Sharpness 5
            "" = Properties.Settings.Default.Browser;
            trackSharpness.Value = "";
            "" = Properties.Settings.Default.Browser;
            numSharpness.Value = "";
                        // Gamma 6
            "" = Properties.Settings.Default.Browser;
            trackGamma.Value = "";
            "" = Properties.Settings.Default.Browser;
            numGamma.Value = "";
                      // Blacklight 7
            "" = Properties.Settings.Default.Browser;
            trackGain.Value = "";
            "" = Properties.Settings.Default.Browser;
            numGain.Value = "";
                       // Blacklight 8
            "" = Properties.Settings.Default.Browser;
            trackBacklight.Value = "";
            "" = Properties.Settings.Default.Browser;
            numBacklight.Value = "";
                      // Explosure  9
            "" = Properties.Settings.Default.Browser;
            trackExplosure.Value = "";
            "" = Properties.Settings.Default.Browser;
            numExplosure.Value = "";*/

        }
        public void SetPara()
        {
           //  G._cv3.SetPara(G._modelCur._ParaCamera.Exposure, G._modelCur._ParaCamera.Brightness, G._modelCur._ParaCamera.Contrast, G._modelCur._ParaCamera.Saturation, G._modelCur._ParaCamera.Gain, G._modelCur._ParaCamera.Gamma);
        }
    
        private void pSetting_Paint(object sender, PaintEventArgs e)
        {

        }
       // ParaCamera parOld = new ParaCamera();
        private void trackBright_Scroll(object sender, EventArgs e)
        {
          //  G._modelCur._ParaCamera.Brightness = trackBright.Value;
          //  numBright.Value = G._modelCur._ParaCamera.Brightness;
            SetPara();
        }

        private void numBright_ValueChanged(object sender, EventArgs e)
        {
          //  G._modelCur._ParaCamera.Brightness = (int)numBright.Value;
          //  trackBright.Value = G._modelCur._ParaCamera.Brightness;
            SetPara();
        }

        private void trackContrast_Scroll(object sender, EventArgs e)
        {
          //  G._modelCur._ParaCamera.Contrast = trackContrast.Value;
           // numContrast.Value = G._modelCur._ParaCamera.Contrast;
            SetPara();
        }

        private void numContrast_ValueChanged(object sender, EventArgs e)
        {
          //  G._modelCur._ParaCamera.Contrast = (int)numContrast.Value;
          //  trackContrast.Value = G._modelCur._ParaCamera.Contrast;
            SetPara();
        }

       
        private void trackSaturation_Scroll(object sender, EventArgs e)
        {
           // G._modelCur._ParaCamera.Saturation = trackSaturation.Value;
           // numSaturation.Value =  G._modelCur._ParaCamera.Saturation;
            SetPara();
        }

        private void numSaturation_ValueChanged(object sender, EventArgs e)
        {
           // G._modelCur._ParaCamera.Saturation = (int)numSaturation.Value;
          //  trackSaturation.Value = G._modelCur._ParaCamera.Saturation;
            SetPara();
        }

       

        private void trackGamma_Scroll(object sender, EventArgs e)
        {
          //  G._modelCur._ParaCamera.Gamma = trackGamma.Value;
         //   numGamma.Value = G._modelCur._ParaCamera.Gamma;
            SetPara();
        }

        private void numGamma_ValueChanged(object sender, EventArgs e)
        {
           // G._modelCur._ParaCamera.Gamma = (int)numGamma.Value;
          //  trackGamma.Value = G._modelCur._ParaCamera.Gamma;
            SetPara();
        }

        private void trackGain_Scroll(object sender, EventArgs e)
        {
           // G._modelCur._ParaCamera.Gain = trackGain.Value;
           // numGain.Value = G._modelCur._ParaCamera.Gain;
            SetPara();
        }

        private void numGain_ValueChanged(object sender, EventArgs e)
        {
           // G._modelCur._ParaCamera.Gain = (int)numGain.Value;
           // trackGain.Value = G._modelCur._ParaCamera.Gain;
            SetPara();
        }

      

        private void trackExplosure_Scroll(object sender, EventArgs e)
        {
          //  G._modelCur._ParaCamera.Exposure = trackExplosure.Value;
           // numExplosure.Value = G._modelCur._ParaCamera.Exposure;
            SetPara();
        }

        private void numExplosure_ValueChanged(object sender, EventArgs e)
        {
            //G._modelCur._ParaCamera.Exposure = (int)numExplosure.Value;
           // trackExplosure.Value = G._modelCur._ParaCamera.Exposure;
            SetPara();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
           // G._modelCur._ParaCamera = parOld;
            SetValue();
            SetPara();
            this.Visible = false;
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
       //  G.EditImage.SaveOneModel(G._modelCur, G._modelCur.name);
          //  File.Exists(G._modelCur.name);
            this.Visible = false;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            /*Properties.Settings.Default."" = _Bright;
            Properties.Settings.Default.Save();
            Properties.Settings.Default."" = _Contrast;
            Properties.Settings.Default.Save();
            Properties.Settings.Default."" = _Hue;
            Properties.Settings.Default.Save();
            Properties.Settings.Default."" = _Saturation;
            Properties.Settings.Default.Save();
            Properties.Settings.Default."" = _Sharpness;
            Properties.Settings.Default.Save();
            Properties.Settings.Default."" = _Gamma;
            Properties.Settings.Default.Save();
            Properties.Settings.Default."" = _Gain;
            Properties.Settings.Default.Save();
            Properties.Settings.Default."" = _Black;
            Properties.Settings.Default.Save();
            Properties.Settings.Default."" = _Explosure;
            Properties.Settings.Default.Save();*/
        }

        private void SettingCamera_VisibleChanged(object sender, EventArgs e)
        {
            if(this.Visible)
            {
             //   parOld = G._modelCur._ParaCamera.Clone();
            }
            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

       
    }
}
