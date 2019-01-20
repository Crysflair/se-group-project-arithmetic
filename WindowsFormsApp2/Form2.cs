using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApp2
{
    public partial class Form2 : Form
    {

        private static readonly string history_path = "./ascii_history.txt";

        public Form2()
        {
            InitializeComponent();
            int n = Form1.wrong;

            textBox1.Text = Form1.Cnt.ToString();
            textBox2.Text = Form1.right.ToString();
          //  right_lv = (Convert.ToDouble(Form1.right) / Form1.Cnt).ToString();
            textBox3.Text = (Convert.ToDouble(Form1.right) / Form1.Cnt).ToString();

            string path = history_path;
            if (Form1.bug == 1)
            {
                for (int j = Form1.history.Item1.Count - 1; j > Form1.history.Item1.Count - n - 1; j--)
                {
                    string str1 = System.DateTime.Now.ToString() + " 题目:" + Form1.history.Item1[j].ToString() + " 错误答案:" + Form1.history.Item2[j].ToString()+"正确答案" + Form1.a[j]+"\n"+"共做：" + Form1.Cnt.ToString()+"做错："+ Form1.wrong.ToString();
                    if (j == Form1.history.Item1.Count - n)
                    {
                        str1 += "正确率：";
                        str1 += (Convert.ToDouble(Form1.right) / Form1.Cnt).ToString();
                        str1 += "\n";
                    }

                    FileStream fs = new FileStream(path, FileMode.Append);//文本加入不覆盖
                    StreamWriter sw = new StreamWriter(fs, Encoding.Unicode);//转码

                    sw.WriteLine(str1);
                    //清空缓冲区
                    sw.Flush();
                    //关闭流
                    sw.Close();
                    fs.Close();
                }

            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
