using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace BeeInterface
{
    // ===================== ReportDashboard.cs =====================
    // ONE FILE - JSON Dashboard with ASYNC WRITER THREAD
    // C# 7.3 compatible

   

    #region ===================== MODEL =====================
    [DataContract]
    public class ReportEntry
    {
        [DataMember] public int STT;
        [DataMember] public DateTime Time;
        [DataMember] public string Model;
        [DataMember] public string PO;
        [DataMember] public string Status;     // OK / NG
        [DataMember] public string RawPath;
        [DataMember] public string ResultPath;
    }
    #endregion

    #region ===================== REPORT STORE (JSON) =====================
    public class ReportStore
    {
        readonly string _file;
        readonly object _lock = new object();
        List<ReportEntry> _data = new List<ReportEntry>();

        public string FilePath => _file;

        public ReportStore(string file)
        {
            _file = file;
            Load();
        }

        void Load()
        {
            _data = new List<ReportEntry>();
            if (!File.Exists(_file)) return;

            using (var fs = File.Open(_file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                var ser = new DataContractJsonSerializer(typeof(List<ReportEntry>));
                var obj = ser.ReadObject(fs) as List<ReportEntry>;
                if (obj != null) _data = obj;
            }
        }

        void Save()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_file));
            var tmp = _file + ".tmp";

            using (var fs = new FileStream(tmp, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                var ser = new DataContractJsonSerializer(typeof(List<ReportEntry>));
                ser.WriteObject(fs, _data);
                fs.Flush(true);
            }

            if (File.Exists(_file)) File.Delete(_file);
            File.Move(tmp, _file);
        }

        public List<ReportEntry> SelectAll()
        {
            lock (_lock) return _data.ToList();
        }

        public void Insert(ReportEntry e)
        {
            lock (_lock)
            {
                e.STT = (_data.Count == 0) ? 1 : _data.Max(x => x.STT) + 1;
                _data.Add(e);
                Save();
            }
        }

        public void Update(Func<ReportEntry, bool> where, Action<ReportEntry> update)
        {
            lock (_lock)
            {
                foreach (var r in _data.Where(where))
                    update(r);
                Save();
            }
        }

        public void Delete(Func<ReportEntry, bool> where)
        {
            lock (_lock)
            {
                _data.RemoveAll(x => where(x));
                Save();
            }
        }

        public void Reload()
        {
            lock (_lock) Load();
        }
    }
    #endregion

    #region ===================== ASYNC WRITER (BACKGROUND THREAD) =====================
    public class ReportStoreAsync : IDisposable
    {
        readonly ReportStore _store;
        readonly ConcurrentQueue<Action> _queue = new ConcurrentQueue<Action>();
        readonly AutoResetEvent _signal = new AutoResetEvent(false);
        readonly Thread _worker;
        bool _running = true;

        public ReportStoreAsync(ReportStore store)
        {
            _store = store;
            _worker = new Thread(Loop)
            {
                IsBackground = true,
                Name = "ReportStoreAsyncWriter"
            };
            _worker.Start();
        }

        void Loop()
        {
            while (_running)
            {
                _signal.WaitOne();
                while (_queue.TryDequeue(out var job))
                {
                    try { job(); }
                    catch { }
                }
            }
        }

        public void InsertAsync(ReportEntry e)
        {
            _queue.Enqueue(() => _store.Insert(e));
            _signal.Set();
        }

        public void UpdateAsync(Func<ReportEntry, bool> where, Action<ReportEntry> update)
        {
            _queue.Enqueue(() => _store.Update(where, update));
            _signal.Set();
        }

        public void DeleteAsync(Func<ReportEntry, bool> where)
        {
            _queue.Enqueue(() => _store.Delete(where));
            _signal.Set();
        }

        public void Dispose()
        {
            _running = false;
            _signal.Set();
        }
    }
    #endregion

    #region ===================== IMAGE VIEWER =====================
    class ImageViewerForm : Form
    {
        public ImageViewerForm(ReportEntry r)
        {
            WindowState = FormWindowState.Maximized;
            var split = new SplitContainer { Dock = DockStyle.Fill };

            var picRaw = new PictureBox { Dock = DockStyle.Fill, SizeMode = PictureBoxSizeMode.Zoom };
            var picRs = new PictureBox { Dock = DockStyle.Fill, SizeMode = PictureBoxSizeMode.Zoom };

            if (File.Exists(r.RawPath)) picRaw.Image = Image.FromFile(r.RawPath);
            if (File.Exists(r.ResultPath)) picRs.Image = Image.FromFile(r.ResultPath);

            split.Panel1.Controls.Add(picRaw);
            split.Panel2.Controls.Add(picRs);
            Controls.Add(split);
        }
    }
    #endregion

    #region ===================== CANVAS =====================
    class ReportCanvas : Control
    {
        public event Action<ReportEntry> LeftClick;
        List<ReportEntry> _items = new List<ReportEntry>();
        int rowH = 50;

        public void SetData(List<ReportEntry> list)
        {
            _items = list;
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            int i = e.Y / rowH;
            if (i >= 0 && i < _items.Count)
                LeftClick?.Invoke(_items[i]);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(Color.Beige);
            for (int i = 0; i < _items.Count; i++)
            {
                int y = i * rowH;
                var r = _items[i];
                var bg = r.Status == "OK" ? Color.LightGreen : Color.LightCoral;
                g.FillRectangle(new SolidBrush(bg), 0, y, Width, rowH);

                TextRenderer.DrawText(g,
                    $"{r.Time:MM/dd HH:mm} | {r.Model} | {r.PO} | {r.Status}",
                    Font,
                    new Rectangle(10, y, Width - 20, rowH),
                    Color.Black,
                    TextFormatFlags.VerticalCenter);
            }
        }
    }
    #endregion

    #region ===================== DASHBOARD =====================
    public class ReportDashboard : UserControl
    {
        string Root => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Report");
        string JsonFile => Path.Combine(Root, DateTime.Now.ToString("yyyy_MM") + ".json");

        ReportStore _store;
        ReportStoreAsync _storeAsync;
        List<ReportEntry> _view = new List<ReportEntry>();

        ReportCanvas canvas = new ReportCanvas();

        public ReportDashboard()
        {
            Dock = DockStyle.Fill;
            Directory.CreateDirectory(Root);

            _store = new ReportStore(JsonFile);
            _storeAsync = new ReportStoreAsync(_store);

            canvas.Dock = DockStyle.Fill;
            canvas.LeftClick += r => new ImageViewerForm(r).Show();

            Controls.Add(canvas);
            ReloadView();
        }

        void ReloadView()
        {
            _store.Reload();
            _view = _store.SelectAll();
            canvas.SetData(_view);
        }

        // ===================== PUBLIC API (CALL FROM OTHER THREADS) =====================

        public void AddReportAsync(string model, string po, string status, string raw, string result)
        {
            var e = new ReportEntry
            {
                Time = DateTime.Now,
                Model = model,
                PO = po,
                Status = status,
                RawPath = raw,
                ResultPath = result
            };
            _storeAsync.InsertAsync(e);
        }

        public void UpdateReportAsync(int stt, string newStatus)
        {
            _storeAsync.UpdateAsync(x => x.STT == stt, x => x.Status = newStatus);
        }

        public void DeleteReportAsync(int stt)
        {
            _storeAsync.DeleteAsync(x => x.STT == stt);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _storeAsync?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
    #endregion

}
