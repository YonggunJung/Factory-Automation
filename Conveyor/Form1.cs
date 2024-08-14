using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Conveyor
{
    public partial class Form1 : Form
    {
        bool blsSimulatorMode = true;

        CConveyor1 conveyor1;
        CConveyor2 conveyor2;
        CConveyor3 conveyor3;
        CConveyor4 conveyor4;
        CConveyorS conveyorS;

        CBarcode barcode;
        public Form1()
        {
            InitializeComponent();
            conveyor1 = new CConveyor1();
            conveyor2 = new CConveyor2();
            conveyor3 = new CConveyor3();
            conveyor4 = new CConveyor4();
            conveyorS = new CConveyorS();

            barcode = new CBarcode();
        }
                
        private void btnAuto_Click(object sender, EventArgs e)
        {
            conveyor1.auto = true;
            conveyor2.auto = true;
            conveyor3.auto = true;
            conveyor4.auto = true;
            conveyorS.auto = true;
        }

        private void btnTakeOut_Click(object sender, EventArgs e)
        {
            if (selectedConv == CONV_TYPE.CONV3)
            {
                if(conveyor3.carrier.id != 0)
                {
                    conveyor3.carrier.use = CCarrier.USE.USE_TAKEOUT;
                }
            }
            if (selectedConv == CONV_TYPE.CONV4)
            {
                if (conveyor4.carrier.id != 0)
                {
                    conveyor4.carrier.use = CCarrier.USE.USE_TAKEOUT;
                }
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            conveyor1.auto = false;
            conveyor2.auto = false;
            conveyor3.auto = false;
            conveyor4.auto = false;
            conveyorS.auto = false;

            barcode.ReadCommand();
        }

        private void btnTakeIn_Click(object sender, EventArgs e)
        {
            // 입고 버튼을 누르면 id와 정보를 넣어줌
            if (conveyor2.carrier.id == 0)      // 캐리어 정보가 없을때만 넣어줌
            {
                conveyor2.carrier.id = 1;       //숫자가 계속 올라가야 하지만 일단 이렇게 함
                conveyor2.carrier.source = 2;   // 출발 지점은 2번 컨베이어
                conveyor2.carrier.use = CCarrier.USE.USE_STACK;  // 저장하게 될 자제 정보

                conveyor2.takeIn = true;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("입고 버튼 클릭 됨!");
                Console.ResetColor();
            }
            
        }

        bool ConvMotionBlink;
        private void ConvMotionProc_Tick(object sender, EventArgs e)
        {
            if(ConvMotionBlink == true)
            {
                btnConveyor1.Text = "";
                btnConveyor2.Text = "";
                btnConveyor3.Text = "";
                btnConveyor4.Text = "";
                btnConveyorS.Text = "";
                ConvMotionBlink = false;
            }
            else
            {
                if (!conveyor1.statusCw && !conveyor1.statusCcw) btnConveyor1.Text = "";
                else if (conveyor1.statusCw) btnConveyor1.Text = "CW";
                else if (conveyor1.statusCcw) btnConveyor1.Text = "CCW";
                if (!conveyor2.statusCw && !conveyor2.statusCcw) btnConveyor2.Text = "";
                else if (conveyor2.statusCw) btnConveyor2.Text = "CW";
                else if (conveyor2.statusCcw) btnConveyor2.Text = "CCW";
                if (!conveyor3.statusCw && !conveyor3.statusCcw) btnConveyor3.Text = "";
                else if (conveyor3.statusCw) btnConveyor3.Text = "CW";
                else if (conveyor3.statusCcw) btnConveyor3.Text = "CCW";
                if (!conveyor4.statusCw && !conveyor4.statusCcw) btnConveyor4.Text = "";
                else if (conveyor4.statusCw) btnConveyor4.Text = "CW";
                else if (conveyor4.statusCcw) btnConveyor4.Text = "CCW";
                if (!conveyorS.statusCw && !conveyorS.statusCcw) btnConveyorS.Text = "";
                else if (conveyorS.statusCw) btnConveyorS.Text = "CW";
                else if (conveyorS.statusCcw) btnConveyorS.Text = "CCW";

                ConvMotionBlink = true;
            }
        }

        //int stepConv1, stepConv2, stepConv3, stepConv4, stepConvS;
        //int oldStepConv1, oldStepConv2, oldStepConv3, oldStepConv4, oldStepConvS;
        //int countConv1, countConv2, countConv3, countConv4, countConvS;

        //bool blsAutoConv1, blsAutoConv2, blsAutoConv3, blsAutoConv4, blsAutoConvS;
        //bool blsTakeIn, blsTakeOut;
        private void MainSchedulerProc_Tick(object sender, EventArgs e)
        {
            conveyor1.Process();
            conveyor2.Process();
            conveyor3.Process();
            conveyor4.Process();
            conveyorS.Process();

            PioExchanger();
            DataExchanger();
            MotionControl();
            Simulator();

            if(barcode.IsSuccess == true)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine(barcode.GetData());
                Console.ResetColor();
                barcode.IsSuccess = false;
            }
        }

        UInt16[] ConvTimer = new UInt16[5];
        private void Simulator()
        {
            if (blsSimulatorMode)
            {
                // 컨베이어 1번
                if (conveyor1.carrier.id != 0 && conveyor1.statusCw)
                {
                    ConvTimer[0]++;
                    if (ConvTimer[0] > 5)
                    {
                        cbSensor1_1.Checked = true;
                        ConvTimer[0] = 0;
                    }
                }
                else
                {
                    if (conveyor1.carrier.id == 0)
                    {
                        cbSensor1_1.Checked = false;
                        cbSensor1_2.Checked = false;
                    }
                    ConvTimer[0] = 0;
                }

                // 컨베이어 2번
                if (conveyor2.carrier.id != 0 && conveyor2.statusCcw)
                {
                    ConvTimer[1]++;
                    if (ConvTimer[1] > 5)
                    {
                        cbSensor2_2.Checked = true;
                        ConvTimer[1] = 0;
                    }
                }
                else
                {
                    if (conveyor2.carrier.id == 0)
                    {
                        cbSensor2_2.Checked = false;
                    }
                    ConvTimer[1] = 0;
                }

                // 컨베이어 3번
                if (conveyor3.carrier.id != 0 && conveyor3.statusCcw)
                {
                    ConvTimer[2]++;
                    if (ConvTimer[2] > 5)
                    {
                        cbSensor3_2.Checked = true;
                        ConvTimer[2] = 0;
                    }
                }
                else
                {
                    if (conveyor3.carrier.id == 0)
                    {
                        cbSensor3_2.Checked = false;
                    }
                    ConvTimer[2] = 0;
                }

                // 컨베이어 4번
                if (conveyor4.carrier.id != 0 && conveyor4.statusCcw)
                {
                    ConvTimer[3]++;
                    if (ConvTimer[3] > 5)
                    {
                        cbSensor4_2.Checked = true;
                        ConvTimer[3] = 0;
                    }
                }
                else
                {
                    if (conveyor4.carrier.id == 0)
                    {
                        cbSensor4_2.Checked = false;
                    }
                    ConvTimer[3] = 0;
                }

                //셔틀 컨베이어
                if (conveyorS.carrier.id == 0)
                {
                    if (conveyorS.statusCcw)
                    {
                        ConvTimer[4]++;
                        if (ConvTimer[4] > 5)
                        {
                            cbSensorS_2.Checked = true;
                            ConvTimer[4] = 0;
                        }
                    }
                    else if (conveyorS.statusCw)
                    {
                        ConvTimer[4]++;
                        if (ConvTimer[4] > 5)
                        {
                            cbSensorS_1.Checked = true;
                            ConvTimer[4] = 0;
                        }
                    }
                }
            }
            
        }

        private void DataExchanger()
        {   //컨베이어들 간 데이터를 옮겨주는 용도
            // 1번 컨베이어
            if (conveyor1.Compt)        // Compt 신호로 들어 왔을 때
            {
                if (conveyor1.LReq) // 이 신호와
                {
                    if (conveyorS.carrier.id != 0)
                    {
                        conveyor1.carrier.id = conveyorS.carrier.id;
                        conveyor1.carrier.source = conveyorS.carrier.source;
                        conveyor1.carrier.dest = conveyorS.carrier.dest;
                        conveyor1.carrier.use = conveyorS.carrier.use;
                        conveyorS.carrier.Clear();

                        Console.WriteLine("Carrier Data Move : S -> 1");
                    }
                }
                if (conveyor1.UReq) // 이 신호로 구분 해서 데이터 옮김
                {
                    if (conveyor1.carrier.id != 0)
                    {
                        conveyorS.carrier.id = conveyor1.carrier.id;
                        conveyorS.carrier.source = conveyor1.carrier.source;
                        conveyorS.carrier.dest = conveyor1.carrier.dest;
                        conveyorS.carrier.use = conveyor1.carrier.use;
                        conveyor1.carrier.Clear();

                        Console.WriteLine("Carrier Data Move : 1 -> S");
                    }
                }
            }

            // 2번 컨베이어
            if (conveyor2.Compt)        // Compt 신호로 들어 왔을 때
            {
                if (conveyor2.LReq) // 이 신호와
                {
                    if (conveyorS.carrier.id != 0)
                    {
                        conveyor2.carrier.id = conveyorS.carrier.id;
                        conveyor2.carrier.source = conveyorS.carrier.source;
                        conveyor2.carrier.dest = conveyorS.carrier.dest;
                        conveyor2.carrier.use = conveyorS.carrier.use;
                        conveyorS.carrier.Clear();

                        Console.WriteLine("Carrier Data Move : S -> 2");
                    }
                }
                if (conveyor2.UReq) // 이 신호로 구분 해서 데이터 옮김
                {
                    if (conveyor2.carrier.id != 0)
                    {
                        conveyorS.carrier.id = conveyor2.carrier.id;
                        conveyorS.carrier.source = conveyor2.carrier.source;
                        conveyorS.carrier.dest = conveyor2.carrier.dest;
                        conveyorS.carrier.use = conveyor2.carrier.use;
                        conveyor2.carrier.Clear();

                        Console.WriteLine("Carrier Data Move : 2 -> S");
                    }
                }
            }

            // 3번 컨베이어
            if (conveyor3.Compt)        // Compt 신호로 들어 왔을 때
            {
                if (conveyor3.LReq) // 이 신호와
                {
                    if (conveyorS.carrier.id != 0)
                    {
                        conveyor3.carrier.id = conveyorS.carrier.id;
                        conveyor3.carrier.source = conveyorS.carrier.source;
                        conveyor3.carrier.dest = conveyorS.carrier.dest;
                        conveyor3.carrier.use = conveyorS.carrier.use;
                        conveyorS.carrier.Clear();

                        Console.WriteLine("Carrier Data Move : S -> 3");
                    }
                }
                if (conveyor3.UReq) // 이 신호로 구분 해서 데이터 옮김
                {
                    if (conveyor3.carrier.id != 0)
                    {
                        conveyorS.carrier.id = conveyor3.carrier.id;
                        conveyorS.carrier.source = conveyor3.carrier.source;
                        conveyorS.carrier.dest = conveyor3.carrier.dest;
                        conveyorS.carrier.use = conveyor3.carrier.use;
                        conveyor3.carrier.Clear();

                        Console.WriteLine("Carrier Data Move : 3 -> S");
                    }
                }
            }

            // 4번 컨베이어
            if (conveyor4.Compt)        // Compt 신호로 들어 왔을 때
            {
                if (conveyor4.LReq) // 이 신호와
                {
                    if (conveyorS.carrier.id != 0)
                    {
                        conveyor4.carrier.id = conveyorS.carrier.id;
                        conveyor4.carrier.source = conveyorS.carrier.source;
                        conveyor4.carrier.dest = conveyorS.carrier.dest;
                        conveyor4.carrier.use = conveyorS.carrier.use;
                        conveyorS.carrier.Clear();

                        Console.WriteLine("Carrier Data Move : S -> 4");
                    }
                }
                if (conveyor4.UReq) // 이 신호로 구분 해서 데이터 옮김
                {
                    if (conveyor4.carrier.id != 0)
                    {
                        conveyorS.carrier.id = conveyor4.carrier.id;
                        conveyorS.carrier.source = conveyor4.carrier.source;
                        conveyorS.carrier.dest = conveyor4.carrier.dest;
                        conveyorS.carrier.use = conveyor4.carrier.use;
                        conveyor4.carrier.Clear();

                        Console.WriteLine("Carrier Data Move : 4 -> S");
                    }
                }
            }
        }

        private void MotionControl()
        {
            switch (conveyorS.TargetPosition)
            {
                case CConveyorS.SERVO_POS.CONV_NONE:
                    btnConveyorS.Location = new Point(220, 153);
                    cbSensorS_1.Location = new Point(99, 240);
                    cbSensorS_2.Location = new Point(99, 160);
                    break;

                case CConveyorS.SERVO_POS.CONV1:
                    btnConveyorS.Location = new Point(100, 160);
                    cbSensorS_1.Location = new Point(51, 240);
                    cbSensorS_2.Location = new Point(51, 160);
                    break;

                case CConveyorS.SERVO_POS.CONV2:
                    btnConveyorS.Location = new Point(354, 160);
                    cbSensorS_1.Location = new Point(300, 240);
                    cbSensorS_2.Location = new Point(300, 160);
                    break;

                case CConveyorS.SERVO_POS.CONV3:
                    btnConveyorS.Location = new Point(100, 160);
                    cbSensorS_1.Location = new Point(51, 240);
                    cbSensorS_2.Location = new Point(51, 160);
                    break;

                case CConveyorS.SERVO_POS.CONV4:
                    btnConveyorS.Location = new Point(354, 160);
                    cbSensorS_1.Location = new Point(300, 240);
                    cbSensorS_2.Location = new Point(300, 160);
                    break;

                default:
                    btnConveyorS.Location = new Point(220, 153);
                    cbSensorS_1.Location = new Point(99, 240);
                    cbSensorS_2.Location = new Point(99, 160);
                    break;
            }
            conveyorS.CurrentPosition = conveyorS.TargetPosition;
        }

        private void PioExchanger()
        {
            conveyor1.TrReq = conveyorS.ConvPio1.TrReq;
            conveyor1.Busy = conveyorS.ConvPio1.Busy;
            conveyor1.Compt = conveyorS.ConvPio1.Compt;
            conveyorS.ConvPio1.LReq = conveyor1.LReq;
            conveyorS.ConvPio1.UReq = conveyor1.UReq;
            conveyorS.ConvPio1.Ready = conveyor1.Ready;

            conveyor2.TrReq = conveyorS.ConvPio2.TrReq;
            conveyor2.Busy = conveyorS.ConvPio2.Busy;
            conveyor2.Compt = conveyorS.ConvPio2.Compt;
            conveyorS.ConvPio2.LReq = conveyor2.LReq;
            conveyorS.ConvPio2.UReq = conveyor2.UReq;
            conveyorS.ConvPio2.Ready = conveyor2.Ready;

            conveyor3.TrReq = conveyorS.ConvPio3.TrReq;
            conveyor3.Busy = conveyorS.ConvPio3.Busy;
            conveyor3.Compt = conveyorS.ConvPio3.Compt;
            conveyorS.ConvPio3.LReq = conveyor3.LReq;
            conveyorS.ConvPio3.UReq = conveyor3.UReq;
            conveyorS.ConvPio3.Ready = conveyor3.Ready;

            conveyor4.TrReq = conveyorS.ConvPio4.TrReq;
            conveyor4.Busy = conveyorS.ConvPio4.Busy;
            conveyor4.Compt = conveyorS.ConvPio4.Compt;
            conveyorS.ConvPio4.LReq = conveyor4.LReq;
            conveyorS.ConvPio4.UReq = conveyor4.UReq;
            conveyorS.ConvPio4.Ready = conveyor4.Ready;
        }

        private void cbSensor1_1_CheckedChanged(object sender, EventArgs e)
        {
            if (cbSensor1_1.Checked)
                conveyor1.sensor1 = true;
            else conveyor1.sensor1 = false;
        }

        private void cbSensor1_2_CheckedChanged(object sender, EventArgs e)
        {
            if (cbSensor1_2.Checked)
                conveyor1.sensor2 = true;
            else conveyor1.sensor2 = false;
        }

        private void cbSensor2_1_CheckedChanged(object sender, EventArgs e)
        {
            if (cbSensor2_1.Checked)
                conveyor2.sensor1 = true;
            else conveyor2.sensor1 = false;
        }

        private void cbSensor2_2_CheckedChanged(object sender, EventArgs e)
        {
            if (cbSensor2_2.Checked)
                conveyor2.sensor2 = true;
            else conveyor2.sensor2 = false;
        }

        private void cbSensor3_1_CheckedChanged(object sender, EventArgs e)
        {
            if (cbSensor3_1.Checked)
                conveyor3.sensor1 = true;
            else conveyor3.sensor1 = false;
        }

        private void cbSensor3_2_CheckedChanged(object sender, EventArgs e)
        {
            if (cbSensor3_2.Checked)
                conveyor3.sensor2 = true;
            else conveyor3.sensor2 = false;
        }

        private void cbSensor4_1_CheckedChanged(object sender, EventArgs e)
        {
            if (cbSensor4_1.Checked)
                conveyor4.sensor1 = true;
            else conveyor4.sensor1 = false;
        }

        private void cbSensor4_2_CheckedChanged(object sender, EventArgs e)
        {
            if (cbSensor4_2.Checked)
                conveyor4.sensor2 = true;
            else conveyor4.sensor2 = false;
        }

        private void cbSensorS_1_CheckedChanged(object sender, EventArgs e)
        {
            if (cbSensorS_1.Checked)
                conveyorS.sensor1 = true;
            else conveyorS.sensor1 = false;
        }

        private void cbSensorS_2_CheckedChanged(object sender, EventArgs e)
        {
            if (cbSensorS_2.Checked)
                conveyorS.sensor2 = true;
            else conveyorS.sensor2 = false;
        }

        private enum CONV_TYPE
        {
            CONV_NONE,
            CONV1,
            CONV2,
            CONV3,
            CONV4,
            CONVS
        }

        private CONV_TYPE selectedConv;

        private void SelectConv(CONV_TYPE select)
        {   //클린된 컨베이어 색 변경
            btnConveyor1.BackColor = Color.Lime;
            btnConveyor2.BackColor = Color.Lime;
            btnConveyor3.BackColor = Color.Lime;
            btnConveyor4.BackColor = Color.Lime;
            btnConveyorS.BackColor = Color.Lime;
            if (select == CONV_TYPE.CONV1) btnConveyor1.BackColor = Color.Azure;
            if (select == CONV_TYPE.CONV2) btnConveyor2.BackColor = Color.Azure;
            if (select == CONV_TYPE.CONV3) btnConveyor3.BackColor = Color.Azure;
            if (select == CONV_TYPE.CONV4) btnConveyor4.BackColor = Color.Azure;
            if (select == CONV_TYPE.CONVS) btnConveyorS.BackColor = Color.Azure;
        }
        private void btnConveyor1_Click(object sender, EventArgs e)
        {
            selectedConv  = CONV_TYPE.CONV1;
            SelectConv(selectedConv);
        }

        private void btnConveyor2_Click(object sender, EventArgs e)
        {
            selectedConv = CONV_TYPE.CONV2;
            SelectConv(selectedConv);
        }

        private void btnConveyor3_Click(object sender, EventArgs e)
        {
            selectedConv = CONV_TYPE.CONV3;
            SelectConv(selectedConv);
        }

        private void btnConveyor4_Click(object sender, EventArgs e)
        {
            selectedConv = CONV_TYPE.CONV4;
            SelectConv(selectedConv);
        }

        private void btnConveyorS_Click(object sender, EventArgs e)
        {
            selectedConv = CONV_TYPE.CONVS;
            SelectConv(selectedConv);
        }

        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {

        }
    }
}
