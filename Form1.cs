using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace HW_07._10._23
{
    public partial class Form1 : Form
    {
        private ComboBox comboBoxTables;
        private DataGridView date;
        private SqlConnection connection;
        private SqlDataAdapter adapter;
        private DataSet dataSet;
        public Form1()
        {
            InitializeComponent();

            connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Sales;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            adapter = new SqlDataAdapter();
            dataSet = new DataSet();

            comboBoxTables = new ComboBox();
            comboBoxTables.Location = new System.Drawing.Point(10, 10);
            comboBoxTables.Width = 200;
            comboBoxTables.SelectedIndexChanged += comboBoxTables_SelectedIndexChanged;
            this.Controls.Add(comboBoxTables);

            date=new DataGridView();
            date.Location = new System.Drawing.Point(10, 40);
            date.Width = 550;
            date.Height = 300;
            this.Controls.Add(date);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadTableName();
        }

        private void LoadTableName() 
        { 
            try
            {
                connection.Open();
                DataTable tables = connection.GetSchema("Tables");
                foreach(DataRow table in tables.Rows)
                {
                    string tableName = table["TABLE_NAME"].ToString();
                    comboBoxTables.Items.Add(tableName);
                }
            }
            catch (Exception ex) { MessageBox.Show("\n\tError loading table names"+ex.Message); }
            finally { connection.Close(); }
        }

        private void comboBoxTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedTableName=comboBoxTables.SelectedItem.ToString();
            string query = $"SELECT * FROM {selectedTableName}";
            adapter.SelectCommand=new SqlCommand(query, connection);
            dataSet.Clear();
            adapter.Fill(dataSet);
            date.DataSource = dataSet.Tables[0];
        }
    }
}
