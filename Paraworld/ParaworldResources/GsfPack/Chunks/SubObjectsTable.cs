using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paraworld.Resources.GsfPack.Chunks
{
    public class SubObjectsTable : BaseChunkList
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
            public const int FIXED_SIZE = 28;

            public int objectType;
            public int nameAddress;
            public int? data1Address;
            public int data1Count;
            public int? data2Address;
            public int data2Count;


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
                GsfPackage.WriteNullablePointer(bw, data2Address);
                bw.Write(data2Count);
                return true;
            }

            public override bool Read(BinaryReader br)
            {
                objectType = br.ReadInt32();
                nameAddress = br.ReadInt32();
                data1Address = GsfPackage.ReadRelativeOffset(br);
                data1Count = br.ReadInt32();
                data2Address = GsfPackage.ReadRelativeOffset(br);
                data2Count = br.ReadInt32();
                return true;
            }
        }
    }
}
