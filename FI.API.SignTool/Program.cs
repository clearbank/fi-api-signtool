using System;
using System.Security.Cryptography;
using System.Text;
using CommandLine;
using FI.API.SignTool.Extensions;
using FI.API.SignTool.Helpers;
using FI.API.SignTool.Parameters;
using FI.API.SignTool.Parameters.Interfaces;
using FI.API.SignTool.Results;
using FI.API.SignTool.SigningProviders;

namespace FI.API.SignTool
{
    internal class Program
    {
        internal class CBResponse
        {
            public int Nonce;
        }

        private static int Main(string[] args)
        {
            try
            {
                var runResult = new RunResult();

                var parser = new Parser(
                    config =>
                    {
                        config.CaseInsensitiveEnumValues = true;
                        config.CaseSensitive = false;
                        config.IgnoreUnknownArguments = true;
                        config.AutoHelp = true;
                        config.AutoVersion = true;
                        config.MaximumDisplayWidth = Console.IsOutputRedirected ? int.MaxValue : Console.WindowWidth;
                        config.HelpWriter = Console.Out;
                    }
                );

                var result = parser.ParseArguments<HashArguments, SignArguments, EncodeArguments, HashSignEncodeArguments>(args)
                    .MapResult(
                        (HashArguments arguments) => RunHash(arguments, runResult),
                        (SignArguments arguments) => RunSign(arguments, runResult),
                        (EncodeArguments arguments) => RunEncode(arguments, runResult),
                        (HashSignEncodeArguments arguments) => RunHashSignEncode(arguments, runResult),
                        errs => 1
                    );

                return result;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine();
                Console.Error.WriteLine($"Error: {ex.Message}");

                return 2;
            }
#if !NETCOREAPP
            finally
            {
                if (System.Diagnostics.Debugger.IsAttached)
                {
                    while (Console.KeyAvailable)
                        Console.ReadKey(true);

                    Console.Out.Write("Press any key to exit...");
                    Console.ReadKey(true);
                    Console.Out.WriteLine();
                }
            }
#endif
        }

        private static int RunHash(IHashArguments arguments, RunResult runResult)
        {
            Console.Out.WriteLine();
            Console.Out.WriteLine($"{nameof(RunHash)}");

            arguments.Validate();

            var inputData = arguments.InputData;

            Console.Out.WriteLine($"Using: {inputData}");

            runResult.UTF8EncodedData = Encoding.UTF8.GetBytes(inputData);
            Console.Out.WriteLine($"{nameof(RunResult.UTF8EncodedData)}: {ByteArrayHelper.DescribeByteArray(runResult.UTF8EncodedData)}");

            using (var hashProvider = new SHA256Managed())
            {
                runResult.HashedData = hashProvider.ComputeHash(runResult.UTF8EncodedData);
                Console.Out.WriteLine($"{nameof(RunResult.HashedData)} (SHA256): {ByteArrayHelper.DescribeByteArray(runResult.HashedData)}");
            }

            return 0;
        }

        private static int RunSign(ISignArguments arguments, RunResult runResult)
        {
            Console.Out.WriteLine();
            Console.Out.WriteLine($"{nameof(RunSign)}");

            arguments.Validate();

            var inputData = runResult.HashedData.HasAny()
                ? runResult.HashedData
                : ByteArrayHelper.TranslateByteArray(arguments.InputData);

            Console.Out.WriteLine($"Using: {ByteArrayHelper.DescribeByteArray(inputData)}");

            var signingProvider = SigningProviderFactory.GetSigningProvider(arguments);

            runResult.SignedData = signingProvider.SignHash(inputData);

            Console.Out.WriteLine($"{nameof(RunResult.SignedData)}: {ByteArrayHelper.DescribeByteArray(runResult.SignedData)}");

             return 0;
        }

        private static int RunEncode(IEncodeArguments arguments, RunResult runResult)
        {
            Console.Out.WriteLine();
            Console.Out.WriteLine($"{nameof(RunEncode)}");

            arguments.Validate();

            var inputData = runResult.SignedData.HasAny()
                ? runResult.SignedData
                : ByteArrayHelper.TranslateByteArray(arguments.InputData);

            Console.Out.WriteLine($"Using: {ByteArrayHelper.DescribeByteArray(inputData)}");

            runResult.EncodedData = Convert.ToBase64String(inputData);
            Console.Out.WriteLine($"{nameof(RunResult.EncodedData)}: {runResult.EncodedData}");

            return 0;
        }

        private static int RunHashSignEncode(IHashSignEncodeArguments arguments, RunResult runResult)
        {
            arguments.Validate();

            RunHash(arguments, runResult);
            RunSign(arguments, runResult);
            RunEncode(arguments, runResult);

            return 0;
        }
    }
}
