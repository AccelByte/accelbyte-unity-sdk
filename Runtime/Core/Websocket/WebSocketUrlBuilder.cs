// Copyright (c) 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Assertions;

namespace AccelByte.Core
{
    public class WebsocketUrlBuilder
    {
        private readonly StringBuilder queryBuilder = new StringBuilder(256);
        private readonly StringBuilder urlBuilder = new StringBuilder(256);
        private string result;

        public static WebsocketUrlBuilder CreateWsUrl(string wsUrl)
        {
            var builder = new WebsocketUrlBuilder();
            
            builder.urlBuilder.Append(wsUrl);

            return builder;
        }

        public string GetResult()
        {
            if (this.queryBuilder.Length > 0)
            {
                this.urlBuilder.Append("?");
                this.urlBuilder.Append(this.queryBuilder);
            }

            this.result = this.urlBuilder.ToString();

            return this.result;
        }

        public WebsocketUrlBuilder WithQueryParam(string key, string value)
        {
            Assert.IsNotNull(key, "query key is null");
            Assert.IsNotNull(value, $"query value is null for key {key}");

            if (string.IsNullOrEmpty(value)) return this;

            if (this.queryBuilder.Length > 0)
            {
                this.queryBuilder.Append("&");
            }

            this.queryBuilder.Append($"{Uri.EscapeDataString(key)}={Uri.EscapeDataString(value)}");

            return this;
        }

        public WebsocketUrlBuilder WithQueryParam(IDictionary<string, string> queriesDict, bool writeNullValue = false)
        {
            Assert.IsNotNull(queriesDict, "query is null");

            foreach (var query in queriesDict)
            {
                if (string.IsNullOrEmpty(query.Value) && !writeNullValue)
                {
                    continue;
                }
                WithQueryParam(query.Key, query.Value);
            }

            return this;
        }
    }
}