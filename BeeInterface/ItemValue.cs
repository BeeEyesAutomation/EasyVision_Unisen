using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using BeeGlobal;

namespace BeeInterface
{
    public partial class ItemValue: UserControl
    {
        public ItemValue()
        {
            InitializeComponent();
            cbTypeResults.DataSource = (ValuePLC[])Enum.GetValues(typeof(ValuePLC));
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
