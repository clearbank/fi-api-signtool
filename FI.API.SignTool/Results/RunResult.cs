namespace FI.API.SignTool.Results
{
    public class RunResult
    {
        public byte[] UTF8EncodedData { get; set; }

        public byte[] HashedData { get; set; }

        public byte[] SignedData { get; set; }

        public string EncodedData { get; set; }
    }
}
