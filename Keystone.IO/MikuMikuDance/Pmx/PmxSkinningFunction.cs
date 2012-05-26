using System;
using System.IO;

namespace Linearstar.Keystone.IO.MikuMikuDance
{
	public abstract class PmxSkinningFunction
	{
		public static PmxSkinningFunction Parse(BinaryReader br, PmxDocument doc, PmxSkinningKind kind)
		{
			PmxSkinningFunction rt;

			switch (kind)
			{
				case PmxSkinningKind.LinearBlendDeforming1:
					rt = new PmxLinearBlendDeforming1();

					break;
				case PmxSkinningKind.LinearBlendDeforming2:
					rt = new PmxLinearBlendDeforming2();

					break;
				case PmxSkinningKind.LinearBlendDeforming4:
					rt = new PmxLinearBlendDeforming4();

					break;
				case PmxSkinningKind.SphericalDeforming:
					rt = new PmxSphericalDeforming();

					break;
				default:
					throw new NotSupportedException();
			}

			rt.Read(br, doc);

			return rt;
		}

		public abstract void Read(BinaryReader br, PmxDocument doc);
		public abstract void Write(BinaryWriter bw, PmxDocument doc);
	}

	public class PmxLinearBlendDeforming1 : PmxSkinningFunction
	{
		public int Bone
		{
			get;
			set;
		}

		public PmxLinearBlendDeforming1()
		{
			this.Bone = -1;
		}

		public override void Read(BinaryReader br, PmxDocument doc)
		{
			this.Bone = doc.ReadIndex(br, PmxIndexKind.Bone);
		}

		public override void Write(BinaryWriter bw, PmxDocument doc)
		{
			doc.WriteIndex(bw, PmxIndexKind.Bone, this.Bone);
		}
	}

	public class PmxLinearBlendDeforming2 : PmxSkinningFunction
	{
		public int[] Bones
		{
			get;
			set;
		}

		public float Weight
		{
			get;
			set;
		}

		public PmxLinearBlendDeforming2()
		{
			this.Bones = new[] { -1, -1 };
		}

		public override void Read(BinaryReader br, PmxDocument doc)
		{
			this.Bones = new[] { doc.ReadIndex(br, PmxIndexKind.Bone), doc.ReadIndex(br, PmxIndexKind.Bone) };
			this.Weight = br.ReadSingle();
		}

		public override void Write(BinaryWriter bw, PmxDocument doc)
		{
			this.Bones.ForEach(_ => doc.WriteIndex(bw, PmxIndexKind.Bone, _));
			bw.Write(this.Weight);
		}
	}

	public class PmxLinearBlendDeforming4 : PmxSkinningFunction
	{
		public int[] Bones
		{
			get;
			set;
		}

		public float[] Weights
		{
			get;
			set;
		}

		public PmxLinearBlendDeforming4()
		{
			this.Bones = new[] { -1, -1, -1, -1 };
		}

		public override void Read(BinaryReader br, PmxDocument doc)
		{
			this.Bones = new[] { doc.ReadIndex(br, PmxIndexKind.Bone), doc.ReadIndex(br, PmxIndexKind.Bone), doc.ReadIndex(br, PmxIndexKind.Bone), doc.ReadIndex(br, PmxIndexKind.Bone) };
			this.Weights = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle() };
		}

		public override void Write(BinaryWriter bw, PmxDocument doc)
		{
			this.Bones.ForEach(_ => doc.WriteIndex(bw, PmxIndexKind.Bone, _));
			this.Weights.ForEach(bw.Write);
		}
	}

	public class PmxSphericalDeforming : PmxSkinningFunction
	{
		public int[] Bones
		{
			get;
			set;
		}

		public float Weight
		{
			get;
			set;
		}

		/// <summary>
		/// Vector3 SDEF-C
		/// </summary>
		public float[] Center
		{
			get;
			set;
		}

		/// <summary>
		/// Vector3 SDEF-R0
		/// </summary>
		public float[] RangeZero
		{
			get;
			set;
		}

		/// <summary>
		/// Vector3 SDEF-R1
		/// </summary>
		public float[] RangeOne
		{
			get;
			set;
		}

		public PmxSphericalDeforming()
		{
			this.Bones = new[] { -1, -1 };
			this.Center = new[] { 0f, 0, 0 };
			this.RangeZero = new[] { 0f, 0, 0 };
			this.RangeOne = new[] { 0f, 0, 0 };
		}

		public override void Read(BinaryReader br, PmxDocument doc)
		{
			this.Bones = new[] { doc.ReadIndex(br, PmxIndexKind.Bone), doc.ReadIndex(br, PmxIndexKind.Bone) };
			this.Weight = br.ReadSingle();
			this.Center = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() };
			this.RangeZero = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() };
			this.RangeOne = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() };
		}

		public override void Write(BinaryWriter bw, PmxDocument doc)
		{
			this.Bones.ForEach(_ => doc.WriteIndex(bw, PmxIndexKind.Bone, _));
			bw.Write(this.Weight);
			this.Center.ForEach(bw.Write);
			this.RangeZero.ForEach(bw.Write);
			this.RangeOne.ForEach(bw.Write);
		}
	}

}
