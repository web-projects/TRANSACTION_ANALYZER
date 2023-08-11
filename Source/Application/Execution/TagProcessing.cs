using BUNDLE_VERIFIER.Config;
using Common.Helpers;
using Helpers;
using LoggerManager.ConsoleLogger;
using System;
using System.Collections.Generic;
using System.Linq;
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
            byte[] iacDefaultBytes = ConversionHelper.HexToByteArray(terminalActionAnalysis.IssuerActionCodeDefault);
            ProcessTACResponse(iacDefaultBytes);

            // Process IssuerActionCodeDenial Bytes
            DisplayLogger.WriteLine($"TAG 9F0E: {Utils.Split(terminalActionAnalysis.IssuerActionCodeDenial, 2, ' ')}");
            byte[] iacDenialBytes = ConversionHelper.HexToByteArray(terminalActionAnalysis.IssuerActionCodeDenial);
            ProcessTACResponse(iacDenialBytes);

            // Process IssuerActionCodeOnline Bytes
            DisplayLogger.WriteLine($"TAG 9F0F: {Utils.Split(terminalActionAnalysis.IssuerActionCodeOnline, 2, ' ')}");
            byte[] iacOnlineBytes = ConversionHelper.HexToByteArray(terminalActionAnalysis.IssuerActionCodeOnline);
            ProcessTACResponse(iacOnlineBytes);

            // Evaluate Denial
            DisplayLogger.WriteLine("\r\n**** TERMINAL ANALYSIS: DENIAL ****");
            bool hasError = EvaluateTACList(tvrBytes, iacDenialBytes);
            if (hasError)
            {
                DisplayLogger.WriteLine("**** TERMINAL ANALYSIS: FOUND ERROR(S) ****");
            }

            // Evaluate Default
            DisplayLogger.WriteLine("\r\n**** TERMINAL ANALYSIS: DEFAULT ****");
            hasError = EvaluateTACList(tvrBytes, iacDefaultBytes);
            if (hasError)
            {
                DisplayLogger.WriteLine("**** TERMINAL ANALYSIS: FOUND ERROR(S) ****");
            }

            // Evaluate Online
            DisplayLogger.WriteLine("\r\n**** TERMINAL ANALYSIS: ONLINE ****");
            hasError = EvaluateTACList(tvrBytes, iacOnlineBytes);
            if (hasError)
            {
                DisplayLogger.WriteLine("**** TERMINAL ANALYSIS: FOUND ERROR(S) ****");
            }
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
            for (byte bitIndex = 7; bitIndex > 0; bitIndex--)
            {
                byte tacByte = (byte)((activeByte >> bitIndex) & 0x01);
                if (tacByte > 0)
                {
                    string message = StringValueAttribute.GetStringValue((Enum)enumValues.GetValue(bitIndex));
                    DisplayLogger.WriteLine(string.Format("          BIT-{0:D2}: {1}", bitIndex + 1, message));
                }
            }
        }

        private static bool EvaluateTACList(byte[] tvrBytes, byte[] iacDenialBytes)
        {
            bool isInError = false;

            // examine each byte
            int tacByteIndex = 1;
            IEnumerable<(byte tac, byte iac)> tacAnalysis = tvrBytes.Zip(iacDenialBytes, (i, j) => (i, j));

            foreach (var items in tacAnalysis)
            {
                DisplayLogger.WriteLine(string.Format("  BYTE {0}: [TAC-{1:X2}, IAC-{2:X2}]", tacByteIndex, items.tac, items.iac));
                Enum tacByteTemplate = GetCurrentTemplateByIndex(tacByteIndex++);
                Array enumValues = Enum.GetValues(tacByteTemplate.GetType());

                // examine each bit
                for (byte bitIndex = 7; bitIndex > 0; bitIndex--)
                {
                    byte tacByte = (byte)((items.tac >> bitIndex) & 0x01);
                    byte iacByte = (byte)((items.iac >> bitIndex) & 0x01);
                    if (tacByte > 0 && iacByte > 0)
                    {
                        string message = StringValueAttribute.GetStringValue((Enum)enumValues.GetValue(bitIndex));
                        DisplayLogger.WriteLine(string.Format("          BIT-{0:D2}: {1}", bitIndex + 1, message));
                        isInError = true;
                    }
                }
            }
            return isInError;
        }

        private static Enum GetCurrentTemplateByIndex(int tacByteIndex) => tacByteIndex switch
        {
            1 => new TACByte1(),
            2 => new TACByte2(),
            3 => new TACByte3(),
            4 => new TACByte4(),
            5 => new TACByte5(),
            _ => throw new Exception("Invalid TACByte index")
        };
    }
}
