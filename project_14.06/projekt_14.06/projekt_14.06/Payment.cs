using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Data.SqlClient;
using System.Globalization;
using System.ComponentModel.DataAnnotations;

namespace projekt_14._06
{
    public partial class Payment : Form
    {

        SQL cmd = new SQL();
        public float amount;

        private BindingSource bindingSource1 = new BindingSource();
        private SqlDataAdapter dataAdapter = new SqlDataAdapter();
        public Payment()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            float hourly_rate = float.Parse(textBox1.Text);
            float hour = float.Parse(textBox2.Text);

            string cur = comboBox1.Text.ToUpper();
            if (cur != "PLN" && cur != "")
            {
                string url = "http://api.nbp.pl/api/exchangerates/rates/a/" + cur + "/?format=json";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                string content = new StreamReader(response.GetResponseStream()).ReadToEnd();
                dynamic data = JObject.Parse(content);
                float MID = data.rates[0].mid;

                string month = data.rates[0].effectiveDate;
                amount = hour * hourly_rate * MID;

                richTextBox1.Text = "Result: " + (hour * hourly_rate* MID).ToString() +" for: " + hour.ToString() +", with course for: " + cur + " costs: " + MID.ToString();
           }
            else
            {
                richTextBox1.Text = "Result: " + (hour * hourly_rate).ToString() +
                                  " for: " + hour.ToString();
                amount = hour * hourly_rate;
            }
        }

        public void get(string selectCommand)
        {
            try
            {
                // Specify a connection string.
                // Replace <SQL Server> with the SQL Server for your Northwind sample database.
                // Replace "Integrated Security=True" with user login information if necessary.
                String connectionString = "Data Source = (localdb)\\MSSQLLocalDB;" +
                    "Initial Catalog = users; Integrated Security = True";

                // Create a new data adapter based on the specified query.
                dataAdapter = new SqlDataAdapter(selectCommand, connectionString);

                // Create a command builder to generate SQL update, insert, and
                // delete commands based on selectCommand.
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);

                // Populate a new data table and bind it to the BindingSource.
                DataTable table = new DataTable
                {
                    Locale = CultureInfo.InvariantCulture
                };
                dataAdapter.Fill(table);
                bindingSource1.DataSource = table;

                // Resize the DataGridView columns to fit the newly loaded content.
                // dataGridView1.AutoResizeColumns(
                //   DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);
                MessageBox.Show("Payment added", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (SqlException)
            {
                MessageBox.Show("To run this example, replace the value of the " +
                    "connectionString variable with a connection string that is " +
                    "valid for your system. - Wrong ID");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string date = DateTime.Today.ToString();
            

           
            string sql = "INSERT INTO payment (Id,month,amount_in_PLN) VALUES('"+textBox3.Text+"','"+date.ToString()+"','"+amount+"')";
            get(sql);
        }

    }
}
