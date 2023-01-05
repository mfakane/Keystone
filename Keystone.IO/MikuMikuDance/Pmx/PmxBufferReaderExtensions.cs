namespace Linearstar.Keystone.IO.MikuMikuDance.Pmx;

static class PmxBufferReaderExtensions
{
    public static unsafe string ReadString(ref this BufferReader reader, PmxHeader header)
    {
        var length = reader.ReadInt32();
        if (length == 0) return "";
        
        var buffer = reader.ReadBytes(length);

        fixed (byte* bytes = buffer)
            return header.Encoding.GetString(bytes, buffer.Length);
    }
}