using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Arithmetic;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        public int currentCount = 21;
        public static int right = 0;
        public static int Count = 0;

        public Form1()
        {
            InitializeComponent();
            textBox3.Text = "1+1000000";//出的题目
            textBox4.KeyDown += new KeyEventHandler(textBox4_KeyDown); 
        }
       
        private void Form1_Load(object sender, EventArgs e)
        {
         
        }

        public void Timer1_Tick(object sender, EventArgs e)//做题：计时器
        {
            if(currentCount>0)
            currentCount -= 1;
            this.label2.Text = currentCount.ToString().Trim();
        }

        private void button3_Click(object sender, EventArgs e)//出题：确认按钮[TTTRRREEEWWWQQQ]
        {
            if (textBox1.Text == "" || textBox5.Text == "")
            {
                 this.errorProvider1.SetError(this.textBox5,"输入不能为空");
                 this.errorProvider1.SetError(this.textBox1, "输入不能为空");
                 return;
            }
            else if(textBox2.Text=="")
            {
                this.errorProvider1.SetError(this.textBox2, "输入不能为空");
                return;
            }
            else
            {
            //将输入题目数转化成int型
                string a = textBox2.Text;
                int a_c = int.Parse(a);
                //报错处理：题目数超过范围
                if (a_c > 1000)
                {
                    this.errorProvider1.SetError(this.textBox2, "请输入1000以下的数字");
                }
                //做题：进度条
                progressBar1.Maximum = a_c;//设置最大长度值
                progressBar1.Value = 0;//设置当前值
                progressBar1.Step = 1;
                //做题：开始计时
                timer1.Interval = 1000;//设置时间间隔为1秒（1000毫秒），覆盖构造函数设置的间隔
                timer1.Enabled = true;
            }        
        }

        private void textBox4_KeyDown(object sender, KeyEventArgs e)//做题：输入答案框
        {
            if (e.KeyCode == Keys.Enter&& textBox3.Text !="")//做题：每当按下回车时
            {
                currentCount = 21;//重新计时
                progressBar1.Value += 1;//进度条加一
                Count++;
                if (textBox3.Text == Calucate(result).ToString())   //直接调用Calucate这个方法计算result的值并与输入的值进行比较
                {
                    right++;
                   
                    //MessageBox.Show("回答正确！");
                }
                else
                {
                    Console.Beep();
                }
            }
        }

         private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }
       
        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 score = new Form2();
            score.ShowDialog();
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {

        }

        //    private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        //   {
        ////       private void button1_Click(object sender, EventArgs e)
        //   {

        //   }

        //  }
    }
}
