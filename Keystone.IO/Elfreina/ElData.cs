using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Linearstar.Keystone.IO.Elfreina
{
    public class ElData
    {
        public string Name { get; set; }

        public List<string> Values { get; set; }

        public List<ElData> Children { get; set; }

        public ElData()
        {
        }

        public ElData(string name)
            : this()
        {
            this.Name = name;
        }

        public ElData SetValues(IEnumerable<string> args)
        {
            return SetValues(args.ToArray());
        }

        public ElData SetValues(params string[] args)
        {
            if (this.Values == null)
                this.Values = new List<string>();
            else
                this.Values.Clear();

            this.Values.AddRange(args);

            return this;
        }

        public ElData Child(string name, bool createIfNotFound = true)
        {
            if (this.Children == null)
                if (createIfNotFound)
                    this.Children = new List<ElData>();
                else
                    return null;

            var value = this.Children.FirstOrDefault(_ => _.Name.Equals(name));

            if (createIfNotFound && value == null)
                this.Children.Add(value = new ElData
                {
                    Name = name,
                });

            return value;
        }

        public void RemoveChildren(string name)
        {
            if (this.Children != null)
                this.Children.Where(_ => _.Name.Equals(name)).ToArray().ForEach(_ => this.Children.Remove(_));
        }

        public static ElData Parse(ElTokenizer tokenizer)
        {
            while (tokenizer.Current.Kind == ElTokenizer.NewLineTokenKind)
                tokenizer.MoveNext();

            var rt = new ElData();

            if (tokenizer.Current.Kind == ElTokenizer.IdentifierTokenKind)
            {
                rt.Name = tokenizer.Current.Text;
                tokenizer.MoveNext();
            }

            if (tokenizer.Current.Kind == ElTokenizer.BeginChildrenTokenKind)
            {
                rt.Children = tokenizer.TakeWhile(_ => _.Kind != ElTokenizer.EndChildrenTokenKind)
                    .Select(_ => Parse(tokenizer))
                    .ToList();
                tokenizer.MoveNext(ElTokenizer.EndChildrenTokenKind);
            }
            else
                rt.Values = tokenizer.StartWith(tokenizer.Current)
                    .TakeWhile(
                        _ => _.Kind != ElTokenizer.NewLineTokenKind && _.Kind != ElTokenizer.EndChildrenTokenKind)
                    .Select(_ => _.Text)
                    .ToList();

            return rt;
        }

        public string GetFormattedText()
        {
            var sb = new StringBuilder();

            if (this.Name != null)
                sb.Append(this.Name);

            if (this.Values != null)
            {
                if (this.Name != null)
                    sb.Append("=");

                sb.Append(string.Join(":", this.Values));
            }

            if (this.Children != null)
            {
                sb.AppendLine(" {");

                foreach (var i in this.Children)
                    sb.AppendLine("\t" + i.GetFormattedText().Replace("\n", "\n\t"));

                sb.Append("}");
            }

            return sb.ToString();
        }
    }
}