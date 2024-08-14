using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//바코드 리더기가 필요하지만 없이 그냥 공부함
namespace BarcodeReaderTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            serialPort1.Open();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            serialPort1.WriteLine("<T/>");
        }

        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            //textBox1.AppendText(serialPort1.ReadLine());  //에러남
            //WriteText(serialPort1.ReadLine());            //에러남
            string sendStr = serialPort1.ReadLine() + "\n";
            BeginInvoke(new WriteTextCallback(WriteText), new object[] { sendStr });
        }

        private void WriteText(string str)
        {
            this.textBox1.AppendText(str);
        }

        delegate void WriteTextCallback(string str);
    }
}
