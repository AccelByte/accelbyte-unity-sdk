// Copyright (c) 2021 - 2025 AccelByte Inc. All Rights Reserved.
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
                    ResultErrorOptionalParameters optionalParameters = new ResultErrorOptionalParameters();
                    optionalParameters.MessageVariables = e.Message;
                    optionalParameters.HttpResponse = response;

                    retval = Result<T>.CreateError(ErrorCode.ErrorFromException, optionalParameters);
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
                ResultErrorOptionalParameters optionalParameters = new ResultErrorOptionalParameters();
                optionalParameters.Message = NoResponseMessage;

                return new Error(ErrorCode.NetworkError, optionalParameters);
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
                ResultErrorOptionalParameters optionalParameters = new ResultErrorOptionalParameters();
                optionalParameters.HttpResponse = response;

                return new Error((ErrorCode)response.Code, optionalParameters);
            }

            try
            {
                return HttpErrorParser.ParseServiceError(response);
            }
            catch (Exception)
            {
                ResultErrorOptionalParameters optionalParameters = new ResultErrorOptionalParameters();
                optionalParameters.HttpResponse = response;

                return new Error((ErrorCode)response.Code, optionalParameters);
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
                case ErrorCode.HttpTooManyRequests:
                case ErrorCode.RetryWith:
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
            ResultErrorOptionalParameters optionalParameters = new ResultErrorOptionalParameters();
            optionalParameters.HttpResponse = response;

            if (error.numericErrorCode != 0)
            {
                optionalParameters.Message = error.errorMessage;
                optionalParameters.MessageVariables = error.messageVariables;

                retval = new Error((ErrorCode)error.numericErrorCode, optionalParameters);
            }
            else if (error.errorCode != 0)
            {
                optionalParameters.Message = error.errorMessage;
                optionalParameters.MessageVariables = error.messageVariables;

                retval = new Error((ErrorCode)error.errorCode, optionalParameters);
            }
            else if (error.code != 0)
            {
                optionalParameters.Message = error.message;

                retval = new Error((ErrorCode)error.code, optionalParameters);
            }
            else if (error.error != null)
            {
                string message = error.error;

                if (error.error_description != null)
                {
                    message += ": " + error.error_description;
                }

                optionalParameters.Message = message;

                retval = new Error((ErrorCode)response.Code, optionalParameters);
            }
            else
            {
                retval = new Error((ErrorCode)response.Code, optionalParameters);
            }

            return retval;
        }

        private static Error ParseDefaultError(IHttpResponse response)
        {
            Error retval;
            ResultErrorOptionalParameters optionalParameters = new ResultErrorOptionalParameters();
            optionalParameters.HttpResponse = response;

            if (response.BodyBytes == null)
            {
                retval = new Error((ErrorCode)response.Code, optionalParameters);
            }
            else
            {
                string body = System.Text.Encoding.UTF8.GetString(response.BodyBytes);

                optionalParameters.Message = "Unknown error: " + body;

                retval = new Error((ErrorCode)response.Code, optionalParameters);
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