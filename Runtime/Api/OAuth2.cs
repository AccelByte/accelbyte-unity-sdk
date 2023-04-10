using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Api;

public class OAuth2 : ApiBase
{
    public OAuth2(IHttpClient HttpClient,
        Config config,
        ISession session)
        : base(HttpClient, config, config.IamServerUrl, session)
    {
        OnNewTokenObtained = (tokenData) =>
        {
            session.SetSession(tokenData);
        };
        session.CallRefresh = RefreshSession;
    }

    //Need to be assigned to handle new access token obtained
    private Action<TokenData> OnNewTokenObtained = null;

    [Obsolete("This end point is going to be deprected, use LoginWithUsernameV3 instead")]
    public IEnumerator LoginWithUsername
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
            .GetResult();

        IHttpResponse response = null;

        yield return HttpClient.SendRequest(request, rsp => response = rsp);

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
    }

    [Obsolete("This end point is going to be deprected, use LoginWithUsernameV3 instead")]
    public IEnumerator LoginWithUsername
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
            .GetResult();

        IHttpResponse response = null;

        yield return HttpClient.SendRequest(request,
            rsp => response = rsp);

        var result = response.TryParseJson<TokenData, OAuthError>();

        if (!result.IsError)
        {
            OnNewTokenObtained?.Invoke(result.Value);
            callback.Try(result);
        }
        else
        {
            callback.TryError(result.Error);
        }
    }

    [Obsolete("Instead, use the overload with the extended callback")]
    public IEnumerator LoginWithUsernameV3
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
            .GetResult();

        IHttpResponse response = null;

        yield return HttpClient.SendRequest(request, rsp => response = rsp);

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
    }

    public IEnumerator LoginWithUsernameV3
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
            .GetResult();

        IHttpResponse response = null;

        yield return HttpClient.SendRequest(request,
            rsp =>
            {
                response = rsp;
            }
        );

        Result<TokenData, OAuthError> result = response.TryParseJson<TokenData, OAuthError>();

        if (!result.IsError)
        {
            SaveAuthTrustId(result.Value);
            OnNewTokenObtained?.Invoke(result.Value);
            callback.TryOk(result.Value);

        }
        else
        {
            callback.TryError(result.Error);
        }
    }

    [Obsolete("Instead, use the overload with the extended callback")]
    public IEnumerator LoginWithDeviceId(ResultCallback callback)
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
            .GetResult();

        IHttpResponse response = null;

        yield return HttpClient.SendRequest(request, rsp => response = rsp);

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
    }

    public IEnumerator LoginWithDeviceId(ResultCallback<TokenData, OAuthError> callback)
    {
        Report.GetFunctionLog(GetType().Name);
        DeviceProvider deviceProvider = DeviceProvider.GetFromSystemInfo(Config.PublisherNamespace);

        IHttpRequest request = HttpRequestBuilder.CreatePost(BaseUrl + "/v3/oauth/platforms/device/token")
            .WithPathParam("platformId", deviceProvider.DeviceType)
            .WithBasicAuthWithCookie(Config.PublisherNamespace)
            .WithContentType(MediaType.ApplicationForm)
            .Accepts(MediaType.ApplicationJson)
            .WithFormParam("device_id", deviceProvider.DeviceId)
            .WithFormParam("namespace", Namespace_)
            .GetResult();

        IHttpResponse response = null;

        yield return HttpClient.SendRequest(request, rsp =>
            {
                response = rsp;
            }
        );

        var result = response.TryParseJson<TokenData, OAuthError>();

        if (!result.IsError)
        {
            OnNewTokenObtained?.Invoke(result.Value);
            callback.TryOk(result.Value);
        }
        else
        {
            callback.TryError(result.Error);
        }
    }

    [Obsolete("Instead, use the overload with the extended callback")]
    public IEnumerator LoginWithOtherPlatform
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
            .GetResult();

        IHttpResponse response = null;

        yield return HttpClient.SendRequest(request, rsp => response = rsp);

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
    }

    public IEnumerator LoginWithOtherPlatform
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
            .GetResult();

        IHttpResponse response = null;

        yield return HttpClient.SendRequest(request,
            rsp => response = rsp);

        var result = response.TryParseJson<TokenData, OAuthError>();

        if (!result.IsError)
        {
            OnNewTokenObtained?.Invoke(result.Value);
            callback.TryOk(result.Value);
        }
        else
        {
            callback.TryError(result.Error);
        }
    }

    [Obsolete("Instead, use the overload with the extended callback")]
    public IEnumerator LoginWithOtherPlatformId
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
            .GetResult();

        IHttpResponse response = null;

        yield return HttpClient.SendRequest(request, rsp => response = rsp);

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
    }

    public IEnumerator LoginWithOtherPlatformId
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
            .GetResult();

        IHttpResponse response = null;

        yield return HttpClient.SendRequest(request,
            rsp => response = rsp);

        var result = response.TryParseJson<TokenData, OAuthError>();

        if (!result.IsError)
        {
            OnNewTokenObtained?.Invoke(result.Value);
            callback.TryOk(result.Value);
        }
        else
        {
            callback.TryError(result.Error);
        }
    }

    [Obsolete("Instead, use the overload with the extended callback")]
    public IEnumerator CreateHeadlessAccountAndResponseToken
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
            .GetResult();

        IHttpResponse response = null;

        yield return HttpClient.SendRequest(request, rsp => response = rsp);

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
    }

    public IEnumerator CreateHeadlessAccountAndResponseToken
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
            .GetResult();

        IHttpResponse response = null;

        yield return HttpClient.SendRequest(request,
            rsp => response = rsp);

        var result = response.TryParseJson<TokenData, OAuthError>();

        if (!result.IsError)
        {
            OnNewTokenObtained?.Invoke(result.Value);
            callback.TryOk(result.Value);
        }
        else
        {
            callback.TryError(result.Error);
        }
    }

    [Obsolete("Instead, use the overload with the extended callback")]
    public IEnumerator AuthenticationWithPlatformLink
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
            .GetResult();

        IHttpResponse response = null;

        yield return HttpClient.SendRequest(request, rsp => response = rsp);

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
    }

    public IEnumerator AuthenticationWithPlatformLink
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
            .GetResult();

        IHttpResponse response = null;

        yield return HttpClient.SendRequest(request,
            rsp => response = rsp);

        var result = response.TryParseJson<TokenData, OAuthError>();

        if (!result.IsError)
        {
            OnNewTokenObtained?.Invoke(result.Value);
            callback.TryOk(result.Value);
        }
        else
        {
            callback.TryError(result.Error);
        }
    }

    [Obsolete("This end point is going to be deprecated, use LoginWithAuthorizationCodeV3 instead")]
    public IEnumerator LoginWithAuthorizationCode
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
            .GetResult();

        IHttpResponse response = null;

        yield return HttpClient.SendRequest(request, rsp => response = rsp);

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
    }

    [Obsolete("This end point is going to be deprecated, use LoginWithAuthorizationCodeV3 instead")]
    public IEnumerator LoginWithAuthorizationCode
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
            .GetResult();

        IHttpResponse response = null;

        yield return HttpClient.SendRequest(request,
            rsp => response = rsp);

        var result = response.TryParseJson<TokenData, OAuthError>();

        if (!result.IsError)
        {
            OnNewTokenObtained?.Invoke(result.Value);
            callback.TryOk(result.Value);
        }
        else
        {
            callback.TryError(result.Error);
        }
    }

    public IEnumerator LoginWithAuthorizationCodeV3
            (string code
            , ResultCallback<TokenData, OAuthError> callback)
    {
        Report.GetFunctionLog(GetType().Name);
        Assert.IsNotNull(code, "Code parameter is null.");

        var request = HttpRequestBuilder.CreatePost(BaseUrl + "v3/oauth/token")
            .WithBasicAuthWithCookie(Config.PublisherNamespace)
            .WithContentType(MediaType.ApplicationForm)
            .Accepts(MediaType.ApplicationJson)
            .WithFormParam("grant_type", "authorization_code")
            .WithFormParam("code", code)
            .WithFormParam("redirect_uri", this.Config.RedirectUri)
            .GetResult();

        IHttpResponse response = null;

        yield return HttpClient.SendRequest(request,
            rsp => response = rsp);

        var result = response.TryParseJson<TokenData, OAuthError>();

        if (!result.IsError)
        {
            OnNewTokenObtained?.Invoke(result.Value);
            callback.TryOk(result.Value);
        }
        else
        {
            callback.TryError(result.Error);
        }
    }

    public IEnumerator Logout(string Bearer, ResultCallback callback)
    {
        Report.GetFunctionLog(GetType().Name);

        var request = HttpRequestBuilder.CreatePost(BaseUrl + "/v3/logout")
            .WithBearerAuth(Bearer)
            .WithContentType(MediaType.TextPlain)
            .GetResult();

        IHttpResponse response = null;

        yield return HttpClient.SendRequest(request,
            rsp => response = rsp);

        var result = response.TryParse();
        callback.Try(result);
    }

    [Obsolete("Instead, use the overload with the extended callback")]
    public IEnumerator RefreshSession(string refreshToken, ResultCallback callback)
    {
        var request = HttpRequestBuilder.CreatePost(BaseUrl + "/v3/oauth/token")
            .WithBasicAuth()
            .WithContentType(MediaType.ApplicationForm)
            .Accepts(MediaType.ApplicationJson)
            .WithFormParam("grant_type", "refresh_token")
            .WithFormParam("refresh_token", refreshToken)
            .GetResult();

        IHttpResponse response = null;

        yield return HttpClient.SendRequest(request, rsp => response = rsp);

        var result = response.TryParseJson<TokenData>();

        if (!result.IsError)
        {
            OnNewTokenObtained?.Invoke(result.Value);
            callback.TryOk();
            yield break;
        }

        callback.TryError(result.Error);
    }

    public IEnumerator RefreshSession(string refreshToken, ResultCallback<TokenData, OAuthError> callback)
    {
        var request = HttpRequestBuilder.CreatePost(BaseUrl + "/v3/oauth/token")
            .WithBasicAuthWithCookie(Config.PublisherNamespace)
            .WithContentType(MediaType.ApplicationForm)
            .Accepts(MediaType.ApplicationJson)
            .WithFormParam("grant_type", "refresh_token")
            .WithFormParam("refresh_token", refreshToken)
            .GetResult();

        IHttpResponse response = null;

        yield return HttpClient.SendRequest(request,
            rsp => response = rsp);

        var result = response.TryParseJson<TokenData, OAuthError>();

        if (!result.IsError)
        {
            OnNewTokenObtained?.Invoke(result.Value);
            callback.TryOk(result.Value);
            yield break;
        }

        callback.TryError(result.Error);
    }


    public IEnumerator Verify2FACode
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

        IHttpResponse response = null;

        yield return HttpClient.SendRequest(request, rsp => response = rsp);

        Result<TokenData, OAuthError> result = response.TryParseJson<TokenData, OAuthError>();

        if (!result.IsError)
        {
            OnNewTokenObtained?.Invoke(result.Value);
            callback.TryOk(result.Value);
        }
        else
        {
            callback.TryError(result.Error);
        }
    }

    public IEnumerator VerifyToken(string token
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

        IHttpResponse response = null;

        yield return HttpClient.SendRequest(request, rsp => response = rsp);

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
    }
    
    public IEnumerator GenerateOneTimeCode(string AccessToken
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

        IHttpResponse response = null;

        yield return HttpClient.SendRequest(request, 
            rsp => response = rsp);

        var result = response.TryParseJson<GeneratedOneTimeCode>();

        callback.Try(result);
    }

    private void SaveAuthTrustId(TokenData tokenData)
    {
        string authTrustId = tokenData.auth_trust_id;
        if (!string.IsNullOrEmpty(authTrustId))
        {
            PlayerPrefs.SetString(UserSession.AuthTrustIdKey, authTrustId);
        }
    }
    
    public IEnumerator GenerateGameToken(string code
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

        IHttpResponse response = null;
        yield return HttpClient.SendRequest(request, rsp => response = rsp);
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
    }
}
