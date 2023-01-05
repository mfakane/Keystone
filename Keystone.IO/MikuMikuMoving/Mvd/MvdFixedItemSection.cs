using System;
using System.IO;

namespace Linearstar.Keystone.IO.MikuMikuMoving.Mvd
{
	public abstract class MvdFixedItemSection : MvdSection
	{
        protected virtual bool IgnoreRawItemSize => false;

		public MvdFixedItemSection(MvdTag tag)
			: base(tag)
		{
		}

		internal abstract void ReadItem(ref BufferReader ir, MvdDocument document, MvdObject obj);
		internal abstract void WriteItem(ref BufferWriter iw, MvdDocument document, int index);

		internal virtual void BeforeWriteHeader(ref BufferWriter bw, MvdDocument document)
		{
		}

		internal override void Read(ref BufferReader br, MvdDocument document, MvdObject obj)
		{
			if (this.RawItemSize <= 0)
				throw new InvalidOperationException("RawItemSize must be greater than 0.");

			for (var i = 0; i < this.RawCount; i++)
                if (this.IgnoreRawItemSize)
                    ReadItem(ref br, document, obj);
                else
                {
	                var ir = new BufferReader(br.ReadSequence(this.RawItemSize));
                    ReadItem(ref ir, document, obj);
                }
        }

		internal override void Write(ref BufferWriter bw, MvdDocument document)
		{
			if (this.RawCount > 0)
			{
				using (var ms = new MemoryStream())
				{
					using (var sbw = new StreamBufferWriter(ms))
					{
						var iw = new BufferWriter(sbw);
						// write the first item to the temp buffer to get the item's size
						WriteItem(ref iw, document, 0);
					}

					this.RawItemSize = (int)ms.Length;

					// then, write the headers to the real buffer
					BeforeWriteHeader(ref bw, document);
					base.Write(ref bw, document);

					// and then, copy the first item's temp buffer to the real one.
					bw.Write(ms.ToArray());
				}

				for (var i = 1; i < this.RawCount; i++)
					WriteItem(ref bw, document, i);
			}
			else
				base.Write(ref bw, document);
		}
	}
}
