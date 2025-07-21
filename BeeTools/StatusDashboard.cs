using BeeCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeeInterface
{
    public partial class StatusDashboard : UserControl
    {

        // Data fields
        private int _totalTimes, _okCount, _ngCount;
        private int _cycleTime, _camTime;
        private string _statusText = "OK";

        // Controls
        public RJButton btnReset;

        // Data properties
        [Category("Data"), Description("Tổng số lần")]
        public int TotalTimes { get => _totalTimes; set { _totalTimes = value; Invalidate(); } }
        [Category("Data"), Description("Số OK")]
        public int OkCount { get => _okCount; set { _okCount = value; Invalidate(); } }
        [Category("Data"), Description("Số NG")]
        public int NgCount { get => _ngCount; set { _ngCount = value; Invalidate(); } }
        [Category("Data"), Description("Cycle Time tổng (ms)")]
        public int CycleTime { get => _cycleTime; set { _cycleTime = value; Invalidate(); } }
        [Category("Data"), Description("Cycle Time camera (ms)")]
        public int CamTime { get => _camTime; set { _camTime = value; Invalidate(); } }
        [Category("Data"), Description("Status text ở ô lớn")]
        public string StatusText { get => _statusText; set { _statusText = value; Invalidate(); } }

        [Browsable(false)]
        public float PercentOk => TotalTimes == 0 ? 0 : _okCount * 100f / _totalTimes;

        // Appearance properties
        [Category("Appearance"), Description("Màu nền ô lớn Status")]
        public Color StatusBlockBackColor { get; set; } = Color.Green;
        [Category("Appearance"), Description("Màu nền chung tiêu đề các ô giữa")]
        public Color MidHeaderBackColor { get; set; } = Color.White;
        [Category("Appearance"), Description("Màu nền giá trị ô Total Times")]
        public Color TotalValueBackColor { get; set; } = Color.White;
        [Category("Appearance"), Description("Màu nền giá trị ô OK count")]
        public Color OkCountValueBackColor { get; set; } = Color.White;
        [Category("Appearance"), Description("Màu nền giá trị ô NG count")]
        public Color NgValueBackColor { get; set; } = Color.White;
        [Category("Appearance"), Description("Màu nền ô thông số CT bên phải")]
        public Color InfoBlockBackColor { get; set; } = Color.White;
        [Category("Appearance"), Description("Font cho phần thông số bên phải")]
        public Font InfoFont { get; set; } = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Pixel);

        public StatusDashboard()
        {
            // Styles
            SetStyle(
                ControlStyles.AllPaintingInWmPaint
              | ControlStyles.OptimizedDoubleBuffer
              | ControlStyles.ResizeRedraw
              | ControlStyles.UserPaint, true);

            // Default font and size
            this.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            this.MinimumSize = new Size(240, 50);

            // Reset button
            btnReset = new RJButton
            {
                IsCLick=false,
                IsNotChange=true,
                IsUnGroup=true,
                Margin= new Padding(5),
                Text = "Reset",
                Dock = DockStyle.Right,
                Width = 60
            };
            btnReset.Click += ResetButton_Click;
            Controls.Add(btnReset);
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
         
           
            if (MessageBox.Show("Bạn chắc chắn muốn Xóa dữ liệu Sản Xuất", "Xóa Tất cả dữ liệu hôm nay", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                TotalTimes = 0;
                OkCount = 0;
                NgCount = 0;
                CycleTime = 0;
                CamTime = 0;
                try
                {
                    String date = DateTime.Now.ToString("yyyyMMdd");

                    if (!File.Exists(Path.Combine(Environment.CurrentDirectory, "Report") + "\\" + date + ".mdf"))
                        return;
                    SqlConnection con = new SqlConnection();

                    //G.cnn.Close();
                    //String path = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + Path.Combine(Environment.CurrentDirectory, "Report") + "\\" + date + ".mdf" + ";Integrated Security=True;Connect Timeout=30"; ;
                    //con = new SqlConnection(path);
                    //con.Open();
                    //SqlServer sqlServer = new SqlServer();
                    //sqlServer.Delete("Report", "Model='" + Properties.Settings.Default.programCurrent.Replace(".prog", "") + "'", con);
                    //con.Close();
                    //G.cnn.Open();
                    //if (Directory.Exists("Report//" + date))
                    //{
                    //    System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo("Report//" + date);
                    //    if (Directory.Exists("Report//" + date))
                    //        G.ucReport.Empty(directory);
                    //}


                }
                catch (Exception ex)
                {

                }
                this.Refresh();
            }

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Exclude reset button area
            int btnW = btnReset.Width;
            int w = Width - btnW;
            int h = Height;
            if (w <= 0 || h <= 0) return;

            // Layout
            int bigW = (int)(w * 0.14f);
            int rightW = (int)(w * 0.12f);
            int midW = (w - bigW - rightW) / 3;
            if (bigW <= 0 || midW <= 0 || rightW <= 0) return;
           
            int remain = w - bigW - rightW;
            if (remain <= 0) return;

            int totalRatio = 50 + 30 + 20;
            int w1 = remain * 50 / totalRatio;
            int w2 = remain * 30 / totalRatio;
            int w3 = remain - w1 - w2;

            var rBig = new Rectangle(0, 0, bigW, h);
            var r1 = new Rectangle(rBig.Right, 0, w1, h);
            var r2 = new Rectangle(r1.Right, 0, w2, h);
            var r3 = new Rectangle(r2.Right, 0, w3, h);
            var rR = new Rectangle(r3.Right, 0, rightW, h);

            DrawBlock(g, rBig, StatusBlockBackColor, clBoder, StatusText, null, Color.White);

            var headers = new[] { "Total Times", "OK", "NG" };
            var values = new[] { TotalTimes.ToString(), OkCount.ToString(), NgCount.ToString() };
            var valueBgs = new[] { TotalValueBackColor, OkCountValueBackColor, NgValueBackColor };
            var fgCols = new[] { Color.Blue, Color.Green, Color.Red };
            var rects = new[] { r1, r2, r3 };
            for (int i = 0; i < 3; i++)
                DrawMidBlock(g, rects[i], headers[i], values[i], valueBgs[i], fgCols[i]);
            // Right info
            DrawRightInfo(g, rR);
        }
        Color clBoder = Color.LightGray;
        int ThinessBoder = 3;
        // Drawing helpers (unchanged): DrawMidBlock, DrawBlock, DrawRightInfo, GetRoundedRect, GetFittingFont
        // ... (same as before)
        private void DrawMidBlock(Graphics g, Rectangle r, string header, string value, Color valueBackColor, Color valColor)
        {
            int headerH = (int)(r.Height * 0.3f);
            var rH = new Rectangle(r.X, r.Y, r.Width, headerH);
            var rC = new Rectangle(r.X, r.Y + headerH, r.Width, r.Height - headerH);

            using (var bgH = new SolidBrush(MidHeaderBackColor)) g.FillRectangle(bgH, rH);
            using (var bgC = new SolidBrush(valueBackColor)) g.FillRectangle(bgC, rC);

            using (var pen = new Pen(clBoder, ThinessBoder))
            {
                g.DrawRectangle(pen, r.X, r.Y, r.Width - 1, r.Height - 1);
                g.DrawLine(pen, r.X, rH.Bottom, r.Right - 1, rH.Bottom);
            }

            using (var fH = GetFittingFont(g, header, this.Font.FontFamily, FontStyle.Bold, rH.Size))
            using (var sfH = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                g.DrawString(header, fH, Brushes.Black, rH, sfH);

            //int radius = Math.Min(rC.Width, rC.Height) / 8;
            //using (var path = GetRoundedRect(rC, radius))
            //using (var penR = new Pen(Color.Gray))
            //    g.DrawPath(penR, path);

            using (var fV = GetFittingFont(g, value, this.Font.FontFamily, FontStyle.Bold, rC.Size))
            using (var sfV = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            using (var fg = new SolidBrush(valColor))
                g.DrawString(value, fV, fg, rC, sfV);
        }

        private void DrawBlock(Graphics g, Rectangle r, Color backColor, Color borderColor, string text, Font fontOverride, Color textColor)
        {
            using (var bg = new SolidBrush(backColor)) g.FillRectangle(bg, r);
            using (var pen = new Pen(borderColor, ThinessBoder)) g.DrawRectangle(pen, r.X, r.Y, r.Width - 1, r.Height - 1);
            var f = fontOverride ?? GetFittingFont(g, text, this.Font.FontFamily, FontStyle.Bold, r.Size);
            using (var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            using (var br = new SolidBrush(textColor))
                g.DrawString(text, f, br, r, sf);
        }

        private void DrawRightInfo(Graphics g, Rectangle r)
        {
            using (var bg = new SolidBrush(InfoBlockBackColor)) g.FillRectangle(bg, r);
            using (var pen = new Pen(Color.Gray)) g.DrawRectangle(pen, r.X, r.Y, r.Width - 1, r.Height - 1);
            int pad = 4;
            var rIn = new Rectangle(r.X + pad, r.Y + pad, r.Width - pad * 2, r.Height - pad * 2);
            var lines = new[]
            {
            $"CT      {CycleTime} ms",
            $"CT cam  {CamTime} ms",
            $"% OK    {PercentOk:0.0} %"
        };
            int lineH = rIn.Height / lines.Length;
            for (int i = 0; i < lines.Length; i++)
            {
                var cell = new Rectangle(rIn.X, rIn.Y + i * lineH, rIn.Width, lineH);
                using (var sf = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center })
                    g.DrawString(lines[i], InfoFont, Brushes.Black, cell, sf);
            }
        }

        // Helper: create rounded rectangle path
        private GraphicsPath GetRoundedRect(Rectangle bounds, int radius)
        {
            int d = radius * 2;
            var path = new GraphicsPath();
            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            path.AddLine(bounds.X + radius, bounds.Y, bounds.Right - radius, bounds.Y);
            path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
            path.AddLine(bounds.Right, bounds.Y + radius, bounds.Right, bounds.Bottom - radius);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddLine(bounds.Right - radius, bounds.Bottom, bounds.X + radius, bounds.Bottom);
            path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
            path.AddLine(bounds.X, bounds.Bottom - radius, bounds.X, bounds.Y + radius);
            path.CloseFigure();
            return path;
        }

        // Helper: auto-fit font
        private Font GetFittingFont(Graphics g, string text, FontFamily fam, FontStyle style, Size boundingSize)
        {
            float em = boundingSize.Height;
            for (; em > 4; em -= 0.5f)
            {
                using (var f = new Font(fam, em, style, GraphicsUnit.Pixel))
                {
                    var sz = g.MeasureString(text, f);
                    if (sz.Width <= boundingSize.Width && sz.Height <= boundingSize.Height)
                        return new Font(fam, em, style, GraphicsUnit.Pixel);
                }
            }
            return new Font(fam, 4, style, GraphicsUnit.Pixel);
        }
    }
    }