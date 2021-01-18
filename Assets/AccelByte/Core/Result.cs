// Copyright (c) 2018 - 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

namespace AccelByte.Core
{
    //Although HTTP Status Codes are standard way to communicate web requests error, it's definitely not sufficient.
    //Justice error codes coincidentally don't conflict with HTTP status codes, so we want to merge them to make
    //error handling simpler. Other than Justice error codes, this will also includes error codes for debugging,
    //such as when a web request is ill formed or when response can't be parsed.

    public enum ErrorCode
    {
        //No Error
        None = 0,

        //HTTP Status
        BadRequest = 400,
        Unauthorized = 401,
        PaymentRequired = 402,
        Forbidden = 403,
        NotFound = 404,
        MethodNotAllowed = 405,
        NotAcceptable = 406,
        ProxyAuthenticationRequired = 407,
        RequestTimeout = 408,
        Conflict = 409,
        Gone = 410,
        LengthRequired = 411,
        PreconditionFailed = 412,
        RequestEntityTooLarge = 413,
        RequestUriTooLong = 414,
        UnsupportedMediaType = 415,
        RequestedRangeNotSatisfiable = 416,
        ExpectationFailed = 417,
        UnprocessableEntity = 422,
        InternalServerError = 500,
        NotImplemented = 501,
        BadGateway = 502,
        ServiceUnavailable = 503,
        GatewayTimeout = 504,
        HttpVersionNotSupported = 505,

        //AccelByte Global Service error codes (20000 - 20022)
        UnauthorizedAccess = 20001,
        ValidationError = 20002,
        OptimisticLock = 20006,
        TooManyRequests = 20007,
        UserNotFound = 20008,
        UnknownError = 20009,
        UserIsNotLinkedToNamespace = 20017,
        ActionBanned = 20016,
        UnableToParseRequestBody = 20019,
        TokenIsNotUserToken = 20022,

        //AccelByte Launcher error codes (20 - 99)

        //AccelByte E-Commerce error codes (1000 - 5000)
        StoreNotFound = 30141,
        PublisherStoreNotExist = 30142,
        CategoryNotFound = 30241,
        ItemNotFound = 30341,
        EntitlementNotDistributable = 31121,
        ExceedDistributeQuantity = 31122,
        PublisherNamespaceNotDistributable = 31123,
        EntitlementIdNotFound = 31141,
        EntitlementAppIdNotFound = 31142,
        DistributionReceiverNotFound = 31241,
        DistributionReceiverAlreadyExist = 31271,
        OrderPriceMismatch = 32121,
        OrderNotFound = 32141,
        OrderInvalidStatus = 32172,
        OrderReceiptNotDownloadable = 32173,
        ExceedItemMaxCountPerUser = 32175,
        ExceedItemMaxCount = 32176,
        OrderNotCancelable = 32177,
        WalletExceedMaxTransactionAmountPerDay = 35121,
        WalletExceedMaxAmountPerTransaction = 35122,
        WalletInactive = 35123,
        WalletInsufficientBalance = 35124,
        WalletExceedMaxBalanceAmount = 35125,
        UserWalletDoesNotExist = 35141,
        CurrencyNotFound = 36141,
        NamespaceNotFound = 2141,
        UserProfileNotFound = 2241,
        WalletAlreadyExist = 3571,

        //Leaderboard error codes
        LeaderboardRankingUnableToRetrieve = 71233,
        LeaderboardRankingNotFound = 71235,
        LeaderboardConfigNotFound = 71230,

        //AccelByte Statistic error codes
        StatisticConfigNotFound = 12241,
        StatisticNotFound = 12241,
        InvalidStatOperator = 12221,
        StatNotDecreasable = 12273,
        UserStatsNotFound = 12242,
        UserStatAlreadyExist = 12274,
        StatValueOutOfRange = 12275,

