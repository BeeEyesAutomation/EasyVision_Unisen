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
    public partial class ucBitInput : UserControl
    {
        ParaBit paraBit=new ParaBit();
        String OldType = "";
        public ucBitInput(ParaBit paraBit)
        {
            InitializeComponent();
            this.paraBit = paraBit;
            Type.DataSource = Enum.GetValues(typeof(I_O_Input));
            OldType = this.paraBit.I_O_Input.ToString();
            Type.SelectedIndex= Type.FindStringExact(OldType);
            Type.DataSourceChanged += Type_DataSourceChanged;
            Type.Text = OldType;
           lbName.Text="Bit"+ this.paraBit.Adddress;
            this.Value.IsCLick =Convert.ToBoolean(this.paraBit.Value);
            this.Value.Text = this.Value.IsCLick == true ? "True" : "False";
            this.Blink.IsCLick = this.paraBit.IsBlink;
            this.Blink.Text = this.paraBit.IsBlink == true ? "ON":"OFF";
            this.paraBit.ValueChanged += ParaBit_ValueChanged;
            this.paraBit.InputChanged += ParaBit_InputChanged;
            Type.TabStop = false;
        }

        private void ParaBit_InputChanged(I_O_Input obj)
        {
            OldType = obj.ToString();
            Type.SelectedIndex = Type.FindStringExact(OldType);

            Type.Text = OldType;
            Type.TabStop = false;
        }

   

        private void Type_DataSourceChanged(object sender, EventArgs e)
        {
            Type.SelectedIndex = Type.FindStringExact(OldType);
            Type.Text = OldType;
            Type.TabStop = false;
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
            if(!Global.Comunication.Protocol.IsConnected)
            {
                Value.IsCLick = !Value.IsCLick;
                MessageBox.Show("Please Connect PLC!");
                return;
            }    
            Global.Comunication.Protocol.WriteInput(paraBit.I_O_Input, Value.IsCLick);
            this.paraBit.Value=Convert.ToInt32( Value.IsCLick);
        }

        private void Type_SelectionChangeCommitted(object sender, EventArgs e)
        {
          
            String name = Type.SelectedValue.ToString();
            var m = Regex.Match(lbName.Text, @"[+-]?\d+");
            int value = m.Success ? int.Parse(m.Value) : 0;  // -42
            this.paraBit.OldInPut = this.paraBit.I_O_Input;
            if (name == "") return;
            //if (name.Contains("None"))
            //{
            //   if (OldType == null) OldType = "None";
            //    Global.Comunication.Protocol.RemoveOutPut(value, (I_O_Output)Enum.Parse(typeof(I_O_Output), OldType, ignoreCase: true));

            //}
            //else
                Global.Comunication.Protocol.AddInPut(value, (I_O_Input)Enum.Parse(typeof(I_O_Input), name, ignoreCase: true), this.paraBit.OldInPut);
          
            OldType = name;

        }

        private void ucBitInput_Load(object sender, EventArgs e)
        {
            //this.ActiveControl = null;
        }
    }
}
