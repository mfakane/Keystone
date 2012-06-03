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

		public int ParentBone
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
		public int ConnectToBone
		{
			get;
			set;
		}

		/// <summary>
		/// this.Capabilities.HasFlag(PmxBoneCapabilities.RotationAffected) || this.Capabilities.HasFlag(PmxBoneCapabilities.MovementAffected)
		/// </summary>
		public int AffectedBone
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
			this.ParentBone = this.ConnectToBone = -1;
			this.ConnectToOffset = new[] { 0f, 0, 0 };
			this.FixedAxis = new[] { 0f, 0, 0 };
			this.LocalVectorX = new[] { 0f, 0, 0 };
			this.LocalVectorZ = new[] { 0f, 0, 0 };
			this.IK = new PmxIK();
		}

		public static PmxBone Parse(BinaryReader br, PmxDocument doc)
		{
			var rt = new PmxBone
			{
				Name = doc.ReadString(br),
				EnglishName = doc.ReadString(br),
				Position = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() },
				ParentBone = doc.ReadIndex(br, PmxIndexKind.Bone),
				Priority = br.ReadInt32(),
				Capabilities = (PmxBoneCapabilities)br.ReadUInt16(),
			};

			if (rt.Capabilities.HasFlag(PmxBoneCapabilities.ConnectToBone))
				rt.ConnectToBone = doc.ReadIndex(br, PmxIndexKind.Bone);
			else
				rt.ConnectToOffset = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() };

			if (rt.Capabilities.HasFlag(PmxBoneCapabilities.RotationAffected) ||
				rt.Capabilities.HasFlag(PmxBoneCapabilities.MovementAffected))
			{
				rt.AffectedBone = doc.ReadIndex(br, PmxIndexKind.Bone);
				rt.AffectionRate = br.ReadSingle();
			}

			if (rt.Capabilities.HasFlag(PmxBoneCapabilities.FixedAxis))
				rt.FixedAxis = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() };

			if (rt.Capabilities.HasFlag(PmxBoneCapabilities.LocalAxis))
			{
				rt.LocalVectorX = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() };
				rt.LocalVectorZ = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() };
			}

			if (rt.Capabilities.HasFlag(PmxBoneCapabilities.TransformByExternalParent))
				rt.ExternalParentKey = br.ReadInt32();

			if (rt.Capabilities.HasFlag(PmxBoneCapabilities.IK))
				rt.IK = PmxIK.Parse(br, doc);

			return rt;
		}

		public void Write(BinaryWriter bw, PmxDocument doc)
		{
			doc.WriteString(bw, this.Name);
			doc.WriteString(bw, this.EnglishName);
			this.Position.ForEach(bw.Write);
			doc.WriteIndex(bw, PmxIndexKind.Bone, this.ParentBone);
			bw.Write(this.Priority);
			bw.Write((ushort)this.Capabilities);

			if (this.Capabilities.HasFlag(PmxBoneCapabilities.ConnectToBone))
				doc.WriteIndex(bw, PmxIndexKind.Bone, this.ConnectToBone);
			else
				this.ConnectToOffset.ForEach(bw.Write);

			if (this.Capabilities.HasFlag(PmxBoneCapabilities.RotationAffected) ||
				this.Capabilities.HasFlag(PmxBoneCapabilities.MovementAffected))
			{
				doc.WriteIndex(bw, PmxIndexKind.Bone, this.AffectedBone);
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
				this.IK.Write(bw, doc);
		}
	}
}
