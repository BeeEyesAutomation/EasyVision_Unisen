using System;
using System.Drawing;
using System.Windows.Forms;

namespace BeeGlobal
{
    public class AdjustNumberPad : UserControl
    {
        // ===== EVENT =====
        public event Action<string> KeyPadPressed;
        public event Action EnterPressed;

        // ===== OPTION =====
        public bool SimulateKeyboard { get; set; } = true;
        public bool DeleteAsBackspace { get; set; } = true;
        public bool AllowDragMove { get; set; } = true;

        public Font ButtonFont { get; set; } = new Font("Segoe UI", 10F, FontStyle.Bold);

        // ===== PRIVATE =====
        private Control _lastTargetControl;
        private bool _dragging = false;
        private Point _dragStart;

        // ===== BUTTON =====
        Button btn0, btn1, btn2, btn3, btn4, btn5, btn6, btn7, btn8, btn9;
        Button btnDot, btnDelete, btnEnter;

        public AdjustNumberPad()
        {
            InitUI();
            HookDragRecursive(this);
        }

        // =========================================================
        // UI
        // =========================================================
        void InitUI()
        {
            this.Size = new Size(190, 180);
            this.BackColor = Color.FromArgb(40, 40, 40);
            this.BorderStyle = BorderStyle.FixedSingle;

            btn7 = CreateBtn("7");
            btn8 = CreateBtn("8");
            btn9 = CreateBtn("9");
            btnDelete = CreateBtn("Del");

            btn4 = CreateBtn("4");
            btn5 = CreateBtn("5");
            btn6 = CreateBtn("6");
            btnEnter = CreateBtn("Enter");

            btn1 = CreateBtn("1");
            btn2 = CreateBtn("2");
            btn3 = CreateBtn("3");

            btn0 = CreateBtn("0");
            btnDot = CreateBtn(".");

            int w = 44, h = 34, gap = 4;
            int x0 = 6, y0 = 6;

            btn7.SetBounds(x0 + (w + gap) * 0, y0 + (h + gap) * 0, w, h);
            btn8.SetBounds(x0 + (w + gap) * 1, y0 + (h + gap) * 0, w, h);
            btn9.SetBounds(x0 + (w + gap) * 2, y0 + (h + gap) * 0, w, h);
            btnDelete.SetBounds(x0 + (w + gap) * 3, y0 + (h + gap) * 0, 54, h);

            btn4.SetBounds(x0 + (w + gap) * 0, y0 + (h + gap) * 1, w, h);
            btn5.SetBounds(x0 + (w + gap) * 1, y0 + (h + gap) * 1, w, h);
            btn6.SetBounds(x0 + (w + gap) * 2, y0 + (h + gap) * 1, w, h);
            btnEnter.SetBounds(x0 + (w + gap) * 3, y0 + (h + gap) * 1, 54, h * 2 + gap);

            btn1.SetBounds(x0 + (w + gap) * 0, y0 + (h + gap) * 2, w, h);
            btn2.SetBounds(x0 + (w + gap) * 1, y0 + (h + gap) * 2, w, h);
            btn3.SetBounds(x0 + (w + gap) * 2, y0 + (h + gap) * 2, w, h);

            btn0.SetBounds(x0 + (w + gap) * 0, y0 + (h + gap) * 3, w * 2 + gap, h);
            btnDot.SetBounds(x0 + (w + gap) * 2, y0 + (h + gap) * 3, w, h);

            // ==== CLICK ====
            btn0.Click += (s, e) => OnKey("0");
            btn1.Click += (s, e) => OnKey("1");
            btn2.Click += (s, e) => OnKey("2");
            btn3.Click += (s, e) => OnKey("3");
            btn4.Click += (s, e) => OnKey("4");
            btn5.Click += (s, e) => OnKey("5");
            btn6.Click += (s, e) => OnKey("6");
            btn7.Click += (s, e) => OnKey("7");
            btn8.Click += (s, e) => OnKey("8");
            btn9.Click += (s, e) => OnKey("9");
            btnDot.Click += (s, e) => OnKey(".");
            btnDelete.Click += (s, e) => OnKey("Delete");
            btnEnter.Click += (s, e) => OnKey("Enter");

            // ==== nhớ control trước khi click ====
            foreach (Control c in new Control[]
            {
                btn0,btn1,btn2,btn3,btn4,btn5,btn6,
                btn7,btn8,btn9,btnDot,btnDelete,btnEnter
            })
                c.MouseDown += (s, e) => RememberTarget();

            this.Controls.AddRange(new Control[]
            {
                btn0,btn1,btn2,btn3,btn4,btn5,btn6,
                btn7,btn8,btn9,btnDot,btnDelete,btnEnter
            });
        }

