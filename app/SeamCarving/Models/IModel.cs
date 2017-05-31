using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeamCarving.Models
{
    public interface IModel
    {
        System.Drawing.Bitmap Bitmap { get; set; }
        long FileSize { get; set; }
        string FileLocation { get; set; }
        bool LoadBitmap(string fileLocation);
        
    }
}
