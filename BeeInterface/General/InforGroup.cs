using BeeGlobal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeeInterface.General
{
    public partial class InforGroup : UserControl
    {
        public  Results Results
        {
            get => _Result;
            set
            {
               
                    _Result = value;
                 switch(_Result)
                    {
                        case Results.OK:
                            lbStatus.Text = "OK";
                            lbStatus.ForeColor = Global.ColorOK;
                            break;
                        case Results.NG:
                            lbStatus.Text = "NG";
                            lbStatus.ForeColor = Global.ColorNG;
                            break;
                        case Results.None:
                            lbStatus.Text = "---";
                            lbStatus.ForeColor = Global.ColorNone;
                            break;
                           
                    }
                  
                
            }
        }
      public  Results _Result = Results.OK;
        public InforGroup()
        {
            InitializeComponent();
            
        }
    }
}
