// Copyright (c) 2018 - 2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Net;
using AccelByte.Models;

namespace AccelByte.Core
{
    public static class HttpResponseExtension
    {
        public static Result TryParse(this IHttpResponse response)
        {
            if (response == null)
            {
                return Result.CreateError(ErrorCode.NetworkError);
            }

            switch (response.Code)
            {
            case (long) HttpStatusCode.OK:
            case (long) HttpStatusCode.Created:
            case (long) HttpStatusCode.Accepted:
            case (long) HttpStatusCode.NoContent:
            case (long) HttpStatusCode.Ambiguous:
            case (long) HttpStatusCode.Moved:
            case (long) HttpStatusCode.Redirect:
            case (long) HttpStatusCode.SeeOther:
            case (long) HttpStatusCode.NotModified:
            case (long) HttpStatusCode.UseProxy:
            case (long) HttpStatusCode.Unused:
            case (long) HttpStatusCode.RedirectKeepVerb: return Result.CreateOk();

            case (long) HttpStatusCode.BadRequest:
            case (long) HttpStatusCode.Unauthorized:
            case (long) HttpStatusCode.PaymentRequired:
            case (long) HttpStatusCode.Forbidden:
            case (long) HttpStatusCode.NotFound:
            case (long) HttpStatusCode.MethodNotAllowed:
            case (long) HttpStatusCode.NotAcceptable:
            case (long) HttpStatusCode.ProxyAuthenticationRequired:
            case (long) HttpStatusCode.RequestTimeout:
            case (long) HttpStatusCode.Conflict:
            case (long) HttpStatusCode.Gone:
            case (long) HttpStatusCode.LengthRequired:
            case (long) HttpStatusCode.PreconditionFailed:
            case (long) HttpStatusCode.RequestEntityTooLarge:
            case (long) HttpStatusCode.RequestUriTooLong:
            case (long) HttpStatusCode.UnsupportedMediaType:
            case (long) HttpStatusCode.RequestedRangeNotSatisfiable:
            case (long) HttpStatusCode.ExpectationFailed:

                if (response.BodyBytes == null)
                {
                    return Result.CreateError((ErrorCode) response.Code);
                }

                try
                {
                    var error = response.BodyBytes.ToObject<ServiceError>();

                    if (error.numericErrorCode == 0)
                    {
                        if (error.errorCode == 0)
                        {
                            return Result.CreateError((ErrorCode)response.Code);
                        }

                        return Result.CreateError((ErrorCode)error.errorCode, error.errorMessage);
                    }

                    return Result.CreateError((ErrorCode) error.numericErrorCode, error.errorMessage);
                }
                catch (Exception)
                {
                    // ignored
                }

                try
                {
                    var err = response.BodyBytes.ToObject<OAuthError>();
                    string message = err.error + ": " + err.error_description;

                    return Result.CreateError((ErrorCode) response.Code, message);
                }
                catch (Exception)
                {
                    // ignored
                }

                return Result.CreateError((ErrorCode) response.Code);

            default:
                string body = System.Text.Encoding.UTF8.GetString(response.BodyBytes);

                if (!string.IsNullOrEmpty(body))
                {
                    return Result.CreateError((ErrorCode) response.Code, "Unknown Service Error: " + body);
                }
                else
                {
                    return Result.CreateError((ErrorCode) response.Code);
                }
            }
        }

        public static Result<T> TryParseJson<T>(this IHttpResponse response)
        {
            if (response == null)
            {
                return Result<T>.CreateError(ErrorCode.NetworkError, "There is no response.");
            }

            string message;
            string responseText = System.Text.Encoding.UTF8.GetString(response.BodyBytes);

            switch (response.Code)
            {
            case (long) HttpStatusCode.OK:
            case (long) HttpStatusCode.Created:

                try
                {
                    var createResponse = responseText.ToObject<T>();

                    return Result<T>.CreateOk(createResponse);
                }
                catch (Exception ex)
                {
                    return Result<T>.CreateError(ErrorCode.ErrorFromException, ex.Message);
                }

            case (long) HttpStatusCode.Accepted:
            case (long) HttpStatusCode.NoContent:
            case (long) HttpStatusCode.ResetContent:
            case (long) HttpStatusCode.PartialContent:
            case (long) HttpStatusCode.Ambiguous:
            case (long) HttpStatusCode.Moved:
            case (long) HttpStatusCode.Redirect:
            case (long) HttpStatusCode.SeeOther:
            case (long) HttpStatusCode.NotModified:
            case (long) HttpStatusCode.UseProxy:
            case (long) HttpStatusCode.Unused:
            case (long) HttpStatusCode.RedirectKeepVerb:
                message = "JSON response body expected but instead found HTTP Response with Status " + response.Code;

                return Result<T>.CreateError(ErrorCode.InvalidResponse, message);

            case (long) HttpStatusCode.BadRequest:
            case (long) HttpStatusCode.Unauthorized:
            case (long) HttpStatusCode.PaymentRequired:
            case (long) HttpStatusCode.Forbidden:
            case (long) HttpStatusCode.NotFound:
            case (long) HttpStatusCode.MethodNotAllowed:
            case (long) HttpStatusCode.NotAcceptable:
            case (long) HttpStatusCode.ProxyAuthenticationRequired:
            case (long) HttpStatusCode.RequestTimeout:
            case (long) HttpStatusCode.Conflict:
            case (long) HttpStatusCode.Gone:
            case (long) HttpStatusCode.LengthRequired:
            case (long) HttpStatusCode.PreconditionFailed:
            case (long) HttpStatusCode.RequestEntityTooLarge:
            case (long) HttpStatusCode.RequestUriTooLong:
            case (long) HttpStatusCode.UnsupportedMediaType:
            case (long) HttpStatusCode.RequestedRangeNotSatisfiable:
            case (long) HttpStatusCode.ExpectationFailed:

                if (string.IsNullOrEmpty(responseText))
                {
                    return Result<T>.CreateError((ErrorCode) response.Code);
                }

                try
                {
                    var error = responseText.ToObject<ServiceError>();

                    if (error.numericErrorCode == 0)
                    {
                        if (error.errorCode == 0)
                        {
                            return Result<T>.CreateError((ErrorCode)response.Code);
                        }
                        return Result<T>.CreateError((ErrorCode)error.errorCode, error.errorMessage);
                    }

                    return Result<T>.CreateError((ErrorCode) error.numericErrorCode, error.errorMessage);
                }
                catch (Exception)
                {
                    // ignored
                }

                try
                {
                    var err = responseText.ToObject<OAuthError>();
                    message = err.error + ": " + err.error_description;

                    return Result<T>.CreateError((ErrorCode) response.Code, message);
                }
                catch (Exception)
                {
                    // ignored
                }

                return Result<T>.CreateError((ErrorCode) response.Code);

            default:

                if (!string.IsNullOrEmpty(responseText))
                {
                    return Result<T>.CreateError((ErrorCode) response.Code, "Unknown Service Error: " + responseText);
                }
                else
                {
                    return Result<T>.CreateError((ErrorCode) response.Code);
                }
            }
        }
    }
}