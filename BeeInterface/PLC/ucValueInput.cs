using BeeGlobal;
using System;
using System.Windows.Forms;

namespace BeeInterface.PLC
{
    public partial class ucValueInput : UserControl
    {
        ParaValue paraBit = new ParaValue();

        public ucValueInput(ParaValue paraBit)
        {
            InitializeComponent();
            this.paraBit = paraBit;

            Type.DataSource = Enum.GetValues(typeof(TypeValuePLC));
            Bit.DataSource = Enum.GetValues(typeof(TypeVar));
            Type.MaxDropDownItems = 10;
            Bit.MaxDropDownItems = 10;

            lbName.Text = string.IsNullOrEmpty(this.paraBit.Name) ? "Value" : this.paraBit.Name;
            Type.SelectedItem = this.paraBit.TypeValuePLC;
            Bit.SelectedItem = this.paraBit.TypeVar;
            txtAdd.Text = this.paraBit.Adddress ?? "";

            this.Value.Text = Convert.ToString(this.paraBit.Value ?? 0);
            this.Value.Enabled = false; // Input: read-only display, engine update
            this.Blink.IsCLick = this.paraBit.IsBlink;
            this.Blink.Text = this.paraBit.IsBlink ? "ON" : "OFF";

            Type.TabStop = false;

            this.paraBit.ValueChanged -= ParaBit_ValueChanged;
            this.paraBit.ValueChanged += ParaBit_ValueChanged;
        }

        private void ParaBit_ValueChanged(object arg1, dynamic arg2)
        {
            if (this.IsDisposed) return;
            try
            {
                this.BeginInvoke((Action)(() =>
                {
                    this.Value.Text = Convert.ToString(arg2 ?? 0);
                }));
            }
            catch { }
        }

        private void Blink_Click(object sender, EventArgs e)
        {
            this.paraBit.IsBlink = Blink.IsCLick;
            this.Blink.Text = this.paraBit.IsBlink ? "ON" : "OFF";
        }

        private void Value_Click(object sender, EventArgs e)
        {
            // Input: value cap nhat tu engine, click khong lam gi
        }

        private void Type_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (Type.SelectedItem == null) return;
            this.paraBit.TypeValuePLC = (TypeValuePLC)Type.SelectedItem;
            var def = TypeValuePLCMap.DefaultTypeVar(this.paraBit.TypeValuePLC);
            if (this.paraBit.TypeVar != def)
            {
                this.paraBit.TypeVar = def;
                Bit.SelectedItem = def;
            }
        }

        private void Type_DropDown(object sender, EventArgs e)
        {
            Type.MaxDropDownItems = 10;
        }

        private void Bit_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (Bit.SelectedItem == null) return;
            this.paraBit.TypeVar = (TypeVar)Bit.SelectedItem;
        }

        private void lbName_TextChanged(object sender, EventArgs e)
        {
            this.paraBit.Name = lbName.Text.Trim();
        }

        private void txtAdd_TextChanged(object sender, EventArgs e)
        {
            this.paraBit.Adddress = txtAdd.Text.Trim();
        }
    }
}
