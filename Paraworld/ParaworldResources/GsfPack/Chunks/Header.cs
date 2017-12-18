using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paraworld.Resources.GsfPack.Chunks
{
    public class Header : BaseChunk
    {
        public const int SIGNATURE = 0x00465347;       // "GSF\0"
        public const int FIXED_SIZE = 12;

        public int SaveVersion;
        public int ContentsTableOffset;

        public override bool IsFixedSize()
        {
            return true;
        }

        public override int GetSize()
        {
            return FIXED_SIZE;
        }

        public override bool Write(BinaryWriter bw)
        {
            bw.Write(SIGNATURE);
            bw.Write(CURRENT_SAVE_VERSION);
            bw.Write(ContentsTableOffset);
            return true;
        }

        public override bool Read(BinaryReader br)
        {
            int signature = br.ReadInt32();
            if (signature != SIGNATURE)
                throw new InvalidDataException("Signature does not match; expected " + SIGNATURE.ToString("X8") + ", found" + signature.ToString("X8"));
            SaveVersion = br.ReadInt32();
            if (SaveVersion > CURRENT_SAVE_VERSION)
                throw new InvalidDataException("Unsupported format; current format version is " + CURRENT_SAVE_VERSION.ToString("X8") + ", found " + SaveVersion.ToString("X8"));
            ContentsTableOffset = br.ReadInt32();
            return true;
        }
    }
}
