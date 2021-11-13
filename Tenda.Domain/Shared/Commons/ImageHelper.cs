using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Tenda.Domain.Shared.Commons
{
    public static class ImageHelper
    {
        public static byte[] MakeThumbnail(byte[] myImage, int thumbWidth, int thumbHeight)
        {
            using (MemoryStream ms = new MemoryStream())
            using (Image thumbnail = Image.FromStream(new MemoryStream(myImage)).GetThumbnailImage(thumbWidth, thumbHeight, null, new IntPtr()))
            {
                thumbnail.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }

        public static byte[] MakeThumbnail(byte[] myImage, int thumbMaxDimension)
        {
            using (var originalImage = Image.FromStream(new MemoryStream(myImage)))
            {
                int biggestDimension = originalImage.Height > originalImage.Width ? originalImage.Height : originalImage.Width;
                var reductionProportion = biggestDimension / thumbMaxDimension;
                int newWidth = 0;
                int newHeigth = 0;
                if (originalImage.Height > originalImage.Width)
                {
                    newHeigth = thumbMaxDimension;
                    newWidth = originalImage.Width / reductionProportion;
                }
                else
                {
                    newWidth = thumbMaxDimension;
                    newHeigth = originalImage.Width / reductionProportion;
                }
                using (MemoryStream ms = new MemoryStream())
                using (Image thumbnail = originalImage.GetThumbnailImage(newWidth, newHeigth, null, new IntPtr()))
                {
                    thumbnail.Save(ms, ImageFormat.Png);
                    return ms.ToArray();
                }
            }
        }

        public static Image FixedSize(byte[] imageContents, int width, int heigth)
        {
            return FixedSize(Image.FromStream(new MemoryStream(imageContents)), width, heigth);
        }

        public static Image FixedSize(Image imgPhoto, int width, int height)
        {
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)width / (float)sourceWidth);
            nPercentH = ((float)height / (float)sourceHeight);
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = System.Convert.ToInt16((width -
                              (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = System.Convert.ToInt16((height -
                              (sourceHeight * nPercent)) / 2);
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(width, height,
                              PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
                             imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(Color.Red);
            grPhoto.InterpolationMode =
                    InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }
    }
}
