namespace Linearstar.Keystone.IO.MikuMikuDance.Pmd;

static class PmdBufferReaderExtensions
{
    public static unsafe string ReadString(ref this BufferReader reader, int capacity)
    {
        var buffer = reader.ReadNullTerminatedBytes(capacity);

        fixed (byte* bytes = buffer)
            return PmdDocument.Encoding.GetString(bytes, buffer.Length);
    }
}