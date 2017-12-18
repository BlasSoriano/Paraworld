using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paraworld.Resources.GsfPack.Chunks
{
    public class MaterialDef : BaseChunk
    {
        public List<int> materialIndices;

        // TODO: 
        public override bool Write(BinaryWriter bw)
        {
            throw new NotImplementedException();
        }

        // TODO: Update when found the meaning for the 2 unknown values
        public override bool Read(BinaryReader br)
        {
            int pointerToContentsTable = (int)GsfPackage.ReadRelativeOffset(br);
            int pointerToMaterialsInfo = (int)GsfPackage.ReadRelativeOffset(br);
            int unknown1 = br.ReadInt32();       // Always 0x0C?
            int amount = br.ReadInt32();
            int unknown2 = br.ReadInt32();      // Always 0x01?
            materialIndices = new List<int>();
            for (int i = 0; i < amount; i++)
            {
                int index = br.ReadInt32();
                materialIndices.Add(index);
            }
            return true;
        }
    }
}
