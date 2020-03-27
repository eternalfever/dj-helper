using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DJhelper
{
    public partial class Form3 : Form
    {
        public int sum_total = 0;
        DataTable myTable2;
        string connectionString = @"Data Source=DESKTOP-JKUAUE3\SQLEXPRESS;Initial Catalog=Fonoteka_db;Integrated Security=True";
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedItem = comboBox2.SelectedItem = "Микс";
            // TODO: This line of code loads data into the 'fonoteka_dbDataSet.song' table. You can move, or remove it, as needed.
            // TODO: This line of code loads data into the 'fonoteka_dbDataSet.person' table. You can move, or remove it, as needed.
            this.personTableAdapter.Fill(this.fonoteka_dbDataSet.person);
            // TODO: This line of code loads data into the 'fonoteka_dbDataSet.genre' table. You can move, or remove it, as needed.
            this.genreTableAdapter.Fill(this.fonoteka_dbDataSet.genre);
            string sql = "select personname AS 'Исполнитель', songtitle AS 'Название', songlength AS 'Длина', genre AS 'Жанр'," +
                " speed AS 'Скорость', lang AS 'Язык', discid AS 'Номер носителя', typeof AS 'Тип носителя'" +
                " from song INNER JOIN genre ON genre_genreid=genre.genreid" +
                " INNER JOIN person ON person_persid=person.persid INNER JOIN songdisc ON song.songid=songdisc.song_songid" +
                " INNER JOIN discinfo ON discinfo_discid=discinfo.discid order by RATING DESC";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                DataTable myTable = new DataTable();
                adapter.Fill(myTable);
                dataGridView1.DataSource = myTable;
                dataGridView1.Refresh();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            int sum_sec_inp;
            int sum_sec = 0;
            int cut = 0;
            
            string speed = "'" + comboBox1.SelectedItem.ToString() + "'";
            string lang = "'" + comboBox2.SelectedItem.ToString() + "'";
            string persons = "";
            string genres = "";
            for (int i = 0; i < listBox1.SelectedItems.Count; i++)
            {
                genres += ((DataRowView)listBox1.SelectedItems[i])["genreid"].ToString() + " ";
            }
            for (int i = 0; i < listBox2.SelectedItems.Count; i++)
            {
                persons += ((DataRowView)listBox2.SelectedItems[i])["persid"].ToString() + " ";
            }
            genres = genres.Trim().Replace(" ", ", ");
            persons = persons.Trim().Replace(" ", ", ");
            try
            {
                if (textBox1.Text == "" & textBox2.Text != "")
                {
                    sum_sec_inp = Convert.ToInt32(textBox2.Text);
                }
                else if (textBox1.Text != "" & textBox2.Text == "")
                {
                    sum_sec_inp = Convert.ToInt32(textBox1.Text) * 60;
                }
                else
                {
                    sum_sec_inp = Convert.ToInt32(textBox1.Text) * 60 + Convert.ToInt32(textBox2.Text);
                }

                if (comboBox1.SelectedItem.ToString() == "Микс")
                {
                    speed = "'Быстрая','Медленная'";
                }
                if (comboBox2.SelectedItem.ToString() == "Микс")
                {
                    lang = "'Русский', 'Иностранный'";
                }
                string sql = "select *, (cast(substring(cast(songlength as varchar), 1, 2) as int) * 3600+" +
                "cast(substring(cast(songlength as varchar), 4, 2) as int) * 60+" +
                "cast(substring(cast(songlength as varchar), 7, 2) as int))" +
                " as sumn into #t from song INNER JOIN person ON person_persid = person.persid INNER JOIN genre ON genre_genreid = genre.genreid" +
                " INNER JOIN songdisc ON song.songid=songdisc.song_songid" +
                " INNER JOIN discinfo ON discinfo_discid=discinfo.discid " +
                " order by RATING DESC;" +

                "DELETE FROM #t WHERE sumn>" + sum_sec_inp + ";" +

                "select personname AS 'Исполнитель', songtitle AS 'Название', songlength AS 'Длина'," +
                " genre AS 'Жанр', speed AS 'Скорость', lang AS 'Язык', sumn, discid AS 'Номер носителя'," +
                " typeof AS 'Тип носителя' from #t where" +
                " (genreid IN(" + genres + ") and lang in(" + lang + ") and persid in(" + persons + ") and speed in(" + speed + "));" +
                "drop table #t;";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                    DataTable myTable = new DataTable();
                    adapter.Fill(myTable);
                    dataGridView1.DataSource = myTable;
                    dataGridView1.Refresh();
                    dataGridView1.Columns["sumn"].Visible = false;
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        int length = Convert.ToInt32(dataGridView1.Rows[i].Cells[6].Value);
                        sum_sec += length;
                        cut = i;
                        if (sum_sec > sum_sec_inp)
                            break;
                    }
                    int len = dataGridView1.Rows.Count;
                    if (sum_sec > sum_sec_inp ^ (sum_sec_inp == 0))
                    {
                        for (int i = len - 1; i > cut - 1; i--)
                        {

                            dataGridView1.Rows.RemoveAt(i);

                        }

                    }
                    myTable2 = myTable.Copy();
                }
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    sum_total += Convert.ToInt32(dataGridView1.Rows[i].Cells[6].Value);
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Введите корректные данные продолжительности плейлиста.");
            }
            catch (SqlException)
            {
                MessageBox.Show("Выберите элементы в разделах 'Жанры' и 'Артисты'.");
            }
            catch (InvalidOperationException)
            {

            }
        }

    
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form6 myForm = new Form6();
            myForm.myTable3 = myTable2;
            myForm.sum_total1 = sum_total;
            myForm.Show();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                listBox1.SelectedIndex = i;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < listBox2.Items.Count; i++)
            {
                listBox2.SelectedIndex = i;
            }
        }
    }
}
