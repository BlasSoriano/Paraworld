using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paraworld.Resources.GsfPack.Chunks
{
    public class Def : BaseChunk
    {
        public uint attributes1;
        public uint attributes2;
        public uint guid;
        public BaseChunk nextChunk;

        public override bool Write(BinaryWriter bw)
        {
            throw new NotImplementedException();
        }

        public override bool Read(BinaryReader br)
        {
            attributes1 = br.ReadUInt32();
            attributes2 = br.ReadUInt32();
            guid = br.ReadUInt32();
            nextChunk = SetNextChunk(br, attributes1);
            if (nextChunk == null) return true;
            return nextChunk.Read(br);
        }

        // TODO: Add new chunk types when studied
        private BaseChunk SetNextChunk(BinaryReader br, uint attributes1)
        {
            switch (attributes1)
            {
                case 0x00000000:                        // MeshDef v0
                    return new MeshDefV0();
                case 0x80000000:                        // MeshDef v2
                    return new MeshDefV2();
                case 0x00000005:                        // ???
                    return new NextChunkType00();
                case 0x00000002:                        // Investigate in all_special_weapons.gsf
                default:
                    return null;
            }
        }
    }
}
