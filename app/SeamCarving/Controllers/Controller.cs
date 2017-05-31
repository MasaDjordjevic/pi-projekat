using SeamCarving.Filters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeamCarving.Controllers
{
    public class Controller : IController
    {
        private Models.IModel model;
        private Views.IView view;
        public Options options { get; set; }

        private List<Action> undoList = new List<Action>(5);
        private List<Action> redoListe = new List<Action>(5);

        private Thread workerThread = null;

        public Views.CommonControls commonControls { get; set; }

        private Form parent;

        public Controller(Models.IModel model, Views.IView view, Views.CommonControls commonControls, Form parent)
        {
            this.model = model;
            this.view = view;
            this.view.BringToFront();
            this.commonControls = commonControls;
            this.parent = parent;
        }
      

        public void SetView(Views.IView view)
        {
            this.view = view;
            this.view.Bitmap = this.model.Bitmap;
            this.view.BringToFront();
        }      

        public Views.IView GetView()
        {
            return this.view;
        }


        public void LoadImage(string fileLocation)
        {
            if (this.model.Bitmap != null)
                this.DoAction("Load");

            if (!this.model.LoadBitmap(fileLocation))
                return;           

            this.SetImage(this.model.Bitmap);
        }

        public void ReloadImage()
        {
            this.DoAction("Reload");
            if (!this.model.LoadBitmap(this.model.FileLocation))
                return;

            this.SetImage(this.model.Bitmap);
        }

        public void ReloadImageModel()
        {
            this.model.LoadBitmap(this.model.FileLocation);
        }

        public void SetImage(System.Drawing.Bitmap bitmap)
        {
            this.model.Bitmap = bitmap;
            this.view.Bitmap = (Bitmap)bitmap.Clone();
            this.commonControls.status = bitmap.Width.ToString() + " x " + bitmap.Height.ToString() + "         " + (this.model.FileSize / 1024).ToString() + "KB";
        }     

      
        public void ZoomChanged()
        {
            this.view.Zoom = this.options.Zoom;
            this.view.Bitmap = this.model.Bitmap;
        }

        public void showUndoStack()
        {
            this.commonControls.listView.LargeImageList.Images.Clear();
            this.commonControls.listView.Items.Clear();
            for (int i = 0; i < this.undoList.Count; i++)
            {
                this.commonControls.listView.LargeImageList.Images.Add(this.undoList[i].Bitmap);

                ListViewItem item = new ListViewItem();
                item.ImageIndex = i;
                this.commonControls.listView.Items.Add(item);
            }

            this.commonControls.listView.Items.Add("--Redo--");

            for (int i = 0; i < this.redoListe.Count; i++)
            {
                this.commonControls.listView.LargeImageList.Images.Add(this.redoListe[i].Bitmap);

                ListViewItem item = new ListViewItem();
                item.ImageIndex = i + this.undoList.Count;
                this.commonControls.listView.Items.Add(item);
            }

        }

        public void DoAction(string name)
        {
            undoList.Push(new Action((Bitmap)this.model.Bitmap.Clone(), name));
            this.showUndoStack();
            this.redoListe.Clear();
        }

        public void UndoAction()
        {
            Action a = undoList.Pop();
            if (a == null)
                return;
            redoListe.Push(new Action((Bitmap)this.model.Bitmap.Clone(), a.Name));
            this.showUndoStack();
            this.model.Bitmap = a.Bitmap;
            this.view.Bitmap = this.model.Bitmap;

        }

        public void RedoAction()
        {
            Action a = redoListe.Pop();
            if (a == null)
                return;
            undoList.Push(new Action((Bitmap)this.model.Bitmap.Clone(), a.Name));
            this.showUndoStack();
            this.model.Bitmap = a.Bitmap;
            this.view.Bitmap = this.model.Bitmap;
        }
    }
}
