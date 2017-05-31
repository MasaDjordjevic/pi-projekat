using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeamCarving.Controls
{
    public partial class SeamCarvingInputs : UserControl
    {
        public event EventHandler PixelsToRemoveChanged;
        public event EventHandler MatrixChanged;

        public ConvolutionMatrix Matrix
        {
            get
            {
                try
                {
                    return (ConvolutionMatrix)this.comboBox1.SelectedIndex;
                }
                catch {}
                return ConvolutionMatrix.kirsh;
            }
        }

        public int PixelsToRemove
        {
            get
            {
                try
                {
                    return Convert.ToInt32(this.textBox2.Text);
                }
                catch { }
                return 1;
            }
        }
     
        public SeamCarvingInputs()
        {
            InitializeComponent();
            this.comboBox1.SelectedIndex = 0;
        }


        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            this.PixelsToRemoveChanged?.Invoke(this, e);
        }    

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.MatrixChanged?.Invoke(this, e);
        }
    }
}
