using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeCore
{
    public static class BitmapExtensions
    {
        public static bool IsDisposed(this Image img)
        {
            try
            {
                var _ = img.Flags; // thuộc tính đơn giản, không tốn tài nguyên
                return false;
            }
            catch (ObjectDisposedException)
            {
                return true;
            }
        }
    }
}
