// BeeInterface - ControlStylePersistence & StyleEditorForm (Style + Layout + Import/Export)
// - Load immediately, NO autosave (Save button writes file)
// - Supports: BackColor, ForeColor, Font, Image, Tooltip, Text,
//             Enabled, Visible, Size(w,h), Margin(L,T,R,B), Padding(L,T,R,B)
// - PropertyGrid: Current (effective) + Override (StyleMap) with VS-like editors
// - Save/Export: PRUNE — nếu value trên StyleMap == baseline hoặc null → KHÔNG LƯU (xoá khỏi map)
// - Import hỏi xác nhận; áp dụng & refresh ngay. Export ghi JSON.
// - UI: Margin/Padding/Size gói gọn 1 dòng trong "Override (StyleMap)".

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Windows.Forms;

namespace BeeInterface
{
    #region Model + Serialization

    [DataContract]
    internal sealed class FontSpec
    {
        [DataMember] public string Family;
        [DataMember] public float Size;
        [DataMember] public FontStyle Style;
        [DataMember] public GraphicsUnit Unit;

        public static FontSpec FromFont(Font f) => f == null ? null : new FontSpec
        {
            Family = f.FontFamily?.Name ?? "Segoe UI",
            Size = f.Size,
            Style = f.Style,
            Unit = f.Unit == 0 ? GraphicsUnit.Point : f.Unit
        };

        public Font ToFontSafe()
        {
            try
            {
                var fam = string.IsNullOrWhiteSpace(Family) ? "Segoe UI" : Family;
                return new Font(fam, Size > 0 ? Size : 9f, Style, Unit == 0 ? GraphicsUnit.Point : Unit);
            }
            catch { return SystemFonts.DefaultFont; }
        }

        public bool EqualsTo(Font f)
        {
            if (f == null) return false;
            return string.Equals(Family ?? "", f.FontFamily?.Name ?? "", StringComparison.OrdinalIgnoreCase)
                && Math.Abs(Size - f.Size) < 0.01f
                && Style == f.Style
                && (Unit == 0 ? GraphicsUnit.Point : Unit) == f.Unit;
        }
    }

    [DataContract]
    internal sealed class SizeSpec
    {
        [DataMember(Order = 0)] public int? W;
        [DataMember(Order = 1)] public int? H;
        public bool IsEmpty => W == null && H == null;
    }

    [DataContract]
    internal sealed class PaddingSpec
    {
        [DataMember(Order = 0)] public int? L;
        [DataMember(Order = 1)] public int? T;
        [DataMember(Order = 2)] public int? R;
        [DataMember(Order = 3)] public int? B;
        public bool IsEmpty => L == null && T == null && R == null && B == null;
    }

    [DataContract]
    internal sealed class ControlStyleRecord
    {
        [DataMember] public string Path;

        // Style
        [DataMember] public int? BackArgb;
        [DataMember] public int? ForeArgb;
        [DataMember] public FontSpec Font;
        [DataMember] public string ImageBase64;       // null => clear
        [DataMember] public string Tooltip;           // null = no override; "" = clear
        [DataMember] public string Text;              // null = no override

        // Layout/State
        [DataMember] public bool? Enabled;            // null = not set
        [DataMember] public bool? Visible;            // null = not set
        [DataMember] public SizeSpec Size;            // nullable members
        [DataMember] public PaddingSpec Margin;       // nullable members
        [DataMember] public PaddingSpec Padding;      // nullable members

        public bool IsEmpty =>
            BackArgb == null && ForeArgb == null && Font == null &&
            string.IsNullOrEmpty(ImageBase64) && Tooltip == null && Text == null &&
            Enabled == null && Visible == null &&
            (Size == null || Size.IsEmpty) &&
            (Margin == null || Margin.IsEmpty) &&
            (Padding == null || Padding.IsEmpty);
    }

    [DataContract]
    internal sealed class StyleMap
    {
        [DataMember] public List<ControlStyleRecord> Items = new List<ControlStyleRecord>();

        public ControlStyleRecord Find(string path)
            => Items.FirstOrDefault(i => string.Equals(i.Path, path, StringComparison.OrdinalIgnoreCase));

        public void Put(ControlStyleRecord rec)
        {
            var old = Find(rec.Path);
            if (old != null) Items.Remove(old);
            Items.Add(rec);
        }

        public void RemoveIfEmpty(string path)
        {
            var old = Find(path);
            if (old != null && old.IsEmpty) Items.Remove(old);
        }
    }

    internal static class JsonUtil
    {
        public static void Save<T>(string path, T obj)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            using (var fs = File.Create(path))
            {
                var ser = new DataContractJsonSerializer(typeof(T));
                ser.WriteObject(fs, obj);
            }
        }

