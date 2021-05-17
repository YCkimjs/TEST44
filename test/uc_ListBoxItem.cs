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
    public partial class uc_ListBoxItem : UserControl
    {
        private String strTitle;
        public String _strTitle {
            get {
                return strTitle;
            }
            set {
                lbTitle.Text = value;
                strTitle = value;                 
            }
        }
        private String strDesc;
        public String _strDesc
        {
            get
            {
                return strDesc;
            }
            set
            {
                strDesc = value; lbDesc.Text = value;
            }
        }
        private Image imIcon;
        public Image _imIcon
        {
            get
            {
                return imIcon;
            }
            set
            {
                imIcon = value; pictureBox1.Image = value;
            }
        }
        private object objItem;
        public object _objItem
        {
            get
            {
                return objItem;
            }
            set
            {
                objItem = value;
            }
        }

        public uc_ListBoxItem(String strTitle, String strDesc, Image imIcon = null, object objItem = null)
        {
            InitializeComponent();

            this._strTitle = strTitle;
            this._strDesc = strDesc;
            this._imIcon = imIcon;
            this._objItem = objItem;
        }

        private void uc_ListBoxItem_Load(object sender, EventArgs e)
        {

        }
    }
}
