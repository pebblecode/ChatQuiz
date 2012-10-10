using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CampfireHoon
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Join();
        }

        private readonly string accountName ="pebbleit";
        private readonly string authToken = "fee994663c634db07ea450f0b1de0cdbbc583d61";
        private readonly int roomId = 536178;
        private readonly bool isHttps = true;

        public void Join()
        {
            PostTo("join", (req) => { });
        }


    }
}
