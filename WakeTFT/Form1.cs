using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WakeTFT
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public static UInt32 testi;
        private void Form1_Load(object sender, EventArgs e)
        {
            
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                Engine engine = new Engine();
                engine.Run();
            }).Start();

            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                Visuals v = new Visuals();
                v.Run();
            }).Start();
        }
    }
}
