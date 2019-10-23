using System;
using System.IO;

namespace FI.API.SignTool.Parameters
{
    public class ArgumentsHelper
    {
        public static string GetDataOrFileContent(string data, string dataFileName)
        {
            if (string.IsNullOrWhiteSpace(data) && string.IsNullOrWhiteSpace(dataFileName))
                throw new Exception("Must specify Data or DataFileName");

            if (!string.IsNullOrWhiteSpace(data))
                return data;

            var fileInfo = new FileInfo(dataFileName);
            if (!fileInfo.Exists)
                throw new Exception($"File does not exist - {dataFileName}");

            return File.ReadAllText(fileInfo.FullName);
        }
    }
}
