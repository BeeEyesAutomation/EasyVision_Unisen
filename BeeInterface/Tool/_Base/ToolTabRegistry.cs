using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace BeeInterface.Tool._Base
{
    public static class ToolTabRegistry
    {
        private static readonly object SyncRoot = new object();
        private static readonly Dictionary<string, List<TabEntry>> Entries =
            new Dictionary<string, List<TabEntry>>(StringComparer.OrdinalIgnoreCase);

        public sealed class TabEntry
        {
            public string TabKey { get; set; }
            public string DisplayName { get; set; }
            public Func<ToolTabContext, Control> Build { get; set; }
        }

        public static void Register(string toolKind, string tabKey, string displayName, Func<ToolTabContext, Control> build)
        {
            if (string.IsNullOrWhiteSpace(toolKind))
                throw new ArgumentException("Tool kind is required.", "toolKind");
            if (string.IsNullOrWhiteSpace(tabKey))
                throw new ArgumentException("Tab key is required.", "tabKey");
            if (build == null)
                throw new ArgumentNullException("build");

            lock (SyncRoot)
            {
                List<TabEntry> list;
                if (!Entries.TryGetValue(toolKind, out list))
                {
                    list = new List<TabEntry>();
                    Entries[toolKind] = list;
                }

                list.RemoveAll(e => string.Equals(e.TabKey, tabKey, StringComparison.OrdinalIgnoreCase));
                list.Add(new TabEntry
                {
                    TabKey = tabKey,
                    DisplayName = string.IsNullOrWhiteSpace(displayName) ? tabKey : displayName,
                    Build = build
                });
            }
        }

        public static IReadOnlyList<TabEntry> Get(string toolKind)
        {
            if (string.IsNullOrWhiteSpace(toolKind))
                return new TabEntry[0];

            lock (SyncRoot)
            {
                List<TabEntry> list;
                if (!Entries.TryGetValue(toolKind, out list))
                    return new TabEntry[0];

                return list.ToArray();
            }
        }
    }
}
