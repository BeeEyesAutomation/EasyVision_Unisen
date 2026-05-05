using System;
using System.Windows.Forms;

namespace BeeInterface.Tool._Base
{
    [Serializable]
    public partial class ToolViewBase : UserControl, IToolView
    {
        private BeeCore.PropetyTool _ownerTool;
        private bool _dynamicTabsBuilt;
        private string _requestedToolKind;

        public ToolViewBase()
        {
            InitializeComponent();

            HandleCreated -= OnHandleCreatedBuildDynamicTabs;
            HandleCreated += OnHandleCreatedBuildDynamicTabs;

            if (tabRoot != null)
            {
                tabRoot.SelectedIndexChanged -= OnTabRootSelectedChanged;
                tabRoot.SelectedIndexChanged += OnTabRootSelectedChanged;
            }
        }

        public BeeCore.PropetyTool OwnerTool
        {
            get
            {
                if (_ownerTool == null && Propety != null)
                {
                    int? index = TryReadPropetyIndex(Propety);
                    if (index.HasValue)
                        _ownerTool = BeeCore.Common.TryGetTool(BeeGlobal.Global.IndexProgChoose, index.Value);
                }

                return _ownerTool;
            }
        }

        public virtual object Propety { get; set; }

        public virtual string ToolKind
        {
            get
            {
                if (!string.IsNullOrEmpty(_requestedToolKind))
                    return _requestedToolKind;

                return Propety != null ? Propety.GetType().Name : GetType().Name;
            }
        }

        public virtual void LoadPara()
        {
        }

        public virtual void OnTabChanged(string tabKey)
        {
        }

        protected void InvalidateOwnerToolCache()
        {
            _ownerTool = null;
        }

        protected void RebuildDynamicTabs()
        {
            _dynamicTabsBuilt = false;
            RemoveDynamicTabs();
            BuildDynamicTabs();
        }

        protected void RequestDynamicTabsForKind(string toolKind)
        {
            _requestedToolKind = toolKind;
            RebuildDynamicTabs();
        }

        private static int? TryReadPropetyIndex(object propety)
        {
            var property = propety.GetType().GetProperty("Index");
            if (property != null)
            {
                object value = property.GetValue(propety, null);
                if (value is int)
                    return (int)value;
            }

            var field = propety.GetType().GetField("Index");
            if (field != null)
            {
                object value = field.GetValue(propety);
                if (value is int)
                    return (int)value;
            }

            return null;
        }

        private void OnHandleCreatedBuildDynamicTabs(object sender, EventArgs e)
        {
            BuildDynamicTabs();
        }

        private void BuildDynamicTabs()
        {
            if (_dynamicTabsBuilt || Propety == null || tabRoot == null)
                return;

            GroupBToolTabRegistrar.RegisterDefaults();

            var context = new ToolTabContext(this);
            foreach (var entry in ToolTabRegistry.Get(ToolKind))
            {
                var page = new TabPage(entry.DisplayName)
                {
                    Name = "tabDynamic" + entry.TabKey,
                    Tag = entry.TabKey
                };

                Control content = entry.Build(context);
                if (content != null)
                {
                    content.Dock = DockStyle.Fill;
                    page.Controls.Add(content);
                }

                tabRoot.TabPages.Add(page);
            }

            _dynamicTabsBuilt = true;
        }

        private void RemoveDynamicTabs()
        {
            if (tabRoot == null)
                return;

            for (int i = tabRoot.TabPages.Count - 1; i >= 0; i--)
            {
                TabPage page = tabRoot.TabPages[i];
                if (page.Tag != null && !IsFixedTabKey(page.Tag.ToString()))
                    tabRoot.TabPages.RemoveAt(i);
            }
        }

        private static bool IsFixedTabKey(string tabKey)
        {
            return string.Equals(tabKey, "general", StringComparison.OrdinalIgnoreCase)
                || string.Equals(tabKey, "roi", StringComparison.OrdinalIgnoreCase)
                || string.Equals(tabKey, "params", StringComparison.OrdinalIgnoreCase)
                || string.Equals(tabKey, "result", StringComparison.OrdinalIgnoreCase);
        }

        private void OnTabRootSelectedChanged(object sender, EventArgs e)
        {
            if (tabRoot == null || tabRoot.SelectedTab == null)
                return;

            var selected = tabRoot.SelectedTab;
            OnTabChanged(selected.Tag != null ? selected.Tag.ToString() : selected.Name);
        }
    }
}
