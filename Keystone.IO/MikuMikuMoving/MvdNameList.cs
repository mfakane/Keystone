using System.Collections.Generic;
using System.IO;

namespace Linearstar.Keystone.IO.MikuMikuMoving
{
	public class MvdNameList : MvdSection
	{
		public Dictionary<int, string> Names
		{
			get;
			private set;
		}

		public MvdNameList()
			: base(MvdTag.NameList)
		{
			this.Names = new Dictionary<int, string>();
		}

		protected override void Read(MvdDocument document, BinaryReader br)
		{
			for (int i = 0; i < this.RawCount; i++)
				this.Names.Add(br.ReadInt32(), document.Encoding.GetString(br.ReadSizedBuffer()));
		}

		public override void Write(MvdDocument document, BinaryWriter bw)
		{
			this.MinorType = 0;
			this.RawCount = this.Names.Count;

			base.Write(document, bw);

			foreach (var i in this.Names)
			{
				var buf = document.Encoding.GetBytes(i.Value);

				bw.Write(i.Key);
				bw.Write(buf.Length);
				bw.Write(buf);
			}
		}
	}
}
