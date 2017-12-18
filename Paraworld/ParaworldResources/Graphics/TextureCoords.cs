using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paraworld.Resources.Graphics
{
    public class TextureCoords
    {
        public const float MAX_VALUE_08BITS = 255.0f;

        public float U;
        public float V;

        public TextureCoords() : this(0.0f, 0.0f) { }

        public TextureCoords(float u, float v)
        {
            U = u;
            V = v;
        }

        public bool Write(BinaryWriter bw)
        {
            bw.Write(U);
            bw.Write(V);
            return true;
        }

        public bool Read(BinaryReader br)
        {
            U = br.ReadSingle();
            V = br.ReadSingle();
            return true;
        }

        public static TextureCoords FromVertex(Vertex v)
        {
            return new TextureCoords(v.X, v.Y);
        }

        public static TextureCoords FromUInt64(ulong packedBits)
        {
            int iu, iv;
            float u, v;

            iu = (int)((packedBits >> 48) & 0x00FF);
            iv = (int)((packedBits >> 56) & 0x00FF);

            u = iu / MAX_VALUE_08BITS;
            v = iv / MAX_VALUE_08BITS;

            return new TextureCoords(u, v);
        }
    }
}
