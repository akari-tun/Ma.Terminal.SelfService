using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Ma.Terminal.SelfService.Utils
{
    public static class ImageUtils
    {
        #region Bitmap 转为位图
        /// <summary>
        /// Bitmap 转为位图
        /// </summary>
        /// <param name="bitmap">Bitmap 对象</param>
        /// <returns>ImageSource 位图对象</returns>
        public static ImageSource BitmapToImageSource(System.Drawing.Bitmap bitmap)
        {
            try
            {
                IntPtr intPtr = bitmap.GetHbitmap();
                ImageSource imageSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(intPtr, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                return imageSource;

            }
            catch (Exception ex)
            {
            }

            return null;
        }
        #endregion

        #region ImageSource 转为Bitmap
        /// <summary>
        /// ImageSource 转为Bitmap
        /// </summary>
        /// <param name="imageSource">imageSource 对象</param>
        /// <returns>返回 Bitmap 对象</returns>
        public static System.Drawing.Bitmap ImageSourceToBitmap(ImageSource imageSource)
        {
            try
            {
                BitmapSource bitmapSource = (BitmapSource)imageSource;
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(bitmapSource.PixelWidth, bitmapSource.PixelHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                System.Drawing.Imaging.BitmapData data = bitmap.LockBits(new System.Drawing.Rectangle(System.Drawing.Point.Empty, bitmap.Size), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                bitmapSource.CopyPixels(Int32Rect.Empty, data.Scan0, data.Height * data.Stride, data.Stride);
                bitmap.UnlockBits(data);
                return bitmap;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        #endregion
        #region  Bitmap  转 BitmapImage
        /// <summary>
        /// Bitmap  转 BitmapImage
        /// </summary>
        /// <param name="bitmap">Bitmap 对象</param>
        /// <returns>转换后的 BitmapImage对象</returns>
        public static BitmapImage BitmapToBitmapImage(System.Drawing.Bitmap bitmap)
        {
            try
            {
                BitmapImage relust = new BitmapImage();
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    MemoryStream memory = new MemoryStream();
                    bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                    memoryStream.Position = 0;
                    relust.BeginInit();
                    relust.CacheOption = BitmapCacheOption.OnLoad;
                    memoryStream.CopyTo(memory);
                    relust.StreamSource = memory;
                    relust.EndInit();
                    relust.Freeze();
                    return relust;
                }
            }
            catch (Exception ex)
            {
            }
            return null;
        }
        #endregion

        #region BitmapImage 转为byte[]
        /// <summary>
        /// BitmapImage 转为byte[]
        /// </summary>
        /// <param name="bitmapImage">BitmapImage 对象</param>
        /// <returns>byte[] 数组</returns>
        public static byte[] BitmapImageToByteArray(BitmapImage bitmapImage)
        {
            byte[] buffer = new byte[] { };
            try
            {
                Stream stream = bitmapImage.StreamSource;
                if (stream != null && stream.Length > 0)
                {
                    stream.Position = 0;
                    using (BinaryReader binary = new BinaryReader(stream))
                    {
                        buffer = binary.ReadBytes((int)stream.Length);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return buffer;
        }
        #endregion

        #region 图片压缩
        /// <summary>
        /// 图片压缩
        /// </summary>
        /// <param name="bitmap">要压缩的源图像</param>
        /// <param name="height">要求的高</param>
        /// <param name="width">要求的宽</param>
        /// <returns></returns>
        public static System.Drawing.Bitmap GetPicThumbnail(System.Drawing.Bitmap bitmap, int height, int width)
        {
            try
            {
                lock (bitmap)
                {
                    System.Drawing.Bitmap iSource = bitmap;
                    System.Drawing.Imaging.ImageFormat imageFormat = iSource.RawFormat;
                    int sw = width, sh = height;
                    //按比例缩放
                    System.Drawing.Bitmap ob = new System.Drawing.Bitmap(width, height);
                    System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(ob);
                    graphics.Clear(System.Drawing.Color.WhiteSmoke);
                    graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    graphics.DrawImage(iSource, new System.Drawing.Rectangle((width - sw) / 2, (height - sh) / 2, sw, sh), 0, 0, iSource.Width, iSource.Height, System.Drawing.GraphicsUnit.Pixel);
                    graphics.Dispose();
                    return ob;
                }
            }
            catch (Exception ex)
            {
            }
            return bitmap;
        }
        #endregion
    }
}
