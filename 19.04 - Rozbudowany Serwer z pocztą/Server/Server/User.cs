using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;

namespace Server
{
    public class User
    {
        
        public Socket s;
        public String name;
        public bool logged_in=false;


        // public List<User> users;

        //Skrzynka w liscie stringów
       // List<String> delay_message;


        public void get_post(Socket s)
        {
            //string path = @"skrzynka\" + name + ".txt";
            //using (StreamReader sr = new StreamReader(path))
            using (StreamReader sr = new StreamReader(@"skrzynka\" + name + ".txt"))
                {
                    string wiersz = sr.ReadLine();
                    string path = @"skrzynka\" + name + ".txt";
                    while (wiersz != null)// po dokumencie wpierw szukamy loginu
                    {

                    byte[] msg = Encoding.ASCII.GetBytes(wiersz);
                    s.Send(msg);
                   // wiersz = ""; //zerowanie wiersza gdy już pobralismy    
                    wiersz = sr.ReadLine();
                    }

       

            }
            
           

        }
        
       
        /*
        public void dodaj_uzytkownika(User klient)
        {
            users.Add(klient);
        }
        */

        public Socket set_socket()
        {
            return s;
        }

        public string get_name()
        {
            return name;
        }
    }
}
