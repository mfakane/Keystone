namespace Linearstar.Keystone.IO.MikuMikuDance.Vmd;

static class VmdBufferReaderExtensions
{
    public static unsafe string ReadString(ref this BufferReader reader, int capacity)
    {
        var buffer = reader.ReadNullTerminatedBytes(capacity);

        fixed (byte* bytes = buffer)
            return VmdDocument.Encoding.GetString(bytes, buffer.Length);
    }
}