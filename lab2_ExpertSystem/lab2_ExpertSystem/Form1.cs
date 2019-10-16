using System;
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
        bool fl = false;

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
            if (openFileDialog1.ShowDialog() == DialogResult.OK) //Выбор файла для десериализации
            {
                path = openFileDialog1.FileName;
                ctr.path = path;
                textBox1.Text += "(load " + path + ")";
                InputMethod();
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            InputMethod();
        }

        private void ResetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Text += "(reset)";
            InputMethod();
        }

        private void RunToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Text += "(run)";
            InputMethod();
        }

        void InputMethod() //Метод передачи команды в млв => Получение обработанного ответа
        {
            try
            {
                textBox1.Text += "\r\n>";
                string strInput = textBox1.Lines[textBox1.Lines.Length - 2];
                string input = null;
                input = ctr.Input(strInput, checkBox1.Checked);
                if (fl)
                {
                    textBox1.Text += ctr.Print_coment(fl);
                }
                textBox1.Text += input;

                if (input != null) textBox1.Text += "\r\n>";                

                textBox1.SelectionStart = textBox1.Text.Length;
                textBox1.ScrollToCaret();
            }
            catch { textBox1.Text += "\r\n>Некорректный ввод\r\n>"; }
        }

        private void watchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!fl)
            {
                textBox1.Text += "\r\n>Компонент объяснения включён\r\n>";
                fl = true;
            }
            else
            {
                fl = false;
                textBox1.Text += "\r\n>Компонент объяснения выключён\r\n>";
            }
            InputMethod();
            textBox1.ScrollToCaret();
        }
    }
}
