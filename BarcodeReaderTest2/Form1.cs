using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BarcodeReaderTest2
{
    public partial class Form1 : Form
    {
        TcpClient client;
        NetworkStream stream;
        public Form1()
        {
            InitializeComponent();

            client = new TcpClient();
            client.Connect("192.168.100.1", 2000);
            stream = client.GetStream();

            // 쓰레드 기본 문법. 익혀 두면 좋음
            Thread thread = new Thread(GetMessage);
            thread.IsBackground = true;
            thread.Start();
        }
        ~Form1()
        {
            stream.Close();
            client.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            byte[] buffer = Encoding.ASCII.GetBytes("<T/>");
            stream.Write(buffer, 0, buffer.Length);
        }

        private void GetMessage()
        {
            // 혼자 노는 함수.
            while (true)
            {
                int size = client.ReceiveBufferSize;
                byte[] buffer = new byte[size];
                int bytes = stream.Read(buffer, 0, buffer.Length);
                string sendStr = Encoding.ASCII.GetString(buffer, 0, bytes) + "\n";

                BeginInvoke(new WriteTextCallback(WriteText), new object[] { sendStr });
            }
        }

        private void WriteText(string str)
        {
            this.textBox1.AppendText(str);
        }

        delegate void WriteTextCallback(string str);
    }
}
