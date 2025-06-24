using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeeUi.Commons
{
    [Serializable()]
    public partial class ShowEraser : Form
    {
        public ShowEraser()
        {
            InitializeComponent();
        }

        private void btnHide_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void ShowEraser_Load(object sender, EventArgs e)
        {
          
        }
    }
}
