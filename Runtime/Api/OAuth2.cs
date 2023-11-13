// Copyright (c) 2018 - 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System;
using UnityEngine;
using UnityEngine.Assertions;
using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Api;

public class OAuth2 : ApiBase
{
    public OAuth2(IHttpClient httpClient,
           Config config,
           ISession session)
           : this(httpClient, config, session, null)
    {
    }

    public OAuth2(IHttpClient httpClient,
        Config config,
        ISession session,
        HttpOperator httpOperator)
        : base(httpClient, config, config.IamServerUrl, session, httpOperator)
    {
        OnNewTokenObtained = (tokenData) =>
        {
            session.SetSession(tokenData);
        };
        session.CallRefresh = RefreshSession;
    }

    //Need to be assigned to handle new access token obtained
    internal Action<TokenData> OnNewTokenObtained = null;

    [Obsolete("This end point is going to be deprected, use LoginWithUsernameV3 instead")]
    public void LoginWithUsername
        (string username
        , string password
        , ResultCallback callback
        , bool rememberMe = false)
    {
        Report.GetFunctionLog(GetType().Name);
        Assert.IsNotNull(username, "Username parameter is null.");
        Assert.IsNotNull(password, "Password parameter is null.");

        var request = HttpRequestBuilder.CreatePost(BaseUrl + "/oauth/token")
            .WithBasicAuth()
            .WithContentType(MediaType.ApplicationForm)
            .Accepts(MediaType.ApplicationJson)
            .WithFormParam("grant_type", "password")
            .WithFormParam("username", username)
            .WithFormParam("password", password)
            .WithFormParam("namespace", Namespace_)
            .WithFormParam("extend_exp", rememberMe ? "true" : "false")
            .AddAdditionalData("flightId", AccelByteSDK.FlightId)
            .GetResult();

        httpOperator.SendRequest(request, response =>
        {
            var result = response.TryParseJson<TokenData>();

            if (!result.IsError)
            {
                OnNewTokenObtained?.Invoke(result.Value);
                callback.TryOk();
            }
            else
            {
                callback.TryError(result.Error);
            }
        });
    }

    [Obsolete("This end point is going to be deprected, use LoginWithUsernameV3 instead")]
    public void LoginWithUsername
        (string username
        , string password
        , ResultCallback<TokenData, OAuthError> callback
        , bool rememberMe = false)
    {
        Report.GetFunctionLog(GetType().Name);
        Assert.IsNotNull(username, "Username parameter is null.");
        Assert.IsNotNull(password, "Password parameter is null.");

        var request = HttpRequestBuilder.CreatePost(BaseUrl + "/oauth/token")
            .WithBasicAuthWithCookie(Config.PublisherNamespace)
            .WithContentType(MediaType.ApplicationForm)
            .Accepts(MediaType.ApplicationJson)
            .WithFormParam("grant_type", "password")
            .WithFormParam("username", username)
            .WithFormParam("password", password)
            .WithFormParam("namespace", Namespace_)
            .WithFormParam("extend_exp", rememberMe ? "true" : "false")
            .AddAdditionalData("flightId", AccelByteSDK.FlightId)
            .GetResult();

        httpOperator.SendRequest(request, (response, error) =>
        {
            var result = response.TryParseJson<TokenData, OAuthError>();

            if (!result.IsError)
            {
                OnNewTokenObtained?.Invoke(result.Value);
                callback.Try(result);
            }
            else
            {
                OAuthError errorResult = GenerateOAuthError(result.Error, request, error, httpOperator);
                callback.TryError(errorResult);
            }
        });
    }

