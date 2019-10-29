// Copyright (c) 2018 - 2019 AccelByte Inc. All Rights Reserved.
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

        //AccelByte Launcher error codes (20 - 99)

        //AccelByte E-Commerce error codes (1000 - 5000)
        OptimisticLock = 2006,
        NamespaceNotFound = 2141,
        UserProfileNotFound = 2241,
        CategoryNotFound = 30241,
        ItemNotFound = 30341,
        StoreNotFound = 3043,
        OrderPriceMismatch = 3221,
        OrderNotFound = 3241,
        OrderInvalidStatus = 3272,
        WalletExceedMaxTransactionAmountPerDay = 3521,
        WalletExceedMaxAmountPerTransaction = 3522,
        WalletInactive = 3523,
        WalletExceedMaxBalanceAmount = 3525,
        UserWalletDoesNotExist = 3543,
        WalletAlreadyExist = 3571,
        PublisherStoreNotExist = 30142,

        //AccelByte Statistic error codes
        StatisticNotFound = 12241,

        //Client side error codes
        GeneralClientError = 14000,
        ErrorFromException = 14001,
        InvalidArgument = 14002,
        InvalidRequest = 14003,
        InvalidResponse = 14004,
        NetworkError = 14005,
        IsNotLoggedIn = 14006,
        UserProfileIsNotCreated = 14007,
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
        
        UnauthorizedAccess = 20001,
        ValidationError = 20002,
        TooManyRequests = 20001,
        UserNotFound = 20008,
        TokenIsNotUserToken = 20022,
        UnableToParseRequestBody = 20019,
        
    }

    public class Error
    {
        public readonly ErrorCode Code;
        public readonly string Message;
        public readonly Error InnerError;

        public Error(ErrorCode code, string message = null, Error innerError = null)
        {
            this.Code = code;
            this.Message = string.IsNullOrEmpty(message) ? GetDefaultErrorMessage() : message;
            this.InnerError = innerError;
        }

        public Error WrapWith(ErrorCode code, string message = null) { return new Error(code, message, this); }

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

        public static Result<T> CreateError(ErrorCode errorCode, string errorMessage = null)
        {
            return new Result<T>(new Error(errorCode, errorMessage), default(T));
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

        public static Result CreateError(ErrorCode errorCode, string errorMessage = null)
        {
            return new Result(new Error(errorCode, errorMessage));
        }

        public static Result CreateError(Error error) { return new Result(error); }

        private Result(Error error) { this.Error = error; }
    }
}