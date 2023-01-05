namespace Linearstar.Keystone.IO.MikuMikuDance.Xx;

static class XxBufferReaderExtensions
{
    public static unsafe string ReadString(ref this BufferReader reader)
    {
        const int capacity = XxDocument.StringCapacity;

        var buffer = reader.ReadNullTerminatedBytes(capacity);

        fixed (byte* bytes = buffer)
            return XxDocument.Encoding.GetString(bytes, buffer.Length);
    }
}