    [Obsolete("Instead, use the overload with the extended callback")]
    public void LoginWithUsernameV3
        (string username
        , string password
        , ResultCallback callback
        , bool rememberMe = false)
    {
        Report.GetFunctionLog(GetType().Name);
        Assert.IsNotNull(username, "Username parameter is null.");
        Assert.IsNotNull(password, "Password parameter is null.");

        var request = HttpRequestBuilder.CreatePost(BaseUrl + "/v3/oauth/token")
            .WithBasicAuthWithCookieAndAuthTrustId(Config.PublisherNamespace)
            .WithContentType(MediaType.ApplicationForm)
            .Accepts(MediaType.ApplicationJson)
            .WithFormParam("grant_type", "password")
            .WithFormParam("username", username)
            .WithFormParam("password", password)
            .WithFormParam("namespace", Namespace_)
            .WithFormParam("extend_exp", rememberMe ? "true" : "false")
            .AddAdditionalData("flightId", AccelByteSDK.FlightId)
            .GetResult();

        httpOperator.SendRequest(request, response =>
        {
            var result = response.TryParseJson<TokenData>();

            if (!result.IsError)
            {
                SaveAuthTrustId(result.Value);
                OnNewTokenObtained?.Invoke(result.Value);
                callback.TryOk();
            }
            else
            {
                callback.TryError(result.Error);
            }
        });
    }

    public void LoginWithUsernameV3
        (string username
        , string password
        , ResultCallback<TokenData, OAuthError> callback
        , bool rememberMe = false)
    {
        Report.GetFunctionLog(GetType().Name);
        Assert.IsNotNull(username, "Username parameter is null.");
        Assert.IsNotNull(password, "Password parameter is null.");

        var request = HttpRequestBuilder.CreatePost(BaseUrl + "/v3/oauth/token")
            .WithBasicAuthWithCookieAndAuthTrustId(Config.PublisherNamespace)
            .WithContentType(MediaType.ApplicationForm)
            .Accepts(MediaType.ApplicationJson)
            .WithFormParam("grant_type", "password")
            .WithFormParam("username", username)
            .WithFormParam("password", password)
            .WithFormParam("namespace", Namespace_)
            .WithFormParam("extend_exp", rememberMe ? "true" : "false")
            .AddAdditionalData("flightId", AccelByteSDK.FlightId)
            .GetResult();

        httpOperator.SendRequest(request, (response, error) =>
        {
            Result<TokenData, OAuthError> result = response.TryParseJson<TokenData, OAuthError>();

            if (!result.IsError)
            {
                SaveAuthTrustId(result.Value);
                OnNewTokenObtained?.Invoke(result.Value);
                callback.TryOk(result.Value);

            }
            else
            {
                OAuthError errorResult = GenerateOAuthError(result.Error, request, error, httpOperator);
                callback.TryError(errorResult);
            }
        });
    }

    [Obsolete("Instead, use the overload with the extended callback")]
    public void LoginWithDeviceId(ResultCallback callback)
    {
        Report.GetFunctionLog(GetType().Name);
        DeviceProvider deviceProvider = DeviceProvider.GetFromSystemInfo(Config.PublisherNamespace);

        IHttpRequest request = HttpRequestBuilder.CreatePost(BaseUrl + "/v3/oauth/platforms/device/token")
            .WithPathParam("platformId", deviceProvider.DeviceType)
            .WithBasicAuth()
            .WithContentType(MediaType.ApplicationForm)
            .Accepts(MediaType.ApplicationJson)
            .WithFormParam("device_id", deviceProvider.DeviceId)
            .WithFormParam("namespace", Namespace_)
            .AddAdditionalData("flightId", AccelByteSDK.FlightId)
            .GetResult();

        httpOperator.SendRequest(request, response =>
        {
            var result = response.TryParseJson<TokenData>();

            if (!result.IsError)
            {
                OnNewTokenObtained?.Invoke(result.Value);
                callback.TryOk();
            }
            else
            {
                callback.TryError(result.Error);
            }
        });
    }

