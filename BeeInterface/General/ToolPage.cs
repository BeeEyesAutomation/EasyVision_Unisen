 using BeeCore;
using BeeCore.Funtion;
using BeeGlobal;
using BeeInterface;
using Newtonsoft.Json.Linq;
using OpenCvSharp.Dnn;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Windows.Forms;

using UserControl = System.Windows.Forms.UserControl;

namespace BeeInterface
{
    public partial class ToolPage : UserControl
    {
        public ToolPage()
        {
            InitializeComponent();
        }
        //23
        public RJButton NewRadioButton(ItemNew item, int y)
        {
            RJButton btn = new RJButton();
            btn.Name = item.TypeTool.ToString();
          btn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            btn.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            if (IsRect)
            {
                btn.IsCLick = true;
             //   btn.BackgroundImage = global::BeeUi.Properties.Resources.btnChoose1;
                IsRect = false;
            }
           // else
            //    btn.BackgroundImage = global::BeeUi.Properties.Resources.btnUnChoose;
            btn.BorderColor = System.Drawing.Color.Silver;
          btn.BorderRadius = 5;
           btn.BorderSize = 1;
            btn.AutoFont = true;
            btn.AutoFontMin = 10;
            btn.AutoFontWidthRatio = 1;
            btn.AutoFontHeightRatio = 0.8f;
            btn.ImageTextSpacing = 1;
            btn.ImageAlign = ContentAlignment.MiddleLeft;
            btn.AutoImageMaxRatio = 0.8f;
           btn.FlatAppearance.BorderSize = 0;
           btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
           btn.ForeColor = System.Drawing.Color.White;

            btn.Image = item.Icon;
          
           
           
                btn.Text = item.TypeTool.ToString();

            btn.Location = new System.Drawing.Point(25, y);
          //  if (!btn.Text.Contains("Color Area"))
            //    btn.Enabled = false;
           btn.Size = new System.Drawing.Size(277, 50);
          btn.TabIndex = 0;
            btn.Click += Btn_Click;
           btn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
           btn.TextColor = System.Drawing.Color.Black;
          btn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
         btn.UseVisualStyleBackColor = false;
        
            return btn;
        }
      public  TypeTool TypeTool;

        private void Btn_Click(object sender, EventArgs e)
        {
            RJButton btn = sender as RJButton;
      
         int index=   Global.itemNews.FindIndex(a => a.btn == btn);
            if(index>-1)
            {
                TypeTool = Global.itemNews[index].TypeTool;// (TypeTool)Enum.Parse(typeof(TypeTool), , true);
              
                lbName.Text = TypeTool.ToString();
                lbContent.Text = Global.itemNews[index].Content;
                img.Image = Global.itemNews[index].IconContent;// (Image)Properties.Resources.ResourceManager.GetObject("content"+ nameBtn);
            }
        }

        public List<string> GetListTool()
        {
            return Enum.GetNames(typeof(TypeTool)).ToList();

        }
        //
   
        int lenMax = 0;
       
        bool IsRect = true;
       
        public void LoadGuiAllTool()
        {
            int y= 23; int y2 = 23; int y3 = 23;
            int space = 60;
         
            foreach(ItemNew itemNew in Global.itemNews)
            {
                String name = itemNew.TypeTool.ToString();
                GroupTool groupTool = itemNew.GroupTool;
                RJButton btn = new RJButton();
                switch (groupTool)
                {
                    case GroupTool.Basic_Tool:

                        btn = NewRadioButton(itemNew, y);
                        y += space;
                        btn.Parent = Basic_Tool;
                        //if (tool.TypeTool != TypeTool.Position_Adjustment&& tool.TypeTool != TypeTool.Pattern && tool.TypeTool != TypeTool.MatchingShape)
                        //    btn.Enabled = false;
                        break;
                    case GroupTool.Extra_Tool_1:
                        btn = NewRadioButton(itemNew, y2);
                        // btn.Enabled = false;
                        y2 += space;
                        btn.Parent = Extra_Tool_1;
                        // btn.Enabled = false;
                        break;
                    case GroupTool.Extra_Tool_2:

                        btn = NewRadioButton(itemNew, y3);
                        //btn.Enabled = false;
                        y3 += space;
                        btn.Parent = Extra_Tool_2;
                        // btn.Enabled = false;
                        break;

                }
                btn.Enabled = itemNew.IsEn;
                btn.IsUnGroup = false;
                btn.IsTouch = false;
                itemNew.btn = btn;

            }
         
        }

        private void btn_Click(object sender, EventArgs e)
        {

        }

        private void ToolPage_Load(object sender, EventArgs e)
        {
         
            LoadGuiAllTool();
        }

