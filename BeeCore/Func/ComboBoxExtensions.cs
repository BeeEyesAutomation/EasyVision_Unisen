using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeeCore.Func
{
    public static class ComboBoxExtensions
    {
        public static void SafeSelectIndex(this ComboBox cb, int index) =>
            cb.SelectedIndex = (index >= 0 && index < cb.Items.Count) ? index : -1;
    }
}
