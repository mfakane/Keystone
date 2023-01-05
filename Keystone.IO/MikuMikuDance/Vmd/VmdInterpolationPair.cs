namespace Linearstar.Keystone.IO.MikuMikuDance.Vmd;

public struct VmdInterpolationPair
{
    public static readonly VmdInterpolationPair Default = new(VmdInterpolationPoint.DefaultA, VmdInterpolationPoint.DefaultB);
    
    public VmdInterpolationPoint A;
    public VmdInterpolationPoint B;

    public VmdInterpolationPair(VmdInterpolationPoint a, VmdInterpolationPoint b)
    {
        A = a;
        B = b;
    }

    public override string ToString() => $"{{{A}, {B}}}";
}