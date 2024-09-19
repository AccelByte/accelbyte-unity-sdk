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
        public static Result TryParse(this IHttpResponse response)
        {
            Result retval;
            Error error = ParseError(response);
            if (error != null)
            {
                retval = Result.CreateError(error);
            }
            else
            {
#if ACCELBYTE_STRONG_RESPONSE_VALIDATION
                bool bodyIsEmpty = response.BodyBytes == null || response.BodyBytes.Length == 0;
                if (!bodyIsEmpty)
                {
                    UnityEngine.Debug.LogError("Unexpected response body isn't null");
                }
#endif
                retval = Result.CreateOk();
            }
            return retval;
        }

        public static Result<T> TryParseJson<T>(this IHttpResponse response)
        {
            Result<T> retval;
            Error error = ParseError(response);

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
#if ACCELBYTE_STRONG_RESPONSE_VALIDATION
                        ValidateResponseType<T>(response);
#endif
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
#if ACCELBYTE_STRONG_RESPONSE_VALIDATION
                        if (!bodyResponseNullOrEmpty)
                        {
                            ValidateResponseType<U>(response);
                        }
#endif
                        retval = bodyResponseNullOrEmpty ? Result<T, U>.CreateError(new U()) :
                            Result<T, U>.CreateError(response.BodyBytes.ToObject<U>());
                    }
                    else
                    {
#if ACCELBYTE_STRONG_RESPONSE_VALIDATION
                        if (!bodyResponseNullOrEmpty)
                        {
                            ValidateResponseType<T>(response);
                        }
#endif
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

        private static Error ParseServiceError(IHttpResponse response)
        {
#if ACCELBYTE_STRONG_RESPONSE_VALIDATION
            ValidateResponseType<ServiceError>(response);
#endif
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

#if ACCELBYTE_STRONG_RESPONSE_VALIDATION
        private static void ValidateResponseType<ModelT>(IHttpResponse response)
        {
            Type modelType = typeof(ModelT);
            string modelName = modelType.Name;
            UnityEngine.Assertions.Assert.IsNotNull(response);
            UnityEngine.Assertions.Assert.IsNotNull(response.BodyBytes);
            if(typeof(System.Collections.IEnumerable).IsAssignableFrom(modelType))
            {
                // Array type isn't supported yet
                return;
            }

            Newtonsoft.Json.Linq.JObject modelJObject = null;
            try
            {
                ModelT model = Activator.CreateInstance<ModelT>();
                string modelJson = model.ToJsonString(); 
                modelJObject = Newtonsoft.Json.Linq.JObject.Parse(modelJson);
            }
            catch (Exception ex)
            {
                string error = $"Failed converting {modelName}\n{ex.Message}";
                UnityEngine.Debug.LogError(error);
            }
                
            string responseBodyJson = System.Text.Encoding.UTF8.GetString( response.BodyBytes ); 
            var responseBodyJObject = Newtonsoft.Json.Linq.JObject.Parse(responseBodyJson);
            var responseBodyDictObj = responseBodyJObject.ToObject<System.Collections.Generic.Dictionary<string, object>>();

            var missingFields = new System.Collections.Generic.List<string>();
            foreach (System.Collections.Generic.KeyValuePair<string, object> keyValuePair in responseBodyDictObj)
            {
                if (!modelJObject.ContainsKey(keyValuePair.Key))
                {
                    missingFields.Add(keyValuePair.Key);
                }
            }

            if (missingFields.Count > 0)
            {
                string error = $"Missing fields from {modelName} : {string.Join(",", missingFields)}";
                UnityEngine.Debug.LogWarning(error);
            }
        }
#endif
    }
}