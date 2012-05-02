using System;
using System.IO;
using System.Linq;

namespace Linearstar.Keystone.IO.MikuMikuMoving
{
	public class MvdCameraFrame
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

		public float Radius
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

		public float Fov
		{
			get;
			set;
		}

		public bool Perspective
		{
			get;
			set;
		}

		public MvdInterpolationPoint[] PositionInterpolation
		{
			get;
			set;
		}

		public MvdInterpolationPoint[] AngleInterpolation
		{
			get;
			set;
		}

		public MvdInterpolationPoint[] RadiusInterpolation
		{
			get;
			set;
		}

		public MvdInterpolationPoint[] FovInterpolation
		{
			get;
			set;
		}

		public MvdCameraFrame()
		{
			this.Radius = 50;
			this.Position = new[] { 0f, 10, 0 };
			this.Angle = new[] { 0, (float)Math.PI, 0 };
			this.Fov = (float)(Math.PI * 1.666667);
			this.Perspective = true;
			this.PositionInterpolation = new[] { MvdInterpolationPoint.DefaultA, MvdInterpolationPoint.DefaultB };
			this.AngleInterpolation = new[] { MvdInterpolationPoint.DefaultA, MvdInterpolationPoint.DefaultB };
			this.RadiusInterpolation = new[] { MvdInterpolationPoint.DefaultA, MvdInterpolationPoint.DefaultB };
			this.FovInterpolation = new[] { MvdInterpolationPoint.DefaultA, MvdInterpolationPoint.DefaultB };
		}

		public static MvdCameraFrame Parse(BinaryReader br)
		{
			return new MvdCameraFrame
			{
				StageId = br.ReadInt32(),
				FrameTime = br.ReadInt64(),
				Radius = br.ReadSingle(),
				Position = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() },
				Angle = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() },
				Fov = br.ReadSingle(),
				Perspective = br.ReadBoolean(),
				PositionInterpolation = new[] { MvdInterpolationPoint.Parse(br), MvdInterpolationPoint.Parse(br) },
				AngleInterpolation = new[] { MvdInterpolationPoint.Parse(br), MvdInterpolationPoint.Parse(br) },
				RadiusInterpolation = new[] { MvdInterpolationPoint.Parse(br), MvdInterpolationPoint.Parse(br) },
				FovInterpolation = new[] { MvdInterpolationPoint.Parse(br), MvdInterpolationPoint.Parse(br) },
			};
		}

		public void Write(BinaryWriter bw)
		{
			bw.Write(this.StageId);
			bw.Write(this.FrameTime);
			bw.Write(this.Radius);
			this.Position.ForEach(bw.Write);
			this.Angle.ForEach(bw.Write);
			bw.Write(this.Fov);
			bw.Write(this.Perspective);
			this.PositionInterpolation.ForEach(_ => _.Write(bw));
			this.AngleInterpolation.ForEach(_ => _.Write(bw));
			this.RadiusInterpolation.ForEach(_ => _.Write(bw));
			this.FovInterpolation.ForEach(_ => _.Write(bw));
		}
	}
}
