﻿using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace W9xNET.User32.Gfx
{
    /// <summary>
    /// High performance bitmap class.
    /// Retrieved from https://stackoverflow.com/questions/24701703/c-sharp-faster-alternatives-to-setpixel-and-getpixel-for-bitmaps-for-windows-f
    /// </summary>
    public class DirectBitmap : IDisposable
    {
        public Bitmap Bitmap { get; private set; }

        public Int32[] Bits { get; private set; }

        public bool Disposed { get; private set; }

        public int Height { get; private set; }

        public int Width { get; private set; }

        protected GCHandle BitsHandle { get; private set; }

        public DirectBitmap(int width, int height)
        {
            Width = width;
            Height = height;
            Bits = new Int32[width * height];
            BitsHandle = GCHandle.Alloc(Bits, GCHandleType.Pinned);
            Bitmap = new Bitmap(width, height, width * 4, PixelFormat.Format32bppPArgb, BitsHandle.AddrOfPinnedObject());
        }

        ~DirectBitmap() => Dispose();

        public void SetPixel(int x, int y, Color colour)
        {
            int index = x + (y * Width);
            int col = colour.ToArgb();

            Bits[index] = col;
        }

        public Color GetPixel(int x, int y)
        {
            int index = x + (y * Width);
            int col = Bits[index];
            Color result = Color.FromArgb(col);

            return result;
        }

        public void Dispose()
        {
            if (Disposed) return;
            Disposed = true;
            Bitmap.Dispose();
            BitsHandle.Free();
        }
    }
}