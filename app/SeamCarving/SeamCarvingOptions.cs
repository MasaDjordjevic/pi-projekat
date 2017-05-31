using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeamCarving
{
    public class SeamCarvingOptions
    {
        public ConvolutionMatrix ConvolutionMatrix { get; set; }
        public int PixelsToRemove { get; set; }

        public SeamCarvingOptions()
        {
            this.ConvolutionMatrix = ConvolutionMatrix.kirsh;
            this.PixelsToRemove = 1;
        }
    }
}
