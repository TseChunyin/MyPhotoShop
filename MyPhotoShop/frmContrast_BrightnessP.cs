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
    public partial class frmContrast_BrightnessP : Form
    {
        public frmContrast_BrightnessP()
        {
            InitializeComponent();
        }
        private frmMain mainForm;
        private int ContrastValue;
        private int BrightnessPValue;
        private Bitmap ContrastPhoto;
        private Bitmap BrightnessPPhoto;
        private string ConfigPath = AppDomain.CurrentDomain.BaseDirectory + "Configuration\\Config.ini";

        private void skContrast_Scroll(object sender, EventArgs e)
        {
            mainForm = (frmMain)this.Owner;
            txtContrastValue.Text = skContrast.Value.ToString();
            BrightnessPValue = int.Parse(txtContrastValue.Text);
            //mainForm.picUpdate.Visible = true;
            mainForm.picUpdate.Image = mainForm.picDisplay.Image;
            Bitmap bitmap = new Bitmap(mainForm.picDisplay.Image);
            bitmap = photoShop.BrightnessP(bitmap,BrightnessPValue);
            mainForm.picUpdate.Image = bitmap;
        }

        private void skBrightnessP_Scroll(object sender, EventArgs e)
        {
            mainForm = (frmMain)this.Owner;
            txtBrightnessPValue.Text = skBrightnessP.Value.ToString();
            ContrastValue = int.Parse(txtBrightnessPValue.Text);
            //mainForm.picUpdate.Visible = true;
            mainForm.picUpdate.Image = mainForm.picDisplay.Image;
            Bitmap bitmap = new Bitmap(mainForm.picDisplay.Image);
            bitmap = photoShop.KiContrast(bitmap, ContrastValue);
            mainForm.picUpdate.Image = bitmap;
        }

        private void txtContrastValue_TextChanged(object sender, EventArgs e)
        {
            skContrast.Value = int.Parse(txtContrastValue.Text);
        }

        private void txtBrightnessPValue_TextChanged(object sender, EventArgs e)
        {
            skBrightnessP.Value = int.Parse(txtBrightnessPValue.Text);
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                string SectionName = "Contrast_BrightnessP";
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmContrast_BrightnessP_Load(object sender, EventArgs e)
        {
            try
            {

                List<string> list = IniFile.ReadSections(ConfigPath);
                List<string> ReadList = new List<string>();
                if (list.Contains("Contrast_BrightnessP"))
                {
                    ReadList = IniFile.GetValues(ConfigPath, "Contrast_BrightnessP");

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
    }
}
