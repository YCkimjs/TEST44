using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test
{
    public partial class Form2 : Form
    {

        [DllImport("user32.dll")]
        public static extern int FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        public static extern int SendMessage(int hwnd, int wMsg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern int GetWindow(int hWnd, int uCmd);

        const int WM_LBUTTONDOWN = 0x0201;
        const int WM_LBUTTONUP = 0x0202;

        const int GW_HWNDFIRST = 0;
        const int GW_HWNDLAST = 1;
        const int GW_HWNDNEXT = 2;
        const int GW_HWNDPREV = 3;
        const int GW_OWNER = 4;
        const int GW_CHILD = 5;

         int lParam_1st;
        int lParam_2nd;
        int lParam_3rd;
        int lParam_4th;
        int lParam_5th;

        int fsdf;

        int handle;

        public Form2()
        {
            InitializeComponent();
        }

        
        private void Form2_Load(object sender, EventArgs e)
        {
            Point pt_1st = new Point(34, 80);
            Point pt_2nd = new Point(84, 36);
            Point pt_3rd = new Point(140, 36);
            Point pt_4th = new Point(380, 32);
            Point pt_5th = new Point(422, 30);

            lParam_1st = (pt_1st.Y << 16) + pt_1st.X;
            lParam_2nd = (pt_2nd.Y << 16) + pt_2nd.X;
            lParam_3rd = (pt_3rd.Y << 16) + pt_3rd.X;
            lParam_4th = (pt_4th.Y << 16) + pt_4th.X;
            lParam_5th = (pt_5th.Y << 16) + pt_5th.X;

            int hwndP = FindWindow(null, "Google Chrome");
            handle = GetHandleTh(hwndP, 1);

            return;

        }
        int GetHandleTh(int parentHandle, int index)
        {
            int hwnd1 = GetWindow(parentHandle, GW_CHILD);
            if (index == 1) return hwnd1;

            int hwnd = GetWindow(hwnd1, GW_HWNDNEXT);
            for (int _index = 2; _index < index; _index++)
            {
                hwnd = GetWindow(hwnd, GW_HWNDNEXT);
                string hexValue = hwnd.ToString("X");
            }
            return hwnd;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SendMessage(handle, WM_LBUTTONDOWN, 0, lParam_1st);
            SendMessage(handle, WM_LBUTTONUP, 0, lParam_1st);
        }


        private void button2_Click(object sender, EventArgs e)
        {
            SendMessage(handle, WM_LBUTTONDOWN, 0, lParam_2nd);
            SendMessage(handle, WM_LBUTTONUP, 0, lParam_2nd);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SendMessage(handle, WM_LBUTTONDOWN, 0, lParam_3rd);
            SendMessage(handle, WM_LBUTTONUP, 0, lParam_3rd);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SendMessage(handle, WM_LBUTTONDOWN, 0, lParam_4th);
            SendMessage(handle, WM_LBUTTONUP, 0, lParam_4th);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SendMessage(handle, WM_LBUTTONDOWN, 0, lParam_5th);
            SendMessage(handle, WM_LBUTTONUP, 0, lParam_5th);
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Show();
        }

        private void Form2_Move(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
            }
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                notifyIcon1.Visible = true;
                this.Hide();
                e.Cancel = true;
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Add(button1);
            flowLayoutPanel1.Controls.Add(button2);
            flowLayoutPanel1.AutoScroll = true;

        }
    }
}
