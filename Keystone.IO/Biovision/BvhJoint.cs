using System;
using System.Collections.Generic;
using System.Linq;

namespace Linearstar.Keystone.IO.Biovision
{
	public class BvhJoint
	{
		BvhData baseData;

		public BvhKind Kind
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public float[] Offset
		{
			get;
			set;
		}

		public BvhChannel[] Channels
		{
			get;
			set;
		}

		public IList<BvhJoint> Children
		{
			get;
			set;
		}

		public BvhJoint()
		{
			this.Kind = BvhKind.Joint;
			this.Children = new List<BvhJoint>();
		}

		public static BvhJoint Parse(BvhData data)
		{
			var rt = new BvhJoint
			{
				baseData = data,
				Kind = (BvhKind)Enum.Parse(typeof(BvhKind), data.Name, true),
				Name = data.Values.First(),
				Offset = data.Child("OFFSET").Values.Select(_ => float.Parse(_)).ToArray(),
				Channels = data.Child("CHANNELS").Values.Skip(1).Select(_ => (BvhChannel)Enum.Parse(typeof(BvhChannel), _, true)).ToArray(),
			};

			foreach (var i in data.Children)
				if (i.Name == "JOINT" || i.Name == "End")
					rt.Children.Add(Parse(i));

			return rt;
		}

		public BvhData ToData()
		{
			baseData = baseData ?? new BvhData();
			baseData.Name = this.Kind == BvhKind.End ? "End" : this.Kind.ToString().ToUpper();
			baseData.SetValues(this.Name);
			baseData.Child("OFFSET").SetValues(this.Offset.Select(_ => _.ToString("0.00")));

			if (this.Channels.Any())
				baseData.Child("CHANNELS").SetValues(new[] { this.Channels.Length.ToString() }.Concat(this.Channels.Select(_ => _.ToString()).Select(_ => _.Substring(0, 1).ToUpper() + _.Substring(1).ToLower())));
			else
				baseData.RemoveChildren("CHANNELS");

			baseData.RemoveChildren("JOINT");
			baseData.RemoveChildren("End");
			baseData.Children.AddRange(this.Children.Select(_ => _.ToData()));

			return baseData;
		}

		public string GetFormattedText()
		{
			return this.ToData().GetFormattedText();
		}
	}
}
