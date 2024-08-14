using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conveyor
{
    abstract class CDevice : IProcess
    {
        protected bool blsTrReq;
        protected bool blsBusy;
        protected bool blsCompt;
        protected bool blsUReq;
        protected bool blsLReq;
        protected bool blsReady;

        public CCarrier carrier;

        public bool TrReq
        {
            get { return blsTrReq; }
            set { blsTrReq = value; }
        }
        public bool Busy
        {
            get { return blsBusy; }
            set { blsBusy = value; }
        }
        public bool Compt
        {
            get { return blsCompt; }
            set { blsCompt = value; }
        }
        public bool UReq
        {
            get { return blsUReq; }
        }
        public bool LReq
        {
            get { return blsLReq; }
        }
        public bool Ready
        {
            get { return blsReady; }
        }

        protected bool statusCwConv;
        protected bool statusCcwConv;
        public bool statusCw
        {
            get { return statusCwConv; }
            set { statusCwConv = value; }
        }
        public bool statusCcw
        {
            get { return statusCcwConv; }
            set { statusCcwConv = value; }
        }

        protected int stepConv;
        protected int oldStepConv;
        protected int countConv;
        public int step
        {
            get { return stepConv; }
            set { stepConv = value; }
        }
        public int oldStep
        {
            get { return oldStepConv; }
            set { oldStepConv = value; }
        }
        public int count
        {
            get { return countConv; }
            set { countConv = value; }
        }

        protected bool blsAutoConv;
        protected bool blsTakeIn, blsTakeOut;

        public bool auto
        {
            get { return blsAutoConv; }
            set { blsAutoConv = value; }
        }
        public bool takeIn
        {
            get { return blsTakeIn; }
            set { blsTakeIn = value; }
        }
        public bool takeOut
        {
            get { return blsTakeOut; }
            set { blsTakeOut = value; }
        }

        public abstract void Process();

        protected bool blsSensorDetect1, blsSensorDetect2;
        public bool sensor1
        {
            get { return blsSensorDetect1; }
            set { blsSensorDetect1 = value; }
        }
        public bool sensor2
        {
            get { return blsSensorDetect2; }
            set { blsSensorDetect2 = value; }
        }
    }
}
