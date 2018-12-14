using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ControlClass
{
    public partial class Mymenu : MenuStrip
    {
        public Mymenu()
        {
            InitializeComponent();
            this.Renderer = new MyMenuRender();//设置渲染
        }

        public Mymenu(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
            this.Renderer = new MyMenuRender();//设置渲染
        }
    }
}
