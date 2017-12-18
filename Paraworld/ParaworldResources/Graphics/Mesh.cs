using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paraworld.Resources.Graphics
{
    public class Mesh
    {
        public string Name;
        public BoundingBox BBox;
        public List<Vertex> Vertices;
        public List<Triangle> Triangles;
        public List<TextureCoords> UVMap;
        public string TextureName;

        public Mesh() : this(new List<Vertex>(), new List<Triangle>()) { }

        public Mesh(List<Vertex> vertices, List<Triangle> triangles)
        {
            Vertices = vertices;
            Triangles = triangles;
        }

        public Mesh(List<Vertex> vertices, List<Triangle> triangles, List<TextureCoords> textureVertices)
        {
            Vertices = vertices;
            Triangles = triangles;
            UVMap = textureVertices;
        }

        public Mesh(BoundingBox bBox, List<Vertex> vertices, List<Triangle> triangles, List<TextureCoords> textureVertices)
        {
            BBox = bBox;
            Vertices = vertices;
            Triangles = triangles;
            UVMap = textureVertices;
        }
    }
}
