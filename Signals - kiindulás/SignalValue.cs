using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Signals
{
    class SignalValue : IComparable
    {
        public double Value;
        public DateTime TimeStamp;
        public SignalValue (double value)
        {
            Value = value;
            TimeStamp = new DateTime(DateTime.Now.Ticks, DateTimeKind.Utc) ;
        }
        public SignalValue(double value, DateTime timeStamp)
        {
            Value = value;
            timeStamp = timeStamp.ToUniversalTime();
            TimeStamp = timeStamp;
        }
        public override string ToString()
        {
            return base.ToString();
        }
        public int CompareTo(object obj)
        {
            if( obj is SignalValue )
            {
                SignalValue tmp = (SignalValue)obj;
                if (tmp.TimeStamp.Ticks > this.TimeStamp.Ticks)
                {
                    return -1;
                }
                else if (tmp.TimeStamp.Ticks < this.TimeStamp.Ticks)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }

            }
            else
            {
                throw new InvalidCastException("Obj is not Signalvalue");
            }
            return 0;
        }
    }
}