    public void LoginWithDeviceId(ResultCallback<TokenData, OAuthError> callback)
    {
        Report.GetFunctionLog(GetType().Name);
        DeviceProvider deviceProvider = DeviceProvider.GetFromSystemInfo(Config.PublisherNamespace);

        string targetUri = BaseUrl + "/v3/oauth/platforms/device/token";
        IHttpRequest request = HttpRequestBuilder.CreatePost(targetUri)
            .WithPathParam("platformId", deviceProvider.DeviceType)
            .WithBasicAuthWithCookie(Config.PublisherNamespace)
            .WithContentType(MediaType.ApplicationForm)
            .Accepts(MediaType.ApplicationJson)
            .WithFormParam("device_id", deviceProvider.DeviceId)
            .WithFormParam("namespace", Namespace_)
            .AddAdditionalData("flightId", AccelByteSDK.FlightId)
            .GetResult();

        httpOperator.SendRequest(request, (response, error) =>
        {
            var result = response.TryParseJson<TokenData, OAuthError>();

            if (!result.IsError)
            {
                OnNewTokenObtained?.Invoke(result.Value);
                callback.TryOk(result.Value);
            }
            else
            {
                OAuthError errorResult = GenerateOAuthError(result.Error, request, error, httpOperator);
                callback.TryError(errorResult);
            }
        });
    }

    [Obsolete("Instead, use the overload with the extended callback")]
    public void LoginWithOtherPlatform
        (PlatformType platformType
        , string platformToken
        , ResultCallback callback
        , bool createHeadless = true)
    {
        Report.GetFunctionLog(GetType().Name);
        Assert.IsNotNull(platformToken, "PlatformToken parameter is null."); 

        var request = HttpRequestBuilder.CreatePost(BaseUrl + "/v3/oauth/platforms/{platformId}/token")
            .WithPathParam("platformId", platformType.ToString().ToLower())
            .WithBasicAuth()
            .WithContentType(MediaType.ApplicationForm)
            .Accepts(MediaType.ApplicationJson)
            .WithFormParam("platform_token", platformToken)
            .WithFormParam("namespace", Namespace_)
            .WithFormParam("createHeadless", createHeadless ? "true" : "false")
            .AddAdditionalData("flightId", AccelByteSDK.FlightId)
            .GetResult();

        httpOperator.SendRequest(request, response =>
        {
            var result = response.TryParseJson<TokenData>();

            if (!result.IsError)
            {
#if UNITY_GAMECORE
                if(result.Value != null)
                {
                    string fetchedDeviceId = result.Value.DeviceId;
                    if (!string.IsNullOrEmpty(fetchedDeviceId))
                    {
                        DeviceProvider.CacheDeviceId(result.Value.DeviceId);
                    }
                }
#endif

                OnNewTokenObtained?.Invoke(result.Value);
                callback.TryOk();
            }
            else
            {
                callback.TryError(result.Error);
            }
        });
    }

    public void LoginWithOtherPlatform
        (PlatformType platformType
        , string platformToken
        , ResultCallback<TokenData, OAuthError> callback
        , bool createHeadless = true)
    {
        Report.GetFunctionLog(GetType().Name);
        Assert.IsNotNull(platformToken, "PlatformToken parameter is null."); 

        var request = HttpRequestBuilder.CreatePost(BaseUrl + "/v3/oauth/platforms/{platformId}/token")
            .WithPathParam("platformId", platformType.ToString().ToLower())
            .WithBasicAuthWithCookie(Config.PublisherNamespace)
            .WithContentType(MediaType.ApplicationForm)
            .Accepts(MediaType.ApplicationJson)
            .WithFormParam("platform_token", platformToken)
            .WithFormParam("namespace", Namespace_)
            .WithFormParam("createHeadless", createHeadless ? "true" : "false")
            .AddAdditionalData("flightId", AccelByteSDK.FlightId)
            .GetResult();

        httpOperator.SendRequest(request, (response, error) =>
        {
            var result = response.TryParseJson<TokenData, OAuthError>();

            if (!result.IsError)
            {
#if UNITY_GAMECORE
                if(result.Value != null)
                {
                    string fetchedDeviceId = result.Value.DeviceId;
                    if (!string.IsNullOrEmpty(fetchedDeviceId))
                    {
                        DeviceProvider.CacheDeviceId(result.Value.DeviceId);
                    }
                }
#endif

            OnNewTokenObtained?.Invoke(result.Value);
                callback.TryOk(result.Value);
            }
            else
            {
                OAuthError errorResult = GenerateOAuthError(result.Error, request, error, httpOperator);
                callback.TryError(errorResult);
            }
        });
    }

