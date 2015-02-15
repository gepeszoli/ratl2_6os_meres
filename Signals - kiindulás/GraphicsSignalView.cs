using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
namespace Signals
{
    public partial class GraphicsSignalView : UserControl, IView
    {
        /// <summary>
        /// A dokumentum, melynek adatait a nézet megjeleníti.
        /// TODO: a típusa legyen a Document leszármazottunk.
        /// </summary>
        private Document document;
        public GraphicsSignalView()
        {
            InitializeComponent();
        }
        public GraphicsSignalView(Document document)
        {
            InitializeComponent();
            this.document = document;
        }
        /// <summary>
        /// A View interfész Update műveletének implementációja.
        /// </summary>
        public void Update()
        {
            Invalidate();
        }

        public Document GetDocument()
        {
            return document;
        }

        /// <summary>
        /// A UserControl.Paint felüldefiniálása, ebben rajzolunk.
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            SignalDocument doc = (SignalDocument)this.document;
            int centerH = this.Size.Height / 2;
            int centerW = this.Size.Width / 2;
            if (doc.SignalValues.Count < 1)
            {}
                DateTime start, stop;
                double max, min, interval = 0;
                start = doc.SignalValues.ElementAt(0).TimeStamp;
                stop = doc.SignalValues.ElementAt(0).TimeStamp;
                max = min = doc.SignalValues.ElementAt(0).Value;
                foreach (SignalValue sv in doc.SignalValues)
                {
                    if (sv.TimeStamp.Ticks <= start.Ticks)
                    {
                        start = sv.TimeStamp;
                    }
                    if (sv.TimeStamp.Ticks >= stop.Ticks) 
                    {
                        stop = sv.TimeStamp;
                    }
                    if (sv.Value > max) max = sv.Value;
                    if (sv.Value < min) min = sv.Value;
                }
            TimeSpan duration = stop - start;
            //duration.
            double yrange = 40;// max - min;
            //double xres = Convert.ToDouble( duration.Ticks ) / Convert.ToDouble( this.Size.Width ) ;
            //double yres = 0.0; 
            //if (max > 0 && min > 0)
            //{
            //    yres = yrange / Convert.ToDouble(centerH);
            //} 
            //else if( max <0 && min < 0 )
            //{
            //    yres = yrange / Convert.ToDouble(centerH) * -1.0;
            //}
            //else 
            //{
            //    yres = max / Convert.ToDouble(centerH);
            //}
            double xres = 1.0;
            double yres = 1.0;
            using (Pen pen = new Pen(Color.FromArgb(150, Color.Blue)))
            {
                e.Graphics.DrawLine(pen, new Point(0,centerH),new Point(this.Size.Width,centerH) ); // x axis
                e.Graphics.DrawLine(pen, new Point(0, 0), new Point(0, this.Size.Height)); // y axis
            }
            using (SolidBrush brush = new SolidBrush(Color.Green))
            {
                foreach (SignalValue sv in doc.SignalValues)
                {
                    TimeSpan ts = sv.TimeStamp - start;
                    long xpos = Convert.ToInt32( xres* ts.Milliseconds );
                    long ypos = Convert.ToInt32( yres * sv.Value);
//                    Trace.WriteLine("Value {0} {1}", ypos);
                    e.Graphics.FillRectangle(brush, xpos, ypos, 3, 3); 
                }
            }
        }

    }
}
