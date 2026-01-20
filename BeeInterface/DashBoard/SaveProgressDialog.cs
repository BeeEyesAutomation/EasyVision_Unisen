using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

public sealed class SaveProgressDialog : Form
{
    private readonly Label _lblTitle;
    private readonly Label _lblMsg;
    private readonly ProgressBar _pb;
    private readonly Button _btnOk;
    private readonly Button _btnCancel;
    private readonly StatusStrip _status;
    private readonly ToolStripStatusLabel _st;

    private volatile bool _cancelRequested;

    public bool CancelRequested => _cancelRequested;
    

    public SaveProgressDialog(string title = "Thông báo")
    {
        this.Shown += SaveProgressDialog_Shown;
        Text = title;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = FormStartPosition.CenterParent;
        ShowInTaskbar = false;
        TopMost = true;
        Width = 520;
        Height = 220;
       
        _lblTitle = new Label
        {
            AutoSize = false,
            Text = "Đang lưu...",
            Font = new Font("Segoe UI", 12f, FontStyle.Bold),
            Dock = DockStyle.Top,
            Height = 40,
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(14, 8, 14, 0)
        };

        _lblMsg = new Label
        {
            AutoSize = false,
            Text = "Vui lòng chờ trong giây lát.",
            Font = new Font("Segoe UI", 10f, FontStyle.Regular),
            Dock = DockStyle.Top,
            Height = 40,
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(14, 0, 14, 0)
        };

        _pb = new ProgressBar
        {
            Dock = DockStyle.Top,
            Height = 18,
            Style = ProgressBarStyle.Marquee,
            MarqueeAnimationSpeed = 25,
            Margin = new Padding(14, 0, 14, 0)
        };

        var pnlButtons = new Panel { Dock = DockStyle.Bottom, Height = 52, Padding = new Padding(10) };

        _btnOk = new Button
        {
            Text = "OK",
            Enabled = false,
            Width = 90,
            Height = 30,
            Anchor = AnchorStyles.Right | AnchorStyles.Top
        };
        _btnOk.Click += (s, e) => Close();

        _btnCancel = new Button
        {
            Text = "Hủy",
            Width = 90,
            Height = 30,
            Anchor = AnchorStyles.Right | AnchorStyles.Top
        };
        _btnCancel.Click += (s, e) =>
        {
            _cancelRequested = true;
            _btnCancel.Enabled = false;
            SetStatus("Đang hủy...", "Đang yêu cầu hủy...");
        };

        // đặt nút về bên phải
        _btnOk.Location = new Point(pnlButtons.Width - _btnOk.Width - 10, 10);
        _btnCancel.Location = new Point(_btnOk.Left - _btnCancel.Width - 10, 10);
        pnlButtons.Resize += (s, e) =>
        {
            _btnOk.Location = new Point(pnlButtons.Width - _btnOk.Width - 10, 10);
            _btnCancel.Location = new Point(_btnOk.Left - _btnCancel.Width - 10, 10);
        };

        pnlButtons.Controls.Add(_btnOk);
        pnlButtons.Controls.Add(_btnCancel);

        _status = new StatusStrip();
        _st = new ToolStripStatusLabel("Đang lưu...");
        _status.Items.Add(_st);

        // Body padding
        var body = new Panel { Dock = DockStyle.Fill, Padding = new Padding(14, 8, 14, 8) };
        body.Controls.Add(_pb);
        body.Controls.Add(_lblMsg);
        body.Controls.Add(_lblTitle);

        Controls.Add(body);
        Controls.Add(pnlButtons);
        Controls.Add(_status);
      
    }

    private void SaveProgressDialog_Shown(object sender, EventArgs e)
    {
        this.Location = new Point(Screen.PrimaryScreen.Bounds.Width / 2 - this.Width / 2, Screen.PrimaryScreen.Bounds.Height / 2 - this.Height / 2);

    }

    public void SetStatus(string statusText, string message)
    {
        if (IsDisposed) return;

        if (InvokeRequired)
        {
            BeginInvoke(new Action(() => SetStatus(statusText, message)));
            return;
        }

        _lblTitle.Text = statusText;
        _lblMsg.Text = message;
        _st.Text = statusText;
    }

    public void MarkCompleted(string doneText = "Lưu hoàn thành", string message = "Đã lưu xong.")
    {
        if (IsDisposed) return;

        if (InvokeRequired)
        {
            BeginInvoke(new Action(() => MarkCompleted(doneText, message)));
            return;
        }

        _lblTitle.Text = doneText;
        _lblMsg.Text = message;
        _st.Text = doneText;

        _pb.Style = ProgressBarStyle.Continuous;
        _pb.MarqueeAnimationSpeed = 0;
        _pb.Minimum = 0;
        _pb.Maximum = 100;
        _pb.Value = 100;

        _btnOk.Enabled = true;
        _btnCancel.Enabled = false;

        // tuỳ bạn: tự focus OK cho tiện bấm Enter
        ActiveControl = _btnOk;
    }
}
