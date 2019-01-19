using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            int n=Form1.wrong;
            for (int i= 0; i < n;i++)
            {
                richTextBox1.Text += "题目：";
                richTextBox1.Text += Form1.history.Item1[i].ToString();
                richTextBox1.Text += " 错误答案：";
                richTextBox1.Text += Form1.history.Item2[i].ToString();
                richTextBox1.Text += "           ";
            }
            
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
    }
}
