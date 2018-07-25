using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hypsipetes
{
    public class CyclicModifier
    {
        public int StartIndex { get; set; }
        public int StopIndex { get; set; }
        public int Step { get; set; }
        public string TargetText { set; get; }

        public CyclicModifier()
        {
            Step = 1;
        }

        public string Modify()
        {
            var normalizeStep = Math.Abs(Step);
            Func<int, int, bool> pre;

            if (StartIndex > StopIndex)
            {
                normalizeStep *= -1;
                pre = (index, limit) => index >=limit;
            }
            else
            {
                pre = (index, limit) => index <=limit;
            }
            
            var buff = new StringBuilder();
            for(int i = StartIndex; pre(i, StopIndex); i += normalizeStep)
            {
                buff.AppendLine(string.Format(TargetText, i));
            }

            return buff.ToString();
        }
    }
}
