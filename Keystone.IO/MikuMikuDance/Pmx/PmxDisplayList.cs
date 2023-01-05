using System.Collections.Generic;

namespace Linearstar.Keystone.IO.MikuMikuDance.Pmx
{
    public class PmxDisplayList
    {
        public string Name { get; set; }

        public string EnglishName { get; set; }

        public bool IsSpecial { get; set; }

        public IList<PmxDisplayItem> Items { get; set; } = new List<PmxDisplayItem>();

        internal static PmxDisplayList Parse(ref BufferReader br, PmxDocument doc)
        {
            var displayList = new PmxDisplayList()
            {
                Name = br.ReadString(doc.Header),
                EnglishName = br.ReadString(doc.Header),
                IsSpecial = br.ReadBoolean(),
            };

            var items = br.ReadInt32();
            for (var i = 0; i < items; i++)
                displayList.Items.Add(PmxDisplayItem.Parse(ref br, doc));

            return displayList;
        }

        internal void Write(ref BufferWriter bw, PmxDocument doc, PmxIndexCache cache)
        {
            bw.Write(this.Name, doc.Header);
            bw.Write(this.EnglishName, doc.Header);
            bw.Write(this.IsSpecial);
            bw.Write(this.Items.Count);

            foreach (var item in this.Items)
                item.Write(ref bw, doc, cache);
        }
    }
}