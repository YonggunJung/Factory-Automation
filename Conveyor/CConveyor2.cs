using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conveyor
{
    internal class CConveyor2 : CDevice
    {
        public CConveyor2()
        {
            carrier = new CCarrier();
        }

        public override void Process()
        {
            if (blsAutoConv == true)
            {
                switch (stepConv)
                {
                    case 0:
                        blsUReq = false;
                        blsLReq = false;
                        blsReady = false;
                        statusCwConv = false;
                        statusCcwConv = false;

                        stepConv = 100;
                        break;

                    case 100:
                        if (blsSensorDetect2 == true)
                            stepConv = 200;
                        else
                        {
                            if (blsTakeIn == true)
                            {
                                stepConv = 110;
                                countConv = 0;
                            }
                        }
                        break;

                    case 110:
                        statusCcwConv = true;
                        if (blsSensorDetect2 == true)
                        {
                            statusCcwConv = false;
                            blsUReq = true;
                            stepConv = 200;
                        }
                        break;

                    case 200:
                        if (blsSensorDetect2 == false)
                            stepConv = 100;
                        else if (blsTrReq)
                        {
                            stepConv = 210;
                            blsReady = true;
                        }
                        break;

                    case 210:
                        if (blsBusy)
                        {
                            blsUReq = true;
                            stepConv = 220;
                        }
                        break;

                    case 220:
                        statusCcwConv = true;
                        if (!blsTrReq && !blsBusy && blsCompt)
                        {
                            blsUReq = false;
                            blsReady = false;
                            statusCcwConv = false;
                            stepConv = 230;
                        }
                        break;

                    case 230:
                        if (!blsCompt)
                            stepConv = 100;
                        break;

                    default:
                        stepConv = 0;
                        break;
                }
            }
            else
                stepConv = 0;

            if (oldStepConv != stepConv)
                Console.WriteLine("Conveyor 2 Step = {0}", stepConv);

            oldStepConv = stepConv;
            blsTakeIn = false;
        }
    }
}
