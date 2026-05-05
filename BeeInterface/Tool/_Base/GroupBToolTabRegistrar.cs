using System;
using System.Drawing;
using System.Windows.Forms;

namespace BeeInterface.Tool._Base
{
    public static class GroupBToolTabRegistrar
    {
        private static bool _registered;

        public static void RegisterDefaults()
        {
            if (_registered)
                return;

            RegisterPreset("Pattern");
            RegisterPreset("Patterns");
            RegisterPreset("MultiPattern");
            RegisterPreset("MatchingShape");
            RegisterPreset("Pitch");
            RegisterPreset("Barcode");
            RegisterPreset("OCR");
            RegisterPreset("CraftOCR");

            RegisterPreprocess("Pattern");
            RegisterPreprocess("Patterns");
            RegisterPreprocess("MultiPattern");
            RegisterPreprocess("MatchingShape");
            RegisterPreprocess("Pitch");
            RegisterPreprocess("OCR");
            RegisterPreprocess("CraftOCR");

            _registered = true;
        }

        private static void RegisterPreset(string toolKind)
        {
            ToolTabRegistry.Register(toolKind, "preset", "Preset", BuildPresetTab);
        }

        private static void RegisterPreprocess(string toolKind)
        {
            ToolTabRegistry.Register(toolKind, "preprocess", "Preprocess", BuildPreprocessTab);
        }

        private static Control BuildPresetTab(ToolTabContext context)
        {
            var panel = CreatePanel();
            panel.Controls.Add(CreateLabel("Preset", 0, 0));

            var combo = new ComboBox
            {
                Dock = DockStyle.Fill,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            combo.Items.Add("Default");
            combo.SelectedIndex = 0;
            panel.Controls.Add(combo, 1, 0);

            var grid = new ResultMiniGrid
            {
                Dock = DockStyle.Fill
            };
            grid.SetRow("Tool", context != null && context.View != null ? context.View.ToolKind : string.Empty);
            grid.SetRow("Preset", combo.SelectedItem);
            panel.Controls.Add(grid, 0, 1);
            panel.SetColumnSpan(grid, 2);

            combo.SelectedIndexChanged += delegate
            {
                grid.SetRow("Preset", combo.SelectedItem);
            };

            return panel;
        }

        private static Control BuildPreprocessTab(ToolTabContext context)
        {
            var panel = CreatePanel();
            panel.Controls.Add(CreateLabel("Pipeline", 0, 0));

            var options = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding(0, 4, 0, 0)
            };

            options.Controls.Add(CreateCheckBox("Bitwise not"));
            options.Controls.Add(CreateCheckBox("SIMD"));
            options.Controls.Add(CreateCheckBox("Sub pixel"));
            panel.Controls.Add(options, 1, 0);

            var score = new ScoreThresholdBar
            {
                Dock = DockStyle.Top,
                Caption = "Score",
                Min = 0,
                Max = 100,
                Step = 1,
                Value = 80
            };
            panel.Controls.Add(score, 0, 1);
            panel.SetColumnSpan(score, 2);

            return panel;
        }

        private static TableLayoutPanel CreatePanel()
        {
            var panel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(8),
                ColumnCount = 2,
                RowCount = 2
            };
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 96));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            return panel;
        }

        private static Label CreateLabel(string text, int column, int row)
        {
            return new Label
            {
                Text = text,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                AutoSize = true,
                Margin = new Padding(0, 8, 8, 8)
            };
        }

        private static CheckBox CreateCheckBox(string text)
        {
            return new CheckBox
            {
                Text = text,
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 8)
            };
        }
    }
}
