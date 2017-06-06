﻿using System.Collections.Generic;
using System.Linq;
using Linearstar.Keystone.IO;

namespace Linearstar.Keystone.IO.Elfreina
{
	public class ElMesh
	{
		ElData baseData;

		public string Name
		{
			get;
			set;
		}

		public IList<float[]> Positions
		{
			get;
			set;
		}

		public IList<float[]> Normals
		{
			get;
			set;
		}

		public IList<float[]> Diffuse
		{
			get;
			set;
		}

		public IList<ElBlendPart> BlendList
		{
			get;
			set;
		}

		public IDictionary<int, IList<float[]>> TextureUV
		{
			get;
			set;
		}

		public IList<int[]> VertexIndices
		{
			get;
			set;
		}

		public IList<int> Attributes
		{
			get;
			set;
		}

		public ElMesh()
		{
			this.Positions = new List<float[]>();
			this.Normals = new List<float[]>();
			this.Diffuse = new List<float[]>();
			this.BlendList = new List<ElBlendPart>();
			this.TextureUV = new Dictionary<int, IList<float[]>>();
			this.VertexIndices = new List<int[]>();
			this.Attributes = new List<int>();
		}

		public static ElMesh Parse(ElData data)
		{
			var rt = new ElMesh
			{
				baseData = data,
			};

			foreach (var i in data.Children)
				switch (i.Name)
				{
					case "Name":
						rt.Name = i.Values.First().Trim('"');

						break;
					case "Positions":
						rt.Positions = i.Children.Select(_ => _.Values.Select(float.Parse).ToArray()).ToList();

						break;
					case "Normals":
						rt.Normals = i.Children.Select(_ => _.Values.Select(float.Parse).ToArray()).ToList();

						break;
					case "Diffuse":
						rt.Diffuse = i.Children.Select(_ => _.Values.Select(float.Parse).ToArray()).ToList();

						break;
					case "BlendList":
						rt.BlendList = i.Children.Select(ElBlendPart.Parse).ToList();

						break;
					case "TextureUV":
					case "Texture1UV":
					case "Texture2UV":
					case "Texture3UV":
					case "Texture4UV":
					case "Texture5UV":
					case "Texture6UV":
					case "Texture7UV":
					case "Texture8UV":
						rt.TextureUV[int.Parse(string.Join("", i.Name.Where(char.IsDigit).DefaultIfEmpty('0')))] =
							i.Children.Select(_ => _.Values.Select(float.Parse).ToArray()).ToList();

						break;
					case "VertexIndices":
						rt.VertexIndices = i.Children.Select(_ => _.Values.Select(f => f.Split(',').Last()).Select(int.Parse).ToArray()).ToList();

						break;
					case "Attributes":
						rt.Attributes = i.Children.Select(_ => int.Parse(_.Values.First())).ToList();

						break;
				}

			return rt;
		}

		public ElData ToData()
		{
			baseData = baseData ?? new ElData();
			baseData.Name = "Mesh";
			baseData.Child("Name").SetValues("\"" + this.Name + "\"");
			baseData.Child("VertexCount").SetValues(this.Positions.Count.ToString());
			baseData.Child("FaceCount").SetValues(this.VertexIndices.Count.ToString());
			baseData.Child("Positions").Children = this.Positions.Select(_ => new ElData().SetValues(_.Select(f => f.ToString("0.000000")))).ToList();
			baseData.Child("Normals").Children = this.Normals.Select(_ => new ElData().SetValues(_.Select(f => f.ToString("0.000000")))).ToList();
			baseData.Child("BlendList").Children = this.BlendList.Select(_ => _.ToData()).ToList();

			baseData.RemoveChildren("TextureUV");

			for (int i = 1; i < 9; i++)
				baseData.RemoveChildren("Texture" + i + "UV");

			if (this.TextureUV.Any())
				if (this.TextureUV.Count == 1)
					baseData.Child("TextureUV").Children = this.TextureUV.First().Value.Select(_ => new ElData().SetValues(_.Select(f => f.ToString("0.000000")))).ToList();
				else
					foreach (var i in this.TextureUV)
						baseData.Child("Texture" + i.Key + "UV").Children = i.Value.Select(_ => new ElData().SetValues(_.Select(f => f.ToString("0.000000")))).ToList();

			baseData.Child("VertexIndices").Children = this.VertexIndices.Select(_ => new ElData().SetValues(new[] { _.Length + "," + _.First().ToString() }.Concat(_.Skip(1).Select(i => i.ToString())))).ToList();
			baseData.Child("Attributes").Children = this.Attributes.Select(_ => new ElData().SetValues(_.ToString())).ToList();

			return baseData;
		}

		public string GetFormattedText()
		{
			return this.ToData().GetFormattedText();
		}
	}
}