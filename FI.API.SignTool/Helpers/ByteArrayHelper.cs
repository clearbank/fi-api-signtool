using System;
using System.Collections.Generic;
using System.Linq;

namespace FI.API.SignTool.Helpers
{
    public class ByteArrayHelper
    {
        public static byte[] TranslateByteArray(string text)
        {
            var values = (text ?? string.Empty).Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                .Select(byte.Parse)
                .ToArray();

            return values;
        }

        public static string DescribeByteArray(IEnumerable<byte> bytes)
        {
            var text = string.Join(",", bytes?.Select(b => b.ToString()) ?? Enumerable.Empty<string>());

            return text;
        }
    }
}
