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
using System.Configuration;
using System.Diagnostics;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata;

namespace DBCSVPROJEKT2
{
    public partial class Form1 : Form
    {      


        public Form1()
        {
            InitializeComponent();
        }

        string[] ordersdata;
        string[] orders_detailsdata;
        SqlCommand comm;
        SqlCommand comm2;
        SqlConnection con;
        

        private void button1_Click(object sender, EventArgs e)
        {
            ///orders
            ///
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.ShowDialog();
            int ImportedRecord = 0, inValidItem = 0;
            string SourceURl = "";
            label3.Text = dialog.FileName;


            DataTable dt = new DataTable();
            try
            {
                string[] lines = System.IO.File.ReadAllLines(dialog.FileName);
                button1.Text = "Wczytuje dane....";

                if (lines.Length > 0)
                {
                    // addByWholeDataSqlBulkCopy(lines);
                    //addByWholeDataSqlClient(lines);
                    ordersdata = lines;
                    ///for wszystkie linie ///
                    //TWORZENEI NAGLOWKA W TABELCE
                    string firstLine = lines[0];
                    string[] headerLabels = firstLine.Split(';');
                    foreach (string headerWord in headerLabels)
                    {
                        dt.Columns.Add(new DataColumn(headerWord));
                    }
                    ///dla WYPELNIENIA DANYCH
                    for (int i = 1; i < lines.Length; i++) //lines.Length
                    {

                        ////////// PO JEDNYM REKORDZIE //////////////////
                       // addByOneByRecordbySqlClient(lines[i]);

                        //addByOneByRecordbySqlBulkCopy(lines[i]);

                        string[] dataWords = lines[i].Split(';');
                        DataRow dr = dt.NewRow();
                        int columnIndex = 0;
                        foreach (string headerWord in headerLabels)
                        {
                            dr[headerWord] = dataWords[columnIndex++];


                        }
                        dt.Rows.Add(dr);
                    }
                }
                if (dt.Rows.Count > 0)
                {
                    dataGridView1.DataSource = dt;

                }
                //MessageBox.Show("Suma rekordu po rekordzie: " + sumka);
                //MessageBox.Show("Srednia rekordu: " + (float)(sumka / lines.Length));
            }
            catch (Exception)
            {
                MessageBox.Show("error, zla sciezka lub plik / niepoprawny plik");
            }





        }

        private void button2_Click(object sender, EventArgs e)
        {
            ///orders details

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.ShowDialog();
            int ImportedRecord = 0, inValidItem = 0;
            string SourceURl = "";
            label4.Text = dialog.FileName;


            DataTable dt = new DataTable();
            try
            {
                string[] lines = System.IO.File.ReadAllLines(dialog.FileName);
                button1.Text = "Wczytuje dane....";
                orders_detailsdata = lines;

                if (lines.Length > 0)
                {
                    // addByWholeDataSqlBulkCopy(lines);
                    //addByWholeDataSqlClient(lines);

                    ///for wszystkie linie ///
                    //TWORZENEI NAGLOWKA W TABELCE
                    string firstLine = lines[0];
                    string[] headerLabels = firstLine.Split(';');
                    foreach (string headerWord in headerLabels)
                    {
                        dt.Columns.Add(new DataColumn(headerWord));
                    }
                    ///dla WYPELNIENIA DANYCH
                    for (int i = 1; i < lines.Length; i++) //lines.Length
                    {

                        ////////// PO JEDNYM REKORDZIE //////////////////
                        // addByOneByRecordbySqlClient(lines[i]);

                        //addByOneByRecordbySqlBulkCopy(lines[i]);

                        string[] dataWords = lines[i].Split(';');
                        DataRow dr = dt.NewRow();
                        int columnIndex = 0;
                        foreach (string headerWord in headerLabels)
                        {
                            dr[headerWord] = dataWords[columnIndex++];
                           // Debug.WriteLine(dataWords[c]);



                        }
                        dt.Rows.Add(dr);
                    }
                }
                if (dt.Rows.Count > 0)
                {
                    dataGridView2.DataSource = dt;

                }
                //MessageBox.Show("Suma rekordu po rekordzie: " + sumka);
                //MessageBox.Show("Srednia rekordu: " + (float)(sumka / lines.Length));
            }
            catch (Exception)
            {
                MessageBox.Show("error, zla sciezka lub plik / niepoprawny plik");
            }





        }

