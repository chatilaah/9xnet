using System.IO;
using WORD = System.UInt16;

namespace W9xNET.RcFileReader.Extensions
{
    internal static class BinaryReaderExtensions
    {
        public static object ReadObject(this BinaryReader br)
        {
            object obj;
            if (br.PeekChar() == 0xFFFF)
            {
                obj = (WORD)br.ReadUInt16();
                obj = (WORD)br.ReadUInt16();
            }
            else
            {
                obj = GetString(br);
            }

            return obj;
        }

        public static string GetString(this BinaryReader reader)
        {
            string text = "";
            while (true)
            {
                char c = reader.ReadChar();
                if (c == '\0') break;
                text += c;
            }

            return text;
        }
    }
}