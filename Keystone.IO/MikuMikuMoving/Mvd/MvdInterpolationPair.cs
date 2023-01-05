namespace Linearstar.Keystone.IO.MikuMikuMoving.Mvd;

public struct MvdInterpolationPair
{
    public static readonly MvdInterpolationPair Default = new(MvdInterpolationPoint.DefaultA, MvdInterpolationPoint.DefaultB);
    
    public MvdInterpolationPoint A;
    public MvdInterpolationPoint B;

    public MvdInterpolationPair(MvdInterpolationPoint a, MvdInterpolationPoint b)
    {
        A = a;
        B = b;
    }

    internal static MvdInterpolationPair Parse(ref BufferReader br) => new(MvdInterpolationPoint.Parse(ref br), MvdInterpolationPoint.Parse(ref br));

    internal void Write(ref BufferWriter bw)
    {
        this.A.Write(ref bw);
        this.B.Write(ref bw);
    }

    public override string ToString() => $"{{{A}, {B}}}";
}