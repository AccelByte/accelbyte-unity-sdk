// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

namespace AccelByte.Utils
{
    public class AccelByteIdValidator
    {
        public const int AccelByteIdLength = 32;
        public const int AccelByteIdWithHypensLength = 36;

        internal virtual bool IsAccelByteIdValid(string accelByteId, AccelByteIdValidator.HypensRule hypensRule = HypensRule.NoRule)
        {
            if (string.IsNullOrEmpty(accelByteId))
            {
                return false;
            }

            string processedAccelByteId = accelByteId;
            int expectedIdLength = AccelByteIdLength;

            if (hypensRule == HypensRule.NoRule)
            {
                processedAccelByteId = processedAccelByteId.Replace("-", "");
            }
            else if (hypensRule == HypensRule.WithHypens)
            {
                expectedIdLength = AccelByteIdWithHypensLength;
            }

            if (processedAccelByteId.Length != expectedIdLength)
            {
                return false;
            }

            bool isValid = System.Guid.TryParse(processedAccelByteId, out _);
            return isValid;
        }

        public static string GetChatIdInvalidMessage(string chatId)
        {
            const string formatMessage = "Invalid request, Chat Id format is invalid";
            return $"{formatMessage}\nValue: {chatId}";
        }

        public static string GetPartyIdInvalidMessage(string partyId)
        {
            const string formatMessage = "Invalid request, Party Id format is invalid";
            return $"{formatMessage}\nValue: {partyId}";
        }

        public static string GetSessionIdInvalidMessage(string sessionId)
        {
            const string formatMessage = "Invalid request, Session Id format is invalid";
            return $"{formatMessage}\nValue: {sessionId}";
        }

        public static string GetUserIdInvalidMessage(string userId)
        {
            const string formatMessage = "Invalid request, User Id format is invalid";
            return $"{formatMessage}\nValue: {userId}";
        }

        public static string GetChannelIdInvalidMessage(string channelId)
        {
            const string formatMessage = "Invalid request, Channel Id format is invalid";
            return $"{formatMessage}\nValue: {channelId}";
        }

        public static string GetContenttIdInvalidMessage(string contentId)
        {
            const string formatMessage = "Invalid request, Content Id format is invalid";
            return $"{formatMessage}\nValue: {contentId}";
        }
        
        public static string GetProfileIdInvalidMessage(string profileId)
        {
            const string formatMessage = "Invalid request, Profile Id format is invalid";
            return $"{formatMessage}\nValue: {profileId}";
        }

        public static string GetEntitlementIdInvalidMessage(string entitlementId)
        {
            const string formatMessage = "Invalid request, Entitlement Id format is invalid";
            return $"{formatMessage}\nValue: {entitlementId}";
        }

        public static string GetTicketIdInvalidMessage(string ticketId)
        {
            const string formatMessage = "Invalid request, Ticket Id format is invalid";
            return $"{formatMessage}\nValue: {ticketId}";
        }

        public static string GetSeasonIdInvalidMessage(string seasonId)
        {
            const string formatMessage = "Invalid request, Season Id format is invalid";
            return $"{formatMessage}\nValue: {seasonId}";
        }

        public static string GetCycleIdInvalidMessage(string cycleId)
        {
            const string formatMessage = "Invalid request, Cycle Id format is invalid";
            return $"{formatMessage}\nValue: {cycleId}";
        }

        public static string GetMatchIdInvalidMessage(string matchId)
        {
            const string formatMessage = "Invalid request, Match Id format is invalid";
            return $"{formatMessage}\nValue: {matchId}";
        }

        public static string GetBanIdInvalidMessage(string banId)
        {
            const string formatMessage = "Invalid request, Ban Id format is invalid";
            return $"{formatMessage}\nValue: {banId}";
        }

        internal enum HypensRule
        {
            NoRule,
            NoHypens,
            WithHypens
        }
    }
}
