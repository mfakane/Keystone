using System.Linq;

namespace Linearstar.Keystone.IO.Metasequoia
{
	/// <summary>
	/// material
	/// </summary>
	public class MqMaterial
	{
		MqChunk baseChunk;

		public string Name
		{
			get;
			set;
		}

		/// <summary>
		/// shader
		/// </summary>
		public MqShader Shader
		{
			get;
			set;
		}

		/// <summary>
		/// vcol
		/// </summary>
		public bool VertexColor
		{
			get;
			set;
		}

		/// <summary>
		/// col(R G B A)
		/// </summary>
		public float[] Color
		{
			get;
			set;
		}

		/// <summary>
		/// dif
		/// </summary>
		public float Diffusion
		{
			get;
			set;
		}

		/// <summary>
		/// amb
		/// </summary>
		public float Ambient
		{
			get;
			set;
		}

		/// <summary>
		/// emi
		/// </summary>
		public float Emmisive
		{
			get;
			set;
		}

		/// <summary>
		/// spc
		/// </summary>
		public float Specular
		{
			get;
			set;
		}

		/// <summary>
		/// power
		/// </summary>
		public float Power
		{
			get;
			set;
		}

		/// <summary>
		/// tex
		/// </summary>
		public string Texture
		{
			get;
			set;
		}

		/// <summary>
		/// aplane
		/// </summary>
		public string AlphaMap
		{
			get;
			set;
		}

		/// <summary>
		/// bump
		/// </summary>
		public string Bump
		{
			get;
			set;
		}

		/// <summary>
		/// proj_type
		/// </summary>
		public MqProjectionType ProjectionType
		{
			get;
			set;
		}

		/// <summary>
		/// proj_pos(X Y Z)
		/// </summary>
		public float[] ProjectionPosition
		{
			get;
			set;
		}

		/// <summary>
		/// proj_scale(X Y Z)
		/// </summary>
		public float[] ProjectionScale
		{
			get;
			set;
		}

		/// <summary>
		/// proj_angle(H P B)
		/// </summary>
		public float[] ProjectionAngle
		{
			get;
			set;
		}

		public MqMaterial()
		{
			this.Shader = MqShader.Phong;
			this.Color = new[] { 0.8f, 0.8f, 0.8f, 1 };
			this.Diffusion = 0.8f;
			this.Power = 5;
			this.ProjectionPosition = new[] { 0f, 0, 0 };
			this.ProjectionScale = new[] { 1f, 1, 1 };
			this.ProjectionAngle = new[] { 0f, 0, 0 };
		}

		public static MqMaterial Parse(MqChunk chunk)
		{
			var rt = new MqMaterial
			{
				baseChunk = chunk,
				Name = chunk.Name.Trim('"'),
			};

			foreach (var i in chunk.Attributes)
				switch (i.Name.ToLower())
				{
					case "shader":
						rt.Shader = (MqShader)int.Parse(i.Arguments.First());

						break;
					case "vcol":
						rt.VertexColor = i.Arguments.FirstOrDefault() == "1";

						break;
					case "col":
						rt.Color = i.Arguments.Select(float.Parse).ToArray();

						break;
					case "dif":
						rt.Diffusion = float.Parse(i.Arguments.First());

						break;
					case "amb":
						rt.Ambient = float.Parse(i.Arguments.First());

						break;
					case "emi":
						rt.Emmisive = float.Parse(i.Arguments.First());

						break;
					case "spc":
						rt.Specular = float.Parse(i.Arguments.First());

						break;
					case "power":
						rt.Power = float.Parse(i.Arguments.First());

						break;
					case "tex":
						rt.Texture = i.Arguments.First().Trim('"');

						break;
					case "aplane":
						rt.Texture = i.Arguments.First().Trim('"');

						break;
					case "bump":
						rt.Texture = i.Arguments.First().Trim('"');

						break;
					case "proj_type":
						rt.ProjectionType = (MqProjectionType)int.Parse(i.Arguments.First());

						break;
					case "proj_pos":
						rt.ProjectionPosition = i.Arguments.Select(float.Parse).ToArray();

						break;
					case "proj_scale":
						rt.ProjectionScale = i.Arguments.Select(float.Parse).ToArray();

						break;
					case "proj_angle":
						rt.ProjectionAngle = i.Arguments.Select(float.Parse).ToArray();

						break;
				}

			return rt;
		}

		public MqChunk ToChunk()
		{
			baseChunk = baseChunk ?? new MqChunk();
			baseChunk.Name = Surround(this.Name);
			baseChunk.Attribute("shader").SetArguments(((int)this.Shader).ToString());
			baseChunk.Attribute("vcol").SetArguments(this.VertexColor ? "1" : "0");
			baseChunk.Attribute("col").SetArguments(this.Color.Select(_ => _.ToString("0.000")));
			baseChunk.Attribute("dif").SetArguments(this.Diffusion.ToString("0.000"));
			baseChunk.Attribute("amb").SetArguments(this.Ambient.ToString("0.000"));
			baseChunk.Attribute("emi").SetArguments(this.Emmisive.ToString("0.000"));
			baseChunk.Attribute("spc").SetArguments(this.Specular.ToString("0.000"));
			baseChunk.Attribute("power").SetArguments(this.Power.ToString("0.000"));

			if (string.IsNullOrEmpty(this.Texture))
				baseChunk.RemoveAttributes("tex");
			else
				baseChunk.Attribute("tex").SetArguments(Surround(this.Texture));

			if (string.IsNullOrEmpty(this.AlphaMap))
				baseChunk.RemoveAttributes("aplane");
			else
				baseChunk.Attribute("aplane").SetArguments(Surround(this.AlphaMap));

			if (string.IsNullOrEmpty(this.Bump))
				baseChunk.RemoveAttributes("bump");
			else
				baseChunk.Attribute("bump").SetArguments(Surround(this.Bump));

			if (this.ProjectionType == MqProjectionType.UV)
				baseChunk.RemoveAttributes("proj_type");
			else
				baseChunk.Attribute("proj_type").SetArguments(((int)this.ProjectionType).ToString());

			if (this.ProjectionPosition.All(_ => _ == 0))
				baseChunk.RemoveAttributes("proj_pos");
			else
				baseChunk.Attribute("proj_pos").SetArguments(this.ProjectionPosition.Select(_ => _.ToString("0.000")));

			if (this.ProjectionScale.All(_ => _ == 1))
				baseChunk.RemoveAttributes("proj_scale");
			else
				baseChunk.Attribute("proj_scale").SetArguments(this.ProjectionScale.Select(_ => _.ToString("0.000")));

			if (this.ProjectionAngle.All(_ => _ == 0))
				baseChunk.RemoveAttributes("proj_angle");
			else
				baseChunk.Attribute("proj_angle").SetArguments(this.ProjectionAngle.Select(_ => _.ToString("0.000")));

			return baseChunk;
		}

		public string GetFormattedText()
		{
			return this.ToChunk().GetFormattedText();
		}

		static string Surround(string text)
		{
			return string.IsNullOrEmpty(text) ? null : "\"" + text + "\"";
		}
	}
}
