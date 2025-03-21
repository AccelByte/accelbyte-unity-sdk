﻿// Copyright (c) 2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Core
{
    /// <summary>
    /// Inherit from this class to set common Client Api functionality.
    /// - Used for Api modules, such as "GroupApi".
    /// </summary>
    public class ApiBase : ApiBaseParent
    {
        #region Constructor
        /// <summary>
        /// Contains namespace, baseUrl's
        /// TODO: Do we want this private, then just make protected Getters() for Config.x?
        /// </summary>
        internal readonly Config Config;
        internal readonly HttpOperator HttpOperator;

        protected ApiBase( IHttpClient inHttpClient
            , Config inConfig
            , string inBaseUrl
            , ISession inSession
            , HttpOperator httpOperator = null)
            : base( inHttpClient, inSession, inBaseUrl )
        {
            Assert.IsNotNull(inConfig, $"Creating {GetType().Name} failed. " +
                "Parameter `inConfig` is null");
            Config = inConfig;
            this.HttpOperator = httpOperator != null ? httpOperator : HttpOperator.CreateDefault(inHttpClient);
        }
        #endregion /Constructor

        
        #region High-Level Shortcuts
        /// <summary>Config.Namespace wrapper</summary>
        protected internal string Namespace_ => Config.Namespace;
        #endregion /High-Level Shortcuts
    }
}
