using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paraworld.Resources.GsfPack.Chunks
{
    public class MaterialsHeader : BaseChunk
    {
        public const int FIXED_SIZE = 12;

        public int materialsCount;
        public int materialsTableOffset;
        public int materialsTableEntriesCount = MaterialsTable.COUNT_DEFAULT;

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
            bw.Write(materialsCount);
            bw.Write(materialsTableOffset);
            bw.Write(materialsTableEntriesCount);
            return true;
        }

        public override bool Read(BinaryReader br)
        {
            materialsCount = br.ReadInt32();
            materialsTableOffset = (int)GsfPackage.ReadRelativeOffset(br);
            materialsTableEntriesCount = br.ReadInt32();
            return true;
        }
    }
}
