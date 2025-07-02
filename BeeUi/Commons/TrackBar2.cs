using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Point = System.Drawing.Point;

namespace BeeUi
{
    [Serializable()]
    public partial class TrackBar2 : UserControl
    {
        public TrackBar2()
        {
            InitializeComponent();
            
            //btnTick.Parent = this.pT;
        }
        private float valueScore;
        [Category("Min")]
        private float min;
        private float max=100;
        private float value;
        private float step=1;
        private void label2_Click(object sender, EventArgs e)
        {

        }
        bool IsDown;
        private void Tick_MouseDown(object sender, MouseEventArgs e)
        {
            IsDown = true;
        }

        private void Tick_MouseLeave(object sender, EventArgs e)
        {
            imgTick = Properties.Resources.BID_SLIDER_HANDLE_ON_NORMAL;
        }

        private void Tick_MouseUp(object sender, MouseEventArgs e)
        {
            if(IsDown)
            {
                if (ValueChanged != null) ValueChanged(Value);
            }    
        
            IsDown = false;
        }
      
       

        public float Min { get => min; set => min = value; }
        public float Max { get => max; set => max = value; }
        public float Value { get => value;
            set {
                if (Max == Min) Max++;
                if (value > Max) value = Max;
                if (value < Min) value = Min;

                this.value = value;
                pTick = new Point((int)((value *1.0/ (Max-Min))*(this.pT.Width- imgTick.Width-4)), 5);
                // lbValue.Location = new Point(pTick.X + imgTick.Width / 2 - lbValue.Width / 2, lbValue.Location.Y);
                // lbValue.Text = this.value + "";
                pT.Invalidate();

            } }
        public event Action<float> ValueChanged;
        
        public float Step { get => step; set => step = value; }
        public float ValueScore
        {
            get => valueScore; set
            {
                
             if(value!=valueScore)
                {

                    valueScore = value- value%step;
                    pT.Invalidate();
                }
            }
        
        }

        public Color ColorTrack { get => colorTrack; set { 
                if(value!=colorTrack)
                {
                   // bmpBase = new Bitmap(pT.Width, pT.Height);
                    colorTrack = value;
                    this.Invalidate();
                    //gcBase = Graphics.FromImage(bmpBase);
                    //Rectangle rect = new Rectangle(0, 0, pT.Width - 2, (int)(pT.Height / 3.5));
                    //gcBase.FillRectangle(new SolidBrush(colorTrack), rect);
                    //gcBase.DrawImage(Properties.Resources.BID_SLIDER_SCALE_8PIX_W303, new Rectangle(0, (int)(pT.Height / 3.5), pT.Width - 2, pT.Height / 3));


                    //pT.Image = bmpBase;
                    //if(bmBase!=null)
                    //bmBase.Dispose();
                }    
                } }

        private void Tick_MouseMove(object sender, MouseEventArgs e)
        {
            if(IsDown)
            {
                imgTick = Properties.Resources.BID_SLIDER_HANDLE_ON_DOWN;
                Point pointPanel = e.Location;
                if (pointPanel.X >= imgTick.Width/2 +2&& pointPanel.X <= pT.Width- imgTick.Width/2-2 )
                {
                   Value = (float)Math.Round( (float)((pointPanel.X- imgTick.Width / 2-2) / ((pT.Width - imgTick.Width-4) * 1.0) * (Max-Min)),1);
                    Value = Value- Value % Step;
                }
                
            }
            else
                imgTick = Properties.Resources.BID_SLIDER_HANDLE_ON_ROLLOVER;
        }

        private void imgTrack_MouseClick(object sender, MouseEventArgs e)
        {
           
            if(e.Location.X> pTick.X)
                Value += Step;
            else
                Value -= Step;
        }

        private void pTrack_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Location.X > pTick.X)
                Value += Step;
            else
                Value -= Step;
        }

        private void pTrack_BackColorChanged(object sender, EventArgs e)
        {
           
        }

        private void pTrack_Paint(object sender, PaintEventArgs e)
        {
            }
        public Point pTick;
        private Color colorTrack=Color.Gray;
        private Image imgTick = Properties.Resources.BID_SLIDER_HANDLE_ON_NORMAL;
      public Graphics DrawBase(Graphics gc)
        {
            Rectangle rect = new Rectangle(0, 0, pT.Width, (int)(pT.Height / 3.5));
            gc.FillRectangle(new SolidBrush(colorTrack), rect);
            gc.DrawImage(Properties.Resources.BID_SLIDER_SCALE_8PIX_W303, new Rectangle(0, (int)(pT.Height / 3.5), pT.Width, pT.Height / 3));
            return gc;
        }
        Graphics gcBase;
        Bitmap bmBase;
        bool IsLoadBase = false;
        private void pT_Paint(object sender, PaintEventArgs e)
        {

            if (max == min) max++;
            int LocalValue = (int)((valueScore / ((max - min) * 1.0)) * (pT.Width -2));
            // int w = imgTick.Width;
            Image imgBar = Properties.Resources.BID_SLIDER_SCALE_8PIX_W303;
            Rectangle rect = new Rectangle(0, 0, pT.Width - 2, (int)(pT.Height / 3.5));

            e.Graphics.FillRectangle(new SolidBrush(colorTrack), rect);

            e.Graphics.DrawImage(imgBar, new Rectangle(0, (int)(pT.Height / 3.5), pT.Width - 2, imgBar.Height));

            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(100, 255, 255, 255)), new RectangleF(0, 0, LocalValue , (int)(pT.Height / 3.5)));

            e.Graphics.DrawImage(imgTick, pTick);
         SizeF sz=   e.Graphics.MeasureString(value + "", new Font("Arial", 11));
           
            e.Graphics.DrawString(value + "", new Font("Arial", 11), Brushes.Black, new Point(pTick.X+imgTick.Width/2-(int)sz.Width/2,pTick.Y+imgTick.Height));
           
        }
        Bitmap bmpBase;
        private void TrackBar2_Load(object sender, EventArgs e)
        {
            bmpBase = new Bitmap(pT.Width, pT.Height);
            gcBase = Graphics.FromImage(bmpBase);
            //Rectangle rect = new Rectangle(0, 0, pT.Width-2, (int)(pT.Height / 3.5));
            //gcBase.FillRectangle(new SolidBrush(colorTrack), rect);
            //gcBase.DrawImage(Properties.Resources.BID_SLIDER_SCALE_8PIX_W303, new Rectangle(0, (int)(pT.Height / 3.5), pT.Width-2, pT.Height / 3));
           

            //pT.Image = bmpBase;
        }

        private void pT_SizeChanged(object sender, EventArgs e)
        {
           
        }

        private void TrackBar2_SizeChanged(object sender, EventArgs e)
        {
            this.Invalidate();
        }
    }
}
