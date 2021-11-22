// Copyright (c) 2021 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections.Generic;

namespace AccelByte.Core
{
    public abstract class HttpApiBase
    {
        private IHttpClient http;

        public IHttpClient Http
        {
            get => this.http;
            internal set
            {
                if (this.http != null)
                {
                    throw new InvalidOperationException("Http has been set");
                }

                this.http = value;
            }
        }
    }

    public class HttpApiContainer
    {
        public HttpApiContainer(IHttpClient http)
        {
            this.httpClient = http ?? throw new ArgumentException("Parameter can't be null", nameof(http));
        }

        public T GetApi<T>() where T : HttpApiBase
        {
            if (!this.httpApiMap.TryGetValue(typeof(T), out HttpApiBase api))
            {
                throw new ArgumentException($"The type needs to be configured with Configure<{typeof(T).Name}>.");
            }
            
            return api as T;
        }

        public void ConfigureApi<T>(params object[] args) where T : HttpApiBase
        {
            if (this.httpApiMap.TryGetValue(typeof(T), out HttpApiBase _))
            {
                throw new InvalidOperationException("The type has been configured already");
            }

            var api = (HttpApiBase)Activator.CreateInstance(typeof(T), args);
            
            api.Http = this.httpClient;
            this.httpApiMap[typeof(T)] = api;
        }
        
        private readonly IDictionary<Type, HttpApiBase> httpApiMap = new Dictionary<Type, HttpApiBase>();
        private readonly IHttpClient httpClient;
    }
}