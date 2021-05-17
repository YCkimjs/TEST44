using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            //uc_ListBox1 
            
            //lb.Dock = DockStyle.Fill;
            //uc_ListBox1.Visible = true;
            uc_ListBox1.AddItem("테스트1", "설명");
            uc_ListBox1.AddItem("테스트2", "설명");
        }


    }
}
