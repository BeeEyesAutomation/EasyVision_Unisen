using BeeCpp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeCore.Func
{
    public static class CodeSymbologyCliExtensions
    {
        // 1D: linear (kể cả các biến thể stacked của RSS vẫn xem là 1D)
        private static readonly HashSet<CodeSymbologyCli> OneD = new HashSet<CodeSymbologyCli>()
    {
        CodeSymbologyCli.EAN8,
        CodeSymbologyCli.EAN13,
        CodeSymbologyCli.UPC_A,
        CodeSymbologyCli.UPC_E,
        CodeSymbologyCli.Code39,
        CodeSymbologyCli.Code93,
        CodeSymbologyCli.Code128,
        CodeSymbologyCli.ITF,
        CodeSymbologyCli.Codabar,
        CodeSymbologyCli.RSS14,
        CodeSymbologyCli.RSSExpanded
    };

        // 2D
        private static readonly HashSet<CodeSymbologyCli> TwoD = new HashSet<CodeSymbologyCli>()
    {
        CodeSymbologyCli.QR,
        CodeSymbologyCli.MicroQR,
        CodeSymbologyCli.DataMatrix,
        CodeSymbologyCli.Aztec,
        CodeSymbologyCli.MaxiCode,
        CodeSymbologyCli.PDF417,
        CodeSymbologyCli.MicroPDF417
    };

        public static bool Is1D(this CodeSymbologyCli s) => OneD.Contains(s);

        public static bool Is2D(this CodeSymbologyCli s) => TwoD.Contains(s);

        /// <summary>
        /// Trả về 1 nếu 1D, 2 nếu 2D, 0 nếu Unknown/không xác định.
        /// </summary>
        public static int GetDimension(this CodeSymbologyCli s)
            => s.Is1D() ? 1 : (s.Is2D() ? 2 : 0);
    }

}
