using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;


namespace test
{
    public partial class Form1 : Form
    {
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal,
        int size, string filePath);
        string[] list_var;
        string exp_name;
        string[] exp_gradient;

        string[] plus_stat;
        string[] plus_stat_element;
        string[] plus_stat_coefficient;
        string[] MaxMin_stat;
        
        string[] mainmin_addtional_stat;
        string[] mainmin_addtional_effect_max;
        string[] mainmin_addtional_effect_min;

        string k = "sadsaf[test]dsads[test1] &dsma";

        Hashtable Play_data = new Hashtable();
        public Form1()
        {
            InitializeComponent();
            Play_data.Add("test1_dum", 600);
            Play_data.Add("test_dum", 500);
            Play_data.Add("test1", -50);
            Play_data.Add("test", 150);
            Play_data.Add("q1", 90);
            Play_data.Add("q2", 99);
            Play_data.Add("q3", 99);

            set_ini();         
            
        }
        
        private int get_Playdate_to_int(string target_data)
        {
            int get_date = int.Parse(Play_data[target_data].ToString());
            return get_date;
        }
        /// <summary>
        /// player class 생성자에 입력은 필요없겠다 로딩할 때 들고오기로
        /// </summary>
        private void set_ini()
        {
            string Filename = "init.ini";
            StringBuilder Ini_Varname = new StringBuilder(999);
            char x = ',';
            char y = '/';
            char z = '&';
            GetPrivateProfileString("Set_stat", "exp_stat", "", Ini_Varname, 999, Environment.CurrentDirectory + "\\" + Filename);
            list_var = Ini_Varname.ToString().Split(x);
            GetPrivateProfileString("Set_stat", "exp_name", "", Ini_Varname, 999, Environment.CurrentDirectory + "\\" + Filename);
            exp_name = Ini_Varname.ToString();

            GetPrivateProfileString("Set_stat", "plus_stat", "", Ini_Varname, 999, Environment.CurrentDirectory + "\\" + Filename);
            plus_stat = Ini_Varname.ToString().Split(x);
            GetPrivateProfileString("Set_stat", "plus_stat_element", "", Ini_Varname, 999, Environment.CurrentDirectory + "\\" + Filename);
            plus_stat_element = Ini_Varname.ToString().Split(y);
            GetPrivateProfileString("Set_stat", "plus_stat_coefficient", "", Ini_Varname, 999, Environment.CurrentDirectory + "\\" + Filename);
            plus_stat_coefficient = Ini_Varname.ToString().Split(y);

            GetPrivateProfileString("Set_stat", "MaxMin_stat", "", Ini_Varname, 999, Environment.CurrentDirectory + "\\" + Filename);
            MaxMin_stat = Ini_Varname.ToString().Split(x);

            GetPrivateProfileString("Set_stat", "mainmin_addtional_stat", "", Ini_Varname, 999, Environment.CurrentDirectory + "\\" + Filename);
            mainmin_addtional_stat = Ini_Varname.ToString().Split(x);
            GetPrivateProfileString("Set_stat", "mainmin_addtional_effect_max", "", Ini_Varname, 999, Environment.CurrentDirectory + "\\" + Filename);
            mainmin_addtional_effect_max = Ini_Varname.ToString().Split(z);
            GetPrivateProfileString("Set_stat", "mainmin_addtional_effect_min", "", Ini_Varname, 999, Environment.CurrentDirectory + "\\" + Filename);
            mainmin_addtional_effect_min = Ini_Varname.ToString().Split(z);

            GetPrivateProfileString("Set_stat", "exp_gradient", "", Ini_Varname, 999, Environment.CurrentDirectory + "\\" + Filename);
            exp_gradient = Ini_Varname.ToString().Split(x);
        }

        /// <summary>
        /// player class 생성자에 입력
        /// </summary>
        /// <param name="as_value"></param>
        private void find_exp(string as_value)
        {
            string[] list_var_dum = new string[list_var.Count()];

            for (int i = 0; i < list_var.Count(); i++)
            {
                list_var_dum[i] = list_var[i] + exp_name;
            }


            if (list_var_dum.Contains(as_value))
            {
                Set_exp(as_value);
            }
            
        }

        /// <summary>
        /// 레벨과 경험치를 들고오면 적용
        /// </summary>
        /// <param name="target_level"></param>
        /// <param name="exp"></param>
        private void Set_exp(string exp)
        {
            var index = Array.FindIndex(list_var, x => x == exp.Replace(exp_name,""));

            int var_level = get_Playdate_to_int(exp.Replace(exp_name,""));
            int var_exp = get_Playdate_to_int(exp);
            double gradient = double.Parse(exp_gradient[index]);

            int var_level_1 = 0;
            int var_exp_1 = 0; //100+((F1)*F1-F1/1.05*F1)
            int maxexp = 100 + Convert.ToInt32((var_level * var_level) - (var_level * (1 - gradient *0.01) * var_level));

            if (var_exp < 0)
            {
                var_exp_1 = maxexp - Math.Abs(var_exp) % maxexp;
                var_level_1 = var_level - (Math.Abs(var_exp) / maxexp);
                if (var_exp_1 == maxexp)
                {
                    var_exp_1 = 0;
                    var_level_1++;
                }
            }
            else if (var_exp >= maxexp)
            {
                var_exp_1 = var_exp % maxexp;
                var_level_1 = var_level + (var_exp / maxexp);
            }

            Play_data[exp.Replace(exp_name, "")] = MAXMIN_100(exp.Replace(exp_name, ""));
            Play_data[exp] = var_exp_1;
        }        
        
