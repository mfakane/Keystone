using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Linearstar.Keystone.IO;

namespace Linearstar.Keystone.IO.MikuMikuMoving
{
	/// <summary>
	/// Motion Vector Data File created by Mogg
	/// </summary>
	public class MvdDocument
	{
		public const string DisplayName = "Motion Vector Data file";
		public const string Filter = "*.mvd";

		public float Version
		{
			get;
			set;
		}

		public Encoding Encoding
		{
			get;
			set;
		}

		public string ObjectName
		{
			get;
			set;
		}

		public float KeyFps
		{
			get;
			set;
		}

		public List<MvdSection> Sections
		{
			get;
			private set;
		}

		public MvdDocument()
		{
			this.Sections = new List<MvdSection>();
		}

		public static MvdDocument Parse(Stream stream)
		{
			var rt = new MvdDocument();
			var systemEncoding = Encoding.GetEncoding(932);

			using (var br = new BinaryReader(stream))
			{
				var header = ReadMvdString(br, 30, systemEncoding);

				if (header != DisplayName)
					throw new InvalidOperationException("invalid format");

				rt.Version = br.ReadSingle();

				if (rt.Version >= 2)
					throw new NotSupportedException("specified format version not supported");

				switch (br.ReadByte())
				{
					case 0:
						rt.Encoding = Encoding.Unicode;

						break;
					case 1:
					default:
						rt.Encoding = Encoding.UTF8;

						break;
				}

				rt.ObjectName = rt.Encoding.GetString(br.ReadSizedBuffer());
				br.ReadSizedBuffer();	// objectNameSize2 / objectName2
				rt.KeyFps = br.ReadSingle();
				br.ReadSizedBuffer();	// reservedSize / reserved

				while (br.GetRemainingLength() > 1)
				{
					var section = MvdSection.Parse(rt, br);

					if (section == null)
						break;

					rt.Sections.Add(section);
				}
			}

			return rt;
		}

		static string ReadMvdString(BinaryReader br, int count, Encoding encoding)
		{
			return encoding.GetString(br.ReadBytes(count).TakeWhile(_ => _ != '\0').ToArray());
		}

		public void Write(Stream stream)
		{
			using (var bw = new BinaryWriter(stream))
			{
				var buf = new byte[30];

				Encoding.GetEncoding(932).GetBytes(DisplayName, 0, DisplayName.Length, buf, 0);
				bw.Write(buf);
				bw.Write(this.Version);
				bw.Write((byte)(this.Encoding.CodePage == Encoding.Unicode.CodePage ? 0 : 1));

				bw.WriteSizedBuffer(this.Encoding.GetBytes(this.ObjectName));
				bw.WriteSizedBuffer(this.Encoding.GetBytes(this.ObjectName));
				bw.Write(this.KeyFps);
				bw.WriteSizedBuffer(new byte[0]);

				foreach (var i in this.Sections)
					i.Write(this, bw);

				bw.Write((byte)MvdTag.Eof);
				bw.Write((byte)0);
			}
		}
	}
}
