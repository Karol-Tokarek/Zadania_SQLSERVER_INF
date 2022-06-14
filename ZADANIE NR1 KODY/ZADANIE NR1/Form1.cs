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



namespace DBCSV_PROJECT
{
    public partial class Form1 : Form 
    {


        SqlConnection con;
        SqlCommand comm;
        SqlDataReader reader;
        //public long sum;
        //public long srednia;
        double sumka;

        public Form1()
        {
            InitializeComponent();
        }
        private void connection()
        {
           
            
        }
        private void button1_Click(object sender, EventArgs e)
        {
            sumka = 0;
           
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.ShowDialog();
                int ImportedRecord = 0, inValidItem = 0;
                string SourceURl = "";
                txtFile.Text = dialog.FileName;


                DataTable dt = new DataTable();
            try
            {
                string[] lines = System.IO.File.ReadAllLines(dialog.FileName);
                button1.Text = "Wczytuje dane....";

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
                          addByOneByRecordbySqlClient(lines[i]);

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
                MessageBox.Show("Suma rekordu po rekordzie: " + sumka);
                MessageBox.Show("Srednia rekordu: " + (float)(sumka / lines.Length));
            }
            catch (Exception)
            {
                MessageBox.Show("error, zla sciezka lub plik / niepoprawny plik");
            }
            //addByWholeDataSqlClient(lines);
            
           


        }

