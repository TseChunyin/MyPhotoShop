using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyPhotoShop
{
    public partial class frmGaussianBlur : Form
    {
        [DllImport("Kernel32.dll", EntryPoint = "RtlMoveMemory", SetLastError = true)]
        private static extern void CopyMemory(IntPtr Dest, IntPtr src, int Length);                 // Marshal.Copy 居然没有从一个内存地址直接复制到另外一个内存的重载函数        

        private Bitmap Bmp;
        private IntPtr ImageCopyPointer, ImagePointer;
        private int DataLength;
        public frmGaussianBlur()
        {
            InitializeComponent();
        }

        private Bitmap SaveBitmap;
        private frmMain mainForm;
        private string ConfigPath = AppDomain.CurrentDomain.BaseDirectory + "Configuration\\Config.ini";
        private List<Bitmap> BitmapList;



        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmGaussianBlur_Load(object sender, EventArgs e)
        {
            //GaussianBlur
            try
            {
                BitmapList = new List<Bitmap>();
                mainForm = (frmMain)this.Owner;
                //picPreview.Image = mainForm.picDisplay.Image;
                //Bmp = (Bitmap)Bitmap.FromFile(mainForm.fullPath);
                Bmp = new Bitmap(mainForm.picDisplay.Image);
                BitmapData BmpData = new BitmapData();
                Bmp.LockBits(new Rectangle(0, 0, Bmp.Width, Bmp.Height), ImageLockMode.ReadWrite, Bmp.PixelFormat, BmpData); // 用原始格式LockBits,得到图像在内存中真正地址，这个地址在图像的大小，色深等未发生变化时，每次Lock返回的Scan0值都是相同的。
                ImagePointer = BmpData.Scan0;//记录图像在内存中的真正地址
                DataLength = BmpData.Stride * BmpData.Height;//记录整幅图像占用的内存大小
                ImageCopyPointer = Marshal.AllocHGlobal(DataLength);//直接用内存数据来做备份，AllocHGlobal在内部调用的是LocalAlloc函数
                CopyMemory(ImageCopyPointer, ImagePointer, DataLength);//这里当然也可以用Bitmap的Clone方式来处理，但是我总认为直接占用处理内存数据比用对象的方式速度快。
                Bmp.UnlockBits(BmpData);
                picPreview.SizeMode = PictureBoxSizeMode.CenterImage;
                picPreview.Image = Bmp;
                

                List<string> list = IniFile.ReadSections(ConfigPath);
                List<string> ReadList = new List<string>();
                if (list.Contains("GaussianBlur"))
                {
                    ReadList = IniFile.GetValues(ConfigPath, "GaussianBlur");

                    if (ReadList[0].ToString() == "True")
                        chkPreview.Checked = true;
                    else
                        chkPreview.Checked = false;
                }
            }
            catch (Exception ex)
            {

                LogHelper.WriteError(ex.ToString());
            }
        }

        private void chkPreview_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkPreview.Checked != true)
                {
                    mainForm = (frmMain)this.Owner;
                    mainForm.picUpdate.Width = mainForm.picDisplay.Width;
                    mainForm.picUpdate.Height = mainForm.picDisplay.Height;
                    mainForm.picUpdate.Visible = false;
                    mainForm.picUpdate.Image = mainForm.picDisplay.Image;

                    //mainForm.picUpdate.SizeMode = PictureBoxSizeMode.Zoom;
                }
                else
                {
                    mainForm = (frmMain)this.Owner;
                    mainForm.picUpdate.Width = mainForm.picDisplay.Width;
                    mainForm.picUpdate.Height = mainForm.picDisplay.Height;
                    mainForm.picUpdate.Visible = true;
                    mainForm.picUpdate.Image = mainForm.picDisplay.Image;
                    //mainForm.picUpdate.SizeMode = PictureBoxSizeMode.Zoom;
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.ToString());
            }
        }

        private void btnComfirm_Click(object sender, EventArgs e)
        {
            try
            {
                string SectionName = "GaussianBlur";
                List<string> list = new List<string>();
                if (chkPreview.Checked == true)
                {
                    list.Add("True");
                }
                else
                    list.Add("False");
                List<string> Key = new List<string>();
                Key.Add("Preview");
                IniFile.WriteSections(SectionName, Key, list, ConfigPath);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {

                LogHelper.WriteError(ex.ToString());
            }
        }

        private void skinTrackBar1_Scroll(object sender, EventArgs e)
        {
            try
            {
                txtValue.Text = skinTrackBar1.Value.ToString();
                mainForm = (frmMain)this.Owner;
                mainForm.picUpdate.Image = mainForm.picDisplay.Image;
                //mainForm.picUpdate.Visible = true;
                //mainForm.picUpdate.SizeMode = PictureBoxSizeMode.Zoom;
                //int Green = int.Parse(txtGreen.Text);
                //int Blue = int.Parse(txtBlue.Text);
                //Bitmap bitmap = new Bitmap(mainForm.picDisplay.Image);
                //bitmap = photoShop.KiColorBalance(bitmap, Red, Green, Blue);
                CopyMemory(ImagePointer, ImageCopyPointer, DataLength);             // 需要恢复原始的图像数据，不然模糊就会叠加了。
                Rectangle Rect = new Rectangle(0, 0, Bmp.Width, Bmp.Height);
                Bmp.GaussianBlur(ref Rect, skinTrackBar1.Value, ChkExpandEdge.Checked);
                picPreview.Image = Bmp;
                mainForm.picUpdate.Image = Bmp;
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex.ToString());
            }
        }

        private void btnSubtract_Click(object sender, EventArgs e)
        {
            //if (lblNumber.Text == "200%")
            //{
            //    lblNumber.Text = "100%";
            //    SaveBitmap = new Bitmap(picPreview.Image);
            //    picPreview.Image = photoShop.KiResizeImage(SaveBitmap, 2, 1);
            //    return;
            //}
            //if (lblNumber.Text == "300%")
            //{
            //    lblNumber.Text = "200%";
            //    SaveBitmap = new Bitmap(picPreview.Image);
            //    picPreview.Image = photoShop.KiResizeImage(SaveBitmap, 3, 1);
            //    return;
            //}
            //if (lblNumber.Text == "400%")
            //{
            //    lblNumber.Text = "300%";
            //    SaveBitmap = new Bitmap(picPreview.Image);
            //    picPreview.Image = photoShop.KiResizeImage(SaveBitmap, 4, 1);
            //    return;
            //}
            //if (lblNumber.Text == "500%")
            //{
            //    lblNumber.Text = "400%";
            //    SaveBitmap = new Bitmap(picPreview.Image);
            //    picPreview.Image = photoShop.KiResizeImage(SaveBitmap, 5, 1);
            //    return;
            //}
            //if (lblNumber.Text == "600%")
            //{
            //    lblNumber.Text = "500%";
            //    SaveBitmap = new Bitmap(picPreview.Image);
            //    picPreview.Image = photoShop.KiResizeImage(SaveBitmap, 6, 1);
            //    return;
            //}
            //if (lblNumber.Text == "700%")
            //{
            //    lblNumber.Text = "600%";
            //    SaveBitmap = new Bitmap(picPreview.Image);
            //    picPreview.Image = photoShop.KiResizeImage(SaveBitmap, 7, 1);
            //    return;
            //}
            //if (lblNumber.Text == "800%")
            //{
            //    lblNumber.Text = "700%";
            //    SaveBitmap = new Bitmap(picPreview.Image);
            //    picPreview.Image = photoShop.KiResizeImage(SaveBitmap, 8, 1);
            //    btnAdd.Enabled = false;
            //    return;
            //}
            if (lblNumber.Text=="100%")
            {
                lblNumber.Text = "67%";
                SaveBitmap = new Bitmap(picPreview.Image);
                BitmapList.Add(SaveBitmap);
                picPreview.Image = photoShop.KiResizeImage(SaveBitmap,0.33,0);
                btnAdd.Enabled = true;
                return;
            }
            if (lblNumber.Text=="67%")
            {
                lblNumber.Text = "50%";
                SaveBitmap = new Bitmap(picPreview.Image);
                BitmapList.Add(SaveBitmap);
                picPreview.Image = photoShop.KiResizeImage(SaveBitmap, 0.17,0);
                return;
            }
            if (lblNumber.Text=="50%")
            {
                lblNumber.Text = "33%";
                SaveBitmap = new Bitmap(picPreview.Image);
                BitmapList.Add(SaveBitmap);
                picPreview.Image = photoShop.KiResizeImage(SaveBitmap, 0.17,0);
                return;
            }
            if (lblNumber.Text == "33%")
            {
                lblNumber.Text = "25%";
                SaveBitmap = new Bitmap(picPreview.Image);
                BitmapList.Add(SaveBitmap);
                picPreview.Image = photoShop.KiResizeImage(SaveBitmap, 0.8,0);
                btnSubtract.Enabled = false;
                return;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (lblNumber.Text == "25%")
            {
                lblNumber.Text = "33%";
                //SaveBitmap = new Bitmap(picPreview.Image);
                //picPreview.Image = photoShop.KiResizeImage(SaveBitmap, 0.08,1);
                picPreview.Image = BitmapList[BitmapList.Count-1];
                btnSubtract.Enabled = true;
                return;
            }
            if (lblNumber.Text == "33%")
            {
                lblNumber.Text = "50%";
                //SaveBitmap = new Bitmap(picPreview.Image);
                //picPreview.Image = photoShop.KiResizeImage(SaveBitmap, 0.17, 1);
                picPreview.Image = BitmapList[BitmapList.Count - 2];
                return;
            }
            if (lblNumber.Text == "50%")
            {
                lblNumber.Text = "67%";
                //SaveBitmap = new Bitmap(picPreview.Image);
                //picPreview.Image = photoShop.KiResizeImage(SaveBitmap, 0.17, 1);
                picPreview.Image = BitmapList[BitmapList.Count - 3];
                return;
            }
            if (lblNumber.Text == "67%")
            {
                lblNumber.Text = "100%";
                //SaveBitmap = new Bitmap(picPreview.Image);
                //picPreview.Image = photoShop.KiResizeImage(SaveBitmap, 0.33, 1);
                picPreview.Image = BitmapList[BitmapList.Count - 4];
                btnAdd.Enabled = false;
                return;
            }
            //if (lblNumber.Text == "100%")
            //{
            //    lblNumber.Text = "200%";
            //    SaveBitmap = new Bitmap(picPreview.Image);
            //    picPreview.Image = photoShop.KiResizeImage(SaveBitmap, 2, 0);
            //    return;
            //}
            //if (lblNumber.Text == "200%")
            //{
            //    lblNumber.Text = "300%";
            //    SaveBitmap = new Bitmap(picPreview.Image);
            //    picPreview.Image = photoShop.KiResizeImage(SaveBitmap,3, 0);
            //    return;
            //}
            //if (lblNumber.Text == "300%")
            //{
            //    lblNumber.Text = "400%";
            //    SaveBitmap = new Bitmap(picPreview.Image);
            //    picPreview.Image = photoShop.KiResizeImage(SaveBitmap, 4, 0);
            //    return;
            //}
            //if (lblNumber.Text == "400%")
            //{
            //    lblNumber.Text = "500%";
            //    SaveBitmap = new Bitmap(picPreview.Image);
            //    picPreview.Image = photoShop.KiResizeImage(SaveBitmap, 5, 0);
            //    return;
            //}
            //if (lblNumber.Text == "500%")
            //{
            //    lblNumber.Text = "600%";
            //    SaveBitmap = new Bitmap(picPreview.Image);
            //    picPreview.Image = photoShop.KiResizeImage(SaveBitmap, 6, 0);
            //    return;
            //}
            //if (lblNumber.Text == "600%")
            //{
            //    lblNumber.Text = "700%";
            //    SaveBitmap = new Bitmap(picPreview.Image);
            //    picPreview.Image = photoShop.KiResizeImage(SaveBitmap, 7, 0);
            //    return;
            //}
            //if (lblNumber.Text == "700%")
            //{
            //    lblNumber.Text = "800%";
            //    SaveBitmap = new Bitmap(picPreview.Image);
            //    picPreview.Image = photoShop.KiResizeImage(SaveBitmap, 8, 0);
            //    btnAdd.Enabled = false;
            //    return;
            //}
        }

        private void txtValue_TextChanged(object sender, EventArgs e)
        {
            skinTrackBar1.Value = int.Parse(txtValue.Text);
        }
    }
}
