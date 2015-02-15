using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Signals
{
    class SignalValue
    {
        public double Value;
        public DateTime TimeStamp;
        public long Ticks;
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
            Ticks = TimeStamp.Ticks;
        }
        public override string ToString()
        {
            return base.ToString();
        }
    }
}
