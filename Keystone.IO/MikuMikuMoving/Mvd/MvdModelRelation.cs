namespace Linearstar.Keystone.IO.MikuMikuMoving.Mvd
{
    public class MvdModelRelation
    {
        public int ExternalParentKey { get; set; }

        public int RelatedModelId { get; set; }

        public int RelatedBoneId { get; set; }

        internal static MvdModelRelation Parse(ref BufferReader br) =>
            new()
            {
                ExternalParentKey = br.ReadInt32(),
                RelatedModelId = br.ReadInt32(),
                RelatedBoneId = br.ReadInt32(),
            };

        internal void Write(ref BufferWriter bw)
        {
            bw.Write(this.ExternalParentKey);
            bw.Write(this.RelatedModelId);
            bw.Write(this.RelatedBoneId);
        }
    }
}