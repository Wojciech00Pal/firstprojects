using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;


namespace _8._03_zajęcia
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string user;
        int score,counter;
        //private Control nazwa;

        private void button1_Click(object sender, EventArgs e)
        {

            user = (textBox1.Text);

            Controls.Remove(button1);
            button1.Dispose();
            Controls.Remove(textBox1);
            textBox1.Dispose();
            Controls.Remove(label1);
            button2.Visible = true;
            label2.Visible = true;
            label3.Visible = true;
            label4.Visible = true;
            trackBar1.Visible = true;
            textBox2.Visible = true;
            button3.Visible = true;
         
            Image my = new Bitmap(@"C:\Users\User\Politechnika_4sem\Informatyka\8.03_zajęcia\pole.png");
            this.BackgroundImage = my;
            
            /*
            Random r = new Random();
            Button nowy = new Button();
            nowy.Text = "Kaczka nowa";
            nowy.Visible = true;
            nowy.Click += new System.EventHandler(this.modyfikuj);

            */

        }

        /*  private void duck_Click()
          {
              var nazwa = Controls.Find("Duck",true);
              Controls.Remove(this.nazwa);
              score++;
              textBox2.Text = "";
              textBox2.Text += score + "/" + counter;

          }*/

        



        //public void show_duck(object sender, EventArgs e)
        public void show_duck()
        {

            /*
            Random r = new Random();
            Button nowy = new Button();
            nowy.Text = "Kaczka nowa";
            nowy.Click += new System.EventHandler(this.modyfikuj);
            // int rInt = r.Next(0, 100);
            */

            Random r = new Random();
            Point loc = button3.Location;
            loc.X = r.Next(0, 719);
            loc.Y = r.Next(0, 299);
            button3.Location = loc;
            button3.Visible = true;
            
         //  counter++;


        }

   

        private void button2_Click(object sender, EventArgs e)
        {
            using (StreamWriter sw = new StreamWriter("Username.txt", true))//true nadpisuje
            {
                sw.WriteLine(user + " " + score);
            }

            button2.Visible = false;
            label2.Visible = false;
            label3.Visible = false;
            label4.Visible = false;
            trackBar1.Visible = false;
            textBox2.Visible = false;
            button3.Visible = false;
            button4.Visible = false;
            score = 0;
            counter=0;

            this.InitializeComponent();

        }
 
        private void timer1_Tick(object sender, EventArgs e)
        {
            int time = 2000/trackBar1.Value;

            if (label2.Visible == true)
            {
                timer1.Interval = time;
                show_duck();
                counter++;

              // EventHandler eventHandler = new System.EventHandler(this.show_duck);
               //timer1.Tick += eventHandler;
            }
           //Thread.Sleep(100);
            if (trackBar1.Value >= 4 )
            {
                Thread.Sleep(100);
                timer1.Interval = time;
         
                show2_duck();
                counter++;
            }
            // button3.Visible = true;

            //EventHandler eventHandler = new System.EventHandler(this.show_duck);
            // timer1.Tick += eventHandler;


        }

      

      


        public void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Image my = new Bitmap(@"C:\Users\User\Politechnika_4sem\Informatyka\8.03_zajęcia\pole.png");
            this.BackgroundImage = my;
        }

     
        private void button3_Click(object sender, EventArgs e)//kaczka
        {
            button3.Visible = false;
            score++;
            textBox2.Text = "";
            textBox2.Text += score+"/"+counter;

        }

        private void show2_duck()
        {
            Random r = new Random();
            Point loc = button3.Location;
            loc.X = r.Next()%720;
            loc.Y = r.Next()%300;
            button4.Location = loc;
            button4.Visible = true;


        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            button4.Visible = false;
            score++;
            textBox2.Text = "";
            textBox2.Text += score + "/" + counter;

        }

    }

}
