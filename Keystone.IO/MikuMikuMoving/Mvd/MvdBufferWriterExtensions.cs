using System;
using System.Buffers.Binary;

namespace Linearstar.Keystone.IO.MikuMikuMoving.Mvd;

static class MvdBufferWriterExtensions
{
    public static unsafe void Write(ref this BufferWriter writer, string value, MvdDocument document)
    {
        if (string.IsNullOrEmpty(value))
        {
            writer.Write(0);
            return;
        }

        var buffer = writer.GetSpan(value.Length * 3 + 4);
        var writtenBytes = 4;

        fixed (byte* bytes = buffer.Slice(writtenBytes))
        fixed (char* chars = value.AsSpan())
            writtenBytes += document.Encoding.GetBytes(chars, value.Length, bytes, buffer.Length);

        BinaryPrimitives.WriteInt32LittleEndian(buffer, value.Length);
        writer.Advance(writtenBytes);
    }
}