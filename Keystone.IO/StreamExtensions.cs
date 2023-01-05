using System.Buffers;
using System.IO;

namespace Linearstar.Keystone.IO;

static class StreamExtensions
{
    public static ReadOnlySequence<byte> AsReadOnlySequence(this Stream stream)
    {
        if (stream is MemoryStream memoryStream)
            return new ReadOnlySequence<byte>(memoryStream.ToArray());
        
        memoryStream = new MemoryStream();
        stream.CopyTo(memoryStream);

        return new ReadOnlySequence<byte>(memoryStream.ToArray());
    }
}