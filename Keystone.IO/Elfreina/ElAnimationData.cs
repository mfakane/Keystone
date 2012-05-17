using System.Collections.Generic;
using System.Linq;

namespace Linearstar.Keystone.IO.Elfreina
{
	public class ElAnimationData
	{
		ElData baseData;

		public string AnimationName
		{
			get;
			set;
		}

		public int AnimationTime
		{
			get;
			set;
		}

		public int FramesPerSecond
		{
			get;
			set;
		}

		public int TransitionTime
		{
			get;
			set;
		}

		public int Priority
		{
			get;
			set;
		}

		public bool Loop
		{
			get;
			set;
		}

		public IList<float> BoneAnimationTimeKeys
		{
			get;
			set;
		}

		public IList<ElBoneAnimationPart> BoneAnimation
		{
			get;
			set;
		}

		public IList<float> UVAnimationTimeKeys
		{
			get;
			set;
		}

		public IList<ElUVAnimationPart> UVAnimation
		{
			get;
			set;
		}

		public ElAnimationData()
		{
			this.BoneAnimationTimeKeys = new List<float>();
			this.BoneAnimation = new List<ElBoneAnimationPart>();
			this.UVAnimationTimeKeys = new List<float>();
			this.UVAnimation = new List<ElUVAnimationPart>();
		}

		public static ElAnimationData Parse(ElData data)
		{
			var rt = new ElAnimationData
			{
				baseData = data,
			};

			foreach (var i in data.Children)
				switch (i.Name)
				{
					case "AnimationName":
						rt.AnimationName = i.Values.First().Trim('"');

						break;
					case "AnimationTime":
						rt.AnimationTime = int.Parse(i.Values.First());

						break;
					case "FrameParSecond":
						rt.FramesPerSecond = int.Parse(i.Values.First());

						break;
					case "TransitionTime":
						rt.TransitionTime = int.Parse(i.Values.First());

						break;
					case "Priority":
						rt.Priority = int.Parse(i.Values.First());

						break;
					case "Loop":
						rt.Loop = bool.Parse(i.Values.First());

						break;
					case "BoneAnimation":
						rt.BoneAnimationTimeKeys = i.Child("TimeKeys").Children.Select(_ => float.Parse(_.Values.First())).ToList();
						rt.BoneAnimation = i.Children.Where(_ => _.Name == "AnimationPart").Select(ElBoneAnimationPart.Parse).ToList();

						break;
					case "UVAnimation":
						rt.UVAnimationTimeKeys = i.Child("TimeKeys").Children.Select(_ => float.Parse(_.Values.First())).ToList();
						rt.UVAnimation = i.Children.Where(_ => _.Name == "AnimationPart").Select(ElUVAnimationPart.Parse).ToList();

						break;
				}

			return rt;
		}

		public ElData ToData()
		{
			baseData = baseData ?? new ElData();
			baseData.Child("AnimationName").SetValues("\"" + this.AnimationName + "\"");
			baseData.Child("AnimationTime").SetValues(this.AnimationTime.ToString());
			baseData.Child("FrameParSecond").SetValues(this.FramesPerSecond.ToString());
			baseData.Child("TransitionTime").SetValues(this.TransitionTime.ToString());
			baseData.Child("Priority").SetValues(this.Priority.ToString());
			baseData.Child("Loop").SetValues(this.Loop.ToString());

			if (this.BoneAnimation.Any())
				baseData.Child("BoneAnimation").Children =
					this.BoneAnimation.Select(_ => _.ToData())
						.StartWith(new ElData("TimeKeys")
						{
							Children = this.BoneAnimationTimeKeys.Select(f => new ElData().SetValues(f.ToString("0.000000"))).ToList(),
						}).ToList();
			else
				baseData.RemoveChildren("BoneAnimation");

			if (this.UVAnimation.Any())
				baseData.Child("UVAnimation").Children =
					this.UVAnimation.Select(_ => _.ToData())
						.StartWith(new ElData("TimeKeys")
						{
							Children = this.UVAnimationTimeKeys.Select(f => new ElData().SetValues(f.ToString("0.000000"))).ToList(),
						}).ToList();
			else
				baseData.RemoveChildren("UVAnimation");

			return baseData;
		}

		public string GetFormattedText()
		{
			return this.ToData().GetFormattedText();
		}
	}
}
