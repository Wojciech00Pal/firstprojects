using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Data;
using System.Windows.Forms;

namespace projekt_14._06
{
    class SQL
    {
        private SqlConnection con = new SqlConnection("Data Source = (localdb)\\MSSQLLocalDB;" +
            "Initial Catalog = users; Integrated Security = True");
      

        public void delete(string log, string haslo)
        {
            con.Open();
            string sql = "delete from names WHERE login='" + log + "' and password='" + haslo + "'";
            SqlCommand c = new SqlCommand(sql, con);
            int i = c.ExecuteNonQuery();

            if (i != 0)
            {

                Form2.logged = true;
                con.Close();
                add(log, haslo); // na około
                //update_date(log);
            }
            else
            {
                Form2.logged = false;
                MessageBox.Show("You are not in our base");
                con.Close();
            }
            //con.Close();
        }
        public void Create(string na,string su,string hd,string sex,string tel,string b_day)
        {
            con.Open();
            string sql = "Insert into workers VALUES ('" + na + "','" + su + "','" + hd + "','" + sex +
                "','" + tel + "','" + b_day + "')";
            SqlCommand c = new SqlCommand(sql, con);
            int i = c.ExecuteNonQuery();
            if (i != 0)
            {
                MessageBox.Show("Created");
                con.Close();
                //add(log, haslo); // na około
            }
            con.Close();
        }


        public void add(string log, string haslo)
        {
            string data = DateTime.Now.ToString("s");
            con.Open();
            string sql = "Insert into names VALUES ('" + log + "','" + haslo + "','" + data + "');";
            SqlCommand c = new SqlCommand(sql, con);

            int i = c.ExecuteNonQuery();
            if (i != 0)
            {

                con.Close();
                //add(log, haslo); // na około
            }



            con.Close();
        }

       public void Update(string na, string su, string hd, string sex, string tel, string b_day,string id)
        {
            con.Open();
            string sql = "UPDATE workers SET name='"+na+"',surname='"+su+"',hiring_date='"+hd+"',sex='"
                +sex+"', contact_number = '"+tel+"', birthday = '"+b_day+"' WHERE Id = '"+id+"'";
            SqlCommand c = new SqlCommand(sql, con);
            int i = c.ExecuteNonQuery();
            if (i != 0)
            {
                MessageBox.Show("Updated", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                con.Close();
               
            }



            con.Close();

        }
     




        public void result(string log, string password)
        {

            //add();
            con.Open();
            string sql = "SELECT *from names WHERE login= '" + log + "' and password = '" + password + "';";

            SqlCommand c = new SqlCommand(sql, con);
            try
            {
                int i = c.ExecuteNonQuery();

                if (i != 0)
                {

                    MessageBox.Show("Logged");
                    Form2.logged = true;
                }
                else
                {
                    MessageBox.Show("Ups");
                    con.Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        } //IDK why it doesn't work
    } 
}

    /*
            SqlConnection com = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;" +
                       "Initial Catalog=users;Integrated Security=True");
            com.Open();
            string cmd = "SELECT *from names WHERE login ='"+ log + "' and password ='" + password + "'";
            MessageBox.Show(cmd);
            SqlCommand c = new SqlCommand(cmd, com);

            int i = c.ExecuteNonQuery();
            if (i > 0)
            {
                MessageBox.Show("Connection done");
            }
            else
            {
                MessageBox.Show("error, shit");
            }
        }  }
}*/
