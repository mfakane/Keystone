namespace Linearstar.Keystone.IO.MikuMikuMoving.Mvd;

static class MvdBufferReaderExtensions
{
    public static unsafe string ReadString(ref this BufferReader reader, MvdDocument document)
    {
        var length = reader.ReadInt32();
        if (length == 0) return "";
        
        var buffer = reader.ReadBytes(length);

        fixed (byte* bytes = buffer)
            return document.Encoding.GetString(bytes, buffer.Length);
    }
}