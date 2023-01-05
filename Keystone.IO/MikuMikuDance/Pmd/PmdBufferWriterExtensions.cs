using System;

namespace Linearstar.Keystone.IO.MikuMikuDance.Pmd;

static class PmdBufferWriterExtensions
{
    public static unsafe void Write(ref this BufferWriter writer, string value, int capacity, byte padding = 0xFD)
    {
        var buffer = writer.GetSpan(capacity);
        int writtenBytes;
        
        buffer.Fill(padding);

        fixed (byte* bytes = buffer)
        fixed (char* chars = value.AsSpan())
            writtenBytes = PmdDocument.Encoding.GetBytes(chars, value.Length, bytes, buffer.Length);

        if (writtenBytes < capacity)
            buffer[writtenBytes] = 0;
        
        writer.Advance(capacity);
    }

    public static void Write(ref this BufferWriter writer, PmdVertex? texture, PmdIndexCache cache) => 
        writer.Write(cache[texture]);

    public static void Write(ref this BufferWriter writer, PmdBone? texture, PmdIndexCache cache) => 
        writer.Write(cache[texture]);
}