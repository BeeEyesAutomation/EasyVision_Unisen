using BeeGlobal;
using System;
using System.Linq;
using System.Windows.Forms;

namespace BeeInterface.PLC
{
    public partial class ucValuePicker : UserControl
    {
        // Quyet dinh list nguon: ValueOut (Write) hoac ValueIn (Read).
        // Default = ValueOut (tool muon ghi ket qua xuong PLC).
        public TypeIO Direction { get; set; } = TypeIO.ValueOut;

        public event Action<string> SelectedNameChanged;

        public string SelectedName
        {
            get => cbName.SelectedItem as string ?? "";
            set
            {
                if (cbName.Items.Contains(value))
                    cbName.SelectedItem = value;
            }
        }

        public ucValuePicker()
        {
            InitializeComponent();
        }

        // Goi tu Form chua khi mo (hoac khi danh sach ParaValue thay doi).
        public void Refresh(string keepName = null)
        {
            cbName.Items.Clear();
            cbName.Items.Add(""); // option de empty - khong push
            var p = Global.Comunication?.Protocol;
            if (p != null)
            {
                var list = (Direction == TypeIO.ValueIn) ? p.ListParaValueInput : p.ListParaValueOut;
                if (list != null)
                {
                    foreach (var name in list.Select(v => v.Name).Where(n => !string.IsNullOrEmpty(n)).Distinct())
                        cbName.Items.Add(name);
                }
            }
            if (!string.IsNullOrEmpty(keepName) && cbName.Items.Contains(keepName))
                cbName.SelectedItem = keepName;
        }

        private void cbName_SelectionChangeCommitted(object sender, EventArgs e)
        {
            SelectedNameChanged?.Invoke(SelectedName);
        }
    }
}
