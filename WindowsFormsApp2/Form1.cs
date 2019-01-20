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
using System.IO;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        public int currentCount = 21;
        public static int right = 0;
        public static int wrong = 0;
        public static int Cnt = 0;
        private static readonly string history_path = "./ascii_history.txt";
        public int Flag = 0;
        public static bool chutienabled = true;
        public static bool historyenabled = true;
        public static int bug = 0;
        public static string[] a=new string[1000];

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
            MessageBox.Show("欢迎使用本算术练习系统！\n如果初次使用本系统请在相应输入框上按F1来了解使用方法，谢谢！");
            helpProvider1.SetHelpString(textBox1, "输入运算数范围的下界，比如练习0~20加减法时填写0");
            helpProvider1.SetHelpString(textBox4, "回车进入下一题，保证每做完一道题都要回车");
            helpProvider1.SetHelpString(textBox2, "输入的出题数不能超过1000");
            helpProvider1.SetHelpString(textBox5, "输入运算数范围的上界，比如练习0~20加减法时填写20");  // MYSRIO: 你这里原先写的是下界，估计复制粘贴忘了改，可以理解 ^_^
            helpProvider1.SetHelpString(textBox6, "输入自定义的加法符号，比如 +，ADD");
            helpProvider1.SetHelpString(textBox7, "输入自定义的减法符号，比如 -，SUB");
            helpProvider1.SetHelpString(textBox8, "输入自定义的乘法符号，比如 *，×");
            helpProvider1.SetHelpString(textBox9, "输入自定义的除法符号，比如 /，÷");
            helpProvider1.SetHelpString(textBox10, "输入自定义的乘方符号，比如 ^，**");
            helpProvider1.SetHelpString(textBox11, "请确保输入的是0-1之间的数，包括0和1");
            helpProvider1.SetHelpString(textBox12, "请确保输入的运算符个数不超过10，否则题目将很难在20秒内做完。"); // MYSRIO: 你这里原先写的是textBox11.
            

        }

        public void Timer1_Tick(object sender, EventArgs e)//做题：计时器
        {
            if (currentCount > 0)
                currentCount -= 1;
            this.label2.Text = currentCount.ToString().Trim();
           if (currentCount == 0)
            {
                timer1.Enabled = false;
              //  avoidrepeat = false;
                MessageBox.Show("时间用尽！");
               
                Console.Beep();
                //历史记录中....
                a[history.Item1.Count] = A[Cnt];
                history.Item1.Add(Q[Cnt]);
                history.Item2.Add(textBox4.Text);
               

                wrong++;
               
                    Cnt++;
                textBox4.Text = "";  //MYSRIO
                if (Cnt != int.Parse(textBox2.Text))
                {
                    textBox3.Text = Q[Cnt];//出的题目 
                    currentCount = 21;//重新计时 
                    timer1.Enabled = true;
                //    avoidrepeat = true;
                }
                progressBar1.Value += 1;//进度条加一  
                if (progressBar1.Value == int.Parse(textBox2.Text))
                {
                    MessageBox.Show("题目已全部完成，请点击提交！");
                    chutienabled = true;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)//出题：确认按钮
        {
            if (chutienabled == true)
            {
                Flag = 1;//标志是否点过确认键
                bug = 0;//标志是否是在同一次出题下反复查看历史记录
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
                    //MYSRIO: 没有异常处理直接Parse差评
                    int a_c;
                    try
                    {
                        a_c = int.Parse(a);
                    }
                    catch (Exception)
                    {
                        this.errorProvider1.SetError(this.textBox2, "非法输入.请输入100000以下的数字");
                        return;
                    }
                    //报错处理：题目数超过范围
                    if (a_c > 1000)
                    {
                        this.errorProvider1.SetError(this.textBox2, "最大生成100000道题.请输入100000以下的数字");  //MYSRIO
                    }
                    // MSYSRIO: 不进行异常处理差评
                    else
                    {
                        try
                        {
                            if (int.Parse(textBox1.Text) > int.Parse(textBox5.Text.Trim()) ||
                          double.Parse(textBox11.Text.Trim()) > 1 || double.Parse(textBox11.Text.Trim()) < 0 ||
                          int.Parse(textBox12.Text.Trim()) > 10)
                            {
                                MessageBox.Show("请保证输入满足要求！");
                                if (int.Parse(textBox1.Text) > int.Parse(textBox5.Text.Trim()))
                                    this.errorProvider1.SetError(this.textBox5, "请保证输入范围左边小于右边");
                                if (double.Parse(textBox11.Text.Trim()) > 1 || double.Parse(textBox11.Text.Trim()) < 0)
                                    this.errorProvider1.SetError(this.textBox11, "请保证输入小于或等于1大于等于0");
                                if (int.Parse(textBox12.Text.Trim()) > 10)
                                    this.errorProvider1.SetError(this.textBox12, "请保证输入小于10");
                            }
                            else
                            {
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
                                    else
                                    {
                                        MessageBox.Show("请保证输入对应加法符号！eg. +");
                                        this.errorProvider1.SetError(this.textBox6, "请保证输入对应加法符号！eg. +");
                                    }
                                }
                                if (checkBox2.Checked)
                                {
                                    str += "-";
                                    if (textBox7.Text != "")
                                    {
                                        list_print.Add(textBox7.Text);
                                    }
                                    else
                                    {
                                        MessageBox.Show("请保证输入对应减法符号！eg. -");
                                        this.errorProvider1.SetError(this.textBox7, "请保证输入对应减法符号！eg. -");
                                    }
                                }
                                if (checkBox3.Checked)
                                {
                                    str += "*";
                                    if (textBox8.Text != "")
                                    {
                                        list_print.Add(textBox8.Text);
                                    }
                                    else
                                    {
                                        MessageBox.Show("请保证输入对应乘法符号！eg. *");
                                        this.errorProvider1.SetError(this.textBox8, "请保证输入对应乘法符号！eg. *");
                                    }
                                }
                                if (checkBox4.Checked)
                                {
                                    str += "/";
                                    if (textBox9.Text != "")
                                    {
                                        list_print.Add(textBox9.Text);
                                    }
                                    else
                                    {
                                        MessageBox.Show("请保证输入对应除法符号！eg. /");
                                        this.errorProvider1.SetError(this.textBox9, "请保证输入对应除法符号！eg. /");
                                    }
                                }
                                if (checkBox5.Checked)
                                {
                                    str += "^";
                                    if (textBox10.Text != "")
                                    {
                                        list_print.Add(textBox10.Text);
                                    }
                                    else
                                    {
                                        MessageBox.Show("请保证输入对应乘方符号！eg. ^");
                                        this.errorProvider1.SetError(this.textBox10, "请保证输入对应乘方符号！eg. ^");
                                    }
                                }
                                char[] arr = str.ToCharArray();
                                string[] symbol_print = list_print.ToArray();
                                if (symbol_print.Length != arr.Length)  //MYSRIO: 这段逻辑判断代码好评! 还有机智的str转char[]
                                {
                                    MessageBox.Show("请保证运算范围和显示符号的对应！");
                                    this.errorProvider1.SetError(this.label3, "请保证运算范围和显示符号的对应！");
                                    this.errorProvider1.SetError(this.label11, "请保证运算范围和显示符号的对应！");
                                }
                                else
                                {
                                    // MSYSRIO: 不进行异常处理差评, 我在上面加了
                                    QuestionGenerator generator = new QuestionGenerator(
                                        num_range_low: int.Parse(textBox1.Text.Trim()),
                                        num_range_high: int.Parse(textBox5.Text.Trim()),
                                        use_fraction: double.Parse(textBox11.Text.Trim()),
                                        MaxNodeCeiling: int.Parse(textBox12.Text.Trim()),
                                        symbol_set: arr, symbol_print: symbol_print);
                                    // MSYRIO: 发现有无解的参数，烦
                                    try
                                    {
                                        generator.Generate(int.Parse(textBox2.Text.Trim()));
                                    }
                                    catch (TimeoutException)
                                    {
                                        MessageBox.Show("生成题目超时！该设置下无法生成足够数量的题目组合，请更改输入重试");
                                        return;
                                    }
                                   
                                    Cnt = 0;
                                    var QA_pairs = generator.Get_QA_pairs();
                                    Q = QA_pairs.Item1;
                                    A = QA_pairs.Item2;
                                    textBox3.Text = Q[Cnt];//出的题目 

                                    //初始化
                                    right = 0;
                                    wrong = 0;
                                    //做题：进度条
                                    progressBar1.Maximum = a_c;//设置最大长度值
                                    progressBar1.Value = 0;//设置当前值
                                    progressBar1.Step = 1;
                                    //做题：开始计时
                                    timer1.Interval = 1000;//设置时间间隔为1秒（1000毫秒），覆盖构造函数设置的间隔
                                    currentCount = 21;
                                    timer1.Enabled = true;
                                    //    avoidrepeat = true;
                                    chutienabled = false;
                                    historyenabled = false;
                                }

                            }
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("请保证输入满足要求！\n输入范围需左边小于右边, 真分数使用比率为0~1, 符号使用个数0~10");
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("做完题再出！");
            }
        }

        private void textBox4_KeyDown(object sender, KeyEventArgs e)//做题：输入答案框
        {
            if (timer1.Enabled)
            {
                if (e.KeyCode == Keys.Enter && textBox4.Text != "")//做题：每当按下回车时 // MYSRIO: 我觉得你想说text4(原来写的text3)
                {
                    if (Standardizer.Standardize_Number(textBox4.Text) == A[Cnt])   //直接与输入的值进行比较
                    {
                        right++;
                    }
                    else
                    {

                        Console.Beep();
                        //历史记录中....
                        a[history.Item1.Count] = A[Cnt];
                        history.Item1.Add(Q[Cnt]);
                        history.Item2.Add(textBox4.Text);


                        wrong++;
                    }
                    
                        Cnt++;
                    if (Cnt != int.Parse(textBox2.Text))
                    {
                        textBox3.Text = Q[Cnt];//出的题目 
                        currentCount = 21;//重新计时 
                    }
                    progressBar1.Value += 1;//进度条加一  
                    textBox4.Text = ""; //MYSRIO: 清空文本框
                    if (progressBar1.Value == int.Parse(textBox2.Text))
                    {
                        MessageBox.Show("恭喜完成所有题目!请点击提交按钮查看成绩结算！");  // MYSRIO
                        textBox3.Text = "";//每次答完题后清空
                        timer1.Enabled = false;
                        chutienabled = true;
                        
                    }

                }
                else if (e.KeyCode == Keys.Enter && textBox4.Text == "") // MYSRIO: 我觉得你想说text4(原来写的text3)
                {
                    MessageBox.Show("请输入答案！");
                }
            }
            else
            {
                MessageBox.Show("此时不要调戏输入答案的文本框！");
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

        private void button2_Click(object sender, EventArgs e)//提交按钮
        {
            string subPath = history_path;
            if (false == System.IO.File.Exists(subPath))
            {
                //创建pic文件夹
                //  System.IO.Directory.CreateDirectory(subPath);
                FileStream fs1 = new FileStream(history_path, FileMode.Create, FileAccess.ReadWrite);
                fs1.Close();
            }// MYSRIO: 竟然直接引用另一个类的成员 高耦合差评 不能直接把需要的参数传给构造函数吗

            bug++;
            if (Flag==1)
            {
                if(Cnt==int.Parse(textBox2.Text))
                {
                    timer1.Enabled = false;
                    Form2 score = new Form2();
                    score.ShowDialog();

                    historyenabled = true;
                }
                else
                {
                    MessageBox.Show("禁止提前交卷！");
                }
            }
            else
            {
                MessageBox.Show("尚未做题！");
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)//历史记录按钮
        {
            /*    string subPath = history_path;
                if (false == System.IO.File.Exists(subPath))
                {
                    //创建pic文件夹
                    //  System.IO.Directory.CreateDirectory(subPath);
                    FileStream fs1 = new FileStream(history_path, FileMode.Create, FileAccess.ReadWrite);
                    fs1.Close();
                }// MYSRIO: 竟然直接引用另一个类的成员 高耦合差评 不能直接把需要的参数传给构造函数吗

                bug++;
    */
            if (historyenabled)
            {
                if (Flag == 1)
                {
                    if (Cnt == int.Parse(textBox2.Text)) //提交按钮
                    {
                        timer1.Enabled = false;
                        Form3 history = new Form3();
                        history.ShowDialog();
                        // avoidrepeat = false;
                    }
                    else
                    {
                        MessageBox.Show("专心做题！做完题才能看！");
                    }
                }
                else
                {
                    Form3 history = new Form3();
                    history.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("试卷还没有提交！");
            }
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }
    }
}
