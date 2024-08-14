using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MxComponentTest
{
    public partial class Form1 : Form
    {
        ActUtlType64Lib.ActUtlType64 MxComponent;
        public Form1()
        {
            InitializeComponent();
            MxComponent = new ActUtlType64Lib.ActUtlType64();
        }

        void MxOpen()
        {
            int iReturnCode;

            MxComponent.ActLogicalStationNumber = 0;
            iReturnCode = MxComponent.Open();
        }
        void MxClose()
        {
            int iReturnCode;

            iReturnCode = MxComponent.Close();
        }

        short ReadDeviceOne(string name)
        {
            int iReturnCode;
            short deviceValue;

            iReturnCode = MxComponent.ReadDeviceRandom2(name, 1, out deviceValue);
            return deviceValue;
        }

        void WriteDeviceOne(string name, short value)
        {
            int iReturnCode;

            iReturnCode = MxComponent.WriteDeviceRandom2(name, 1, value);
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            MxOpen();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            MxClose();
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            short retValue;
            retValue = ReadDeviceOne("M100");

            MessageBox.Show(retValue.ToString());
        }

        private void btnWrite_Click(object sender, EventArgs e)
        {
            WriteDeviceOne("M100", 1);
        }
    }
}
