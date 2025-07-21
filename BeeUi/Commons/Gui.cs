using BeeCore;
using BeeGlobal;
using BeeInterface;
using BeeUi.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Windows.Forms;
using Control = System.Windows.Forms.Control;
using UserControl = System.Windows.Forms.UserControl;

namespace BeeUi.Commons
{
    public class Gui
    {
        public static  void RefreshColor(Color color,int AlphaBar= 80, int AlphaMenu= 150, int AlphaBg=20,int AlphaText=40)
        {
            G.EditTool.LayoutEnd.BackColor = Color.White;
           // G.EditTool.pMain.BackColor = Color.FromArgb(AlphaBg, color.R, color.G, color.B);
            G.EditTool.BackColor = Color.FromArgb(AlphaBg, color.R, color.G, color.B);
            G.EditTool.View.BackColor = Color.FromArgb(AlphaBg, color.R, color.G, color.B);
            G.EditTool.pHeader.BackColor = Color.FromArgb(190, color.R, color.G, color.B);
            G.Header.BackColor =  Color.FromArgb(AlphaBg, color.R,color.G,color.B);
            // G.Header.pPO.BackColor = Color.FromArgb(AlphaBar, color.R, color.G, color.B);
            //G.Header.pModel.BackColor = Color.FromArgb(AlphaBar, color.R, color.G, color.B);
            //G.Header.pCamera.BackColor = Color.FromArgb(AlphaBar, color.R, color.G, color.B);
            G.StatusDashboard.InfoBlockBackColor = Color.FromArgb(AlphaBar, color.R, color.G, color.B);
            G.StatusDashboard.StatusBlockBackColor= Color.FromArgb(AlphaBar-50, color.R, color.G, color.B);
            G.StatusDashboard.MidHeaderBackColor= Color.FromArgb(AlphaBar, color.R, color.G, color.B);
            //G.StepEdit.BackColor = Color.FromArgb(AlphaBar, color.R, color.G, color.B);
          //   G.StatusDashboard.picChart.BackColor = Color.FromArgb(AlphaText, color.R, color.G, color.B);
           // G.EditTool.View.pTool.BackColor = Color.FromArgb(AlphaMenu, color.R, color.G, color.B);
            Global.ToolSettings.pBtn.BackColor = Color.FromArgb(AlphaMenu, color.R, color.G, color.B);
            foreach (Control c in G.Header.Controls)
            {
                if (c is RJButton)
                {
                    RJButton btn = c as RJButton;
                   // btn.BorderRadius = 8;
                    btn.BorderColor = btn.Parent.BackColor;
                    btn.BorderSize = 1;
                    btn.Invalidate();
                }
                else if (c is Panel|| c.GetType()== typeof( UserControl))
                {
                  
                    foreach (Control c1 in c.Controls)
                    {

                        if (c1 is RJButton)
                        {
                            RJButton btn = c1 as RJButton;
                       
                            btn.BorderRadius =8;
                            btn.BorderColor = btn.Parent.BackColor;
                           // btn.BorderSize = 1;
                            btn.Invalidate();
                        }
                        //else if (c1 is Label)
                        //    c1.BackColor = c1.Parent.BackColor;
                        else if (c1 is Panel || c.GetType() == typeof(UserControl))
                        {
                         //   c.BackColor=c.Parent.BackColor;
                            foreach (Control c2 in c1.Controls)
                            {
                                if (c2 is RJButton)
                                {
                                    RJButton btn = c2 as RJButton;
                                    btn.BorderRadius = 8;
                                    btn.BorderColor = btn.Parent.BackColor;
                                    //btn.BorderSize = 1;
                                    btn.Invalidate();
                                }
                              //  c2.BackColor = c1.BackColor;
                            }
                        }
                    }
                }
            }
        }
        public static void RefreshRadius( int radius)
        {
           // G.Header.pPO.Region = System.Drawing.Region.FromHrgn(Draws.CreateRoundRectRgn(0, 0, G.Header.pPO.Width, G.Header.pPO.Height, radius, radius));
            //G.Header.pModel.Region = System.Drawing.Region.FromHrgn(Draws.CreateRoundRectRgn(0, 0, G.Header.pModel.Width, G.Header.pModel.Height, radius, radius));
           // G.Header.pCamera.Region = System.Drawing.Region.FromHrgn(Draws.CreateRoundRectRgn(0, 0, G.Header.pCamera.Width, G.Header.pCamera.Height, radius, radius));
         //   G.EditTool.View.pTool.Region = System.Drawing.Region.FromHrgn(Draws.CreateRoundRectRgn(0, 0, G.EditTool.View.pTool.Width, G.EditTool.View.pTool.Height, radius, radius));
        
            Global.ToolSettings.pBtn.Region = System.Drawing.Region.FromHrgn(Draws.CreateRoundRectRgn(0, 0, Global.ToolSettings.pBtn.Width, Global.ToolSettings.pBtn.Height, radius, radius));
        }
    }
}
