using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace Server
{
    public class User
    {
        
        public Socket s;
        public String name;
        public bool logged_in=false;

        // public List<User> users;

        //Skrzynka w liscie stringów
        List<String> delay_message;


        public void  write_to_list(string str)
        {

            delay_message.Add(str);

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
