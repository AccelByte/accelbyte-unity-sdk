﻿// Copyright (c) 2022 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Core
{
    /// <summary>
    /// Inherit this abstract class to set common Api functionality.
    /// - Inheritted by, for example: ApiBase (Client) & ServerApiBase
    /// </summary>
    public abstract class ApiBaseParent
    {
        #region Constructor
        /// <summary>
        /// Contains session credential actions
        /// </summary>
        private readonly IHttpClient httpClient;
        
        /// <summary>
        /// Contains AuthorizationToken and related auth
        /// </summary>
        protected readonly ISession Session; // contains accessToken

        /// <summary>
        /// Comes from Config || ServerConfig, set @ SetBaseUrl() from constructor.
        /// - Eg, Config.BaseUrl || Config.CloudStorageServerUrl
        /// </summary>
        protected internal readonly string BaseUrl;

        private ApiSharedMemory sharedMemory;
        
        internal ApiSharedMemory SharedMemory
        {
            get
            {
                if(sharedMemory == null)
                {
                    sharedMemory = new ApiSharedMemory();
                }
                return sharedMemory;
            }
            set
            {
                sharedMemory = value;
            }
        }

        public IHttpClient HttpClient
        {
            get
            {
                return httpClient;
            }
        }

        protected ApiBaseParent( IHttpClient inHttpClient
            , ISession inSession
            , string inBaseUrl )
        {
            Assert.IsNotNull(inHttpClient, $"Creating {GetType().Name} failed. " +
                $"Parameter `inHttpClient` is null");
            
            Assert.IsNotNull(inSession, $"Creating {GetType().Name} failed. " +
                $"Parameter `inSession` is null");
            
            Assert.IsNotNull(inBaseUrl, $"Creating {GetType().Name} failed. " +
                $"Parameter `inBaseUrl` is null");
            
            httpClient = inHttpClient;
            Session = inSession;
            BaseUrl = inBaseUrl;
        }
        #endregion /Constructor
        
        #region Child Constructor


        // /// <summary>Sets BaseUrl and Asserts IsNotNull</summary>
        // protected void SetBaseUrl(string _baseUrl, string _childTypeName)
        // {
        //     this.BaseUrl = _baseUrl;
        //     Assert.IsNotNull(BaseUrl, $"Creating {_childTypeName} failed. Parameter baseUrl is null");
        // }
        #endregion /Child Constructor
        
        #region High-Level Shortcuts
        /// <summary>
        /// Wrapper for Session.AuthorizationToken
        /// - Sometimes called AccessToken
        /// </summary>
        protected internal string AuthToken => Session.AuthorizationToken;
        #endregion /High-Level Shortcuts
        
        internal virtual void SetSharedMemory(ApiSharedMemory newSharedMemory)
        {
            SharedMemory = newSharedMemory;
        }
    }
}
