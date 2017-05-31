using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeamCarving
{
    public class Options
    {
        private int weight;
        public int Weight
        {
            get
            {
                return this.weight;
            }
            set
            {
                this.weight = value;
               // this.controller.WeightChangedRedo();
            }
        }

        private double zoom;
        public double Zoom
        {
            get
            {
                return this.zoom;
            }
            set
            {
                this.zoom = value;
                this.controller.ZoomChanged();
            }
        }

        
        private Controllers.Controller controller;
        
        public SeamCarvingOptions SeamCarvingOptions { get; set;}

        public Options(Controllers.Controller controller)
        {
            this.controller = controller;
            this.SeamCarvingOptions = new SeamCarvingOptions();
        }
    }
}
