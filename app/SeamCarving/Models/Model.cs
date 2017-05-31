using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;


namespace SeamCarving.Models
{
    public class Model : IModel
    {
        
        public Bitmap Bitmap { get; set; }
        public long FileSize { get; set; }
        public string FileLocation { get; set; }
        public bool LoadBitmap(string fileLocation)
        {
            try
            {
                this.Bitmap = (Bitmap)Bitmap.FromFile(fileLocation);
                this.FileSize = new System.IO.FileInfo(fileLocation).Length;
                this.FileLocation = fileLocation;
                return true;
            }
            catch
            {
                MessageBox.Show("Image doesn't exist");
                return false;
            }
            
        }

        
    }
}
