using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hypsipetes
{
    public class TextModifier
    {
        private CyclicModifier _cyclic;
        private FixModifier _fix;
        private MemoHolder _holder;

        public CyclicModifier Cyclic 
        {
            get { return _cyclic; }
        }

        public FixModifier Fix
        {
            get { return _fix; }
        }

        public MemoHolder Holder
        {
            get { return _holder; }
        }

        public TextModifier()
        {
            _cyclic = new CyclicModifier();
            _fix = new FixModifier();
            _holder = new MemoHolder();
        }
    }
}