    [Obsolete("Instead, use the overload with the extended callback")]
    public void LoginWithOtherPlatformId
        (string platformId
        , string platformToken
        , ResultCallback callback
        , bool createHeadless = true)
    {
        Report.GetFunctionLog(GetType().Name);
        Assert.IsNotNull(platformToken, "PlatformToken parameter is null.");

        var request = HttpRequestBuilder.CreatePost(BaseUrl + "/v3/oauth/platforms/{platformId}/token")
            .WithPathParam("platformId", platformId)
            .WithBasicAuth()
            .WithContentType(MediaType.ApplicationForm)
            .Accepts(MediaType.ApplicationJson)
            .WithFormParam("platform_token", platformToken)
            .WithFormParam("namespace", Namespace_)
            .WithFormParam("createHeadless", createHeadless ? "true" : "false")
            .WithFormParam("macAddress", string.Join("-", DeviceProvider.GetMacAddress()))
            .AddAdditionalData("flightId", AccelByteSDK.FlightId)
            .GetResult();

        httpOperator.SendRequest(request, response =>
        {
            var result = response.TryParseJson<TokenData>();

            if (!result.IsError)
            {
                OnNewTokenObtained?.Invoke(result.Value);
                callback.TryOk();
            }
            else
            {
                callback.TryError(result.Error);
            }
        });
    }

    public void LoginWithOtherPlatformId
        (string platformId
        , string platformToken
        , ResultCallback<TokenData, OAuthError> callback
        , bool createHeadless = true)
    {
        Report.GetFunctionLog(GetType().Name);
        Assert.IsNotNull(platformToken, "PlatformToken parameter is null.");

        var request = HttpRequestBuilder.CreatePost(BaseUrl + "/v3/oauth/platforms/{platformId}/token")
            .WithPathParam("platformId", platformId)
            .WithBasicAuthWithCookie(Config.PublisherNamespace)
            .WithContentType(MediaType.ApplicationForm)
            .Accepts(MediaType.ApplicationJson)
            .WithFormParam("platform_token", platformToken)
            .WithFormParam("namespace", Namespace_)
            .WithFormParam("createHeadless", createHeadless ? "true" : "false")
            .WithFormParam("macAddress", string.Join("-", DeviceProvider.GetMacAddress()))
            .AddAdditionalData("flightId", AccelByteSDK.FlightId)
            .GetResult();

        httpOperator.SendRequest(request, (response, error) =>
        {
            var result = response.TryParseJson<TokenData, OAuthError>();

            if (!result.IsError)
            {
                OnNewTokenObtained?.Invoke(result.Value);
                callback.TryOk(result.Value);
            }
            else
            {
                OAuthError errorResult = GenerateOAuthError(result.Error, request, error, httpOperator);
                callback.TryError(errorResult);
            }
        });
    }

