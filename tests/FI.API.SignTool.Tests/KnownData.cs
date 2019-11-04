using System;
using System.IO;
using System.Linq;

namespace FI.API.SignTool.Tests
{
    public class KnownData
    {
        // NOTE: These files are the same as the ones provided with this repo in the `Data` folder
        //       Copied into this project to ensure they are always available to the test runner
        public const string KnownPublicKeyFileName = @"Data\FI.SignTool.Testing.pem";
        public const string KnownPrivateKeyFileName = @"Data\FI.SignTool.Testing.key";
        public static string KnownPublicKey => File.ReadAllText(KnownPublicKeyFileName);
        public static string KnownPrivateKey => File.ReadAllText(KnownPrivateKeyFileName);

        // NOTE: These values are copied from the reference example in the `readme.md`
        public const string KnownBody = "{ \"Message\": \"Hello World\" }";

        public const string KnownBodyHashText = "148,189,234,202,214,13,8,58,105,172,74,24,83,242,253,161,227,250,250,248,102,40,47,23,103,197,125,115,243,112,62,107";
        public static readonly byte[] KnownBodyHash = KnownBodyHashText
            .Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
            .Select(x => Convert.ToByte(x))
            .ToArray();

        public const string KnownBodySignatureText = "88,236,245,56,121,85,215,254,53,165,40,89,201,107,186,50,141,129,5,224,31,110,12,64,28,133,128,180,60,122,9,42,85,137,136,178,108,14,179,55,246,228,5,229,119,78,119,189,32,212,47,158,4,109,200,202,36,93,81,2,64,252,31,190,196,90,224,53,157,234,214,9,197,3,38,219,27,30,51,39,109,218,75,15,179,39,200,138,49,158,152,170,2,76,74,104,26,168,57,102,51,19,30,179,241,244,203,66,108,199,174,169,48,33,148,101,243,92,193,57,255,202,2,238,24,97,29,249,141,171,54,212,229,163,72,37,17,49,117,182,182,255,190,51,210,178,79,59,217,214,243,147,75,18,112,99,107,0,104,114,119,171,66,195,163,231,13,197,1,158,15,172,40,133,158,68,130,39,111,142,179,83,14,199,207,235,250,132,10,31,163,135,9,164,200,56,12,58,226,96,114,1,91,139,144,54,47,167,115,166,55,80,248,183,48,121,10,6,149,85,210,175,173,33,189,135,202,106,232,29,46,236,187,169,164,101,254,175,38,110,31,24,197,224,138,17,77,246,30,135,175,90,245,151,187,184";
        public static readonly byte[] KnownBodySignature = KnownBodySignatureText
            .Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
            .Select(x => Convert.ToByte(x))
            .ToArray();

        public const string KnownBodyDigitalSignature = "WOz1OHlV1/41pShZyWu6Mo2BBeAfbgxAHIWAtDx6CSpViYiybA6zN/bkBeV3Tne9INQvngRtyMokXVECQPwfvsRa4DWd6tYJxQMm2xseMydt2ksPsyfIijGemKoCTEpoGqg5ZjMTHrPx9MtCbMeuqTAhlGXzXME5/8oC7hhhHfmNqzbU5aNIJRExdba2/74z0rJPO9nW85NLEnBjawBocnerQsOj5w3FAZ4PrCiFnkSCJ2+Os1MOx8/r+oQKH6OHCaTIOAw64mByAVuLkDYvp3OmN1D4tzB5CgaVVdKvrSG9h8pq6B0u7LuppGX+ryZuHxjF4IoRTfYeh69a9Ze7uA==";
    }
}
