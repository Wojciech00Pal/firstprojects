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
using System.Globalization;

namespace projekt_14._06
{
    public partial class workers : Form
    {
        SQL cmd = new SQL();
        
        private BindingSource bindingSource1 = new BindingSource();
        private SqlDataAdapter dataAdapter = new SqlDataAdapter();
        public workers()
        {
            InitializeComponent();
            
        }

        public void Get(string selectCommand)
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
                dataGridView1.AutoResizeColumns(
                    DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);
            }
            catch (SqlException)
            {
                MessageBox.Show("To run this example, replace the value of the " +
                    "connectionString variable with a connection string that is " +
                    "valid for your system.");
            }
        }

private void button1_Click(object sender, EventArgs e)//create
        {
            string na, su, hd, sex, tel, b_day;
            na = textBox1.Text;
            su = textBox2.Text;
            hd = dateTimePicker1.Text;

            if (radioButton1.Checked == true)
            {
                sex = "F";
            }
            else
            {
                sex = "M";
            }

            tel = textBox3.Text;
            b_day = dateTimePicker2.Text;

            cmd.Create(na, su, hd, sex, tel, b_day);


        }

        private void button2_Click(object sender, EventArgs e) //UPDATE
        {
            string sex;
            string hd = dateTimePicker1.Text;
            string b_day = dateTimePicker2.Text;


            if (textBox4.Text != "")
            {
                if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "")
                {
                    MessageBox.Show("Textboxes required are empty ", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                else
                {
                    if (radioButton1.Checked == true)
                    {
                        sex = "F";
                    }
                    else
                    {
                        sex = "M";
                    }

                    string id = textBox4.Text;
                    cmd.Update(textBox1.Text, textBox2.Text, hd, sex, textBox3.Text, b_day,id);

                }
            }
            else
            {
                MessageBox.Show("Textboxes required are empty ", "Info",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = bindingSource1;
            Get("select * from workers");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (radioButton3.Checked==true) ///surname
            {
                Get("select* from workers WHERE surname='" + textBox5.Text + "'");
            }
            else
            {
                Get("select* from workers WHERE name='" + textBox5.Text + "'");
            }
        }
    }


}
