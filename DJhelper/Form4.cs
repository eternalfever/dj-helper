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
    public partial class Form4 : Form
    {
        public int id;
        string connectionString = @"Data Source=DESKTOP-JKUAUE3\SQLEXPRESS;Initial Catalog=Fonoteka_db;Integrated Security=True";
        public Form4()
        {
            InitializeComponent();
        }
        private void Form4_Load(object sender, EventArgs e)
        {
            string sql = "SELECT Comment FROM song INNER JOIN person ON person_persid=person.persid WHERE songid=" + id;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sql, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    try
                    {
                        textBox1.Text += reader.GetString(0);
                    }
                    catch
                    {

                    }

                }
                reader.Close();
                connection.Close();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            
            string sql = "UPDATE song SET Comment = '"+textBox1.Text+"' where songid="+id;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sql, connection);
                connection.Open();
                command.ExecuteNonQuery();
                MessageBox.Show("Изменения успешно внесены!");

            }
        }

        }
    }

