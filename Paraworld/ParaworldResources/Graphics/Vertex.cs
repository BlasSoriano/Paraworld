using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paraworld.Resources.Graphics
{
    public class Vertex
    {
        public const float MAX_VALUE_13BITS = 8191.0f;

        public float X;
        public float Y;
        public float Z;

        public Vertex() : this(0.0f, 0.0f, 0.0f) { }

        public Vertex(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public bool Write(BinaryWriter bw)
        {
            bw.Write(X);
            bw.Write(Y);
            bw.Write(Z);
            return true;
        }

        public bool Read(BinaryReader br)
        {
            X = br.ReadSingle();
            Y = br.ReadSingle();
            Z = br.ReadSingle();
            return true;
        }

        public static Vertex FromReader(BinaryReader br)
        {
            Vertex v = new Vertex();
            v.Read(br);
            return v;
        }

        public static Vertex FromUInt64(ulong packedBits, BoundingBox boundingBox)
        {
            int ix, iy, iz;
            float x, y, z;

            ix = (int)((packedBits >> 0) & 0x1FFF);
            iy = (int)((packedBits >> 13) & 0x1FFF);
            iz = (int)((packedBits >> 26) & 0x1FFF);

            x = ix * (boundingBox.max.X - boundingBox.min.X) / MAX_VALUE_13BITS + boundingBox.min.X;
            y = iy * (boundingBox.max.Y - boundingBox.min.Y) / MAX_VALUE_13BITS + boundingBox.min.Y;
            z = ix * (boundingBox.max.Z - boundingBox.min.Z) / MAX_VALUE_13BITS + boundingBox.min.Z;

            return new Vertex(x, y, z);
        }
    }
}
