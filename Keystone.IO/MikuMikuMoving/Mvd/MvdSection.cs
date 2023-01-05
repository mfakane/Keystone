using System.IO;

namespace Linearstar.Keystone.IO.MikuMikuMoving.Mvd
{
    public abstract class MvdSection
    {
        protected MvdTag Tag { get; set; }

        public byte MinorType { get; protected set; }

        protected int RawKey { get; set; }

        protected int RawItemSize { get; set; }

        protected int RawCount { get; set; }

        protected MvdSection(MvdTag tag)
        {
            this.Tag = tag;
        }

        internal virtual void ReadExtensionRegion(ref BufferReader br, MvdDocument document, MvdObject obj)
        {
        }

        internal virtual void WriteExtensionRegion(ref BufferWriter bw, MvdDocument document)
        {
        }

        internal abstract void Read(ref BufferReader bw, MvdDocument document, MvdObject obj);

        internal static MvdSection? Parse(ref BufferReader br, MvdDocument document, MvdObject obj)
        {
            var tag = (MvdTag)br.ReadByte();
            MvdSection? rt = null;

            switch (tag)
            {
                case MvdTag.NameList:
                    rt = new MvdNameList();

                    break;
                case MvdTag.Bone:
                    rt = new MvdBoneData();

                    break;
                case MvdTag.Morph:
                    rt = new MvdMorphData();

                    break;
                case MvdTag.MotionClip:
                    rt = new MvdMotionClipData();

                    break;
                case MvdTag.MotionBlend:
                    rt = new MvdMotionBlendLinkData();

                    break;
                case MvdTag.ModelProperty:
                    rt = new MvdModelPropertyData();

                    break;
                case MvdTag.AccessoryProperty:
                    rt = new MvdAccessoryPropertyData();

                    break;
                case MvdTag.EffectProperty:
                    rt = new MvdEffectPropertyData();

                    break;
                case MvdTag.Camera:
                    rt = new MvdCameraData();

                    break;
                case MvdTag.CameraProperty:
                    rt = new MvdCameraPropertyData();

                    break;
                case MvdTag.Light:
                    rt = new MvdLightData();

                    break;
                case MvdTag.Project:
                    rt = new MvdProjectData();

                    break;
                case MvdTag.Filter:
                    rt = new MvdFilterData();

                    break;
                case MvdTag.Eof:
                    br.ReadByte();

                    return null;
            }

            rt.MinorType = br.ReadByte();
            rt.RawKey = br.ReadInt32();
            rt.RawItemSize = br.ReadInt32();
            rt.RawCount = br.ReadInt32();

            var extensionRegionLength = br.ReadInt32();
            if (extensionRegionLength > 0)
            {
                var exr = new BufferReader(br.ReadSequence(extensionRegionLength));
                rt.ReadExtensionRegion(ref exr, document, obj);
            }

            rt.Read(ref br, document, obj);

            return rt;
        }

        internal virtual void Write(ref BufferWriter bw, MvdDocument document)
        {
            bw.Write((byte)this.Tag);
            bw.Write(this.MinorType);
            bw.Write(this.RawKey);
            bw.Write(this.RawItemSize);
            bw.Write(this.RawCount);

            using var ms = new MemoryStream();
            
            using (var sbw = new StreamBufferWriter(ms))
            {
                var exw = new BufferWriter(sbw);
                WriteExtensionRegion(ref exw, document);
            }

            bw.Write(ms.Length);
            bw.Write(ms.ToArray());
        }
    }
}