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

namespace projekt_14._06
{

    public partial class Form1 : Form
    {
       
        public Form1()
        {
            InitializeComponent();
            
        }

    

        public  void closeChildren()
        {
            foreach(Form f in this.MdiChildren)
            {
                f.Close();
            }
        }
        


        private void show_From(Form f)
        {
            this.IsMdiContainer = true;
            f.MdiParent = this;
            f.Show();
           
            
        }



        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Form2.logged == true)
            {

                toolStripLabel1.Text = "Hi, " + Form2.name;
                label1.Visible = false;
            }
            else
            {
                toolStripLabel1.Text = "Waiting...       ";
                label1.Visible = true;
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

            closeChildren();
            if (Form2.logged == true)
            {
                Form2.logged = false;
                toolStripLabel1.Text = "log around";
                show_From(new Form2());
            }
            else
            {

                show_From(new Form2());
            }
        }

     

        private void toolStripButton2_Click(object sender, EventArgs e)//robotnik
        {
            closeChildren();
            if(Form2.logged)
            {
                show_From(new workers());
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            closeChildren();
            if (Form2.logged)
            {
                show_From(new admin());
            }
        }

        private void toolStripButton3_Click_1(object sender, EventArgs e)
        {
            closeChildren();
            if (Form2.logged)
            {
                show_From(new Payment());
            }
        }
    }
}
