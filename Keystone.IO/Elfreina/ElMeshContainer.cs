using System.Collections.Generic;
using System.Linq;
using Linearstar.Keystone.IO;

namespace Linearstar.Keystone.IO.Elfreina
{
	public class ElMeshContainer
	{
		ElData baseData;

		public string Name
		{
			get;
			set;
		}

		public List<ElVertexFormat> VertexFormat
		{
			get;
			private set;
		}

		public List<string> BoneNames
		{
			get;
			private set;
		}

		public List<float[]> OffsetMatrices
		{
			get;
			private set;
		}

		public List<ElMaterial> Materials
		{
			get;
			private set;
		}

		public List<ElMesh> Meshes
		{
			get;
			private set;
		}

		public ElMeshContainer()
		{
			this.VertexFormat = new List<ElVertexFormat>();
			this.BoneNames = new List<string>();
			this.OffsetMatrices = new List<float[]>();
			this.Materials = new List<ElMaterial>();
			this.Meshes = new List<ElMesh>();
		}

		public static ElMeshContainer Parse(ElData data)
		{
			var rt = new ElMeshContainer
			{
				baseData = data,
			};

			foreach (var i in data.Children)
				switch (i.Name)
				{
					case "Name":
						rt.Name = i.Values.First().Trim('"');

						break;
					case "VertexFormat":
						rt.VertexFormat.AddRange(i.Children.Select(_ => Util.ParseEnum<ElVertexFormat>(_.Values.First().Trim('"'))));

						break;
					case "BoneNames":
						rt.BoneNames.AddRange(i.Children.Select(_ => _.Values.First().Trim('"')));

						break;
					case "OffsetMatrices":
						rt.OffsetMatrices.AddRange(i.Children.Select(_ => _.Values.Select(float.Parse).ToArray()));

						break;
					case "Materials":
						rt.Materials.AddRange(i.Children.Where(_ => _.Name == "Material").Select(ElMaterial.Parse));

						break;
					case "Mesh":
						rt.Meshes.Add(ElMesh.Parse(i));

						break;
				}

			return rt;
		}

		public ElData ToData()
		{
			baseData = baseData ?? new ElData();
			baseData.Name = "MeshContainer";
			baseData.Child("Name").SetValues("\"" + this.Name + "\"");
			baseData.Child("BoneCount").SetValues(this.BoneNames.Count.ToString());
			baseData.Child("MeshCount").SetValues(this.Meshes.Count.ToString());
			baseData.Child("VertexFormat").Children = this.VertexFormat.Select(_ => new ElData().SetValues("\"" + _ + "\"")).ToList();
			baseData.Child("BoneNames").Children = this.BoneNames.Select(_ => new ElData().SetValues("\"" + _ + "\"")).ToList();
			baseData.Child("OffsetMatrices").Children = this.OffsetMatrices.Select(_ => new ElData().SetValues(_.Select(f => f.ToString("0.000000")))).ToList();
			baseData.Child("Materials").Children = this.Materials.Select(_ => _.ToData()).StartWith(new ElData("MaterialCount").SetValues(this.Materials.Count.ToString())).ToList();
			baseData.Children.AddRange(this.Meshes.Select(_ => _.ToData()));

			return baseData;
		}

		public string GetFormattedText()
		{
			return this.ToData().GetFormattedText();
		}
	}
}
