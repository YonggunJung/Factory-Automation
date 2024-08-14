using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conveyor
{
    internal class CBarcode
    {
        private System.IO.Ports.SerialPort serBarCode;
        private string barcode;         // 바코드 저장 변수
        private bool blsSuccess;        // 바코드를 읽었는지 확인
        public bool IsSuccess
        {
            get { return blsSuccess; }
            set { blsSuccess = value; }
        }

        public CBarcode() 
        {
            serBarCode = new System.IO.Ports.SerialPort();

            this.serBarCode.BaudRate = 9600;
            this.serBarCode.DataBits = 8;
            this.serBarCode.Parity = System.IO.Ports.Parity.None;
            this.serBarCode.PortName = "COM1";
            this.serBarCode.StopBits = System.IO.Ports.StopBits.One;
            this.serBarCode.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.DataReceiveCallback);

            this.serBarCode.Open();
        }
        private void DataReceiveCallback(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            barcode = this.serBarCode.ReadLine();
            blsSuccess = true;
        }

        public void ReadCommand()
        {
            blsSuccess = false;
            serBarCode.WriteLine("<T/>");
        }

        public string GetData()
        {
            return barcode;
        }
    }
}
