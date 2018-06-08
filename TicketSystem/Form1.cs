using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
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
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "MM/dd/yyyy hh:mm:ss tt";
            dateTimePicker1.Value = DateTime.Now;
            if(Environment.UserName.ToLower() == "vancef") this.BackColor = Color.ForestGreen;
            changeColor();
        }
       
        void changeColor()
        {
            try{ dateTimePicker1.CalendarForeColor = Color.FromName(Properties.Settings.Default.AlarmFore); } catch { }
            try { dateTimePicker1.CalendarMonthBackground = Color.FromName(Properties.Settings.Default.AlarmBack); } catch { }
            try{ lstBoxTickets.ForeColor = Color.FromName(Properties.Settings.Default.TicketBoxFore); } catch { }
            try{ lstBoxTickets.BackColor = Color.FromName(Properties.Settings.Default.TicketBoxBack); } catch { }
            try{ txtNotes.ForeColor = Color.FromName(Properties.Settings.Default.NotesFore); } catch { }
            try{ txtNotes.BackColor = Color.FromName(Properties.Settings.Default.NotesBack); } catch { }
            try{ txtSolution.ForeColor = Color.FromName(Properties.Settings.Default.SolutionFore); } catch { }
            try{ txtSolution.BackColor = Color.FromName(Properties.Settings.Default.SolutionBack); } catch { }
            try { btnAdd.ForeColor = Color.FromName(Properties.Settings.Default.ButtonFore); } catch { }
            try { btnAdd.BackColor = Color.FromName(Properties.Settings.Default.ButtonBack); } catch { }
            try { btnRemove.ForeColor = Color.FromName(Properties.Settings.Default.ButtonFore); } catch { }
            try { btnRemove.BackColor = Color.FromName(Properties.Settings.Default.ButtonBack); } catch { }
            try { lblID.ForeColor = Color.FromName(Properties.Settings.Default.LabelFore); } catch { }
            try { lblSubject.ForeColor = Color.FromName(Properties.Settings.Default.LabelFore); } catch { }
            try { txtStatus.ForeColor = Color.FromName(Properties.Settings.Default.StatusFore); } catch { }
            try { txtStatus.BackColor = Color.FromName(Properties.Settings.Default.StatusBack); } catch { }
            try { lstBoxTickets.BackColor = Color.FromName(Properties.Settings.Default.TicketBoxBack); } catch { }
            try { this.BackColor = Color.FromName(Properties.Settings.Default.FormBack); } catch { }
            try { lstBoxTickets.ForeColor = Color.FromName(Properties.Settings.Default.FormFore); } catch { }
        }

        public void fillTicketTable()
        {
            lstBoxTickets.Items.Clear();
            foreach (Ticket t in Ticket.ticketArray) {
                var test = lstBoxTickets.Items.Add("#" + t.ID + " [" + t.Status + "] " + t.Subject);
                AlarmClock clock = new AlarmClock(t.alarm, t);
                clock.Alarm += (sender, e) => MessageBox.Show("Alarm for Ticket #" + t.ID + " - " + t.Subject);
            }
        }

        private void populateTicketInformation(Ticket t)
        {
            txtStatus.Text = "";
            lblID.Text = "#" + t.ID;
            lblSubject.Text = t.Subject;
            txtNotes.Text = t.Notes;
            dateTimePicker1.Value = t.alarm;
            try { txtSolution.Rtf = t.Solution; } catch { txtSolution.Text = t.Solution; }
            txtStatus.Text = t.Status;
        }
        
        #region Other Methods
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (lastLstIndex < Ticket.ticketArray.Count)
                if (lastLstIndex != -1) Ticket.saveTicket(Ticket.ticketArray[lastLstIndex], lblID.Text, lblSubject.Text, txtNotes.Text, txtSolution.Text, txtStatus.Text, dateTimePicker1.Value);
            JsonController.SaveJson();
        }

        bool fullOpen = false;
        private void Form1_Activated(object sender, EventArgs e)
        {
            if (lastLstIndex != -1 && !fullOpen && lstBoxTickets.Items.Count > 0 && Ticket.ticketArray.Count < lastLstIndex) Ticket.saveTicket(Ticket.ticketArray[lastLstIndex], lblID.Text, lblSubject.Text, txtNotes.Text, txtSolution.Text, txtStatus.Text, dateTimePicker1.Value);
            if (fullOpen) fullOpen = false;
            fillTicketTable();
            if (lastLstIndex != -1) try { populateTicketInformation(Ticket.ticketArray[lastLstIndex]); } catch { }
            changeColor();
        }

        private void lstBoxTickets_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lastLstIndex != -1) try { Ticket.saveTicket(Ticket.ticketArray[lastLstIndex], lblID.Text, lblSubject.Text, txtNotes.Text, txtSolution.Text, txtStatus.Text, dateTimePicker1.Value); } catch { }
            if (lstBoxTickets.SelectedIndex != -1) populateTicketInformation(Ticket.ticketArray[lstBoxTickets.SelectedIndex]);
            lastLstIndex = lstBoxTickets.SelectedIndex;
            fillTicketTable();
        }
        private void txtSolution_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lblID.Text == "") return;
            FullScreen f = new FullScreen(txtSolution.Text, lastLstIndex);
            f.Show();
            fullOpen = true;
        }
        #endregion

        #region Click Events
        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddTicket d = new AddTicket();
            d.Show();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (lastLstIndex != -1) Ticket.ticketArray.Remove(Ticket.ticketArray[lastLstIndex]);
            fillTicketTable();
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

        private void deleteAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var test = MessageBox.Show("Are you sure you want all of your tickets deleted?", "Warning!", MessageBoxButtons.YesNo);
            if (test == DialogResult.Yes)
            {
                Ticket.ticketArray.Clear();
                JsonController.SaveJson();
                fillTicketTable();
            }
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            fillTicketTable();
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            fillTicketTable();
        }
        #endregion

        private void setColorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorChange c = new ColorChange();
            c.Show();
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
        internal DateTime alarm;

        public Ticket(int _ID, string _Subject, string _Notes, string _Solution, string _Status, DateTime _alarm)
        {
            ID = _ID;
            Subject = _Subject;
            Notes = _Notes;
            Solution = _Solution;
            Status = _Status;
            alarm = _alarm;
        }

        internal static void saveTicket(Ticket t, string ID, string Subject, string Notes, string Solution, string Status, DateTime Alarm)
        {
            t.ID = Convert.ToInt32(ID.Replace("#", ""));
            t.Subject = Subject;
            t.Notes = Notes;
            t.Solution = Solution;
            t.Status = Status;
            t.alarm = Alarm;
        }
    }

    public class JsonController
    {
        internal static void ReadJson()
        {
            try
            {
                var ticketLines = Properties.Settings.Default.Json.Split('µ');
                string[][] individualLine = new string[ticketLines.Length][];
                for (int i = 0; i < ticketLines.Length - 1; i++)
                {
                    individualLine[i] = ticketLines[i].Split('¼');
                }
                for (int i = 0; i < individualLine.Length - 1; i++)
                {
                    Ticket.ticketArray.Add(new Ticket(Convert.ToInt32(individualLine[i][0]), individualLine[i][1], individualLine[i][2], individualLine[i][3], individualLine[i][4], Convert.ToDateTime(individualLine[i][5])));
                }
            }
            catch
            {
                try //Just for Date Update
                {
                    var ticketLines = Properties.Settings.Default.Json.Split('µ');
                    string[][] individualLine = new string[ticketLines.Length][];
                    for (int i = 0; i < ticketLines.Length - 1; i++)
                    {
                        individualLine[i] = ticketLines[i].Split('¼');
                    }
                    for (int i = 0; i < individualLine.Length - 1; i++)
                    {
                        Ticket.ticketArray.Add(new Ticket(Convert.ToInt32(individualLine[i][0]), individualLine[i][1], individualLine[i][2], individualLine[i][3], individualLine[i][4], Convert.ToDateTime("1753/1/1")));
                    }
                }
                catch {
                    var test = MessageBox.Show("It appears that your ticket file is corrupt. Would you like me to delete it?", "Error: Corruption!", MessageBoxButtons.YesNo);
                    if (test == DialogResult.Yes)
                    {
                        Properties.Settings.Default.Json = "";
                        Ticket.ticketArray.Clear();
                    }
                }
            }
        }

        internal static void SaveJson()
        {
            Properties.Settings.Default.Json = "";
            foreach (Ticket t in Ticket.ticketArray)
            {
                Properties.Settings.Default.Json += t.ID + "¼" + t.Subject + "¼" + t.Notes + "¼" + t.Solution + "¼" + t.Status + "¼" + t.alarm + "µ";
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
                    writer.WriteLine(Encrypt.EncryptString(t.ID + "¼" + t.Subject + "¼" + t.Notes + "¼" + t.Solution + "¼" + t.Status + "¼" + t.alarm + "µ", "Password"));
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
                if (Ticket.ticketArray.Count >= 1)
                {
                    var test = MessageBox.Show("Would you like to add on to your tickets? If not I would delete all existing tickets.", "Warning!", MessageBoxButtons.YesNo);
                    if (test == DialogResult.No)
                    {
                        Properties.Settings.Default.Json = "";
                        Ticket.ticketArray.Clear();
                    }
                }
                using (StreamReader sr = new StreamReader(path))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        Properties.Settings.Default.Json += Encrypt.DecryptString(line, "Password");
                    }
                    if(Properties.Settings.Default.Json.Contains("¼") && Properties.Settings.Default.Json.Contains("µ"))
                        JsonController.ReadJson();
                    else{
                        MessageBox.Show("File is corrupted!", "Error: Corruption", MessageBoxButtons.OK);
                        Properties.Settings.Default.Json = "";
                        JsonController.ReadJson();
                    }
                }
            }
        }
    }

    public static class Encrypt
    {
        // This size of the IV (in bytes) must = (keysize / 8).  Default keysize is 256, so the IV must be
        // 32 bytes long.  Using a 16 character string here gives us 32 bytes when converted to a byte array.
        private const string initVector = "pemgail9uzpgzl88";
        // This constant is used to determine the keysize of the encryption algorithm
        private const int keysize = 256;
        //Encrypt
        public static string EncryptString(string plainText, string passPhrase)
        {
            byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] cipherTextBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            return Convert.ToBase64String(cipherTextBytes);
        }
        //Decrypt
        public static string DecryptString(string cipherText, string passPhrase)
        {
            byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];
            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
        }
    }

    public class AlarmClock
    {
        public AlarmClock(DateTime alarmTime, Ticket _i)
        {
            this.alarmTime = alarmTime;

            timer = new System.Timers.Timer();
            timer.Elapsed += timer_Elapsed;
            timer.Interval = 1000;
            timer.Start();
            i = _i;

            enabled = true;
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (enabled && DateTime.Now > alarmTime && alarmTime != new DateTime(1753, 1, 1))
            {
                enabled = false;
                OnAlarm();
                i.alarm = new DateTime(1753, 1, 1);

                timer.Stop();
            }
        }

        protected virtual void OnAlarm()
        {
            if (alarmEvent != null)
                alarmEvent(this, EventArgs.Empty);
        }


        public event EventHandler Alarm
        {
            add { alarmEvent += value; }
            remove { alarmEvent -= value; }
        }

        private EventHandler alarmEvent;
        private System.Timers.Timer timer;
        private DateTime alarmTime;
        private bool enabled;
        private Ticket i;
    }
}
