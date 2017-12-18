using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paraworld.Resources.GsfPack.Chunks
{
    // Temporary name
    public class NextChunkType00 : BaseChunk
    {
        public uint gui;
        public int dummyZero;
        public TransformationData transformationData;
        public int c1;
        public int? a1;
        public int c2;
        public int? a2;
        public int zero;
        public List<Type00Block> type00Blocks;
        public List<Type00Data> type00Datas;

        public override bool Write(BinaryWriter bw)
        {
            throw new NotImplementedException();
        }

        public override bool Read(BinaryReader br)
        {
            dummyZero = br.ReadInt32();
            gui = br.ReadUInt32();
            dummyZero = br.ReadInt32();
            transformationData = TransformationData.FromReader(br);
            c1 = br.ReadInt32();
            a1 = GsfPackage.ReadRelativeOffset(br);
            c1 = br.ReadInt32();
            a2 = GsfPackage.ReadRelativeOffset(br);
            c2 = br.ReadInt32();
            c2 = br.ReadInt32();
            zero = br.ReadInt32();
            type00Blocks = new List<Type00Block>();
            type00Datas = new List<Type00Data>();
            if (!ReadBlocks(br, a1, c1, type00Blocks)) return false;
            if (!ReadDatas(br, a2, c2)) return false;
            return true;
        }

        // Recursive
        private bool ReadBlocks(BinaryReader br, int? address, int amount, List<Type00Block> t00blocks)
        {
            if (amount > 0 && address != null)
            {
                List<Type00Block> t00NextBlocks = new List<Type00Block>();
                br.BaseStream.Seek(address.Value, SeekOrigin.Begin);
                for (int i = 0; i < amount; i++)
                {
                    Type00Block t0b = Type00Block.FromReader(br);
                    t00NextBlocks.Add(t0b);
                }
                for (int i = 0; i < t00NextBlocks.Count; i++)
                {
                    if (!ReadBlocks(br, t00NextBlocks[i].nextAddress, t00NextBlocks[i].nextAmount, t00NextBlocks)) return false;
                }
                t00blocks.AddRange(t00NextBlocks);
            }
            return true;
        }

        private bool ReadDatas(BinaryReader br, int? address, int amount)
        {
            if (amount > 0 && address != null)
            {
                br.BaseStream.Seek(address.Value, SeekOrigin.Begin);
                for (int i = 0; i < amount; i++)
                {
                    Type00Data t0d = Type00Data.FromReader(br);
                    type00Datas.Add(t0d);
                }
            }
            return true;
        }

        public class Type00Block
        {
            public uint gui;
            public int dummyZero;
            public TransformationData transformationData;
            public int nextAmount;
            public int? nextAddress;
            public int dummy;

            public bool Write(BinaryWriter bw)
            {
                throw new NotImplementedException();
            }

            public bool Read(BinaryReader br)
            {
                gui = br.ReadUInt32();
                dummyZero = br.ReadInt32();
                transformationData = TransformationData.FromReader(br);
                nextAmount = br.ReadInt32();
                nextAddress = GsfPackage.ReadRelativeOffset(br);
                nextAmount = br.ReadInt32();
                return true;
            }

            public static Type00Block FromReader(BinaryReader br)
            {
                Type00Block t0b = new Type00Block();
                t0b.Read(br);
                return t0b;
            }
        }

        public class Type00Data
        {
            public byte[] data;

            public bool Write(BinaryWriter bw)
            {
                throw new NotImplementedException();
            }

            public bool Read(BinaryReader br)
            {
                data = br.ReadBytes(64);
                return true;
            }

            public static Type00Data FromReader(BinaryReader br)
            {
                Type00Data t0d = new Type00Data();
                t0d.Read(br);
                return t0d;
            }
        }
    }
}
