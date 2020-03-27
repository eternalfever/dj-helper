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
    public partial class Form5 : Form
    {
        string connectionString = @"Data Source=DESKTOP-JKUAUE3\SQLEXPRESS;Initial Catalog=Fonoteka_db;Integrated Security=True";
        public Form5()
        {
            InitializeComponent();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            string sql = "SELECT personname AS 'Артист', songtitle AS 'Название', songlength AS 'Длина',"+
                " Comment AS 'Комментарий' FROM song INNER JOIN person ON song.person_persid = person.persid"+
                " ORDER BY rating DESC";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                // Создаем объект DataAdapter
                SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                // Создаем объект Dataset
                DataTable myTable = new DataTable();
                // Заполняем Dataset
                adapter.Fill(myTable);
                // Отображаем данные
                dataGridView1.DataSource = myTable;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string sql;
            if (textBox1.Text == "")
            {
                sql = "SELECT personname AS 'Артист', songtitle AS 'Название', songlength AS 'Длина',"+
                    " Comment AS 'Комментарий' FROM song INNER JOIN person ON song.person_persid = person.persid"+
                    " ORDER BY rating DESC";
            }
            else
            {
                sql = "SELECT personname AS 'Артист', songtitle AS 'Название', songlength AS 'Длина',"+
                    " Comment AS 'Комментарий' FROM song INNER JOIN person ON song.person_persid = person.persid"+
                    " where Comment LIKE '%" + textBox1.Text + "%'" + 
                    " ORDER BY rating DESC";
            }
            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                DataTable myTable = new DataTable();
                adapter.Fill(myTable);
                dataGridView1.DataSource = myTable;
            }
        }
    }
}
