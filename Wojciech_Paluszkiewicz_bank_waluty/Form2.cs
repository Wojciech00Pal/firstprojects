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
 
    public partial class Form2 : Form
    {
       string name;
       internal static Form3 form3;
       internal static Form2 form2;
       internal static Form1 form1;

        public static List<string> waluty = new List<string>();
        public Form2()
        {
            form3 = new Form3();
            
           // string name = Form1.return_log(); //zeby zronic reszte bo nie dziala komunikacja
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e) //save
        {

            

            string name = Form1.return_log();

            using (StreamWriter baza = new StreamWriter("pliki/"+Form1.name+".txt", true))
            {
                String sDate = DateTime.Now.ToString();
                baza.WriteLine( richTextBox1.Text);   ///!!!!!!!!!!
                baza.WriteLine(sDate);
            }
           MessageBox.Show("Zmiany zapisane do pliku uzytkownika");
          
          //  System.Threading.Thread.Sleep(100);
        }

       
        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            

            form3.Show();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            textBox1.Text = DateTime.Now.ToString("HH:mm:ss"); /// zegar

        }

        private void button3_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }
    }
}
