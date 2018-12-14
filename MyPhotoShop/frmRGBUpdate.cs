using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyPhotoShop
{
    public partial class frmRGBUpdate : Form
    {
        public frmRGBUpdate()
        {
            InitializeComponent();
        }
        private frmMain mainForm;
        private int Red;
        private int Green;
        private int Blue;
        private string ConfigPath = AppDomain.CurrentDomain.BaseDirectory + "Configuration\\Config.ini";

        private void tbRed_Scroll(object sender, EventArgs e)
        {
            try
            {
                mainForm = (frmMain)this.Owner;
                txtRed.Text = tbRed.Value.ToString();
                Red = int.Parse(txtRed.Text);
                mainForm.picUpdate.Image = mainForm.picDisplay.Image;
                //mainForm.picUpdate.Visible = true;
                //mainForm.picUpdate.SizeMode = PictureBoxSizeMode.Zoom;
                //int Green = int.Parse(txtGreen.Text);
                //int Blue = int.Parse(txtBlue.Text);
                Bitmap bitmap = new Bitmap(mainForm.picDisplay.Image);
                bitmap = photoShop.KiColorBalance(bitmap, Red, Green, Blue);
                mainForm.picUpdate.Image = bitmap;
            }
            catch (Exception ex)
            {

                LogHelper.WriteError(ex.ToString());
            }
            
        }

        private void tbGreen_Scroll(object sender, EventArgs e)
        {
            try
            {
                mainForm = (frmMain)this.Owner;
                txtGreen.Text = tbGreen.Value.ToString();
                //int Red = int.Parse(txtRed.Text);
                Green = int.Parse(txtGreen.Text);
                //int Blue = int.Parse(txtBlue.Text);
                mainForm.picUpdate.Image = mainForm.picDisplay.Image;
                //mainForm.picUpdate.Visible = true;
                //mainForm.picUpdate.SizeMode = PictureBoxSizeMode.Zoom;
                Bitmap bitmap = new Bitmap(mainForm.picDisplay.Image);
                bitmap = photoShop.KiColorBalance(bitmap, Red, Green, Blue);
                mainForm.picUpdate.Image = bitmap;
            }
            catch (Exception ex)
            {

                LogHelper.WriteError(ex.ToString());
            }
            
        }

        private void tbBlue_Scroll(object sender, EventArgs e)
        {
            try
            {
                mainForm = (frmMain)this.Owner;
                txtBlue.Text = tbBlue.Value.ToString();
                //int Red = int.Parse(txtRed.Text);
                //int Green = int.Parse(txtGreen.Text);
                Blue = int.Parse(txtBlue.Text);
                mainForm.picUpdate.Image = mainForm.picDisplay.Image;
                //mainForm.picUpdate.Visible = true;
                //mainForm.picUpdate.SizeMode = PictureBoxSizeMode.Zoom;
                //mainForm.picUpdate.Width = mainForm.picDisplay.Width;
                Bitmap bitmap = new Bitmap(mainForm.picDisplay.Image);
                bitmap = photoShop.KiColorBalance(bitmap, Red, Green, Blue);
                mainForm.picUpdate.Image = bitmap;
            }
            catch (Exception ex)
            {

                LogHelper.WriteError(ex.ToString());
            }
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                string SectionName = "RGB_Update";
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

        private void frmRGBUpdate_Load(object sender, EventArgs e)
        {
            try
            {
               
                List<string> list = IniFile.ReadSections(ConfigPath);
                List<string> ReadList = new List<string>();
                if (list.Contains("RGB_Update"))
                {
                    ReadList = IniFile.GetValues(ConfigPath, "RGB_Update");

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

        private void txtRed_TextChanged(object sender, EventArgs e)
        {
            tbRed.Value = int.Parse(txtRed.Text);
        }

        private void txtGreen_TextChanged(object sender, EventArgs e)
        {
            tbGreen.Value = int.Parse(txtGreen.Text);
        }

        private void txtBlue_TextChanged(object sender, EventArgs e)
        {
            tbBlue.Value = int.Parse(txtBlue.Text);
        }
    }
}
