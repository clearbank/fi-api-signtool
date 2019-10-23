using System;

namespace FI.API.SignTool.Exceptions
{
    public class CommandLineParserException : Exception
    {
        public CommandLineParserException(string message)
            : base(message)
        {
        }

        public CommandLineParserException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
