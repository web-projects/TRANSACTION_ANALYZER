using BUNDLE_VERIFIER.Config;
using Common.Helpers;
using Helpers;
using LoggerManager.ConsoleLogger;
using System;
using static TransactionValidator.Execution.Templates.TAC;

namespace Application.Execution
{
    internal static class TagProcessing
    {
        public static void PerformTerminalAnalysis(TerminalActionAnalysis terminalActionAnalysis)
        {
            // Process TVR Bytes
            DisplayLogger.WriteLine($"TAG 95  : {Utils.Split(terminalActionAnalysis.TerminalVerificationResults, 2, ' ')}");
            byte[] tvrBytes = ConversionHelper.HexToByteArray(terminalActionAnalysis.TerminalVerificationResults);
            ProcessTACResponse(tvrBytes);

            // Process IssuerActionCodeDefault Bytes
            DisplayLogger.WriteLine($"TAG 9F0D: {Utils.Split(terminalActionAnalysis.IssuerActionCodeDefault, 2, ' ')}");
            byte[] iacdfBytes = ConversionHelper.HexToByteArray(terminalActionAnalysis.IssuerActionCodeDefault);
            ProcessTACResponse(iacdfBytes);

            // Process IssuerActionCodeDenial Bytes
            DisplayLogger.WriteLine($"TAG 9F0E: {Utils.Split(terminalActionAnalysis.IssuerActionCodeDenial, 2, ' ')}");
            byte[] iacdnBytes = ConversionHelper.HexToByteArray(terminalActionAnalysis.IssuerActionCodeDenial);
            ProcessTACResponse(iacdnBytes);

            // Process IssuerActionCodeOnline Bytes
            DisplayLogger.WriteLine($"TAG 9F0F: {Utils.Split(terminalActionAnalysis.IssuerActionCodeOnline, 2, ' ')}");
            byte[] iacoBytes = ConversionHelper.HexToByteArray(terminalActionAnalysis.IssuerActionCodeOnline);
            ProcessTACResponse(iacoBytes);
        }

        private static void ProcessTACResponse(byte[] tvrBytes)
        {
            int index = 0;
            foreach (byte activeByte in tvrBytes)
            {
                switch (index)
                {
                    case 0:
                    {
                        DisplayLogger.WriteLine(string.Format("  BYTE 1: {0:X2}", activeByte));
                        DisplayTACActiveValues(activeByte, new TACByte1());
                        break;
                    }

                    case 1:
                    {
                        DisplayLogger.WriteLine(string.Format("  BYTE 2: {0:X2}", activeByte));
                        DisplayTACActiveValues(activeByte, new TACByte2());
                        break;
                    }

                    case 2:
                    {
                        DisplayLogger.WriteLine(string.Format("  BYTE 3: {0:X2}", activeByte));
                        DisplayTACActiveValues(activeByte, new TACByte3());
                        break;
                    }

                    case 3:
                    {
                        DisplayLogger.WriteLine(string.Format("  BYTE 4: {0:X2}", activeByte));
                        DisplayTACActiveValues(activeByte, new TACByte4());
                        break;
                    }

                    case 4:
                    {
                        DisplayLogger.WriteLine(string.Format("  BYTE 5: {0:X2}", activeByte));
                        DisplayTACActiveValues(activeByte, new TACByte5());
                        break;
                    }
                }

                index++;
            }
        }

        private static void DisplayTACActiveValues(byte activeByte, Enum tacByteTemplate)
        {
            Array enumValues = Enum.GetValues(tacByteTemplate.GetType());
            for (byte bitIndex = 8; bitIndex > 0; bitIndex--)
            {
                byte tacByte = (byte)((activeByte >> bitIndex) & 0x01);
                if (tacByte > 0)
                {
                    string message = StringValueAttribute.GetStringValue((Enum)enumValues.GetValue(bitIndex));
                    DisplayLogger.WriteLine(string.Format("          BIT-{0:D2}: {1}", bitIndex + 1, message));
                }
            }
        }
    }
}
