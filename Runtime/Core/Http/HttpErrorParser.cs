// Copyright (c) 2021 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Net;
using AccelByte.Models;

namespace AccelByte.Core
{
    public static class HttpErrorParser
    {
        internal static string NoResponseMessage = "There is no response.";
        internal static System.Collections.Generic.List<IAccelByteResponseValidator> ResponseValidators = null;
        
        public static Result TryParse(this IHttpResponse response)
        {
            Result retval;
            ExecuteResponseValidators(request: null, response);
            
            Error error = ParseError(response);
            if (error != null)
            {
                retval = Result.CreateError(error);
            }
            else
            {
                retval = Result.CreateOk();
            }
            return retval;
        }

        public static Result<T> TryParseJson<T>(this IHttpResponse response)
        {
            Result<T> retval;
            Error error = ParseError(response);
            ExecuteResponseValidators<T>(request: null, response);

            if (error != null)
            {
                retval = Result<T>.CreateError(error);
            }
            else
            {
                try
                {
                    if (response.BodyBytes == null || response.BodyBytes.Length == 0)
                    {
                        retval = Result<T>.CreateOk(default(T));
                    }
                    else
                    {
                        retval = Result<T>.CreateOk(response.BodyBytes.ToObject<T>());
                    }
                }
                catch (Exception e)
                {
                    retval = Result<T>.CreateError(ErrorCode.ErrorFromException, e.Message);
                }
            }
            return retval;
        }

        public static Result<T, U> TryParseJson<T, U>(this IHttpResponse response) where U : new()
        {
            Result<T, U> retval = null;
            ExecuteResponseValidators<T>(request: null, response);
            
            if (response == null)
            {
                retval = Result<T, U>.CreateError(new U());
            }
            else
            {
                bool errorResponse = false;
                bool successResponseCode = response.Code >= 200 && response.Code < 300;
                if (successResponseCode == false)
                {
                    errorResponse = true;
                }

                bool bodyResponseNullOrEmpty = response.BodyBytes == null || response.BodyBytes.Length == 0;
                try
                {
                    if (errorResponse)
                    {
                        retval = bodyResponseNullOrEmpty ? Result<T, U>.CreateError(new U()) :
                            Result<T, U>.CreateError(response.BodyBytes.ToObject<U>());
                    }
                    else
                    {
                        retval = bodyResponseNullOrEmpty ? Result<T, U>.CreateOk() : Result<T, U>.CreateOk(response.BodyBytes.ToObject<T>());
                    }
                }
                catch (Exception)
                {
                    retval = Result<T, U>.CreateError(new U());
                }
            }
            return retval;
        }

        private static OAuthError ParseOAuthError(IHttpResponse response)
        { 
            try
            {
                return response.BodyBytes.ToObject<OAuthError>();
            }
            catch (Exception ex)
            {
                return new OAuthError() {
                    error = "Exception Error",
                    error_description = ex.Message 
                };
            }

        }

        internal static Error ParseError(IHttpResponse response)
        {
            if (response == null)
            {
                return new Error(ErrorCode.NetworkError, NoResponseMessage);
            }
            if (response.Code >= 200 && response.Code < 300)
            {
                return null;
            }

            if (response.Code < 400 || response.Code >= 600)
            {
                return HttpErrorParser.ParseDefaultError(response);
            }

            if (response.BodyBytes == null)
            {
                return new Error((ErrorCode)response.Code);
            }

            try
            {
                return HttpErrorParser.ParseServiceError(response);
            }
            catch (Exception)
            {
                return new Error((ErrorCode)response.Code);
            }
        }

        internal static bool IsHasServerError(IHttpResponse response)
        {
            if(response == null)
            {
                return false;
            }
            else
            {
                switch ((HttpStatusCode)response.Code)
                {
                    case HttpStatusCode.InternalServerError:
                    case HttpStatusCode.BadGateway:
                    case HttpStatusCode.ServiceUnavailable:
                    case HttpStatusCode.GatewayTimeout:
                        return true;
                }
                return false;
            }
        }

        internal static bool IsInternalErrorRetriable(IHttpResponse response)
        {
            var error = ParseError(response);
            if (error == null)
            {
                return false;
            }

            switch (error.Code)
            {
                case ErrorCode.SessionVersionMismatch:
                case ErrorCode.AchievementTooManyRequests:
                    return true;
            }

            return false;
        }

        private static Error ParseServiceError(IHttpResponse response)
        {
            var error = response.BodyBytes.ToObject<ServiceError>();

            Error retval;
            if (error.numericErrorCode != 0)
            {
                retval = new Error((ErrorCode)error.numericErrorCode, error.errorMessage, error.messageVariables);
            }
            else if (error.errorCode != 0)
            {
                retval = new Error((ErrorCode)error.errorCode, error.errorMessage, error.messageVariables);
            }
            else if (error.code != 0)
            {
                retval = new Error((ErrorCode)error.code, error.message);
            }
            else if (error.error != null)
            {
                string message = error.error;

                if (error.error_description != null)
                {
                    message += ": " + error.error_description;
                }
                retval = new Error((ErrorCode)response.Code, message);
            }
            else
            {
                retval = new Error((ErrorCode)response.Code);
            }

            return retval;
        }

        private static Error ParseDefaultError(IHttpResponse response)
        {
            Error retval;
            if (response.BodyBytes == null)
            {
                retval = new Error((ErrorCode)response.Code);
            }
            else
            {
                string body = System.Text.Encoding.UTF8.GetString(response.BodyBytes);
                retval = new Error((ErrorCode)response.Code, "Unknown error: " + body);
            }
            return retval;
        }

        private static void ExecuteResponseValidators(IHttpRequest request, IHttpResponse response)
        {
            if (ResponseValidators != null)
            {
                foreach (IAccelByteResponseValidator responseValidator in ResponseValidators)
                {
                    responseValidator.Validate(request, response);
                }
            }
        }

        private static void ExecuteResponseValidators<T>(IHttpRequest request, IHttpResponse response)
        {
            if (ResponseValidators != null)
            {
                foreach (IAccelByteResponseValidator responseValidator in ResponseValidators)
                {
                    responseValidator.Validate<T>(request, response);
                }
            }
        }
    }
}