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
        public double yrange, yres, xrange, xres;
        public bool reCalculate;
        public int xoffset = 10;
        public double boxsize = 3.0;
        public GraphicsSignalView()
        {
            InitializeComponent();
            reCalculate = true;
            this.Invalidate();
        }
        public GraphicsSignalView(Document document)
        {
            InitializeComponent();
            this.document = document;
            reCalculate = true;
            this.Invalidate();
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
            int buttonHeight = this.button1.Height + 5;
            int usableGraphicHeight = this.Size.Height ;
            int usableGraphicWidth = this.Size.Width ;
            int centerH = usableGraphicHeight / 2;
            int centerW = usableGraphicWidth / 2;
            if (doc.SignalValues.Count < 1)
            {}
            DateTime start, stop;
            double max, min, interval = 0;
            doc.SignalValues.Sort(); // sorts by time
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
            if (reCalculate)
            {
                yrange = Math.Abs(max - min);
                xrange = Convert.ToDouble(duration.Ticks);
                Trace.WriteLine("xrange is : {0}", xrange.ToString());
                Trace.WriteLine("yrange is : {0}", yrange.ToString());
                xres = Convert.ToDouble(usableGraphicWidth) / xrange;
                yres = Convert.ToDouble(centerH) / yrange;
                Trace.WriteLine("xres is : {0}", xres.ToString());
                Trace.WriteLine("yres is : {0}", yres.ToString());
                if (yres == 0) throw new ArgumentOutOfRangeException(" Divison by 0");
            }
            using (Pen pen = new Pen(Color.FromArgb(150, Color.Blue)))
            {
                e.Graphics.DrawLine(pen, new Point(xoffset, centerH), new Point(usableGraphicWidth, centerH)); // x axis
                e.Graphics.DrawLine(pen, new Point(xoffset, buttonHeight), new Point(xoffset, usableGraphicHeight)); // y axis
            }
            using (SolidBrush brush = new SolidBrush(Color.Green) )
            {
                label2.Text = "Data is scaled normally";
                label2.ForeColor = Color.Green;
                Point previousPoint = new Point(xoffset, centerH);
                foreach (SignalValue sv in doc.SignalValues)
                {
                    TimeSpan ts = sv.TimeStamp - start;
                    long xpos = Convert.ToInt64(Convert.ToDouble(ts.Ticks) * xres) + xoffset ;
                    long ypos = Convert.ToInt64(centerH - sv.Value * yres) ;
                    if (ypos < buttonHeight)
                    {
                        ypos = buttonHeight;
                        label2.Text = "Warning: the displayed y values have been truncated";
                        label2.ForeColor = Color.Red;
                    }
                    e.Graphics.FillRectangle(brush, xpos - Convert.ToInt64(boxsize / 2), ypos - Convert.ToInt64(boxsize / 2), Convert.ToInt32(boxsize), Convert.ToInt32(boxsize));
                    using( Pen pen = new Pen(Color.FromArgb(150, Color.Red) ) )
                    {
                        e.Graphics.DrawLine(pen, previousPoint.X, previousPoint.Y, xpos, ypos);
                    }
                    previousPoint.X = (int)xpos;
                    previousPoint.Y = (int)ypos;
                }
                toolStripStatusLabel1.Text = "xrange: " + xrange.ToString() + " ticks";
                toolStripStatusLabel1.Text += " yrange: " + yrange.ToString() + " val.";
                toolStripStatusLabel1.Text += " xres: " + xres.ToString() + " pixel/tick";
                toolStripStatusLabel1.Text += " yres: " + yres.ToString() + " pixel/val.";
            }
            
        }

        /// <summary>
        /// This is the zoom + buttons event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            reCalculate = false;
            yres *= 1.1;
            xres *= 1.1;
            boxsize *= 1.1;
            this.Invalidate();
        }
        /// <summary>
        /// This is the event handler of the zoom - button 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            reCalculate = false;
            yres *= 0.9;
            xres *= 0.9;
            boxsize *= 0.9;
            this.Invalidate();
        }
        /// <summary>
        /// This is the event handler of the zoom reset button 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            reCalculate = true;
            this.Invalidate();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

    }
}
