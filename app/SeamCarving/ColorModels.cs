using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeamCarving
{
    public class RGB
    {
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }

        public RGB(byte r, byte g, byte b)
        {
            this.R = r;
            this.B = b;
            this.G = g;
        }        
    }

    public class RGBInt
    {
        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }

        public RGBInt(int r, int g, int b)
        {
            this.R = r;
            this.B = b;
            this.G = g;
        }

        public RGB ConvertToRGB()
        {
            byte R = (byte)Math.Max(0, Math.Min(255, this.R));
            byte G = (byte)Math.Max(0, Math.Min(255, this.G));
            byte B = (byte)Math.Max(0, Math.Min(255, this.B));
            return new RGB(R, G, B);
        }
        public Color ConvertToColor()
        {
            byte R = (byte)Math.Max(0, Math.Min(255, this.R));
            byte G = (byte)Math.Max(0, Math.Min(255, this.G));
            byte B = (byte)Math.Max(0, Math.Min(255, this.B));
            return Color.FromArgb(R, G, B);
        }

        public RGBInt(Color color)
        {
            this.R = color.R;
            this.B = color.B;
            this.G = color.G;
        }

        public static implicit operator RGBInt(Color c)
        {
            return new RGBInt(c);
        }

        public static RGBInt operator +(RGBInt left, RGBInt right)
        {
            return new RGBInt(left.R + right.R, left.G + right.G, left.B + right.B);
        }

        public static RGBInt operator +(RGBInt left, int right)
        {
            return new RGBInt(left.R + right, left.G + right, left.B + right);
        }
        public static RGBInt operator -(RGBInt left, RGBInt right)
        {
            return new RGBInt(left.R - right.R, left.G - right.G, left.B - right.B);
        }
        public static RGBInt operator *(int left, RGBInt right)
        {
            return new RGBInt(left * right.R, left * right.G, left * right.B);
        }

        public static RGBInt operator /(RGBInt left, int right)
        {
            return new RGBInt(left.R / right, left.G / right, left.B / right);
        }
    }

    public class YCbCr
    {
        public byte Y { get; set; }
        public byte Cb { get; set; }
        public byte Cr { get; set; }

        public YCbCr(byte y, byte b, byte r)
        {
            this.Y = y;
            this.Cb = b;
            this.Cr = r;
        }
    }

    public class ColorModels
    {
        public static RGB YCbCrToRGB(YCbCr ycbcr)
        {
            float r = Math.Max(0.0f, Math.Min(255f, (float)(ycbcr.Y + 1.402525 * (ycbcr.Cr - 128))));
            float g = Math.Max(0.0f, Math.Min(255f, (float)(ycbcr.Y - 0.34730 * (ycbcr.Cb - 128) - 0.71440 * (ycbcr.Cr - 128))));
            float b = Math.Max(0.0f, Math.Min(255f, (float)(ycbcr.Y + 1.769905 * (ycbcr.Cb - 128))));

            return new RGB((byte)r, (byte)g, (byte)b);
        }


        public static YCbCr RGBtoYCbCr(RGB rgb)
        {
            byte resY = (byte)((0.299 * rgb.R) + (0.587 * rgb.G) + (0.114 * rgb.B));
            byte resCb = (byte)(128 - (0.168736 * rgb.R) - (0.331264 * rgb.G) + (0.5 * rgb.B));
            byte resCr = (byte)(128 + (0.5 * rgb.R) - (0.418688 * rgb.G) - (0.081312 * rgb.B));

            return new YCbCr(resY, resCb, resCr);
        }


        

    }


}
