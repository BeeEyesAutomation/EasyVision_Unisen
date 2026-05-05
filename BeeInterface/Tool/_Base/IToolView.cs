namespace BeeInterface.Tool._Base
{
    public interface IToolView
    {
        object Propety { get; set; }
        BeeCore.PropetyTool OwnerTool { get; }
        string ToolKind { get; }

        void LoadPara();
        void OnTabChanged(string tabKey);
    }
}
