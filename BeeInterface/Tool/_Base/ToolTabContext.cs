namespace BeeInterface.Tool._Base
{
    public sealed class ToolTabContext
    {
        public IToolView View { get; private set; }
        public BeeCore.PropetyTool OwnerTool { get; private set; }
        public object Propety { get; private set; }

        public ToolTabContext(IToolView view)
        {
            View = view;
            OwnerTool = view.OwnerTool;
            Propety = view.Propety;
        }
    }
}
