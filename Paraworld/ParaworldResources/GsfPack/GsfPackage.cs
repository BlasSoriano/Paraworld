using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Paraworld.Resources.GsfPack.Chunks;
using Paraworld.Resources.Graphics;

namespace Paraworld.Resources
{
    // Graphics Set File for Paraworld game
    public class GsfPackage
    {
        public const uint NULL_POINTER = 0x80000000;

        #region Fields
        private string filename;
        #endregion Fields

        #region Properties
        public string Name { get; set; }
        public List<Model> Models { get; set; }
        public List<Animation> Animations { get; set; }
        public List<Material> Materials { get; set; }
        #endregion Properties

        #region Constructors
        public GsfPackage() : this("Unnamed") { }

        public GsfPackage(string name)
        {
            Name = name;
            Models = new List<Model>();
            Animations = new List<Animation>();
            Materials = new List<Material>();
        }
        #endregion Constructors

        #region Public Methods
        public static GsfPackage Read(string filename)
        {
            GsfPackage gsfp = new GsfPackage();
            gsfp.filename = filename;
            using (FileStream fs = File.OpenRead(filename))
            using (BinaryReader br = new BinaryReader(fs))
            {
                gsfp.Read(br);
            }
            return gsfp;
        }

        public bool Save(string filename)
        {
            bool result = false;
            string folder = Path.GetDirectoryName(filename);
            Directory.CreateDirectory(folder);      // Does nothing if already exists
            using (FileStream fs = File.Create(filename))
            using (BinaryWriter bw = new BinaryWriter(fs))
            {
                result = Write(bw);
            }
            return result;
        }

        public static int? ReadRelativeOffset(BinaryReader br)
        {
            int currentAddress = (int)br.BaseStream.Position;
            uint offset = br.ReadUInt32();
            if (offset == NULL_POINTER) return null;
            return (int)offset + currentAddress;
        }

        public static void WriteNullablePointer(BinaryWriter bw, int? pointer)
        {
            if (pointer == null) bw.Write(NULL_POINTER);
                else bw.Write(pointer.Value);
        }
        #endregion Public Methods

        #region Private Methods
        // TODO: Clean up this method, split in smaller parts, update it as more info is available
        private bool Read(BinaryReader br)
        {
            try
            {
                Header header = new Header();
                header.Read(br);
                NodesTree nodesTree = new NodesTree();
                nodesTree.Read(br);
                Name = nodesTree.name;
                br.BaseStream.Seek(header.ContentsTableOffset, SeekOrigin.Begin);
                ContentsTable contentsTable = new ContentsTable();
                contentsTable.Read(br);
                MaterialsHeader materialsHeader = new MaterialsHeader();
                materialsHeader.Read(br);
                ObjectsTable objectsTable = new ObjectsTable();
                objectsTable.SetCount(contentsTable.objectsCount);
                objectsTable.Read(br);
                SubObjectsTable subObjectsTable = new SubObjectsTable();
                subObjectsTable.SetCount(contentsTable.subObjectsCount);
                subObjectsTable.Read(br);

                // In theory, it could be read all the data from current stream position
                //      but it would be safer to read by using the already read offsets

                // Read the materials table
                MaterialsTable materialsTable = new MaterialsTable();
                materialsTable.SetCount(materialsHeader.materialsCount);
                br.BaseStream.Seek(materialsHeader.materialsTableOffset, SeekOrigin.Begin);
                materialsTable.Read(br);
                // Build the materials list
                for (int i = 0; i < materialsTable.GetCount(); i++)
                {
                    Material mat = materialsTable[i].GetMaterial(br);
                    Materials.Add(mat);
                }

                // TODO: Read data for all the subobjects


                // Read data for all the objects
                for (int i = 0; i < objectsTable.GetCount(); i++)
                {
                    Model model = new Model();
                    br.BaseStream.Seek(objectsTable[i].nameAddress, SeekOrigin.Begin);
                    model.name = Text.ReadText(br);
                    if (objectsTable[i].materialDefsAddress != null && objectsTable[i].materialDefsCount > 0)
                    {
                        br.BaseStream.Seek(objectsTable[i].materialDefsAddress.Value, SeekOrigin.Begin);
                        MaterialDef matDef = new MaterialDef();
                        matDef.Read(br);
                        model.materialIndices = matDef.materialIndices;
                    }
                    if (objectsTable[i].childDefsAddress != null && objectsTable[i].childsDefsCount > 0)
                    {
                        br.BaseStream.Seek(objectsTable[i].childDefsAddress.Value, SeekOrigin.Begin);
                        ChildsDef childsDef = new ChildsDef();
                        childsDef.SetCount(objectsTable[i].childsDefsCount);
                        childsDef.Read(br);
                        // TODO: Decide how to add it to the model
                    }
                    if (objectsTable[i].data1Address != null && objectsTable[i].data1Count > 0)
                    {
                        List<int> meshesAddress = new List<int>();
                        br.BaseStream.Seek(objectsTable[i].data1Address.Value, SeekOrigin.Begin);
                        for (int j = 0; j < objectsTable[i].data1Count; j++)
                        {
                            int address = (int)ReadRelativeOffset(br);
                            meshesAddress.Add(address);
                        }
                        for (int j = 0; j < objectsTable[i].data1Count; j++)
                        {
                            br.BaseStream.Seek(meshesAddress[j], SeekOrigin.Begin);
                            Def def = new Def();
                            if (!def.Read(br)) return false;
                            if (def.nextChunk != null && def.nextChunk is MeshDef)
                            {
                                MeshDef md = (MeshDef)def.nextChunk;
                                for (int k = 0; k < md.meshesData.Count; k++)
                                {
                                    MeshData data = md.meshesData[k];
                                    Mesh mesh = new Mesh(data.boundingBox, data.vertices, data.triangles, data.textureVertices);
                                    model.meshes.Add(mesh);
                                }
                            }
                        }
                    }
                    Models.Add(model);
                }
            }
            catch
            {
                // TODO: Handle here any error reading
                return false;
            }
            return true;
        }

        // TODO:
        private bool Write(BinaryWriter bw)
        {
            throw new NotImplementedException();
        }
        #endregion Private Methods
    }
}
