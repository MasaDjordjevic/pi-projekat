using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeamCarving
{
    public class Action
    {
        public Bitmap Bitmap { get; set; }
        public string Name { get; set; }

        public Action(Bitmap b, string name = null)
        {
            this.Bitmap = b;
            this.Name = name;
        }
    }
}
