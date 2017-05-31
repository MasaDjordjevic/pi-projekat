using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeamCarving.Views
{
    public class CommonControls
    {
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private LollipopProgressBar progressBar { get; set; }

        public ListView listView { get; set; }

        public CommonControls(System.Windows.Forms.ToolStripStatusLabel statusLabel, LollipopProgressBar progressBar, ListView listView)
        {
            this.statusLabel = statusLabel;
            this.progressBar = progressBar;
            this.listView = listView;
        }

        public int progress
        {
            get { return this.progressBar.Value; }
            set
            {
                if (this.progressBar != null && this.progressBar.Value != value)
                {        
                    
                    this.progressBar.Value = value;
                    
                    //this.statusLabel.Text += value.ToString();
                }
            }
        }

        public string status
        {
            get { return this.statusLabel.Text; }
            set
            {
                if (this.statusLabel != null)
                {
                    this.statusLabel.Text = value;
                }
            }
        }
    }
}
