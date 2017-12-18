using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paraworld.Resources.GsfPack.Chunks
{
    // TODO: Study if the 4 floats are a quaternion, and implement the needed methods
    public class TransformationData : BaseChunk
    {
        public Graphics.BoundingBox boundingBox;
        public float d1;
        public float d2;
        public float d3;
        public float d4;

        public override bool Write(BinaryWriter bw)
        {
            throw new NotImplementedException();
        }

        public override bool Read(BinaryReader br)
        {
            boundingBox = Graphics.BoundingBox.FromReader(br);
            d1 = br.ReadSingle();
            d2 = br.ReadSingle();
            d3 = br.ReadSingle();
            d4 = br.ReadSingle();
            return true;
        }

        public static TransformationData FromReader(BinaryReader br)
        {
            TransformationData td = new TransformationData();
            td.Read(br);
            return td;
        }
    }
}
