using System;
using System.Buffers.Binary;

namespace Linearstar.Keystone.IO.MikuMikuDance.Pmx;

static class PmxBufferWriterExtensions
{
    public static unsafe void Write(ref this BufferWriter writer, string value, PmxHeader header)
    {
        if (string.IsNullOrEmpty(value))
        {
            writer.Write(0);
            return;
        }

        var writtenBytes = 4;
        var buffer = writer.GetSpan(value.Length * 3 + writtenBytes);

        fixed (byte* bytes = buffer.Slice(writtenBytes))
        fixed (char* chars = value.AsSpan())
            writtenBytes += header.Encoding.GetBytes(chars, value.Length, bytes, buffer.Length);

        BinaryPrimitives.WriteInt32LittleEndian(buffer, writtenBytes - 4);
        writer.Advance(writtenBytes);
    }

    public static void Write(ref this BufferWriter writer, PmxVertex? vertex, PmxIndexCache cache)
    {
        switch (cache.Header.VertexIndexSize)
        {
            case PmxVertexIndexSize.UInt8:
                writer.Write((byte)cache[vertex]);
                break;
            case PmxVertexIndexSize.UInt16:
                writer.Write((ushort)cache[vertex]);
                break;
            case PmxVertexIndexSize.Int32:
                writer.Write(cache[vertex]);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public static void Write(ref this BufferWriter writer, PmxTexture? texture, PmxIndexCache cache)
    {
        switch (cache.Header.TextureIndexSize)
        {
            case PmxIndexSize.Int8:
                writer.Write((sbyte)cache[texture]);
                break;
            case PmxIndexSize.Int16:
                writer.Write((short)cache[texture]);
                break;
            case PmxIndexSize.Int32:
                writer.Write(cache[texture]);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    public static void Write(ref this BufferWriter writer, PmxMaterial? material, PmxIndexCache cache)
    {
        switch (cache.Header.MaterialIndexSize)
        {
            case PmxIndexSize.Int8:
                writer.Write((sbyte)cache[material]);
                break;
            case PmxIndexSize.Int16:
                writer.Write((short)cache[material]);
                break;
            case PmxIndexSize.Int32:
                writer.Write(cache[material]);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    public static void Write(ref this BufferWriter writer, PmxBone? bone, PmxIndexCache cache)
    {
        switch (cache.Header.MaterialIndexSize)
        {
            case PmxIndexSize.Int8:
                writer.Write((sbyte)cache[bone]);
                break;
            case PmxIndexSize.Int16:
                writer.Write((short)cache[bone]);
                break;
            case PmxIndexSize.Int32:
                writer.Write(cache[bone]);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    public static void Write(ref this BufferWriter writer, PmxMorph? morph, PmxIndexCache cache)
    {
        switch (cache.Header.MaterialIndexSize)
        {
            case PmxIndexSize.Int8:
                writer.Write((sbyte)cache[morph]);
                break;
            case PmxIndexSize.Int16:
                writer.Write((short)cache[morph]);
                break;
            case PmxIndexSize.Int32:
                writer.Write(cache[morph]);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    public static void Write(ref this BufferWriter writer, PmxRigidBody? rigidBody, PmxIndexCache cache)
    {
        switch (cache.Header.RigidIndexSize)
        {
            case PmxIndexSize.Int8:
                writer.Write((sbyte)cache[rigidBody]);
                break;
            case PmxIndexSize.Int16:
                writer.Write((short)cache[rigidBody]);
                break;
            case PmxIndexSize.Int32:
                writer.Write(cache[rigidBody]);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}