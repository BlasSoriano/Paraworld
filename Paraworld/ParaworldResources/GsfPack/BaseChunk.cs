using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paraworld.Resources.GsfPack
{
    public abstract class BaseChunk
    {
        public const int CURRENT_SAVE_VERSION = 1;

        protected byte[] data;
        protected bool isDataCached = false;
        private int fixedDataSize = -1;

        // Data size
        public virtual bool IsFixedSize()
        {
            return false;
        }
        public virtual int GetSize()
        {
            if (IsFixedSize()) return fixedDataSize;
            if (!isDataCached) data = GetData();
            return data.Length;
        }

        // Write data
        public virtual void WriteBegin(BinaryWriter bw) { }
        public abstract bool Write(BinaryWriter bw);
        public virtual void WriteEnd(BinaryWriter bw) { }
        public virtual byte[] GetData()
        {
            byte[] data = null;
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                Write(bw);
                data = ms.ToArray();
            }
            isDataCached = true;
            return data;
        }

        // Read data
        public virtual void ReadBegin(BinaryReader br) { }
        public abstract bool Read(BinaryReader br);
        public virtual void ReadEnd(BinaryReader br) { }
        public virtual bool Parse(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("Data cannot be null", "data");
            if (IsFixedSize() && data.Length != GetSize())
                throw new ArgumentException("Data must be " + GetSize().ToString() + " bytes length", "data");
            using (MemoryStream ms = new MemoryStream(data))
            using (BinaryReader br = new BinaryReader(ms))
            {
                return Read(br);
            }
        }
    }
}
