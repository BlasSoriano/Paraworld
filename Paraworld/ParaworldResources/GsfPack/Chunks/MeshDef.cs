using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paraworld.Resources.GsfPack.Chunks
{
    public class MeshDefV0 : MeshDef
    {
        public override bool Write(BinaryWriter bw)
        {
            throw new NotImplementedException();
        }

        public override bool Read(BinaryReader br)
        {
            unparsed1 = br.ReadBytes(64);
            formatVersion = br.ReadInt32();
            return base.Read(br);
        }
    }

    public class MeshDefV2 : MeshDef
    {
        public int unknown1;


        public override bool Write(BinaryWriter bw)
        {
            throw new NotImplementedException();
        }

        public override bool Read(BinaryReader br)
        {
            unparsed1 = br.ReadBytes(64);
            formatVersion = br.ReadInt32();
            unknown1 = br.ReadInt32();
            return base.Read(br);
        }
    }

    public class MeshDef : BaseChunk
    {
        public byte[] unparsed1;        // 16 * 4 = 64 bytes
        public int formatVersion;       // Should be 2
        public int? offset;
        public int unknown2;
        public List<MeshData> meshesData;
        public List<short> frameIndices;


        public override bool Write(BinaryWriter bw)
        {
            throw new NotImplementedException();
        }

        public override bool Read(BinaryReader br)
        {
            offset = GsfPackage.ReadRelativeOffset(br);
            unknown2 = br.ReadInt32();
            meshesData = new List<MeshData>();
            frameIndices = new List<short>();
            if (offset != null)
            {
                br.BaseStream.Seek(offset.Value, SeekOrigin.Begin);
                Graphics.BoundingBox bb = Graphics.BoundingBox.FromReader(br);
                // TODO: Use this bounding box in addition (transformation) to the ones from each mesh
                int amount1 = br.ReadInt32();
                int meshesAddress = GsfPackage.ReadRelativeOffset(br).Value;
                int amount2 = br.ReadInt32();
                int frameIndicesAddress = GsfPackage.ReadRelativeOffset(br).Value;
                int amount3 = br.ReadInt32();
                br.BaseStream.Seek(meshesAddress, SeekOrigin.Begin);    // Usually should not be needed
                for (int i = 0; i < amount2; i++)
                {
                    MeshData md = MeshData.ReadHeader(br);
                    meshesData.Add(md);
                }
                for (int i = 0; i < amount2; i++)
                {
                    meshesData[i].Read(br);
                }
                br.BaseStream.Seek(frameIndicesAddress, SeekOrigin.Begin);
                for (int i = 0; i < amount3; i++)
                {
                    short index = br.ReadInt16();
                    frameIndices.Add(index);
                }
            }
            return true;
        }
    }
}