        /// <summary>
        /// 합연산에 쓰이는 변수 찾기
        /// </summary>
        /// <param name="as_value"></param>
        private void find_plus(string as_value)
        {
            for (int i = 0; i < plus_stat_element.Count(); i++)
            { 
                if (plus_stat_element[i].Contains(as_value))
                {
                    set_plus(i);
                }
            }     
        }
        /// <summary>
        /// 합연산 실행
        /// </summary>
        /// <param name="as_index"></param>
        private void set_plus(int as_index)
        {
            char x = ',';
            string set_stat = plus_stat[as_index];
            string[] set_element = plus_stat_element[as_index].Split(x);
            string[] set_coefficient = plus_stat_coefficient[as_index].Split(x);

            int sum = 0;

            if (set_element.Count() != set_coefficient.Count())
            {
                MessageBox.Show("ini에러");
            }

            for (int i = 0; i < set_element.Count(); i++)
            { 
                //sum = Convert.ToInt32(((sum + (get_Playdate_to_int(set_element[i]) * double.Parse(set_coefficient[i])))));
                sum = Convert.ToInt32(double.Parse(sum.ToString()) +
                      (double.Parse(get_Playdate_to_int(set_element[i]).ToString()) *
                      double.Parse(set_coefficient[i])));
            }

            Play_data[set_stat] = sum;
        }

        public static string InputBox(string title, string promptText, string value)
        {
            Form form = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();

            form.Text = title;
            label.Text = promptText;
            textBox.Text = value;
            buttonOk.Text = "OK";
            buttonOk.DialogResult = DialogResult.OK;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.ControlBox = false;
            form.AcceptButton = buttonOk;

            DialogResult dialogResult = form.ShowDialog();
            value = textBox.Text;
            return value;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(get_Playdate_to_int("q1").ToString());
            MessageBox.Show(get_Playdate_to_int("q2").ToString());             
        }
        private void button2_Click(object sender, EventArgs e)
        {
            //MAXMIN_100("test1");
            //find_plus("q1");
            find_exp("test_dum");
        }

        private void find_maxmin(string as_value)
        {
            int set_value = get_Playdate_to_int(as_value);
            Play_data[as_value] = (MaxMin_stat.Contains(as_value) ? MAXMIN_100(as_value) : set_value);
        }

        /// <summary>
        /// 100이상 넘을 시 100으로 고정
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private int MAXMIN_100(string as_value)
        {
            int value =int.Parse(Play_data[as_value].ToString());
            int gap = 0;

            if (value > 100)
            {
                gap = value - 100;
                value = 100;
                find_addtional_effect(as_value, gap);
                return value;
            }
            else if (value < 0)
            {
                gap = value;
                value = 0;
                find_addtional_effect(as_value, gap);
                return value;
            }
            else
            {
                return value;
            }
        }
        private void find_addtional_effect(string as_value, int as_gap)
        {
            if (mainmin_addtional_stat.Contains(as_value))
            {
                addtional_effect(as_value, as_gap);
            }
        }
        private void addtional_effect(string as_value, int as_gap)
        {
            int count = mainmin_addtional_stat.Count();
            int num = 0;
            string[] cal;
            char w = '/';
            char c = ',';

            for (int i = 0; i < count; i++)
            { 
                if (mainmin_addtional_stat[i]==as_value)
                {
                    num = i;
                }
            }

            string x = (as_gap > 0) ? mainmin_addtional_effect_max[num] : mainmin_addtional_effect_max[num];
            string[] xx = x.Split(w);

            if (x == "none")
            {
                return;
            }

            for (int i = 0; i < xx.Count(); i++)
            {
                cal = xx[i].Split(c);
                string pm = cal[0].Substring(0, 1);
                string var_name = cal[0].Substring(1, cal[0].Count()-1);
                double coefficient = double.Parse(cal[1]);

                double result = Math.Floor(double.Parse(Math.Abs(as_gap).ToString()) * coefficient);

                int value = get_Playdate_to_int(var_name);                
                
                switch (pm)
                    {               
                    case "-":
                        value = value - (int)result;
                        break;
                    case "+":
                        value = value + (int)result;
                        break;
                    }

                Play_data[var_name] = value;

                pm = null;
                var_name = null;    //해제            
            }

            x = null;
            xx = null;
            cal = null;
        }
        
        private void button3_Click(object sender, EventArgs e)
        {
            char x = '[';
            char y = ']';
            string z = "";
            
            string var_dum; 
            List<string> var_first = new List<string>();
            List<int> var_start = new List<int>();
            List<int> var_last = new List<int>();

            StringBuilder string_line = new StringBuilder(k);

            for (int i = 0; i < string_line.Length; i++)
            {
                if (string_line.ToString().Substring(i, 1) == "[")
                { 
                    var_start.Add(i);
                }
                if (string_line.ToString().Substring(i, 1) == "]")
                {
                    var_last.Add(i);
                }
            }
            for (int i = 0; i < var_start.Count(); i++)
            {
                var_first.Add(string_line.ToString().Substring(var_start[i], var_last[i] - var_start[i] +1));                
            }
            for (int i = 0; i < var_start.Count(); i++)
            {
                var_dum = Play_data[var_first[i].Substring(1, var_first[i].Count() - 2)].ToString();
                string_line.Replace(var_first[i], var_dum);
            }
            MessageBox.Show(string_line.ToString());
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
        }

        private void panel1_DragDrop(object sender, DragEventArgs e)
        {

        }

        private void panel1_DragEnter(object sender, DragEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }


    }
}

