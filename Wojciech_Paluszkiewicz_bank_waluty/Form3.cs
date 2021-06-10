using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Wojciech_Paluszkiewicz_144130_kolokwium._7._06
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)  //send request
        {
            double mid;
            string currency;
            string date_and_time;
            try
            {
                string url = "http://api.nbp.pl/api/exchangerates/rates/a/" + textBox1.Text + "/?format=json";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                string content = new StreamReader(response.GetResponseStream()).ReadToEnd();
                dynamic data = JObject.Parse(content);

                date_and_time = data.rates[0].effectiveDate;
                currency = textBox1.Text.ToUpper();

                if (on_list(currency) == false)
                {
                    Form2.waluty.Add(currency);
                    listBox1.Items.Add(currency);
                }
               // mid = data.rates[0].mid;

                DateTime current_date = DateTime.Now;
                //string result = "";
                // result = currency + ":" + mid.ToString() + "/" + date_and_time + "/" + current_date ;
                
                using (StreamWriter sw = new StreamWriter("pliki/" + Form1.name + ".txt", true))// do txt
                {
                    sw.WriteLine("NEW positions:");
                    int i = 0;
                    foreach (string element in Form2.waluty)
                    {
                        i++;
                        url = "http://api.nbp.pl/api/exchangerates/rates/a/" + element + "/?format=json";
                        request = (HttpWebRequest)WebRequest.Create(url);
                        response = (HttpWebResponse)request.GetResponse();
                        content = new StreamReader(response.GetResponseStream()).ReadToEnd();
                        data = JObject.Parse(content);
                        mid = data.rates[0].mid;
                        
                
                        if (i==Form2.waluty.Count()) //na koniec z data
                        {
                            string result = currency + ":" + mid.ToString() + "/" + date_and_time + "/" + current_date;
                            sw.WriteLine( result); 
                        }
                        else sw.WriteLine(element + ":" + mid.ToString());
                    }
                }

            }

            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
                return;
            }

        }

        public bool  on_list(string e)
        {
            foreach(string element in Form2.waluty)
            {
                if (e == element) 
                {
                    return true;
                }
            }
            return false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
           string currency = textBox1.Text.ToUpper();
            if (on_list(currency) == true)
            {
                Form2.waluty.Remove(currency);
                listBox1.Items.Remove(currency);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            double mid;
            string date_and_time;

            DateTime current_date = DateTime.Now;
           
            using (StreamWriter sw = new StreamWriter("pliki/" + Form1.name + ".txt", true))// do txt
            {
                sw.WriteLine(" ");
                sw.WriteLine("NEW positions:");
                int i = 0;
                foreach (string element in Form2.waluty)
                {
                    i++;
                    string url = "http://api.nbp.pl/api/exchangerates/rates/a/" + element + "/?format=json";
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    string content = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    dynamic data = JObject.Parse(content);
                    mid = data.rates[0].mid;
                    date_and_time = data.rates[0].effectiveDate;

                    if (i == Form2.waluty.Count()) //na koniec z data
                    {
                        string result = element + ":" + mid.ToString() + "/ " + date_and_time + " / " + current_date;
                        sw.WriteLine(result);
                    }
                    else sw.WriteLine(element + ":" + mid.ToString());
                }

            }


        }
    }
}
