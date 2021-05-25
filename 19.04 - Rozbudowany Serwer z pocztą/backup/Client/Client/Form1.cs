using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;



namespace Client
{
    public partial class Form1 : Form
    {
        Socket localClientSocket;
        Thread myth;

        

        public Form1()
        {
         

            InitializeComponent();
            IPHostEntry host = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = host.AddressList[0]; // localhost
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);

            String imie;
            // Create a TCP/IP  socket.    
            localClientSocket = new Socket(ipAddress.AddressFamily,
            SocketType.Stream, ProtocolType.Tcp);
            localClientSocket.Connect(remoteEP);

            richTextBox1.Text = "Socket connected to {0}" + localClientSocket.RemoteEndPoint.ToString();

            

            myth = new Thread(listen);
            myth.Start(this);

        }

        public void write(Object form,string moja_wiadomosc)
        {
            RichTextBox brd1 = ((Form1)form).richTextBox1;
            brd1.Text += Environment.NewLine + moja_wiadomosc;
        }

        private void button1_Click(object sender, EventArgs e)
        {
          

            //wysylanie wiadomości
            //znacznik all do wszystkich lub login
            try
            {
                byte[] bytes = new byte[1024];
                // Encode the data string into a byte array.    
                byte[] msg = Encoding.ASCII.GetBytes(textBox2.Text +"/"+textBox1.Text);

                write(this, "Ja:" + textBox1.Text);

                int bytesSent = localClientSocket.Send(msg);
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
            }

        }
        // Release the socket.    
        //senderSocket.Shutdown(SocketShutdown.Both);
        //   senderSocket.Close();

        public static void listen(Object form)
        {
            RichTextBox brd1 = ((Form1)form).richTextBox1;
        
            //aktywny bedzie dla serwera jak bedzie zalogowany
            //inaczej nic stąd nie używane
           
            Button klik = ((Form1)form).button1; // aktywuje przycisk gdy zalogowany
            Button logout = ((Form1)form).button3;
            while (true)
            {
                
                string data = null;
                byte[] bytes = null;

                bytes = new byte[1024];
                int bytesRec = ((Form1)form).localClientSocket.Receive(bytes);
                data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                

                
                if (data.IndexOf("<??Q??80??????>") > -1)
                {
                    klik.Enabled = true;//zalogowany
                    logout.Enabled = true;
                    
                    data = "Logowanie udane :)";
                    brd1.Text += Environment.NewLine + data;

                }
                else
                {
                    brd1.Text += Environment.NewLine + data;
                }
               
                
            }
            // brd.Text = "";
            //brd.Text = "NAURA - close window";
            //((Form1)form).localClientSocket.Shutdown(SocketShutdown.Both);
            // ((Form1)form).localClientSocket.Close();
        }

       

        public void wejscie(string log,string haslo)
        {
            String radio_option;
            textBox3.Text = "";
            textBox4.Text = "";
            //r_button1 = logowanie

            if (radioButton1.Checked)
            {
                radio_option = "log";
            }
            else
            {
                radio_option = "reg"; // znacznik do rejestracji
            }

            try
            {

                //znacznik na logowanie => /log/
                byte[] bytes = new byte[1024];
                // Encode the data string into a byte array.    
                byte[] msg = Encoding.ASCII.GetBytes(radio_option + "/" + log + "/" + haslo);//wysyla haslo i login do serwera
                                                                                               //int bytesSent = localClientSocket.Send(msg);
                localClientSocket.Send(msg);
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
            }

        }


        private void button2_Click(object sender, EventArgs e) //logowanie i sprawdzanie bazy danych
        {
            String name = textBox3.Text;//login
            var haslo = textBox4.Text; //haslo

            if(haslo!="" && name !="")
                {
                using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
                {
                    byte[] haslo_ = Encoding.ASCII.GetBytes(haslo);
                    byte[] hashbytes = md5.ComputeHash(haslo_); //szyfrowanie hasla;

                    string haslop = Encoding.UTF8.GetString(hashbytes); //dziala
                    wejscie(name, haslop); // chce wejsc do serwera
                }
            }
            else
            {
                MessageBox.Show("Podaj login i hasło ");
            }
                /*

                String radio_option ;
                
                //r_button1 = logowanie
                
                if (radioButton1.Checked)
                {
                    radio_option = "log";
                }
                else
                {
                    radio_option = "reg"; // znacznik do rejestracji
                }

                try
                {
                    
                    //znacznik na logowanie => /log/
                    byte[] bytes = new byte[1024];
                    // Encode the data string into a byte array.    
                    byte[] msg = Encoding.ASCII.GetBytes(radio_option+"/" + name + "/"+ haslop);//wysyla haslo i login do serwera
                    //int bytesSent = localClientSocket.Send(msg);
                    localClientSocket.Send(msg);
                }
                catch (Exception x)
                {
                    MessageBox.Show(x.Message);
                }
                */


        }

        private void button3_Click(object sender, EventArgs e)
        {
            byte[] msg = Encoding.ASCII.GetBytes("<logout>");                                         //Problem z wylogowaniem
            int bytesSent = localClientSocket.Send(msg);
        }
    }
}