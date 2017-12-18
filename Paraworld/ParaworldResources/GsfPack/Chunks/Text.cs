using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Paraworld.Helpers.Text;

namespace Paraworld.Resources.GsfPack.Chunks
{
    public class Text : BaseChunk
    {
        public string text;
        
        // TODO: 
        public override bool Write(BinaryWriter bw)
        {
            throw new NotImplementedException();
        }

        public override bool Read(BinaryReader br)
        {
            long currentAddress = br.BaseStream.Position;
            int dummy = br.ReadInt32();
            if (dummy  != 1)
            {
                // If found a different value, please tell me to study the possibilities
                throw new InvalidDataException("Unexpected data reading Text chunk; expected 0x00000001, but found " + dummy.ToString("X8"));
            }
            int bytesUsed = br.ReadInt32();
            text = CText.ReadP4bAsciiString(br);
            // Consume padding bytes, just in case
            br.BaseStream.Seek(currentAddress + bytesUsed + 8, SeekOrigin.Begin);
            return true;
        }

        public static string ReadText(BinaryReader br)
        {
            Text textChunk = new Text();
            textChunk.Read(br);
            return textChunk.text;
        }
    }
}
