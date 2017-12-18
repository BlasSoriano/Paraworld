using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paraworld.Resources.GsfPack.Chunks
{
    public class ObjectsTable : BaseChunkList
    {
        public Entry this[int index]
        {
            get
            {
                if (entries == null || entries.Count < index) return null;
                return (Entry)entries[index];
            }
        }

        public override bool Write(BinaryWriter bw)
        {
            if (entries == null) return true;
            Entry entry;
            for (int i = 0; i < entries.Count; i++)
            {
                entry = (Entry)entries[i];
                entry.Write(bw);
            }
            return true;
        }

        public override bool Read(BinaryReader br)
        {
            int c = GetCount();     // Will throw an exception if count is not yet set
            entries = new List<BaseChunk>();
            Entry entry;
            for (int i = 0; i < c; i++)
            {
                entry = new Entry();
                entry.Read(br);
                entries.Add(entry);
            }
            return true;
        }


        public class Entry : BaseChunk
        {
            public const int FIXED_SIZE = 84;

            public int objectType;
            public int nameAddress;
            public int? data1Address;
            public int data1Count;
            public int? materialDefsAddress;
            public int materialDefsCount;
            public int unk1;
            public int unk2;
            public int unk3;
            public int unk4;
            public int unk5;
            public int unk6;
            public int unk7;
            public float minX;
            public float minY;
            public float minZ;
            public float maxX;
            public float maxY;
            public float maxZ;
            public int? childDefsAddress;
            public int childsDefsCount;



            public override bool IsFixedSize()
            {
                return true;
            }

            public override int GetSize()
            {
                return FIXED_SIZE;
            }

            public override bool Write(BinaryWriter bw)
            {
                bw.Write(objectType);
                bw.Write(nameAddress);
                GsfPackage.WriteNullablePointer(bw, data1Address);
                bw.Write(data1Count);
                GsfPackage.WriteNullablePointer(bw, materialDefsAddress);
                bw.Write(materialDefsCount);
                bw.Write(unk1);
                bw.Write(unk2);
                bw.Write(unk3);
                bw.Write(unk4);
                bw.Write(unk5);
                bw.Write(unk6);
                bw.Write(unk7);
                bw.Write(minX);
                bw.Write(minY);
                bw.Write(minZ);
                bw.Write(maxX);
                bw.Write(maxY);
                bw.Write(maxZ);
                GsfPackage.WriteNullablePointer(bw, childDefsAddress);
                bw.Write(childsDefsCount);
                return true;
            }

            public override bool Read(BinaryReader br)
            {
                objectType = br.ReadInt32();
                nameAddress = (int)GsfPackage.ReadRelativeOffset(br);
                data1Address = GsfPackage.ReadRelativeOffset(br);
                data1Count = br.ReadInt32();
                materialDefsAddress = GsfPackage.ReadRelativeOffset(br);
                materialDefsCount = br.ReadInt32();
                unk1 = br.ReadInt32();
                unk2 = br.ReadInt32();
                unk3 = br.ReadInt32();
                unk4 = br.ReadInt32();
                unk5 = br.ReadInt32();
                unk6 = br.ReadInt32();
                unk7 = br.ReadInt32();
                minX = br.ReadSingle();
                minY = br.ReadSingle();
                minZ = br.ReadSingle();
                maxX = br.ReadSingle();
                maxY = br.ReadSingle();
                maxZ = br.ReadSingle();
                childDefsAddress = GsfPackage.ReadRelativeOffset(br);
                childsDefsCount = br.ReadInt32();
                return true;
            }

            public Graphics.Model GetModel(BinaryReader br)
            {
                Graphics.Model model = new Graphics.Model();

                return model;
            }
        }
    }
}
