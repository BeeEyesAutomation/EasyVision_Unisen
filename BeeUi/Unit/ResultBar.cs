using BeeCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeeUi.Unit
{
    public partial class ResultBar : UserControl
    {
        public ResultBar()
        {
            InitializeComponent();
           G.ResultBar = this;
        }

        private void btnResetQty_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn chắc chắn muốn Xóa dữ liệu Sản Xuất", "Xóa Tất cả dữ liệu hôm nay", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                G.Config.SumTime = 0;
                G.Config.SumOK = 0;
                G.Config.SumNG = 0;
                G.Config.TotalTime = 0;
                G.Config.Percent = 0;
                lbTotalTime.Text = "---";
                lbPercent.Text = "---";
                lbSumOK.Text = "---";
                lbSumNG.Text = "---";
                lbStatus.Text = "---";
                lbCycleTrigger.Text = "---";
                lbTimes.Text = "---";
                try
                {
                    String date = DateTime.Now.ToString("yyyyMMdd");

                    if (!File.Exists(Path.Combine(Environment.CurrentDirectory, "Report") + "\\" + date + ".mdf"))
                        return;
                    SqlConnection con = new SqlConnection();
                  
                    G.cnn.Close();
                    String path = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + Path.Combine(Environment.CurrentDirectory, "Report") + "\\" + date + ".mdf" + ";Integrated Security=True;Connect Timeout=30"; ;
                    con = new SqlConnection(path);
                    con.Open();
                    SqlServer sqlServer = new SqlServer();
                    sqlServer.Delete("Report", "Model='" + Properties.Settings.Default.programCurrent.Replace(".prog", "") + "'", con);
                    con.Close();
                    G.cnn.Open();
                    if (Directory.Exists("Report//" + date))
                    {
                        System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo("Report//" + date);
                        if (Directory.Exists("Report//" + date))
                            G.ucReport.Empty(directory);
                    }
                
                  
                }
                catch (Exception ex)
                {

                }
            }
        }

        private void pInfor_SizeChanged(object sender, EventArgs e)
        {
            //BeeCore.CustomGui.RoundRg(pInfor, G.Config.RoundRad);

        }

        private void ResultBar_Load(object sender, EventArgs e)
        {
            if (G.Header == null) return;
            pInforTotal.BackColor = BeeCore.CustomGui.BackColor(TypeCtr.Bar, G.Config.colorGui);
            pStatus.BackColor = BeeCore.CustomGui.BackColor(TypeCtr.Bar, G.Config.colorGui);

            BeeCore.CustomGui.RoundRg(pInforTotal, G.Config.RoundRad);
            G.ResultBar.lbTimes.Text = G.Config.SumTime.ToString();
            G.ResultBar.lbSumOK.Text = G.Config.SumOK + "";
            G.ResultBar.lbSumNG.Text = G.Config.SumNG + "";
            G.ResultBar.lbTotalTime.Text = Math.Round(G.Config.TotalTime, 1) + " min";
            G.ResultBar.lbPercent.Text = Math.Round(G.Config.Percent, 1) + " %";
            G.ResultBar.lbCycleTrigger.Text = "00 ms";

        }

        private void pStatus_SizeChanged(object sender, EventArgs e)
        {
          //  pStatus.ResumeLayout(true);
        }
    }
}
