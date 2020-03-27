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
    public partial class Form1 : Form
    {
        string connectionString = @"Data Source=DESKTOP-JKUAUE3\SQLEXPRESS;Initial Catalog=Fonoteka_db;Integrated Security=True";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.personTableAdapter.Fill(this.fonoteka_dbDataSet.person);
            personComboBox.SelectedIndex = -1;
            string sql = "SELECT personname AS 'Артист', songtitle AS 'Название', songlength AS 'Длина',"+
                " discid AS 'Номер носителя', typeof AS 'Тип носителя', songid FROM song"+
                " INNER JOIN person ON song.person_persid = person.persid INNER JOIN"+
                " songdisc ON song.songid=songdisc.song_songid INNER JOIN discinfo ON"+
                " discinfo_discid=discinfo.discid ORDER BY rating DESC";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                DataTable myTable = new DataTable();
                adapter.Fill(myTable);
                dataGridView1.DataSource = myTable;
                dataGridView1.Columns["songid"].Visible = false;
            }
        }

        private void personComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (personComboBox.SelectedIndex != -1)
            {
                string pers = personComboBox.SelectedValue.ToString();
                string sql = "SELECT personname AS 'Артист', songtitle AS 'Название', songlength AS 'Длина',"+
                    " discid AS 'Номер носителя', typeof AS 'Тип носителя', songid FROM song"+
                    " INNER JOIN person ON song.person_persid = person.persid INNER JOIN"+
                    " songdisc ON song.songid=songdisc.song_songid INNER JOIN discinfo ON"+
                    " discinfo_discid=discinfo.discid where persid=" + pers + 
                    " ORDER BY rating DESC";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                    DataTable myTable = new DataTable();
                    adapter.Fill(myTable);
                    dataGridView1.DataSource = myTable;
                    dataGridView1.Columns["songid"].Visible = false;
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string sql;
            try
            {
                string pers = personComboBox.SelectedValue.ToString();
                if (pers != "")
                {
                    sql = "SELECT  personname AS 'Артист', songtitle AS 'Название', songlength AS 'Длина'," +
                     " discid AS 'Номер носителя', typeof AS 'Тип носителя',  songid FROM song" +
                     " INNER JOIN person ON song.person_persid = person.persid INNER JOIN" +
                     " songdisc ON song.songid=songdisc.song_songid INNER JOIN discinfo ON" +
                     " discinfo_discid=discinfo.discid where (songtitle LIKE '%" + textBox1.Text + "%' and persid=" +
                     pers + ") ORDER BY rating DESC";
                }
                else
                {
                    sql = "SELECT  personname AS 'Артист', songtitle AS 'Название', songlength AS 'Длина'," +
                    " discid AS 'Номер носителя', typeof AS 'Тип носителя',  songid FROM song" +
                    " INNER JOIN person ON song.person_persid = person.persid INNER JOIN" +
                    " songdisc ON song.songid=songdisc.song_songid INNER JOIN discinfo ON" +
                    " discinfo_discid=discinfo.discid where songtitle LIKE '%" + textBox1.Text + "%'  ORDER BY rating DESC";
                }


            }
            catch (NullReferenceException)
            {
                sql = "SELECT  personname AS 'Артист', songtitle AS 'Название', songlength AS 'Длина'," +
                    " discid AS 'Номер носителя', typeof AS 'Тип носителя',  songid FROM song" +
                    " INNER JOIN person ON song.person_persid = person.persid INNER JOIN" +
                    " songdisc ON song.songid=songdisc.song_songid INNER JOIN discinfo ON" +
                    " discinfo_discid=discinfo.discid where songtitle LIKE '%" + textBox1.Text + "%'  ORDER BY rating DESC";
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                DataTable myTable = new DataTable();
                adapter.Fill(myTable);
                dataGridView1.DataSource = myTable;
                dataGridView1.Columns["songid"].Visible = false;
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                Form4 myForm = new Form4();
                myForm.textBox2.Text = (dataGridView1[1, dataGridView1.CurrentRow.Index]).Value.ToString();
                myForm.textBox3.Text = (dataGridView1[0, dataGridView1.CurrentRow.Index]).Value.ToString();
                myForm.id = Convert.ToInt32((dataGridView1[5, dataGridView1.CurrentRow.Index]).Value);
                myForm.Show();
            }
            catch(InvalidCastException)
            {
                MessageBox.Show("Пустой объект.");
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        { 
            textBox2.Text = "";
            if ((dataGridView1[5, dataGridView1.CurrentRow.Index]).Value.ToString() != "")
            {
                string sql = "SELECT count(*) FROM songdisc WHERE song_songid=" + (dataGridView1[5, dataGridView1.CurrentRow.Index]).Value.ToString();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(sql, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        try
                        {
                            textBox2.Text += reader.GetInt32(0).ToString();
                        }
                        catch
                        {

                        }

                    }
                    reader.Close();
                    connection.Close();
                }
            }           
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form5 myForm = new Form5();
            myForm.Show();
        }

    }
 }

