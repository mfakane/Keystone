using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Linearstar.Keystone.IO;

readonly ref struct BufferWriter
{
    readonly IBufferWriter<byte> writer;

    public BufferWriter(IBufferWriter<byte> writer) => this.writer = writer;

    public Span<byte> GetSpan(int sizeHint) => writer.GetSpan(sizeHint);
    
    public void Advance(int count) => writer.Advance(count);
    
    public void Write(bool value)
    {
        var destination = writer.GetSpan(1);
        destination[0] = (byte)(value ? 1 : 0);
        writer.Advance(1);
    }
    
    public void Write(sbyte value)
    {
        var destination = writer.GetSpan(1);
        destination[0] = Unsafe.As<sbyte, byte>(ref value);
        writer.Advance(1);
    }
    
    public void Write(byte value)
    {
        var destination = writer.GetSpan(1);
        destination[0] = value;
        writer.Advance(1);
    }
    
    public void Write(short value)
    {
        var destination = writer.GetSpan(2);
        BinaryPrimitives.WriteInt16LittleEndian(destination, value);
        writer.Advance(2);
    }
    
    public void Write(ushort value)
    {
        var destination = writer.GetSpan(2);
        BinaryPrimitives.WriteUInt16LittleEndian(destination, value);
        writer.Advance(2);
    }
    
    public void Write(int value)
    {
        var destination = writer.GetSpan(4);
        BinaryPrimitives.WriteInt32LittleEndian(destination, value);
        writer.Advance(4);
    }
    
    public void Write(uint value)
    {
        var destination = writer.GetSpan(4);
        BinaryPrimitives.WriteUInt32LittleEndian(destination, value);
        writer.Advance(4);
    }
    
    public void Write(long value)
    {
        var destination = writer.GetSpan(8);
        BinaryPrimitives.WriteInt64LittleEndian(destination, value);
        writer.Advance(8);
    }
    
    public void Write(ulong value)
    {
        var destination = writer.GetSpan(8);
        BinaryPrimitives.WriteUInt64LittleEndian(destination, value);
        writer.Advance(8);
    }
    
    public void Write(float value)
    {
        var destination = writer.GetSpan(4);
        MemoryMarshal.Write(destination, ref value);
        writer.Advance(4);
    }

    public void Write(in ReadOnlySpan<byte> span)
    {
        var destination = writer.GetSpan(span.Length);
        span.CopyTo(destination);
        writer.Advance(span.Length);
    }
    
    public void Write(Color3B value)
    {
        var destination = writer.GetSpan(3);
        MemoryMarshal.Write(destination, ref value);
        writer.Advance(3);
    }
    
    public void Write(Color4B value)
    {
        var destination = writer.GetSpan(4);
        MemoryMarshal.Write(destination, ref value);
        writer.Advance(4);
    }
    
    public void Write(Vector2 value)
    {
        var destination = writer.GetSpan(8);
        MemoryMarshal.Write(destination, ref value);
        writer.Advance(8);
    }

    public void Write(Vector3 value)
    {
        var destination = writer.GetSpan(12);
        MemoryMarshal.Write(destination, ref value);
        writer.Advance(12);
    }
    
    public void Write(Color3 value)
    {
        var destination = writer.GetSpan(12);
        MemoryMarshal.Write(destination, ref value);
        writer.Advance(12);
    }
    
    public void Write(Vector4 value)
    {
        var destination = writer.GetSpan(16);
        MemoryMarshal.Write(destination, ref value);
        writer.Advance(16);
    }
    
    public void Write(Color4 value)
    {
        var destination = writer.GetSpan(16);
        MemoryMarshal.Write(destination, ref value);
        writer.Advance(16);
    }
    
    public void Write(Quaternion value)
    {
        var destination = writer.GetSpan(16);
        MemoryMarshal.Write(destination, ref value);
        writer.Advance(16);
    }
}