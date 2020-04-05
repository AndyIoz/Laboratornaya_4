using System;
using System.Windows.Forms;

namespace Lavoratornaya_4_Forma
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private int A { get; set; }
        private int B { get; set; }
        private int Count { get; set; }
        private Random Random { get; set; }

        private double[,] Table { get; set; }

        private void btnCalc_Click(object sender, EventArgs e)
        {
            try
            {
                //  Проверка на заполненность полей
                if (string.IsNullOrEmpty(textBox1.Text) && string.IsNullOrEmpty(textBox2.Text) && string.IsNullOrEmpty(textBox3.Text))
                    throw new Exception("Поля не заполнены");
                if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text)|| string.IsNullOrEmpty(textBox3.Text))
                    throw new Exception("Не указано значение парметра");

                A = Convert.ToInt32(textBox1.Text);
                B = Convert.ToInt32(textBox2.Text);
                Count = Convert.ToInt32(textBox3.Text);

                Random = new Random();

                Table = new double[Count, 8];
                double a = Random.NextDouble() * A, b = Random.NextDouble() * B;
                //  Заполнение первых значений
                Table[0, 0] = a;
                Table[0, 1] = b;
                Table[0, 2] = 0;
                Table[0, 3] = 0;
                Table[0, 4] = b;
                Table[0, 5] = Table[0, 4] - Table[0, 2];
                Table[0, 6] = 0;
                Table[0, 7] = 0;

                for (int i = 1; i < Count; ++i)
                {
                    a = Random.NextDouble() * A;
                    b = Random.NextDouble() * B;

                    Table[i, 0] = a; Table[i, 1] = b;
                    Table[i, 2] = Table[i - 1, 2] + Table[i, 0];
                    Table[i, 3] = Math.Max(Table[i, 2], Table[i - 1, 4]);
                    Table[i, 4] = Table[i, 1] + Table[i, 3];
                    Table[i, 5] = Table[i, 4] - Table[i, 2];
                    Table[i, 6] = Table[i, 5] - Table[i, 1];
                    Table[i, 7] = Table[i, 3] - Table[i - 1, 4];
                }
                ViewTable();
                toolStripLabel2.Text = String.Format("{0:F3}", GetAvgQueueTime());
                toolStripLabel4.Text = String.Format("{0:F3}", GetAvgWaitTime());
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }
        /// <summary>
        /// Вывод полученных значений в таблицу
        /// </summary>
        private void ViewTable()
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Rows.Add(Count);
            for(int i = 0; i < Count; ++i)
            {
                dataGridView1["Column1", i].Value = Table[i, 0];
                dataGridView1["Column2", i].Value = Table[i, 1];
                dataGridView1["Column3", i].Value = Table[i, 2];
                dataGridView1["Column4", i].Value = Table[i, 3];
                dataGridView1["Column5", i].Value = Table[i, 4];
                dataGridView1["Column6", i].Value = Table[i, 5];
                dataGridView1["Column7", i].Value = Table[i, 6];
                dataGridView1["Column8", i].Value = Table[i, 7];
            }
        }
        /// <summary>
        /// Подсчёт среднего времени в очереди
        /// </summary>
        /// <returns></returns>
        private double GetAvgQueueTime()
        {
            double sum=0;
            for(int i = 0; i < Count;)
            {
                sum += Table[i++, 6];
            }
            return sum / Count;
        }
        /// <summary>
        /// Подсчёт среднего времени ожидания
        /// </summary>
        /// <returns></returns>
        private double GetAvgWaitTime()
        {
            double sum = 0;
            for (int i = 0; i < Count;)
            {
                sum += Table[i++, 7];
            }
            return sum / Count;
        }
    }
}
