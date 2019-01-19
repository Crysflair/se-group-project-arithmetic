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
    public partial class Form2 : Form
    {
       // public static string right_lv;
        public Form2()
        {
            InitializeComponent();

            textBox1.Text = Form1.Cnt.ToString();
            textBox2.Text = Form1.right.ToString();
          //  right_lv = (Convert.ToDouble(Form1.right) / Form1.Cnt).ToString();
            textBox3.Text = (Convert.ToDouble(Form1.right) / Form1.Cnt).ToString();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
