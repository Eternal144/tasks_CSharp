using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SequencerDemo
{
    public partial class TransParentListBox : ListBox
    {
        public TransParentListBox()
        {
            this.SetStyle(ControlStyles.UserPaint, true);


            //如果为 true，控件接受 alpha 组件小于 255 的 BackColor 以模拟透明。  
            //仅在 UserPaint 位设置为 true 并且父控件派生自 Control 时才模拟透明。  
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            InitializeComponent();
        }

        private void TransParentListBox_Load(object sender, EventArgs e)
        {

        }

    }
}