        private void addByOneByRecordbySqlBulkCopy(string one_record_in_csv)
        {
            Stopwatch stopwatch = new Stopwatch();
            string[] dataWords = one_record_in_csv.Split(';');
            stopwatch.Start();
            try
            {

                using (SqlConnection dbConnection = new SqlConnection("Server= localhost; Database = Kody; Integrated Security = SSPI"))
                {

                    dbConnection.Open();

                    //MessageBox.Show(dbConnection.State.ToString());
                    using (SqlBulkCopy s = new SqlBulkCopy(dbConnection))
                    {
                        s.BatchSize = 10000;

                        DataTable tbl = new DataTable();
                        tbl.Columns.Add(new DataColumn("Kod_Pocztowy", typeof(string)));
                        tbl.Columns.Add(new DataColumn("Adres", typeof(string)));
                        tbl.Columns.Add(new DataColumn("Miejscowosc", typeof(string)));
                        tbl.Columns.Add(new DataColumn("Wojewodztwo", typeof(string)));
                        tbl.Columns.Add(new DataColumn("Powiat", typeof(string)));
                        DataRow dr = tbl.NewRow();
                        dr["Kod_Pocztowy"] = dataWords[0];
                        dr["Adres"] = dataWords[1];
                        dr["Miejscowosc"] = dataWords[2];
                        dr["Wojewodztwo"] = dataWords[3];
                        dr["Powiat"] = dataWords[4];
                        tbl.Rows.Add(dr);

                        s.DestinationTableName = "dbo.Kody_Pocztowe";
                        s.ColumnMappings.Add("Kod_Pocztowy", "Kod_Pocztowy");
                        s.ColumnMappings.Add("Adres", "Adres");
                        s.ColumnMappings.Add("Miejscowosc", "Miejscowosc");
                        s.ColumnMappings.Add("Wojewodztwo", "Wojewodztwo");
                        s.ColumnMappings.Add("Powiat", "Powiat");
                        s.WriteToServer(tbl);
                        s.Close();

                    }
                    dbConnection.Close();
                    stopwatch.Stop();
                    sumka += stopwatch.Elapsed.TotalMilliseconds;

                }
            }
            catch(SqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        private void addByWholeDataSqlBulkCopy(string[] wholedata)
        {

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            try {
                using (SqlConnection dbConnection = new SqlConnection("Server= localhost; Database = Kody; Integrated Security = SSPI"))
                {

                    dbConnection.Open();

                    //MessageBox.Show(dbConnection.State.ToString());
                    using (SqlBulkCopy s = new SqlBulkCopy(dbConnection))
                    {

                        DataTable tbl = new DataTable();
                        tbl.Columns.Add(new DataColumn("Kod_Pocztowy", typeof(string)));
                        tbl.Columns.Add(new DataColumn("Adres", typeof(string)));
                        tbl.Columns.Add(new DataColumn("Miejscowosc", typeof(string)));
                        tbl.Columns.Add(new DataColumn("Wojewodztwo", typeof(string)));
                        tbl.Columns.Add(new DataColumn("Powiat", typeof(string)));
                        for (int i = 1; i < wholedata.Length; i++)
                        {
                            string wierszyk = wholedata[i];
                            string[] dataWords = wierszyk.Split(';');

                            DataRow dr = tbl.NewRow();
                            dr["Kod_Pocztowy"] = dataWords[0];
                            dr["Adres"] = dataWords[1];
                            dr["Miejscowosc"] = dataWords[2];
                            dr["Wojewodztwo"] = dataWords[3];
                            dr["Powiat"] = dataWords[4];
                            tbl.Rows.Add(dr);


                        }
                        s.BatchSize = 10000;

                        s.DestinationTableName = "dbo.Kody_Pocztowe";
                        s.ColumnMappings.Add("Kod_Pocztowy", "Kod_Pocztowy");
                        s.ColumnMappings.Add("Adres", "Adres");
                        s.ColumnMappings.Add("Miejscowosc", "Miejscowosc");
                        s.ColumnMappings.Add("Wojewodztwo", "Wojewodztwo");
                        s.ColumnMappings.Add("Powiat", "Powiat");
                        s.WriteToServer(tbl);
                        s.Close();

                    }
                    dbConnection.Close();
                    stopwatch.Stop();
                    MessageBox.Show("TIME TO END; " + stopwatch.ElapsedMilliseconds.ToString());
                }
                
                


            }
            catch(SqlException ex)
            {
                MessageBox.Show("ERROR !" + ex.ToString());
            }
        }


        private void addByOneByRecordbySqlClient(string one_record_in_csv)  ///dziala
        {

            Stopwatch stopwatch = new Stopwatch();
               
        //  MessageBox.Show("OD NOWA !!1");
        //  for(int i=0; i<dataWords.Length; i++)
        //  {
        //     MessageBox.Show(dataWords[i]);
        //  }

        string sqlconn = "Server= localhost; Database = Kody; Integrated Security = SSPI";
            stopwatch.Start();

            // sqlconn = ConfigurationManager.ConnectionStrings["SqlCom"].ConnectionString;
       
            //MessageBox.Show("DO BAZY:" +one_record_in_csv); //insert into zapytanbie
           
            try
            {
                con = new SqlConnection(sqlconn);
                string[] dataWords = one_record_in_csv.Split(';');
                string sql = "INSERT INTO Kody_Pocztowe VALUES(@id, @username, @password, @email, @tel)";

                con.Open();
                comm = new SqlCommand(sql, con);
                comm.Parameters.AddWithValue("@id", dataWords[0]);
                comm.Parameters.AddWithValue("@username", dataWords[1]);
                comm.Parameters.AddWithValue("@password", dataWords[2]);
                comm.Parameters.AddWithValue("@email", dataWords[3]);
                comm.Parameters.AddWithValue("@tel", dataWords[4]);

                comm.ExecuteNonQuery();
                Console.WriteLine("Records Inserted Successfully");
                //  reader = comm.ExecuteReader();

                //  while (reader.Read())
                //  {
                // MessageBox.Show(reader.GetValue(0) + " - " + reader.GetValue(1) + " - " + reader.GetValue(2));
                // }
                //reader.Close();
               // comm.Dispose();
                
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Can not  ! "+ex.ToString());
            }
            finally
            {
                //MessageBox.Show("koniec polaczenia");
                con.Close();
                stopwatch.Stop();
                sumka += stopwatch.Elapsed.TotalMilliseconds;
                //Debug.WriteLine(sumka);
                //MessageBox.Show("SUMA: " + stopwatch.Elapsed.TotalMilliseconds);
               // MessageBox.Show("TIME TO END; " + stopwatch.ElapsedMilliseconds.ToString());

            }


        }

        private void addByWholeDataSqlClient(string[] wholedata)  ///dziala
        {

            Stopwatch stopwatch = new Stopwatch();


            string sqlconn = "Server= localhost; Database= Kody; Integrated Security = SSPI";
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

                    string sql = "INSERT INTO Kody_Pocztowe VALUES(@id, @username, @password, @email, @tel)";
                    comm = new SqlCommand(sql, con);
                    comm.Parameters.AddWithValue("@id", dataWords[0]);
                    comm.Parameters.AddWithValue("@username", dataWords[1]);
                    comm.Parameters.AddWithValue("@password", dataWords[2]);
                    comm.Parameters.AddWithValue("@email", dataWords[3]);
                    comm.Parameters.AddWithValue("@tel", dataWords[4]);
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
                stopwatch.Stop();
                MessageBox.Show("TIME TO END; " + stopwatch.ElapsedMilliseconds.ToString());
            }
        }





        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
