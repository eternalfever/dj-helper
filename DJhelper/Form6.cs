using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace DJhelper
{
    public partial class Form6 : Form
    {
        public int sum_total1;
        public DataTable myTable3;
        public Form6()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form6_Load(object sender, EventArgs e)
        {

            dataGridView1.DataSource = myTable3;
            dataGridView1.Columns["Жанр"].Visible = false;
            dataGridView1.Columns["Тип носителя"].Visible = false;
            dataGridView1.Columns["Номер носителя"].Visible = false;
            dataGridView1.Columns["Язык"].Visible = false;
            dataGridView1.Columns["Скорость"].Visible = false;
            dataGridView1.Columns["Sumn"].Visible = false;
            dataGridView1.Columns.Add("Носитель","Носитель");
            dataGridView1.Columns["Носитель"].DisplayIndex = 0;
            int numb = 1;//фиксированное количество дисков
            decimal sum = 0;
            decimal c = 0;//продолжительность на 1 диске
            decimal cd = sum_total1 / 4800;
            decimal CD = Math.Floor(cd);//необходимое целое количество дисков
            if (CD > 0)
            {
                c = sum_total1 / (CD + 1);//равномерное распределенное
                                          //количество времени по дискам
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    sum += Convert.ToDecimal(dataGridView1.Rows[i].Cells[6].Value);
                    if (sum <= c)
                    {
                        dataGridView1.Rows[i].Cells["Носитель"].Value = "CD-"+ numb.ToString();
                    }
                    else
                    {
                        numb += 1;
                        sum = 0;
                    }
                    dataGridView1.Rows[i].Cells["Носитель"].Value = "CD-" + numb.ToString();
                }
            }
            else
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    dataGridView1.Rows[i].Cells["Носитель"].Value = "CD-" + numb.ToString();
                }
            }
            textBox1.Text = numb.ToString();
            
        }
    }
}
