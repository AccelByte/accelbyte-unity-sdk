// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

namespace AccelByte.Core 
{
    /// <summary>
	/// https://www.rfc-editor.org/rfc/rfc6455#section-7.4.1
    /// </summary>
    public enum WsCloseCode
    {
        /* Do NOT use NotSet - it's only purpose is to indicate that the close code cannot be parsed. */
        NotSet = 0,
        Normal = 1000,
        Away = 1001,
        ProtocolError = 1002,
        UnsupportedData = 1003,
        Undefined = 1004,
        NoStatus = 1005,
        Abnormal = 1006,
        InvalidData = 1007,
        PolicyViolation = 1008,
        TooBig = 1009,
        MandatoryExtension = 1010,
        ServerError = 1011,
        ServiceRestart = 1012,
        TryAgainLater = 1013,
        TlsHandshakeFailure = 1015,

        //RESERVED 0-999
        //Status codes in the range 0 - 999 are not used.

        //RESERVED 1000-2999
        //Status codes in the range 1000 - 2999 are reserved for definition by
        //this protocol, its future revisions, and extensions specified in a
        //permanentand readily available public specification.

        // RESERVED 3000-3999=
        DisconnectSenderBrokenPipe = 3101,
        DisconnectSenderUnhandledError = 3102,
        DisconnectSenderUnableToPing = 3103,
        DisconnectReaderIOTimeout = 3120,
        DisconnectReaderUnexpectedEOF = 3121,
        DisconnectReaderUnhandledCloseError = 3122,

        // RESERVED 4000-4999=
        ServerShuttingDown = 4000,
        ServerDisconnected = 4003,
        DisconnectTokenRevokedCode = 4020,
        DisconnectDueToIAMLoggedOut = 4021,
        DisconnectDueToIAMDisconnect = 4022,
        DisconnectDueToFastReconnect = 4040,
        DisconnectDueToMultipleSessions = 4041,
        DisconnectFromExternalReconnect = 4042,
        DisconnectDueCCULimiter = 4060,
        DisconnectDelayedLost = 4098,
        DisconnectUnknownError = 4099,
        OutsideBoundaryOfDisconnection = 4400,

        //WILL BE DEPRECATED
        //Translated & split into 3101-3103
        DisconnectSenderError = 4004,
        //WILL BE DEPRECATED
        //Translated & split into 3120-3121
        DisconnectReaderClosed = 4005,
        //WILL BE DEPRECATED
        //Translated into the 3122
        DisconnectReaderClosedAbnormal = 4006,
        //WILL BE DEPRECATED
        DisconnectReaderError = 4007
    }
}