using Application.Config;
using Application.Execution;
using BUNDLE_VERIFIER.Config;
using Common.LoggerManager;
using System;

namespace BUNDLE_VERIFIER
{
    class Program
    {
        private static AppConfig configuration;

        static void Main(string[] args)
        {
            configuration = SetupEnvironment.SetEnvironment();

            // Validate Active Index
            if (configuration.Application.ActiveBundleIndex > configuration.TerminalActionAnalysis.Count)
            {
                Console.WriteLine($"INVALID INDEX {configuration.Application.ActiveBundleIndex} FOR TerminalActionAnalysis COUNT: {configuration.TerminalActionAnalysis.Count}");
                Logger.error($"INVALID INDEX {configuration.Application.ActiveBundleIndex} FOR TerminalActionAnalysis COUNT: {configuration.TerminalActionAnalysis.Count}");
            }
            else
            {
                TagProcessing.PerformTerminalAnalysis(new TerminalActionAnalysis()
                {
                    TerminalVerificationResults = configuration.TerminalActionAnalysis[configuration.Application.ActiveBundleIndex].TerminalVerificationResults,
                    IssuerActionCodeDefault = configuration.TerminalActionAnalysis[configuration.Application.ActiveBundleIndex].IssuerActionCodeDefault,
                    IssuerActionCodeDenial = configuration.TerminalActionAnalysis[configuration.Application.ActiveBundleIndex].IssuerActionCodeDenial,
                    IssuerActionCodeOnline = configuration.TerminalActionAnalysis[configuration.Application.ActiveBundleIndex].IssuerActionCodeOnline
                });
            }

#if !DEBUG
            Console.WriteLine("\r\n\r\nPress <ENTER> key to exit...");

            ConsoleKeyInfo keypressed = Console.ReadKey(true);

            while (keypressed.Key != ConsoleKey.Enter)
            {
                keypressed = Console.ReadKey(true);
                System.Threading.Thread.Sleep(100);
            }
#endif

            Console.WriteLine("APPLICATION EXITING ...");
            Console.WriteLine("");
        }
    }
}
