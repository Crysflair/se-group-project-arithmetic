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
        public static int wrong = 0;
        public static int Cnt = 0;

        private static List<string> Q = new List<string>();
        private static List<string> A = new List<string>();
        public static Tuple<List<string>, List<string>> history = Tuple.Create(Q, A);
        public Form1()
        {
            InitializeComponent();
            //textBox3.Text = "1+1000000";//出的题目 
            textBox4.KeyDown += new KeyEventHandler(textBox4_KeyDown);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public void Timer1_Tick(object sender, EventArgs e)//做题：计时器
        {
            if (currentCount > 0)
                currentCount -= 1;
            this.label2.Text = currentCount.ToString().Trim();
        }

        private void button3_Click(object sender, EventArgs e)//出题：确认按钮
        {
            if (textBox1.Text == "" || textBox5.Text == "")
            {
                this.errorProvider1.SetError(this.textBox5, "输入不能为空");
                this.errorProvider1.SetError(this.textBox1, "输入不能为空");
                return;
            }
            else if (textBox2.Text == "")
            {
                this.errorProvider1.SetError(this.textBox2, "输入不能为空");
                return;
            }
            else if (textBox11.Text == "")
            {
                this.errorProvider1.SetError(this.textBox11, "输入不能为空");
                return;
            }
            else if (textBox12.Text == "")
            {
                this.errorProvider1.SetError(this.textBox12, "输入不能为空");
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
                //出题：
                string str = "";

                List<string> list_print = new List<string>();

                if (checkBox1.Checked)
                {
                    str += "+";
                    if (textBox6.Text != "")
                    {
                        list_print.Add(textBox6.Text);
                    }
                }
                if (checkBox2.Checked)
                {
                    str += "-";
                    if (textBox7.Text != "")
                    {
                        list_print.Add(textBox7.Text);
                    }
                }
                if (checkBox3.Checked)
                {
                    str += "*";
                    if (textBox8.Text != "")
                    {
                        list_print.Add(textBox8.Text);
                    }
                }
                if (checkBox4.Checked)
                {
                    str += "/";
                    if (textBox9.Text != "")
                    {
                        list_print.Add(textBox9.Text);
                    }
                }
                if (checkBox5.Checked)
                {
                    str += "^";
                    if (textBox10.Text != "")
                    {
                        list_print.Add(textBox10.Text);
                    }
                }
                char[] arr = str.ToCharArray();
                string[] symbol_print = list_print.ToArray();

                QuestionGenerator generator = new QuestionGenerator(
                    num_range_low: int.Parse(textBox1.Text),
                    num_range_high: int.Parse(textBox5.Text.Trim()),
                    use_fraction: double.Parse(textBox11.Text.Trim()),
                    MaxNodeCeiling: int.Parse(textBox12.Text.Trim()),
                    symbol_set: arr, symbol_print: symbol_print);
                generator.Generate(int.Parse(textBox2.Text.Trim()));

                var QA_pairs = generator.Get_QA_pairs();
                Q = QA_pairs.Item1;
                A = QA_pairs.Item2;
                textBox3.Text = Q[Cnt];//出的题目 

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
            if (e.KeyCode == Keys.Enter && textBox3.Text != "")//做题：每当按下回车时
            {
                if (textBox4.Text == A[Cnt])   //直接调用Calucate这个方法计算result的值并与输入的值进行比较
                {
                    right++;
                }
                else
                {
                    Console.Beep();
                    //历史记录中....
                    history.Item1.Add(Q[Cnt]);
                    history.Item2.Add(textBox3.Text);
                    wrong++;
                }
                Cnt++;
                if (Cnt != int.Parse(textBox2.Text))
                {
                    textBox3.Text = Q[Cnt];//出的题目 
                    currentCount = 21;//重新计时 
                }
                progressBar1.Value += 1;//进度条加一  
                if (progressBar1.Value == int.Parse(textBox2.Text))
                {
                    MessageBox.Show("请点击提交！");
                }
            }
            else if (e.KeyCode == Keys.Enter && textBox3.Text == "")
            {
                MessageBox.Show("请输入答案！");
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

        private void button1_Click(object sender, EventArgs e)
        {
            Form3 history = new Form3();
            history.ShowDialog();
        }

        private void label10_Click(object sender, EventArgs e)
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
