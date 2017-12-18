using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paraworld.Resources.GsfPack.Chunks
{
    public class MeshData : BaseChunk
    {
        public Graphics.BoundingBox boundingBox;
        public int verticesCount;
        public int trianglesCount;
        public int? verticesAddress;
        public List<Graphics.Vertex> vertices;
        public List<Graphics.TextureCoords> textureVertices;
        public int? trianglesAddress;
        public List<Graphics.Triangle> triangles;
        public int vertexDataSize;
        public int unknown1Count;
        public int? unknown1Address;
        public List<short> unknown1;


        public MeshData()
        {
            boundingBox = new Graphics.BoundingBox();
            vertices = new List<Graphics.Vertex>();
            triangles = new List<Graphics.Triangle>();
            unknown1 = new List<short>();
        }

        public override bool Write(BinaryWriter bw)
        {
            throw new NotImplementedException();
        }

        public override bool Read(BinaryReader br)
        {
            if (verticesAddress != null)
            {
                br.BaseStream.Seek(verticesAddress.Value, SeekOrigin.Begin);
                for (int i = 0; i < verticesCount; i++)
                {
                    ulong packedBits = br.ReadUInt64();
                    Graphics.Vertex v = Graphics.Vertex.FromUInt64(packedBits, boundingBox);
                    vertices.Add(v);
                    Graphics.TextureCoords tc = Graphics.TextureCoords.FromUInt64(packedBits);
                    textureVertices.Add(tc);
                    if (vertexDataSize == 16)
                    {
                        ulong unknownBits = br.ReadUInt64();
                    }
                }
            }
            if (trianglesAddress != null)
            {
                br.BaseStream.Seek(trianglesAddress.Value, SeekOrigin.Begin);
                for (int i = 0; i < trianglesCount; i++)
                {
                    Graphics.Triangle t = Graphics.Triangle.FromReader(br);
                    triangles.Add(t);
                }
            }
            if (unknown1Address != null)
            {
                br.BaseStream.Seek(unknown1Address.Value, SeekOrigin.Begin);
                for (int i = 0; i < unknown1Count; i++)
                {
                    short val = br.ReadInt16();
                    unknown1.Add(val);
                }
            }
            return true;
        }

        public static MeshData ReadHeader(BinaryReader br)
        {
            MeshData md = new MeshData();
            md.vertices = new List<Graphics.Vertex>();
            md.textureVertices = new List<Graphics.TextureCoords>();
            md.triangles = new List<Graphics.Triangle>();
            md.unknown1 = new List<short>();
            md.boundingBox = Graphics.BoundingBox.FromReader(br);
            md.verticesCount = br.ReadInt32();
            md.trianglesCount = br.ReadInt32();
            md.verticesAddress = GsfPackage.ReadRelativeOffset(br);
            md.trianglesAddress = GsfPackage.ReadRelativeOffset(br);
            md.trianglesCount = br.ReadInt32();     // Yes, the same value again  :-)
            md.vertexDataSize = br.ReadInt32();
            md.unknown1Address = GsfPackage.ReadRelativeOffset(br);
            md.unknown1Count = br.ReadInt32();      // Same value as verticesCount, or 0
            return md;
        }
    }
}
