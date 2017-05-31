using SeamCarving.Filters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeamCarving.Views.SplitViewStrategies
{
    public class EnergyStrategy : splitViewStrategy
    {

        private SplitView view;
        private CommonControls commonControls;
        private Thread workerThread = null;
        private Form form;


        public EnergyStrategy(SplitView view, CommonControls commonControls, Form form)
        {
            this.view = view;
            this.commonControls = commonControls;
            this.form = form;
        }

        private void RedoChannelsWorker()
        {
            ConvFilters conv = new ConvFilters(null);

            this.commonControls.progress = 0;
            int matrixNumber = 3;
            double step = 100.0 / matrixNumber;
            double progress = 0;
            for (int i = 0; i < matrixNumber; i++)
            {
                Bitmap temp = (Bitmap)this.view.channels[0].Clone();
                CoreFilters.ToGrayscale(temp);

                conv.EdgeDetectConvolution(temp, (short)i);
                this.view.channels[i] = temp;


                progress += step;
                commonControls.progress = (int)(progress);
            }            
            this.form.Enabled = true;
            this.view.Invalidate();
        }

        private void RedoChannels()
        {
            this.form.Enabled = false;
            this.workerThread = new Thread(new ThreadStart(this.RedoChannelsWorker));
            this.workerThread.Start();
        }


        public virtual Bitmap[] generateImages(Bitmap b)
        {
            this.view.channels[0] = (Bitmap)b.Clone();
            Bitmap g = (Bitmap)b.Clone();
            CoreFilters.ToGrayscale(g);
            for (int i = 1; i < 3; i++)
            {
                this.view.channels[i] = (Bitmap)g.Clone();
            }

            this.RedoChannels();

            return this.view.channels;
        }
    }
}