        private void operationstwobtn_Click(object sender, EventArgs e)
        {
          
            ///////////jako dwie zupelnie rozne operacje
            using (SqlConnection connection = new SqlConnection("Server= localhost; Database = Fabryka; Integrated Security = SSPI"))
            {
                try
                {
                    connection.Open();
                    using (SqlTransaction trans = connection.BeginTransaction())
                    {
                        for (int i = 1; i < ordersdata.Length; i++)
                        {
                            string wierszyk = ordersdata[i];
                            string[] dataWords = wierszyk.Split(';');

                            string sql = "INSERT INTO Orders VALUES(@id_klienta, @username_klienta, @data_zamowienia)";
                            comm = new SqlCommand(sql, connection, trans);
                            comm.Parameters.AddWithValue("@id_klienta", dataWords[0]);
                            comm.Parameters.AddWithValue("@username_klienta", dataWords[1]);
                            comm.Parameters.AddWithValue("@data_zamowienia", dataWords[2]);
                            comm.ExecuteNonQuery();


                            Debug.WriteLine("Records Inserted Successfully");

                             
                        }


                        trans.Commit();
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("ERROR !");              
                }
            }

            using (SqlConnection connection = new SqlConnection("Server= localhost; Database = Fabryka; Integrated Security = SSPI"))
            {
                try
                {
                    connection.Open();
                    using (SqlTransaction trans = connection.BeginTransaction())
                    {
                        for (int i = 1; i < orders_detailsdata.Length; i++)
                        {
                            string wierszyk = orders_detailsdata[i];
                            string[] dataWords = wierszyk.Split(';');

                            string sql = "INSERT INTO Orders_details VALUES(@id_zamowienia, @cena, @waga, @znizka, @status)";
                            comm2 = new SqlCommand(sql, connection, trans);
                            comm2.Parameters.AddWithValue("@id_zamowienia", dataWords[0]);
                            comm2.Parameters.AddWithValue("@cena", dataWords[1]);
                            comm2.Parameters.AddWithValue("@waga", dataWords[2]);
                            comm2.Parameters.AddWithValue("@znizka", dataWords[3]);
                            comm2.Parameters.AddWithValue("@status", dataWords[4]);

                            comm2.ExecuteNonQuery();


                            Debug.WriteLine("Records Inserted Successfully");


                        }


                        trans.Commit();
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("ERROR !"+ex.ToString());
                }
            }





        }

        private void oneoperationbtn_Click(object sender, EventArgs e)
        {
            ///w obrebie jednej transakcji - dziala

            using (SqlConnection connection = new SqlConnection("Server= localhost; Database = Fabryka; Integrated Security = SSPI"))
            {
                try
                {
                    connection.Open();
                    using (SqlTransaction trans = connection.BeginTransaction())
                    {
                        for (int i = 1; i < ordersdata.Length; i++)
                        {
                            string wierszyk = ordersdata[i];
                            string[] dataWords = wierszyk.Split(';');

                            string sql = "INSERT INTO Orders VALUES(@id_klienta, @username_klienta, @data_zamowienia)";
                            comm = new SqlCommand(sql, connection, trans);
                            comm.Parameters.AddWithValue("@id_klienta", dataWords[0]);
                            comm.Parameters.AddWithValue("@username_klienta", dataWords[1]);
                            comm.Parameters.AddWithValue("@data_zamowienia", dataWords[2]);
                            comm.ExecuteNonQuery();


                            Debug.WriteLine("Records Inserted Successfully");


                        }



                        for (int i = 1; i < orders_detailsdata.Length; i++)
                        {
                            string wierszyk = orders_detailsdata[i];
                            string[] dataWords = wierszyk.Split(';');

                            string sql = "INSERT INTO Orders_details VALUES(@id_zamowienia, @cena, @waga, @znizka, @status)";
                            comm2 = new SqlCommand(sql, connection, trans);
                            comm2.Parameters.AddWithValue("@id_zamowienia", dataWords[0]);
                            comm2.Parameters.AddWithValue("@cena", dataWords[1]);
                            comm2.Parameters.AddWithValue("@waga", dataWords[2]);
                            comm2.Parameters.AddWithValue("@znizka", dataWords[3]);
                            comm2.Parameters.AddWithValue("@status", dataWords[4]);

                            comm2.ExecuteNonQuery();


                            Debug.WriteLine("Records Inserted Successfully");


                        }


                        trans.Commit();
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("ERROR !"+ex.ToString());
                }
            }







        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.ShowDialog();
            int ImportedRecord = 0, inValidItem = 0;
            string SourceURl = "";
            label4.Text = dialog.FileName;


            DataTable dt = new DataTable();
            try
            {
                string[] lines = System.IO.File.ReadAllLines(dialog.FileName);
                button4.Text = "Wczytuje dane....";

                if (lines.Length > 0)
                {
                    addByWholeDataSqlClient(lines);
                    //addByWholeDataSqlClient(lines);

                    ///for wszystkie linie ///
                    //TWORZENEI NAGLOWKA W TABELCE
                    string firstLine = lines[0];
                    string[] headerLabels = firstLine.Split(';');
                    foreach (string headerWord in headerLabels)
                    {
                        dt.Columns.Add(new DataColumn(headerWord));
                    }
                    ///dla WYPELNIENIA DANYCH
                    for (int i = 1; i < lines.Length; i++) //lines.Length
                    {

                        ////////// PO JEDNYM REKORDZIE //////////////////
                        // addByOneByRecordbySqlClient(lines[i]);

                        //addByOneByRecordbySqlBulkCopy(lines[i]);

                        string[] dataWords = lines[i].Split(';');
                        DataRow dr = dt.NewRow();
                        int columnIndex = 0;
                        foreach (string headerWord in headerLabels)
                        {
                            dr[headerWord] = dataWords[columnIndex++];
                            // Debug.WriteLine(dataWords[c]);



                        }
                        dt.Rows.Add(dr);
                    }
                }
                if (dt.Rows.Count > 0)
                {
                    dataGridView3.DataSource = dt;

                }

                ///ZAPIS DO BAZY //////////////////////////////////////////////////////////////////////////////
                ///
                //MessageBox.Show("Suma rekordu po rekordzie: " + sumka);
                //MessageBox.Show("Srednia rekordu: " + (float)(sumka / lines.Length));
            }
            catch (Exception)
            {
                MessageBox.Show("error, zla sciezka lub plik / niepoprawny plik");
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {

            string sql = "SELECT * FROM Dostawcy";

            using (var connection = new SqlConnection("Server= localhost; Database= Fabryka; Integrated Security = SSPI"))
            using (var command = new SqlCommand(sql, connection))
            using (var adapter = new SqlDataAdapter(command))
            {
                connection.Open();
                var myTable = new DataTable();
                adapter.Fill(myTable);
                dataGridView4.DataSource = myTable;
            }

            SqlConnection conn2 = new SqlConnection("Server= localhost; Database= Fabryka; Integrated Security = SSPI");
            using (conn2)
            {
                SqlCommand command = new SqlCommand(
                  "SELECT nazwa, ilosc_pracownikow, data_dodania FROM Dostawcy",
                  conn2);
                conn2.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string var1 = (string)reader["nazwa"];
                        int? var2 = null;
                        if (!reader.IsDBNull(reader.GetOrdinal("ilosc_pracownikow")))
                        {
                            var2 = (int)reader["ilosc_pracownikow"];
                        }
                        DateTime var3 = ((DateTime)reader["data_dodania"]);
                        Debug.WriteLine(var1 + ", ", var2 + ", " + var3);
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }
                reader.Close();
            }

            ///oraz w debugu jak to wyglada:
          

        }


        private void addByWholeDataSqlClient(string[] wholedata)  ///dziala
        {

            Stopwatch stopwatch = new Stopwatch();


            string sqlconn = "Server= localhost; Database= Fabryka; Integrated Security = SSPI";
            // sqlconn = ConfigurationManager.ConnectionStrings["SqlCom"].ConnectionString;
            stopwatch.Start();

            try
            {
                con = new SqlConnection(sqlconn);
                // string[] dataWords = one_record_in_csv.Split(';');
                con.Open();

                for (int i = 1; i < wholedata.Length; i++)
                {
                    string wierszyk = wholedata[i];
                    string[] dataWords = wierszyk.Split(';');

                    string sql = "INSERT INTO Dostawcy VALUES(@nazwa, @ilosc_pracownikow, @data_dodania)";
                    comm = new SqlCommand(sql, con);
                    comm.Parameters.AddWithValue("@nazwa", dataWords[0]);
                    if (dataWords[1] == null || dataWords[1] == "")
                    {
                        comm.Parameters.AddWithValue("@ilosc_pracownikow", DBNull.Value);
                    }
                    else
                    {
                        comm.Parameters.AddWithValue("@ilosc_pracownikow", dataWords[1]);

                    }
                    comm.Parameters.AddWithValue("@data_dodania", dataWords[2]);
                    comm.ExecuteNonQuery();
                    Console.WriteLine("Records Inserted Successfully");

                    //  comm.Dispose();
                }

            }
            catch (SqlException ex)

            {
                MessageBox.Show("Can not  ! " + ex.ToString());
            }
            finally
            {
                //MessageBox.Show("koniec polaczenia");

                con.Close();

            }
        }

    }
}
