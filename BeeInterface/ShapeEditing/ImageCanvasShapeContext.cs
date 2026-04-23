using System;
using System.Drawing;
using BeeGlobal;

namespace BeeInterface.ShapeEditing
{
    public sealed class ImageCanvasShapeContext
    {
        public Func<bool> IsWaitShowTool { get; set; }
        public Func<int> GetIndexTool { get; set; }
        public Func<int> GetIndexProg { get; set; }
        public Func<int> GetIndexCamera { get; set; }
        public Func<bool> IsRun { get; set; }
        public Func<bool> IsGetColor { get; set; }
        public Func<Color> ColorChoose { get; set; }
        public Action<bool> SetIsSetColor { get; set; }
        public Action StartColorPicker { get; set; }
        public Func<bool> IsLive { get; set; }
        public Func<Config> GetConfig { get; set; }
        public Func<ParaShow> GetParaShow { get; set; }
        public Action<string, string, Results> UpdateResultInfo { get; set; }
        public Func<StatusDraw> GetMode { get; set; }
        public Action<StatusDraw> SetMode { get; set; }
        public Func<RectRotate> GetCurrent { get; set; }
        public Action<RectRotate> SetCurrent { get; set; }
        public Func<float> GetHandleRadius { get; set; }
        public Func<bool> IsPanEnabled { get; set; }
        public Action<int> SetIndexRotChoose { get; set; }
        public Action<Point> ShowAngleControl { get; set; }
        public Action<Point> ShowCenterControl { get; set; }
        public Action HideAngleControl { get; set; }

        public bool WaitingForTool => IsWaitShowTool != null && IsWaitShowTool();
        public int IndexTool => GetIndexTool == null ? -1 : GetIndexTool();
        public int IndexProg => GetIndexProg == null ? -1 : GetIndexProg();
        public int IndexCamera => GetIndexCamera == null ? -1 : GetIndexCamera();
        public bool Running => IsRun != null && IsRun();
        public bool GettingColor => IsGetColor != null && IsGetColor();
        public Color _clChoose =>  ColorChoose();
        public bool Live => IsLive != null && IsLive();
        public Config Config => GetConfig == null ? null : GetConfig();
        public ParaShow ParaShow => GetParaShow == null ? null : GetParaShow();
        public StatusDraw Mode => GetMode == null ? StatusDraw.None : GetMode();
        public RectRotate Current => GetCurrent == null ? null : GetCurrent();
        public float HandleRadius => GetHandleRadius == null ? 6f : GetHandleRadius();
        public bool PanEnabled => IsPanEnabled != null && IsPanEnabled();
    }
}
