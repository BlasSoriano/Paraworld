using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paraworld.Resources.GsfPack.Chunks
{
    public class ContentsTable : BaseChunk
    {
        public const int FIXED_SIZE = 28;

        public int unknown1;
        public int objectsCount;
        public int subObjectsCount;
        public int unknown2;
        public int objectsTableOffset;
        public int subObjectsTableOffset;

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
            bw.Write(unknown1);
            bw.Write(objectsCount);
            bw.Write(unknown2);
            bw.Write(objectsTableOffset);
            bw.Write(objectsCount);
            bw.Write(subObjectsTableOffset);
            bw.Write(subObjectsCount);
            return true;
        }

        public override bool Read(BinaryReader br)
        {
            unknown1 = br.ReadInt32();
            objectsCount = br.ReadInt32();
            subObjectsCount = br.ReadInt32();
            unknown2 = br.ReadInt32();
            objectsTableOffset = br.ReadInt32();
            objectsCount = br.ReadInt32();
            subObjectsTableOffset = br.ReadInt32();
            subObjectsCount = br.ReadInt32();

            return true;
        }
    }
}
