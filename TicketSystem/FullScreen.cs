using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TicketSystem
{
    public partial class FullScreen : Form
    {
        int pos = -1;
        public FullScreen(string _Solution, int i)
        {
            InitializeComponent();
            richTextBox1.Size = this.Size;
            richTextBox1.Text = _Solution;
            pos = i;
            this.Text = "Ticket #" + Ticket.ticketArray[i].ID + " - " + Ticket.ticketArray[i].Subject;
        }

        private void FullScreen_Resize(object sender, EventArgs e)
        {
            richTextBox1.Size = this.Size;
        }

        private void FullScreen_FormClosing(object sender, FormClosingEventArgs e)
        {
            Ticket.ticketArray[pos].Solution = richTextBox1.Text;
        }
    }
}
