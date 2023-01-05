using System.Buffers;
using System.Numerics;
using Linearstar.Keystone.IO.MikuMikuDance;
using Linearstar.Keystone.IO.MikuMikuDance.Vmd;

namespace Linearstar.Keystone.IO.Tests;

public class VmdTests
{
    [Fact]
    public void WriteBoneFrame()
    {
        var bytes = BinaryHelper.FromString("""
            42 6F 6E 65 00 FD FD FD FD FD FD FD FD FD FD
            23 00 00 00
            00 00 00 00 00 00 80 3F 00 00 00 00
            00 00 00 00 00 00 00 00 00 00 00 00 00 00 80 3F
            14 14 14 14 14 14 14 14 6B 6B 6B 6B 6B 6B 6B 6B          
            14 14 14 14 14 14 14 6B 6B 6B 6B 6B 6B 6B 6B 01
            14 14 14 14 14 14 6B 6B 6B 6B 6B 6B 6B 6B 01 00
            14 14 14 14 14 6B 6B 6B 6B 6B 6B 6B 6B 01 00 00
        """);
        var buffer = new ArrayBufferWriter<byte>();
        var bw = new BufferWriter(buffer);
        
        var frame = new VmdBoneFrame("Bone")
        {
            FrameTime = 35,
            Position = Vector3.UnitY,
        };
        frame.Write(ref bw);

        var result = buffer.WrittenSpan.ToArray();
        
        Assert.Equal(result, bytes);
    }

    [Fact]
    public void WriteMorphFrame()
    {
        var bytes = BinaryHelper.FromString("""
            4D 6F 72 70 68 00 FD FD FD FD FD FD FD FD FD
            23 00 00 00
            00 00 80 3F
        """);
        var buffer = new ArrayBufferWriter<byte>();
        var bw = new BufferWriter(buffer);
        
        var frame = new VmdMorphFrame
        {
            Name = "Morph",
            FrameTime = 35,
            Weight = 1,
        };
        frame.Write(ref bw);

        var result = buffer.WrittenSpan.ToArray();
        
        Assert.Equal(result, bytes);
    }

    [Fact]
    public void WriteCameraFrame()
    {
        var bytes = BinaryHelper.FromString("""
            23 00 00 00
            00 00 A0 41
            00 00 00 00 00 00 20 41 00 00 00 00
            00 00 00 00 C3 F5 48 40 00 00 00 00
            14 6B 14 6B
            14 6B 14 6B
            14 6B 14 6B
            14 6B 14 6B
            14 6B 14 6B
            14 6B 14 6B
            1E 00 00 00
            00
        """);
        var buffer = new ArrayBufferWriter<byte>();
        var bw = new BufferWriter(buffer);
        
        var frame = new VmdCameraFrame
        {
            FrameTime = 35,
            Radius = 20,
            Position = new(0, 10, 0),
            Angle = new(0, 3.14f, 0),
            FovInDegree = 30,
        };
        frame.Write(ref bw, VmdVersion.MMDVer3);

        var result = buffer.WrittenSpan.ToArray();
        
        Assert.Equal(result, bytes);
    }

    [Fact]
    public void WriteLightFrame()
    {
        var bytes = BinaryHelper.FromString("""
            23 00 00 00
            9A 99 19 3F 9A 99 19 3F 9A 99 19 3F
            00 00 00 BF 00 00 80 BF 00 00 00 BF
        """);
        var buffer = new ArrayBufferWriter<byte>();
        var bw = new BufferWriter(buffer);
        
        var frame = new VmdLightFrame
        {
            FrameTime = 35,
            Color = new(0.6f, 0.6f, 0.6f),
            Position = new(-0.5f, -1, -0.5f),
        };
        frame.Write(ref bw);

        var result = buffer.WrittenSpan.ToArray();
        
        Assert.Equal(result, bytes);
    }

    [Fact]
    public void WriteSelfShadowFrame()
    {
        var bytes = BinaryHelper.FromString("""
            23 00 00 00
            02
            EC 51 38 3C
        """);
        var buffer = new ArrayBufferWriter<byte>();
        var bw = new BufferWriter(buffer);
        
        var frame = new VmdSelfShadowFrame
        {
            FrameTime = 35,
            Model = VmdSelfShadowModel.Model2,
            Distance = (10000f - 8875) / 100000,
        };
        frame.Write(ref bw);

        var result = buffer.WrittenSpan.ToArray();
        
        Assert.Equal(result, bytes);
    }

    [Fact]
    public void WritePropertyFrame()
    {
        var bytes = BinaryHelper.FromString("""
            23 00 00 00
            01
            01 00 00 00
            42 6F 6E 65 00 FD FD FD FD FD FD FD FD FD FD FD FD FD FD FD
            01
        """);
        var buffer = new ArrayBufferWriter<byte>();
        var bw = new BufferWriter(buffer);
        
        var frame = new VmdPropertyFrame
        {
            FrameTime = 35,
            IsVisible = true,
            IKEnabled =
            {
                { "Bone", true },  
            },
        };
        frame.Write(ref bw);

        var result = buffer.WrittenSpan.ToArray();
        
        Assert.Equal(result, bytes);
    }
}