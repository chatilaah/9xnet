namespace W9xNET.Explorer.Models
{
    /// <summary>
    /// Runtime bitmap face data.
    /// </summary>
    struct RtFaceData
    {
        public RtFaceData(int width, int height)
        {
            Image = new Bitmap(width, height);
        }

        public readonly Bitmap Image;

        public int X { get; set; } = 0;

        public int Y { get; set; } = 0;

        public int Top { get => X; set => X = value; }

        public int Left { get => Y; set => Y = value; }

        /// <summary>
        /// Gets the height of this bitmap.
        /// </summary>
        public int Height => Image.Height;

        /// <summary>
        /// Gets the width of this bitmap.
        /// </summary>
        public int Width => Image.Width;

        /// <summary>
        /// Gets the width and height of this bitmap.
        /// </summary>
        public Size Size => Image.Size;
    }
}