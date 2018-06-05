using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace TicketSystem
{
    public partial class Form1 : Form
    {
        int lastLstIndex = -1;

        public Form1()
        {
            InitializeComponent();
            JsonController.ReadJson();
            fillTicketTable();
        }
       
        public void fillTicketTable()
        {
            lstBoxTickets.Items.Clear();
            foreach (Ticket t in Ticket.ticketArray) {
                lstBoxTickets.Items.Add("#"+t.ID+" ["+t.Status+"] "+t.Subject);
            }
        }

        private void lstBoxTickets_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lastLstIndex != -1) try { Ticket.saveTicket(Ticket.ticketArray[lastLstIndex], lblID.Text, lblSubject.Text, txtNotes.Text, txtSolution.Text, txtStatus.Text); } catch { }
            if(lstBoxTickets.SelectedIndex != -1)populateTicketInformation(Ticket.ticketArray[lstBoxTickets.SelectedIndex]);
            lastLstIndex = lstBoxTickets.SelectedIndex;
            fillTicketTable();
        }

        private void populateTicketInformation(Ticket t)
        {
            txtStatus.Text = "";
            lblID.Text = "#" + t.ID;
            lblSubject.Text = t.Subject;
            txtNotes.Text = t.Notes;
            txtSolution.Text = t.Solution;
            txtStatus.SelectedText = t.Status;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(lastLstIndex < Ticket.ticketArray.Count)
            if (lastLstIndex != -1) Ticket.saveTicket(Ticket.ticketArray[lastLstIndex], lblID.Text, lblSubject.Text, txtNotes.Text, txtSolution.Text, txtStatus.Text);
            JsonController.SaveJson();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddTicket d = new AddTicket();
            d.Show();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if(lastLstIndex != -1)Ticket.ticketArray.Remove(Ticket.ticketArray[lastLstIndex]);
            fillTicketTable();
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            fillTicketTable();
            if (lastLstIndex != -1) try { populateTicketInformation(Ticket.ticketArray[lastLstIndex]); } catch { }
        }

        private void txtSolution_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lblID.Text == "") return;
            FullScreen f = new FullScreen(txtSolution.Text, lastLstIndex);
            f.Show();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            JsonController.SaveJson();
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            JsonController.ExportJson();
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            JsonController.ImportJson();
        }
    }

    public class Ticket
    {
        internal static List<Ticket> ticketArray = new List<Ticket>();

        internal int ID;
        internal string Subject;
        internal string Notes;
        internal string Solution;
        internal string Status;

        public Ticket(int _ID, string _Subject, string _Notes, string _Solution, string _Status)
        {
            ID = _ID;
            Subject = _Subject;
            Notes = _Notes;
            Solution = _Solution;
            Status = _Status;
        }

        internal static void saveTicket(Ticket t, string ID, string Subject, string Notes, string Solution, string Status)
        {
            t.ID = Convert.ToInt32(ID.Replace("#", ""));
            t.Subject = Subject;
            t.Notes = Notes;
            t.Solution = Solution;
            t.Status = Status;
        }
    }

    public class JsonController
    {
        internal static void ReadJson()
        {
            try
            {
                var ticketLines = Properties.Settings.Default.Json.Split('µ');
                //Console.WriteLine("Line: " + ticketLines[0]);
                string[][] individualLine = new string[ticketLines.Length][];
                for (int i = 0; i < ticketLines.Length - 1; i++)
                {
                    individualLine[i] = ticketLines[i].Split('¼');
                }
                for (int i = 0; i < individualLine.Length - 1; i++)
                {
                    Ticket.ticketArray.Add(new Ticket(Convert.ToInt32(individualLine[i][0]), individualLine[i][1], individualLine[i][2], individualLine[i][3], individualLine[i][4]));
                }
            }
            catch
            {
                var test = MessageBox.Show("It appears that your ticket file is corrupt. Would you like me to delete it?", "Error: Corruption!", MessageBoxButtons.YesNo);
                if(test == DialogResult.Yes)
                {
                    Properties.Settings.Default.Json = "";
                }
            }
        }

        internal static void SaveJson()
        {
            Properties.Settings.Default.Json = "";
            foreach (Ticket t in Ticket.ticketArray)
            {
                Properties.Settings.Default.Json += t.ID+"¼"+ t.Subject + "¼" + t.Notes + "¼" + t.Solution + "¼" + t.Status + "µ";
                Properties.Settings.Default.Save();
            }
            if(Ticket.ticketArray.Count == 0)
            {
                Properties.Settings.Default.Json = "";
                Properties.Settings.Default.Save();
            }
        }

        internal static void ExportJson()
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.FileName = "Tickets.tkt";
            saveFileDialog1.Filter = "Tickets |*.tkt";
            saveFileDialog1.Title = "Export a Ticket File";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamWriter writer = new StreamWriter(saveFileDialog1.OpenFile());

                foreach (Ticket t in Ticket.ticketArray)
                {
                    writer.WriteLine(t.ID + "¼" + t.Subject + "¼" + t.Notes + "¼" + t.Solution + "¼" + t.Status + "µ");
                }

                writer.Dispose();
                writer.Close();
            }
        }

        internal static void ImportJson()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Select Ticket File to Import";
            ofd.Filter = "Tickets |*.tkt";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string path = ofd.FileName;
                var test = MessageBox.Show("Would you like to add on to your tickets? If not I would delete all existing tickets.", "Warning!", MessageBoxButtons.YesNo);
                if (test == DialogResult.No)
                {
                    Properties.Settings.Default.Json = "";
                    Ticket.ticketArray.Clear();
                }
                using (StreamReader sr = new StreamReader(path))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        Properties.Settings.Default.Json += line;
                        JsonController.ReadJson();
                    }
                }
            }
        }
    }
}
