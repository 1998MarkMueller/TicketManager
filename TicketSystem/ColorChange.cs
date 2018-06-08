using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TicketSystem.Properties;

namespace TicketSystem
{
    public partial class ColorChange : Form
    {
        public ColorChange()
        {
            InitializeComponent();
            fillColors();
        }

        void fillColors()
        {
            boxMainBackground.Text = Properties.Settings.Default.FormBack;
            boxMainFore.Text = Properties.Settings.Default.FormFore;
            comboBox1.Text = Properties.Settings.Default.TicketBoxBack;
            comboBox2.Text = Properties.Settings.Default.TicketBoxFore;
            comboBox3.Text = Properties.Settings.Default.AlarmBack;
            comboBox4.Text = Properties.Settings.Default.AlarmFore;
            comboBox5.Text = Properties.Settings.Default.NotesBack;
            comboBox6.Text = Properties.Settings.Default.NotesFore;
            comboBox7.Text = Properties.Settings.Default.SolutionBack;
            comboBox8.Text = Properties.Settings.Default.SolutionFore;
            comboBox9.Text = Properties.Settings.Default.ButtonBack;
            comboBox10.Text = Properties.Settings.Default.ButtonFore;
            comboBox11.Text = Properties.Settings.Default.LabelFore;
            comboBox12.Text = Properties.Settings.Default.StatusBack;
            comboBox13.Text = Properties.Settings.Default.StatusFore;
            foreach (KnownColor c in Enum.GetValues(typeof(KnownColor))) {
                boxMainBackground.Items.Add(c.ToString());
                boxMainFore.Items.Add(c.ToString());
                comboBox1.Items.Add(c.ToString());
                comboBox2.Items.Add(c.ToString());
                comboBox3.Items.Add(c.ToString());
                comboBox4.Items.Add(c.ToString());
                comboBox5.Items.Add(c.ToString());
                comboBox6.Items.Add(c.ToString());
                comboBox7.Items.Add(c.ToString());
                comboBox8.Items.Add(c.ToString());
                comboBox9.Items.Add(c.ToString());
                comboBox10.Items.Add(c.ToString());
                comboBox11.Items.Add(c.ToString());
                comboBox12.Items.Add(c.ToString());
                comboBox13.Items.Add(c.ToString());
            }
        }

        void saveColors()
        {
            if (boxMainBackground.Text != "") try { Properties.Settings.Default.FormBack =  boxMainBackground.Text; } catch { }
            if (boxMainFore.Text != "") try { Properties.Settings.Default.FormFore =  boxMainFore.Text; } catch { }
            if (comboBox1.Text != "") try { Properties.Settings.Default.TicketBoxBack =  comboBox1.Text; } catch { }
            if (comboBox2.Text != "") try { Properties.Settings.Default.TicketBoxFore =  comboBox2.Text; } catch { }
            if (comboBox3.Text != "") try { Properties.Settings.Default.AlarmBack =  comboBox3.Text; } catch { }
            if (comboBox4.Text != "") try { Properties.Settings.Default.AlarmFore =  comboBox4.Text; } catch { }
            if (comboBox5.Text != "") try { Properties.Settings.Default.NotesBack =  comboBox5.Text; } catch { }
            if (comboBox6.Text != "") try { Properties.Settings.Default.NotesFore =  comboBox6.Text; } catch { }
            if (comboBox7.Text != "") try { Properties.Settings.Default.SolutionBack =  comboBox7.Text; } catch { }
            if (comboBox8.Text != "") try { Properties.Settings.Default.SolutionFore =  comboBox8.Text; } catch { }
            if (comboBox9.Text != "") try { Properties.Settings.Default.ButtonBack = comboBox9.Text; } catch { }
            if (comboBox10.Text != "") try { Properties.Settings.Default.ButtonFore = comboBox10.Text; } catch { }
            if (comboBox11.Text != "") try { Properties.Settings.Default.LabelFore = comboBox11.Text; } catch { }
            if (comboBox12.Text != "") try { Properties.Settings.Default.StatusBack = comboBox12.Text; } catch { }
            if (comboBox13.Text != "") try { Properties.Settings.Default.StatusFore = comboBox13.Text; } catch { }
        }

        private void ColorChange_FormClosing(object sender, FormClosingEventArgs e)
        {
            var d = MessageBox.Show("You have unsaved changes. Do you want to save?", "Warning!", MessageBoxButtons.YesNo);
            if(d == DialogResult.Yes)
                saveColors();
        }
    }
}
