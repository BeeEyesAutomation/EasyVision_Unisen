using BeeGlobal;
using Newtonsoft.Json.Linq;
using OpenCvSharp.Flann;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeeInterface.PLC
{
    public partial class ucValueOutput : UserControl
    {
        ParaValue paraBit=new ParaValue();
        TypeValuePLC TypeValuePLCOld ;
        TypeVar TypeOutputRSOld;
        public ucValueOutput(ParaValue paraBit)
        {
            InitializeComponent();
            this.paraBit = paraBit;
            Type.DataSource = Enum.GetValues(typeof(TypeValuePLC));
            Bit.DataSource = Enum.GetValues(typeof(TypeVar));
            Type.MaxDropDownItems = 10;
            TypeValuePLCOld = this.paraBit.TypeValuePLC;
            Bit.SelectedIndex= Type.FindStringExact(TypeOutputRSOld.ToString());
            Type.SelectedIndex = Type.FindStringExact(TypeValuePLCOld.ToString());
            Type.DataSourceChanged += Type_DataSourceChanged;
            Type.Text = TypeValuePLCOld.ToString();
          lbName.Text="Bit"+ this.paraBit.Adddress;
            this.Value.IsCLick =Convert.ToBoolean(this.paraBit.Value);
            this.Value.Text = this.Value.IsCLick == true ? "True" : "False";
            this.Blink.IsCLick = this.paraBit.IsBlink;
            this.Blink.Text = this.paraBit.IsBlink == true ? "ON":"OFF";
            //this.paraBit.ValueChanged += ParaBit_ValueChanged;
            //this.paraBit.OutputChanged += ParaBit_OutputChanged;
            Type.TabStop = false;
        }

        private void ParaBit_OutputChanged( I_O_Output arg2)
        {
            //OldType = arg2.ToString();
            //Type.SelectedIndex = Type.FindStringExact(OldType);
         
            //Type.Text = OldType;
        }

        private void Type_DataSourceChanged(object sender, EventArgs e)
        {
            //Type.SelectedIndex = Type.FindStringExact(OldType);
            //Type.Text = OldType;
        }

        private void ParaBit_ValueChanged(object arg1, int arg2)
        {
            this.Invoke((Action)(() =>
            {
                this.Value.IsCLick = Convert.ToBoolean(arg2);
                this.Value.Text = this.Value.IsCLick == true ? "True" : "False";
            }));
         
        }

        private void Blink_Click(object sender, EventArgs e)
        {
            this.paraBit.IsBlink = Blink.IsCLick;
            this.Blink.Text = this.paraBit.IsBlink == true ? "ON" : "OFF";
        }

        private void Value_Click(object sender, EventArgs e)
        {
            if (Global.Config.IsExternal)
            {
                Value.IsCLick = !Value.IsCLick;
                MessageBox.Show("Change Mode to Internal!");
                return;
            }
            if (!Global.Comunication.Protocol.IsConnected)
            {
                Value.IsCLick = !Value.IsCLick;
                MessageBox.Show("Please Connect PLC!");
                return;
            }
         //   Global.Comunication.Protocol.WriteOutputBit(paraBit.I_O_Output, Value.IsCLick);
            this.paraBit.Value=Convert.ToInt32( Value.IsCLick);
        }

        private void Type_SelectionChangeCommitted(object sender, EventArgs e)
        {
          
            String name = Type.SelectedValue.ToString();
            //var m = Regex.Match(lbName.Text, @"[+-]?\d+");
            //int value = m.Success ? int.Parse(m.Value) : 0;  // -42
         
            if (name == "") return;
            this.paraBit.TypeValuePLC= (TypeValuePLC)Enum.Parse(typeof(TypeValuePLC), name);
            //       Global.Comunication.Protocol.AddOutPut(value, (I_O_Output)Enum.Parse(typeof(I_O_Output), name, ignoreCase: true), this.paraBit.OldOutPut);

            // OldType = name;

        }

        private void Type_DropDown(object sender, EventArgs e)
        {
            Type.MaxDropDownItems = 10;
        }

        private void Bit_SelectionChangeCommitted(object sender, EventArgs e)
        {
            String name = Bit.SelectedValue.ToString();
            //var m = Regex.Match(lbName.Text, @"[+-]?\d+");
            //int value = m.Success ? int.Parse(m.Value) : 0;  // -42

            if (name == "") return;
            this.paraBit.TypeVar = (TypeVar)Enum.Parse(typeof(TypeVar), name);
        }
    }
}
