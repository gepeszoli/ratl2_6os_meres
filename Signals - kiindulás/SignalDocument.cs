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
        private string myPath;
        public SignalDocument(string name) : base(name)
        {
            SignalValues = new List<SignalValue>();
                for (double i = 1.3; i < 11.0; i += 1.0)
                {
                    SignalValues.Add(new SignalValue(i));
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
            TraceValues();
            this.UpdateAllViews();
        }
        public void TraceValues()
        {
            foreach(SignalValue value in SignalValues)
            {
                Trace.WriteLine(value.ToString()+DateTime.Now.ToString());
            }
        }
    }
}
