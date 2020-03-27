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
    public partial class Form2 : Form
    {
        string connectionString = @"Data Source=DESKTOP-JKUAUE3\SQLEXPRESS;Initial Catalog=Fonoteka_db;Integrated Security=True";
        public Form2()
        {
            InitializeComponent();
        }

        private void genreBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.genreBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.fonoteka_dbDataSet);
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'fonoteka_dbDataSet.person' table. You can move, or remove it, as needed.
            this.personTableAdapter.Fill(this.fonoteka_dbDataSet.person);
            // TODO: This line of code loads data into the 'fonoteka_dbDataSet.genre' table. You can move, or remove it, as needed.
            this.genreTableAdapter.Fill(this.fonoteka_dbDataSet.genre);         
            string sql = "SELECT personname AS 'Исполнитель', songtitle AS 'Название', songlength AS 'Длина'"+
                " FROM song INNER JOIN genre ON genre_genreid=genre.genreid"+
                " INNER JOIN person ON person_persid=person.persid"+
                " order by rating DESC";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                DataTable myTable = new DataTable();
                adapter.Fill(myTable);
                dataGridView2.DataSource = myTable;
                dataGridView1.DataSource = myTable;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {        
            int sum_sec_inp;
            int cut = 0;
            int sum_sec = 0;
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
                string sql = "select *,(cast(substring(cast(songlength as varchar), 1, 2) as int) * 3600"+
                    " + cast(substring(cast(songlength as varchar), 4, 2) as int) * 60"+
                    " +cast(substring(cast(songlength as varchar), 7, 2) as int)) as sumn"+
                    " into #t from song INNER JOIN person ON person_persid=person.persid"+
                    " INNER JOIN genre ON genre_genreid=genre.genreid order by RATING DESC;"+
                    "DELETE FROM #t WHERE sumn>" + sum_sec_inp +
                    ";select personname AS 'Исполнитель', songtitle AS 'Название', songlength AS 'Длина', sumn from #t where genreid="
                    + genreComboBox.SelectedValue.ToString() + 
                    ";drop table #t;";
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
                        int length = Convert.ToInt32(dataGridView1.Rows[i].Cells[3].Value);
                        sum_sec += length;
                        cut = i;
                        if (sum_sec > sum_sec_inp)
                            break;
                    }
                    int len = dataGridView1.Rows.Count;
                    if (sum_sec > sum_sec_inp ^ (sum_sec_inp == 0))
                    {
                        for (int i = len-1; i >cut-1; i--)
                        {

                            dataGridView1.Rows.RemoveAt(i);

                        }

                    }
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Укажите корректные данные продолжительности плейлиста.");
            }
            catch(NullReferenceException)
            {
                MessageBox.Show("Выберите существующий жанр.");
            }
            catch (InvalidOperationException)
            {
               
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int sum_sec_inp;
            int cut = 0;
            int sum_sec = 0;
            try
            {
                if (textBox4.Text == "" & textBox3.Text != "")
                {
                    sum_sec_inp = Convert.ToInt32(textBox3.Text);
                }
                else if (textBox4.Text != "" & textBox3.Text == "")
                {
                    sum_sec_inp = Convert.ToInt32(textBox4.Text) * 60;
                }
                else
                {
                    sum_sec_inp = Convert.ToInt32(textBox4.Text) * 60 + Convert.ToInt32(textBox3.Text);
                }
                string sql = "select *,(cast(substring(cast(songlength as varchar), 1, 2) as int) * 3600" +
                    " + cast(substring(cast(songlength as varchar), 4, 2) as int) * 60" +
                    " +cast(substring(cast(songlength as varchar), 7, 2) as int)) as sumn" +
                    " into #t from song INNER JOIN person ON person_persid=person.persid" +
                    " INNER JOIN genre ON genre_genreid=genre.genreid order by RATING DESC;" +
                    "DELETE FROM #t WHERE sumn>" + sum_sec_inp +
                    ";select personname AS 'Исполнитель', songtitle AS 'Название', songlength AS 'Длина', sumn from #t where persid=" 
                    + personComboBox.SelectedValue.ToString() + 
                    ";drop table #t;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                    DataTable myTable = new DataTable();
                    adapter.Fill(myTable);
                    dataGridView2.DataSource = myTable;
                    dataGridView2.Refresh();
                    dataGridView2.Columns["sumn"].Visible = false;     
                    for (int i = 0; i < dataGridView2.Rows.Count; i++)
                    {
                        int length = Convert.ToInt32(dataGridView2.Rows[i].Cells[3].Value);
                        sum_sec += length;
                        cut = i;
                        if (sum_sec > sum_sec_inp)
                            break;
                    }
                    int len = dataGridView2.Rows.Count;
                    if (sum_sec > sum_sec_inp ^ (sum_sec_inp == 0))
                    {
                        for (int i = len - 1; i > cut - 1; i--)
                        {

                            dataGridView2.Rows.RemoveAt(i);

                        }

                    }
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Укажите корректные данные продолжительности плейлиста.");
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Выберите существующего исполнителя.");
            }
            catch (InvalidOperationException)
            {

            }
        }
    }
}
