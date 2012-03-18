using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace SpriteSheetPipeline.LoRes
{
    enum ColorDepth
    {
        RGB565,
        RGB666
    }

    abstract class DitherImage : IDisposable
    {
        protected Bitmap bitmap;
        protected ImageFormat image_format;

        public DitherImage()
        {
            bitmap = null;
            image_format = null;
        }

        public abstract bool LoadFromFile(string filename);
        public abstract bool WriteToFile(string filename, bool hexdump);
        public abstract bool ConvertImage(ColorDepth depth, bool dither);

        #region Parameters
        public bool IsAppropriateFormat
        {
            get
            {
                if (bitmap == null)
                    return false;

                return ((bitmap.PixelFormat == System.Drawing.Imaging.PixelFormat.Format24bppRgb)
                    || (bitmap.PixelFormat == System.Drawing.Imaging.PixelFormat.Format32bppArgb)
                     || (bitmap.PixelFormat == System.Drawing.Imaging.PixelFormat.Format32bppRgb));
            }
        }
        public System.Drawing.Imaging.PixelFormat PixelFormat
        {
            get
            {
                return bitmap.PixelFormat;
            }
        }
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (bitmap != null)
                bitmap.Dispose();
        }

        #endregion
    }

    // can make it even faster by locking the bitmap in memory
    // but this will require knowledge of bitmap format
    // processing it one-bit at a time from the bitmap is 20% slower
    // loading it all into two 2dim bitmaps is also 20% slower
    class DitherImageByGIMPHalftoning : DitherImage
    {
        protected int[, ,] color_planes; // 0 = R, 1 = G, 2 = B, 3 = A

        public DitherImageByGIMPHalftoning()
        {
            color_planes = null;
        }

        public override bool LoadFromFile(string filename)
        {
            if (bitmap != null)
            {
                bitmap.Dispose();
                bitmap = null;
            }

            try
            {
                bitmap = new Bitmap(filename);

                // get the image type
                image_format = bitmap.RawFormat;

                // allocate memory for this operation
                color_planes = new int[4, bitmap.Height, bitmap.Width];

                // copy data from the bitmap to these planes
                for (int row = 0; row < bitmap.Height; row++)
                    for (int col = 0; col < bitmap.Width; col++)
                    {
                        Color pixel = bitmap.GetPixel(col, row);
                        color_planes[0, row, col] = pixel.R;
                        color_planes[1, row, col] = pixel.G;
                        color_planes[2, row, col] = pixel.B;
                        color_planes[3, row, col] = pixel.A;
                    }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public override bool WriteToFile(string filename, bool hexdump)
        {
            if (bitmap == null)
                return false;

            try
            {
                // copy the data back into the bitmap
                for (int row = 0; row < bitmap.Height; row++)
                    for (int col = 0; col < bitmap.Width; col++)
                    {
                        Color pixel = Color.FromArgb(color_planes[3, row, col], color_planes[0, row, col],
                            color_planes[1, row, col], color_planes[2, row, col]);
                        bitmap.SetPixel(col, row, pixel);
                    }

                string dest_dir = Path.GetDirectoryName(filename);
                //if (!Directory.Exists(dest_dir))
                //{
                //    Directory.CreateDirectory(dest_dir);
                //}

                FileStream fs = File.Create(filename);
                bitmap.Save(fs, image_format);
                fs.Close();

                // debug
                if (hexdump)
                {
                    // write converted file
                    StreamWriter ftxt = new StreamWriter(filename + ".new.hex", false);
                    for (int row = 0; row < bitmap.Height; row++)
                    {
                        ftxt.Write("{0:D4} : ", row);
                        for (int col = 0; col < bitmap.Width; col++)
                        {
                            Color pixel = Color.FromArgb(color_planes[3, row, col], color_planes[0, row, col],
                                color_planes[1, row, col], color_planes[2, row, col]);
                            ftxt.Write("[{0:X2},{1:X2},{2:X2},{3:X2}] ", color_planes[3, row, col], color_planes[0, row, col],
                                color_planes[1, row, col], color_planes[2, row, col]);
                        }
                        ftxt.WriteLine();
                    }
                    ftxt.Close();
                }

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("** fatal error: " + e.Message);
                return false;
            }
        }

        public override bool ConvertImage(ColorDepth depth, bool dither)
        {
            if (bitmap == null)
                return false;

            for (int row = 0; row < bitmap.Height; row++)
            {
                int dmp_offset = (row & (DitherTable.DM_HEIGHT - 1)) << DitherTable.DM_WIDTH_SHIFT;

                for (int col = 0; col < bitmap.Width; col++)
                {
                    if (dither)
                    {
                        uint rgb = (uint)((color_planes[0, row, col] << 20)
                                            + (color_planes[1, row, col] << 10)
                                            + color_planes[2, row, col]);

                        rgb += DitherTable.DM_565[dmp_offset + (col & (DitherTable.DM_WIDTH - 1))];
                        rgb += (uint)0x10040100 - ((rgb & (uint)0x1e0001e0) >> 5) - ((rgb & (uint)0x00070000) >> 6);

                        uint rgb24 = ((rgb & 0x0ff00000) >> 4) |
                                        ((rgb & 0x0003fc00) >> 2) |
                                        (rgb & 0x000000ff);

                        if (depth == ColorDepth.RGB565)
                        {

                            color_planes[0, row, col] = (byte)((rgb24 >> 16) & 0xf8);
                            color_planes[1, row, col] = (byte)((rgb24 >> 8) & 0xfc);
                            color_planes[2, row, col] = (byte)(rgb24 & 0xf8);
                        }
                        else
                        {
                            color_planes[0, row, col] = (byte)((rgb24 >> 16) & 0xfc);
                            color_planes[1, row, col] = (byte)((rgb24 >> 8) & 0xfc);
                            color_planes[2, row, col] = (byte)(rgb24 & 0xfc);
                        }
                    }
                    else
                    {
                        if (depth == ColorDepth.RGB565)
                        {
                            color_planes[0, row, col] &= 0xf8;
                            color_planes[1, row, col] &= 0xfc;
                            color_planes[2, row, col] &= 0xf8;
                        }
                        else
                        {
                            color_planes[0, row, col] &= 0xfc;
                            color_planes[1, row, col] &= 0xfc;
                            color_planes[2, row, col] &= 0xfc;
                        }
                    }
                }
            }
            return true;
        }
    }
}
