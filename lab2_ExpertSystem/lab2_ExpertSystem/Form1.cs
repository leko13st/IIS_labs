using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab2_ExpertSystem
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            textBox1.Text += ">";
        }

        Controller ctr = new Controller();
        string path;

        private void ВыходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void СправкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("bla-bla");
        }

        private void ВыбратьФайлToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                path = openFileDialog1.FileName;
                ctr.path = path;
                textBox1.Text += "(load " + path + ")";
                textBox1.SelectionStart = textBox1.Text.Length;
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            //try
            {
                textBox1.Text += "\r\n>";
                string strInput = textBox1.Lines[textBox1.Lines.Length - 2];

                string input = ctr.Input(strInput);
                textBox1.Text += input;

                if (input != null) textBox1.Text += "\r\n>";
                textBox1.SelectionStart = textBox1.Text.Length;
            }
            //catch { textBox1.Text += "\r\n>Некорректный ввод\r\n>"; }
        }

        private void ResetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Text += "(reset)";
            textBox1.SelectionStart = textBox1.Text.Length;
        }

        private void RunToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Text += "(run)";
            textBox1.SelectionStart = textBox1.Text.Length;
        }
    }
}
