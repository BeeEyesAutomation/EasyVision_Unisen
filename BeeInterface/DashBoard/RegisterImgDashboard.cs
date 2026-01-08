
using BeeCore;
using BeeCore.Funtion;
using BeeGlobal;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace BeeInterface
{
    // ====
    // EventArgs cho sự kiện chọn item
    // ====
    public sealed class RegisterImgSelectionChangedEventArgs : EventArgs
    {
        /// <summary>Tên ảnh được chọn.</summary>
        public string Name { get; }

        /// <summary>Mat của ảnh được chọn (mat gốc trong dashboard). Nếu cần giữ lâu, hãy Clone() ở ngoài.</summary>
        public Mat Image { get; }

        public RegisterImgSelectionChangedEventArgs(string name, Mat image)
        {
            Name = name;
            Image = image;
        }
    }

    // ====
    // Model ảnh — chỉ giữ Mat và Name
    // ====
    public sealed class ImgItem : IDisposable
    {
        public string Name { get; set; }
        public Mat Image { get;  set; }

        public ImgItem(string name, Mat img)
        {
            Name = string.IsNullOrWhiteSpace(name) ? "Image" : name.Trim();
            SetImage(img);
        }

        public void SetImage(Mat img)
        {
            Image?.Dispose();
            Image = img?.Clone();
        }

        public void Dispose()
        {
            Image?.Dispose();
            Image = null;
        }

        public override string ToString() => Name;
    }

    // ====
    // UI thumbnail + tên + inline rename
    // ====
    public class ImageThumbControl : Control
    {
        public ImgItem Item { get; private set; }
        public bool Selected { get; set; }

        private const int LabelHeight = 26;
        private const int PaddingTop = 8;
        private const int PaddingBottom = 4;

        private readonly TextBox _editBox;

        public event EventHandler NameCommitted;

        public ImageThumbControl(ImgItem item, Font font,bool UpdateGlobal, int W = 240, int H = 260)
        {
            Item = item ?? throw new ArgumentNullException(nameof(item));
            DoubleBuffered = true;
            Width = W; Height = H;
            Cursor = Cursors.Hand;
            Margin = new Padding(8);
            TabStop = true;
            this.UpdateGlobal= UpdateGlobal;
            _editBox = new TextBox();
            _editBox.Visible = false;
            _editBox.BorderStyle = BorderStyle.FixedSingle;
            _editBox.Font = font;
            _editBox.KeyDown += EditBox_KeyDown;
            _editBox.Leave += EditBox_Leave;
            Controls.Add(_editBox);
        }

        public void ApplySize(int w, int h)
        {
            if (w < 40) w = 40;
            if (h < 40) h = 40;
            Width = w;
            Height = h;
            if (_editBox.Visible)
            {
                var r = GetNameRect();
                _editBox.SetBounds(r.Left + 2, r.Top + 3, r.Width - 4, r.Height - 6);
            }
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.Clear(BackColor);

            using (var bg = new SolidBrush(Color.FromArgb(245, 245, 245)))
                g.FillRectangle(bg, ClientRectangle);

            // 1) vùng ảnh
            int imageAreaTop = PaddingTop;
            int imageAreaHeight = Height - LabelHeight - PaddingTop - PaddingBottom;
            int imageAreaWidth = Width;
            var imageArea = new Rectangle(0, imageAreaTop, imageAreaWidth, imageAreaHeight);

            // 2) vẽ ảnh từ Mat
            if (Item?.Image != null && !Item.Image.Empty())
            {
                using (var bmp = Item.Image.ToBitmap())
                {
                    float scale = 1f;
                    if (bmp.Width > imageArea.Width || bmp.Height > imageArea.Height)
                    {
                        float sx = (float)imageArea.Width / bmp.Width;
                        float sy = (float)imageArea.Height / bmp.Height;
                        scale = Math.Min(sx, sy);
                    }

                    int drawW = (int)(bmp.Width * scale);
                    int drawH = (int)(bmp.Height * scale);

                    int x = imageArea.Left + (imageArea.Width - drawW) / 2;
                    int y = imageArea.Top + (imageArea.Height - drawH) / 2;

                    g.DrawImage(bmp, x, y, drawW, drawH);
                }
            }

            // 3) vẽ tên ở đáy nếu không edit
            var nameRect = GetNameRect();
            if (!_editBox.Visible)
            {
                TextRenderer.DrawText(
                    g,
                    Item?.Name ?? "",
                    Font,
                    nameRect,
                    Color.Black,
                    TextFormatFlags.EndEllipsis | TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
                );
            }

            // 4) viền chọn
            var color = Selected ? Color.OrangeRed : Color.Silver;
            int thick = Selected ? 3 : 1;
            using (var pen = new Pen(color, thick))
            {
                var r = ClientRectangle; r.Width -= 1; r.Height -= 1;
                g.DrawRectangle(pen, r);
            }
        }

        private Rectangle GetNameRect()
        {
            return new Rectangle(6, Height - LabelHeight, Width - 12, LabelHeight);
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            Focus();
            Invalidate();
        }

        protected override void OnDoubleClick(EventArgs e)
        {
            base.OnDoubleClick(e);
            BeginRename();
        }

        public void BeginRename()
        {
            var r = GetNameRect();
            _editBox.Text = Item?.Name ?? "";
            _editBox.SetBounds(r.Left + 2, r.Top -3, r.Width - 4, r.Height - 6);
            _editBox.Visible = true;
            _editBox.BringToFront();
            _editBox.SelectAll();
            _editBox.Focus();
        }
        public bool UpdateGlobal;
        private void CommitRename(bool cancel = false)
        {
            if (!_editBox.Visible) return;

            if (!cancel)
            {
                string txt = _editBox.Text?.Trim() ?? "";
                if (txt.Length == 0) txt = "Image";
                if (Item != null)
                {
                    int id = Global.listRegsImg.FindIndex(a => a.Name.Contains(Item.Name));
                    if (UpdateGlobal)
                    {
                       
                        if (id > -1)
                            Global.listRegsImg[id].Name = txt;
                    }
                    else
                          if (id > -1)
                        Global.listSimImg[id].Name = txt;
                    Item.Name = txt;
                }    
                   
                NameCommitted?.Invoke(this, EventArgs.Empty);
            }

            _editBox.Visible = false;
            Invalidate();
        }

        private void EditBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                CommitRename( false);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                CommitRename(true);
                e.Handled = true;
            }
        }

        private void EditBox_Leave(object sender, EventArgs e)
        {
            CommitRename(false);
        }
    }

    // ====
    // Dashboard
    // ====
    public class RegisterImgDashboard : UserControl
    {
        private bool _hideTopBar = false;
       [Category("HideTopBar")] public bool HideTopBar { get => _hideTopBar; set { 
                _hideTopBar = value;
                _topTL.Visible=!_hideTopBar;
                _topTL2.Visible = !_hideTopBar;
                _topTL3.Visible = !_hideTopBar;
            } }
        private bool _updateGlobal = true;
        [Category("UpdateGlobal")]
        public bool UpdateGlobal
        {
            get => _updateGlobal; set
            {
                _updateGlobal = value;
               
            }
        }
        private int _heightTopBar1 = 50;
        [Category("HeightTopBar1")]
        public int HeightTopBar1
        {
            get => _heightTopBar1; set
            {
                _heightTopBar1 = value;
              
                _topTL2.Height= _heightTopBar1;
            }
        }
        private int _heightTopBar2 = 70;
        [Category("HeightTopBar1")]
        public int HeightTopBar2
        {
            get => _heightTopBar2; set
            {
                _heightTopBar2 = value;

                _topTL.Height = _heightTopBar2;
            }
        }

        private int _heightTopBar3 = 40;
        [Category("HeightTopBar3")]
        public int HeightTopBar3
        {
            get => _heightTopBar3; set
            {
                _heightTopBar3 = value;

                _topTL3.Height = _heightTopBar3;
            }
        }
        private FlowLayoutPanel _flow;
        private RJButton _btnAddFile, _btnAddCam,btnChange, btnNew, btnDeleteAll,btnDelete,btnUp,btndown;
        private ContextMenuStrip _ctx;
        public  List<ImgItem> _items = new List<ImgItem>();
        private ImageThumbControl _selectedThumb;
        private bool IsChange = true;
        private const int THUMB_MARGIN_HORIZONTAL = 16;

        private int _nameSeq = 1;
        public string AutoNamePrefix { get; set; } = "Img";
        public int AutoNameDigits { get; set; } = 3;

        /// <summary>Khi item được chọn đổi: bắn ra tên + Mat.</summary>
        public event EventHandler<RegisterImgSelectionChangedEventArgs> SelectedItemChanged;

        /// <summary>Event cũ (nếu bạn đã dùng) — giữ lại.</summary>
        public event EventHandler SelectedChanged;

        public event EventHandler ItemsChanged;
      public  TableLayoutPanel _topTL;
        public TableLayoutPanel _topTL2;
        public TableLayoutPanel _topTL3;
        public RegisterImgDashboard()
        {
            DoubleBuffered = true;
            Dock = DockStyle.Fill;
            Color bgTL2 = Color.FromArgb(50, 114, 114, 114);
            _topTL = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = false,
         Height=HeightTopBar2,
                 BackColor = bgTL2,
                 ColumnCount = 3,
                Padding = new Padding(5, 0, 5, 0),
                RowCount = 1,
            };
           
            _topTL2 = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = false,
                Height = HeightTopBar1,
                BackColor = bgTL2,
             Padding=new Padding(5,5,5,5),
                ColumnCount = 3,
                RowCount = 1,
            };
            _topTL3 = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = false,
                Height = HeightTopBar3,
                BackColor = bgTL2,
                Padding = new Padding(5, 0, 5, 0),
                ColumnCount = 4,
                RowCount = 1,
            };
            _topTL.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));
            _topTL.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            _topTL.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            _topTL.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            _topTL2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));
            _topTL2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            _topTL2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            _topTL2.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            _topTL3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            _topTL3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            _topTL3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            _topTL3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            _topTL3.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            RJButton lb = new RJButton
            {
                AutoFontMin=9,
               
                Text = "Mode Register",
               Corner=Corner.Left,
               BorderRadius=10,
               Enabled=false,
                Dock = DockStyle.Fill,
                AutoSize=false,
                TextColor=Color.Black,
                TextAlign=ContentAlignment.MiddleLeft,
                Margin = new Padding(0,0, 0, 0)
            };
            RJButton lb2 = new RJButton
            {
                AutoFontMin = 9,

                Text = "Image From",
                Corner = Corner.Left,
                BorderRadius = 10,
                Enabled = false,
                Dock = DockStyle.Fill,
                AutoSize = false,
                TextColor = Color.Black,
                TextAlign = ContentAlignment.MiddleLeft,
                Margin = new Padding(0, 0, 0, 0)
            };
            btnChange = new RJButton { BorderColor = bgTL2, Corner = Corner.None, BorderRadius = 10, BorderSize = 0, AutoFontMin = 9, ImageTextSpacing = 5, IsUnGroup = false, IsNotChange = false, Text = "Change", IsCLick = IsChange, TextImageRelation = TextImageRelation.ImageBeforeText, Dock = DockStyle.Fill, Image = Properties.Resources.Refresh25, Margin = new Padding(0, 0, 0, 0) };
            btnNew = new RJButton { BorderColor = bgTL2, Corner = Corner.Right, BorderRadius = 10, BorderSize = 0, AutoFontMin = 9, ImageTextSpacing = 5, IsUnGroup = false, IsNotChange = false, Text = "Add", IsCLick =! IsChange, TextImageRelation = TextImageRelation.ImageBeforeText, Dock = DockStyle.Fill, Image = Properties.Resources.Add, Margin = new Padding(0, 0, 5, 0) };
            btnUp = new RJButton {BorderRadius=5, BorderColor = bgTL2, BorderSize = 0, AutoFontMin = 9, ImageTextSpacing = 5, IsUnGroup = true, IsNotChange = true, Text = "Up", IsCLick = false,Dock = DockStyle.Fill, Margin = new Padding(0, 5, 5, 5)};
            btndown = new RJButton { BorderRadius = 5, BorderColor = bgTL2, BorderSize = 0, AutoFontMin = 9, ImageTextSpacing = 5, IsUnGroup = true, IsNotChange = true, Text = "Down", IsCLick = false,  Dock = DockStyle.Fill, Margin = new Padding(5, 5, 5, 5) };
            btnDelete= new RJButton { BorderRadius = 5, BorderColor = bgTL2, BorderSize = 0, AutoFontMin = 9, ImageTextSpacing = 5, IsUnGroup = true, IsNotChange = true, Text = "Delete", IsCLick = false,  Dock = DockStyle.Fill,  Margin = new Padding(5, 5, 5, 5) };
            btnDeleteAll = new RJButton { BorderRadius = 5, BorderColor = bgTL2, BorderSize = 0, AutoFontMin = 9, ImageTextSpacing = 5, IsUnGroup = true, IsNotChange = true, Text = "Delete All", IsCLick = false,  Dock = DockStyle.Fill, Margin = new Padding(5, 5, 5, 5)};
            _btnAddCam = new RJButton {Corner=Corner.None, AutoFontMin = 9, ImageTextSpacing = 1, IsUnGroup = true, IsNotChange = true, Text = "Camera", TextImageRelation = TextImageRelation.ImageBeforeText, Dock = DockStyle.Fill, Image = Properties.Resources.Camera, Margin = new Padding(0, 0, 0, 0) };
            _btnAddFile = new RJButton { Corner = Corner.Right, AutoFontMin = 9,ImageTextSpacing=1,  IsUnGroup = true, IsNotChange = true, Text = "Files", TextImageRelation = TextImageRelation.ImageBeforeText, Dock = DockStyle.Fill, Image = Properties.Resources.Folder,  Margin = new Padding(0, 0, 5, 0) };
            btnDeleteAll.Click += BtnDeleteAll_Click;
            btnNew.Click += BtnNew_Click;
            btnChange.Click += BtnChange_Click;
            btnUp.Click += BtnUp_Click;
            btndown.Click += Btndown_Click;
            btnDelete.Click += BtnDelete_Click;
            _topTL.Controls.Add(lb2, 0, 0);
            _topTL.Controls.Add(_btnAddCam, 1, 0);
            _topTL.Controls.Add(_btnAddFile, 2, 0);
            _topTL2.Controls.Add(lb, 0, 0);
            _topTL2.Controls.Add(btnChange, 1, 0);

            _topTL2.Controls.Add(btnNew, 2, 0);

            _topTL3.Controls.Add(btnUp, 0, 0);
            _topTL3.Controls.Add(btndown, 1, 0);
            _topTL3.Controls.Add(btnDelete, 2, 0);
            _topTL3.Controls.Add(btnDeleteAll, 3, 0);
            _flow = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                WrapContents = true
            };

            Controls.Add(_flow);
            Controls.Add(_topTL3);
            Controls.Add(_topTL);
            Controls.Add(_topTL2);
            _ctx = new ContextMenuStrip();
            // Đổi tên (F2)
            var miRename = new ToolStripMenuItem("Rename", null, (s, e) => RenameSelectedInline());
            miRename.ShortcutKeys = Keys.Control | Keys.R;
            _ctx.Items.Add(miRename);
            // Xóa (Delete)
            var miDelete = new ToolStripMenuItem("Detele", null, (s, e) => DeleteSelected());
            miDelete.ShortcutKeys = Keys.Delete;
            _ctx.Items.Add(miDelete);
            _ctx.Items.Add(new ToolStripSeparator());
            // Up (Ctrl + Up)
            var miUp = new ToolStripMenuItem("Up", null, (s, e) => MoveSelected(-1));
            miUp.ShortcutKeys = Keys.Control | Keys.Up;
            _ctx.Items.Add(miUp);
            // Down (Ctrl + Down)
            var miDown = new ToolStripMenuItem("Down", null, (s, e) => MoveSelected(+1));
            miDown.ShortcutKeys = Keys.Control | Keys.Down;
            _ctx.Items.Add(miDown);

            _flow.ContextMenuStrip = _ctx;

            _btnAddFile.Click += (s, e) => AddFromFile();
            _btnAddCam.Click += (s, e) => AddFromCamera();

            this.SizeChanged += (s, e) => UpdateThumbLayout();

            UpdateCameraButtonState();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            DeleteSelected();
        }

        private void Btndown_Click(object sender, EventArgs e)
        {
            MoveSelected(+1);
        }

        private void BtnUp_Click(object sender, EventArgs e)
        {
            MoveSelected(-1);
        }

        private void BtnNew_Click(object sender, EventArgs e)
        {
            IsChange = !btnNew.IsCLick;
        }

        private void BtnDeleteAll_Click(object sender, EventArgs e)
        {
          if(MessageBox.Show("Warming","Delete All Images",MessageBoxButtons.YesNo)==DialogResult.Yes)
            {
                DeleteAllItems();
            }    
        }
        public void DeleteAllItems()
        {
            // 1. Xóa model trong dashboard
            _items.Clear();

            // 2. Xóa UI
            _flow.Controls.Clear();

            // 3. Xóa chọn hiện tại
            _selectedThumb = null;

            // 4. Xóa trong global (nếu bạn muốn đồng bộ luôn)
            if(UpdateGlobal)
            {
                if (Global.ParaCommon != null && Global.listRegsImg != null)
                    Global.listRegsImg.Clear();
            }
            else
            {
                if (Global.ParaCommon != null && Global.listSimImg != null)
                    Global.listSimImg.Clear();
            }

                // 5. Báo ra ngoài: danh sách thay đổi
                ItemsChanged?.Invoke(this, EventArgs.Empty);
            IndexSelected = -1;
            // 6. Báo là hiện không có item nào được chọn
            SelectedItemChanged?.Invoke(
                this,
                new RegisterImgSelectionChangedEventArgs(null, null)
            );

            // 7. Cập nhật layout (flow trống thì cũng ok)
            UpdateThumbLayout();
        }
        private void BtnChange_Click(object sender, EventArgs e)
        {
            IsChange = btnChange.IsCLick;
            //if(!IsChange)
            //{
            //    btnChange.Image = Properties.Resources.Add;
            //    btnChange.Text = "Add Image";
            //}
            //else
            //{
            //    btnChange.Image = Properties.Resources.Refresh25;
            //    btnChange.Text = "Change Image";

            //}    
        }

        private void UpdateCameraButtonState()
        {
            if (_btnAddCam == null) return;
            _btnAddCam.Visible = _showCamDefault;
            _btnAddCam.Enabled = _showCamDefault;
        }
        private bool _showCamDefault = true;
        //public bool ShowCameraButton
        //{
        //    get => _btnAddCam?.Visible ?? _showCamDefault;
        //    set
        //    {
        //        _showCamDefault = value;
        //        if (_btnAddCam != null) _btnAddCam.Visible = value;
        //    }
        //}

        private string NextAutoName()
        {
            string num = _nameSeq.ToString(new string('0', Math.Max(1, AutoNameDigits)));
            _nameSeq++;
            return $"{AutoNamePrefix}_{num}";
        }

        public Mat SelectedMat => _selectedThumb?.Item?.Image;
        public Mat GetSelectedMat() => SelectedMat?.Clone();
        public int Count => _items.Count;

        private void AddFromFile()
        {
            using (var ofd = new OpenFileDialog
            {
                Filter = "Images|*.png;*.jpg;*.jpeg;*.bmp;*.tif;*.tiff",
                
                Multiselect = !IsChange
            })
            {
                if (ofd.ShowDialog(FindForm()) != DialogResult.OK) return;
                if (IsChange && _items.Count>0)
                {
                    var mat = Cv2.ImRead(ofd.FileName, ImreadModes.Color);
                    if (mat.Empty()) { mat?.Dispose(); return; }
                    UpdateSelectedItemImage(mat.Clone());
                    mat.Dispose();
                  
                }
                else
                    foreach (var path in ofd.FileNames)
                {
                    var mat = Cv2.ImRead(path, ImreadModes.Color);
                    if (mat.Empty()) { mat?.Dispose(); continue; }

                    var autoName = NextAutoName();
                    AddItem(new ImgItem(autoName, mat));
                    mat.Dispose();
                }
            }
        }

        private async void AddFromCamera()
        {
            try
            {
                Global.Config.IsOnLight = true;
                Global.Comunication.Protocol.IO_Processing = IO_Processing.Light;
                await TimingUtils.DelayAccurateAsync((int)Global.Comunication.Protocol.DelayTrigger);
                BeeCore.Common.listCamera[Global.IndexChoose].Read();
                if( BeeCore.Common.listCamera[Global.IndexChoose].Para.TypeCamera==TypeCamera.USB)
                BeeCore.Common.listCamera[Global.IndexChoose].Read();
                Global.Config.IsOnLight = false;
                Global.Comunication.Protocol.IO_Processing = IO_Processing.Light;
                if (BeeCore.Common.listCamera[Global.IndexChoose].matRaw == null) return;
                if (BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Empty()) return;
                var autoName = NextAutoName();
                if(IsChange && _items.Count > 0)
                {
                    UpdateSelectedItemImage(BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Clone());

                }
                else
                    AddItem(new ImgItem(autoName, BeeCore.Common.listCamera[Global.IndexChoose].matRaw.Clone()));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lấy ảnh từ camera thất bại: " + ex.Message);
            }
        }
        public void LoadAllItem(List<ItemRegsImg> ItemRegsImgs,int indexItem = 0)
        {
            // xóa dữ liệu cũ
            _items.Clear();
            _flow.Controls.Clear();

            // lưu tạm các thumb để lát chọn
            var createdThumbs = new List<ImageThumbControl>();

            foreach (ItemRegsImg regsImg in ItemRegsImgs)
            {
                // tạo model
                ImgItem item = new ImgItem(regsImg.Name, regsImg.Image.ToMat());
                _items.Add(item);

                // tính size theo width hiện tại
                int clientW = _flow.ClientSize.Width;
                if (clientW <= 0) clientW = this.Width;

                int targetW = Math.Max(60, clientW - THUMB_MARGIN_HORIZONTAL);
                int targetH = CalcHeightFromImage(item, targetW);

                // tạo thumb (bạn đang có ctor có Font)
                var thumb = new ImageThumbControl(item, this.Font,UpdateGlobal, targetW, targetH);
                thumb.Click += (s, e) => SelectThumb(thumb);
                thumb.NameCommitted += (s, e) => ItemsChanged?.Invoke(this, EventArgs.Empty);
                thumb.MouseUp += (s, e) =>
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        SelectThumb(thumb);
                        _ctx.Show(thumb, e.Location);
                    }
                };

                _flow.Controls.Add(thumb);
                createdThumbs.Add(thumb);
            }

            // sau khi add xong thì chọn item theo index
            if (createdThumbs.Count > 0)
            {
                // clamp index
                if (indexItem < 0) indexItem = 0;
                if (indexItem >= createdThumbs.Count) indexItem = createdThumbs.Count - 1;

                SelectThumb(createdThumbs[indexItem]);
            }

            // báo thay đổi 1 lần thôi
            ItemsChanged?.Invoke(this, EventArgs.Empty);

            // cập nhật layout
            UpdateThumbLayout();
        }


        private void AddItem(ImgItem item)
        {
            _items.Add(item);
            if(UpdateGlobal)
            {
                Global.listRegsImg.Add(new ItemRegsImg(item.Name, item.Image.ToBitmap()));
            }
            else
            {
                Global.listSimImg.Add(new ItemRegsImg(item.Name, item.Image.ToBitmap()));
            }    

                int clientW = _flow.ClientSize.Width;
            if (clientW <= 0) clientW = this.Width;

            int targetW = Math.Max(60, clientW - THUMB_MARGIN_HORIZONTAL);
            int targetH = CalcHeightFromImage(item, targetW);

            var thumb = new ImageThumbControl(item,this.Font, UpdateGlobal, targetW, targetH);
            thumb.Click += (s, e) => SelectThumb(thumb);
            thumb.NameCommitted += (s, e) => ItemsChanged?.Invoke(this, EventArgs.Empty);
            thumb.MouseUp += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    SelectThumb(thumb);
                    _ctx.Show(thumb, e.Location);
                }
            };

            _flow.Controls.Add(thumb);
            SelectThumb(thumb);
            ItemsChanged?.Invoke(this, EventArgs.Empty);

            UpdateThumbLayout();
        }
        public void UpdateSelectedItemImage(Mat newMat)
        {
            if (_selectedThumb == null) return;
            if (newMat == null || newMat.Empty()) return;

            // 1. cập nhật Mat trong model
            _selectedThumb.Item.SetImage(newMat);
          
        
            if (UpdateGlobal)
            {
                int id = Global.listRegsImg.FindIndex(a => a.Name.Contains(_selectedThumb.Item.Name));
                if (id > -1)
                    Global.listRegsImg[id].Image = newMat.ToBitmap();
            }
            else

            {
                int id = Global.listSimImg.FindIndex(a => a.Name.Contains(_selectedThumb.Item.Name));
                if (id > -1)
                    Global.listSimImg[id].Image = newMat.ToBitmap();

            }    

                // 2. tính lại size của thumb theo ảnh mới
                int clientW = _flow.ClientSize.Width;
            if (clientW <= 0) clientW = this.Width;
            int targetW = Math.Max(60, clientW - THUMB_MARGIN_HORIZONTAL);
            int targetH = CalcHeightFromImage(_selectedThumb.Item, targetW);

            // 3. apply size + vẽ lại
            _selectedThumb.ApplySize(targetW, targetH);
            _selectedThumb.Invalidate();

            // 4. notify list thay đổi (nếu bạn đang lưu ra file...)
            ItemsChanged?.Invoke(this, EventArgs.Empty);

            // 5. bắn lại selected item để bên ngoài cập nhật Mat
            SelectedItemChanged?.Invoke(
                this,
                new RegisterImgSelectionChangedEventArgs(
                    _selectedThumb.Item.Name,
                    _selectedThumb.Item.Image
                )
            );

            // 6. cập nhật layout chung
            UpdateThumbLayout();
        }

        private int CalcHeightFromImage(ImgItem item, int targetW)
        {
            const int labelH = 26;
            if (item?.Image == null || item.Image.Empty())
            {
                return 200;
            }

            double scale = (double)targetW / Math.Max(1, item.Image.Width);
            int imgH = (int)Math.Round(item.Image.Height * scale);

            return imgH + labelH + 12;
        }

        private void UpdateThumbLayout()
        {
            if (_flow == null) return;
            if (_flow.Controls.Count == 0) return;

            int clientW = _flow.ClientSize.Width;
            if (clientW <= 0) clientW = this.Width;

            int targetW = Math.Max(60, clientW - THUMB_MARGIN_HORIZONTAL);

            for (int i = 0; i < _flow.Controls.Count; i++)
            {
                if (!(_flow.Controls[i] is ImageThumbControl thc)) continue;
                var item = thc.Item;
                int targetH = CalcHeightFromImage(item, targetW);
                thc.ApplySize(targetW, targetH);
            }
        }
        public int IndexSelected = 0;
        private void SelectThumb(ImageThumbControl thumb)
        {
            if (_selectedThumb == thumb) return;
            if (_selectedThumb != null)
            {
                _selectedThumb.Selected = false; _selectedThumb.Invalidate();
            }
            _selectedThumb = thumb;
            if (_selectedThumb != null)
            {
                _selectedThumb.Selected = true; _selectedThumb.Invalidate();
            }

            // bắn event cũ (nếu đang dùng)
            SelectedChanged?.Invoke(this, EventArgs.Empty);

            // bắn event mới với Mat + Name
            if (_selectedThumb != null && _selectedThumb.Item != null)
            {
                IndexSelected = _items.IndexOf(_selectedThumb.Item);
                SelectedItemChanged?.Invoke(
                    this,
                    new RegisterImgSelectionChangedEventArgs(
                        _selectedThumb.Item.Name,
                        _selectedThumb.Item.Image
                    )
                );
            }

           
        }

        private void RenameSelectedInline()
        {
            if (_selectedThumb == null) return;
            _selectedThumb.BeginRename();
        }

        private void DeleteSelected()
        {
            if (_selectedThumb == null) return;
            int idx = _flow.Controls.IndexOf(_selectedThumb);
            if (idx < 0) return;

            var item = _selectedThumb.Item;
            _flow.Controls.RemoveAt(idx);
            if (UpdateGlobal)
                Global.listRegsImg.RemoveAt(idx);
            else
                Global.listSimImg.RemoveAt(idx);
            _items.Remove(item);
            _selectedThumb.Dispose();
            item.Dispose();

            _selectedThumb = null;
            if (_flow.Controls.Count > 0)
                SelectThumb((ImageThumbControl)_flow.Controls[Math.Max(0, idx - 1)]);

            ItemsChanged?.Invoke(this, EventArgs.Empty);

            UpdateThumbLayout();
        }

        private void MoveSelected(int delta)
        {
            if (_selectedThumb == null) return;
            int from = _flow.Controls.IndexOf(_selectedThumb);
            int to = from + delta;
            if (to < 0 || to >= _flow.Controls.Count) return;

            _flow.Controls.SetChildIndex(_selectedThumb, to);

            var item = _items[from];
            _items.RemoveAt(from);
            _items.Insert(to, item);

            ItemsChanged?.Invoke(this, EventArgs.Empty);

            UpdateThumbLayout();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var it in _items) it.Dispose();
                _items.Clear();
            }
            base.Dispose(disposing);
        }
    }
}
