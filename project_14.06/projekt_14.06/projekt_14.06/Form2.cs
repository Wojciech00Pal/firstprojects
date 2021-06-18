using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;
using System.Security;
using Amazon.Runtime.Internal.Util;

namespace projekt_14._06
{
    public partial class Form2 : Form
    {
        public static string name = "";
        public static bool logged = false;
        public Form2()
        {
            InitializeComponent();
        }



        SQL cmd = new SQL();

        CngAlgorithm cng = new CngAlgorithm("Sh256");
        // string sql;
        private void button1_Click(object sender, EventArgs e)
        {

            string log = textBox1.Text;
            var password = textBox2.Text;



               int i = Hashing.Hash(password);
               password = i.ToString();
       

                cmd.delete(log,password);
                if (logged)
                {
                    MessageBox.Show("Witaj " + log);
                    name = log;
                    this.Close();
                }
            
        }


  

            

            //   void w_pliku_txt()
          //  {
                /* ------------------------------BEZ bazy danych--------------------------

                bool inside = false;

                if (log != "")
                {
                    using (StreamWriter save = new StreamWriter("loginy/log.txt", true))
                    {
                    }
                    using (StreamReader LINE = new StreamReader("loginy/log.txt", true))
                    {
                        String line = LINE.ReadLine();

                        while (line != null)
                        {
                            if (!(line == ""))
                            {
                                if (line == log)
                                {
                                    MessageBox.Show("logged as " + log);
                                    name = log;
                                    logged = true;
                                    inside = true;

                                    break;
                                }
                                line = LINE.ReadLine();
                            }
                        }
                    }

                    if (inside == false)
                    {
                        using (StreamWriter save = new StreamWriter("loginy/log.txt", true))
                        {
                            save.WriteLine(log);
                            name = log;
                            logged = true;
                            MessageBox.Show("You are created");


                        }
                    }
                }
                else
                {
                    MessageBox.Show("Enter your name");
                }
                */

            

            //cmd.delete(log, password);
           // cmd.result(log, password);
            
    }
}    
       
    
   
