using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileReadingProgressBar
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        const int BUFFER = 1024 * 1024 * 50;    // Размер буфера = 50 Мб, но лучше
                                                // большой не ставить, иначе скорость
                                                // только уменьшится.


        private void button1_Click(object sender, EventArgs e)
        {
            progressBar1.Maximum = 100;
            progressBar1.Value = 0;
            new Thread(GetFileCrc).Start();
        }
        void GetFileCrc()
        {
            byte[] buffer = new byte[BUFFER];
            using (FileStream reader = new FileStream(@"file", FileMode.Open, 
                FileAccess.Read))
            {

                long read_len = reader.Length;
                while (reader.Position < read_len)
                {
                    if (reader.Position + BUFFER > read_len)
                        buffer = new byte[read_len - reader.Position];

                    reader.Read(buffer, 0, buffer.Length);

                    // Считаешь CRC

                    progressBar1.Invoke(new MethodInvoker(() => {
                        progressBar1.Value = (int)(((double)(reader.Position) / (double)(read_len)) * 100.0);
                    }), null);
                }
            }
            buffer = null;
            GC.Collect(); // Освобождаем буфер, иначе будет висеть некоторое время в памяти
        }
    }
}
