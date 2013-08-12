using System.IO;

namespace Linearstar.Keystone.IO.MikuMikuDance
{
	public class PmxBone
	{
		public string Name
		{
			get;
			set;
		}

		public string EnglishName
		{
			get;
			set;
		}

		/// <summary>
		/// Vector3
		/// </summary>
		public float[] Position
		{
			get;
			set;
		}

		public PmxBone ParentBone
		{
			get;
			set;
		}

		public int Priority
		{
			get;
			set;
		}

		public PmxBoneCapabilities Capabilities
		{
			get;
			set;
		}

		/// <summary>
		/// Vector3, !this.Capabilities.HasFlag(PmxBoneCapabilities.ConnectToBone)
		/// </summary>
		public float[] ConnectToOffset
		{
			get;
			set;
		}

		/// <summary>
		/// this.Capabilities.HasFlag(PmxBoneCapabilities.ConnectToBone)
		/// </summary>
		public PmxBone ConnectToBone
		{
			get;
			set;
		}

		/// <summary>
		/// this.Capabilities.HasFlag(PmxBoneCapabilities.RotationAffected) || this.Capabilities.HasFlag(PmxBoneCapabilities.MovementAffected)
		/// </summary>
		public PmxBone AffectedBone
		{
			get;
			set;
		}

		/// <summary>
		/// this.Capabilities.HasFlag(PmxBoneCapabilities.RotationAffected) || this.Capabilities.HasFlag(PmxBoneCapabilities.MovementAffected)
		/// </summary>
		public float AffectionRate
		{
			get;
			set;
		}

		/// <summary>
		/// Vector3, this.Capabilities.HasFlag(PmxBoneCapabilities.FixedAxis)
		/// </summary>
		public float[] FixedAxis
		{
			get;
			set;
		}

		/// <summary>
		/// Vector3, this.Capabilities.HasFlag(PmxBoneCapabilities.LocalAxis)
		/// </summary>
		public float[] LocalVectorX
		{
			get;
			set;
		}

		/// <summary>
		/// Vector3, this.Capabilities.HasFlag(PmxBoneCapabilities.LocalAxis)
		/// </summary>
		public float[] LocalVectorZ
		{
			get;
			set;
		}

		/// <summary>
		/// this.Capabilities.HasFlag(PmxBoneCapabilities.TransformByExternalParent)
		/// </summary>
		public int ExternalParentKey
		{
			get;
			set;
		}

		/// <summary>
		/// this.Capabilities.HasFlag(PmxBoneCapabilities.IK)
		/// </summary>
		public PmxIK IK
		{
			get;
			set;
		}

		public PmxBone()
		{
			this.Position = new[] { 0f, 0, 0 };
			this.ConnectToOffset = new[] { 0f, 0, 0 };
			this.FixedAxis = new[] { 0f, 0, 0 };
			this.LocalVectorX = new[] { 0f, 0, 0 };
			this.LocalVectorZ = new[] { 0f, 0, 0 };
			this.IK = new PmxIK();
		}

		public void Parse(BinaryReader br, PmxDocument doc)
		{
			this.Name = doc.ReadString(br);
			this.EnglishName = doc.ReadString(br);
			this.Position = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() };
			this.ParentBone = doc.ReadBone(br);
			this.Priority = br.ReadInt32();
			this.Capabilities = (PmxBoneCapabilities)br.ReadUInt16();

			if (this.Capabilities.HasFlag(PmxBoneCapabilities.ConnectToBone))
				this.ConnectToBone = doc.ReadBone(br);
			else
				this.ConnectToOffset = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() };

			if (this.Capabilities.HasFlag(PmxBoneCapabilities.RotationAffected) ||
				this.Capabilities.HasFlag(PmxBoneCapabilities.MovementAffected))
			{
				this.AffectedBone = doc.ReadBone(br);
				this.AffectionRate = br.ReadSingle();
			}

			if (this.Capabilities.HasFlag(PmxBoneCapabilities.FixedAxis))
				this.FixedAxis = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() };

			if (this.Capabilities.HasFlag(PmxBoneCapabilities.LocalAxis))
			{
				this.LocalVectorX = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() };
				this.LocalVectorZ = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() };
			}

			if (this.Capabilities.HasFlag(PmxBoneCapabilities.TransformByExternalParent))
				this.ExternalParentKey = br.ReadInt32();

			if (this.Capabilities.HasFlag(PmxBoneCapabilities.IK))
				this.IK = PmxIK.Parse(br, doc);
		}

		public void Write(BinaryWriter bw, PmxDocument doc, PmxIndexCache cache)
		{
			doc.WriteString(bw, this.Name);
			doc.WriteString(bw, this.EnglishName);
			this.Position.ForEach(bw.Write);
			cache.Write(this.ParentBone);
			bw.Write(this.Priority);
			bw.Write((ushort)this.Capabilities);

			if (this.Capabilities.HasFlag(PmxBoneCapabilities.ConnectToBone))
				cache.Write(this.ConnectToBone);
			else
				this.ConnectToOffset.ForEach(bw.Write);

			if (this.Capabilities.HasFlag(PmxBoneCapabilities.RotationAffected) ||
				this.Capabilities.HasFlag(PmxBoneCapabilities.MovementAffected))
			{
				cache.Write(this.AffectedBone);
				bw.Write(this.AffectionRate);
			}

			if (this.Capabilities.HasFlag(PmxBoneCapabilities.FixedAxis))
				this.FixedAxis.ForEach(bw.Write);

			if (this.Capabilities.HasFlag(PmxBoneCapabilities.LocalAxis))
			{
				this.LocalVectorX.ForEach(bw.Write);
				this.LocalVectorZ.ForEach(bw.Write);
			}

			if (this.Capabilities.HasFlag(PmxBoneCapabilities.TransformByExternalParent))
				bw.Write(this.ExternalParentKey);

			if (this.Capabilities.HasFlag(PmxBoneCapabilities.IK))
				this.IK.Write(bw, doc, cache);
		}
	}
}
