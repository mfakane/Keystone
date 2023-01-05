using System;
using System.Buffers;
using System.IO;

namespace Linearstar.Keystone.IO;
        
class StreamBufferWriter : IBufferWriter<byte>, IDisposable
{
    readonly Stream stream;
    byte[] buffer;

    public StreamBufferWriter(Stream stream)
    {
        this.stream = stream;
        buffer = ArrayPool<byte>.Shared.Rent(256);
    }

    public void Advance(int count)
    {
        this.stream.Write(buffer, 0, count);
    }

    public Memory<byte> GetMemory(int sizeHint = 0)
    {
        EnsureBuffer(sizeHint);
        return new Memory<byte>(buffer);
    }

    public Span<byte> GetSpan(int sizeHint = 0)
    {
        EnsureBuffer(sizeHint);
        return new Span<byte>(buffer);
    }
            
    void EnsureBuffer(int sizeHint)
    {
        if (sizeHint <= buffer.Length) return;
        
        ArrayPool<byte>.Shared.Return(buffer);
        buffer = ArrayPool<byte>.Shared.Rent(sizeHint);
    }

    public void Dispose()
    {
        stream.Flush();
        ArrayPool<byte>.Shared.Return(buffer);
    }
}