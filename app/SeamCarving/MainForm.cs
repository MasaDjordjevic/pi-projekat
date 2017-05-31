using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SeamCarving.Views;
using System.IO;
using SeamCarving.Views.SplitViewStrategies;

namespace SeamCarving
{
    public partial class MainForm : Form
    {

        Models.IModel model = new Models.Model();
        private IView simpleView, splitView;
        private Controllers.Controller controller;
        private Options options;

        public MainForm()
        {
            loadComponents();

            InitializeComponent();
        }

        private void loadComponents()
        {
            this.simpleView = new SimpleImageView();
            UserControl simpleView = (UserControl)this.simpleView;
            simpleView.Location = new System.Drawing.Point(0, 0);
            simpleView.Name = "simple view";
            Controls.Add(simpleView);

            this.splitView = new SplitView();
            UserControl splitView = (UserControl)this.splitView;
            splitView.Location = new System.Drawing.Point(0, 0);
            splitView.Name = "split view";
            Controls.Add(splitView);          


            LollipopToggleText.CheckForIllegalCrossThreadCalls = false;
        }

        private void loadImage()
        {
            this.controller.LoadImage(System.IO.Path.GetFullPath(@"..\..\Test images\BroadwayTowerSeamCarvingA.png"));

            this.listView1.LargeImageList = new ImageList();
            this.listView1.LargeImageList.ImageSize = new Size(70, 70);
            this.listView1.View = View.LargeIcon;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.controller = new Controllers.Controller(this.model, this.simpleView, new CommonControls(this.statusLabel, this.progressBar, this.listView1), this);
            this.options = new Options(this.controller);
            this.controller.options = this.options;
            loadImage();

            this.SetSeamCravingEventHandlers();
        }
       

        private void SetSeamCravingEventHandlers()
        {
            this.seamCarvingInputs1.PixelsToRemoveChanged += (object sender2, EventArgs e2) => {
                if (((SplitView)splitView).Strategy is SeamCravingStrategy)
                {
                    ((SeamCravingStrategy)((SplitView)splitView).Strategy).pixelsToRemove = this.seamCarvingInputs1.PixelsToRemove;
                }
            };
            this.seamCarvingInputs1.MatrixChanged += (object sender2, EventArgs e2) => {
                if (((SplitView)splitView).Strategy is SeamCravingStrategy)
                {
                    ((SeamCravingStrategy)((SplitView)splitView).Strategy).matrix = (short)this.seamCarvingInputs1.Matrix;
                }
               
            };           
        }

      

        private void reloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.controller.ReloadImage();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {          
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.InitialDirectory = System.IO.Path.GetFullPath(@"..\..\Test images\");
            saveFileDialog.Filter = "Jpeg files (*.jpg)|*.jpg|Bitmap files (*.bmp)|*.bmp|PNG files(*.png)|*.png";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;

            if (DialogResult.OK == saveFileDialog.ShowDialog())
            {
                Bitmap b;
                if(this.controller.GetView() is SplitView)
                {
                    b = ((SplitView)this.controller.GetView()).ResizedBitmap;
                }
                else
                {
                    b = this.model.Bitmap;
                }

                    string f = saveFileDialog.FileName;

                    if (f.EndsWith(".jpg"))
                        b.Save(f, System.Drawing.Imaging.ImageFormat.Jpeg);

                    if (f.EndsWith(".bmp"))
                        b.Save(f, System.Drawing.Imaging.ImageFormat.Bmp);

                    if (f.EndsWith(".png"))
                        b.Save(f, System.Drawing.Imaging.ImageFormat.Png);
                
              
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            this.options.Zoom = (double)trackBar1.Value / 10.0;
        }              

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.InitialDirectory = System.IO.Path.GetFullPath(@"..\..\Test images\");
            openFileDialog.Filter = "All valid files|*.bmp;*.jpg;*.png|Jpeg files (*.jpg)|*.jpg|Bitmap files (*.bmp)|*.bmp|PNG files(*.png)|*.png";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {     
                    this.controller.LoadImage(openFileDialog.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void simpleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.controller.SetView(this.simpleView);
            this.trackBar1.Enabled = true;
        }

        private void energyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ((SplitView)splitView).Strategy = new EnergyStrategy((SplitView)this.splitView, this.controller.commonControls, this);
            this.controller.SetView(this.splitView);
            this.trackBar1.Enabled = false;
        }
        
        private void seamCarvingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ((SplitView)splitView).Strategy = new SeamCravingStrategy((SplitView)splitView, this.controller.commonControls, this.seamCarvingInputs1, this);
            this.controller.SetView(this.splitView);
            trackBar1.Enabled = false;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if(this.Enabled)
            {
                // All other key messages process as usual
                return base.ProcessCmdKey(ref msg, keyData);
            }
            return false;
        }

        private void undoToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            this.controller.UndoAction();
        }

        private void redoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.controller.RedoAction();
        }

        private void seamCravingResizeToolStripMenuItem_Click(object sender, EventArgs e)
        {          
            if (!(((SplitView)splitView).Strategy is SeamCravingStrategy))
            {
                seamCarvingToolStripMenuItem_Click(this, null);

            }
            ((SeamCravingStrategy)((SplitView)splitView).Strategy).Redo();
        }        
    
    }
}
