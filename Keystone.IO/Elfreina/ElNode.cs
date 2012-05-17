using System.Collections.Generic;
using System.Linq;

namespace Linearstar.Keystone.IO.Elfreina
{
	public class ElNode
	{
		ElData baseData;

		public string NodeName
		{
			get;
			set;
		}

		public float[] InitPostureMatrix
		{
			get;
			set;
		}

		public IList<ElNode> Nodes
		{
			get;
			set;
		}

		public ElNode()
		{
			this.InitPostureMatrix = new float[16];
			this.Nodes = new List<ElNode>();
		}

		public static ElNode Parse(ElData data)
		{
			var rt = new ElNode
			{
				baseData = data,
			};

			foreach (var i in data.Children)
				switch (i.Name)
				{
					case "NodeName":
						rt.NodeName = i.Values.First().Trim('"');

						break;
					case "InitPostureMatrix":
						rt.InitPostureMatrix = i.Values.Select(float.Parse).ToArray();

						break;
					case "Node":
						rt.Nodes.Add(Parse(i));

						break;
				}

			return rt;
		}

		public ElData ToData()
		{
			baseData = baseData ?? new ElData();
			baseData.Child("NodeName").SetValues("\"" + this.NodeName + "\"");
			baseData.Child("InitPostureMatrix").SetValues(this.InitPostureMatrix.Select(_ => _.ToString("0.000000")));
			baseData.Children.AddRange(this.Nodes.Select(_ => _.ToData()));

			return baseData;
		}

		public string GetFormattedText()
		{
			return this.ToData().GetFormattedText();
		}
	}
}
