using CommandLine;
using FI.API.SignTool.Parameters.Interfaces;
using FI.API.SignTool.Types;

namespace FI.API.SignTool.Parameters
{
    [Verb(nameof(CommandType.Hash), HelpText = "Hash input text using SHA256")]
    public class HashArguments : IHashArguments
    {
        [Option('d', "data", Required = false, HelpText = "The data to be processed")]
        public string Data { get; set; }

        [Option('f', "datafilename", Required = false, HelpText = "The filename containing data to be processed")]
        public string DataFileName { get; set; }

        public string InputData => ArgumentsHelper.GetDataOrFileContent(Data, DataFileName);
        public void Validate()
        {
        }
    }
}