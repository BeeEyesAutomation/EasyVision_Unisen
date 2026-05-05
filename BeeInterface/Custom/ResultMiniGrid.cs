using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BeeInterface
{
    public partial class ResultMiniGrid : UserControl
    {
        private Color _okColor = Color.LimeGreen;
        private Color _ngColor = Color.OrangeRed;

        public ResultMiniGrid()
        {
            InitializeComponent();
        }

        [Category("Appearance")]
        public Color OkColor
        {
            get { return _okColor; }
            set
            {
                _okColor = value;
                RefreshStatusColors();
            }
        }

        [Category("Appearance")]
        public Color NgColor
        {
            get { return _ngColor; }
            set
            {
                _ngColor = value;
                RefreshStatusColors();
            }
        }

        public void SetRow(string name, object value, bool? isOk = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                return;

            DataGridViewRow row = FindRow(name);
            if (row == null)
            {
                int index = grid.Rows.Add();
                row = grid.Rows[index];
                row.Cells[colName.Index].Value = name;
            }

            row.Cells[colValue.Index].Value = value == null ? string.Empty : value.ToString();
            row.Cells[colStatus.Index].Value = FormatStatus(isOk);
            row.Tag = isOk;
            ApplyStatusColor(row, isOk);
        }

        public void Clear()
        {
            grid.Rows.Clear();
        }

        private DataGridViewRow FindRow(string name)
        {
            foreach (DataGridViewRow row in grid.Rows)
            {
                object value = row.Cells[colName.Index].Value;
                if (value != null && string.Equals(value.ToString(), name, StringComparison.OrdinalIgnoreCase))
                    return row;
            }

            return null;
        }

        private static string FormatStatus(bool? isOk)
        {
            if (!isOk.HasValue)
                return string.Empty;

            return isOk.Value ? "OK" : "NG";
        }

        private void RefreshStatusColors()
        {
            foreach (DataGridViewRow row in grid.Rows)
            {
                bool? isOk = row.Tag as bool?;
                ApplyStatusColor(row, isOk);
            }
        }

        private void ApplyStatusColor(DataGridViewRow row, bool? isOk)
        {
            if (!isOk.HasValue)
            {
                row.Cells[colStatus.Index].Style.BackColor = grid.DefaultCellStyle.BackColor;
                row.Cells[colStatus.Index].Style.ForeColor = grid.DefaultCellStyle.ForeColor;
                return;
            }

            row.Cells[colStatus.Index].Style.BackColor = isOk.Value ? _okColor : _ngColor;
            row.Cells[colStatus.Index].Style.ForeColor = Color.White;
        }
    }
}
