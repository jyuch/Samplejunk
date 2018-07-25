using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hypsipetes
{
    public class FixModifier
    {
        public string Prefix { set; get; }
        public string Suffix { set; get; }
        public string TargetText { set; get; }

        public string Modify()
        {
            var lines = TargetText.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            var buff = new StringBuilder();

            foreach (var it in lines)
            {
                buff.Append(Prefix).Append(it.Replace(Environment.NewLine, string.Empty)).AppendLine(Suffix);
            }

            return buff.ToString();
        }
    }
}
