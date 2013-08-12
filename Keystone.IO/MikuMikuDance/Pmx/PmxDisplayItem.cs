using System;
using System.IO;

namespace Linearstar.Keystone.IO.MikuMikuDance
{
	public abstract class PmxDisplayItem
	{
		public abstract PmxDisplayItemKind Kind
		{
			get;
		}

		public static PmxDisplayItem Parse(BinaryReader br, PmxDocument doc)
		{
			switch ((PmxDisplayItemKind)br.ReadByte())
			{
				case PmxDisplayItemKind.Bone:
					return new PmxBoneDisplayItem
					{
						Bone = doc.ReadBone(br),
					};
				case PmxDisplayItemKind.Morph:
					return new PmxMorphDisplayItem
					{
						Morph = doc.ReadMorph(br),
					};
				default:
					throw new InvalidOperationException();
			}
		}

		public virtual void Write(BinaryWriter bw, PmxDocument doc, PmxIndexCache cache)
		{
			bw.Write((byte)this.Kind);
		}
	}

	public class PmxBoneDisplayItem : PmxDisplayItem
	{
		public override PmxDisplayItemKind Kind
		{
			get
			{
				return PmxDisplayItemKind.Bone;
			}
		}

		public PmxBone Bone
		{
			get;
			set;
		}

		public override void Write(BinaryWriter bw, PmxDocument doc, PmxIndexCache cache)
		{
			base.Write(bw, doc, cache);
			cache.Write(this.Bone);
		}
	}

	public class PmxMorphDisplayItem : PmxDisplayItem
	{
		public override PmxDisplayItemKind Kind
		{
			get
			{
				return PmxDisplayItemKind.Morph;
			}
		}

		public PmxMorph Morph
		{
			get;
			set;
		}

		public override void Write(BinaryWriter bw, PmxDocument doc, PmxIndexCache cache)
		{
			base.Write(bw, doc, cache);
			cache.Write(this.Morph);
		}
	}
}
