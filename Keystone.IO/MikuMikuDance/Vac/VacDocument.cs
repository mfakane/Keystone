using System.IO;
using System.Linq;
using System.Text;

namespace Linearstar.Keystone.IO.MikuMikuDance
{
	/// <summary>
	/// Vocaloid Accessory Connection file created by Higuchi_U
	/// </summary>
	public class VacDocument
	{
		public const string DisplayName = "Vocaloid Accessory Connection file";
		public const string Filter = "*.vac";
		public static readonly Encoding Encoding = Encoding.GetEncoding(932);
		
		public string Name
		{
			get;
			set;
		}

		public string FileName
		{
			get;
			set;
		}

		public float Scale
		{
			get;
			set;
		}

		public float[] Position
		{
			get;
			set;
		}

		public float[] Angle
		{
			get;
			set;
		}

		public string BoneName
		{
			get;
			set;
		}

		public VacDocument()
		{
			this.Position = new[] { 0f, 0, 0 };
			this.Angle = new[] { 0f, 0, 0 };
		}

		public static VacDocument Parse(string text)
		{
			using (var sr = new StringReader(text))
				return new VacDocument
				{
					Name = sr.ReadLine(),
					FileName = sr.ReadLine(),
					Scale = float.Parse(sr.ReadLine()),
					Position = sr.ReadLine().Split(',').Select(float.Parse).ToArray(),
					Angle = sr.ReadLine().Split(',').Select(float.Parse).ToArray(),
					BoneName = sr.ReadLine(),
				};
		}

		public string GetFormattedText()
		{
			return string.Join("\r\n", new[]
			{
				this.Name,
				this.FileName,
				this.Scale.ToString(),
				string.Join(",", this.Position),
				string.Join(",", this.Angle),
				this.BoneName,
			});
		}
	}
}
