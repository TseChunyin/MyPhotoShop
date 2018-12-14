using CCWin.SkinControl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyPhotoShop
{
    public partial class frmMain : Form
    {
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        Bitmap originImg;
        Image finishImg;
        Graphics g;
        DrawType dType;
        Point StartPoint, EndPoint, FontPoint;
        Pen p = new Pen(Color.Black, 1);
        bool IsDraw;
        Font font;
        Rectangle FontRect;

        /// <summary>
        /// 画笔颜色
        /// </summary>
        Color DrawColor
        {
            get { return p.Color; }
            set { p.Color = value; }
        }
        /// <summary>
        /// 画笔宽度
        /// </summary>
        float PenWidth
        {
            set { p.Width = value; }
        }
        public frmMain()
        {
            InitializeComponent();
            cmbThickness.SelectedIndex = 0;
            //将文本输入框的父容器设为PicDisplay，否则显示会出现错位
            txtWrite.Parent = PicWrite;

            this.SetStyle(ControlStyles.OptimizedDoubleBuffer|ControlStyles.AllPaintingInWmPaint| ControlStyles.UserPaint, true);
            this.UpdateStyles();

            //将线帽样式设为圆线帽，否则笔宽度变宽时会出现明显的缺口
            p.StartCap = System.Drawing.Drawing2D.LineCap.Round;
            p.EndCap = System.Drawing.Drawing2D.LineCap.Round;

            originImg = new Bitmap(PicWrite.Width, PicWrite.Height);
            g = Graphics.FromImage(originImg);
            //画布背景初始化为白底
            g.Clear(Color.Transparent);

            PicWrite.DrawColor = originImg;
            finishImg = (Image)originImg.Clone();
            this.Load += new EventHandler(frmMain_Load);
            this.DragEnter += new DragEventHandler(frmMain_DragEnter);
            this.DragDrop += new DragEventHandler(frmMain_DragDrop);
        }
        //图片路径
        public string fullPath;
        //保存原始图片
        public Bitmap SaveImage;
        //撤销
        public List<Bitmap> Ctrl_Z;
        //恢复
        public List<Bitmap> Ctrl_Y;
        //用来判断恢复的总数
        public int Ctrl_Y_Count;
        //打开的图片的宽
        public string PhotoWitdh;
        //打开的图片的高
        public string PhotoHeight;

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (picDisplay.Image != null)
            {
                DialogResult dialog = MessageBox.Show("是否确认关闭窗口", "Warring", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialog == DialogResult.No)
                    return;
                else
                    this.Close();
            }
            else
                this.Close();
        }

        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (picDisplay.Image != null)
            {
                DialogResult dialogResult = MessageBox.Show("是否确认更换图片","警告",MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.No)
                    return;
                else
                {
                    txtWrite.Visible = false;
                    txtWrite.Text = "";
                    OpenFileDialog open = new OpenFileDialog();
                    open.Filter = "All Image Files|*.bmp;*.ico;*.gif;*.jpeg;*.jpg;*.png;*.tif;*.tiff|Windows Bitmap(*.bmp)|*.bmp|Windows Icon(*.ico)|*.ico|Graphics Interchange Format (*.gif)|(*.gif)|JPEG File Interchange Format (*.jpg)|*.jpg;*.jpeg|Portable Network Graphics (*.png)|*.png|Tag Image File Format (*.tif)|*.tif;*.tiff";
                    open.FilterIndex = 1;
                    open.RestoreDirectory = true;
                    if (open.ShowDialog() == DialogResult.Cancel)
                        return;
                    else
                    {
                        fullPath = open.FileName;//你选中的图片的绝对路径
                        picDisplay.Image = Image.FromFile(fullPath);
                        PhotoWitdh = picDisplay.Image.Width.ToString();
                        PhotoHeight = picDisplay.Image.Height.ToString();
                        SaveImage = new Bitmap(picDisplay.Image);
                        picDisplay.SizeMode = PictureBoxSizeMode.Zoom;
                        picUpdate.SizeMode = PictureBoxSizeMode.Zoom;
                        DrawImage(Image.FromFile(open.FileName));

                        模式ToolStripMenuItem.Enabled = true;
                        调整ToolStripMenuItem.Enabled = true;
                        图像大小ToolStripMenuItem.Enabled = true;
                        图像旋转ToolStripMenuItem.Enabled = true;
                        风格化ToolStripMenuItem.Enabled = true;
                        模糊ToolStripMenuItem.Enabled = true;
                        锐化ToolStripMenuItem.Enabled = true;
                        柔化ToolStripMenuItem.Enabled = true;
                        放大ToolStripMenuItem.Enabled = true;
                        缩小ToolStripMenuItem.Enabled = true;
                        按屏幕大小缩放ToolStripMenuItem.Enabled = true;
                        实际像素ToolStripMenuItem.Enabled = true;
                        保存ToolStripMenuItem.Enabled = true;
                        另存为ToolStripMenuItem.Enabled = true;
                        关闭ToolStripMenuItem1.Enabled = true;
                        open.Dispose();
                    }
                }
            }
            else
            {
                OpenFileDialog open = new OpenFileDialog();
                open.Filter = "All Image Files|*.bmp;*.ico;*.gif;*.jpeg;*.jpg;*.png;*.tif;*.tiff|Windows Bitmap(*.bmp)|*.bmp|Windows Icon(*.ico)|*.ico|Graphics Interchange Format (*.gif)|(*.gif)|JPEG File Interchange Format (*.jpg)|*.jpg;*.jpeg|Portable Network Graphics (*.png)|*.png|Tag Image File Format (*.tif)|*.tif;*.tiff";
                open.FilterIndex = 1;
                open.RestoreDirectory = true;
                if (open.ShowDialog() == DialogResult.Cancel)
                    return;
                else
                {
                    fullPath = open.FileName;//你选中的图片的绝对路径
                    picDisplay.Image = Image.FromFile(fullPath);
                    PhotoWitdh = picDisplay.Image.Width.ToString();
                    PhotoHeight = picDisplay.Image.Height.ToString();
                    SaveImage = new Bitmap(picDisplay.Image);
                    picDisplay.SizeMode = PictureBoxSizeMode.Zoom;
                    picUpdate.SizeMode = PictureBoxSizeMode.Zoom;

                    模式ToolStripMenuItem.Enabled = true;
                    调整ToolStripMenuItem.Enabled = true;
                    图像大小ToolStripMenuItem.Enabled = true;
                    图像旋转ToolStripMenuItem.Enabled = true;
                    风格化ToolStripMenuItem.Enabled = true;
                    模糊ToolStripMenuItem.Enabled = true;
                    锐化ToolStripMenuItem.Enabled = true;
                    柔化ToolStripMenuItem.Enabled = true;
                    放大ToolStripMenuItem.Enabled = true;
                    缩小ToolStripMenuItem.Enabled = true;
                    按屏幕大小缩放ToolStripMenuItem.Enabled = true;
                    实际像素ToolStripMenuItem.Enabled = true;
                    保存ToolStripMenuItem.Enabled = true;
                    另存为ToolStripMenuItem.Enabled = true;
                    关闭ToolStripMenuItem1.Enabled = true;
                    open.Dispose();
                }
            }
            
        }

        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (picDisplay.Image != null)
            {
                if (File.Exists(fullPath))
                {
                    txtWrite.Visible = false;
                    txtWrite.Text = "";
                    Bitmap bitmap = new Bitmap(picDisplay.Image);
                    picDisplay.Image.Dispose();
                    File.Delete(fullPath);
                    bitmap.Save(fullPath);
                }
            }
            else
                return;
            //picDisplay.Image.Save(@"C:\Users\xjy\Documents\666.png");
        }

        private void 另存为ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (picDisplay.Image != null)
            {
                txtWrite.Visible = false;
                txtWrite.Text = "";
                string DefaultName = System.IO.Path.GetFileNameWithoutExtension(fullPath);
                string extension = Path.GetExtension(fullPath);//扩展名 ".aspx"
                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.Filter = "All Image Files|*.bmp;*.ico;*.gif;*.jpeg;*.jpg;*.png;*.tif;*.tiff|Windows Bitmap(*.bmp)|*.bmp|Windows Icon(*.ico)|*.ico|Graphics Interchange Format (*.gif)|(*.gif)|JPEG File Interchange Format (*.jpg)|*.jpg;*.jpeg|Portable Network Graphics (*.png)|*.png|Tag Image File Format (*.tif)|*.tif;*.tiff";
                //设置默认文件类型显示顺序 
                saveFile.FilterIndex = 1;
                //保存对话框是否记忆上次打开的目录 
                saveFile.RestoreDirectory = true;
                //设置默认扩展名
                saveFile.DefaultExt = extension;
                //设置默认名称
                saveFile.FileName = DefaultName;
                if (saveFile.ShowDialog() == DialogResult.Cancel)
                {
                    return;
                }
                else
                {
                    picDisplay.Image.Save(saveFile.FileName);
                    saveFile.Dispose();
                }
            }
            else
                return;

        }
        private void 关闭ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            picDisplay.Image = null;
            picUpdate.Image = null;
            模式ToolStripMenuItem.Enabled = false;
            调整ToolStripMenuItem.Enabled = false;
            图像大小ToolStripMenuItem.Enabled = false;
            图像旋转ToolStripMenuItem.Enabled = false;
            风格化ToolStripMenuItem.Enabled = false;
            模糊ToolStripMenuItem.Enabled = false;
            锐化ToolStripMenuItem.Enabled = false;
            柔化ToolStripMenuItem.Enabled = false;
            放大ToolStripMenuItem.Enabled = false;
            缩小ToolStripMenuItem.Enabled = false;
            按屏幕大小缩放ToolStripMenuItem.Enabled = false;
            实际像素ToolStripMenuItem.Enabled = false;
        }

        private void rGB修改ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap Ctrl_zBitmap = new Bitmap(picDisplay.Image);
            Ctrl_Z.Add(Ctrl_zBitmap);
            frmRGBUpdate rGBUpdate = new frmRGBUpdate();
            if (DialogResult.Cancel == rGBUpdate.ShowDialog(this))
            {
                picUpdate.Visible = false;
                picDisplay.Image = SaveImage;
            }
            else
            {
                picDisplay.Image = picUpdate.Image;
                SaveImage =new Bitmap(picDisplay.Image);
                picUpdate.Visible = false;
            }
            Bitmap Ctrl_yBitmap = new Bitmap(picDisplay.Image);
            Ctrl_Z.Add(Ctrl_yBitmap);
        }

        private void label1_Click(object sender, EventArgs e)
        {
            if (picDisplay.Image != null)
            {
                DialogResult dialog = MessageBox.Show("是否确认关闭窗口", "Warring", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialog == DialogResult.No)
                    return;
                else
                    this.Close();
            }
            else
                this.Close();
        }

        

        private void 去色ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap Ctrl_zBitmap = new Bitmap(picDisplay.Image);
            Ctrl_Z.Add(Ctrl_zBitmap);
            Bitmap bitmap = new Bitmap(picDisplay.Image);
            //Bitmap bitmap2 = ToGray(bitmap);
            //picDisplay.Image = bitmap2;
            picDisplay.Image =photoShop.ToGray(bitmap);
            Bitmap Ctrl_yBitmap = new Bitmap(picDisplay.Image);
            Ctrl_Y.Add(Ctrl_yBitmap);
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            this.AllowDrop = true;
            Ctrl_Z = new List<Bitmap>();
            Ctrl_Y = new List<Bitmap>();
            Ctrl_Y_Count = 0;
            if (picDisplay.Image==null)
            {
                模式ToolStripMenuItem.Enabled = false;
                调整ToolStripMenuItem.Enabled = false;
                图像大小ToolStripMenuItem.Enabled = false;
                图像旋转ToolStripMenuItem.Enabled = false;
                风格化ToolStripMenuItem.Enabled = false;
                模糊ToolStripMenuItem.Enabled = false;
                锐化ToolStripMenuItem.Enabled = false;
                柔化ToolStripMenuItem.Enabled = false;
                放大ToolStripMenuItem.Enabled = false;
                缩小ToolStripMenuItem.Enabled = false;
                按屏幕大小缩放ToolStripMenuItem.Enabled = false;
                实际像素ToolStripMenuItem.Enabled = false;
                保存ToolStripMenuItem.Enabled = false;
                另存为ToolStripMenuItem.Enabled = false;
                关闭ToolStripMenuItem1.Enabled = false;
            }
            
        }

        private void frmMain_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.All;
            else e.Effect = DragDropEffects.None;
        }

        private void frmMain_DragDrop(object sender, DragEventArgs e)
        {
            //获取第一个文件名
            fullPath = (e.Data.GetData(DataFormats.FileDrop, false) as String[])[0];
            try
            {
                this.picDisplay.ImageLocation = fullPath;
                picDisplay.SizeMode = PictureBoxSizeMode.Zoom;
                picUpdate.SizeMode = PictureBoxSizeMode.Zoom;
                模式ToolStripMenuItem.Enabled = true;
                调整ToolStripMenuItem.Enabled = true;
                图像大小ToolStripMenuItem.Enabled = true;
                图像旋转ToolStripMenuItem.Enabled = true;
                风格化ToolStripMenuItem.Enabled = true;
                模糊ToolStripMenuItem.Enabled = true;
                锐化ToolStripMenuItem.Enabled = true;
                柔化ToolStripMenuItem.Enabled = true;
                放大ToolStripMenuItem.Enabled = true;
                缩小ToolStripMenuItem.Enabled = true;
                按屏幕大小缩放ToolStripMenuItem.Enabled = true;
                实际像素ToolStripMenuItem.Enabled = true;
                保存ToolStripMenuItem.Enabled = true;
                另存为ToolStripMenuItem.Enabled = true;
                关闭ToolStripMenuItem1.Enabled = true;
            }
            catch (Exception)
            {

                MessageBox.Show("文件格式不对");
            }
        }
        private void mymenu1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x0112, 0xF012, 0);
        }

        private void 度ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap Ctrl_zBitmap = new Bitmap(picDisplay.Image);
            Ctrl_Z.Add(Ctrl_zBitmap);
            Bitmap bitmap = new Bitmap(picDisplay.Image);
            bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
            picDisplay.Image = bitmap;
            Bitmap Ctrl_yBitmap = new Bitmap(picDisplay.Image);
            Ctrl_Y.Add(Ctrl_yBitmap);
        }

        private void 度顺时针ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap Ctrl_zBitmap = new Bitmap(picDisplay.Image);
            Ctrl_Z.Add(Ctrl_zBitmap);
            Bitmap bitmap = new Bitmap(picDisplay.Image);
            bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
            picDisplay.Image = bitmap;
            Bitmap Ctrl_yBitmap = new Bitmap(picDisplay.Image);
            Ctrl_Y.Add(Ctrl_yBitmap);
        }

        private void 度逆时针ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap Ctrl_zBitmap = new Bitmap(picDisplay.Image);
            Ctrl_Z.Add(Ctrl_zBitmap);
            Bitmap bitmap = new Bitmap(picDisplay.Image);
            bitmap.RotateFlip(RotateFlipType.Rotate270FlipNone);
            picDisplay.Image = bitmap;
            Bitmap Ctrl_yBitmap = new Bitmap(picDisplay.Image);
            Ctrl_Y.Add(Ctrl_yBitmap);
        }

        private void 水平翻转ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap Ctrl_zBitmap = new Bitmap(picDisplay.Image);
            Ctrl_Z.Add(Ctrl_zBitmap);
            Bitmap bitmap = new Bitmap(picDisplay.Image);
            bitmap.RotateFlip(RotateFlipType.Rotate180FlipY);
            picDisplay.Image = bitmap;
            Bitmap Ctrl_yBitmap = new Bitmap(picDisplay.Image);
            Ctrl_Y.Add(Ctrl_yBitmap);
        }

        private void 垂直翻转ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap Ctrl_zBitmap = new Bitmap(picDisplay.Image);
            Ctrl_Z.Add(Ctrl_zBitmap);
            Bitmap bitmap = new Bitmap(picDisplay.Image);
            bitmap.RotateFlip(RotateFlipType.Rotate180FlipX);
            picDisplay.Image = bitmap;
            Bitmap Ctrl_yBitmap = new Bitmap(picDisplay.Image);
            Ctrl_Y.Add(Ctrl_yBitmap);
        }

        private void 撤销ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Ctrl_Z.Count == 0)
                return;
            else
            {
                picDisplay.Image = Ctrl_Z[Ctrl_Z.Count - 1];
                Ctrl_Z.RemoveAt(Ctrl_Z.Count - 1);
            }
            
        }

        private void 恢复ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Ctrl_Y.Count == 0)
                return;
            else
            {
                if (Ctrl_Y_Count >= Ctrl_Y.Count)
                    return;
                else
                {
                    picDisplay.Image = Ctrl_Y[Ctrl_Z.Count];
                    Bitmap bitmap = new Bitmap(picDisplay.Image);
                    Ctrl_Z.Add(bitmap);
                    Ctrl_Y_Count++;
                }
                
            }
                
        }

        private void frmMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Add&&e.Modifiers==Keys.Control)
            {
                picDisplay.Width = picDisplay.Width + 10;
                picDisplay.Height = picDisplay.Height + 10;
                picUpdate.Width = picDisplay.Width;
                picUpdate.Height = picDisplay.Height;
            }
            if (e.KeyCode==Keys.Subtract&&e.Modifiers==Keys.Control)
            {
                picDisplay.Width = picDisplay.Width - 10;
                picDisplay.Height = picDisplay.Height - 10;
                picUpdate.Width = picDisplay.Width;
                picUpdate.Height = picDisplay.Height;
            }
        }

        private void 按屏幕大小缩放ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //picDisplay.Dock = DockStyle.Fill;
            picDisplay.Width = 1000;
            picDisplay.Height = 500;
            picUpdate.Width = picDisplay.Width;
            picUpdate.Height = picDisplay.Height;
        }

        private void 实际像素ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            picDisplay.Dock = DockStyle.None;
            picDisplay.Width = int.Parse(PhotoWitdh);
            picDisplay.Height = int.Parse(PhotoHeight);
            picUpdate.Width = picDisplay.Width;
            picUpdate.Height = picUpdate.Height;
        }

        private void 放大ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            picDisplay.Width = picDisplay.Width + 10;
            picDisplay.Height = picDisplay.Height + 10;
        }

        private void 缩小ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            picDisplay.Width = picDisplay.Width- 10;
            picDisplay.Height = picDisplay.Height - 10;
        }

        private void 亮度对比度ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap Ctrl_zBitmap = new Bitmap(picDisplay.Image);
            Ctrl_Z.Add(Ctrl_zBitmap);
            frmContrast_BrightnessP contrast_ = new frmContrast_BrightnessP();
            if (DialogResult.Cancel == contrast_.ShowDialog(this))
            {
                picUpdate.Visible = false;
                picDisplay.Image = SaveImage;
            }
            else
            {
                picDisplay.Image = picUpdate.Image;
                SaveImage = new Bitmap(picDisplay.Image);
                picUpdate.Visible = false;
            }
            Bitmap Ctrl_yBitmap = new Bitmap(picDisplay.Image);
            Ctrl_Z.Add(Ctrl_yBitmap);
        }

        private void 反相ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap Ctrl_zBitmap = new Bitmap(picDisplay.Image);
            Ctrl_Z.Add(Ctrl_zBitmap);
            Bitmap bitmap = new Bitmap(picDisplay.Image);
            //Bitmap bitmap2 = ToGray(bitmap);
            //picDisplay.Image = bitmap2;
            picDisplay.Image = photoShop.NegativeImage(bitmap);
            Bitmap Ctrl_yBitmap = new Bitmap(picDisplay.Image);
            Ctrl_Y.Add(Ctrl_yBitmap);
        }

        private void 浮雕效果ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap Ctrl_zBitmap = new Bitmap(picDisplay.Image);
            Ctrl_Z.Add(Ctrl_zBitmap);
            Bitmap bitmap = new Bitmap(picDisplay.Image);
            //Bitmap bitmap2 = ToGray(bitmap);
            //picDisplay.Image = bitmap2;
            picDisplay.Image = photoShop.EmbossmentImage(bitmap);
            Bitmap Ctrl_yBitmap = new Bitmap(picDisplay.Image);
            Ctrl_Y.Add(Ctrl_yBitmap);
        }

        private void 雾化ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap Ctrl_zBitmap = new Bitmap(picDisplay.Image);
            Ctrl_Z.Add(Ctrl_zBitmap);
            Bitmap bitmap = new Bitmap(picDisplay.Image);
            //Bitmap bitmap2 = ToGray(bitmap);
            //picDisplay.Image = bitmap2;
            picDisplay.Image = photoShop.AtomizationImage(bitmap);
            Bitmap Ctrl_yBitmap = new Bitmap(picDisplay.Image);
            Ctrl_Y.Add(Ctrl_yBitmap);
        }

        private void 锐化ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap Ctrl_zBitmap = new Bitmap(picDisplay.Image);
            Ctrl_Z.Add(Ctrl_zBitmap);
            Bitmap bitmap = new Bitmap(picDisplay.Image);
            //Bitmap bitmap2 = ToGray(bitmap);
            //picDisplay.Image = bitmap2;
            picDisplay.Image = photoShop.SharpenImage(bitmap);
            Bitmap Ctrl_yBitmap = new Bitmap(picDisplay.Image);
            Ctrl_Y.Add(Ctrl_yBitmap);
        }

        private void 柔化ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap Ctrl_zBitmap = new Bitmap(picDisplay.Image);
            Ctrl_Z.Add(Ctrl_zBitmap);
            Bitmap bitmap = new Bitmap(picDisplay.Image);
            //Bitmap bitmap2 = ToGray(bitmap);
            //picDisplay.Image = bitmap2;
            picDisplay.Image = photoShop.SoftenImage(bitmap);
            Bitmap Ctrl_yBitmap = new Bitmap(picDisplay.Image);
            Ctrl_Y.Add(Ctrl_yBitmap);
        }

        private void 高斯模糊ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap Ctrl_zBitmap = new Bitmap(picDisplay.Image);
            Ctrl_Z.Add(Ctrl_zBitmap);
            frmGaussianBlur gaussian = new frmGaussianBlur();
            if (DialogResult.Cancel == gaussian.ShowDialog(this))
            {
                picUpdate.Visible = false;
                picDisplay.Image = SaveImage;
            }
            else
            {
                picDisplay.Image = picUpdate.Image;
                SaveImage = new Bitmap(picDisplay.Image);
                picUpdate.Visible = false;
            }
            Bitmap Ctrl_yBitmap = new Bitmap(picDisplay.Image);
            Ctrl_Z.Add(Ctrl_yBitmap);

        }

        private void 模糊ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = new Bitmap(picDisplay.Image);
            bitmap = photoShop.Blur(bitmap);
            picDisplay.Image = bitmap;
        }

        private void 灰度ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dialog = MessageBox.Show("是否扔掉颜色信息？\n\n要控制转换,请使用\n\"图像\">\"调整\">\"去色\"。","提示",MessageBoxButtons.OKCancel);
            if (dialog == DialogResult.Cancel)
                return;
            else
            {
                Bitmap Ctrl_zBitmap = new Bitmap(picDisplay.Image);
                Ctrl_Z.Add(Ctrl_zBitmap);
                rGB颜色ToolStripMenuItem.Checked = false;
                Bitmap bitmap = new Bitmap(picDisplay.Image);
                //Bitmap bitmap2 = ToGray(bitmap);
                //picDisplay.Image = bitmap2;
                picDisplay.Image = photoShop.ToGray(bitmap);
                Bitmap Ctrl_yBitmap = new Bitmap(picDisplay.Image);
                Ctrl_Y.Add(Ctrl_yBitmap);
            }
        }

        private void 指针ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dType = DrawType.None;
            txtWrite.Visible = false;
            txtWrite.Text = "";
        }

        private void 画笔ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dType = DrawType.Pen;
            txtWrite.Visible = false;
            txtWrite.Text = "";
        }

        private void aboutMeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAbout about = new frmAbout();
            about.ShowDialog();
        }

        private void 线ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dType = DrawType.Line;
            txtWrite.Visible = false;
            txtWrite.Text = "";
        }

        private void 矩形ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dType = DrawType.Rect;
            txtWrite.Visible = false;
            txtWrite.Text = "";
        }

        private void 椭圆ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dType = DrawType.Ellipse;
            txtWrite.Visible = false;
            txtWrite.Text = "";
        }

        private void 橡皮檫ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dType = DrawType.Eraser;
            txtWrite.Visible = false;
            txtWrite.Text = "";
        }

        private void 写字ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dType = DrawType.Write;
            FontDialog fd = new FontDialog();//写字前先选择字体
            if (fd.ShowDialog()==DialogResult.OK)
            {
                font = fd.Font;
            }
        }

        private void PicWrite_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button==MouseButtons.Left)
            {
                IsDraw = true;
                StartPoint = e.Location;
                switch (dType)
                {
                    case DrawType.Pen:
                    case DrawType.Eraser:
                        finishImg = (Image)originImg.Clone();
                        break;
                    case DrawType.Write://隐藏写字板
                        if (!txtWrite.Bounds.Contains(StartPoint))
                        {
                            txtWrite.Visible = false;
                            DrawString(txtWrite.Text);
                            txtWrite.Text = "";
                            return;
                        }
                        break;
                }
            }
        }

        private void 裁剪ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            picDisplay.BringToFront();
            picDisplay.MouseDown += new MouseEventHandler(picDisplay_MouseDown);
            picDisplay.MouseMove += new MouseEventHandler(picDisplay_MouseMove);
            picDisplay.MouseUp += new MouseEventHandler(picDisplay_MouseUp);
            picDisplay.Paint += new PaintEventHandler(picDisplay_Paint);
        }
        #region 绘制及剪切
        Point fPtStart;                     //开始位置矩形的位置
        bool fHaveImage = false;            //是否有图片
        Point fPtFirst;                     //鼠标按下去的位置
        Point fPtSecond;                    //鼠标当前位置
        /// <summary>
        /// 设置图片显示模式
        /// </summary>
        /// <param name="pImage"></param>
        private void SetImageSizeMode(PictureBox pImage)
        {
            if (pImage == null) return;
            Image imgResult = pImage.Image;
            if (imgResult != null)
            {
                //控制图片显示方式
                if (imgResult.Width > pImage.Width || imgResult.Height > pImage.Height)
                {
                    pImage.SizeMode = PictureBoxSizeMode.Zoom;
                }
                else
                {
                    pImage.SizeMode = PictureBoxSizeMode.CenterImage;
                }
            }
        }
        /// <summary>
        /// 记录鼠标按下去的位置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picDisplay_MouseDown(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Cross;
            fPtFirst = new Point(e.X, e.Y);
            fPtStart = new Point(e.X, e.Y);
            if ((sender as PictureBox).Image != null)
            {
                this.fHaveImage = true;
            }
            else
            {
                this.fHaveImage = false;
            }
        }

        private void picDisplay_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.Cursor == Cursors.Cross)
            {
                fPtSecond = new Point(e.X, e.Y);


                if ((fPtSecond.X - fPtFirst.X) > 0 && (fPtSecond.Y - fPtFirst.Y) > 0)
                {
                    //鼠标当前位置在，按下去位置的右下角
                    fPtStart = new Point(fPtFirst.X, fPtFirst.Y);
                }
                else if ((fPtSecond.X - fPtFirst.X) > 0 && (fPtSecond.Y - fPtFirst.Y) < 0)
                {
                    //鼠标当前位置在，按下去位置的右上角
                    fPtStart = new Point(fPtFirst.X, fPtSecond.Y);
                }
                else if ((fPtSecond.X - fPtFirst.X) < 0 && (fPtSecond.Y - fPtFirst.Y) > 0)
                {
                    //鼠标当前位置在，按下去位置的左下角
                    fPtStart = new Point(fPtSecond.X, fPtFirst.Y);
                }
                else
                {
                    //鼠标当前位置在，按下去位置的左上角
                    fPtStart = new Point(fPtSecond.X, fPtSecond.Y);
                }
                picDisplay.Invalidate();       //移动时绘制矩形选择框
            }
        }

        /// <summary>
        /// 剪切图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picDisplay_MouseUp(object sender, MouseEventArgs e)
        {
            if (this.fHaveImage == false) return;
            try
            {
                fPtSecond = new Point(e.X, e.Y);
                if ((fPtSecond.X - fPtFirst.X) > 0 && (fPtSecond.Y - fPtFirst.Y) > 0)
                {
                    //鼠标当前位置在，按下去位置的右下角
                    fPtStart = new Point(fPtFirst.X, fPtFirst.Y);
                }
                else if ((fPtSecond.X - fPtFirst.X) > 0 && (fPtSecond.Y - fPtFirst.Y) < 0)
                {
                    //鼠标当前位置在，按下去位置的右上角
                    fPtStart = new Point(fPtFirst.X, fPtSecond.Y);
                }
                else if ((fPtSecond.X - fPtFirst.X) < 0 && (fPtSecond.Y - fPtFirst.Y) > 0)
                {
                    //鼠标当前位置在，按下去位置的左下角
                    fPtStart = new Point(fPtSecond.X, fPtFirst.Y);
                }
                else
                {
                    //鼠标当前位置在，按下去位置的左上角
                    fPtStart = new Point(fPtSecond.X, fPtSecond.Y);
                }

                Rectangle rectDisplay = this.GetPictureDisplaySize(picDisplay);        //图片实际显示的大小


                int Width = Math.Abs((fPtSecond.X - fPtFirst.X));
                int Height = Math.Abs((fPtSecond.Y - fPtFirst.Y));

                double dWRate = picDisplay.Image.Width / (double)rectDisplay.Width;    //缩放比例：宽
                double dHRate = picDisplay.Image.Height / (double)rectDisplay.Height;  //缩放比例：高

                int intRealWidth = (int)(dWRate * Width);           //实际需要截取的图片宽度
                int intRealHeight = (int)(dHRate * Height);         //实际需要截取的图片高度

                int intRealX = (int)((fPtStart.X - rectDisplay.X) * dWRate); //实际的X坐标
                int intRealY = (int)((fPtStart.Y - rectDisplay.Y) * dHRate); //实际的Y坐标

                Bitmap bmpDest = new Bitmap(intRealWidth, intRealHeight, PixelFormat.Format32bppRgb);       //目标图片大小

                Graphics g = Graphics.FromImage(bmpDest);               //创建GDI

                Rectangle rectDest = new Rectangle(0, 0, intRealWidth, intRealHeight);
                Rectangle rectSource = new Rectangle(intRealX, intRealY, intRealWidth, intRealHeight);

                g.DrawImage(picDisplay.Image, rectDest, rectSource, GraphicsUnit.Pixel);               //绘图

                picDisplay.Image = (Image)bmpDest;

                g.Dispose();
                SetImageSizeMode(picDisplay);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            { this.Cursor = Cursors.Default; }
        }

        /// <summary>
        /// 获取图片实际显示的大小
        /// </summary>
        /// <param name="pbxImage"></param>
        /// <returns></returns>
        public Rectangle GetPictureDisplaySize(PictureBox pbxImage)
        {
            if (pbxImage != null)
            {
                PropertyInfo ppiImageRect = pbxImage.GetType().GetProperty("ImageRectangle", BindingFlags.Instance | BindingFlags.NonPublic);
                return (Rectangle)ppiImageRect.GetValue(pbxImage, null);
            }
            return new Rectangle(0, 0, 1, 1);
        }

        /// <summary>
        /// 绘制矩形选择框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picDisplay_Paint(object sender, PaintEventArgs e)
        {
            int intWidth = 0;
            int intHeight = 0;
            if (this.fHaveImage == true)
            {
                Pen p = new Pen(Color.Black, 1);
                p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

                intWidth = Math.Abs(fPtFirst.X - fPtSecond.X);
                intHeight = Math.Abs(fPtFirst.Y - fPtSecond.Y);

                Rectangle rectDraw = new Rectangle(fPtStart, new Size(intWidth, intHeight));
                e.Graphics.DrawRectangle(p, rectDraw);
            }
        }
        #endregion
        /// <summary>
        /// 重绘绘图区（二次缓冲技术）
        /// </summary>
        private void reDraw()
        {
            Graphics graphics = PicWrite.CreateGraphics();
            graphics.DrawImage(finishImg, new Point(0, 0));
            graphics.Dispose();
        }
        /// <summary>
        /// 在画布 写字
        /// </summary>
        /// <param name="str"></param>
        private void DrawString(string str)
        {
            g.DrawString(str, font, new SolidBrush(DrawColor), FontPoint);
            reDraw();
        }

        private void PicWrite_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsDraw)
            {
                EndPoint = e.Location;
                if (dType!=DrawType.Pen&&dType!=DrawType.Eraser)
                {
                    finishImg = (Image)originImg.Clone();
                }
                g = Graphics.FromImage(finishImg);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;//抗锯齿
                switch (dType)
                {
                    case DrawType.Line:
                        g.DrawLine(p, StartPoint, EndPoint);
                        break;
                    case DrawType.Pen:
                        g.DrawLine(p, StartPoint, EndPoint);
                        StartPoint = EndPoint;
                        break;
                    case DrawType.Rect:
                        Point leftTop = new Point(StartPoint.X,StartPoint.Y);
                        int width = Math.Abs(StartPoint.X - e.X), height = Math.Abs(StartPoint.Y - e.Y);
                        if (e.X < StartPoint.X)
                            leftTop.X = e.X;
                        if (e.Y < StartPoint.Y)
                            leftTop.Y = e.Y;
                        Rectangle rect = new Rectangle(leftTop, new Size(width, height));
                        g.DrawRectangle(p, rect);
                        break;
                    case DrawType.Ellipse:
                        leftTop = new Point(StartPoint.X, StartPoint.Y);
                        int Ewidth = Math.Abs(StartPoint.X - e.X), Eheight = Math.Abs(StartPoint.Y - e.Y);
                        if (e.X < StartPoint.X)
                            leftTop.X = e.X;
                        if (e.Y < StartPoint.Y)
                            leftTop.Y = e.Y;
                        rect = new Rectangle(leftTop, new Size(Ewidth, Eheight));
                        g.DrawEllipse(p, rect);
                        break;
                    case DrawType.Eraser:
                        Pen pen1 = new Pen(Color.White, 20);
                        pen1.StartCap = LineCap.Round;
                        pen1.StartCap = LineCap.Round;
                        g.DrawLine(pen1, StartPoint, EndPoint);
                        StartPoint = EndPoint;
                        pen1.Dispose();
                        break;
                    case DrawType.Write:  //写字前画虚线框
                        leftTop = new Point(StartPoint.X, StartPoint.Y);
                        int w = Math.Abs(StartPoint.X - e.X);
                        int h = Math.Abs(StartPoint.Y - e.Y);
                        if (e.X < StartPoint.X)
                            leftTop.X = e.X;
                        if (e.Y < StartPoint.Y)
                            leftTop.Y = e.Y;
                        FontRect = new Rectangle(leftTop, new Size(w, h));
                        Pen pRect = new Pen(Color.Black);
                        pRect.DashPattern = new float[] { 4.0F, 2.0F, 1.0F, 3.0F };
                        g.DrawRectangle(pRect, FontRect);
                        pRect.Dispose();
                        break;
                }
                reDraw();
            }
        }

        private void PicWrite_MouseUp(object sender, MouseEventArgs e)
        {
            IsDraw = false;
            originImg = (Bitmap)finishImg;
            if (dType==DrawType.Write)
            {
                //清除虚线框
                Pen pRect = new Pen(Color.White);
                g.DrawRectangle(pRect, FontRect);
                pRect.Dispose();

                //写字文本框 呈现
                txtWrite.SetBounds(FontRect.Left, FontRect.Top, FontRect.Width, FontRect.Height);
                txtWrite.Font = font;
                FontPoint = FontRect.Location;
                txtWrite.Visible = true;
                txtWrite.Focus();
            }
            //此句的作用是避免窗体最小化后还原窗体时，画布内容“丢失”
            //其实没有丢失，只是没刷新而已，读者可以在画布任意处画，便可还原画布内容
            PicWrite.DrawColor = originImg;
        }
        /// <summary>
        /// 画笔颜色设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picWhite_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            cd.FullOpen = true;
            cd.Color = DrawColor;
            if (cd.ShowDialog()==DialogResult.OK)
            {
                DrawColor = cd.Color;
                picWhite.BackColor = cd.Color;
            }
        }
        /// <summary>
        /// 画笔宽度设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbThickness_SelectedIndexChanged(object sender, EventArgs e)
        {
            PenWidth = Convert.ToSingle(cmbThickness.Text);
        }

        /// <summary>
        /// 在画布 作画
        /// </summary>
        /// <param name="img"></param>
        private void DrawImage(Image img)
        {
            g = Graphics.FromImage(finishImg);
            g.DrawImage(img, new Point(0, 0));
            reDraw();
        }
        /// <summary>
        /// 画笔类型
        /// </summary>
        enum DrawType
        {
            None,
            Pen,
            Line,
            Rect,
            Ellipse,
            Eraser,
            Write
        }
    }
    
}
