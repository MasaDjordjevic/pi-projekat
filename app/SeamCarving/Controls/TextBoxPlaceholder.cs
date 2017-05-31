using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeamCarving.Controls
{
    public partial class TextBoxPlaceholder : TextBox
    {

        private string placeholder = "Enter text here...";
        private Color placeholderColor = Color.Gray;
        private Color foreColor;


        public string Placeholder
        {
            get
            {
                return this.placeholder;
            }
            set
            {
                this.placeholder = value;
                this.Text = placeholder;
            }
        }

        public Color PlaceholderColor
        {
            get
            {
                return this.placeholderColor;
            }
            set
            {
                this.placeholderColor = value;
                this.ForeColor = this.placeholderColor;
            }
        }

        public string Tooltip { get; set; }

        public TextBoxPlaceholder()
        {
            this.Text = this.placeholder;
            this.Enter += new System.EventHandler(this.RemovePlaceholder);
            this.Enter += new System.EventHandler(this.ShowTooltip);
            this.Leave += new System.EventHandler(this.AddPlaceholder);
            
            InitializeComponent();
            this.foreColor = this.ForeColor;
        }
       
        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        public void ShowTooltip(object sender, EventArgs e)
        {
            ToolTip tt = new ToolTip();
            tt.Show(this.Tooltip, this, 0, 0, 2000);
        }

        public void RemovePlaceholder(object sender, EventArgs e)
        {
            if(this.Text == this.placeholder)
            {
                this.Text = "";
                this.ForeColor = this.foreColor;

            }
        }

        public void AddPlaceholder(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(this.Text))
            {
                this.Text = placeholder;
                this.ForeColor = this.PlaceholderColor;
            }

        }
    }
}
