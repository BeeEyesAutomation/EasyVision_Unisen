using BeeGlobal;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeGlobal
{
    [Serializable()]
    public class iTool
    {
       
        public  TypeTool TypeTool;
        public String  Content = "Nhập nội dung tool vào đây";
        public GroupTool GroupTool;
        public dynamic Control; //

        public iTool()
        {

        }

        public iTool(TypeTool TypeTool, dynamic Control, String Content, GroupTool GroupTool)
        {
            this.Control = Control;
            this.TypeTool = TypeTool;
            this.Content = Content;
            this.GroupTool= GroupTool ;


        }
    }
}
