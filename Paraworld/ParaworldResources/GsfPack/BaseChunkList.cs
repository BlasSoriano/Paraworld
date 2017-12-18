using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paraworld.Resources.GsfPack
{
    public abstract class BaseChunkList : BaseChunk
    {
        protected bool isCountSet = false;
        protected int count = -1;
        protected List<BaseChunk> entries;

        public virtual void SetCount(int count)
        {
            if (entries == null) entries = new List<BaseChunk>();
            this.count = count;
            isCountSet = true;
        }

        public virtual int GetCount()
        {
            if (!isCountSet || entries == null)
            {
                throw new ArgumentNullException(string.Empty, "The chunk list cannot be null; use SetCount() before calling Read().");
            }
            return Math.Max(count, entries.Count);
        }
    }
}
