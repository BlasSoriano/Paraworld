using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paraworld.Helpers.Text
{
    /// <summary>
    /// Helper class to handle with strings saved from C programs to binary files
    /// </summary>
    public static class CText
    {
        /// <summary>
        /// Read from a BinaryReader a Null Terminated Ascii string, prefixed with 4 bytes specifying the string length
        /// </summary>
        /// <remarks>The Null Terminator '0x00' is discarded</remarks>
        /// <param name="br">The BinaryReader</param>
        /// <returns>A string</returns>
        public static string ReadP4bNtAsciiString(BinaryReader br)
        {
            int charsCount = br.ReadInt32();
            byte[] b = br.ReadBytes(charsCount);
            return Encoding.ASCII.GetString(b, 0, charsCount - 1);
        }

        /// <summary>
        /// Read from a BinaryReader an Ascii string, prefixed with 4 bytes specifying the string length
        /// </summary>
        /// <remarks>The string is not null terminated</remarks>
        /// <param name="br">The BinaryReader</param>
        /// <returns>A string</returns>
        public static string ReadP4bAsciiString(BinaryReader br)
        {
            int charsCount = br.ReadInt32();
            if (charsCount == -1)
            {
                return ReadNTAsciiString(br);
            }
            if (charsCount == 0)
            {
                return null;
            }
            byte[] b = br.ReadBytes(charsCount);
            return Encoding.ASCII.GetString(b, 0, charsCount);
        }

        /// <summary>
        /// Read from a BinaryReader a Null Terminated Ascii string
        /// </summary>
        /// <remarks>The Null Terminator '0x00' is discarded</remarks>
        /// <param name="br">The BinaryReader</param>
        /// <returns>A string</returns>
        public static string ReadNTAsciiString(BinaryReader br)
        {
            StringBuilder sb = new StringBuilder();
            bool foundNullTerminator = false;
            while (!foundNullTerminator)
            {
                byte b = br.ReadByte();
                if (b == 0) foundNullTerminator = true;
                else sb.Append((char)b);
            }
            return sb.ToString();
        }
    }
}
