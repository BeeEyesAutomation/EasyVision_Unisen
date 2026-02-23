using BeeUi.Commons;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace BeeUi
{
    // -------- 1) Đóng băng vẽ khi batch --------
    public static class RedrawScope
    {
        const int WM_SETREDRAW = 0x000B;

        [DllImport("user32.dll")]
        static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        public static IDisposable Freeze(Control c) => new ScopeImpl(c);

        private sealed class ScopeImpl : IDisposable
        {
            readonly Control _c; readonly bool _hadHandle;
            public ScopeImpl(Control c)
            {
                _c = c ?? throw new ArgumentNullException(nameof(c));
                _hadHandle = c.IsHandleCreated;
                if (_hadHandle) SendMessage(_c.Handle, WM_SETREDRAW, (IntPtr)0, IntPtr.Zero);
                _c.SuspendLayout();
            }
            public void Dispose()
            {
                _c.ResumeLayout(true);
                if (_hadHandle)
                {
                    SendMessage(_c.Handle, WM_SETREDRAW, (IntPtr)1, IntPtr.Zero);
                    _c.Invalidate(true);
                    _c.Update();
                }
            }
        }
    }

    // -------- 2) Bật double-buffer đệ quy + warm-up --------
    public static class WinFormsPerfExtensions
    {
        public static void EnableDoubleBufferRecursive(this Control root)
        {
            if (root == null) return;
            TrySet(root, "DoubleBuffered", true);
            TrySet(root, "ResizeRedraw", true);
            foreach (Control c in root.Controls)
                EnableDoubleBufferRecursive(c);
        }

        public static void WarmUpHandle(this Control c)
        {
            if (c == null) return;
            if (!c.IsHandleCreated) c.CreateControl();
            // ép layout một lần cho “ấm máy”
            c.PerformLayout();
            c.Update();
        }

        static void TrySet(Control c, string prop, object value)
        {
            try
            {
                c.GetType()
                 .GetProperty(prop, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                 ?.SetValue(c, value, null);
            }
            catch { /* ignore */ }
        }
    }

    // -------- 3) Host đổi view tự động cho mọi UserControl --------
    [DesignerCategory("Code")]
    public class ViewHost : Panel
    {
        public Control Current { get; private set; }

        // Đăng ký factory để lazy-create khi cần
        private readonly Dictionary<string, Func<Control>> _factories = new Dictionary<string, Func<Control>>();

        public ViewHost()
        {
            // Mặc định mượt
            this.DoubleBuffered = true;
            this.ResizeRedraw = true;
        }

        // Tự cấu hình tất cả child nếu bạn kéo thả bằng Designer
        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            this.Parent?.EnableDoubleBufferRecursive();
            this.EnableDoubleBufferRecursive();

            using (RedrawScope.Freeze(this))
            {
                bool firstShown = false;
                foreach (Control c in this.Controls)
                {
                    c.Dock = DockStyle.Fill;
                    if (!firstShown)
                    {
                        c.Visible = true;
                        c.WarmUpHandle();
                        Current = c;
                        firstShown = true;
                    }
                    else
                    {
                        c.Visible = false;
                        c.WarmUpHandle(); // preload ẩn
                    }
                }
            }
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);
            // Tự chuẩn hoá mọi UserControl bạn add sau này
            e.Control.Dock = DockStyle.Fill;
            e.Control.Visible = false;
            e.Control.WarmUpHandle();

            if (Current == null)
            {
                Current = e.Control;
                Current.Visible = true;
                Current.BringToFront();
            }
        }

        // Đăng ký view theo key -> factory
        public void Register(string key, Func<Control> factory)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));
            _factories[key] = factory ?? throw new ArgumentNullException(nameof(factory));
        }
        private void Ui(Action a)
        {
            if (IsDisposed) return;
            if (InvokeRequired) BeginInvoke(a);
            else a();
        }
        public bool Unregister(string key, bool removeExistingView = true, bool disposeView = true)
        {
           

            return false;
        }


        // Hiện view theo key (tạo lần đầu, sau dùng lại)
        public bool Show(string key)
        {
            if (!_factories.TryGetValue(key, out var factory))
                return false;
            var view = EnsureView(key, factory);
            if(view==null)
                return false;
            if(view.IsDisposed) 
                return false;
            ShowView(view);
            return true;
        }

        // Hiện trực tiếp 1 control đã có/được tạo nơi khác
        public void ShowView(Control view)
        {
            if (view == null || view == Current) return;

            using (RedrawScope.Freeze(this))
            {
                if (!this.Controls.Contains(view))
                {
                    view.Dock = DockStyle.Fill;
                    view.Visible = false;
                    this.Controls.Add(view);
                    view.WarmUpHandle();
                }

                if (Current != null) Current.Visible = false;

                view.Visible = true;
                view.BringToFront();
                Current = view;
            }
        }

        private Control EnsureView(string key, Func<Control> factory)
        {
            // Nếu đã add view mang Name == key -> dùng lại
            foreach (Control c in this.Controls)
                if (string.Equals(c.Name, key, StringComparison.OrdinalIgnoreCase))
                    return c;

            var v = factory();
            if (string.IsNullOrEmpty(v.Name)) v.Name = key;
            return v;
        }
    }
}
