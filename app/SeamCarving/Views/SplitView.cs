using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Windows.Forms.DataVisualization.Charting;

namespace SeamCarving.Views
{
    public partial class SplitView : UserControl, IView
    {
        #region Variables
        private Bitmap bitmap = null;
        public Bitmap[] channels = new Bitmap[3];
        private Rectangle[] rectangles = new Rectangle[4];
        private double zoom = 0.2;
        private Chart[] charts = new Chart[4];

        private int selectedChannel = -1;
        #endregion

        #region Properties

        public Bitmap ResizedBitmap
        {
            get
            {
                return this.channels[0];
            }
        }

        public Bitmap Bitmap
        {
            get
            {
                return this.bitmap;
            }

            set
            {
                if (value == null) return;
                this.AutoScrollMinSize = new Size((int)(value.Width * zoom), (int)(value.Height * zoom));
                this.bitmap = value;
                this.channels = this.Strategy.generateImages(this.Bitmap);
                this.OnResize(null);
            }
        }
        #endregion
        //unused
        public double Zoom { get; set; }

        private splitViewStrategy strategy;

        public splitViewStrategy Strategy
        {
            get
            {
                return this.strategy;
            }

            set
            {
                this.strategy = value;       
                this.OnResize(null);
            }
        }


        public new void BringToFront()
        {
            base.BringToFront();
        }

        public SplitView()
        {
            this.Dock = DockStyle.Fill;
            this.AutoScroll = true;
            this.AutoSize = true;
            InitializeComponent();            
        }

     
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Invalidate();
        }

        private void SetRectangles()
        {
            if (this.Bitmap != null)
            {
                int width = (this.ClientSize.Width - this.AutoScrollPosition.X) / 2;
                int height = (this.ClientSize.Height - this.AutoScrollPosition.Y) / 2;

                double scale = Math.Min((double)width / (double)this.Bitmap.Width, (double)height / (double)this.Bitmap.Height);

                int imgWidth = (int)(this.Bitmap.Width * scale);
                int imgHeight = (int)(this.Bitmap.Height * scale);
                int wOffset = (int)((width - imgWidth) / 2.0);
                int hOffset = (int)((height - imgHeight) / 2.0);
                int wStart = this.AutoScrollPosition.X + wOffset;
                int hStart = this.AutoScrollPosition.Y;

                this.rectangles[0] = new Rectangle(wStart, hStart, imgWidth, imgHeight);
                this.rectangles[1] = new Rectangle(wStart + width, hStart, imgWidth, imgHeight);
                this.rectangles[2] = new Rectangle(wStart, hStart + height + 2 * hOffset, imgWidth, imgHeight);
                this.rectangles[3] = new Rectangle(wStart + width, hStart + height + 2 * hOffset, imgWidth, imgHeight);
            }
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            var g = pevent.Graphics;

            if (this.Bitmap != null)
            {
                this.SetRectangles();


                int width = (this.ClientSize.Width - this.AutoScrollPosition.X) / 2;
                int height = (this.ClientSize.Height - this.AutoScrollPosition.Y) / 2;

                double scale = Math.Min((double)width / (double)this.Bitmap.Width, (double)height / (double)this.Bitmap.Height);

                // g.DrawRectangle(new Pen(Color.Green), this.rectangles[1]);
                g.DrawImage(this.Bitmap, this.rectangles[0]);
                g.DrawImage(this.channels[0], this.rectangles[1].X, this.rectangles[1].Y, (float)(this.channels[0].Width * scale), (float)(this.channels[0].Height * scale));
                g.DrawImage(this.channels[1], this.rectangles[2].X, this.rectangles[2].Y, (float)(this.channels[1].Width * scale), (float)(this.channels[1].Height * scale));
                g.DrawImage(this.channels[2], this.rectangles[3].X, this.rectangles[3].Y, (float)(this.channels[2].Width * scale), (float)(this.channels[2].Height * scale));


                if (this.selectedChannel > -1)
                {
                    g.DrawRectangle(new Pen(Color.Red), this.rectangles[this.selectedChannel + 1]);
                }

            }

        }

     
    }
}
