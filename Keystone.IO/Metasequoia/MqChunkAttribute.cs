using System.Collections.Generic;
using System.Linq;

namespace Linearstar.Keystone.IO.Metasequoia
{
    /// <summary>
    /// attribute(args)
    /// </summary>
    public class MqChunkAttribute
    {
        public string Name { get; set; }

        public IList<string> Arguments { get; set; }
        
        public MqChunkAttribute(string name, params string[] args)
        {
            this.Name = name;
            this.Arguments = new List<string>(args);
        }

        public MqChunkAttribute(string name, IEnumerable<string> args)
        {
            this.Name = name;
            this.Arguments = new List<string>(args);
        }

        public MqChunkAttribute SetArguments(IEnumerable<string> args)
        {
            return SetArguments(args.ToArray());
        }

        public MqChunkAttribute SetArguments(params string[] args)
        {
            this.Arguments = new List<string>(args);

            return this;
        }

        public string GetFormattedText()
        {
            return this.Name + "(" + string.Join(" ", this.Arguments) + ")";
        }
    }
}