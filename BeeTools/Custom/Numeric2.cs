using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeeInterface
{
    [Serializable()]
    public partial class Numeric2 : UserControl
    {
        public Numeric2()
        {
            InitializeComponent();
        }
        [Category("RJ Code Advance")]
        public int maxnimum = 100; public int minimum = 0;
        private int _value = 0;
        [Category("Maxnimum")]
        public int Maxnimum
        {
            get {
                return maxnimum; }
            set
            {
                maxnimum = value;
                if (_value > maxnimum) Value = maxnimum;


            }
        }
        [Category("Minimum")]
        public int Minimum
        {
            get {
                return minimum; }
            set
            {
                minimum = value;
                if (_value < minimum) Value = minimum;
               

            }
        }
        [Category("Value")]
        public int Value
        {
            get {
                return _value; }///lay
            set//gasn
            {
                _value = value;
                if (_value < minimum) _value = minimum;
                if (_value > maxnimum) _value = maxnimum;
                txt.Text = _value + "";
               
                
            }
        }

        private void btnSub_Click(object sender, EventArgs e)
        {
            Value -= 10;
          
        }

        private void btnPlus_Click(object sender, EventArgs e)
        {
            Value += 10;
           
        }

        private void txt_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
