using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;

using System.Runtime.InteropServices;
using System.Text;


namespace WallpaperDownloader_nationalgeographic
{
    public sealed class Wallpaper
    {
        Wallpaper() { }

        const int SPI_SETDESKWALLPAPER = 20;
        const int SPIF_UPDATEINIFILE = 0x01;
        const int SPIF_SENDWININICHANGE = 0x02;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

        public enum Style : int
        {
            Tiled,
            Centered,
            Stretched,
            Filled,
            Fitted
        }
        static void GetImageFormat(System.Drawing.Image img, out System.Drawing.Imaging.ImageFormat imgFormat)
        {
            if (img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Bmp))
            {
                imgFormat = System.Drawing.Imaging.ImageFormat.Bmp;

            }
            if (img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Emf))
            {
                imgFormat = System.Drawing.Imaging.ImageFormat.Emf;
            }
            if (img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Exif))
            {
                imgFormat = System.Drawing.Imaging.ImageFormat.Exif;
            }
            if (img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Gif))
            {
                imgFormat = System.Drawing.Imaging.ImageFormat.Gif;
            }
            if (img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Icon))
            {
                imgFormat = System.Drawing.Imaging.ImageFormat.Icon;
            }
            if (img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.MemoryBmp))
            {
                imgFormat = System.Drawing.Imaging.ImageFormat.MemoryBmp;
            }
            if (img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Png))
            {
                imgFormat = System.Drawing.Imaging.ImageFormat.Png;
            }
            if (img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Tiff))
            {
                imgFormat = System.Drawing.Imaging.ImageFormat.Tiff;
            }
            if (img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Wmf))
            {
                imgFormat = System.Drawing.Imaging.ImageFormat.Wmf;
            }


            imgFormat = System.Drawing.Imaging.ImageFormat.Jpeg;

        }

        static string GetFilenameExtension(System.Drawing.Imaging.ImageFormat format)
        {
            return System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders()
                                 .First(x => x.FormatID == format.Guid)
                                 .FilenameExtension
                                 .Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                                 .First()
                                 .Trim('*');
        }
        public static void Set(string wpaper, Style style)
        {
            System.Drawing.Image img = System.Drawing.Image.FromFile(wpaper);

            System.Drawing.Imaging.ImageFormat imageFormat;
            GetImageFormat(img, out imageFormat);
            string ext = GetFilenameExtension(imageFormat);

            img.Save(wpaper + ext);

            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);
            if (style == Style.Stretched)
            {
                key.SetValue(@"WallpaperStyle", 2.ToString());
                key.SetValue(@"TileWallpaper", 0.ToString());
            }
            if (style == Style.Centered)
            {
                key.SetValue(@"WallpaperStyle", 0.ToString());
                key.SetValue(@"TileWallpaper", 0.ToString());
            }
            if (style == Style.Tiled)
            {
                key.SetValue(@"WallpaperStyle", 0.ToString());
                key.SetValue(@"TileWallpaper", 1.ToString());
            }
            if (style == Style.Filled)
            {
                key.SetValue(@"WallpaperStyle", 10.ToString());
                key.SetValue(@"TileWallpaper", 0.ToString());
            }
            if (style == Style.Fitted)
            {
                key.SetValue(@"WallpaperStyle", 6.ToString());
                key.SetValue(@"TileWallpaper", 0.ToString());
            }


            SystemParametersInfo(SPI_SETDESKWALLPAPER,
                0,
                wpaper,
                SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
        }
    }
}

