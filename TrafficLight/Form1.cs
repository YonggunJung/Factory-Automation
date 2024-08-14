using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrafficLight
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            Thread thread = new Thread(SignalTower);
            thread.Start();
        }

        void SignalTower()
        {
            int D100 = 0;
            while (true)
            {
                switch (D100)
                {
                    case 0:
                        LampRed.BackColor = Color.Red;
                        LampYellow.BackColor = SystemColors.Control;
                        LampGreen.BackColor = SystemColors.Control;
                        Thread.Sleep(3000);
                        D100 = 100;
                        break;

                    case 100:
                        LampRed.BackColor = SystemColors.Control; 
                        LampYellow.BackColor = Color.Yellow;
                        LampGreen.BackColor = SystemColors.Control;
                        Thread.Sleep(3000);
                        D100 = 200;
                        break;

                    case 200:
                        LampRed.BackColor = SystemColors.Control;
                        LampYellow.BackColor = SystemColors.Control;
                        LampGreen.BackColor = Color.Green;
                        Thread.Sleep(3000);
                        D100 = 0;
                        break;
                }
                
            }
        }
    }
}