    [Obsolete("Instead, use the overload with the extended callback")]
    public void CreateHeadlessAccountAndResponseToken
        (string linkingToken
        , bool extendExp
        , ResultCallback callback)
    {
        Report.GetFunctionLog(GetType().Name);
        Assert.IsNotNull(linkingToken, "linkingToken parameter is null.");

        var request = HttpRequestBuilder.CreatePost(BaseUrl + "/v3/headless/token")
            .WithBasicAuth()
            .WithContentType(MediaType.ApplicationForm)
            .Accepts(MediaType.ApplicationJson)
            .WithFormParam("linkingToken", linkingToken)
            .WithFormParam("extend_exp", extendExp ? "true" : "false")
            .AddAdditionalData("flightId", AccelByteSDK.FlightId)
            .GetResult();

        httpOperator.SendRequest(request, response =>
        {
            var result = response.TryParseJson<TokenData>();

            if (!result.IsError)
            {
                OnNewTokenObtained?.Invoke(result.Value);
                callback.TryOk();
            }
            else
            {
                callback.TryError(result.Error);
            }
        });
    }

    public void CreateHeadlessAccountAndResponseToken
        (string linkingToken
        , bool extendExp
        , ResultCallback<TokenData, OAuthError> callback)
    {
        Report.GetFunctionLog(GetType().Name);
        Assert.IsNotNull(linkingToken, "linkingToken parameter is null.");

        var request = HttpRequestBuilder.CreatePost(BaseUrl + "/v3/headless/token")
            .WithBasicAuth()
            .WithContentType(MediaType.ApplicationForm)
            .Accepts(MediaType.ApplicationJson)
            .WithFormParam("linkingToken", linkingToken)
            .WithFormParam("extend_exp", extendExp ? "true" : "false")
            .AddAdditionalData("flightId", AccelByteSDK.FlightId)
            .GetResult();

        httpOperator.SendRequest(request, (response, error) =>
        {
            var result = response.TryParseJson<TokenData, OAuthError>();

            if (!result.IsError)
            {
                OnNewTokenObtained?.Invoke(result.Value);
                callback.TryOk(result.Value);
            }
            else
            {
                OAuthError errorResult = GenerateOAuthError(result.Error, request, error, httpOperator);
                callback.TryError(errorResult);
            }
        });
    }

    [Obsolete("Instead, use the overload with the extended callback")]
    public void AuthenticationWithPlatformLink
        (string username
        , string password
        , string linkingToken
        , ResultCallback callback)
    {
        Report.GetFunctionLog(GetType().Name);
        Assert.IsNotNull(linkingToken, "linkingToken parameter is null.");

        var request = HttpRequestBuilder.CreatePost(BaseUrl + "/v3/authenticateWithLink")
            .WithBasicAuth()
            .WithContentType(MediaType.ApplicationForm)
            .Accepts(MediaType.ApplicationJson)
            .WithFormParam("username", username)
            .WithFormParam("password", password)
            .WithFormParam("linkingToken", linkingToken)
            .WithFormParam("client_id", AccelBytePlugin.OAuthConfig.ClientId)
            .AddAdditionalData("flightId", AccelByteSDK.FlightId)
            .GetResult();

        httpOperator.SendRequest(request, response =>
        {
            var result = response.TryParseJson<TokenData>();

            if (!result.IsError)
            {
                OnNewTokenObtained?.Invoke(result.Value);
                callback.TryOk();
            }
            else
            {
                callback.TryError(result.Error);
            }
        });
    }

    public void AuthenticationWithPlatformLink
        (string username
        , string password
        , string linkingToken
        , ResultCallback<TokenData, OAuthError> callback)
    {
        Report.GetFunctionLog(GetType().Name);
        Assert.IsNotNull(linkingToken, "linkingToken parameter is null.");

        var request = HttpRequestBuilder.CreatePost(BaseUrl + "/v3/authenticateWithLink")
            .WithBasicAuth()
            .WithContentType(MediaType.ApplicationForm)
            .Accepts(MediaType.ApplicationJson)
            .WithFormParam("username", username)
            .WithFormParam("password", password)
            .WithFormParam("linkingToken", linkingToken)
            .WithFormParam("client_id", AccelBytePlugin.OAuthConfig.ClientId)
            .AddAdditionalData("flightId", AccelByteSDK.FlightId)
            .GetResult();

        httpOperator.SendRequest(request, (response, error) =>
        {
            var result = response.TryParseJson<TokenData, OAuthError>();

            if (!result.IsError)
            {
                OnNewTokenObtained?.Invoke(result.Value);
                callback.TryOk(result.Value);
            }
            else
            {
                OAuthError errorResult = GenerateOAuthError(result.Error, request, error, httpOperator);
                callback.TryError(errorResult);
            }
        });
    }

