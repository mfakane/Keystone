using System;

namespace Linearstar.Keystone.IO.MikuMikuDance.Xx;

static class XxBufferWriterExtensions
{
    public static unsafe void Write(ref this BufferWriter writer, string value)
    {
        const byte padding = 0xFD;
        const int capacity = XxDocument.StringCapacity;
        
        var buffer = writer.GetSpan(capacity);
        int writtenBytes;
        
        buffer.Fill(padding);

        fixed (byte* bytes = buffer)
        fixed (char* chars = value.AsSpan())
            writtenBytes = XxDocument.Encoding.GetBytes(chars, value.Length, bytes, buffer.Length);

        if (writtenBytes < capacity)
            buffer[writtenBytes] = 0;
        
        writer.Advance(capacity);
    }
}