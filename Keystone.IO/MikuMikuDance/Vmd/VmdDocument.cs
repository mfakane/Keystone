﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Linearstar.Keystone.IO.MikuMikuDance
{
	/// <summary>
	/// Vocaloid Motion Data file created by Higuchi_U
	/// </summary>
	public class VmdDocument
	{
		public const string DisplayName = "Vocaloid Motion Data file";
		public const string Filter = "*.vmd";
		public static readonly Encoding Encoding = Encoding.GetEncoding(932);

		public VmdVersion Version
		{
			get;
			set;
		}

		public string ModelName
		{
			get;
			set;
		}

		public List<VmdBoneFrame> BoneFrames
		{
			get;
			private set;
		}

		public List<VmdMorphFrame> MorphFrames
		{
			get;
			private set;
		}

		public List<VmdCameraFrame> CameraFrames
		{
			get;
			private set;
		}

		public List<VmdLightFrame> LightFrames
		{
			get;
			private set;
		}

		public List<VmdSelfShadowFrame> SelfShadowFrames
		{
			get;
			private set;
		}

		public VmdDocument()
		{
			this.Version = VmdVersion.MMDVer3;
			this.BoneFrames = new List<VmdBoneFrame>();
			this.MorphFrames = new List<VmdMorphFrame>();
			this.CameraFrames = new List<VmdCameraFrame>();
			this.LightFrames = new List<VmdLightFrame>();
			this.SelfShadowFrames = new List<VmdSelfShadowFrame>();
		}

		public static VmdDocument Parse(Stream stream)
		{
			var rt = new VmdDocument();
			var encoding = Encoding.GetEncoding(932);

			using (var br = new BinaryReader(stream))
			{
				var header = ReadVmdString(br, 30);

				if (header == "Vocaloid Motion Data file")
					rt.Version = VmdVersion.MMDVer2;
				else if (header == "Vocaloid Motion Data 0002")
					rt.Version = VmdVersion.MMDVer3;
				else
					throw new InvalidOperationException("invalid format");

				rt.ModelName = ReadVmdString(br, rt.Version == VmdVersion.MMDVer2 ? 10 : 20);

				rt.BoneFrames.AddRange(ReadBoneFrames(br));
				rt.MorphFrames.AddRange(ReadMorphFrames(br));
				rt.CameraFrames.AddRange(ReadCameraFrames(br, rt.Version));
				rt.LightFrames.AddRange(ReadLightFrames(br));

				if (br.GetRemainingLength() > 4)
					rt.SelfShadowFrames.AddRange(ReadSelfShadowFrames(br));
			}

			return rt;
		}

		static IEnumerable<VmdBoneFrame> ReadBoneFrames(BinaryReader br)
		{
			var count = br.ReadUInt32();

			for (uint i = 0; i < count; i++)
				yield return VmdBoneFrame.Parse(br);
		}

		static IEnumerable<VmdMorphFrame> ReadMorphFrames(BinaryReader br)
		{
			var count = br.ReadUInt32();

			for (uint i = 0; i < count; i++)
				yield return VmdMorphFrame.Parse(br);
		}

		static IEnumerable<VmdCameraFrame> ReadCameraFrames(BinaryReader br, VmdVersion version)
		{
			var count = br.ReadUInt32();

			for (uint i = 0; i < count; i++)
				yield return VmdCameraFrame.Parse(br, version);
		}

		static IEnumerable<VmdLightFrame> ReadLightFrames(BinaryReader br)
		{
			var count = br.ReadUInt32();

			for (uint i = 0; i < count; i++)
				yield return VmdLightFrame.Parse(br);
		}

		static IEnumerable<VmdSelfShadowFrame> ReadSelfShadowFrames(BinaryReader br)
		{
			var count = br.ReadUInt32();

			for (uint i = 0; i < count; i++)
				yield return VmdSelfShadowFrame.Parse(br);
		}

		internal static string ReadVmdString(BinaryReader br, int count)
		{
			return Encoding.GetString(br.ReadBytes(count).TakeWhile(_ => _ != '\0').ToArray());
		}

		internal static void WriteVmdString(BinaryWriter bw, string s, int count, VmdVersion version)
		{
			var bytes = Encoding.GetBytes(s);

			bw.Write(bytes.Take(count - 1).ToArray());
			bw.Write(Enumerable.Repeat(version == VmdVersion.MMDVer2 ? (byte)0 : (byte)0xFD, Math.Max(count - bytes.Length, 1)).Select((_, idx) => idx == 0 ? (byte)0 : _).ToArray());
		}

		public void Write(Stream stream)
		{
			using (var bw = new BinaryWriter(stream))
			{
				if (this.Version == VmdVersion.MMDVer2)
				{
					WriteVmdString(bw, "Vocaloid Motion Data file", 30, VmdVersion.MMDVer2);
					WriteVmdString(bw, this.ModelName, 10, this.Version);
				}
				else
				{
					WriteVmdString(bw, "Vocaloid Motion Data 0002", 30, VmdVersion.MMDVer2);
					WriteVmdString(bw, this.ModelName, 20, this.Version);
				}

				bw.Write(this.BoneFrames.Count);
				this.BoneFrames.ForEach(_ => _.Write(bw, this.Version));
				bw.Write(this.MorphFrames.Count);
				this.MorphFrames.ForEach(_ => _.Write(bw, this.Version));
				bw.Write(this.CameraFrames.Count);
				this.CameraFrames.ForEach(_ => _.Write(bw, this.Version));
				bw.Write(this.LightFrames.Count);
				this.LightFrames.ForEach(_ => _.Write(bw));

				if (this.Version == VmdVersion.MMDVer3)
				{
					bw.Write(this.SelfShadowFrames.Count);
					this.SelfShadowFrames.ForEach(_ => _.Write(bw));
				}

				bw.Write(0);
			}
		}
	}
}
