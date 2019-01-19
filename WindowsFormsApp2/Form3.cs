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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            int n=Form1.wrong;
           
            for (int i= Form1.history.Item1.Count - 1; i > Form1.history.Item1.Count - 1-n;i--)
            {
                richTextBox1.Text += "题目：";
                richTextBox1.Text += Form1.history.Item1[i].ToString();
                richTextBox1.Text += " 错误答案：";
                richTextBox1.Text += Form1.history.Item2[i].ToString();
                richTextBox1.Text += "           ";
            }
            richTextBox1.Text += " 正确率：";
            richTextBox1.Text += (Convert.ToDouble(Form1.right) / Form1.Cnt).ToString();
            richTextBox1.Text += "           ";
            string path = @"c:\temp\test\ascii.txt";
            if(Form1.avoidrepeat==true)
            {
               for(int j= Form1.history.Item1.Count-1; j > Form1.history.Item1.Count-n-1; j--)
                  //  for(int j=0;j>n;j++)
                {    
                    string str1 = System.DateTime.Now.ToString()+" 题目:" + Form1.history.Item1[j].ToString() + " 错误答案:" + Form1.history.Item2[j].ToString() + "   ";
                    if(j== Form1.history.Item1.Count - n )
                    {
                        str1 += "正确率：";
                        str1+= (Convert.ToDouble(Form1.right) / Form1.Cnt).ToString();
                        str1 += "         ";
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
           
            string str = File.ReadAllText(@"c:\temp\test\ascii.txt");
            richTextBox2.Text = str;
        }

        private void Form3_Load(object sender, EventArgs e)
        {
           
            //textBox2.Text = Form1.right.ToString();
           // textBox3.Text = (Form1.right / Form1.Count).ToString();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            File.WriteAllText(@"c:\temp\test\ascii.txt", "");
        }
    }
}
