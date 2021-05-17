using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YC_COMMON
{
    public partial class uc_ListBox : UserControl
    {
        public uc_ListBox()
        {
            InitializeComponent();
        }

        private void uc_ListBox_Load(object sender, EventArgs e)
        {

        }

        public void AddItem(String strTitle, String strDesc, Image imIcon = null, object objItem = null)
        {
            uc_ListBoxItem ucItem = new uc_ListBoxItem(strTitle, strDesc, imIcon, objItem);
            //ucItem.Visible = true;
            //ucItem.Height = 100;
            //ucItem.Width = 200;
            flowLayoutPanel1.Controls.Add(ucItem);
        }
    }
}
