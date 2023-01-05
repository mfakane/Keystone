using System.Collections.Generic;
using System.Linq;

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

		public IList<ElVertexFormat> VertexFormat
		{
			get;
			set;
		}

		public IList<string> BoneNames
		{
			get;
			set;
		}

		public IList<float[]> OffsetMatrices
		{
			get;
			set;
		}

		public IList<ElMaterial> Materials
		{
			get;
			set;
		}

		public IList<ElMesh> Meshes
		{
			get;
			set;
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
						rt.VertexFormat = i.Children.Select(_ => EnumEx.Parse<ElVertexFormat>(_.Values.First().Trim('"'))).ToList();

						break;
					case "BoneNames":
						rt.BoneNames = i.Children.Select(_ => _.Values.First().Trim('"')).ToList();

						break;
					case "OffsetMatrices":
						rt.OffsetMatrices = i.Children.Select(_ => _.Values.Select(float.Parse).ToArray()).ToList();

						break;
					case "Materials":
						rt.Materials = i.Children.Where(_ => _.Name == "Material").Select(ElMaterial.Parse).ToList();

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
