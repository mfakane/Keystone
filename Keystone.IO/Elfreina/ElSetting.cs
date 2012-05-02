using System.Linq;

namespace Linearstar.Keystone.IO.Elfreina
{
	public class ElSetting
	{
		ElData baseData;

		public float ElfreinaSoftVersion
		{
			get;
			set;
		}

		public int ElfreinaBetaVersion
		{
			get;
			set;
		}

		public string LoadType
		{
			get;
			set;
		}

		public bool IsRightHand
		{
			get;
			set;
		}

		public ElSetting()
		{
			this.LoadType = "LoadType";
		}

		public static ElSetting Parse(ElData data)
		{
			var rt = new ElSetting
			{
				baseData = data,
			};

			foreach (var i in data.Children)
				switch (i.Name)
				{
					case "ElfreinaSoftVersion":
						rt.ElfreinaSoftVersion = float.Parse(i.Values.First());

						break;
					case "ElfreinaBetaVersion":
						rt.ElfreinaBetaVersion = int.Parse(i.Values.First());

						break;
					case "LoadType":
						rt.LoadType = i.Values.First().Trim('"');

						break;
					case "IsRightHand":
						rt.IsRightHand = bool.Parse(i.Values.First());

						break;
				}

			return rt;
		}

		public ElData ToData()
		{
			baseData = baseData ?? new ElData();
			baseData.Name = "Setting";
			baseData.Child("ElfreinaSoftVersion").SetValues(this.ElfreinaSoftVersion.ToString("0.00"));

			if (this.ElfreinaBetaVersion == 0)
				baseData.RemoveChildren("ElfreinaBetaVersion");
			else
				baseData.Child("ElfreinaBetaVersion").SetValues(this.ElfreinaBetaVersion.ToString());

			baseData.Child("LoadType").SetValues("\"" + this.LoadType + "\"");
			baseData.Child("IsRightHand").SetValues(this.IsRightHand.ToString());

			return baseData;
		}

		public string GetFormattedText()
		{
			return this.ToData().GetFormattedText();
		}
	}
}
