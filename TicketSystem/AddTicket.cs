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
    public partial class AddTicket : Form
    {
        public AddTicket()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int i = 0;
            if (int.TryParse(textBox1.Text, out i))
            {
                Ticket.ticketArray.Add(new Ticket(Convert.ToInt32(textBox1.Text), textBox2.Text, "", "", "Not Completed"));
                this.Close();
            }else
            {
                MessageBox.Show("Error: Not a Valid Number");
            }
        }
    }
}