        //AccelByte Group error codes
        UserNotBelongToAnyGroup = 73034,
        InsufficientMemberRolePermission = 73036,
        UserAccessDifferentGroup = 73037,
        GroupNotFound = 73333,
        UserAlreadyJoinedGroup = 73342,
        UserAlreadyInvited = 73437,
        UserAlreadyRequestedToJoin = 73438,
        GroupAdminCannotLeaveGroup = 73440,
        PrivateGroupIsNotJoinable = 73441,
        UserAlreadyJoinedInAnotherGroup = 73442,
        MemberRequestNotFound = 73443,

        //Client side error codes
        GeneralClientError = 14000,
        ErrorFromException = 14001,
        InvalidArgument = 14002,
        InvalidRequest = 14003,
        InvalidResponse = 14004,
        NetworkError = 14005,
        IsNotLoggedIn = 14006,
        UserProfileIsNotCreated = 14007,
        UserProfileConflict = 11441,
        GenerateTokenFailed = 14008,
        AccountIsNotUpgraded = 14009,
        VerificationCodeIsNotRequested = 14010,
        EmailIsNotVerified = 14011,
        EntitlementNotCreated = 14012,
        GenerateAuthCodeFailed = 14013,
        AccessDenied = 14014,
        MessageFieldTypeNotSupported = 14015,
        MessageFormatInvalid = 14016,
        MessageFieldDoesNotExist = 14017,
        MessageFieldConversionFailed = 14018,
        MessageCannotBeSent = 14019,
        MessageTypeNotSupported = 14020,
        
        UserUnderage = 10130,
        EmailAlreadyUsed = 10133,
        CountryNotFound = 10154,
        PlatformAlreadyLinked = 10173,
        UserAlreadyExist = 10180,
        FriendListIsEmpty = 11732,

