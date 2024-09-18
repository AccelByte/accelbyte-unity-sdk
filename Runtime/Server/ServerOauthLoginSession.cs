// Copyright (c) 2020 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Server
{
    public class ServerOauthLoginSession : ISession
    {
        internal System.Action OnLoginSuccess;
        internal System.Action OnLogoutSuccess;
        internal readonly string BaseUrl;
        internal readonly HttpOperator HttpOperator;
        private readonly string clientId;
        private readonly string clientSecret;
        private readonly IHttpClient httpClient;

        internal ServerOauthLoginSession( string inBaseUrl
            , string inClientId
            , string inClientSecret
            , IHttpClient inHttpClient
            , CoroutineRunner inCoroutineRunner
            , HttpOperator httpOperator = null)
        {
            Assert.IsNotNull(inBaseUrl, $"Creating {GetType().Name} failed. Parameter inBaseUrl is null");
            Assert.IsNotNull(inClientId, "Creating " + GetType().Name + " failed. inClientId parameter is null!");
            Assert.IsNotNull(inClientSecret, "Creating " + GetType().Name + " failed. inClientSecret parameter is null!");
            Assert.IsNotNull(inHttpClient, "Creating " + GetType().Name + " failed. Parameter inHttpClient is null");
            Assert.IsNotNull(inCoroutineRunner, "Creating " + GetType().Name + " failed. Parameter inCoroutineRunner is null" );

            BaseUrl = inBaseUrl;
            clientId = inClientId;
            clientSecret = inClientSecret;
            httpClient = inHttpClient;
#if UNITY_WEBGL && !UNITY_EDITOR
            this.HttpOperator = httpOperator == null ? new HttpCoroutineOperator(httpClient, inCoroutineRunner) : httpOperator;
#else
            this.HttpOperator = httpOperator == null ? new HttpAsyncOperator(httpClient) : httpOperator;
#endif
            CreateSessionMaintainer();
        }

        public override string AuthorizationToken
        {
            get { return tokenData != null ? tokenData.access_token : null; }
            set { tokenData.access_token = value; }
        }

        public IEnumerator LoginWithClientCredentials( ResultCallback callback )
        {
            ResultCallback<TokenData> getTokenCallback = result =>
            {
                if (!result.IsError)
                {
                    callback?.TryOk();
                }
                else
                {
                    callback?.TryError(result.Error);
                }
            };
            LoginWithClientCredentials(getTokenCallback);
            yield break;
        }

        internal void LoginWithClientCredentials(ResultCallback<TokenData> callback)
        {
            const bool clienSecretRequired = false;
            IHttpRequest request = HttpRequestBuilder.CreatePost(BaseUrl + "/v3/oauth/token")
                .WithBasicAuth(clientId, clientSecret, clienSecretRequired)
                .WithContentType(MediaType.ApplicationForm)
                .Accepts(MediaType.ApplicationJson)
                .WithFormParam("grant_type", "client_credentials")
                .GetResult();
            
            HttpOperator.SendRequest(request, response =>
            {
                Result<TokenData> getClientTokenResult = response.TryParseJson<TokenData>();

                if (!getClientTokenResult.IsError)
                {
                    OnReceivedLoginToken(getClientTokenResult.Value);
                    SessionMaintainer?.Start(SharedMemory?.CoreHeartBeat, SharedMemory?.Logger, tokenData.expires_in);

                    callback?.TryOk(getClientTokenResult.Value);
                }
                else
                {
                    callback?.TryError(getClientTokenResult.Error);
                }
            });
        }

        private IEnumerator GetClientToken( ResultCallback<TokenData> callback )
        {
            const bool clienSecretRequired = false;
            IHttpRequest request = HttpRequestBuilder.CreatePost(BaseUrl + "/v3/oauth/token")
                .WithBasicAuth(clientId, clientSecret, clienSecretRequired)
                .WithContentType(MediaType.ApplicationForm)
                .Accepts(MediaType.ApplicationJson)
                .WithFormParam("grant_type", "client_credentials")
                .GetResult();

            IHttpResponse response = null;

            yield return httpClient.SendRequest(request,
                rsp => response = rsp);

            Result<TokenData> result = response.TryParseJson<TokenData>();
            callback.Try(result);
        }

        public IEnumerator Logout( ResultCallback callback )
        {
            LogoutAsync(callback);
            yield break;
        }

        internal void LogoutAsync(ResultCallback callback)
        {
            var request = HttpRequestBuilder.CreatePost(BaseUrl + "/v3/oauth/revoke")
                .WithBasicAuth(clientId, clientSecret)
                .WithContentType(MediaType.ApplicationForm)
                .Accepts(MediaType.ApplicationJson)
                .WithFormParam("token", AuthorizationToken)
                .GetResult();
            
            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParse();

                if (!result.IsError)
                {
                    tokenData = null;
                    SessionMaintainer?.Stop();
                    OnLogoutSuccess?.Invoke();
                }

                callback?.Try(result);
            });
        }

        public override IEnumerator RefreshSessionApiCall(ResultCallback<TokenData, OAuthError> callback)
        {
            yield return GetClientToken(result=>
            {
                if (result.IsError || result.Value == null)
                {
                    var error = new OAuthError();
                    if (result.Error != null)
                    {
                        error.error = result.Error.Code.ToString();
                        error.error_description = result.Error.Message?.ToString();
                    }

                    callback?.TryError(error);
                }
                else
                {
                    SetSession(result.Value);
                    callback?.TryOk(result.Value);
                }
            });
        }

        public IEnumerator GetJwks(ResultCallback<JwkSet> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            var request = HttpRequestBuilder.CreateGet(BaseUrl + "/v3/oauth/jwks")
                .GetResult();

            IHttpResponse response = null;

            yield return httpClient.SendRequest(request,
                rsp =>
                {
                    response = rsp;
                });

            var result = response.TryParseJson<JwkSet>();
            callback.Try(result);
        }

        public override void SetSession(TokenData loginResponse)
        {
            httpClient.AddAdditionalHeaderInfo(AccelByteHttpClient.NamespaceHeaderKey, loginResponse.Namespace);
            tokenData = loginResponse;
            SessionMaintainer?.Start(SharedMemory?.CoreHeartBeat, SharedMemory?.Logger, tokenData.expires_in * 0.8f);
        }

        internal void Reset()
        {
            SessionMaintainer?.Stop();
            OnLoginSuccess = null;
            OnLogoutSuccess = null;
            tokenData = null;
        }

        internal void OnReceivedLoginToken(TokenData token)
        {
            SetSession(token);
            OnLoginSuccess?.Invoke();
        }

        public override AccelByteResult<TokenData, OAuthError> RefreshSessionApiCall()
        {
            var retval = new AccelByteResult<TokenData, OAuthError>();
            LoginWithClientCredentials(result =>
            {
                if (result.IsError || result.Value == null)
                {
                    var error = new OAuthError();
                    error.error = result.Error.Code.ToString();
                    error.error_description = result.Error.Message?.ToString();
                    retval.Reject(error);
                }
                else
                {
                    SetSession(result.Value);
                    retval.Resolve(result.Value);
                }
            });

            return retval;
        }
    }
}
