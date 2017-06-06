﻿using System.IO;
using System.Linq;

namespace Linearstar.Keystone.IO.MikuMikuMoving
{
	public class MvdBoneFrame
	{
		public int StageId
		{
			get;
			set;
		}

		public long FrameTime
		{
			get;
			set;
		}

		public float[] Position
		{
			get;
			set;
		}

		public float[] Quaternion
		{
			get;
			set;
		}

		public MvdInterpolationPoint[] XInterpolation
		{
			get;
			set;
		}

		public MvdInterpolationPoint[] YInterpolation
		{
			get;
			set;
		}

		public MvdInterpolationPoint[] ZInterpolation
		{
			get;
			set;
		}

		public MvdInterpolationPoint[] RotationInterpolation
		{
			get;
			set;
		}

		public MvdBoneFrame()
		{
			this.Position = new[] { 0f, 0, 0 };
			this.Quaternion = new[] { 0f, 0, 0, 1 };
			this.XInterpolation = new[] { MvdInterpolationPoint.DefaultA, MvdInterpolationPoint.DefaultB };
			this.YInterpolation = new[] { MvdInterpolationPoint.DefaultA, MvdInterpolationPoint.DefaultB };
			this.ZInterpolation = new[] { MvdInterpolationPoint.DefaultA, MvdInterpolationPoint.DefaultB };
			this.RotationInterpolation = new[] { MvdInterpolationPoint.DefaultA, MvdInterpolationPoint.DefaultB };
		}

		public static MvdBoneFrame Parse(BinaryReader br)
		{
			return new MvdBoneFrame
			{
				StageId = br.ReadInt32(),
				FrameTime = br.ReadInt64(),
				Position = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() },
				Quaternion = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle() },
				XInterpolation = new[] { MvdInterpolationPoint.Parse(br), MvdInterpolationPoint.Parse(br) },
				YInterpolation = new[] { MvdInterpolationPoint.Parse(br), MvdInterpolationPoint.Parse(br) },
				ZInterpolation = new[] { MvdInterpolationPoint.Parse(br), MvdInterpolationPoint.Parse(br) },
				RotationInterpolation = new[] { MvdInterpolationPoint.Parse(br), MvdInterpolationPoint.Parse(br) },
			};
		}

		public void Write(BinaryWriter bw)
		{
			bw.Write(this.StageId);
			bw.Write(this.FrameTime);
			this.Position.ForEach(bw.Write);
			this.Quaternion.ForEach(bw.Write);
			this.XInterpolation.ForEach(_ => _.Write(bw));
			this.YInterpolation.ForEach(_ => _.Write(bw));
			this.ZInterpolation.ForEach(_ => _.Write(bw));
			this.RotationInterpolation.ForEach(_ => _.Write(bw));
		}

		public string GetName(MvdNameList names, MvdBoneData boneData)
		{
			if (this.StageId == 0)
				return names.Names[boneData.Key];
			else
			{
				var key = boneData.Key * -1000 - this.StageId;

				return names.Names.ContainsKey(key)
					? names.Names[key]
					: this.StageId.ToString("000");
			}
		}
	}
}
