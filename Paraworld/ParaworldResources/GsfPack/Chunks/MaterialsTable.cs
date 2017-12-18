using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paraworld.Resources.GsfPack.Chunks
{
    public class MaterialsTable : BaseChunkList
    {
        public const int COUNT_DEFAULT = 1000;

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
            entry = new Entry();    // Empty entry
            for (int i = entries.Count; i < COUNT_DEFAULT; i++)
            {
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
            // Consume bytes from reader for empty entries, just in case
            if (c < COUNT_DEFAULT)
            {
                int bytesCount = (COUNT_DEFAULT - c) * Entry.FIXED_SIZE;
                br.ReadBytes(bytesCount);
            }
            return true;
        }



        public class Entry : BaseChunk
        {
            public const int FIXED_SIZE = 24;

            public int attributes1;
            public int attributes2;
            public int? textureFilename1Offset = null;
            public int? textureFilename2Offset = null;
            public int? textureFilename3Offset = null;
            public int unknown;


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
                attributes1 = br.ReadInt32();
                attributes2 = br.ReadInt32();
                textureFilename1Offset = GsfPackage.ReadRelativeOffset(br);
                textureFilename2Offset = GsfPackage.ReadRelativeOffset(br);
                textureFilename3Offset = GsfPackage.ReadRelativeOffset(br);
                unknown = br.ReadInt32();
                return true;
            }

            public override bool Write(BinaryWriter bw)
            {
                bw.Write(attributes1);
                bw.Write(attributes2);
                GsfPackage.WriteNullablePointer(bw, textureFilename1Offset);
                GsfPackage.WriteNullablePointer(bw, textureFilename2Offset);
                GsfPackage.WriteNullablePointer(bw, textureFilename3Offset);
                bw.Write(unknown);
                return true;
            }

            public Graphics.Material GetMaterial(BinaryReader br)
            {
                Graphics.Material material = new Graphics.Material();
                material.attributes1 = attributes1;
                material.attributes2 = attributes2;
                material.unknown = unknown;
                if (textureFilename1Offset != null)
                {
                    br.BaseStream.Seek((long)textureFilename1Offset, SeekOrigin.Begin);
                    material.textureFilename1 = Text.ReadText(br);
                }
                if (textureFilename2Offset != null)
                {
                    br.BaseStream.Seek((long)textureFilename2Offset, SeekOrigin.Begin);
                    material.textureFilename2 = Text.ReadText(br);
                }
                if (textureFilename3Offset != null)
                {
                    br.BaseStream.Seek((long)textureFilename3Offset, SeekOrigin.Begin);
                    material.textureFilename3 = Text.ReadText(br);
                }
                return material;
            }
        }
    }
}
