using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Linearstar.Keystone.IO.MikuMikuDance.Pmx
{
	/// <summary>
	/// (PMX 2.1)
	/// </summary>
	public class PmxSoftBody
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

		public PmxSoftBodyKind Kind
		{
			get;
			set;
		}

		public int RelatedMaterial
		{
			get;
			set;
		}

		public byte Group
		{
			get;
			set;
		}

		public PmdRigidGroups CollidableGroups
		{
			get;
			set;
		}

		public PmxSoftBodyOptions Options
		{
			get;
			set;
		}

		public int BendingLinkDistance
		{
			get;
			set;
		}

		public int ClusterCount
		{
			get;
			set;
		}

		public float TotalMass
		{
			get;
			set;
		}

		public float Margin
		{
			get;
			set;
		}

		public PmxSoftBodyAeroModel AeroModel
		{
			get;
			set;
		}

		public Dictionary<PmxConfigurationIndex, float> Configuration
		{
			get;
			set;
		}

		/// <summary>
		/// V_IT
		/// </summary>
		public int VelocitySolverIteration
		{
			get;
			set;
		}

		/// <summary>
		/// P_IT
		/// </summary>
		public int PositonSolverIteration
		{
			get;
			set;
		}

		/// <summary>
		/// D_IT
		/// </summary>
		public int DriftSolverIteration
		{
			get;
			set;
		}

		/// <summary>
		/// C_IT
		/// </summary>
		public int ClusterSolverIteration
		{
			get;
			set;
		}

		/// <summary>
		/// LST
		/// </summary>
		public float LinearStiffnessCoefficient
		{
			get;
			set;
		}

		/// <summary>
		/// AST
		/// </summary>
		public float AreaAngularStiffnessCoefficient
		{
			get;
			set;
		}

		/// <summary>
		/// VST
		/// </summary>
		public float VolumeStiffnessCoefficient
		{
			get;
			set;
		}

		public List<PmxSoftBodyAnchor> Anchors
		{
			get;
			set;
		}

		public List<int> PinnedVertices
		{
			get;
			set;
		}

		public PmxSoftBody()
		{
			this.Configuration = Enum.GetValues(typeof(PmxConfigurationIndex)).Cast<PmxConfigurationIndex>().ToDictionary(_ => _, _ => 0f);
			this.Anchors = new List<PmxSoftBodyAnchor>();
			this.PinnedVertices = new List<int>();
		}

		public static PmxSoftBody Parse(BinaryReader br, PmxDocument doc)
		{
			return new PmxSoftBody
			{
				Name = doc.ReadString(br),
				EnglishName = doc.ReadString(br),
				Kind = (PmxSoftBodyKind)br.ReadByte(),
				RelatedMaterial = doc.ReadIndex(br, PmxIndexKind.Material),
				Group = br.ReadByte(),
				CollidableGroups = (PmdRigidGroups)br.ReadUInt16(),
				Options = (PmxSoftBodyOptions)br.ReadByte(),
				BendingLinkDistance = br.ReadInt32(),
				ClusterCount = br.ReadInt32(),
				TotalMass = br.ReadSingle(),
				Margin = br.ReadSingle(),
				AeroModel = (PmxSoftBodyAeroModel)br.ReadInt32(),
				Configuration = Enum.GetValues(typeof(PmxConfigurationIndex)).Cast<PmxConfigurationIndex>().ToDictionary(_ => _, _ => br.ReadSingle()),
				VelocitySolverIteration = br.ReadInt32(),
				PositonSolverIteration = br.ReadInt32(),
				DriftSolverIteration = br.ReadInt32(),
				ClusterSolverIteration = br.ReadInt32(),
				LinearStiffnessCoefficient = br.ReadSingle(),
				AreaAngularStiffnessCoefficient = br.ReadSingle(),
				VolumeStiffnessCoefficient = br.ReadSingle(),
				Anchors = Enumerable.Range(0, br.ReadInt32()).Select(_ => PmxSoftBodyAnchor.Parse(br, doc)).ToList(),
				PinnedVertices = Enumerable.Range(0, br.ReadInt32()).Select(_ => doc.ReadIndex(br, PmxIndexKind.Vertex)).ToList(),
			};
		}

		public void Write(BinaryWriter bw, PmxDocument doc)
		{
			doc.WriteString(bw, this.Name);
			doc.WriteString(bw, this.EnglishName);
			bw.Write((byte)this.Kind);
			doc.WriteIndex(bw, PmxIndexKind.Material, this.RelatedMaterial);
			bw.Write(this.Group);
			bw.Write((ushort)this.CollidableGroups);
			bw.Write((byte)this.Options);
			bw.Write(this.BendingLinkDistance);
			bw.Write(this.ClusterCount);
			bw.Write(this.TotalMass);
			bw.Write(this.Margin);
			bw.Write((int)this.AeroModel);
			this.Configuration.OrderBy(_ => _.Key).Select(_ => _.Value).ForEach(bw.Write);
			bw.Write(this.VelocitySolverIteration);
			bw.Write(this.PositonSolverIteration);
			bw.Write(this.DriftSolverIteration);
			bw.Write(this.ClusterSolverIteration);
			bw.Write(this.LinearStiffnessCoefficient);
			bw.Write(this.AreaAngularStiffnessCoefficient);
			bw.Write(this.VolumeStiffnessCoefficient);
			bw.Write(this.Anchors.Count);
			this.Anchors.ForEach(_ => _.Write(bw, doc));
			bw.Write(this.PinnedVertices.Count);
			this.PinnedVertices.ForEach(_ => doc.WriteIndex(bw, PmxIndexKind.Vertex, _));
		}
	}
}
