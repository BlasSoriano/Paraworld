using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParaworldResources.Graphics
{
    public class BoundingBox
    {
        public Vertex min;
        public Vertex max;

        public BoundingBox() : this(new Vertex(), new Vertex()) { }

        public BoundingBox(Vertex min, Vertex max)
        {
            this.min = min;
            this.max = max;
        }

        public bool Write(BinaryWriter bw)
        {
            if (!min.Write(bw)) return false;
            if (!max.Write(bw)) return false;
            return true;
        }

        public bool Read(BinaryReader br)
        {
            min = new Vertex();
            max = new Vertex();
            if (!min.Read(br)) return false;
            if (!max.Read(br)) return false;
            return true;
        }

        public static BoundingBox FromReader(BinaryReader br)
        {
            BoundingBox bb = new BoundingBox();
            bb.Read(br);
            return bb;
        }
    }
}
