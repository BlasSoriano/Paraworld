using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paraworld.Resources.GsfPack.Chunks
{
    public class ChildsDef : BaseChunkList
    {
        public List<int> childOffsets;

        public override bool Write(BinaryWriter bw)
        {
            if (entries == null) return true;
            Entry entry;
            for (int i = 0; i < entries.Count; i++)
            {
                entry = (Entry)entries[i];
                entry.Write(bw);
            }
            for (int i = 0; i < entries.Count; i++)
            {
                entry = (Entry)entries[i];
                if (entry.childsAddress != null)
                {
                    for (int j = 0; j < entry.childsAddress.Count; j++)
                    {
                        GsfPackage.WriteNullablePointer(bw, entry.childsAddress[j].Item1);
                        bw.Write(entry.childsAddress[j].Item2);
                    }
                }
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
            for (int i = 0; i < c; i++)
            {
                entry = (Entry)entries[i];
                if (entry.defsAddress != null && entry.count > 0)
                {
                    br.BaseStream.Seek(entry.defsAddress.Value, SeekOrigin.Begin);
                    for (int j = 0; j < entry.count; j++)
                    {
                        int address = (int)GsfPackage.ReadRelativeOffset(br);
                        int val = br.ReadInt32();
                        entry.childsAddress.Add(Tuple.Create(address, val));
                    }
                }
            }
            return true;
        }


        public class Entry : BaseChunk
        {
            public const int FIXED_SIZE = 12;

            public int objectsTableAddress;
            public int? defsAddress;
            public int count;
            public List<Tuple<int, int>> childsAddress;


            public override bool IsFixedSize()
            {
                return true;
            }

            public override int GetSize()
            {
                return FIXED_SIZE;
            }

            public override bool Read(BinaryReader br)
            {
                objectsTableAddress = (int)GsfPackage.ReadRelativeOffset(br);
                defsAddress = GsfPackage.ReadRelativeOffset(br);
                count = br.ReadInt32();
                childsAddress = new List<Tuple<int, int>>();
                return true;
            }

            public override bool Write(BinaryWriter bw)
            {
                bw.Write(objectsTableAddress);
                GsfPackage.WriteNullablePointer(bw, defsAddress);
                bw.Write(count);
                return true;
            }
        }
    }
}
