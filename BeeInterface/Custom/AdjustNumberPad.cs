using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace BeeGlobal
{
    internal class NumberPadButton : Button
    {
        public NumberPadButton()
        {
            SetStyle(ControlStyles.Selectable, false);
            TabStop = false;
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 1;
            FlatAppearance.BorderColor = Color.FromArgb(85, 85, 85);
            BackColor = Color.FromArgb(58, 58, 58);
            ForeColor = Color.White;
            UseMnemonic = false;
            CausesValidation = false;
            TextAlign = ContentAlignment.MiddleCenter;
            Cursor = Cursors.Hand;
        }

        protected override bool ShowFocusCues
        {
            get { return false; }
        }
    }

    public class AdjustNumberPad : UserControl
    {
        // ===== EVENT =====
        public event Action<string> KeyPadPressed;
        public event Action EnterPressed;

        // ===== OPTION =====
        public bool SimulateKeyboard { get; set; } = true;
        public bool DeleteAsBackspace { get; set; } = true;
        public bool AllowDragMove { get; set; } = true;
        public bool AllowResizePad { get; set; } = true;
        public bool AllowDot { get; set; } = true;
        public bool KeepSquareLayout { get; set; } = true;

        public bool SaveLayoutOnDisk { get; set; } = true;
        public bool SaveLocationOnDisk { get; set; } = true;
        public bool SaveSizeOnDisk { get; set; } = true;
        public string LayoutKey { get; set; } = "Default";

        public Font ButtonFont { get; set; } = new Font("Segoe UI", 12F, FontStyle.Bold);
        public Font HeaderFont { get; set; } = new Font("Segoe UI", 9F, FontStyle.Bold);

        // ===== PRIVATE =====
        private Control _lastTargetControl;
        private Form _hookedForm;

        private bool _dragging;
        private Point _dragStart;

        private bool _resizing;
        private Point _resizeMouseStart;
        private Size _resizeSizeStart;

        private bool _internalResize;
        private bool _layoutLoaded;
        private bool _suspendSave;

        private Timer _saveTimer;

        private TableLayoutPanel _root;
        private Panel _header;
        private Label _title;
        private TableLayoutPanel _grid;
        private Label _grip;

        // ===== BUTTON =====
        private NumberPadButton btn0, btn1, btn2, btn3, btn4, btn5, btn6, btn7, btn8, btn9;
        private NumberPadButton btnDot, btnDelete, btnEnter;

        public AdjustNumberPad()
        {
            SetStyle(ControlStyles.Selectable, false);
            SetStyle(ControlStyles.ResizeRedraw, true);
            DoubleBuffered = true;
            TabStop = false;

            this.BackColor = Color.FromArgb(36, 36, 36);
            this.BorderStyle = BorderStyle.FixedSingle;
            this.MinimumSize = new Size(50, 50);
            this.Size = new Size(110, 130);

            InitUI();
            InitSaveTimer();

            this.ParentChanged += AdjustNumberPad_ParentChanged;
            this.VisibleChanged += AdjustNumberPad_VisibleChanged;
            this.SizeChanged += AdjustNumberPad_SizeChanged;
            this.LocationChanged += AdjustNumberPad_LocationChanged;
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            AttachToHostForm();
            TryLoadLayoutOnce();
        }

        private void InitSaveTimer()
        {
            _saveTimer = new Timer();
            _saveTimer.Interval = 250;
            _saveTimer.Tick += delegate
            {
                _saveTimer.Stop();
                SaveLayout();
            };
        }

        private void AdjustNumberPad_ParentChanged(object sender, EventArgs e)
        {
            AttachToHostForm();
            TryLoadLayoutOnce();
        }

        private void AdjustNumberPad_VisibleChanged(object sender, EventArgs e)
        {
            AttachToHostForm();
            TryLoadLayoutOnce();
        }

        private void AdjustNumberPad_SizeChanged(object sender, EventArgs e)
        {
            if (KeepSquareLayout)
                ApplyPreferredHeightFromWidth();

            ScheduleSaveLayout();
        }

        private void AdjustNumberPad_LocationChanged(object sender, EventArgs e)
        {
            ScheduleSaveLayout();
        }

        // =========================================================
        // UI
        // =========================================================
        private void InitUI()
        {
            SuspendLayout();

            _root = new TableLayoutPanel();
            _root.Dock = DockStyle.Fill;
            _root.Margin = new Padding(0);
            _root.Padding = new Padding(0);
            _root.ColumnCount = 1;
            _root.RowCount = 2;
            _root.BackColor = Color.FromArgb(36, 36, 36);
            _root.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            _root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            _header = new Panel();
            _header.Dock = DockStyle.Fill;
            _header.Margin = new Padding(0);
            _header.Padding = new Padding(8, 0, 8, 0);
            _header.BackColor = Color.FromArgb(48, 48, 48);

            _title = new Label();
            _title.Dock = DockStyle.Fill;
            _title.Text = "Number Pad";
            _title.TextAlign = ContentAlignment.MiddleLeft;
            _title.Font = HeaderFont;
            _title.ForeColor = Color.Gainsboro;

            _header.Controls.Add(_title);

            _grid = new TableLayoutPanel();
            _grid.Dock = DockStyle.Fill;
            _grid.Margin = new Padding(6);
            _grid.Padding = new Padding(0);
            _grid.ColumnCount = 4;
            _grid.RowCount = 4;
            _grid.BackColor = Color.FromArgb(36, 36, 36);

            _grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            _grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            _grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            _grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));

            _grid.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            _grid.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            _grid.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            _grid.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));

            btn7 = CreateBtn("7", "7");
            btn8 = CreateBtn("8", "8");
            btn9 = CreateBtn("9", "9");
            btnDelete = CreateBtn("⌫", "Delete");

            btn4 = CreateBtn("4", "4");
            btn5 = CreateBtn("5", "5");
            btn6 = CreateBtn("6", "6");
            btnEnter = CreateBtn("Enter", "Enter");

            btn1 = CreateBtn("1", "1");
            btn2 = CreateBtn("2", "2");
            btn3 = CreateBtn("3", "3");

            btn0 = CreateBtn("0", "0");
            btnDot = CreateBtn(".", ".");

            AddButton(btn7, 0, 0);
            AddButton(btn8, 1, 0);
            AddButton(btn9, 2, 0);
            AddButton(btnDelete, 3, 0);

            AddButton(btn4, 0, 1);
            AddButton(btn5, 1, 1);
            AddButton(btn6, 2, 1);

            AddButton(btn1, 0, 2);
            AddButton(btn2, 1, 2);
            AddButton(btn3, 2, 2);

            AddButton(btn0, 0, 3);
            _grid.SetColumnSpan(btn0, 2);

            AddButton(btnDot, 2, 3);

            AddButton(btnEnter, 3, 1);
            _grid.SetRowSpan(btnEnter, 3);

            _grip = new Label();
            _grip.AutoSize = false;
            _grip.Size = new Size(18, 18);
            _grip.Text = "◢";
            _grip.TextAlign = ContentAlignment.BottomRight;
            _grip.ForeColor = Color.Silver;
            _grip.BackColor = Color.Transparent;
            _grip.Cursor = Cursors.SizeNWSE;
            _grip.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            _grip.Location = new Point(this.Width - 20, this.Height - 20);

            _grip.MouseDown += Grip_MouseDown;
            _grip.MouseMove += Grip_MouseMove;
            _grip.MouseUp += Grip_MouseUp;

            this.Controls.Add(_grip);
            this.Controls.Add(_root);

            _root.Controls.Add(_header, 0, 0);
            _root.Controls.Add(_grid, 0, 1);

            HookDrag(_header);
            HookDrag(_title);

            ResumeLayout(false);

            UpdateGripPosition();
            ApplyPreferredHeightFromWidth();
        }

        private NumberPadButton CreateBtn(string text, string key)
        {
            NumberPadButton btn = new NumberPadButton();
            btn.Text = text;
            btn.Tag = key;
            btn.Dock = DockStyle.Fill;
            btn.Margin = new Padding(4);
            btn.Font = ButtonFont;

            btn.MouseDown += Btn_MouseDown;
            btn.MouseEnter += Btn_MouseEnter;
            btn.MouseLeave += Btn_MouseLeave;

            return btn;
        }

        private void AddButton(Control c, int col, int row)
        {
            _grid.Controls.Add(c, col, row);
        }

        private void Btn_MouseEnter(object sender, EventArgs e)
        {
            NumberPadButton btn = sender as NumberPadButton;
            if (btn == null) return;
            btn.BackColor = Color.FromArgb(72, 72, 72);
        }

        private void Btn_MouseLeave(object sender, EventArgs e)
        {
            NumberPadButton btn = sender as NumberPadButton;
            if (btn == null) return;
            btn.BackColor = Color.FromArgb(58, 58, 58);
        }

        private void Btn_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            RememberCurrentActiveControl();

            NumberPadButton btn = sender as NumberPadButton;
            if (btn == null) return;

            string key = btn.Tag as string;
            ProcessVirtualKey(key);
        }

        // =========================================================
        // RESIZE
        // =========================================================
        private void Grip_MouseDown(object sender, MouseEventArgs e)
        {
            if (!AllowResizePad) return;
            if (e.Button != MouseButtons.Left) return;

            _resizing = true;
            _resizeMouseStart = Control.MousePosition;
            _resizeSizeStart = this.Size;
        }

        private void Grip_MouseMove(object sender, MouseEventArgs e)
        {
            if (!AllowResizePad) return;
            if (!_resizing) return;

            Point cur = Control.MousePosition;
            int dx = cur.X - _resizeMouseStart.X;
            int dy = cur.Y - _resizeMouseStart.Y;

            int newW = _resizeSizeStart.Width + dx;
            int newH = _resizeSizeStart.Height + dy;

            if (newW < this.MinimumSize.Width) newW = this.MinimumSize.Width;
            if (newH < this.MinimumSize.Height) newH = this.MinimumSize.Height;

            if (KeepSquareLayout)
            {
                this.Size = new Size(newW, GetPreferredHeightFromWidth(newW));
            }
            else
            {
                this.Size = new Size(newW, newH);
            }

            UpdateGripPosition();
        }

        private void Grip_MouseUp(object sender, MouseEventArgs e)
        {
            _resizing = false;
            SaveLayout();
        }

        private void ApplyPreferredHeightFromWidth()
        {
            if (!KeepSquareLayout) return;
            if (_internalResize) return;

            int newH = GetPreferredHeightFromWidth(this.Width);
            if (this.Height == newH)
            {
                UpdateGripPosition();
                return;
            }

            _internalResize = true;
            try
            {
                this.Height = Math.Max(this.MinimumSize.Height, newH);
            }
            finally
            {
                _internalResize = false;
            }

            UpdateGripPosition();
        }

        private int GetPreferredHeightFromWidth(int width)
        {
            // header 28 + viền/margin khoảng 12
            int h = width + 40;
            if (h < this.MinimumSize.Height) h = this.MinimumSize.Height;
            return h;
        }

        private void UpdateGripPosition()
        {
            if (_grip == null) return;
            _grip.Location = new Point(this.Width - _grip.Width - 2, this.Height - _grip.Height - 2);
            _grip.BringToFront();
        }

        // =========================================================
        // CORE INPUT
        // =========================================================
        private void ProcessVirtualKey(string key)
        {
            KeyPadPressed?.Invoke(key);

            bool handled = false;

            if (SimulateKeyboard)
            {
                handled = SendToTarget(key);

                if (!handled)
                    SendFallback(key);
            }

            if (key == "Enter")
                EnterPressed?.Invoke();

            RestoreFocusToTarget();
        }

        private bool SendToTarget(string key)
        {
            if (_lastTargetControl == null || _lastTargetControl.IsDisposed)
                return false;

            if (_lastTargetControl is TextBoxBase)
                return HandleTextBox((TextBoxBase)_lastTargetControl, key);

            if (_lastTargetControl is ComboBox)
                return HandleCombo((ComboBox)_lastTargetControl, key);

            return false;
        }

        private bool HandleTextBox(TextBoxBase tb, string key)
        {
            if (!tb.Enabled || tb.ReadOnly) return false;

            tb.Focus();

            if (key == "Enter")
                return false;

            if (key == "Delete")
            {
                int s = tb.SelectionStart;
                int l = tb.SelectionLength;
                string text = tb.Text ?? "";

                if (l > 0)
                {
                    tb.Text = text.Remove(s, l);
                }
                else if (DeleteAsBackspace && s > 0)
                {
                    tb.Text = text.Remove(s - 1, 1);
                    s--;
                }

                tb.SelectionStart = s;
                tb.SelectionLength = 0;
                return true;
            }

            if (key == ".")
            {
                if (!AllowDot) return true;
                if ((tb.Text ?? "").Contains(".")) return true;
            }

            InsertText(tb, key);
            return true;
        }

        private bool HandleCombo(ComboBox cb, string key)
        {
            if (!cb.Enabled || cb.DropDownStyle == ComboBoxStyle.DropDownList)
                return false;

            cb.Focus();

            if (key == "Enter")
                return false;

            int s = cb.SelectionStart;
            int l = cb.SelectionLength;
            string t = cb.Text ?? "";

            if (key == "Delete")
            {
                if (l > 0)
                {
                    t = t.Remove(s, l);
                }
                else if (DeleteAsBackspace && s > 0)
                {
                    t = t.Remove(s - 1, 1);
                    s--;
                }

                cb.Text = t;
                cb.SelectionStart = s;
                cb.SelectionLength = 0;
                return true;
            }

            if (key == ".")
            {
                if (!AllowDot) return true;
                if (t.Contains(".")) return true;
            }

            if (l > 0)
                t = t.Remove(s, l);

            t = t.Insert(s, key);

            cb.Text = t;
            cb.SelectionStart = s + key.Length;
            cb.SelectionLength = 0;
            return true;
        }

        private void InsertText(TextBoxBase tb, string text)
        {
            int s = tb.SelectionStart;
            int l = tb.SelectionLength;
            string t = tb.Text ?? "";

            if (l > 0)
                t = t.Remove(s, l);

            t = t.Insert(s, text);

            tb.Text = t;
            tb.SelectionStart = s + text.Length;
            tb.SelectionLength = 0;
        }

        private void SendFallback(string key)
        {
            try
            {
                if (key == "Enter")
                {
                    SendKeys.SendWait("{ENTER}");
                }
                else if (key == "Delete")
                {
                    if (DeleteAsBackspace)
                        SendKeys.SendWait("{BACKSPACE}");
                    else
                        SendKeys.SendWait("{DELETE}");
                }
                else
                {
                    SendKeys.SendWait(key);
                }
            }
            catch
            {
            }
        }

        // =========================================================
        // TARGET CONTROL
        // =========================================================
        private void AttachToHostForm()
        {
            Form frm = this.FindForm();
            if (frm == null) return;
            if (_hookedForm == frm) return;

            _hookedForm = frm;
            HookTrackRecursive(frm);
            RememberCurrentActiveControl();
        }

        private void HookTrackRecursive(Control parent)
        {
            if (parent == null) return;
            if (IsChildOfMe(parent)) return;

            parent.ControlAdded -= Parent_ControlAdded;
            parent.ControlAdded += Parent_ControlAdded;

            foreach (Control c in parent.Controls)
            {
                if (c == null) continue;
                if (IsChildOfMe(c)) continue;

                c.Enter -= Target_Enter;
                c.Enter += Target_Enter;

                c.MouseDown -= Target_MouseDown;
                c.MouseDown += Target_MouseDown;

                if (c.HasChildren)
                    HookTrackRecursive(c);
            }
        }

        private void Parent_ControlAdded(object sender, ControlEventArgs e)
        {
            if (e == null || e.Control == null) return;
            HookTrackRecursive(e.Control);
        }

        private void Target_Enter(object sender, EventArgs e)
        {
            SaveTargetFromControl(sender as Control);
        }

        private void Target_MouseDown(object sender, MouseEventArgs e)
        {
            SaveTargetFromControl(sender as Control);
        }

        private void SaveTargetFromControl(Control c)
        {
            if (c == null) return;
            if (IsChildOfMe(c)) return;

            if (IsEditableControl(c))
            {
                _lastTargetControl = c;
                return;
            }

            Control deep = GetDeep(c);
            if (deep != null && IsEditableControl(deep) && !IsChildOfMe(deep))
                _lastTargetControl = deep;
        }

        private void RememberCurrentActiveControl()
        {
            Form frm = this.FindForm();
            if (frm == null) return;

            Control c = GetDeep(frm.ActiveControl);
            if (c != null && IsEditableControl(c) && !IsChildOfMe(c))
                _lastTargetControl = c;
        }

        private Control GetDeep(Control c)
        {
            while (c is ContainerControl && ((ContainerControl)c).ActiveControl != null)
                c = ((ContainerControl)c).ActiveControl;

            return c;
        }

        private bool IsEditableControl(Control c)
        {
            return (c is TextBoxBase) || (c is ComboBox);
        }

        private bool IsChildOfMe(Control c)
        {
            while (c != null)
            {
                if (c == this) return true;
                c = c.Parent;
            }
            return false;
        }

        private void RestoreFocusToTarget()
        {
            Control c = _lastTargetControl;
            if (c == null || c.IsDisposed) return;
            if (!c.CanFocus) return;

            try
            {
                c.Focus();
            }
            catch
            {
            }
        }

        public void SetTargetControl(Control c)
        {
            if (c == null) return;
            if (IsChildOfMe(c)) return;

            _lastTargetControl = c;
            RestoreFocusToTarget();
        }

        // =========================================================
        // DRAG MOVE
        // =========================================================
        private void HookDrag(Control c)
        {
            if (c == null) return;

            c.MouseDown -= DragDown;
            c.MouseDown += DragDown;

            c.MouseMove -= DragMove;
            c.MouseMove += DragMove;

            c.MouseUp -= DragUp;
            c.MouseUp += DragUp;
        }

        private void DragDown(object sender, MouseEventArgs e)
        {
            if (!AllowDragMove) return;
            if (e.Button != MouseButtons.Left) return;

            _dragging = true;
            _dragStart = Control.MousePosition;
        }

        private void DragMove(object sender, MouseEventArgs e)
        {
            if (!AllowDragMove) return;
            if (!_dragging) return;
            if (this.Parent == null) return;

            Point cur = Control.MousePosition;
            this.Location = new Point(
                this.Left + (cur.X - _dragStart.X),
                this.Top + (cur.Y - _dragStart.Y));

            _dragStart = cur;
        }

        private void DragUp(object sender, MouseEventArgs e)
        {
            _dragging = false;
            SaveLayout();
        }

        // =========================================================
        // SAVE / LOAD LAYOUT
        // =========================================================
        private void TryLoadLayoutOnce()
        {
            if (_layoutLoaded) return;
            _layoutLoaded = true;
            LoadLayout();
        }

        private void ScheduleSaveLayout()
        {
            if (!SaveLayoutOnDisk) return;
            if (_suspendSave) return;
            if (!this.IsHandleCreated) return;

            _saveTimer.Stop();
            _saveTimer.Start();
        }

        public void SaveLayout()
        {
            if (!SaveLayoutOnDisk) return;
            if (_suspendSave) return;

            try
            {
                string path = GetLayoutFilePath();
                string text =
                    this.Left.ToString() + Environment.NewLine +
                    this.Top.ToString() + Environment.NewLine +
                    this.Width.ToString() + Environment.NewLine +
                    this.Height.ToString();

                File.WriteAllText(path, text);
            }
            catch
            {
            }
        }

        public void LoadLayout()
        {
            if (!SaveLayoutOnDisk) return;

            try
            {
                string path = GetLayoutFilePath();
                if (!File.Exists(path)) return;

                string[] arr = File.ReadAllLines(path);
                if (arr == null || arr.Length < 4) return;

                int x, y, w, h;
                if (!int.TryParse(arr[0], out x)) return;
                if (!int.TryParse(arr[1], out y)) return;
                if (!int.TryParse(arr[2], out w)) return;
                if (!int.TryParse(arr[3], out h)) return;

                _suspendSave = true;

                if (SaveSizeOnDisk)
                {
                    if (w < this.MinimumSize.Width) w = this.MinimumSize.Width;
                    if (h < this.MinimumSize.Height) h = this.MinimumSize.Height;

                    if (KeepSquareLayout)
                        h = GetPreferredHeightFromWidth(w);

                    this.Size = new Size(w, h);
                }

                if (SaveLocationOnDisk)
                {
                    this.Location = new Point(x, y);
                }
            }
            catch
            {
            }
            finally
            {
                _suspendSave = false;
                UpdateGripPosition();
            }
        }

        public void ResetSavedLayout()
        {
            try
            {
                string path = GetLayoutFilePath();
                if (File.Exists(path))
                    File.Delete(path);
            }
            catch
            {
            }
        }

        private string GetLayoutFilePath()
        {
            string dir = Path.Combine(Application.UserAppDataPath, "Layouts");
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            string key = LayoutKey;
            if (string.IsNullOrWhiteSpace(key))
                key = "Default";

            foreach (char ch in Path.GetInvalidFileNameChars())
                key = key.Replace(ch, '_');

            return Path.Combine(dir, "AdjustNumberPad_" + key + ".txt");
        }
    }
}