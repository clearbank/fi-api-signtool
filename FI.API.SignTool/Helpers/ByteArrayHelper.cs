using System;
using System.Collections.Generic;
using System.Linq;

namespace FI.API.SignTool.Helpers;

public static class ByteArrayHelper
{
    public static byte[] TranslateByteArray(string? text) => (text ?? string.Empty)
        .Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
        .Select(byte.Parse)
        .ToArray();

    public static string DescribeByteArray(IEnumerable<byte>? bytes) =>
        string.Join(",", bytes?.Select(b => b.ToString()) ?? []);
}