        private void tabTool_SelectedIndexChanged(object sender, EventArgs e)
        {

            TabPage tab= tabTool.SelectedTab;
          
                GroupTool group = (GroupTool)Enum.Parse(typeof(GroupTool), tab.Name, true);
            int index =Global.itemNews.FindIndex(a => a.GroupTool == group);
            if(index>-1)
            {
                RJButton rJButton =Global.itemNews[index].btn as RJButton;
                
                rJButton.PerformClick();
                //foreach (Control c in tab.Controls)
                //{
                //    RJButton rJButton = c as RJButton;
                //    if (rJButton.Name.Contains("btn" + G.listItool[index].TypeTool.ToString()))
                //    {
                //        RJButton rJButton= G.listItool[index].Control;
                //    }    
                //    else
                //    {
                //        rJButton.IsCLick = false;
                //    }    
                //}    

            }    
            switch (group)
            {
                case GroupTool.Basic_Tool:
                   
                    break;
                case GroupTool.Extra_Tool_1:
                    break;
                case GroupTool.Extra_Tool_2:
                    break;
            }    
        }
        public void CreateNewTool()
        {
            if(TypeTool==TypeTool.Position_Adjustment)
            {
                int ix = BeeCore.Common.PropetyTools[Global.IndexChoose].FindIndex(p => p.TypeTool == TypeTool.Position_Adjustment);
            if(ix>-1)
                {
                    MessageBox.Show("Position Adjustment is Already! ");
                    return;
                }
            else
                {
                    if (BeeCore.Common.PropetyTools[Global.IndexChoose].Count>0)
                    {
                        int indexName2 = 1;
                        dynamic control2 = DataTool.NewControl(TypeTool, indexName2 - 1, Global.IndexChoose, TypeTool.ToString() + " " + indexName2, new Point(Global.pShowTool.X, Global.pShowTool.Y));
                        PropetyTool propetyTool2 = new PropetyTool(control2.Propety, TypeTool, TypeTool.ToString() + " " + indexName2);
                        propetyTool2.UsedTool = UsedTool.Used;
                        BeeCore.Common.PropetyTools[Global.IndexChoose].Insert(0,propetyTool2);
                        ItemTool Itemtool2 = DataTool.CreateItemTool(propetyTool2, indexName2 - 1, Global.IndexChoose, new Point(Global.pShowTool.X, Global.pShowTool.Y));

                        BeeCore.Common.PropetyTools[Global.IndexChoose][0].ItemTool = Itemtool2;
                        BeeCore.Common.PropetyTools[Global.IndexChoose][0].Control = control2;
                        int i = 0;
                        foreach(PropetyTool propety in BeeCore.Common.PropetyTools[Global.IndexChoose])
                        {
                            propety.ItemTool.IndexTool = i;
                            propety.Propety.Index = i;i++;
                        }    
                        propetyTool2.worker = new System.ComponentModel.BackgroundWorker();
                        propetyTool2.timer = new System.Diagnostics.Stopwatch();
                        propetyTool2.worker.DoWork += (sender, e) =>
                        {
                            propetyTool2.DoWork();
                        };
                        propetyTool2.worker.RunWorkerCompleted += (sender, e) =>
                        {
                            propetyTool2.Complete();
                        };
                        propetyTool2.Propety.SetModel();

                        DataTool.LoadPropety(control2);
                        ShowTool.ShowChart(Global.ToolSettings.pAllTool, BeeCore.Common.PropetyTools[Global.IndexChoose]);

                        return;
                    }
                }    

            }    
            int indexName = BeeCore.Common.PropetyTools[Global.IndexChoose].Count() + 1;
            dynamic control = DataTool.NewControl(TypeTool, indexName - 1, Global.IndexChoose, TypeTool.ToString() + " " + indexName, new Point(Global.pShowTool.X, Global.pShowTool.Y));
            PropetyTool propetyTool = new PropetyTool(control.Propety, TypeTool, TypeTool.ToString() + " " + indexName);
            propetyTool.UsedTool = UsedTool.Used;
            BeeCore.Common.PropetyTools[Global.IndexChoose].Add(propetyTool);
            ItemTool Itemtool = DataTool.CreateItemTool(propetyTool ,indexName - 1, Global.IndexChoose, new Point(Global.pShowTool.X, Global.pShowTool.Y));
          
            BeeCore.Common.PropetyTools[Global.IndexChoose][BeeCore.Common.PropetyTools[Global.IndexChoose].Count() - 1].ItemTool = Itemtool;
            BeeCore.Common.PropetyTools[Global.IndexChoose][BeeCore.Common.PropetyTools[Global.IndexChoose].Count() - 1].Control = control;
            
                propetyTool.worker = new System.ComponentModel.BackgroundWorker();
                propetyTool.timer = new System.Diagnostics.Stopwatch();
                propetyTool.worker.DoWork += (sender, e) =>
                {
                    propetyTool.DoWork();
                };
                propetyTool.worker.RunWorkerCompleted += (sender, e) =>
                {
                    propetyTool.Complete();
                };
                propetyTool.Propety.SetModel();
            
                DataTool.LoadPropety(control);
            ShowTool.ShowChart(Global.ToolSettings.pAllTool, BeeCore.Common.PropetyTools[Global.IndexChoose]);
            BeeInterface.Load.ArrangeLogic();

        }
         private void btnOk_Click(object sender, EventArgs e)
        {
            this.Parent.Hide();
          
            CreateNewTool();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Parent.Hide();
        }
    }
}
