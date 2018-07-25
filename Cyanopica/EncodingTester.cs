using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cyanopica
{
    public class EncodingTester
    {
        public static Encoding CreateEncoding(int codepage, bool isBestFit, string replacementText)
        {
            if (isBestFit)
            {
                return Encoding.GetEncoding(codepage);
            }
            else
            {
                var replace = replacementText ?? string.Empty;
                return Encoding.GetEncoding(
                    codepage,
                    new EncoderReplacementFallback(replace),
                    new DecoderReplacementFallback(replace));
            }
        }

        public static string EncodeAndDecode(Encoding encoder, Encoding decoder, string target)
        {
            var text = target ?? "";
            return decoder.GetString(encoder.GetBytes(text));
        }

        public static string Hexdump(Encoding encoder, string target)
        {
            var text = target ?? "";
            var array = encoder.GetBytes(text);
            return GetHexdump(array);
        }

        private static string GetHexdump(byte[] array)
        {
            var content = new StringBuilder();
            content.AppendLine("         00 01 02 03 04 05 06 07 08 09 0a 0b 0c 0d 0e 0f");
            content.AppendLine("======== -----------------------------------------------");

            var l = array.Zip(Enumerable.Range(0, array.Length), Tuple.Create);

            foreach(var it in l)
            {
                if (it.Item2 != 0 && it.Item2 % 16 == 0)
                    content.AppendLine();
                if (it.Item2 % 16 == 0)
                    content.Append(string.Format("{0:X8} ", it.Item2));
                content.Append(string.Format("{0:X2} ", it.Item1));
            }

            return content.ToString();
        }
    }
}