        //AccelByte Lobby error codes
        //Connection
        LobbyConnectionUnableToUpgrade = 11211,
        LobbyConnectionUnableToRegisterUser = 11212,
        LobbyConnectionMultipleLoginAttempt = 11215,
        LobbyConnectionUnableCheckConnectedUser = 11216,
        LobbyConnectionUnableToValidateSession = 11217,
        LobbyConnectionInvalidSession = 11218,
        //Party Info
        PartyInfoUnableGetUserRegistry = 11221,
        PartyInfoUnableGetUserPartyInfo = 11222,
        PartyInfoSuccessGetUserPartyInfoEmpty = 11223,
        //Party Creation
        PartyCreationUnableGetUserRegistry = 11231,
        PartyCreationAlreadyInParty = 11232,
        PartyCreationUnableCreateParty = 11233,
        //Party Leave
        PartyLeaveUnableGetUserRegistry = 11241,
        PartyLeavePartyIsNil = 11242,
        PartyLeavePartyNotFound = 11243,
        PartyLeaveUserNotInParty = 11244,
        PartyLeaveUnableToLeaveParty = 11245,
        // Party Invite
        PartyInviteUnableGetUserRegistry = 11251,
        PartyInviteInviteeNotFound = 11252,
        PartyInviteNotInParty = 11253,
        PartyInviteInviteeInParty = 11254,
        PartyInviteUnableInviteToParty = 11255,
        //Party Join
        PartyJoinUnableGetUserRegistry = 11261,
        PartyJoinPartyNotFound = 11262,
        PartyJoinInvalidInvitationToken = 11263,
        PartyJoinNotInvited = 11264,
        PartyJoinAlreadyInParty = 11265,
        PartyJoinPartyFull = 11266,
        PartyJoinUnableJoinParty = 11267,
        //Party Kick
        PartyKickUnableGetUserRegistry = 11271,
        PartyKickNotInParty = 11272,
        PartyKickNotLeader = 11273,
        PartyKickKickeeNotInParty = 11274,
        PartyKickUnableKickUser = 11275,
        PartyKickKickOwnSelf = 11277,
        //Personal Chat
        PersonalChatUnableGetUserRegistry = 11281,
        PersonalChatSenderBanned = 11282,
        PersonalChatReceiverBanned = 11283,
        PersonalChatReceiverNotFound = 11284,
        PersonalChatUnableSaveChat = 11285,
        //Party Chat
        PartyChatUnableGetUserRegistry = 11291,
        PartyChatSenderBanned = 11292,
        PartyChatPartyNotFound = 11293,
        PartyChatUnableSaveChat = 11294,
        PartyChatReceiverBanned = 11295,
        PartyChatReceiverNotFound = 11296,
        //List Friends
        ListFriendsUnableGetUserRegistry = 11301,
        ListFriendsUnableGetOnlineFiends = 11303,
        ListFriendsUnableGetAllOnlineUsers = 11305,
        //Get All Notification
        ListNotificationUnableGetUserRegistry = 11311,
        ListNotificationUnableGetUserNotification = 11312,
        ListNotificationUnableDeleteUserNotification = 11314,
        //Set User Status
        SetUserStatusUnableSetStatus = 11331,
        SetUserStatusUnableGetUserRegistry = 11332,
        SetUserStatusUnableGetFriendsList = 11333,
        ResetUserStatusUnableGetFriendsList = 11334,
        ResetUserStatusUnableResetStatus = 11335,
        //List Friends Presence
        ListFriendsPresenceUnableGetUserRegistry = 11341,
        ListFriendsPresenceUnableGetOnlineFiends = 11343,
        ListFriendsPresenceUnableGetFriendsPresence = 11344,
        //Start Matchmaking
        StartMatchmakingUnableToPush = 11601,
        StartMatchmakingUnableToFindUserRegistry = 11602,
        StartMatchmakingUserNotInParty = 11603,
        StartMatchmakingUnableToFindUserParty = 11604,
        StartMatchmakingNotPartyLeader = 11605,
        StartMatchmakingUnableToStoreMMStatus = 11606,
        //Cancel Matchmaking
        CancelMatchmakingUnableToFindUserParty = 11611,
        CancelMatchmakingUnableToGetPartyInfo = 11612,
        CancelMatchmakingNotPartyLeader = 11613,
        CancelMatchmakingUnableToPushCancelRequest = 11614,
        CancelMatchmakingNotInParty = 11615,
        //Listen Matchmaking Result
        MatchmakingUnableToFindParty = 11623,
        MatchmakingPartyNotFound = 11624,
        MatchmakingUnablePrepareReadyConsent = 11627,
        MatchmakingUnableSaveMMResult = 11629,
        //Set Ready Consent
        SetReadyConsentUnableGetUserRegistry = 11631,
        SetReadyConsentNotFound = 11632,
        SetReadyConsentUnableSetReadyConsent = 11633,
        SetReadyConsentUnableGetMMResult = 11634,
        //Friend Request
        FriendRequestUnableRequestFriends = 11701,
        FriendRequestInvalidRequestBody = 11702,
        FriendRequestConflictFriendship = 11703,
        FriendRequestUnableGetUserRegistry = 11704,
        FriendRequestFriendRegistryNotFound = 11705,
        FriendRequestUnableGetFriendRegistry = 11706,
        //List Incoming Friends
        ListIncomingFriendsUnableGetListIncoming = 11711,
        ListIncomingFriendsUnableToWriteResponse = 11712,
        ListIncomingFriendsUnableGetUserRegistry = 11713,
        ListIncomingFriendsDataNotFound = 11714,
        ListIncomingFriendsInvalidRequestBody = 11715,
        //List Outgoing Friends
        ListOutgoingFriendsUnableGetListOutgoing = 11721,
        ListOutgoingFriendsUnableToWriteResponse = 11722,
        ListOutgoingFriendsUnableGetUserRegistry = 11723,
        ListOutgoingFriendsDataNotFound = 11724,
        ListOutgoingFriendsInvalidRequestBody = 11725,
        //Accept Friend
        AcceptFriendUnableAcceptFriends = 11731,
        AcceptFriendInvalidRequestBody = 11732,
        AcceptFriendDataNotFound = 11733,
        AcceptFriendUnableGetUserRegistry = 11734,
        AcceptFriendRequestorNotFound = 11735,
        AcceptFriendUnableGetRequestorRegistry = 11736,
        //Reject Friend
        RejectFriendUnableRejectFriends = 11741,
        RejectFriendInvalidRequestBody = 11742,
        RejectFriendDataNotFound = 11743,
        RejectFriendUnableGetUserRegistry = 11744,
        //Cancel Friend Request
        CancelFriendUnableCancelFriends = 11751,
        CancelFriendInvalidRequestBody = 11752,
        CancelFriendDataNotFound = 11753,
        CancelFriendUnableGetUserRegistry = 11754,
        //Unfriend
        UnfriendUnableUnfriend = 11761,
        UnfriendInvalidRequestBody = 11762,
        UnfriendDataNotFound = 11763,
        UnfriendUnableGetUserRegistry = 11764,
        //List of Friends
        ListOfFriendsUnableGetListOfFriends = 11771,
        ListOfFriendsUnableGetUserRegistry = 11773,
        //Get Friendship Status
        GetFriendshipStatusUnableGetFriendshipStatus = 11781,
        GetFriendshipStatusUnableGetUserRegistry = 11783,
        GetFriendshipStatusInvalidRequestBody = 11784,
        //Get Friends
        GetFriendsInvalidRequestBody = 11791,
        GetFriendsDataNotFound = 11792,
        GetFriendsUnableGetListOfFriends = 11793,
        GetFriendsUnableToWriteResponse = 11794,
        //Block/Unblock User
        PlayerBlockRequestInvalidRequestBody      = 11801,
        PlayerBlockRequestUnableGetUserRegistry   = 11802,
        PlayerBlockRequestUnableBlockPlayer       = 11803,
        PlayerUnblockRequestInvalidRequestBody    = 11804,
        PlayerUnblockRequestUnableGetUserRegistry = 11805,
        PlayerUnblockRequestUnableBlockPlayer     = 11806,
        PlayerBlocked                             = 11807,
        PlayerBlockedUnableGet                    = 11808,

