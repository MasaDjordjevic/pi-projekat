using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeamCarving.Views
{
    public partial class SimpleImageView : UserControl, IView
    {

        #region Variables
        private Bitmap bitmap = null;
        #endregion
        public string SelectedChannel { get; set; }

        public double Zoom { get; set; }
        #region Properties
        public Bitmap Bitmap
        {
            get
            {
                return this.bitmap;
            }

            set
            {
                if (value == null) return;
                this.AutoScrollMinSize = new Size((int)(value.Width * Zoom), (int)(value.Height * Zoom));
                this.bitmap = value;
                base.Invalidate();
            }
        }
        #endregion

        public new void BringToFront()
        {
            base.BringToFront();
        }

        public SimpleImageView()
        {
            this.Dock = DockStyle.Fill;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.Zoom = 1;
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            var g = pevent.Graphics;
            if (this.Bitmap != null)
            {
                g.DrawImage(this.Bitmap, new Rectangle(this.AutoScrollPosition.X, this.AutoScrollPosition.Y, (int)(this.Bitmap.Width * Zoom), (int)(this.Bitmap.Height * Zoom)));
            }

        }
    }
}