    [Obsolete("This end point is going to be deprecated, use LoginWithAuthorizationCodeV3 instead")]
    public void LoginWithAuthorizationCode
        (string code
        , ResultCallback callback)
    {
        Report.GetFunctionLog(GetType().Name);
        Assert.IsNotNull(code, "Code parameter is null.");

        var request = HttpRequestBuilder.CreatePost(BaseUrl + "/oauth/token")
            .WithBasicAuth()
            .WithContentType(MediaType.ApplicationForm)
            .Accepts(MediaType.ApplicationJson)
            .WithFormParam("grant_type", "authorization_code")
            .WithFormParam("code", code)
            .WithFormParam("redirect_uri", this.Config.RedirectUri)
            .AddAdditionalData("flightId", AccelByteSDK.FlightId)
            .GetResult();

        httpOperator.SendRequest(request, response =>
        {
            var result = response.TryParseJson<TokenData>();

            if (!result.IsError)
            {
                OnNewTokenObtained?.Invoke(result.Value);
                callback.TryOk();
            }
            else
            {
                callback.TryError(result.Error);
            }
        });
    }

    [Obsolete("This end point is going to be deprecated, use LoginWithAuthorizationCodeV3 instead")]
    public void LoginWithAuthorizationCode
        (string code
        , ResultCallback<TokenData, OAuthError> callback)
    {
        Report.GetFunctionLog(GetType().Name);
        Assert.IsNotNull(code, "Code parameter is null.");

        var request = HttpRequestBuilder.CreatePost(BaseUrl + "/oauth/token")
            .WithBasicAuthWithCookie(Config.PublisherNamespace)
            .WithContentType(MediaType.ApplicationForm)
            .Accepts(MediaType.ApplicationJson)
            .WithFormParam("grant_type", "authorization_code")
            .WithFormParam("code", code)
            .WithFormParam("redirect_uri", this.Config.RedirectUri)
            .AddAdditionalData("flightId", AccelByteSDK.FlightId)
            .GetResult();

        httpOperator.SendRequest(request, (response, error) =>
        {
            var result = response.TryParseJson<TokenData, OAuthError>();

            if (!result.IsError)
            {
                OnNewTokenObtained?.Invoke(result.Value);
                callback.TryOk(result.Value);
            }
            else
            {
                OAuthError errorResult = GenerateOAuthError(result.Error, request, error, httpOperator);
                callback.TryError(errorResult);
            }
        });
    }

    public void LoginWithAuthorizationCodeV3
            (string code
            , ResultCallback<TokenData, OAuthError> callback)
    {
        Report.GetFunctionLog(GetType().Name);
        Assert.IsNotNull(code, "Code parameter is null.");

        var request = HttpRequestBuilder.CreatePost(BaseUrl + "/v3/oauth/token")
            .WithBasicAuthWithCookie(Config.PublisherNamespace)
            .WithContentType(MediaType.ApplicationForm)
            .Accepts(MediaType.ApplicationJson)
            .WithFormParam("grant_type", "authorization_code")
            .WithFormParam("code", code)
            .WithFormParam("redirect_uri", this.Config.RedirectUri)
            .AddAdditionalData("flightId", AccelByteSDK.FlightId)
            .GetResult();

        httpOperator.SendRequest(request, (response, error) =>
        {
            var result = response.TryParseJson<TokenData, OAuthError>();

            if (!result.IsError)
            {
                OnNewTokenObtained?.Invoke(result.Value);
                callback.TryOk(result.Value);
            }
            else
            {
                OAuthError errorResult = GenerateOAuthError(result.Error, request, error, httpOperator);
                callback.TryError(errorResult);
            }
        });
    }