        //AccelByte CloudSave error codes
        GameRecordNotFound = 18003,
        PlayerRecordNotFound = 18022,
        GameRecordPreconditionFailed = 18056,
        PlayerRecordPreconditionFailed = 18103,
        
        //AccelByte DSM error codes
        DedicatedServerNotFound = 9014183
    }

    public class Error
    {
        public readonly ErrorCode Code;
        public readonly string Message;
        public readonly object messageVariables;
        public readonly Error InnerError;

        public Error(ErrorCode code, string message = null, object messageVariables = null, Error innerError = null)
        {
            this.Code = code;
            this.Message = string.IsNullOrEmpty(message) ? GetDefaultErrorMessage() : message;
            this.InnerError = innerError;
            this.messageVariables = messageVariables;
        }

        public Error WrapWith(ErrorCode code, string message = null, object messageVariables = null) { return new Error(code, message, messageVariables, this); }

        private string GetDefaultErrorMessage()
        {
            switch (this.Code)
            {
            case ErrorCode.None:

                return "This error code doesn't make sense and should not happen at all.";

            //HTTP Status Codes
            case ErrorCode.BadRequest:

                return "The request could not be understood by the server due to malformed syntax.";
            case ErrorCode.Unauthorized:

                return "The request requires user authentication.";
            case ErrorCode.PaymentRequired:

                return "The request requires a payment.";
            case ErrorCode.Forbidden:

                return "The server understood the request, but is refusing to fulfill it.";
            case ErrorCode.NotFound:

                return "The server has not found anything matching the Request-URI.";
            case ErrorCode.MethodNotAllowed:

                return "The method specified in the Request-Line is not allowed for the resource identified by the " +
                    "Request-URI.";
            case ErrorCode.NotAcceptable:

                return "The resource identified by the request can not generate content according to the accept " +
                    "headers sent in the request.";
            case ErrorCode.ProxyAuthenticationRequired:

                return "The request requires user authentication via proxy.";
            case ErrorCode.RequestTimeout:

                return "The client did not produce a request within the time that the server was prepared to wait.";
            case ErrorCode.Conflict:

                return "The request could not be completed due to a conflict with the current state of the resource.";
            case ErrorCode.Gone:

                return "The requested resource is no longer available at the server and no forwarding address is " +
                    "known.";
            case ErrorCode.LengthRequired:

                return "The server refuses to accept the request without a defined Content-Length.";
            case ErrorCode.PreconditionFailed:

                return "The precondition given in one or more of the request-header fields evaluated to false when " +
                    "it was tested on the server.";
            case ErrorCode.RequestEntityTooLarge:

                return "The request entity is larger than the server is willing or able to process.";
            case ErrorCode.RequestUriTooLong:

                return "The Request-URI is longer than the server is willing to interpret.";
            case ErrorCode.UnsupportedMediaType:

                return "The entity of the request is in a format not supported by the requested resource for the " +
                    "requested method.";
            case ErrorCode.RequestedRangeNotSatisfiable:

                return "The request included a Range request-header field but none of the range-specifier values in " +
                    "this field overlap the current extent of the selected resource, and the request did not include" +
                    " an If-Range request-header field.";
            case ErrorCode.ExpectationFailed:

                return "The expectation given in an Expect request-header field could not be met by this server.";
            case ErrorCode.UnprocessableEntity:

                return "Entity can not be processed.";
            case ErrorCode.InternalServerError:

                return "Unexpected condition encountered which prevented the server from fulfilling the request.";
            case ErrorCode.NotImplemented:

                return "The server does not support the functionality required to fulfill the request.";
            case ErrorCode.BadGateway:

                return "The gateway or proxy received an invalid response from the upstream server.";
            case ErrorCode.ServiceUnavailable:

                return "The server is currently unable to handle the request due to a temporary overloading or " +
                    "maintenance of the server.";
            case ErrorCode.GatewayTimeout:

                return "The gateway or proxy, did not receive a timely response from the upstream server.";
            case ErrorCode.HttpVersionNotSupported:

                return "The server does not support the HTTP protocol version that was used in the request message.";

            //Client side error codes
            case ErrorCode.IsNotLoggedIn:

                return "User is not logged in.";
            case ErrorCode.NetworkError:

                return "There is no response.";
            case ErrorCode.MessageFieldTypeNotSupported:

                return "Serialization for expected field type is not supported.";
            case ErrorCode.MessageFormatInvalid:

                return "Message is not well formed.";
            case ErrorCode.MessageFieldDoesNotExist:

                return "Expected message field cannot be found.";
            case ErrorCode.MessageFieldConversionFailed:

                return "Message field value cannot be converted to expected field type.";
            case ErrorCode.MessageCannotBeSent:

                return "Sending message to server failed.";
            default:

                return "Unknown error: " + this.Code.ToString("G");
            }
        }
    }

    public interface IResult
    {
        Error Error { get; }
        bool IsError { get; }
    }

    public class Result<T> : IResult
    {
        public Error Error { get; private set; }
        public T Value { get; private set; }

        public bool IsError { get { return this.Error != null; } }

        public static Result<T> CreateOk(T value) { return new Result<T>(null, value); }

        public static Result<T> CreateError(ErrorCode errorCode, string errorMessage = null, object messageVariables = null)
        {
            return new Result<T>(new Error(errorCode, errorMessage, messageVariables), default(T));
        }

        public static Result<T> CreateError(Error error) { return new Result<T>(error, default(T)); }

        private Result(Error error, T value)
        {
            this.Error = error;
            this.Value = value;
        }
    }

    public class Result : IResult
    {
        public Error Error { get; private set; }

        public bool IsError { get { return this.Error != null; } }

        public static Result CreateOk() { return new Result(null); }

        public static Result CreateError(ErrorCode errorCode, string errorMessage = null, object messageVariables = null)
        {
            return new Result(new Error(errorCode, errorMessage, messageVariables));
        }

        public static Result CreateError(Error error) { return new Result(error); }

        private Result(Error error) { this.Error = error; }
    }
}
