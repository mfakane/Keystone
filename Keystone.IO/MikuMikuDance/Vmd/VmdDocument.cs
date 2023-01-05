using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Linearstar.Keystone.IO.MikuMikuDance.Vmd
{
    /// <summary>
    /// Vocaloid Motion Data file created by Higuchi_U
    /// </summary>
    public class VmdDocument
    {
        public const string DisplayName = "Vocaloid Motion Data file";
        public const string Filter = "*.vmd";
        
        internal static Encoding Encoding => Encoding.GetEncoding(932);

        public VmdVersion Version { get; set; } = VmdVersion.MMDVer3;

        public string ModelName { get; set; } = "miku.osm";

        public IList<VmdBoneFrame> BoneFrames { get; set; } = new List<VmdBoneFrame>();

        public IList<VmdMorphFrame> MorphFrames { get; set; } = new List<VmdMorphFrame>();

        public IList<VmdCameraFrame> CameraFrames { get; set; } = new List<VmdCameraFrame>();

        public IList<VmdLightFrame> LightFrames { get; set; } = new List<VmdLightFrame>();

        public IList<VmdSelfShadowFrame> SelfShadowFrames { get; set; } = new List<VmdSelfShadowFrame>();

        public IList<VmdPropertyFrame> PropertyFrames { get; set; } = new List<VmdPropertyFrame>();

        static VmdDocument()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }
        
        public static VmdDocument FromFile(string path)
        {
            using var fs = File.OpenRead(path);

            return Parse(fs);
        }
        
        public static VmdDocument Parse(in ReadOnlySequence<byte> sequence)
        {
            var rt = new VmdDocument();
            var br = new BufferReader(sequence);
            var header = br.ReadString(30);

            if (header == "Vocaloid Motion Data file")
                rt.Version = VmdVersion.MMDVer2;
            else if (header == "Vocaloid Motion Data 0002")
                rt.Version = VmdVersion.MMDVer3;
            else
                throw new InvalidOperationException("invalid format");

            rt.ModelName = br.ReadString(rt.Version == VmdVersion.MMDVer2 ? 10 : 20);

            var bones = br.ReadUInt32();
            for (uint i = 0; i < bones; i++)
                rt.BoneFrames.Add(VmdBoneFrame.Parse(ref br));
            
            var morphs = br.ReadUInt32();
            for (uint i = 0; i < morphs; i++)
                rt.MorphFrames.Add(VmdMorphFrame.Parse(ref br));
            
            var cameras = br.ReadUInt32();
            for (uint i = 0; i < cameras; i++)
                rt.CameraFrames.Add(VmdCameraFrame.Parse(ref br, rt.Version));
            
            var lights = br.ReadUInt32();
            for (uint i = 0; i < lights; i++)
                rt.LightFrames.Add(VmdLightFrame.Parse(ref br));

            if (!br.IsCompleted)
            {
                var shadows = br.ReadUInt32();
                for (uint i = 0; i < shadows; i++)
                    rt.SelfShadowFrames.Add(VmdSelfShadowFrame.Parse(ref br));
            }

            if (!br.IsCompleted)
            {
                var properties = br.ReadUInt32();
                for (uint i = 0; i < properties; i++)
                    rt.PropertyFrames.Add(VmdPropertyFrame.Parse(ref br));
            }

            return rt;
        }

        public static VmdDocument Parse(Stream stream) =>
            Parse(stream.AsReadOnlySequence());

        public void Write(IBufferWriter<byte> writer)
        {
            var bw = new BufferWriter(writer);

            if (this.Version == VmdVersion.MMDVer2)
            {
                bw.Write("Vocaloid Motion Data file", 30, 0);
                bw.Write(this.ModelName, 10);
            }
            else
            {
                bw.Write("Vocaloid Motion Data 0002", 30, 0);
                bw.Write(this.ModelName, 20);
            }

            bw.Write(this.BoneFrames.Count);
            foreach (var bone in this.BoneFrames) bone.Write(ref bw);
            
            bw.Write(this.MorphFrames.Count);
            foreach (var morph in this.MorphFrames) morph.Write(ref bw);
            
            bw.Write(this.CameraFrames.Count);
            foreach (var camera in this.CameraFrames) camera.Write(ref bw, this.Version);
            
            bw.Write(this.LightFrames.Count);
            foreach (var light in this.LightFrames) light.Write(ref bw);

            if (this.Version == VmdVersion.MMDVer3)
            {
                if (this.SelfShadowFrames.Any() || this.PropertyFrames.Any())
                {
                    bw.Write(this.SelfShadowFrames.Count);
                    foreach (var shadow in this.SelfShadowFrames) shadow.Write(ref bw);
                }
                
                if (this.PropertyFrames.Any())
                {
                    bw.Write(this.PropertyFrames.Count);
                    foreach (var property in this.PropertyFrames) property.Write(ref bw);
                }
            }

            bw.Write(0);
        }
		
        public void Write(Stream stream)
        {
            using var sbw = new StreamBufferWriter(stream);

            Write(sbw);
        }
    }
}