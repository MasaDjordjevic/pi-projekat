using SeamCarving.Filters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SeamCarving
{
    public class ConvMatrix
    {
        public int a = 0, b = 0, c = 0;
        public int d = 0, e = 1, f = 0;
        public int g = 0, h = 0, i = 0;
        public int Factor = 1;
        public int Offset = 0;

        public void SetAll(int nVal)
        {
            a = b = c = d = e = f = g = h = i = nVal;
        }

        public int[,] GetMatix(int dim)
        {
            if (dim == 3)
            {
                return new int[3, 3]
                {
                    {a, b, c},
                    {d, e, f},
                    {g, h, i}
                };
            }


            if (dim == 5)
            {
                return new int[5, 5]
                {
                   {a, a, b, b, c},
                   {d, a, b, c, c},
                   {d, d, e, f, f},
                   {g, g, h, i, f},
                   {g, h, h, i, i}
                };
            }

            if (dim == 7)
            {
                return new int[7, 7]
                {
                   {a, a, 0, b, b, 0, c},
                   {0, a, a, b, b, c, c},
                   {d, d, a, b, c, c, 0},
                   {d, d, d, e, f, f, f},
                   {0, g, g, h, i, f, f},
                   {g, g, h, h, i, i, 0},
                   {g, 0, h, h, 0, i, i}
                };
            }

            return null;

        }
    }


    public class ConvFilters
    {
        private Views.CommonControls commonControls;

        public ConvFilters(Views.CommonControls commonControls)
        {
            this.commonControls = commonControls;
        }

        public bool ConvSafe(Bitmap b, ConvMatrix m, int dimension, bool inplace = false)
        {
            // Avoid divide by zero errors
            if (0 == m.Factor) return false;

            Bitmap bSrc = (Bitmap)b.Clone();

            int nWidth = b.Width - 2;
            int nHeight = b.Height - 2;
            int[,] matrix = m.GetMatix(dimension);
            if (matrix == null) return false;

            if (commonControls != null) commonControls.progress = 0;
            double step = 100.0 / nHeight;
            double progress = 0;
            for (int y = 0; y < nHeight; ++y)
            {
                for (int x = 0; x < nWidth; ++x)
                {

                    RGBInt pixel = new RGBInt(0, 0, 0);
                    for (int i = 0; i < dimension; i++)
                    {
                        for (int j = 0; j < dimension; j++)
                        {

                            if (inplace)
                            {
                                pixel += matrix[i, j] * (RGBInt)b.GetPixel(x + i, y + j);
                            }
                            else
                            {
                                pixel += matrix[i, j] * (RGBInt)bSrc.GetPixel(x + i, y + j);
                            }
                        }
                    }

                    pixel = (pixel / m.Factor) + m.Offset;

                    Color newColor = pixel.ConvertToColor();
                    b.SetPixel(x + dimension / 2, y + dimension / 2, newColor);

                }

                progress += step;
                if (commonControls != null) commonControls.progress = (int)(progress);

            }

            return true;
        }

        public static Bitmap ExtendBitmap(Bitmap b, int pixelCount, Color color)
        {
            Bitmap retVal = new Bitmap(b.Width + pixelCount * 2, b.Height + pixelCount * 2);

            using (Graphics gfx = Graphics.FromImage(retVal))
            using (SolidBrush brush = new SolidBrush(color))
            {
                gfx.FillRectangle(brush, 0, 0, retVal.Width, retVal.Height);
            }

            BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData bmRet = retVal.LockBits(new Rectangle(pixelCount, pixelCount, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);


            IntPtr srcPtr = bmData.Scan0;
            IntPtr destPtr = bmRet.Scan0;
            byte[] buffer = new byte[bmData.Stride];
            int len = b.Width * 3;
            for (int i = 0; i < bmData.Height; ++i)
            {
                Marshal.Copy(srcPtr, buffer, 0, len);
                Marshal.Copy(buffer, 0, destPtr, len);

                srcPtr += bmData.Stride;
                destPtr += bmRet.Stride;
            }

            b.UnlockBits(bmData);
            retVal.UnlockBits(bmRet);

            return retVal;
        }


        public bool Conv(Bitmap b, ConvMatrix m, int dimension = 3, bool inplace = false)
        {
            // Avoid divide by zero errors
            if (0 == m.Factor) return false;
            int dim2 = dimension / 2;


            Bitmap bSrc = inplace ? (Bitmap)b.Clone() : ExtendBitmap((Bitmap)b.Clone(), dim2, Color.White);

            BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData bmSrc = bSrc.LockBits(new Rectangle(0, 0, bSrc.Width, bSrc.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int stride = bmData.Stride;
            int strideSrc = bmSrc.Stride;
            System.IntPtr Scan0 = bmData.Scan0;
            System.IntPtr SrcScan0 = bmSrc.Scan0;

            unsafe
            {
                byte* p = (byte*)(void*)Scan0;
                byte* pSrc = inplace ? (byte*)(void*)Scan0 : (byte*)(void*)SrcScan0;

                int nOffset = stride - b.Width * 3;
                int srcOffset = strideSrc - bSrc.Width * 3;
                int nWidth = b.Width * 3;
                int nHeight = b.Height;

                if (inplace)
                {
                    nWidth -= dimension - 1;
                    nHeight -= dimension - 1;
                }

                int dest = 0;
                if (inplace)
                {
                    dest = 3 * dim2 + stride * dim2;
                }


                if (!inplace)
                {
                    srcOffset += +dim2 * 2 * 3;
                }

                m.Factor = (int)(m.Factor / (3 * 0.8) * (dimension * 0.8));
                int[,] matrix = m.GetMatix(dimension);


                if (commonControls != null) commonControls.progress = 0;
                double step = 100.0 / nHeight;
                double progress = 0;
                //Parallel.For(0, nHeight, y =>
                for (int y = 0; y < nHeight; ++y)
                {
                    for (int x = 0; x < nWidth; ++x)
                    {

                        int nPixel = 0;
                        for (int i = 0; i < dimension; i++)
                        {
                            for (int j = 0; j < dimension; j++)
                            {
                                nPixel += pSrc[j * 3 + 0 + strideSrc * i] * matrix[i, j];
                            }
                        }

                        nPixel = nPixel / m.Factor + m.Offset;
                        if (nPixel < 0) nPixel = 0;
                        if (nPixel > 255) nPixel = 255;

                        p[dest] = (byte)nPixel;


                        p += 1;
                        pSrc += 1;
                    }
                    p += nOffset;
                    pSrc += srcOffset;

                    progress += step;
                    if (commonControls != null) commonControls.progress = (int)(progress);
                }

            }

            b.UnlockBits(bmData);
            bSrc.UnlockBits(bmSrc);

            return true;

        }

        public bool EdgeDetectConvolution(Bitmap b, short nType, byte nThreshold = 0)
        {
            ConvMatrix m = new ConvMatrix();

            // I need to make a copy of this bitmap BEFORE I alter it 80)
            Bitmap bTemp = (Bitmap)b.Clone();

            switch (nType)
            {
                case 0:
                    m.SetAll(0);
                    m.a = m.g = 1;
                    m.c = m.i = -1;
                    m.d = 2;
                    m.f = -2;
                    m.Offset = 0;
                    break;
                case 1:
                    m.SetAll(0);
                    m.a = m.d = m.g = -1;
                    m.c = m.f = m.i = 1;
                    m.Offset = 0;
                    break;
                case 2:
                    m.SetAll(-3);
                    m.e = 0;
                    m.a = m.d = m.g = 5;
                    m.Offset = 0;
                    break;
            }

            this.Conv(b, m);

            switch (nType)
            {
                case 0:
                    m.SetAll(0);
                    m.a = m.c = 1;
                    m.g = m.i = -1;
                    m.b = 2;
                    m.h = -2;
                    m.Offset = 0;
                    break;
                case 1:
                    m.SetAll(0);
                    m.g = m.h = m.i = -1;
                    m.a = m.b = m.c = 1;
                    m.Offset = 0;
                    break;
                case 2:
                    m.SetAll(-3);
                    m.e = 0;
                    m.g = m.h = m.i = 5;
                    m.Offset = 0;
                    break;
            }

            this.Conv(bTemp, m);

            // GDI+ still lies to us - the return format is BGR, NOT RGB.
            BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData bmData2 = bTemp.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int stride = bmData.Stride;
            System.IntPtr Scan0 = bmData.Scan0;
            System.IntPtr Scan02 = bmData2.Scan0;

            unsafe
            {
                byte* p = (byte*)(void*)Scan0;
                byte* p2 = (byte*)(void*)Scan02;

                int nOffset = stride - b.Width * 3;
                int nWidth = b.Width * 3;

                int nPixel = 0;

                for (int y = 0; y < b.Height; ++y)
                {
                    for (int x = 0; x < nWidth; ++x)
                    {
                        nPixel = (int)Math.Sqrt((p[0] * p[0]) + (p2[0] * p2[0]));
                        if (nPixel < nThreshold) nPixel = nThreshold;
                        if (nPixel > 255) nPixel = 255;
                        p[0] = (byte)nPixel;
                        ++p;
                        ++p2;
                    }
                    p += nOffset;
                    p2 += nOffset;
                }
            }

            b.UnlockBits(bmData);
            bTemp.UnlockBits(bmData2);

            return true;
        }
    }
}
