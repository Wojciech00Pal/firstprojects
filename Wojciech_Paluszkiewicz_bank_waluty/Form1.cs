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

namespace Wojciech_Paluszkiewicz_144130_kolokwium._7._06
{
    public partial class Form1 : Form
    {
       public static String  name="start";

       internal static Form2 form2;
       internal static Form1 form1;


        public Form1()
        {
            InitializeComponent();
            form1 = this;                            //   Nie wiem czemu nie działa komunkacja miedzy formami
            form2 = new Form2();

        }
         
        private void button1_Click(object sender, EventArgs e) //register
        {

            name = textBox1.Text;//login
            var haslo = textBox2.Text; //haslo

            if (haslo != "" && name != "")
            {
                using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
                {
                    byte[] haslo_ = Encoding.ASCII.GetBytes(haslo);
                    byte[] hashbytes = md5.ComputeHash(haslo_); //szyfrowanie hasla;

                    string haslop = Encoding.UTF8.GetString(hashbytes); //dziala
                    zapisz_do_pliku(name, haslop); 
                }
            }
            else
            {
                MessageBox.Show("Podaj login i hasło ");
            }

        }

        public static string return_log()
        {
            return name;
        }

        public void zapisz_do_pliku(string login,string haslo)
        {
            bool jest = false;
            string[] cut;
            try
            {
                using (StreamReader sr = new StreamReader("logi" + ".txt"))
                {
                        string wiersz = sr.ReadLine();

                        while (wiersz != null)// po dokumencie wpierw szukamy loginu
                        {

                            if (!(wiersz == ""))//gdy jest pusty wiersz zwraca false 
                            {
                                cut = wiersz.Split('/');
                                wiersz = sr.ReadLine();
                                if(cut[0]==login)
                                {
                                    MessageBox.Show("Wybierz inny login");
                                    jest = true;
                                    break;
                                }
                            
                            
                            }

                        }

                   
                    
                }
                if (jest == false)
                {
                    using (StreamWriter baza = new StreamWriter("logi.txt", true))
                    {

                        baza.WriteLine(login + "/" + haslo);   ///!!!!!!!!!!
                    }
                }
            }
            catch (Exception x)
            {
               MessageBox.Show(x.Message);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            String login = textBox1.Text;//login
            String haslo = textBox2.Text; //haslo
            bool log_in = false;
            name = login;
            
            try
            {
                using (StreamReader sr = new StreamReader("logi" + ".txt"))
                {
                    string wiersz = sr.ReadLine();

                    while (wiersz != null)// po dokumencie wpierw szukamy loginu
                    {

                        if (!(wiersz == ""))//gdy jest pusty wiersz zwraca false 
                        {
                            string[] cut = wiersz.Split('/');
                            wiersz = sr.ReadLine();

                            if (cut[0] == login && cut[1] == hash(haslo))
                            {
                                MessageBox.Show("Zalogowano ciebie");
                                log_in = true;
                                //  tworz(login);
                                form2.Show();         //     powinno otwierac sie nowy form nie wiem w czym problem???
                                
                                break;
                                
                            }


                        }

                    }
                    if (!log_in)
                    {
                        MessageBox.Show("Logowanie nieudane");
                    }

                }


            }
            catch { }
        }

        /*
        public void tworz(string log)
        {
            f2 = new Form2(log);
            f2.Show();


        }
        */
        public string hash(string str)
            {

            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] haslo_ = Encoding.ASCII.GetBytes(str);
                byte[] hashbytes = md5.ComputeHash(haslo_); //szyfrowanie hasla;

                string haslop = Encoding.UTF8.GetString(hashbytes); //dziala
                return haslop;
            }
        }

        /*
        private void Form1_Load(object sender, EventArgs e)
        {
            f2 = new Form2();
            f2.Show();
        }
        */
    }



}
