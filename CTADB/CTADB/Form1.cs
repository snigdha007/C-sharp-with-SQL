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


namespace CTADB
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



        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            string filename, version, connectionInfo;
            string msg;
            SqlConnection db; version = "MSSQLLocalDB";
            filename = "CTA.mdf";
            connectionInfo = string.Format(@"Data Source=(LocalDB)\{0};AttachDbFilename=|DataDirectory|\{1};Integrated Security=True;", version, filename);
            db = new SqlConnection(connectionInfo);
            db.Open();
            string sql = string.Format(@"SELECT Name,StationID FROM stations;", this.listBox1.Text);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = db;
            
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            cmd.CommandText = sql;
            adapter.Fill(ds);
            db.Close();
            foreach (DataRow row in ds.Tables["TABLE"].Rows)
            {
                msg = string.Format("{0}:{1}", Convert.ToString(row["StationID"]),
                Convert.ToString(row["Name"]));
                this.listBox1.Items.Add(msg);

            }


        }

        private void listBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            listBox2.Items.Clear();
            listBox3.Items.Clear();
            listBox4.Items.Clear();
            listBox5.Items.Clear();
            listBox6.Items.Clear();
            string filename, version, connectionInfo;
            string selected = this.listBox1.SelectedItem.ToString();
            int delim = selected.IndexOf(':');
            int stationid=Convert.ToInt32(selected.Substring(0,delim));//gets the station id of the selected item in listbox1
            SqlConnection db; version = "MSSQLLocalDB";
            filename = "CTA.mdf";
            connectionInfo = string.Format(@"Data Source=(LocalDB)\{0};AttachDbFilename=|DataDirectory|\{1};Integrated Security=True;", version, filename);
            db = new SqlConnection(connectionInfo);
            db.Open();

            string sql, msg; 
            SqlCommand cmd;
            object result;
           
           sql = string.Format(@"SELECT sum(Convert(float,DailyTotal)) FROM Riderships WHERE StationID={0};",stationid);
           // sql = string.Format(@"SELECT AVG(Convert(float,DailyTotal)) FROM Riderships WHERE StationID={0};", stationid);
            cmd = new SqlCommand();
            cmd.Connection = db;
            cmd.CommandText = sql;
            result = cmd.ExecuteScalar();
            msg = String.Format("{0}", result);
            listBox2.Items.Add(msg);
           
            /////finding thr average
            string sql2,msg2;
            SqlCommand cmd2;object result2;
            sql2 = string.Format(@"SELECT AVG (DailyTotal) FROM Riderships WHERE StationID={0};", stationid);
            cmd2 = new SqlCommand();
            cmd2.Connection = db;
            cmd2.CommandText = sql2;
            result2 = cmd2.ExecuteScalar();
            msg2 = String.Format("{0}", result2);
            listBox3.Items.Add(msg2);
            /// finding the percentage
            string sql3, msg3;
            SqlCommand cmd3; object result3;
            sql3 = string.Format(@"
                 Declare @total as float;
                 Declare @count as float;
                    Set @total = (SELECT sum (Convert(float,DailyTotal))  FROM Riderships);
                        Set @count = (SELECT sum(DailyTotal) FROM Riderships WHERE StationID={0});
                        Select Round(@count / @total * 100.0,2);                
", stationid);
            cmd3 = new SqlCommand();
            cmd3.Connection = db;
            cmd3.CommandText = sql3;
            result3 = cmd3.ExecuteScalar();
            msg3 = String.Format("{0}", result3);
            listBox4.Items.Add(msg3);
            //// finding weekday
            string sql4, msg4;
            SqlCommand cmd4; object result4;
            sql4 = string.Format(@"SELECT   sum( Convert(float,DailyTotal)) FROM Riderships WHERE StationID={0} and TypeOfDay='W';", stationid);
            cmd4 = new SqlCommand();
            cmd4.Connection = db;
            cmd4.CommandText = sql4;
            result4 = cmd4.ExecuteScalar();
            msg4 = String.Format("{0}", result4);
            listBox6.Items.Add(msg4);


            ////finding the Saturaday 
            string sql5, msg5;
            SqlCommand cmd5; object result5;
            sql5 = string.Format(@"SELECT   sum( Convert(float,DailyTotal)) FROM Riderships WHERE StationID={0} and TypeOfDay='A';", stationid);
            cmd5 = new SqlCommand();
            cmd5.Connection = db;
            cmd5.CommandText = sql5;
            result5 = cmd5.ExecuteScalar();
            msg5 = String.Format("{0}", result5);
            listBox7.Items.Add(msg5);


            ////finding the Sunday/Holiday
            string sql6, msg6;
            SqlCommand cmd6; object result6;
            sql6 = string.Format(@"SELECT   sum( Convert(float,DailyTotal)) FROM Riderships WHERE StationID={0} and TypeOfDay='U';", stationid);
            cmd6 = new SqlCommand();
            cmd6.Connection = db;
            cmd6.CommandText = sql6;
            result6 = cmd6.ExecuteScalar();
            msg6= String.Format("{0}", result6);
            listBox8.Items.Add(msg6);

            ///// finding the stops
            string sql1;
            SqlCommand cmd1;
            sql1 = string.Format(@"SELECT StopID,Name FROM Stops where StationID={0};", stationid);
            //cmd1 = new SqlCommand();
            //cmd1.Connection = db;
            //cmd1.CommandText = sql1;
            //result1 = cmd1.ExecuteScalar();
            //msg1 = String.Format("{0}", result1);
            //listBox5.Items.Add(msg1);
            cmd1 = new SqlCommand();
            cmd1.Connection = db;

            SqlDataAdapter adapter = new SqlDataAdapter(cmd1);
            DataSet ds = new DataSet();
            cmd1.CommandText = sql1;
            adapter.Fill(ds);
            db.Close();
            foreach (DataRow row in ds.Tables["TABLE"].Rows)
            {
                msg = string.Format("{1}:                                                           {0}", Convert.ToString(row["StopID"]),
                Convert.ToString(row["Name"]));
                this.listBox5.Items.Add(msg);

            }


        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void top10ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            string filename, version, connectionInfo;
            string msg;
            SqlConnection db; version = "MSSQLLocalDB";
            filename = "CTA.mdf";
            connectionInfo = string.Format(@"Data Source=(LocalDB)\{0};AttachDbFilename=|DataDirectory|\{1};Integrated Security=True;", version, filename);
            db = new SqlConnection(connectionInfo);
            db.Open();
            string sql = string.Format(@"SELECT TOP 10 StationID,Name FROM stations;", this.listBox1.Text);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = db;

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            cmd.CommandText = sql;
            adapter.Fill(ds);
            db.Close();
            foreach (DataRow row in ds.Tables["TABLE"].Rows)
            {
                msg = string.Format("{0}                                 {1}", Convert.ToString(row["Name"]),
                Convert.ToString(row["StationID"]));
                this.listBox1.Items.Add(msg);

            }
        }

        private void listBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox9.Items.Clear();
            listBox10.Items.Clear();
            listBox11.Items.Clear();
            listBox12.Items.Clear();
            string filename, version, connectionInfo;
            string selected = this.listBox1.SelectedItem.ToString();
            int delim = selected.IndexOf(':');
            int stationid = Convert.ToInt32(selected.Substring(0, delim));//gets the station id of the selected item in listbox1
            SqlConnection db; version = "MSSQLLocalDB";
            filename = "CTA.mdf";
            connectionInfo = string.Format(@"Data Source=(LocalDB)\{0};AttachDbFilename=|DataDirectory|\{1};Integrated Security=True;", version, filename);
            db = new SqlConnection(connectionInfo);
            db.Open();
            ////handicap access
            string sql5, msg5;
            SqlCommand cmd5; object result5;
            sql5 = string.Format(@"SELECT CASE WHEN ADA = 'True' THEN 'Yes' ELSE 'No' END  FROM Stops WHERE StationID={0} ;", stationid);
            cmd5 = new SqlCommand();
            cmd5.Connection = db;
            cmd5.CommandText = sql5;
            result5 = cmd5.ExecuteScalar();
            msg5 = String.Format("{0}", result5);
            listBox9.Items.Add(msg5);
            ///////direction of Travel:
            string sql6, msg6;
            SqlCommand cmd6; object result6;
            sql6 = string.Format(@"SELECT Direction  FROM Stops WHERE StationID={0} ;", stationid);
            cmd6 = new SqlCommand();
            cmd6.Connection = db;
            cmd6.CommandText = sql6;
            result6 = cmd6.ExecuteScalar();
            msg6 = String.Format("{0}", result6);
            listBox10.Items.Add(msg6);
            /////Location:
            string sql7,sql8, msg7,msg8;
            SqlCommand cmd7,cmd8; object result7,result8;
            sql7 = string.Format(@"SELECT Latitude,Longitude  FROM Stops WHERE StationID={0} ;", stationid);
            sql8 = string.Format(@"SELECT Longitude  FROM Stops WHERE StationID={0} ;", stationid);
            cmd7 = new SqlCommand();
            cmd7.Connection = db;
            cmd8 = new SqlCommand();
            cmd8.Connection = db;
            cmd7.CommandText = sql7;
            cmd8.CommandText = sql8;
            result7 = cmd7.ExecuteScalar();
            result8 = cmd8.ExecuteScalar();
            msg7 = String.Format("({0},", result7);
            msg8 = String.Format("{0})", result8);
            listBox11.Items.Add(msg7);
            listBox11.Items.Add(msg8);
            ///////Lines:
            //string sql9, msg9;
            //SqlCommand cmd9; object result9;
            //sql9 = string.Format(@"SELECT Color FROM Stop WHERE StationID={0} ;", stationid);
            //cmd9 = new SqlCommand();
            //cmd9.Connection = db;
            //cmd9.CommandText = sql6;
            //result9 = cmd9.ExecuteScalar();
            //msg9 = String.Format("{0}", result9);
            //listBox12.Items.Add(msg9);
            string msg;
            string sql = @"SELECT Color FROM Lines;";
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = db;

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            cmd.CommandText = sql;
            adapter.Fill(ds);
            db.Close();
            foreach (DataRow row in ds.Tables["TABLE"].Rows)
            {
                msg = string.Format("{0}", Convert.ToString(row["Color"]));
                
                this.listBox12.Items.Add(msg);

            }
        

    }
    }
}
