
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeGlobal
{
    [Serializable()]
    public class RectRotate
    {

        public RectangleF _rect { get; set; }
        public PointF _PosCenter { get; set; }
        public Single _angle = 0;
        public Single _rectRotation { get =>_angle; set => _angle=value; }
        public AnchorPoint _dragAnchor { get; set; }
        public bool IsElip = false;
        public RectRotate(RectangleF _rect, PointF _PosCenter, Single _rectRotation, AnchorPoint _dragAnchor, bool _IsElip)
        {
            this._rect = _rect;
            this._PosCenter = _PosCenter;
            this._rectRotation = _rectRotation;
            this._dragAnchor = _dragAnchor;
            this.IsElip = _IsElip;
           
        }
        protected RectRotate(RectRotate clone)
        {
            this._rect = clone._rect;
            this._PosCenter = clone._PosCenter;
            this._rectRotation = clone._rectRotation;
            this._dragAnchor = clone._dragAnchor;
          
        }

        public RectRotate Clone()
        {
            return new RectRotate(this);
        }
        public RectRotate()
        {

        }
    }
}
