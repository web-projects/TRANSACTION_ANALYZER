using Common.Helpers;

namespace TransactionValidator.Execution.Templates
{
    internal class TAC
    {
        public enum TACByte1
        {
            [StringValue("RFU")]
            RFU00 = 1,
            [StringValue("RFU")]
            RFU01,
            [StringValue("CDA failed")]
            CDAFailed,
            [StringValue("DDA failed")]
            DDAFailed,
            [StringValue("Card appears on terminal exception file")]
            CardinTerminalExceptionFile,
            [StringValue("ICC data missing")]
            ICCDataMissing,
            [StringValue("SDA failed")]
            SDAFailed,
            [StringValue("Offline data authentication was not performed")]
            OfflineDataAuthenticationNotPerformed
        }

        public enum TACByte2
        {
            [StringValue("RFU")]
            RFU00 = 1,
            [StringValue("RFU")]
            RFU01,
            [StringValue("RFU")]
            RFU02,
            [StringValue("New card")]
            NewCard,
            [StringValue("Requested service not allowed for card product")]
            RequestedServiceNotAllowed,
            [StringValue("Application not yet effective")]
            ApplicationNotYetEffective,
            [StringValue("Expired application")]
            ExpiredApplication,
            [StringValue("ICC and terminal have different application versions")]
            DifferentApplications
        }

        public enum TACByte3
        {
            [StringValue("RFU")]
            RFU00 = 1,
            [StringValue("RFU")]
            RFU01,
            [StringValue("Online PIN entered")]
            OnlinePINEntered,
            [StringValue("PIN entry required, PIN pad present, but PIN was not entered")]
            PinEntryRequiredPINNotEntered,
            [StringValue("PIN entry required and PIN pad not present or not working")]
            PinEntryRequiredPINPadNotPresent,
            [StringValue("PIN Try Limit exceeded")]
            PinTryLimitExceeded,
            [StringValue("Unrecognized CVM")]
            UnrecognizedCVM,
            [StringValue("Cardholder verification was not successful")]
            CardholderVerificationNotSuccessful
        }

        public enum TACByte4
        {
            [StringValue("RFU")]
            RFU00 = 1,
            [StringValue("RFU")]
            RFU01,
            [StringValue("RFU")]
            RFU02,
            [StringValue("Merchant forced transaction online")]
            MerchantForcedTransactionOnline,
            [StringValue("Transaction selected randomly for online processing")]
            TransactionSelectedRandomlyforOnlineProcessing,
            [StringValue("Upper consecutive offline limit exceeded")]
            UpperConsecutiveOfflineLimitExceeded,
            [StringValue("Lower consecutive offline limit exceeded")]
            LowerConsecutiveOfflineLimitExceeded,
            [StringValue("Transaction exceeds floor limit")]
            TransactionExceedsFloorLimit
        }

        public enum TACByte5
        {
            [StringValue("RFU")]
            RFU00 = 1,
            [StringValue("RFU")]
            RFU01,
            [StringValue("RFU")]
            RFU02,
            [StringValue("RFU")]
            RFU03,
            [StringValue("Script processing failed after final GENERATE AC")]
            ScriptProcessingFailedAfterFinalGAC,
            [StringValue("Script processing failed before final GENERATE AC")]
            ScriptProcessingFailedBeforeFinalGAC,
            [StringValue("Issuer authentication failed")]
            IssuerAuthenticationFailed,
            [StringValue("Default TDOL used")]
            DefaultTDOLUsed
        }
    }
}
