using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.IO;
using System.Runtime.InteropServices;

namespace DeathCounter
{
    public partial class frmDeathCounter : Form
    {
        private int counter = 0;
        public const string APPLICATIONNAME = "DeathCounter";
        const int PLUSHOTKEYID = 1;
        const int SUBHOTKEYID = 2;
        private string DeathCounterFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DeathCounter\\Deaths.txt");
        public int Counter
        {
            get { return counter; }
            set
            {
                if (value >= 0)
                {
                    counter = value;
                    this.lblCounter.Text = this.Counter.ToString();
                    this.UpdateDeaths(this.counter);
                }
            }
        }
        public frmDeathCounter()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.Counter = this.loadPreviousDeaths();
            this.lblCounter.Text = this.Counter.ToString();
            RegisterHotKey(this.Handle, 1, 0, (int)Keys.PageUp);
            RegisterHotKey(this.Handle, 2, 0, (int)Keys.PageDown);
        }
        private void btnUp_Click(object sender, EventArgs e)
        {
            this.Counter++;
        }
        private void btnDown_Click(object sender, EventArgs e)
        {
            this.Counter--;
        }
        private void btnSet_Click(object sender, EventArgs e)
        {
            string counterSet = Interaction.InputBox("Inital value", "what is the inital value you would want to set?");
            if (!int.TryParse(counterSet, out this.counter))
            {
                MessageBox.Show("Invalid value");
            }
            else
            {
                this.lblCounter.Text = this.Counter.ToString();
            }
        }
        private bool CreateAppdataFolder()
        {
            bool result = false;
            if (!Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), APPLICATIONNAME)))
            {
                Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), APPLICATIONNAME));
            }
            result = Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), APPLICATIONNAME));
            return result;
        }
        private void UpdateDeaths(int numberOfDeaths)
        {
            if (CreateAppdataFolder())
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(this.DeathCounterFile, false))
                {
                    file.WriteLine(numberOfDeaths.ToString());
                    file.Close();
                }
            }
        }
        private int loadPreviousDeaths()
        {
            int deaths = 0;
            if (File.Exists(this.DeathCounterFile))
            {
                using (System.IO.StreamReader file = new System.IO.StreamReader(this.DeathCounterFile, false))
                {
                    string deathLine = file.ReadLine();
                    if (!Int32.TryParse(deathLine, out deaths))
                    {
                        deaths = 0;
                    }
                    file.Close();
                }
            }
            return deaths;
        }
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0312 && m.WParam.ToInt32() == PLUSHOTKEYID)
            {
                this.Counter++;
            }
            if (m.Msg == 0x0312 && m.WParam.ToInt32() == SUBHOTKEYID)
            {
                this.Counter--;
            }
            base.WndProc(ref m);
        }
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);
        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);
    }
}
