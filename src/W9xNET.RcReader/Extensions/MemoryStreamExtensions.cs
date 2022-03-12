using W9xNET.RcReader;

namespace W9xNET.RcFileReader.Extensions
{
    public static class MemoryStreamExtension
    {
        public static void ReadDwordAlignment(this MemoryStream inputStream)
        {
            System.UInt32 mod = ((System.UInt32)inputStream.Position & 3);
            if (mod > 0)
            {
                inputStream.Position += 4 - mod;
            }
        }

        public static short PeekWord(this MemoryStream inputStream)
        {
            long oldPos = inputStream.Position;
            var value = Utility.ReadStructure<System.Int16>(inputStream);
            inputStream.Position = oldPos;

            return value;
        }

        public static bool ReadSz(this MemoryStream inputStream, ref string str)
        {
            str = String.Empty;
            System.UInt16 w;
            while (true)
            {
                w = Utility.ReadStructure<System.UInt16>(inputStream);

                if (w == 0)
                    return true;
                str += (char)w;
            }

            return false;
        }
    }
}