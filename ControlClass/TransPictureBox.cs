using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ControlClass
{
    public partial class TransPictureBox : UserControl
    {
        public TransPictureBox()
        {
            InitializeComponent();
        }
        private bool isHovered = false;
        private bool isPressed = false;
        private bool isFocused = false;
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //不进行背景的绘制  
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x00000020; //WS_EX_TRANSPARENT  
                return cp;
            }
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            //绘制panel的背景图像  
            if (BackgroundImage != null) e.Graphics.DrawImage(this.BackgroundImage, new Point(0, 0));
        }
        [Description("修改此值，可修改分割线的颜色"), Category("自定义属性")]
        public Image DrawColor                  // 控件的自定义属性值
        { get; set; } = null;
        /*
         public Image DrawColor                  // 控件的自定义属性值
        {
            get
            {
                return drawImage;
            }
            set
            {
                drawImage = value;

                // 此处修改，为自定义属性变动时，执行的操作
                //this.Invalidate();  // 此处当颜色值属性变动时，使用新的颜色，使自定义控件重绘
            }
        }
             */

        public delegate void click_Handle(object sender, EventArgs e);
        [Description("点击控件处理逻辑"), Category("自定义事件")]
        public event click_Handle Clicked;

        // 鼠标点击控件点击时，调用此逻辑
        private void TransparentPanel_Click(object sender, EventArgs e)
        {
            Clicked?.Invoke(this, new EventArgs());
        }

        //public delegate void MouseEventHandler(object sender, MouseEventArgs e);
        //[Description("按下鼠标按钮时发生"), Category("自定义事件")]
        //public event MouseEventHandler TransMouseDown;
        //[Description("鼠标移过时发生"), Category("自定义事件")]
        //public event MouseEventHandler TransMouseMove;
        //[Description("释放鼠标按钮时发生"), Category("自定义事件")]
        //public event MouseEventHandler TransMouseUp;

        protected override void OnMouseEnter(EventArgs e)
        {
            isHovered = true;
            Invalidate();

            base.OnMouseEnter(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isPressed = true;
                Invalidate();
            }

            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            isPressed = false;
            Invalidate();

            base.OnMouseUp(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            //This will check if control got the focus
            //If not thats the only it will remove the focus color
            if (!isFocused)
            {
                isHovered = false;
            }

            Invalidate();

            base.OnMouseLeave(e);
        }
    }
}
