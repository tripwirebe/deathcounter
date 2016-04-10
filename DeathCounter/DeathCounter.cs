using System;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.IO;
using System.Runtime.InteropServices;
using DeathCounter.Lib;

namespace DeathCounter
{
    public partial class frmDeathCounter : Form
    {
        private int counter = 0;
        public const string APPLICATIONNAME = "DeathCounter";
        const int PLUSHOTKEYID = 1;
        const int SUBHOTKEYID = 2;
        private string DeathCounterFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DeathCounter\\Deaths.txt");
        private IDeathInputOutHandler output;
        public int Counter
        {
            get { return counter; }
            set
            {
                if (value >= 0)
                {
                    counter = value;
                    this.lblCounter.Text = this.Counter.ToString();
                    this.output.UpdateDeaths(this.counter);
                }
            }
        }
        public frmDeathCounter()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.output = new DeathInputOutputFileHandler();
            this.output.Init(APPLICATIONNAME, "deaths");
            this.Counter = this.output.loadPreviousDeaths();
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
            if (int.TryParse(counterSet, out this.counter))
            {
                this.Counter = this.Counter;
            }
            else
            {
                MessageBox.Show("Invalid value");
            }
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
        private void frmDeathCounter_FormClosing(object sender, FormClosingEventArgs e)
        {
            UnregisterHotKey(this.Handle, 1);
            UnregisterHotKey(this.Handle, 2);
        }
    }
}
