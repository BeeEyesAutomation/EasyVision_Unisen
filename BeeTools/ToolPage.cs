﻿using BeeCore;
using BeeGlobal;
using BeeInterface;
using Newtonsoft.Json.Linq;
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
        public RJButton NewRadioButton(String name,int y)
        {
            RJButton btn = new RJButton();
            btn.Name = "btn" + name;
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
           btn.FlatAppearance.BorderSize = 0;
           btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
           btn.ForeColor = System.Drawing.Color.White;
          
            btn.Image = (Image)Properties.Resources.ResourceManager.GetObject(name.Trim());
          
                name = name.Replace("_", " ");
            if (name.Length < lenMax)
            {
                int numLen = lenMax - name.Length;
                for ( int i= 0;i<numLen;i++)
                    name += " ";
                name += ".";
            }
           
                btn.Text = name;
          
                btn.Location = new System.Drawing.Point(25, y);
          //  if (!btn.Text.Contains("Color Area"))
            //    btn.Enabled = false;
           btn.Size = new System.Drawing.Size(277, 44);
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
            String nameBtn = btn.Name.Replace("btn", "");

            TypeTool = (TypeTool)Enum.Parse(typeof(TypeTool), nameBtn, true);
         int index=   Global.listItool.FindIndex(a => a.TypeTool == TypeTool);
            if(index>-1)
            { String name = nameBtn.Replace("_", " ");
                lbName.Text = name;
                lbContent.Text = Global.listItool[index].Content;
                img.Image= (Image)Properties.Resources.ResourceManager.GetObject("content"+ nameBtn);
            }
        }

        public List<string> GetListTool()
        {
            return Enum.GetNames(typeof(TypeTool)).ToList();

        }
        //
        public static List<string> listContent = new List<string>
        {

       "Position_Adjustment",//1
       "Pattern" ,//2
       "Matching Shape" ,//3
       "OutLine" ,//4
       "Edge_Pixels" ,//5
       "Color_Area",//6
       "Width" ,//7
       "Diameter" ,//8
       "Edge" ,//9
        "Pitch",//10
        "OCR",//11
        "QRCODE",//12
         "Learning",//13
         "Position",//14
         "Measure",//15
         "Circle"//16
        };
        public static List<GroupTool> groupTools = new List<GroupTool>
        {
            GroupTool.Basic_Tool,
            GroupTool.Basic_Tool,
            GroupTool.Extra_Tool_2,
            GroupTool.Extra_Tool_2,
            GroupTool.Extra_Tool_2,
            GroupTool.Basic_Tool,
            GroupTool.Extra_Tool_1,
            GroupTool.Extra_Tool_2,
            GroupTool.Extra_Tool_1,
            GroupTool.Extra_Tool_2,
            GroupTool.Basic_Tool,
            GroupTool.Extra_Tool_2,
             GroupTool.Basic_Tool,
             GroupTool.Extra_Tool_1,
              GroupTool.Extra_Tool_1,
               GroupTool.Extra_Tool_1,

        };
        int lenMax = 0;
        public void LoadAllInforTool()
        {
            IsRect = true;
            Global.listItool = new List<iTool>();
            int ix = 0;
            Basic_Tool.Controls.Clear();
            Extra_Tool_1.Controls.Clear();
            Extra_Tool_2.Controls.Clear();
            foreach (String nameTool in GetListTool())
            {
                if(lenMax< nameTool.Length)
                lenMax = nameTool.Length;
                TypeTool type = (TypeTool)Enum.Parse(typeof(TypeTool), nameTool, true);
                Global.listItool.Add(new iTool(type,null, listContent[ix], groupTools[ix]));
                ix++;
            }
          }
        bool IsRect = true;
        public void LoadGuiAllTool()
        {
            int y= 23; int y2 = 23; int y3 = 23;
             foreach (iTool tool in Global.listItool)
            {
               
                 String name = tool.TypeTool.ToString();
                GroupTool groupTool = tool.GroupTool;
                RJButton btn=new RJButton();
                    switch (groupTool)
                    {
                        case GroupTool.Basic_Tool:

                             btn = NewRadioButton(name, y);
                        y += 68;
                        btn.Parent = Basic_Tool;
                        //if (tool.TypeTool != TypeTool.Position_Adjustment&& tool.TypeTool != TypeTool.Pattern && tool.TypeTool != TypeTool.MatchingShape)
                        //    btn.Enabled = false;
                            break;
                        case GroupTool.Extra_Tool_1:
                             btn = NewRadioButton(name, y2);
                       // btn.Enabled = false;
                        y2 += 68;
                        btn.Parent = Extra_Tool_1;
                       // btn.Enabled = false;
                            break;
                        case GroupTool.Extra_Tool_2:
                       
                        btn = NewRadioButton(name, y3);
                        //btn.Enabled = false;
                        y3 += 68;
                        btn.Parent = Extra_Tool_2;
                       // btn.Enabled = false;
                        break;

                }
       
                tool.Control = btn;



            }    
        }

        private void btn_Click(object sender, EventArgs e)
        {

        }

        private void ToolPage_Load(object sender, EventArgs e)
        {
            LoadAllInforTool();
            LoadGuiAllTool();
        }

        private void tabTool_SelectedIndexChanged(object sender, EventArgs e)
        {

            TabPage tab= tabTool.SelectedTab;
          
                GroupTool group = (GroupTool)Enum.Parse(typeof(GroupTool), tab.Name, true);
            int index = Global.listItool.FindIndex(a => a.GroupTool == group);
            if(index>-1)
            {
                RJButton rJButton = Global.listItool[index].Control;
                
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
            int indexName = BeeCore.Common.PropetyTools[Global.IndexChoose].Count() + 1;
            dynamic control = DataTool.NewControl(TypeTool, indexName - 1, Global.IndexChoose, TypeTool.ToString() + " " + indexName, new Point(Global.pShowTool.X, Global.pShowTool.Y));
            PropetyTool propetyTool = new PropetyTool(control.Propety, TypeTool, TypeTool.ToString() + " " + indexName);
            BeeCore.Common.PropetyTools[Global.IndexChoose].Add(propetyTool);
            ItemTool Itemtool = DataTool.CreateItemTool(propetyTool ,indexName - 1, Global.IndexChoose, new Point(Global.pShowTool.X, Global.pShowTool.Y));
         
            BeeCore.Common.PropetyTools[Global.IndexChoose][BeeCore.Common.PropetyTools[Global.IndexChoose].Count() - 1].ItemTool = Itemtool;
            BeeCore.Common.PropetyTools[Global.IndexChoose][BeeCore.Common.PropetyTools[Global.IndexChoose].Count() - 1].Control = control;
            
                propetyTool.worker = new System.ComponentModel.BackgroundWorker();
                propetyTool.timer = new System.Diagnostics.Stopwatch();
                propetyTool.worker.DoWork += (sender, e) =>
                {
                    propetyTool.StatusTool = StatusTool.Processing;
                    propetyTool.timer.Restart();
                    if (!Global.IsRun)
                        propetyTool.Propety.rotAreaAdjustment = propetyTool.Propety.rotArea;
                    propetyTool.Propety.DoWork(propetyTool.Propety.rotAreaAdjustment);
                };
                propetyTool.worker.RunWorkerCompleted += (sender, e) =>
                {
                    propetyTool.Propety.Complete();
                    if (!Global.IsRun)
                        Global.StatusDraw = StatusDraw.Check;
                    propetyTool.StatusTool = StatusTool.Done;
                    propetyTool.timer.Stop();
                    propetyTool.CycleTime = (int)propetyTool.timer.Elapsed.TotalMilliseconds;
                };
                //  G.listAlltool[Global.IndexChoose].Add(DataTool.CreateControl(propetyTool, indexName-1, Global.IndexChoose,new Point(Global.pShowTool.X,Global.pShowTool.Y)));
                DataTool.LoadPropety(control);

                Global.pShowTool.Y += Itemtool.Height + 10;
                Global.ToolSettings.pAllTool.Controls.Add(Itemtool);
            

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
