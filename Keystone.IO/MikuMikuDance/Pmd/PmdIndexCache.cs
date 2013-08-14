using System;
using System.Collections.Generic;
using System.Linq;

namespace Linearstar.Keystone.IO.MikuMikuDance
{
	public class PmdIndexCache
	{
		readonly PmdDocument doc;

		public Dictionary<PmdVertex, int> Vertices
		{
			get;
			private set;
		}

		public Dictionary<PmdBone, int> Bones
		{
			get;
			private set;
		}

		public PmdIndexCache(PmdDocument doc)
		{
			this.doc = doc;
			this.Vertices = doc.Vertices.Select((i, j) => Tuple.Create(i, j)).ToDictionary(_ => _.Item1, _ => _.Item2);
			this.Bones = doc.Bones.Select((i, j) => Tuple.Create(i, j)).ToDictionary(_ => _.Item1, _ => _.Item2);
		}
	}
}
