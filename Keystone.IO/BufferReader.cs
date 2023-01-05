using System;
using System.Buffers;
using System.Buffers.Binary;
using System.IO;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Linearstar.Keystone.IO;

ref struct BufferReader
{
    SequencePosition currentPosition;
    readonly ReadOnlySequence<byte> sequence;
    
    public bool IsCompleted => sequence.End.Equals(currentPosition);
    
    public BufferReader(in ReadOnlySequence<byte> sequence)
    {
        this.sequence = sequence;
        currentPosition = sequence.Start;
    }

    public long TryRead(int length, out ReadOnlySpan<byte> span)
    {
        var nextPosition = sequence.GetPosition(length, currentPosition);
        var slice = sequence.Slice(currentPosition, nextPosition);

        span = slice.IsSingleSegment 
            ? slice.First.Span
            : slice.ToArray().AsSpan();

        currentPosition = nextPosition; 
        
        return slice.Length;
    }

    public ReadOnlySpan<byte> ReadNullTerminatedBytes(int? capacity)
    {
        SequencePosition? nextPosition = capacity != null ? sequence.GetPosition(capacity.Value, currentPosition) : null;
        var slice = sequence.Slice(currentPosition, nextPosition ?? sequence.End);
        var nullPosition = slice.PositionOf((byte)0);
        var nullTerminatedSlice = nullPosition != null
            ? slice.Slice(0, nullPosition.Value)
            : slice;
        var span = nullTerminatedSlice.IsSingleSegment 
            ? nullTerminatedSlice.First.Span
            : nullTerminatedSlice.ToArray().AsSpan();

        currentPosition = nextPosition ?? (nullPosition != null ? sequence.GetPosition(1, nullPosition.Value) : sequence.End);

        return span;
    }

    public ReadOnlySequence<byte> ReadSequence(int length)
    {
        var nextPosition = sequence.GetPosition(length, currentPosition);
        var slice =  sequence.Slice(currentPosition, nextPosition);
        
        currentPosition = nextPosition;

        return slice;
    }

    public bool ReadBoolean()
    {
        if (TryRead(1, out var span) != 1)
            throw new EndOfStreamException();
        
        return span[0] != 0;
    }
    
    public sbyte ReadSByte()
    {
        if (TryRead(sizeof(sbyte), out var span) != sizeof(sbyte))
            throw new EndOfStreamException();

        return (sbyte)span[0];
    }

    public byte ReadByte()
    {
        if (TryRead(1, out var span) != 1)
            throw new EndOfStreamException();
        
        return span[0];
    }
    
    public short ReadInt16()
    {
        if (TryRead(sizeof(short), out var span) != sizeof(short))
            throw new EndOfStreamException();
        
        return BinaryPrimitives.ReadInt16LittleEndian(span);
    }
    
    public ushort ReadUInt16()
    {
        if (TryRead(sizeof(ushort), out var span) != sizeof(ushort))
            throw new EndOfStreamException();
        
        return BinaryPrimitives.ReadUInt16LittleEndian(span);
    }

    public int ReadInt32()
    {
        if (TryRead(sizeof(int), out var span) != sizeof(int))
            throw new EndOfStreamException();

        return BinaryPrimitives.ReadInt32LittleEndian(span);
    }
    
    public uint ReadUInt32()
    {
        if (TryRead(sizeof(uint), out var span) != sizeof(uint))
            throw new EndOfStreamException();

        return BinaryPrimitives.ReadUInt32LittleEndian(span);
    }
    
    public long ReadInt64()
    {
        if (TryRead(sizeof(long), out var span) != sizeof(long))
            throw new EndOfStreamException();

        return BinaryPrimitives.ReadInt64LittleEndian(span);
    }
    
    public ulong ReadUInt64()
    {
        if (TryRead(sizeof(ulong), out var span) != sizeof(ulong))
            throw new EndOfStreamException();

        return BinaryPrimitives.ReadUInt64LittleEndian(span);
    }
    
    public float ReadSingle()
    {
        if (TryRead(sizeof(float), out var span) != sizeof(float))
            throw new EndOfStreamException();

        return MemoryMarshal.Read<float>(span);
    }

    public ReadOnlySpan<byte> ReadBytes(int length)
    {
        if (TryRead(length, out var span) != length)
            throw new EndOfStreamException();

        return span;
    }

    public Color3B ReadColor3B()
    {
        if (TryRead(3, out var span) != 3)
            throw new EndOfStreamException();
        
        return MemoryMarshal.Read<Color3B>(span);
    }
    
    public Color4B ReadColor4B()
    {
        if (TryRead(4, out var span) != 4)
            throw new EndOfStreamException();
        
        return MemoryMarshal.Read<Color4B>(span);
    }
    
    public Vector2 ReadVector2()
    {
        if (TryRead(8, out var span) != 8)
            throw new EndOfStreamException();
        
        return MemoryMarshal.Read<Vector2>(span);
    }
    
    public Vector3 ReadVector3()
    {
        if (TryRead(12, out var span) != 12)
            throw new EndOfStreamException();
        
        return MemoryMarshal.Read<Vector3>(span);
    }
    
    public Color3 ReadColor3()
    {
        if (TryRead(12, out var span) != 12)
            throw new EndOfStreamException();
        
        return MemoryMarshal.Read<Color3>(span);
    }
    
    public Vector4 ReadVector4()
    {
        if (TryRead(16, out var span) != 16)
            throw new EndOfStreamException();
        
        return MemoryMarshal.Read<Vector4>(span);
    }
    
    public Color4 ReadColor4()
    {
        if (TryRead(16, out var span) != 16)
            throw new EndOfStreamException();
        
        return MemoryMarshal.Read<Color4>(span);
    }
    
    public Quaternion ReadQuaternion()
    {
        if (TryRead(16, out var span) != 16)
            throw new EndOfStreamException();
        
        return MemoryMarshal.Read<Quaternion>(span);
    }
}