        Button CreateBtn(string text)
        {
            return new Button
            {
                Text = text,
                Font = ButtonFont,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                BackColor = Color.FromArgb(60, 60, 60),
                TabStop = false,
                TextAlign = ContentAlignment.MiddleCenter
            };
        }

        // =========================================================
        // CORE INPUT
        // =========================================================
        void OnKey(string key)
        {
            KeyPadPressed?.Invoke(key);

            if (key == "Enter")
            {
                EnterPressed?.Invoke();
            }

            if (!SimulateKeyboard) return;

            if (!SendToTarget(key))
                SendFallback(key);
        }

        bool SendToTarget(string key)
        {
            if (_lastTargetControl == null || _lastTargetControl.IsDisposed)
                return false;

            if (_lastTargetControl is TextBoxBase tb)
                return HandleTextBox(tb, key);

            if (_lastTargetControl is ComboBox cb)
                return HandleCombo(cb, key);

            return false;
        }

        bool HandleTextBox(TextBoxBase tb, string key)
        {
            if (!tb.Enabled || tb.ReadOnly) return false;

            tb.Focus();

            if (key == "Enter") return false;

            if (key == "Delete")
            {
                int s = tb.SelectionStart;
                int l = tb.SelectionLength;

                if (l > 0)
                    tb.Text = tb.Text.Remove(s, l);
                else if (DeleteAsBackspace && s > 0)
                {
                    tb.Text = tb.Text.Remove(s - 1, 1);
                    s--;
                }

                tb.SelectionStart = s;
                return true;
            }

            if (key == "." && tb.Text.Contains("."))
                return true;

            InsertText(tb, key);
            return true;
        }

        bool HandleCombo(ComboBox cb, string key)
        {
            if (!cb.Enabled || cb.DropDownStyle == ComboBoxStyle.DropDownList)
                return false;

            cb.Focus();

            if (key == "Enter") return false;

            int s = cb.SelectionStart;
            int l = cb.SelectionLength;
            string t = cb.Text ?? "";

            if (key == "Delete")
            {
                if (l > 0)
                    t = t.Remove(s, l);
                else if (DeleteAsBackspace && s > 0)
                {
                    t = t.Remove(s - 1, 1);
                    s--;
                }

                cb.Text = t;
                cb.SelectionStart = s;
                return true;
            }

            if (key == "." && t.Contains("."))
                return true;

            if (l > 0)
                t = t.Remove(s, l);

            t = t.Insert(s, key);

            cb.Text = t;
            cb.SelectionStart = s + key.Length;
            return true;
        }

        void InsertText(TextBoxBase tb, string text)
        {
            int s = tb.SelectionStart;
            int l = tb.SelectionLength;

            string t = tb.Text ?? "";

            if (l > 0)
                t = t.Remove(s, l);

            t = t.Insert(s, text);

            tb.Text = t;
            tb.SelectionStart = s + text.Length;
        }

        void SendFallback(string key)
        {
            try
            {
                switch (key)
                {
                    case "Enter": SendKeys.SendWait("{ENTER}"); break;
                    case "Delete": SendKeys.SendWait("{BACKSPACE}"); break;
                    default: SendKeys.SendWait(key); break;
                }
            }
            catch { }
        }

        // =========================================================
        // TARGET CONTROL
        // =========================================================
        void RememberTarget()
        {
            var frm = this.FindForm();
            if (frm == null) return;

            var c = GetDeep(frm.ActiveControl);

            if (c == null || c == this) return;

            if (IsChildOfMe(c)) return;

            _lastTargetControl = c;
        }

        Control GetDeep(Control c)
        {
            while (c is ContainerControl cc && cc.ActiveControl != null)
                c = cc.ActiveControl;
            return c;
        }

        bool IsChildOfMe(Control c)
        {
            while (c != null)
            {
                if (c == this) return true;
                c = c.Parent;
            }
            return false;
        }

        public void SetTargetControl(Control c)
        {
            _lastTargetControl = c;
        }

        // =========================================================
        // DRAG MOVE
        // =========================================================
        void HookDragRecursive(Control c)
        {
            c.MouseDown += DragDown;
            c.MouseMove += DragMove;
            c.MouseUp += DragUp;

            foreach (Control child in c.Controls)
                HookDragRecursive(child);
        }

        void DragDown(object s, MouseEventArgs e)
        {
            if (!AllowDragMove || e.Button != MouseButtons.Left) return;
            _dragging = true;
            _dragStart = Control.MousePosition;
        }

        void DragMove(object s, MouseEventArgs e)
        {
            if (!AllowDragMove || !_dragging || this.Parent == null) return;

            var cur = Control.MousePosition;
            this.Location = new Point(
                this.Left + (cur.X - _dragStart.X),
                this.Top + (cur.Y - _dragStart.Y));

            _dragStart = cur;
        }

        void DragUp(object s, MouseEventArgs e)
        {
            _dragging = false;
        }
    }
}