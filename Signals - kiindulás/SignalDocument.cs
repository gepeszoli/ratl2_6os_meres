using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
namespace Signals
{
    class SignalDocument : Document
    {
        public List<SignalValue> SignalValues;

        public SignalDocument(string name) : base(name)
        {
            SignalValues = new List<SignalValue>();
                for (double i = 1.3; i < 110.0; i += 1.0)
                {
                    long ticks = DateTime.Now.Ticks + Convert.ToInt64(i) * 100000 ;
                    SignalValues.Add(new SignalValue(i, new DateTime(ticks)) );
                }
        }
        public override void SaveDocument(string filePath)
        {
            //base.SaveDocument(filePath);
 
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                foreach (SignalValue val in SignalValues)
                {
                    sw.WriteLine("{0}\t{1:o}", val.Value.ToString(), val.TimeStamp );
                }
            }
        }
        public override void LoadDocument(string filePath)
        {
            base.LoadDocument(filePath);
            SignalValues.Clear();
            using( StreamReader sr = new StreamReader( filePath ) )
            {
                while (sr.Peek() >= 0)
                {
                    string line = sr.ReadLine() ;
                    line = line.Trim();
                    string[] columns = line.Split('\t');
                    double d = double.Parse(columns[0]);
                    DateTime dt = DateTime.Parse(columns[1]);
                    dt = dt.ToLocalTime();
                    SignalValues.Add(new SignalValue(d, dt));
                }
            }
            Trace.WriteLine("********* Loaded **************\r\n");
            TraceValues();
            this.UpdateAllViews();
        }
        public void TraceValues()
        {
            foreach(SignalValue v in SignalValues)
            {
                string str ;
                str = v.Value.ToString();
                str += " Timestamp: ";
                str += v.TimeStamp.ToString();
                str += " Ticks: ";
                str += v.TimeStamp.Ticks.ToString();
                Trace.WriteLine(str);
            }
        }
    }
}
