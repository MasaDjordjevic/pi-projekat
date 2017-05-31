using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public class LollipopToggleText : CheckBox
{
    #region Variables

    Timer AnimationTimer = new Timer { Interval = 1 };
    GraphicsPath RoundedRectangle;

    string EllipseBG = "#508ef5";
    string EllipseBorder = "#3b73d1";

    Color EllipseBackColor;
    Color EllipseBorderBackColor;

    Color EnabledUnCheckedColor = ColorTranslator.FromHtml("#bcbfc4");
    Color EnabledUnCheckedEllipseBorderColor = ColorTranslator.FromHtml("#a9acb0");

    Color DisabledEllipseBackColor = ColorTranslator.FromHtml("#c3c4c6");
    Color DisabledEllipseBorderBackColor = ColorTranslator.FromHtml("#90949a");
    
    int PointAnimationNum = 4;

    int toggleWidth = 47;
    FontManager font = new FontManager();
    Color stringColor = ColorTranslator.FromHtml("#999999");


    #endregion
    #region  Properties

    [Category("Appearance")]
    public string EllipseColor
    {
        get { return EllipseBG; }
        set
        {
            EllipseBG = value;
            Invalidate();
        }
    }
    [Category("Appearance")]
    public string EllipseBorderColor
    {
        get { return EllipseBorder; }
        set
        {
            EllipseBorder = value;
            Invalidate();
        }
    }

    #endregion
    #region Events

    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);
        AnimationTimer.Start();
    }

    protected override void OnResize(EventArgs e)
    {
        Height = 19;
        Width = toggleWidth + (int)CreateGraphics().MeasureString(Text, font.Roboto_Medium10).Width;

        RoundedRectangle = new GraphicsPath();
        int radius = 10;
        int W = toggleWidth;
        RoundedRectangle.AddArc(11, 4, radius - 1, radius, 180, 90);
        RoundedRectangle.AddArc(W - 21, 4, radius - 1, radius, -90, 90);
        RoundedRectangle.AddArc(W - 21, Height - 15, radius - 1, radius, 0, 90);
        RoundedRectangle.AddArc(11, Height - 15, radius - 1, radius, 90, 90);

        RoundedRectangle.CloseAllFigures();
        Invalidate(); 
    }

    #endregion
    public LollipopToggleText()
    {
        DoubleBuffered = true;
        AnimationTimer.Tick += new EventHandler(AnimationTick);
    }

    protected override void OnPaint(PaintEventArgs pevent)
    {
        var G = pevent.Graphics;
        G.SmoothingMode = SmoothingMode.AntiAlias;
        G.Clear(Parent.BackColor);

        EllipseBackColor = ColorTranslator.FromHtml(EllipseBG);
        EllipseBorderBackColor = ColorTranslator.FromHtml(EllipseBorder);

        G.FillPath(new SolidBrush(Color.FromArgb(115, Enabled ? Checked ? EllipseBackColor : EnabledUnCheckedColor : EnabledUnCheckedColor)), RoundedRectangle);
        G.DrawPath(new Pen(Color.FromArgb(50, Enabled ? Checked ? EllipseBackColor : EnabledUnCheckedColor : EnabledUnCheckedColor)), RoundedRectangle);

        G.FillEllipse(new SolidBrush(Enabled ? Checked ? EllipseBackColor : Color.White : DisabledEllipseBackColor), PointAnimationNum, 0, 18, 18);
        G.DrawEllipse(new Pen(Enabled ? Checked ? EllipseBorderBackColor : EnabledUnCheckedEllipseBorderColor : DisabledEllipseBorderBackColor), PointAnimationNum, 0, 18, 18);

        //Toggle Text
        G.DrawString(Text, font.Roboto_Medium10, new SolidBrush(stringColor), toggleWidth, 0);
    }

    void AnimationTick(object sender, EventArgs e)
    {
        if (Checked)
        {
            if (PointAnimationNum < 24)
            {
                PointAnimationNum += 1;
                this.Invalidate();
            }
        }
        else if (PointAnimationNum > 4)
        {
            PointAnimationNum -= 1;
            this.Invalidate();
        }
    }
}

