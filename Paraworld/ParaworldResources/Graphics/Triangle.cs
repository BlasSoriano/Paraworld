using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paraworld.Resources.Graphics
{
    public class Triangle
    {
        public int P1;
        public int P2;
        public int P3;

        public Triangle() : this(0, 1, 2) { }

        public Triangle(short p1, short p2, short p3)
        {
            P1 = p1;
            P2 = p2;
            P3 = p3;
        }

        public bool Write(BinaryWriter bw)
        {
            bw.Write((short)P1);
            bw.Write((short)P2);
            bw.Write((short)P3);
            return true;
        }

        public bool Read(BinaryReader br)
        {
            P1 = br.ReadInt16();
            P2 = br.ReadInt16();
            P3 = br.ReadInt16();
            return true;
        }

        public static Triangle FromReader(BinaryReader br)
        {
            Triangle t = new Triangle();
            t.Read(br);
            return t;
        }
    }
}
