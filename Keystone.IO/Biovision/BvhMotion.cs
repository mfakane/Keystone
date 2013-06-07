using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Linearstar.Keystone.IO.Biovision
{
	public class BvhMotion
	{
		public float FrameTime
		{
			get;
			set;
		}

		public IList<IDictionary<BvhJoint, BvhJointMotion>> Frames
		{
			get;
			set;
		}

		public BvhMotion()
		{
			this.Frames = new List<IDictionary<BvhJoint, BvhJointMotion>>();
		}

		public static BvhMotion Parse(BvhDocument doc, BvhTokenizer tokenizer)
		{
			var rt = new BvhMotion();

			if (tokenizer.Current.EnsureKind(BvhTokenizer.IdentifierTokenKind).Text == "MOTION")
				tokenizer.MoveNext();

			tokenizer.MoveNext(BvhTokenizer.NewLineTokenKind);

			var frames = tokenizer.Current.EnsureKind(BvhTokenizer.IdentifierTokenKind).Text == "Frames" ? int.Parse(tokenizer.MoveNext().Text) : 0;

			tokenizer.MoveNext();
			tokenizer.MoveNext(BvhTokenizer.NewLineTokenKind);

			if (tokenizer.Current.EnsureKind(BvhTokenizer.IdentifierTokenKind).Text == "Frame")
				tokenizer.MoveNext();

			rt.FrameTime = tokenizer.Current.EnsureKind(BvhTokenizer.IdentifierTokenKind).Text == "Time" ? float.Parse(tokenizer.MoveNext().Text) : 0;
			tokenizer.MoveNext();

			var joints = GetJoints(doc.Root).ToArray();

			for (int i = 0; i < frames; i++)
			{
				var values = tokenizer.TakeWhile(_ => _.Kind != BvhTokenizer.NewLineTokenKind).Select(_ => _.Text).Select(float.Parse).ToArray();
				var j = 0;

				rt.Frames.Add(joints.ToDictionary(_ => _, _ =>
				{
					var jm = new BvhJointMotion();
					var position = new float[3];
					var rotation = new float[3];

					foreach (var c in _.Channels)
						switch (c)
						{
							case BvhChannel.XPosition:
							case BvhChannel.YPosition:
							case BvhChannel.ZPosition:
								jm.Position = position;
								position[(int)c] = values[j++];

								break;
							case BvhChannel.XRotation:
							case BvhChannel.YRotation:
							case BvhChannel.ZRotation:
								jm.Rotation = rotation;
								rotation[c - BvhChannel.XRotation] = values[j++];

								break;
						}

					return jm;
				}));
			}

			return rt;
		}

		public string GetFormattedText()
		{
			var sb = new StringBuilder();

			sb.AppendLine("MOTION");
			sb.Append("Frames: ");
			sb.AppendLine(this.Frames.Count.ToString());
			sb.Append("Frame Time: ");
			sb.AppendLine(this.FrameTime.ToString());

			foreach (var i in this.Frames)
				sb.AppendLine(string.Join("\t", i.SelectMany(j => j.Key.Channels.Select(_ =>
				{
					switch (_)
					{
						case BvhChannel.XPosition:
						case BvhChannel.YPosition:
						case BvhChannel.ZPosition:
							return j.Value.Position[(int)_];
						case BvhChannel.XRotation:
						case BvhChannel.YRotation:
						case BvhChannel.ZRotation:
							return j.Value.Position[_ - BvhChannel.XRotation];
						default:
							throw new ArgumentException();
					}
				}))));

			return sb.ToString();
		}

		static IEnumerable<BvhJoint> GetJoints(BvhJoint item)
		{
			return new[] { item }.Concat(item.Children.SelectMany(GetJoints));
		}
	}
}
