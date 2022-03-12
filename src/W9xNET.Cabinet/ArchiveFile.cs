using System;

namespace W9xNET.Cabinet
{
    public class ArchiveFile
    {
        public IntPtr Handle { get; set; }
        public string Name { get; set; } = "";
        public bool Found { get; set; }
        public int Length { get; set; }
        public byte[] Data { get; set; }
    }
}