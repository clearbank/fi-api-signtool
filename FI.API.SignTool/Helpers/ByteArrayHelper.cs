using System;
using System.Collections.Generic;
using System.Linq;

namespace FI.API.SignTool.Helpers
{
    public class ByteArrayHelper
    {
        public static byte[] TranslateByteArray(string text)
        {
            var values = text.Split(",".ToCharArray())
                .Select(Byte.Parse)
                .ToArray();

            return values;
        }

        public static string DescribeByteArray(IEnumerable<byte> bytes)
        {
            var text = String.Join(",", bytes.Select(b => b.ToString()));

            return text;
        }
    }
}
