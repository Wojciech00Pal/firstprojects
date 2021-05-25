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
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace Server
{
    public  partial class Form1 : Form
    {
        

        //public partial class Form1 : Form
        public Thread listenThread,klientowy;
        public Socket acceptedSocket; // liste socketów List<Socket>                                      //ewentualnie lsita watkow klienckich 
        public Socket localListeningSocket;

        //List<Socket> sok = new List<Socket>();
        public List<User> users = new List<User>();
        
        List<Socket> soki = new List<Socket>(); //klientowe
        
        List<Thread> trid = new List<Thread>(); //Thready klienta

        User klient;


        public Form1()
        {
            InitializeComponent();

            listenThread = new Thread(StartServer);
            listenThread.Start(this);

        }

    

        public void StartServer(Object form)
        {
            RichTextBox brd = ((Form1)form).richTextBox1;
            
            IPHostEntry host = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = host.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);


            ((Form1)form).localListeningSocket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            ((Form1)form).localListeningSocket.Bind(localEndPoint);
            ((Form1)form).localListeningSocket.Listen(10);

      
            while (true)
            {
              

                ((Form1)form).acceptedSocket = ((Form1)form).localListeningSocket.Accept();
                //sok.Add(acceptedSocket);//list socketow
                klient = new User();
                klient.s = acceptedSocket;
                //users.Add(klient);               // Tu bez sensu dodawalem &&&&&&&&&&&&&&&&&&&&&&&&&&&&&     16.04

                //####################################

                string data = null;//zeby nie wysłać poprzedniej
                byte[] bytes = null;
                bytes = new byte[1024];
                int bytesRec = acceptedSocket.Receive(bytes);
                data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                //brd.Text += Environment.NewLine + data;                      // watki sie wpieprzaja

                

                if (!(data == ""))//gdy nie jest pusty
                {

                    string[] cut = data.Split('/'); // cut[0] = log or reg, [1] = login, [2]=haslo
                                                    //cut[1].Replace(',', '.');
                    klientowy = new Thread(() => baza_danych(cut[0], cut[1], cut[2], klient));  //nowy Thread dla kazdego klienta
                    klientowy.Start();
                   // baza_danych(cut[0], cut[1], cut[2],klient);
                }

                //################################################

                // nasluchiwanie_info_od_klienta(this, acceptedSocket, brd);
                // Thread thread = new Thread(() => nasluchiwanie_info_od_klienta(this, acceptedSocket,brd));
                //trid.Add(thread);
                //thread.Start();
            }

        }

        
        
        public void nasluchiwanie_info_powtorne(Socket s,User client)//i wysylanie do pozostalych
        {
            /// zamknac w nowym wątku / jeden watek na jednego klienta //   nasluchiwanie info od klienta

            //trzeba sprawdzic haslo
            // i login
            while (true)
            {
                string data = null;//zeby nie wysłać poprzedniej
                byte[] bytes = null;
                bytes = new byte[1024];




                try
                {
                    int bytesRec = s.Receive(bytes);
                    data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                    // brd.Text += Environment.NewLine + data;                      // watki sie wpieprzaja

                    richTextBox1.Text += Environment.NewLine + data;




                    if (!(data == "")) //gdy nie jest pusty
                    {

                        string[] cut = data.Split('/'); // cut[0] = log or reg, [1] = login, [2]=haslo
                                                        //cut[1].Replace(',', '.');

                        //double kurs = Double.parseDouble(cut[1]);
                         baza_danych(cut[0], cut[1], cut[2], client); 
                    
                        //baza_danych(cut[0], cut[1], cut[2], client);
                    }
                }
                catch(Exception x)
                {
                    richTextBox1.Text += x.Message;
                }

            }
        }
        
        
        public void baza_danych(String opt,string log,string haslo,User client)
        {
            /*
            int counter = 0;
            
            foreach (var element in users)
            {
                listBox1.Items.Add(counter + ". " + element.name);
                counter++;
            }
            */

            if (opt == "log")
                {
                    if (yes_or_no(log))
                    {
                        check_haslo(log,haslo,client); 
                    }
                    else // 
                    {
                        byte[] msg = Encoding.ASCII.GetBytes("Zarejestruj sie/");
                        acceptedSocket.Send(msg); // nie istnieje
                        nasluchiwanie_info_powtorne(client.s,client);//powtorz rejestracje
                    }
                }
           else //rejestracja
                {
                    if (yes_or_no(log)) // juz taki login jest
                    {

                        byte[] msg = Encoding.ASCII.GetBytes("Wybierz inny login/");
                        acceptedSocket.Send(msg);
                        nasluchiwanie_info_powtorne(client.s, client);
                    }
                    else // nie ma w bazie loginu
                    {
                        zapisz(log,haslo);
                        klient.logged_in = true; //zalogowany
                        klient.s = acceptedSocket;
                        klient.name = log;

                        byte[] msg = Encoding.ASCII.GetBytes("Zarejestrowany, login: "+ klient.name);
                        acceptedSocket.Send(msg);
                    //??Q??80?????? = zalogowany     
                    byte[] veryfication = Encoding.ASCII.GetBytes("<??Q??80??????>");
                    acceptedSocket.Send(veryfication);

                    //dodaj_uzytkownika(klient);
                    users.Add(klient);

                    listBox1.Items.Add(klient.get_name());  // dodanie do listboxa
                    soki.Add(acceptedSocket);

                    messages(klient);
                   // klientowy = new Thread(() => messages(klient));
                   // klientowy.Start();

                }
               }
        }

        public void check_haslo(String login,String pass,User client)
        {
            bool match = false;
            try
            {
                using (StreamReader sr = new StreamReader("logi" + ".txt"))
                {
                    string wiersz;

                    while (sr.ReadLine() != null)// po dokumencie wpierw szukamy loginu
                    {
                        wiersz = sr.ReadLine();

                        if (!(wiersz == ""))//gdy nie jest pusty
                        {
                            string[] cut = wiersz.Split('/');
                            if (cut[0] == login)
                            {
                                if (cut[1] == pass)
                                {
                                    klient.logged_in = true; //zalogowany
                                    klient.s = acceptedSocket;
                                    klient.name = login;
                                    byte[] msg = Encoding.ASCII.GetBytes("Zalogowano, login: "+klient.name);
                                    acceptedSocket.Send(msg);
                                    byte[] veryfication = Encoding.ASCII.GetBytes("<??Q??80??????>");
                                    acceptedSocket.Send(veryfication);

                                    users.Add(klient);
                                    soki.Add(acceptedSocket);
                                    listBox1.Items.Add(klient.get_name());  // dodanie do listboxa
                                    soki.Add(acceptedSocket);
                                    match = true;

                                    klientowy = new Thread(() => messages(klient));
                                    klientowy.Start();
                                }
                            }

                            //double kurs = Double.parseDouble(cut[1]);

                        }

                    }
                }

            }
            catch
            {
                if (!match)
                {
                    byte[] msg = Encoding.ASCII.GetBytes("Bledne haslo :(/");
                    klient.s.Send(msg); // nie istnieje
                    nasluchiwanie_info_powtorne(client.s, client);
                }
                else
                {
                    byte[] msg = Encoding.ASCII.GetBytes("Cos poszlo nie tak :(/");
                    klient.s.Send(msg); // nie istnieje
                }
            }
        }

        // komunikacja miedzy osobamiu lub all - do wszystkich
        public void messages(User client)
        {
            if (client.logged_in)
            {
                //sprawdzamy czy do wszytkich czy do all
                while (true)
                {
                    string data = null;//zeby nie wysłać poprzedniej
                    byte[] bytes = null;
                    bytes = new byte[1024];



                    int bytesRec = client.s.Receive(bytes); // socket clienta - > otrzymana wiadomosc
                    data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                    //brd.Text += Environment.NewLine + data;
                   
                    if (data.IndexOf("<logout>") > -1 && client.logged_in == true)
                    {
                        byte[] msg_logout = Encoding.ASCII.GetBytes("Wylogowalismy Ciebie - zamknij okno/");
                        client.s.Send(msg_logout); // wylogowany

                        delete_connection(acceptedSocket, klient);
                        break;

                    }

                    

                    if (!(data == ""))//gdy nie jest pusty
                    {

                        string[] cut = data.Split('/'); // login/message
                        /*

                        string p = "all";
                        switch(cut[0])
                        {
                            case "ccc":
                                {
                                    foreach (Socket s in soki)
                                    {

                                    }
                                    break;//jwpt4oewpi
                                }
                            default:
                                {
                                    break;
                                }
                        }
                        */


                        


                        if (cut[0]=="all")
                        {
                            foreach (Socket s in soki)
                            {
                                byte[] msg = Encoding.ASCII.GetBytes(klient.name +": "+ cut[1]);//autor wiadomosci do 
                                //wszystkich
                                
                                s.Send(msg);  
                            }
                        }

                        else
                        {
                            bool was = false;
                            foreach(User u in users)
                            {
                                //szukanie loginu
                                if (u.name == cut[0] && was==false)
                                {
                                    byte[] msg = Encoding.ASCII.GetBytes(klient.name+": "+cut[1]);
                                    u.s.Send(msg);  // do konkretnego usera                // TUUUUUUU wysyla do konkretnego @@@@@@@@@@
                                    
                                    was = true;
                                }
                               // byte[] msg = Encoding.ASCII.GetBytes("ddssd");
                               //u.set_socket().Send(msg);
                            }

                            

                            if (!was)//nie ma takiego loginu
                            {
                                
                                    byte[] msg = Encoding.ASCII.GetBytes("Nie ma takiego loginu");
                                    client.s.Send(msg);
                               
                            }
                        }
                    }
                    else
                    {
                        byte[] msg = Encoding.ASCII.GetBytes("Pusta wiadomosc :(/");
                        klient.s.Send(msg); // nie istnieje
                    }


                }
            }

            else
            {
                byte[] msg = Encoding.ASCII.GetBytes("Zaloguj się:(/");
                klient.s.Send(msg); // nie istnieje

            }
        }

        public void zapisz(String l,String h)
        {
            String zapis = l + "/" + h;// login/haslo
            String val = "logi";
            string path = val + ".txt";
            using (StreamWriter label = new StreamWriter(path, true))
            {
                label.WriteLine(zapis);
            }
        }


        public bool yes_or_no(string log)//sprawdza czy jest taki login
        {
            try
            {
                using (StreamReader sr = new StreamReader("logi" + ".txt"))
                {
                    string wiersz= sr.ReadLine();

                    while (wiersz != null)// po dokumencie wpierw szukamy loginu
                    {
                    
                       // if (wiersz != "")//gdy nie jest pusty
                        //
                            string[] cut = wiersz.Split('/');  // NIE MA NI
                            if (cut[0] == log)
                            {
                                return true;//gdy jest w bazie 
                            }

                        //double kurs = Double.parseDouble(cut[1]);

                        //}
                        wiersz = sr.ReadLine();
                    }
                    return false; //gdy nie ma w bazie 
                }
            }
            catch
            {
                return false; //gdy nie ma pliku 
            }
        }

       

        public void delete_connection(Socket s,User klient)
        {
            soki.Remove(s);
            users.Remove(klient);
            listBox1.Items.Remove(klient.get_name());
            klient.logged_in = false;  // DOPISANE
            klientowy.Abort();
            byte[] msg = Encoding.ASCII.GetBytes("close window ");
            acceptedSocket.Send(msg);
            s.Shutdown(SocketShutdown.Both);

        }
    }
}


//Problem z wylogowaniem
// pieprzy sie chyba jak nic klient nie napisal