    public void Logout(string Bearer, ResultCallback callback)
    {
        Report.GetFunctionLog(GetType().Name);

        var request = HttpRequestBuilder.CreatePost(BaseUrl + "/v3/logout")
            .WithBearerAuth(Bearer)
            .WithContentType(MediaType.TextPlain)
            .GetResult();

        httpOperator.SendRequest(request, response =>
        {
            var result = response.TryParse();
            callback.Try(result);
        });
    }

    [Obsolete("Instead, use the overload with the extended callback")]
    public void RefreshSession(string refreshToken, ResultCallback callback)
    {
        var request = HttpRequestBuilder.CreatePost(BaseUrl + "/v3/oauth/token")
            .WithBasicAuth()
            .WithContentType(MediaType.ApplicationForm)
            .Accepts(MediaType.ApplicationJson)
            .WithFormParam("grant_type", "refresh_token")
            .WithFormParam("refresh_token", refreshToken)
            .AddAdditionalData("flightId", AccelByteSDK.FlightId)
            .GetResult();

        request.Priority = 0;

        httpOperator.SendRequest(request, response =>
        {
            var result = response.TryParseJson<TokenData>();

            if (!result.IsError)
            {
                OnNewTokenObtained?.Invoke(result.Value);
                callback.TryOk();
                return;
            }

            callback.TryError(result.Error);
        });
    }

    public void RefreshSession(string refreshToken, ResultCallback<TokenData, OAuthError> callback)
    {
        var request = HttpRequestBuilder.CreatePost(BaseUrl + "/v3/oauth/token")
            .WithBasicAuthWithCookie(Config.PublisherNamespace)
            .WithContentType(MediaType.ApplicationForm)
            .Accepts(MediaType.ApplicationJson)
            .WithFormParam("grant_type", "refresh_token")
            .WithFormParam("refresh_token", refreshToken)
            .AddAdditionalData("flightId", AccelByteSDK.FlightId)
            .GetResult();

        request.Priority = 0;

        httpOperator.SendRequest(request, (response, error) =>
        {
            var result = response.TryParseJson<TokenData, OAuthError>();

            if (!result.IsError)
            {
                OnNewTokenObtained?.Invoke(result.Value);
                callback.TryOk(result.Value);
            }
            else
            {
                OAuthError errorResult = GenerateOAuthError(result.Error, request, error, httpOperator);
                callback.TryError(errorResult);
            }
        });
    }


    public void Verify2FACode
        (string mfaToken
        , TwoFAFactorType factor
        , string code
        , ResultCallback<TokenData, OAuthError> callback
        , bool rememberDevice = false)
    {
        Report.GetFunctionLog(GetType().Name);
        Assert.IsNotNull(mfaToken, "mfaToken parameter is null.");
        Assert.IsNotNull(code, "code parameter is null.");

        var request = HttpRequestBuilder.CreatePost(BaseUrl + "/v3/oauth/mfa/verify")
            .WithBasicAuthWithCookie(Config.PublisherNamespace)
            .WithContentType(MediaType.ApplicationForm)
            .Accepts(MediaType.ApplicationJson)
            .WithFormParam("mfaToken", mfaToken)
            .WithFormParam("factor", factor.GetString())
            .WithFormParam("code", code)
            .WithFormParam("rememberDevice", rememberDevice ? "true" : "false")
            .GetResult();

        httpOperator.SendRequest(request, (response, error) =>
        {
            Result<TokenData, OAuthError> result = response.TryParseJson<TokenData, OAuthError>();

            if (!result.IsError)
            {
                OnNewTokenObtained?.Invoke(result.Value);
                callback.TryOk(result.Value);
            }
            else
            {
                OAuthError errorResult = GenerateOAuthError(result.Error, request, error, httpOperator);
                callback.TryError(errorResult);
            }
        });
    }

