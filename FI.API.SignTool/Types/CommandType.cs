using System;

namespace FI.API.SignTool.Types
{
    [Flags]
    public enum CommandType
    {
        Hash = 1,
        Sign = 2,
        Encode = 4,

        HashSignEncode = Hash | Sign | Encode
    }
}
