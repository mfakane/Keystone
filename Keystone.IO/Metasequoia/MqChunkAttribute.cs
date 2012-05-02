using System.Collections.Generic;
using System.Linq;
using Linearstar.Keystone.IO;

namespace Linearstar.Keystone.IO.Metasequoia
{
	/// <summary>
	/// attribute(args)
	/// </summary>
	public class MqChunkAttribute
	{
		public string Name
		{
			get;
			set;
		}

		public List<string> Arguments
		{
			get;
			set;
		}

		public MqChunkAttribute()
		{
			this.Arguments = new List<string>();
		}

		public MqChunkAttribute(string name, params string[] args)
			: this()
		{
			this.Name = name;
			this.Arguments.AddRange(args);
		}

		public MqChunkAttribute(string name, IEnumerable<string> args)
			: this(name, args.ToArray())
		{
		}

		public MqChunkAttribute SetArguments(IEnumerable<string> args)
		{
			return SetArguments(args.ToArray());
		}

		public MqChunkAttribute SetArguments(params string[] args)
		{
			this.Arguments.Clear();
			this.Arguments.AddRange(args);

			return this;
		}

		public string GetFormattedText()
		{
			return this.Name + "(" + string.Join(" ", this.Arguments) + ")";
		}
	}
}
