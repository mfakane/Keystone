using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Linearstar.Keystone.IO.MikuMikuDance
{
	public class PmdMorph
	{
		public string Name
		{
			get;
			set;
		}

		public PmdMorphKind Kind
		{
			get;
			set;
		}

		/// <summary>
		/// 移動させる頂点のインデックス一覧を取得します。
		/// これは Kind == None の場合、モデルの頂点インデックスです。
		/// それ以外の場合、Kind == None の Indices におけるインデックスです。
		/// </summary>
		public List<ushort> Indices
		{
			get;
			private set;
		}

		public List<float[]> Offsets
		{
			get;
			private set;
		}

		public PmdMorph()
		{
			this.Indices = new List<ushort>();
			this.Offsets = new List<float[]>();
		}

		public static PmdMorph Parse(BinaryReader br)
		{
			var rt = new PmdMorph
			{
				Name = PmdDocument.ReadPmdString(br, 20),
			};
			var count = br.ReadUInt32();

			rt.Kind = (PmdMorphKind)br.ReadByte();

			for (uint i = 0; i < count; i++)
			{
				rt.Indices.Add((ushort)br.ReadUInt32());
				rt.Offsets.Add(new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() });
			}

			return rt;
		}

		public void Write(BinaryWriter bw)
		{
			PmdDocument.WritePmdString(bw, this.Name, 20);
			bw.Write((uint)this.Offsets.Count);
			bw.Write((byte)this.Kind);
			this.Indices.Zip(this.Offsets, Tuple.Create).ForEach(_ =>
			{
				bw.Write((uint)_.Item1);
				_.Item2.ForEach(bw.Write);
			});
		}
	}
}
