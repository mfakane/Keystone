using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Linearstar.Keystone.IO.MikuMikuDance
{
	public class PmxIndexCache
	{
		readonly PmxDocument doc;
		readonly BinaryWriter bw;

		public Dictionary<PmxVertex, int> Vertices
		{
			get;
			private set;
		}

		public Dictionary<PmxTexture, int> Textures
		{
			get;
			private set;
		}

		public Dictionary<PmxMaterial, int> Materials
		{
			get;
			private set;
		}

		public Dictionary<PmxBone, int> Bones
		{
			get;
			private set;
		}

		public Dictionary<PmxMorph, int> Morphs
		{
			get;
			private set;
		}

		public Dictionary<PmxRigidBody, int> Rigids
		{
			get;
			private set;
		}

		public PmxIndexCache(PmxDocument doc, BinaryWriter bw)
		{
			this.doc = doc;
			this.bw = bw;
			this.Vertices = doc.Vertices.Select((i, j) => Tuple.Create(i, j)).ToDictionary(_ => _.Item1, _ => _.Item2);
			this.Textures = doc.Textures.Select((i, j) => Tuple.Create(i, j)).ToDictionary(_ => _.Item1, _ => _.Item2);
			this.Materials = doc.Materials.Select((i, j) => Tuple.Create(i, j)).ToDictionary(_ => _.Item1, _ => _.Item2);
			this.Bones = doc.Bones.Select((i, j) => Tuple.Create(i, j)).ToDictionary(_ => _.Item1, _ => _.Item2);
			this.Morphs = doc.Morphs.Select((i, j) => Tuple.Create(i, j)).ToDictionary(_ => _.Item1, _ => _.Item2);
			this.Rigids = doc.Rigids.Select((i, j) => Tuple.Create(i, j)).ToDictionary(_ => _.Item1, _ => _.Item2);
		}

		public void Write(PmxVertex vertex)
		{
			doc.WriteIndex(bw, PmxIndexKind.Vertex, vertex == null ? -1 : this.Vertices[vertex]);
		}

		public void Write(PmxTexture texture)
		{
			doc.WriteIndex(bw, PmxIndexKind.Texture, texture == null ? -1 : this.Textures[texture]);
		}

		public void Write(PmxMaterial material)
		{
			doc.WriteIndex(bw, PmxIndexKind.Material, material == null ? -1 : this.Materials[material]);
		}

		public void Write(PmxBone bone)
		{
			doc.WriteIndex(bw, PmxIndexKind.Bone, bone == null ? -1 : this.Bones[bone]);
		}

		public void Write(PmxMorph morph)
		{
			doc.WriteIndex(bw, PmxIndexKind.Morph, morph == null ? -1 : this.Morphs[morph]);
		}

		public void Write(PmxRigidBody rigidBody)
		{
			doc.WriteIndex(bw, PmxIndexKind.Rigid, rigidBody == null ? -1 : this.Rigids[rigidBody]);
		}
	}
}
