using System;
using System.IO;
using System.Linq;

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
				case PmxSkinningKind.DualQuaternionDeforming:
					rt = new PmxDualQuaternionDeforming();

					break;
				default:
					throw new NotSupportedException();
			}

			rt.Read(br, doc);

			return rt;
		}

		public abstract void Read(BinaryReader br, PmxDocument doc);
		public abstract void Write(BinaryWriter bw, PmxDocument doc, PmxIndexCache cache);
	}

	public class PmxLinearBlendDeforming1 : PmxSkinningFunction
	{
		public PmxBone Bone
		{
			get;
			set;
		}

		public PmxLinearBlendDeforming1()
		{
		}

		public override void Read(BinaryReader br, PmxDocument doc)
		{
			this.Bone = doc.ReadBone(br);
		}

		public override void Write(BinaryWriter bw, PmxDocument doc, PmxIndexCache cache)
		{
			cache.Write(this.Bone);
		}
	}

	public class PmxLinearBlendDeforming2 : PmxSkinningFunction
	{
		public PmxBone[] Bones
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
			this.Bones = new PmxBone[2];
		}

		public override void Read(BinaryReader br, PmxDocument doc)
		{
			this.Bones = Enumerable.Range(0, 2).Select(_ => doc.ReadBone(br)).ToArray();
			this.Weight = br.ReadSingle();
		}

		public override void Write(BinaryWriter bw, PmxDocument doc, PmxIndexCache cache)
		{
			this.Bones.ForEach(_ => cache.Write(_));
			bw.Write(this.Weight);
		}
	}

	public class PmxLinearBlendDeforming4 : PmxSkinningFunction
	{
		public PmxBone[] Bones
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
			this.Bones = new PmxBone[4];
		}

		public override void Read(BinaryReader br, PmxDocument doc)
		{
			this.Bones = Enumerable.Range(0, 4).Select(_ => doc.ReadBone(br)).ToArray();
			this.Weights = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle() };
		}

		public override void Write(BinaryWriter bw, PmxDocument doc, PmxIndexCache cache)
		{
			this.Bones.ForEach(_ => cache.Write(_));
			this.Weights.ForEach(bw.Write);
		}
	}

	public class PmxSphericalDeforming : PmxSkinningFunction
	{
		public PmxBone[] Bones
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
			this.Bones = new PmxBone[2];
			this.Center = new[] { 0f, 0, 0 };
			this.RangeZero = new[] { 0f, 0, 0 };
			this.RangeOne = new[] { 0f, 0, 0 };
		}

		public override void Read(BinaryReader br, PmxDocument doc)
		{
			this.Bones = Enumerable.Range(0, 2).Select(_ => doc.ReadBone(br)).ToArray();
			this.Weight = br.ReadSingle();
			this.Center = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() };
			this.RangeZero = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() };
			this.RangeOne = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() };
		}

		public override void Write(BinaryWriter bw, PmxDocument doc, PmxIndexCache cache)
		{
			this.Bones.ForEach(_ => cache.Write(_));
			bw.Write(this.Weight);
			this.Center.ForEach(bw.Write);
			this.RangeZero.ForEach(bw.Write);
			this.RangeOne.ForEach(bw.Write);
		}
	}

	/// <summary>
	/// (PMX 2.1)
	/// </summary>
	public class PmxDualQuaternionDeforming : PmxLinearBlendDeforming4
	{
	}
}
