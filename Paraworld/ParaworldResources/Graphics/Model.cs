using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paraworld.Resources.Graphics
{
    public class Model
    {
        public string name;
        public BoundingBox boundingBox;
        public List<int> materialIndices;
        public List<Tuple<int, int>> childsAddress;
        public List<Mesh> meshes;

        public Model()
        {
            boundingBox = new BoundingBox();
            materialIndices = new List<int>();
            childsAddress = new List<Tuple<int, int>>();
            meshes = new List<Mesh>();
        }
    }
}