    public void VerifyToken(string token
       , ResultCallback callback)
    {
        Report.GetFunctionLog(GetType().Name);
        Assert.IsNotNull(token, "Can't verify token! token parameter is null!");

        var request = HttpRequestBuilder
            .CreatePost(BaseUrl + "/v3/oauth/verify")
            .WithBasicAuthWithCookie(Config.PublisherNamespace)
            .WithContentType(MediaType.ApplicationForm)
            .Accepts(MediaType.ApplicationJson)
            .WithFormParam("token", token)
            .GetResult();

        httpOperator.SendRequest(request, response =>
        {
            Result<TokenData> result = response.TryParseJson<TokenData>();

            if (!result.IsError)
            {
                if (token.Equals(result.Value.access_token))
                {
                    callback.TryOk();
                }
                else
                {
                    const string errorMessage = "Access Token is not valid, different value between input token and obtained token.";
                    callback.TryError(new Error(ErrorCode.InvalidResponse, errorMessage));
                }
            }
            else
            {
                callback.TryError(result.Error);
            }
        });
    }
    
    public void GenerateOneTimeCode(string AccessToken
        , PlatformType platformId
        , ResultCallback<GeneratedOneTimeCode> callback)
    {
        Report.GetFunctionLog(GetType().Name);
        Assert.IsNotNull(AccessToken, "Can't verify token! token parameter is null!");

        var request = HttpRequestBuilder
            .CreatePost(BaseUrl + "/v3/link/code/request")
            .WithBearerAuth(AccessToken)
            .WithContentType(MediaType.ApplicationForm)
            .Accepts(MediaType.ApplicationJson)
            .WithFormParam("platformId", platformId.ToString().ToLower())
            .GetResult();

        httpOperator.SendRequest(request, response =>
        {
            var result = response.TryParseJson<GeneratedOneTimeCode>();

            callback.Try(result);
        });
    }

    private void SaveAuthTrustId(TokenData tokenData)
    {
        string authTrustId = tokenData.auth_trust_id;
        if (!string.IsNullOrEmpty(authTrustId))
        {
            PlayerPrefs.SetString(UserSession.AuthTrustIdKey, authTrustId);
        }
    }
    
    public void GenerateGameToken(string code
        , ResultCallback callback)
    {
        Report.GetFunctionLog(GetType().Name);
        Assert.IsNotNull(code, "Can't Generate Game Token! Code parameter is null!");

        var request = HttpRequestBuilder
            .CreatePost(BaseUrl + "/v3/token/exchange")
            .WithBasicAuth()
            .WithContentType(MediaType.ApplicationForm)
            .Accepts(MediaType.ApplicationJson)
            .WithFormParam("code", code)
            .GetResult();

        httpOperator.SendRequest(request, response =>
        {
            var result = response.TryParseJson<TokenData>();
            if (!result.IsError)
            {
                SaveAuthTrustId(result.Value);
                OnNewTokenObtained?.Invoke(result.Value);
                callback.TryOk();
            }
            else
            {
                callback.TryError(result.Error);
            }
        });
    }

    private OAuthError GenerateOAuthError(OAuthError authError, IHttpRequest request , Error requestError, HttpOperator targetHttpOperator)
    {
        OAuthError errorResult = authError;
        if (string.IsNullOrEmpty(errorResult.error_uri) && request != null)
        {
            errorResult.error_uri = request.Url;
        }
        if (string.IsNullOrEmpty(errorResult.clientId) && targetHttpOperator != null)
        {
            errorResult.clientId = targetHttpOperator.HttpClient.GetCredentials().ClientId;
        }
        if ((string.IsNullOrEmpty(errorResult.error) || string.IsNullOrEmpty(errorResult.error_description)) && requestError != null)
        {
            errorResult.error = requestError.Code.ToString();
            errorResult.error_description = requestError.Message;
        }
        return errorResult;
    }
}
