using System.IO;

namespace Linearstar.Keystone.IO.MikuMikuDance
{
	public class PmxModelInformation
	{
		public string ModelName
		{
			get;
			set;
		}

		public string EnglishModelName
		{
			get;
			set;
		}

		public string Description
		{
			get;
			set;
		}

		public string EnglishDescription
		{
			get;
			set;
		}

		public static PmxModelInformation Parse(BinaryReader br, PmxDocument doc)
		{
			return new PmxModelInformation
			{
				ModelName = doc.ReadString(br),
				EnglishModelName = doc.ReadString(br),
				Description = doc.ReadString(br),
				EnglishDescription = doc.ReadString(br),
			};
		}

		public void Write(BinaryWriter bw, PmxDocument doc)
		{
			doc.WriteString(bw, this.ModelName);
			doc.WriteString(bw, this.EnglishModelName);
			doc.WriteString(bw, this.Description);
			doc.WriteString(bw, this.EnglishDescription);
		}
	}
}
