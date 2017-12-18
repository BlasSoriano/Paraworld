using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Paraworld.Helpers.Text;

namespace Paraworld.Resources.GsfPack.Chunks
{
    // TODO: Study the props, so it can be parsed all the tree
    public class NodesTree : BaseChunk
    {
        public string name;

        public override byte[] GetData()
        {
            byte[] data = null;
            BuildTree();
            isDataCached = true;
            return data;
        }

        public override bool Write(BinaryWriter bw)
        {
            return true;
        }

        public override bool Read(BinaryReader br)
        {
            name = CText.ReadP4bNtAsciiString(br);
            return true;
        }

        private void BuildTree()
        {

        }
    }
}