        public static T Load<T>(string path) where T : new()
        {
            if (!File.Exists(path)) return new T();
            using (var fs = File.OpenRead(path))
            {
                var ser = new DataContractJsonSerializer(typeof(T));
                return (T)ser.ReadObject(fs);
            }
        }
    }

    internal static class ImageBase64
    {
        public static string ToPngBase64(Image img)
        {
            if (img == null) return null;
            using (var ms = new MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return Convert.ToBase64String(ms.ToArray());
            }
        }

        public static Image FromPngBase64(string b64)
        {
            if (string.IsNullOrWhiteSpace(b64)) return null;
            var bytes = Convert.FromBase64String(b64);
            using (var ms = new MemoryStream(bytes))
                return Image.FromStream(ms);
        }
    }

    #endregion

    public sealed class ControlStylePersistence
    {
        private readonly Control _host;
        private readonly string _key;

        private bool _applying;
        private StyleMap _map = new StyleMap();

        private readonly Dictionary<string, ControlStyleRecord> _baseline =
            new Dictionary<string, ControlStyleRecord>(StringComparer.OrdinalIgnoreCase);

        private readonly ToolTip _tooltipWriter = new ToolTip();
        private readonly List<ToolTip> _existingTooltips = new List<ToolTip>();

        public bool LoadImmediately { get; set; } = true;

        public ControlStylePersistence(Control host, string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("key must not be empty", nameof(key));
            _host = host ?? throw new ArgumentNullException(nameof(host));
            _key = key;

            _host.ControlAdded += (s, e) => AttachDeep(e.Control);
            _host.ControlRemoved += (s, e) => DetachDeep(e.Control);
            AttachDeep(_host);

            TryCollectExistingToolTips();
            CaptureBaselineRecursive(_host, _host);

            if (LoadImmediately) LoadNow();
        }

        public void LoadNow()
        {
            if (_applying) return;
            _applying = true;
            try
            {
                _map = JsonUtil.Load<StyleMap>(GetStyleFilePath());
                ApplyRecursive(_host, _host, _map);
            }
            finally { _applying = false; }
        }

        public void SaveExplicit()
        {
            if (_applying) return;
            JsonUtil.Save(GetStyleFilePath(), _map);
        }

        internal StyleMap Snapshot() => _map;

        internal void OverwriteAndApply(StyleMap newMap)
        {
            if (newMap == null) return;
            _map = newMap;
            if (_applying) return;
            _applying = true;
            try { ApplyRecursive(_host, _host, _map); }
            finally { _applying = false; }
        }

        public void ShowEditor(IWin32Window owner = null)
        {
            using (var dlg = new StyleEditorForm(this, _host))
            {
                var win = owner ?? (_host.FindForm() as IWin32Window);
                dlg.StartPosition = FormStartPosition.CenterParent;
                if (win != null) dlg.ShowDialog(win);
                else dlg.ShowDialog();
            }
        }

        internal ControlStyleRecord GetOrCreate(string path)
        {
            var rec = _map.Find(path);
            if (rec == null) { rec = new ControlStyleRecord { Path = path }; _map.Put(rec); }
            return rec;
        }

        internal void RemoveIfEmpty(string path) => _map.RemoveIfEmpty(path);

        // ---- NEW: expose baseline for prune
        internal bool TryGetBaseline(string path, out ControlStyleRecord rec)
            => _baseline.TryGetValue(path, out rec);

        private void AttachDeep(Control c)
        {
            c.BackColorChanged += OnChanged;
            c.ForeColorChanged += OnChanged;
            c.FontChanged += OnChanged;
            c.ControlAdded += OnBranchAdded;
            c.ControlRemoved += OnBranchRemoved;
            foreach (Control ch in c.Controls) AttachDeep(ch);
        }

        private void DetachDeep(Control c)
        {
            try
            {
                c.BackColorChanged -= OnChanged;
                c.ForeColorChanged -= OnChanged;
                c.FontChanged -= OnChanged;
                c.ControlAdded -= OnBranchAdded;
                c.ControlRemoved -= OnBranchRemoved;
            }
            catch { }
            foreach (Control ch in c.Controls) DetachDeep(ch);
        }

        private void OnBranchAdded(object sender, ControlEventArgs e) => AttachDeep(e.Control);
        private void OnBranchRemoved(object sender, ControlEventArgs e) => DetachDeep(e.Control);
        private void OnChanged(object sender, EventArgs e) { /* no autosave */ }

        #region Baseline + Tooltip

        private void TryCollectExistingToolTips()
        {
            try
            {
                var form = _host.FindForm();
                if (form == null) return;

                var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
                foreach (var f in form.GetType().GetFields(flags))
                    if (typeof(ToolTip).IsAssignableFrom(f.FieldType))
                        if (f.GetValue(form) is ToolTip tt && !_existingTooltips.Contains(tt))
                            _existingTooltips.Add(tt);

                foreach (var p in form.GetType().GetProperties(flags))
                    if (typeof(ToolTip).IsAssignableFrom(p.PropertyType) && p.CanRead)
                        if (p.GetValue(form, null) is ToolTip tt && !_existingTooltips.Contains(tt))
                            _existingTooltips.Add(tt);
            }
            catch { }
        }

        private string GetExistingToolTipText(Control c)
        {
            try
            {
                foreach (var tt in _existingTooltips)
                {
                    var txt = tt.GetToolTip(c);
                    if (!string.IsNullOrEmpty(txt)) return txt;
                }
            }
            catch { }
            return null;
        }

        internal string GetEffectiveTooltip(Control c)
        {
            try
            {
                var t = _tooltipWriter.GetToolTip(c);
                if (!string.IsNullOrEmpty(t)) return t;
                foreach (var tt in _existingTooltips)
                {
                    var s = tt.GetToolTip(c);
                    if (!string.IsNullOrEmpty(s)) return s;
                }
            }
            catch { }
            return string.Empty;
        }

        private void CaptureBaselineRecursive(Control root, Control c)
        {
            var path = BuildPath(root, c);
            var rec = new ControlStyleRecord { Path = path };

            rec.BackArgb = c.BackColor.IsEmpty ? (int?)null : c.BackColor.ToArgb();
            rec.ForeArgb = c.ForeColor.IsEmpty ? (int?)null : c.ForeColor.ToArgb();
            rec.Font = FontSpec.FromFont(c.Font);

            var tgi = TryGetImage(c);
            if (tgi.hasImage) rec.ImageBase64 = ImageBase64.ToPngBase64(tgi.img);

            rec.Tooltip = GetExistingToolTipText(c);
            try { rec.Text = c.Text; } catch { rec.Text = null; }

            rec.Enabled = c.Enabled;
            rec.Visible = c.Visible;
            rec.Size = new SizeSpec { W = c.Size.Width, H = c.Size.Height };
            rec.Margin = new PaddingSpec { L = c.Margin.Left, T = c.Margin.Top, R = c.Margin.Right, B = c.Margin.Bottom };
            rec.Padding = new PaddingSpec { L = c.Padding.Left, T = c.Padding.Top, R = c.Padding.Right, B = c.Padding.Bottom };

            _baseline[path] = rec;

            foreach (Control ch in c.Controls) CaptureBaselineRecursive(root, ch);
        }

        internal void RestoreBaseline(Control control)
        {
            if (control == null) return;
            var path = BuildPath(_host, control);
            if (_baseline.TryGetValue(path, out var rec))
                ApplyOne(control, rec);
            foreach (Control ch in control.Controls)
                RestoreBaseline(ch);
        }

        private Control FindByPath(Control root, Control current, string targetPath)
        {
            if (BuildPath(root, current).Equals(targetPath, StringComparison.OrdinalIgnoreCase))
                return current;
            foreach (Control ch in current.Controls)
            {
                var found = FindByPath(root, ch, targetPath);
                if (found != null) return found;
            }
            return null;
        }

        internal void RestoreBaselineByPath(string path)
        {
            var target = FindByPath(_host, _host, path);
            RestoreBaseline(target);
        }

        #endregion

        #region Capture / Apply

        internal static string BuildPath(Control root, Control c)
        {
            var stack = new Stack<string>();
            var cur = c;
            while (cur != null && cur != root.Parent)
            {
                string seg = !string.IsNullOrWhiteSpace(cur.Name)
                    ? cur.Name
                    : cur.GetType().Name + "#" + IndexInParent(cur);
                stack.Push(seg);
                if (cur == root) break;
                cur = cur.Parent;
            }
            return string.Join("/", stack);
        }

        private static int IndexInParent(Control c)
        {
            if (c?.Parent == null) return -1;
            int i = 0; foreach (Control x in c.Parent.Controls) { if (x == c) return i; i++; }
            return -1;
        }

        internal static void CaptureOne(Control root, Control c, StyleMap map, bool includeImage)
        {
            var path = BuildPath(root, c);
            var rec = map.Find(path) ?? new ControlStyleRecord { Path = path };

            rec.BackArgb = c.BackColor.IsEmpty ? (int?)null : c.BackColor.ToArgb();
            rec.ForeArgb = c.ForeColor.IsEmpty ? (int?)null : c.ForeColor.ToArgb();
            rec.Font = FontSpec.FromFont(c.Font);

            if (includeImage)
            {
                var tgi = TryGetImage(c);
                if (tgi.hasImage) rec.ImageBase64 = ImageBase64.ToPngBase64(tgi.img);
            }

            if (!rec.IsEmpty) map.Put(rec);
            else map.RemoveIfEmpty(path);
        }

        private static (bool hasImage, Image img) TryGetImage(Control c)
        {
            try
            {
                var pi = c.GetType().GetProperty("Image", BindingFlags.Public | BindingFlags.Instance);
                if (pi != null && typeof(Image).IsAssignableFrom(pi.PropertyType))
                {
                    var img = pi.GetValue(c, null) as Image;
                    return (true, img);
                }
            }
            catch { }
            return (false, null);
        }

        private static void TrySetImage(Control c, string b64)
        {
            try
            {
                var pi = c.GetType().GetProperty("Image", BindingFlags.Public | BindingFlags.Instance);
                if (pi != null && typeof(Image).IsAssignableFrom(pi.PropertyType))
                {
                    Image img = string.IsNullOrEmpty(b64) ? null : ImageBase64.FromPngBase64(b64);
                    pi.SetValue(c, img);
                }
            }
            catch { }
        }

        internal void ApplyTooltip(Control c, string text)
        {
            try { _tooltipWriter.SetToolTip(c, text ?? string.Empty); } catch { }
        }

        private void ApplyTooltipFromRecord(Control c, ControlStyleRecord rec)
        {
            if (rec?.Tooltip != null)
                ApplyTooltip(c, rec.Tooltip);
        }

        private static Padding MergePadding(Padding cur, PaddingSpec spec)
        {
            if (spec == null) return cur;
            return new Padding(
                spec.L ?? cur.Left,
                spec.T ?? cur.Top,
                spec.R ?? cur.Right,
                spec.B ?? cur.Bottom
            );
        }

        private static Size MergeSize(Size cur, SizeSpec spec)
        {
            if (spec == null) return cur;
            return new Size(spec.W ?? cur.Width, spec.H ?? cur.Height);
        }

        private void ApplyOne(Control c, ControlStyleRecord rec)
        {
            if (rec == null) return;

            // Style
            if (rec.BackArgb != null) { try { c.BackColor = Color.FromArgb(rec.BackArgb.Value); } catch { } }
            if (rec.ForeArgb != null) { try { c.ForeColor = Color.FromArgb(rec.ForeArgb.Value); } catch { } }
            if (rec.Font != null) { try { c.Font = rec.Font.ToFontSafe(); } catch { } }

            TrySetImage(c, rec.ImageBase64);
            ApplyTooltipFromRecord(c, rec);
            if (rec.Text != null) { try { c.Text = rec.Text; } catch { } }

            // State/Layout
            if (rec.Enabled != null) { try { c.Enabled = rec.Enabled.Value; } catch { } }
            if (rec.Visible != null) { try { c.Visible = rec.Visible.Value; } catch { } }

            try { c.Size = MergeSize(c.Size, rec.Size); } catch { }
            try { c.Margin = MergePadding(c.Margin, rec.Margin); } catch { }
            try { c.Padding = MergePadding(c.Padding, rec.Padding); } catch { }
        }

        private void ApplyRecursive(Control root, Control c, StyleMap map)
        {
            var path = BuildPath(root, c);
            ApplyOne(c, map.Find(path));
            foreach (Control ch in c.Controls) ApplyRecursive(root, ch, map);
        }

        #endregion

        #region IO + Export/Import/Refresh

        private string GetStyleFilePath()
        {
            var dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "BeeInterface");
            var file = $"{_key}.styles.json";
            return Path.Combine(dir, file);
        }

        public void ExportToFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath)) return;
            JsonUtil.Save(filePath, _map);
        }

        public void ImportFromFile(string filePath, bool alsoSaveToDefault = false)
        {
            if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath)) return;

            var newMap = JsonUtil.Load<StyleMap>(filePath);
            OverwriteAndApply(newMap);   // áp dụng NGAY
            if (alsoSaveToDefault) SaveExplicit();
        }

        public void RefreshApply()
        {
            if (_applying) return;
            _applying = true;
            try
            {
                ApplyRecursive(_host, _host, _map);
            }
            finally { _applying = false; }
        }

        #endregion
    }

    #region Small dialogs + Editors

    internal sealed class TextEntryDialog : Form
    {
        private readonly TextBox _tb;
        private readonly Button _ok, _cancel, _clear, _null;
        public string Result { get; private set; }

        public TextEntryDialog(string title, string initial, bool multiline)
        {
            Text = title;
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MinimizeBox = MaximizeBox = false;
            ShowInTaskbar = false;
            Width = 520;
            Height = multiline ? 360 : 180;

            _tb = new TextBox
            {
                Dock = DockStyle.Top,
                Multiline = multiline,
                Height = multiline ? 220 : 28,
                Text = initial ?? string.Empty,
                ScrollBars = multiline ? ScrollBars.Vertical : ScrollBars.None
            };

            var panel = new FlowLayoutPanel { Dock = DockStyle.Bottom, FlowDirection = FlowDirection.RightToLeft, Height = 40 };
            _ok = new Button { Text = "OK", DialogResult = DialogResult.OK, Width = 80 };
            _cancel = new Button { Text = "Cancel", DialogResult = DialogResult.Cancel, Width = 80 };
            _clear = new Button { Text = "Clear (\"\")", Width = 100 };
            _null = new Button { Text = "(null)", Width = 80 };
            _clear.Click += (s, e) => { _tb.Text = string.Empty; };
            _null.Click += (s, e) => { Result = "(null)"; DialogResult = DialogResult.OK; };

            panel.Controls.AddRange(new Control[] { _ok, _cancel, _clear, _null });
            Controls.Add(panel);
            Controls.Add(_tb);

            AcceptButton = _ok;
            CancelButton = _cancel;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.OK)
                Result = Result ?? _tb.Text;
            base.OnFormClosing(e);
        }
    }

    internal static class EditorUtils
    {
        public static (StyleEditorForm form, Control c, ControlStyleRecord rec) Unwrap(object instance)
        {
            var vm = instance as StyleEditorForm.StyleVM;
            if (vm == null) throw new InvalidOperationException("Unexpected component type.");
            return (vm.Owner, vm.ControlRef, vm.Record);
        }
    }

    internal sealed class ColorHexEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) => UITypeEditorEditStyle.Modal;
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            var (form, c, rec) = EditorUtils.Unwrap(context.Instance);
            int curArgb;
            try
            {
                var s = value as string;
                if (!string.IsNullOrWhiteSpace(s) && !s.Equals("(null)", StringComparison.OrdinalIgnoreCase) &&
                    int.TryParse(s, System.Globalization.NumberStyles.HexNumber, null, out var parsed))
                    curArgb = parsed;
                else
                    curArgb = context.PropertyDescriptor.DisplayName.StartsWith("Back", StringComparison.OrdinalIgnoreCase)
                              ? c.BackColor.ToArgb() : c.ForeColor.ToArgb();
            }
            catch { curArgb = c.ForeColor.ToArgb(); }

            using (var dlg = new ColorDialog { FullOpen = true, Color = Color.FromArgb(curArgb) })
            {
                if (dlg.ShowDialog(form) == DialogResult.OK)
                {
                    int argb = dlg.Color.ToArgb();
                    if (context.PropertyDescriptor.DisplayName.StartsWith("Back", StringComparison.OrdinalIgnoreCase))
                    { rec.BackArgb = argb; c.BackColor = dlg.Color; }
                    else { rec.ForeArgb = argb; c.ForeColor = dlg.Color; }
                    form.TouchWorking(rec);
                    return argb.ToString("X8");
                }
            }
            return value;
        }
    }

    internal sealed class FontSpecEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) => UITypeEditorEditStyle.Modal;
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            var (form, c, rec) = EditorUtils.Unwrap(context.Instance);
            using (var dlg = new FontDialog { Font = c.Font })
            {
                if (dlg.ShowDialog(form) == DialogResult.OK)
                {
                    rec.Font = FontSpec.FromFont(dlg.Font);
                    try { c.Font = dlg.Font; } catch { }
                    form.TouchWorking(rec);
                    return $"{dlg.Font.FontFamily.Name}, {dlg.Font.Size}pt, {dlg.Font.Style}";
                }
            }
            return value;
        }
    }

    internal sealed class ImagePickerEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) => UITypeEditorEditStyle.Modal;
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            var (form, c, rec) = EditorUtils.Unwrap(context.Instance);
            var choice = MessageBox.Show(form, "Yes=Load Image, No=Clear Image, Cancel=Abort", "Image",
                                         MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (choice == DialogResult.Yes)
            {
                using (var ofd = new OpenFileDialog { Filter = "Image Files|*.png;*.jpg;*.jpeg;*.bmp;*.gif" })
                {
                    if (ofd.ShowDialog(form) == DialogResult.OK)
                    {
                        using (var img = Image.FromFile(ofd.FileName))
                        {
                            rec.ImageBase64 = ImageBase64.ToPngBase64(img);
                            TrySetImage(c, rec.ImageBase64);
                            form.TouchWorking(rec);
                            return "(set)";
                        }
                    }
                }
            }
            else if (choice == DialogResult.No)
            {
                rec.ImageBase64 = null;
                TrySetImage(c, null);
                form.TouchWorking(rec);
                return "(cleared)";
            }
            return value ?? "(not set)";
        }

        private static void TrySetImage(Control c, string b64)
        {
            try
            {
                var pi = c.GetType().GetProperty("Image", BindingFlags.Public | BindingFlags.Instance);
                if (pi != null && typeof(Image).IsAssignableFrom(pi.PropertyType))
                {
                    Image img = string.IsNullOrEmpty(b64) ? null : ImageBase64.FromPngBase64(b64);
                    pi.SetValue(c, img);
                }
            }
            catch { }
        }
    }

    internal sealed class TextEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) => UITypeEditorEditStyle.Modal;
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            var (form, c, rec) = EditorUtils.Unwrap(context.Instance);
            var propName = context.PropertyDescriptor?.Name ?? string.Empty;
            bool isTooltip = propName.IndexOf("Tooltip", StringComparison.OrdinalIgnoreCase) >= 0;

            string current = isTooltip ? (rec.Tooltip ?? form.Persist.GetEffectiveTooltip(c))
                                       : (rec.Text ?? SafeGetText(c));

            using (var dlg = new TextEntryDialog(isTooltip ? "Edit Tooltip" : "Edit Text", current, isTooltip))
            {
                if (dlg.ShowDialog(form) == DialogResult.OK)
                {
                    var result = dlg.Result;
                    if (string.Equals(result, "(null)", StringComparison.OrdinalIgnoreCase))
                    {
                        if (isTooltip) rec.Tooltip = null; else rec.Text = null;
                        form.TouchWorking(rec);
                        return isTooltip ? "(null)" : (rec.Text ?? SafeGetText(c));
                    }
                    else
                    {
                        if (isTooltip) { rec.Tooltip = result; form.Persist.ApplyTooltip(c, result); }
                        else { rec.Text = result; try { c.Text = result; } catch { } }
                        form.TouchWorking(rec);
                        return result;
                    }
                }
            }
            return value;
        }

        private static string SafeGetText(Control c) { try { return c.Text; } catch { return string.Empty; } }
    }

    #endregion

    #region Style Editor Form (UI)

    internal sealed class StyleEditorForm : Form
    {
        internal ControlStylePersistence Persist => _persist;
        internal readonly ControlStylePersistence _persist;
        private readonly Control _root;

        private TreeView _tree;
        private PropertyGrid _grid;
        private Button _btnSave, _btnReset, _btnResetAll, _btnClose;
        private Button _btnImport, _btnExport;
        private PictureBox _preview;
        private Label _lblSel;

        private Control _selectedControl;
        private StyleMap _working;

        public StyleEditorForm(ControlStylePersistence persist, Control root)
        {
            _persist = persist; _root = root;

            Text = "Style & Layout Editor — Save to persist";
            FormBorderStyle = FormBorderStyle.Sizable;
            MinimumSize = new Size(1000, 620);

            BuildUi();
            LoadData();
        }

        private void BuildUi()
        {
            var mainSplit = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Vertical,
                SplitterDistance = 260
            };
            Controls.Add(mainSplit);

            _tree = new TreeView { Dock = DockStyle.Fill, HideSelection = false };
            _tree.AfterSelect += (s, e) => { _selectedControl = e.Node?.Tag as Control; UpdateSelectionPanel(); };
            mainSplit.Panel1.Controls.Add(_tree);

            var right = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 5 };
            right.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60));
            right.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
            right.RowStyles.Add(new RowStyle(SizeType.Absolute, 28));
            right.RowStyles.Add(new RowStyle(SizeType.Percent, 60));
            right.RowStyles.Add(new RowStyle(SizeType.Absolute, 120));
            right.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));
            right.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));
            mainSplit.Panel2.Controls.Add(right);

            _lblSel = new Label { Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft };
            right.Controls.Add(_lblSel, 0, 0);
            right.SetColumnSpan(_lblSel, 2);

            _grid = new PropertyGrid { Dock = DockStyle.Fill, ToolbarVisible = false };
            right.Controls.Add(_grid, 0, 1);
            right.SetColumnSpan(_grid, 2);

            _preview = new PictureBox
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.White
            };
            right.Controls.Add(_preview, 0, 2);
            right.SetColumnSpan(_preview, 2);

            var pnlBtn = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.LeftToRight };
            _btnSave = new Button { Text = "Save" };
            _btnImport = new Button { Text = "Import..." };
            _btnExport = new Button { Text = "Export..." };
            _btnReset = new Button { Text = "Reset (Default)" };
            _btnResetAll = new Button { Text = "Reset All" };
            _btnClose = new Button { Text = "Close" };
            pnlBtn.Controls.AddRange(new Control[] { _btnSave, _btnImport, _btnExport, _btnReset, _btnResetAll, _btnClose });
            right.Controls.Add(pnlBtn, 0, 4);
            right.SetColumnSpan(pnlBtn, 2);

            _btnSave.Click += (s, e) =>
            {
                PruneWorkingAgainstBaseline();          // <<— PRUNE theo baseline
                _persist.OverwriteAndApply(_working);
                _persist.SaveExplicit();
                System.Media.SystemSounds.Asterisk.Play();
            };

            _btnImport.Click += (s, e) =>
            {
                using (var ofd = new OpenFileDialog { Filter = "JSON (*.json)|*.json" })
                {
                    if (ofd.ShowDialog(this) == DialogResult.OK)
                    {
                        var confirm = MessageBox.Show(
                            this,
                            "Import style từ file:\r\n" + ofd.FileName +
                            "\r\n\r\nThao tác này sẽ ghi đè các thiết lập hiện tại đang áp dụng lên giao diện.\r\nBạn có chắc muốn tiếp tục?",
                            "Xác nhận Import",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning);

                        if (confirm != DialogResult.Yes) return;

                        try
                        {
                            _persist.ImportFromFile(ofd.FileName, alsoSaveToDefault: false);
                            _working = CloneMap(_persist.Snapshot());
                            RebuildTreeAndSelection();
                            System.Media.SystemSounds.Asterisk.Play();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(this, "Import failed:\r\n" + ex.Message, "Import", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            };

            _btnExport.Click += (s, e) =>
            {
                using (var sfd = new SaveFileDialog { Filter = "JSON (*.json)|*.json", FileName = "styles.export.json" })
                {
                    if (sfd.ShowDialog(this) == DialogResult.OK)
                    {
                        try
                        {
                            PruneWorkingAgainstBaseline();      // <<— PRUNE theo baseline
                            _persist.OverwriteAndApply(_working);
                            _persist.ExportToFile(sfd.FileName);
                            System.Media.SystemSounds.Asterisk.Play();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(this, "Export failed:\r\n" + ex.Message, "Export", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            };

            _btnReset.Click += (s, e) => ResetCurrent();
            _btnResetAll.Click += (s, e) => ResetAll();
            _btnClose.Click += (s, e) => Close();
        }

        private void LoadData()
        {
            _working = CloneMap(_persist.Snapshot());
            BuildTree(_root, _tree.Nodes, _root);
            _tree.ExpandAll();
            if (_tree.Nodes.Count > 0) _tree.SelectedNode = _tree.Nodes[0];
        }

        private static StyleMap CloneMap(StyleMap src)
        {
            using (var ms = new MemoryStream())
            {
                var ser = new DataContractJsonSerializer(typeof(StyleMap));
                ser.WriteObject(ms, src ?? new StyleMap());
                ms.Position = 0;
                return (StyleMap)ser.ReadObject(ms);
            }
        }

        private void BuildTree(Control root, TreeNodeCollection nodes, Control current)
        {
            var node = new TreeNode(TextFor(current)) { Tag = current };
            nodes.Add(node);
            foreach (Control ch in current.Controls) BuildTree(root, node.Nodes, ch);
        }

        private string TextFor(Control c)
            => $"{(string.IsNullOrWhiteSpace(c.Name) ? "(no-name)" : c.Name)}  [{c.GetType().Name}]";

        private static (bool hasImage, Image img) TryGetImage(Control c)
        {
            try
            {
                var pi = c.GetType().GetProperty("Image", BindingFlags.Public | BindingFlags.Instance);
                if (pi != null && typeof(Image).IsAssignableFrom(pi.PropertyType))
                {
                    var img = pi.GetValue(c, null) as Image;
                    return (true, img);
                }
            }
            catch { }
            return (false, null);
        }

        private static string CurrentImageBase64(Control c)
        {
            var t = TryGetImage(c);
            return t.hasImage ? ImageBase64.ToPngBase64(t.img) : null;
        }

        private void UpdateSelectionPanel()
        {
            var c = _selectedControl;
            if (c == null)
            {
                _lblSel.Text = "No control selected";
                _grid.SelectedObject = null;
                _preview.Image = null;
                return;
            }

            var path = ControlStylePersistence.BuildPath(_root, c);
            var rec = _working.Find(path) ?? new ControlStyleRecord { Path = path };
            var vm = new StyleVM(this, c, rec);
            _grid.SelectedObject = vm;

            _lblSel.Text = $"Selected: {c.Name} [{c.GetType().Name}]  |  Path: {path}";
            var tgi = TryGetImage(c);
            _preview.Image = tgi.hasImage ? tgi.img : null;
        }

        private void ResetCurrent()
        {
            if (_selectedControl == null) return;
            var path = ControlStylePersistence.BuildPath(_root, _selectedControl);
            _working.Items.RemoveAll(r => r.Path.StartsWith(path, StringComparison.OrdinalIgnoreCase));
            _persist.RestoreBaseline(_selectedControl);
            _persist.OverwriteAndApply(_working);
            UpdateSelectionPanel();
        }

        private void ResetAll()
        {
            _working.Items.Clear();
            _persist.RestoreBaseline(_root);
            _persist.OverwriteAndApply(_working);
            UpdateSelectionPanel();
        }

        private void RebuildTreeAndSelection()
        {
            _tree.BeginUpdate();
            try
            {
                _tree.Nodes.Clear();
                BuildTree(_root, _tree.Nodes, _root);
                _tree.ExpandAll();
                if (_tree.Nodes.Count > 0) _tree.SelectedNode = _tree.Nodes[0];
            }
            finally { _tree.EndUpdate(); }
            UpdateSelectionPanel();
        }

        internal void TouchWorking(ControlStyleRecord rec)
        {
            if (rec == null) return;
            _working.Put(rec);
            _grid.Refresh();
        }

        private static bool TryParseLTRB(string input, out int L, out int T, out int R, out int B)
        {
            L = T = R = B = 0;
            if (string.IsNullOrWhiteSpace(input)) return false;
            var parts = input.Split(new[] { ',', ' ', ';' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 4) return false;
            return int.TryParse(parts[0], out L)
                && int.TryParse(parts[1], out T)
                && int.TryParse(parts[2], out R)
                && int.TryParse(parts[3], out B);
        }

        private static bool TryParseWH(string input, out int W, out int H)
        {
            W = H = 0;
            if (string.IsNullOrWhiteSpace(input)) return false;
            var parts = input.Split(new[] { ',', ' ', 'x', 'X', ';' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2) return false;
            return int.TryParse(parts[0], out W)
                && int.TryParse(parts[1], out H);
        }

        /// <summary>
        /// PRUNE: gỡ mọi giá trị bằng baseline hoặc null khỏi _working; xoá record rỗng.
        /// </summary>
        private void PruneWorkingAgainstBaseline()
        {
            var toRemove = new List<ControlStyleRecord>();

            WalkControls(_root, c =>
            {
                var path = ControlStylePersistence.BuildPath(_root, c);
                var rec = _working.Find(path);
                if (rec == null) return;

                // Không có baseline (control dynamic sau khi capture) → không prune để giữ data
                if (!_persist.TryGetBaseline(path, out var baseline))
                    return;

                // ==== Style ====
                if (rec.BackArgb != null && baseline.BackArgb != null && rec.BackArgb.Value == baseline.BackArgb.Value)
                    rec.BackArgb = null;

                if (rec.ForeArgb != null && baseline.ForeArgb != null && rec.ForeArgb.Value == baseline.ForeArgb.Value)
                    rec.ForeArgb = null;

                if (rec.Font != null && baseline.Font != null && rec.Font.EqualsTo(baseline.Font.ToFontSafe()))
                    rec.Font = null;

                // Image
                if (rec.ImageBase64 != null && baseline.ImageBase64 != null)
                {
                    if (string.Equals(rec.ImageBase64, baseline.ImageBase64, StringComparison.Ordinal))
                        rec.ImageBase64 = null;
                }

                // Tooltip
                if (rec.Tooltip != null)
                {
                    var baseTip = baseline.Tooltip ?? string.Empty;
                    if (string.Equals(rec.Tooltip ?? string.Empty, baseTip, StringComparison.Ordinal))
                        rec.Tooltip = null;
                }

                // Text
                if (rec.Text != null)
                {
                    var baseText = baseline.Text ?? string.Empty;
                    if (string.Equals(rec.Text ?? string.Empty, baseText, StringComparison.Ordinal))
                        rec.Text = null;
                }

                // ==== State/Layout ====
                if (rec.Enabled != null && baseline.Enabled != null && rec.Enabled.Value == baseline.Enabled.Value)
                    rec.Enabled = null;

                if (rec.Visible != null && baseline.Visible != null && rec.Visible.Value == baseline.Visible.Value)
                    rec.Visible = null;

                if (rec.Size != null)
                {
                    if (baseline.Size != null)
                    {
                        if (rec.Size.W != null && baseline.Size.W != null && rec.Size.W.Value == baseline.Size.W.Value)
                            rec.Size.W = null;
                        if (rec.Size.H != null && baseline.Size.H != null && rec.Size.H.Value == baseline.Size.H.Value)
                            rec.Size.H = null;
                    }
                    if (rec.Size.IsEmpty) rec.Size = null;
                }

                if (rec.Margin != null)
                {
                    if (baseline.Margin != null)
                    {
                        if (rec.Margin.L != null && baseline.Margin.L != null && rec.Margin.L.Value == baseline.Margin.L.Value) rec.Margin.L = null;
                        if (rec.Margin.T != null && baseline.Margin.T != null && rec.Margin.T.Value == baseline.Margin.T.Value) rec.Margin.T = null;
                        if (rec.Margin.R != null && baseline.Margin.R != null && rec.Margin.R.Value == baseline.Margin.R.Value) rec.Margin.R = null;
                        if (rec.Margin.B != null && baseline.Margin.B != null && rec.Margin.B.Value == baseline.Margin.B.Value) rec.Margin.B = null;
                    }
                    if (rec.Margin.IsEmpty) rec.Margin = null;
                }

                if (rec.Padding != null)
                {
                    if (baseline.Padding != null)
                    {
                        if (rec.Padding.L != null && baseline.Padding.L != null && rec.Padding.L.Value == baseline.Padding.L.Value) rec.Padding.L = null;
                        if (rec.Padding.T != null && baseline.Padding.T != null && rec.Padding.T.Value == baseline.Padding.T.Value) rec.Padding.T = null;
                        if (rec.Padding.R != null && baseline.Padding.R != null && rec.Padding.R.Value == baseline.Padding.R.Value) rec.Padding.R = null;
                        if (rec.Padding.B != null && baseline.Padding.B != null && rec.Padding.B.Value == baseline.Padding.B.Value) rec.Padding.B = null;
                    }
                    if (rec.Padding.IsEmpty) rec.Padding = null;
                }

                if (rec.IsEmpty) toRemove.Add(rec);
            });

            if (toRemove.Count > 0)
            {
                foreach (var r in toRemove) _working.Items.Remove(r);
            }
        }

        private static void WalkControls(Control root, Action<Control> action)
        {
            if (root == null || action == null) return;
            var stack = new Stack<Control>();
            stack.Push(root);
            while (stack.Count > 0)
            {
                var c = stack.Pop();
                action(c);
                for (int i = c.Controls.Count - 1; i >= 0; --i)
                    stack.Push(c.Controls[i]);
            }
        }

        // ====== ViewModel for PropertyGrid ======
        internal sealed class StyleVM
        {
            internal StyleEditorForm Owner { get; }
            internal Control ControlRef { get; }
            internal ControlStyleRecord Record { get; }

            public StyleVM(StyleEditorForm owner, Control c, ControlStyleRecord rec)
            {
                Owner = owner; ControlRef = c; Record = rec;
            }

            // ------ Current (effective)
            [Category("Current"), ReadOnly(true)]
            public string Control => $"{ControlRef.Name} [{ControlRef.GetType().Name}]";

            [Category("Current"), ReadOnly(true)] public string EffBackColor => ControlRef.BackColor.ToString();
            [Category("Current"), ReadOnly(true)] public string EffForeColor => ControlRef.ForeColor.ToString();
            [Category("Current"), ReadOnly(true)] public string EffFont => $"{ControlRef.Font?.FontFamily?.Name}, {ControlRef.Font?.Size}pt, {ControlRef.Font?.Style}";
            [Category("Current"), ReadOnly(true)] public string EffText { get { try { return ControlRef.Text; } catch { return ""; } } }
            [Category("Current"), ReadOnly(true)] public string EffTooltip => Owner._persist.GetEffectiveTooltip(ControlRef);
            [Category("Current"), ReadOnly(true)] public bool EffEnabled => ControlRef.Enabled;
            [Category("Current"), ReadOnly(true)] public bool EffVisible => ControlRef.Visible;
            [Category("Current"), ReadOnly(true)] public string EffSize => $"{ControlRef.Width} x {ControlRef.Height}";
            [Category("Current"), ReadOnly(true)] public string EffMargin => $"{ControlRef.Margin.Left},{ControlRef.Margin.Top},{ControlRef.Margin.Right},{ControlRef.Margin.Bottom}";
            [Category("Current"), ReadOnly(true)] public string EffPadding => $"{ControlRef.Padding.Left},{ControlRef.Padding.Top},{ControlRef.Padding.Right},{ControlRef.Padding.Bottom}";

            // ------ Override (StyleMap)

            [Category("Override (StyleMap)"), DisplayName("Back ARGB (HEX)"), Editor(typeof(ColorHexEditor), typeof(UITypeEditor))]
            public string BackArgbHex
            {
                get { int argb = Record.BackArgb ?? ControlRef.BackColor.ToArgb(); return argb.ToString("X8"); }
                set
                {
                    if (string.IsNullOrWhiteSpace(value) || value.Trim().Equals("(null)", StringComparison.OrdinalIgnoreCase))
                    { Record.BackArgb = null; Owner.TouchWorking(Record); return; }

                    if (int.TryParse(value, System.Globalization.NumberStyles.HexNumber, null, out var argb))
                    {
                        Record.BackArgb = argb;
                        try { ControlRef.BackColor = Color.FromArgb(argb); } catch { }
                        Owner.TouchWorking(Record);
                    }
                }
            }

            [Category("Override (StyleMap)"), DisplayName("Fore ARGB (HEX)"), Editor(typeof(ColorHexEditor), typeof(UITypeEditor))]
            public string ForeArgbHex
            {
                get { int argb = Record.ForeArgb ?? ControlRef.ForeColor.ToArgb(); return argb.ToString("X8"); }
                set
                {
                    if (string.IsNullOrWhiteSpace(value) || value.Trim().Equals("(null)", StringComparison.OrdinalIgnoreCase))
                    { Record.ForeArgb = null; Owner.TouchWorking(Record); return; }

                    if (int.TryParse(value, System.Globalization.NumberStyles.HexNumber, null, out var argb))
                    {
                        Record.ForeArgb = argb;
                        try { ControlRef.ForeColor = Color.FromArgb(argb); } catch { }
                        Owner.TouchWorking(Record);
                    }
                }
            }

            [Category("Override (StyleMap)"), DisplayName("Font (editor)"), Editor(typeof(FontSpecEditor), typeof(UITypeEditor)), ReadOnly(true)]
            public string FontEditor
            {
                get { var f = Record.Font?.ToFontSafe() ?? ControlRef.Font; return $"{f?.FontFamily?.Name}, {f?.Size}pt, {f?.Style}"; }
            }

            [Category("Override (StyleMap)"), DisplayName("Image (editor)"), Editor(typeof(ImagePickerEditor), typeof(UITypeEditor)), ReadOnly(true)]
            public string ImageEditor => string.IsNullOrEmpty(Record.ImageBase64) ? "(not set)" : "(set)";

            [Category("Override (StyleMap)"), DisplayName("Tooltip"), Editor(typeof(TextEditor), typeof(UITypeEditor))]
            public string Tooltip
            {
                get => Record.Tooltip ?? Owner._persist.GetEffectiveTooltip(ControlRef);
                set
                {
                    if (!string.IsNullOrEmpty(value) && value.Trim().Equals("(null)", StringComparison.OrdinalIgnoreCase)) { Record.Tooltip = null; Owner.TouchWorking(Record); return; }
                    Record.Tooltip = value; Owner._persist.ApplyTooltip(ControlRef, value); Owner.TouchWorking(Record);
                }
            }

            [Category("Override (StyleMap)"), DisplayName("Text"), Editor(typeof(TextEditor), typeof(UITypeEditor))]
            public string Text
            {
                get { try { return Record.Text ?? ControlRef.Text; } catch { return Record.Text ?? string.Empty; } }
                set
                {
                    if (!string.IsNullOrEmpty(value) && value.Trim().Equals("(null)", StringComparison.OrdinalIgnoreCase)) { Record.Text = null; Owner.TouchWorking(Record); return; }
                    Record.Text = value; try { ControlRef.Text = value; } catch { }
                    Owner.TouchWorking(Record);
                }
            }

            [Category("Override (StyleMap)"), DisplayName("Enabled")]
            public bool Enabled
            {
                get => Record.Enabled ?? ControlRef.Enabled;
                set
                {
                    Record.Enabled = value;
                    try { ControlRef.Enabled = value; } catch { }
                    Owner.TouchWorking(Record);
                }
            }

            [Category("Override (StyleMap)"), DisplayName("Visible")]
            public bool Visible
            {
                get => Record.Visible ?? ControlRef.Visible;
                set
                {
                    Record.Visible = value;
                    try { ControlRef.Visible = value; } catch { }
                    Owner.TouchWorking(Record);
                }
            }

            [Category("Override (StyleMap)"), DisplayName("Margin LTRB (l,t,r,b)")]
            public string MarginLTRB
            {
                get
                {
                    var cur = ControlRef.Margin;
                    var m = Record.Margin;
                    int L = m?.L ?? cur.Left;
                    int T = m?.T ?? cur.Top;
                    int R = m?.R ?? cur.Right;
                    int B = m?.B ?? cur.Bottom;
                    return $"{L},{T},{R},{B}";
                }
                set
                {
                    if (!StyleEditorForm.TryParseLTRB(value, out int L, out int T, out int R, out int B)) return;
                    if (Record.Margin == null) Record.Margin = new PaddingSpec();
                    Record.Margin.L = L; Record.Margin.T = T; Record.Margin.R = R; Record.Margin.B = B;
                    try { ControlRef.Margin = new Padding(L, T, R, B); } catch { }
                    Owner.TouchWorking(Record);
                }
            }

            [Category("Override (StyleMap)"), DisplayName("Padding LTRB (l,t,r,b)")]
            public string PaddingLTRB
            {
                get
                {
                    var cur = ControlRef.Padding;
                    var p = Record.Padding;
                    int L = p?.L ?? cur.Left;
                    int T = p?.T ?? cur.Top;
                    int R = p?.R ?? cur.Right;
                    int B = p?.B ?? cur.Bottom;
                    return $"{L},{T},{R},{B}";
                }
                set
                {
                    if (!StyleEditorForm.TryParseLTRB(value, out int L, out int T, out int R, out int B)) return;
                    if (Record.Padding == null) Record.Padding = new PaddingSpec();
                    Record.Padding.L = L; Record.Padding.T = T; Record.Padding.R = R; Record.Padding.B = B;
                    try { ControlRef.Padding = new Padding(L, T, R, B); } catch { }
                    Owner.TouchWorking(Record);
                }
            }

            [Category("Override (StyleMap)"), DisplayName("Size (w,h)")]
            public string SizeWH
            {
                get
                {
                    var cur = ControlRef.Size;
                    var s = Record.Size;
                    int w = s?.W ?? cur.Width;
                    int h = s?.H ?? cur.Height;
                    return $"{w},{h}";
                }
                set
                {
                    if (!StyleEditorForm.TryParseWH(value, out int w, out int h)) return;
                    if (Record.Size == null) Record.Size = new SizeSpec();
                    Record.Size.W = w; Record.Size.H = h;
                    try { ControlRef.Size = new Size(w, h); } catch { }
                    Owner.TouchWorking(Record);
                }
            }
        }
    }

    #endregion
}
