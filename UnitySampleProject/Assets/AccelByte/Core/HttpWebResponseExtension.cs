// Copyright (c) 2018 - 2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using AccelByte.Models;

namespace AccelByte.Core
{
    public static class HttpWebResponseExtension
    {
        public static string GetBodyText(this HttpWebResponse response)
        {
            using (var stream = response.GetResponseStream())
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }        
        }

        public static Result TryParse(this HttpWebResponse response)
        {
            if (response == null)
            {
                return Result.CreateError(ErrorCode.NetworkError, "There is no response");
            }

            response.Close();

            switch (response.StatusCode)
            {
            case HttpStatusCode.OK:
            case HttpStatusCode.Created:
            case HttpStatusCode.Accepted:
            case HttpStatusCode.NoContent:
            case HttpStatusCode.Ambiguous:
            case HttpStatusCode.Moved:
            case HttpStatusCode.Redirect:
            case HttpStatusCode.SeeOther:
            case HttpStatusCode.NotModified:
            case HttpStatusCode.UseProxy:
            case HttpStatusCode.Unused:
            case HttpStatusCode.RedirectKeepVerb:
                return Result.CreateOk();
            default:
                return Result.CreateError((ErrorCode) response.StatusCode);
            }
        }

        [DataContract]
        private class ServiceError
        {
            [DataMember] public int numericErrorCode { get; set; }
            [DataMember] public string errorMessage { get; set; }
        }

        public static Result<T> TryParseJsonBody<T>(this HttpWebResponse response)
        {
            if (response == null)
            {
                return Result<T>.CreateError(ErrorCode.NetworkError, "There is no response.");
            }

            var responseText = response.GetBodyText();
            response.Close();

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                case HttpStatusCode.Created:
                    try
                    {
                        var createResponse = SimpleJson.SimpleJson.DeserializeObject<T>(responseText);
                        return Result<T>.CreateOk(createResponse);
                    }
                    catch (Exception ex)
                    {
                        return Result<T>.CreateError(ErrorCode.ErrorFromException, ex.Message);                    
                    }
                case HttpStatusCode.Accepted:
                case HttpStatusCode.NoContent:
                case HttpStatusCode.ResetContent:
                case HttpStatusCode.PartialContent:
                case HttpStatusCode.Ambiguous:
                case HttpStatusCode.Moved:
                case HttpStatusCode.Redirect:
                case HttpStatusCode.SeeOther:
                case HttpStatusCode.NotModified:
                case HttpStatusCode.UseProxy:
                case HttpStatusCode.Unused:
                case HttpStatusCode.RedirectKeepVerb:
                    return Result<T>.CreateError(ErrorCode.InvalidResponse, "JSON response body expected but instead " +
                        "found HTTP Response with Status " + response.StatusCode);
                case HttpStatusCode.BadRequest:
                case HttpStatusCode.Unauthorized:
                case HttpStatusCode.PaymentRequired:
                case HttpStatusCode.Forbidden:
                case HttpStatusCode.NotFound:
                case HttpStatusCode.MethodNotAllowed:
                case HttpStatusCode.NotAcceptable:
                case HttpStatusCode.ProxyAuthenticationRequired:
                case HttpStatusCode.RequestTimeout:
                case HttpStatusCode.Conflict:
                case HttpStatusCode.Gone:
                case HttpStatusCode.LengthRequired:
                case HttpStatusCode.PreconditionFailed:
                case HttpStatusCode.RequestEntityTooLarge:
                case HttpStatusCode.RequestUriTooLong:
                case HttpStatusCode.UnsupportedMediaType:
                case HttpStatusCode.RequestedRangeNotSatisfiable:
                case HttpStatusCode.ExpectationFailed:
                    if (string.IsNullOrEmpty(responseText))
                    {
                        return Result<T>.CreateError((ErrorCode)response.StatusCode);
                    }

                    try
                    {
                        var error = SimpleJson.SimpleJson.DeserializeObject<ServiceError>(responseText);
                        
                        if (error.numericErrorCode == 0)
                        {
                            return Result<T>.CreateError((ErrorCode)response.StatusCode);
                        }

                        return Result<T>.CreateError((ErrorCode) error.numericErrorCode, error.errorMessage);
                    }
                    catch (Exception ex)
                    {
                        return Result<T>.CreateError((ErrorCode)response.StatusCode);
                    }
                default:
                    if (!string.IsNullOrEmpty(responseText))
                    {
                        return Result<T>.CreateError((ErrorCode)response.StatusCode, "Unknown Service Error: " +
                            responseText);
                    }
                    else
                    {
                        return Result<T>.CreateError((ErrorCode)response.StatusCode);
                    }
            }
        }

        public static byte[] GetBodyRaw(this HttpWebResponse response)
        {
            using (var stream = response.GetResponseStream())
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    byte[] buffer = new byte[1024];
                    int read;
                    
                    while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        memoryStream.Write(buffer, 0, read);
                    }
                    
                    memoryStream.Flush();
                    
                    return memoryStream.ToArray();
                }
            }
        }
    }
}