// Copyright (c) 2021 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using AccelByte.Api;
using UnityEngine.Assertions;

namespace AccelByte.Core
{
    public class EntitlementTokenGenerator : ITokenGenerator
    {
        public string Token { get; set; }

        public event Action<string> TokenReceivedEvent;

        private readonly Entitlement entitlement;

        private DateTime lastSuccessDateTime;
        private DateTime expiredDateTime;

        private string[] itemIds;
        private string[] appIds;
        private string[] skus;

        public EntitlementTokenGenerator(string[] itemIds, string[] appIds, string[] skus)
        {
            Assert.IsFalse(itemIds == null && appIds == null && skus == null, "Can't create entitlement token generator! all itemIds, appIds and skus parameters are null!");

            this.entitlement = AccelByteSDK.GetClientRegistry().GetApi().GetEntitlement();

            this.itemIds = itemIds;
            this.appIds = appIds;
            this.skus = skus;
        }

        /// <summary>
        /// Request token to the entitlement and will use old token if it still valid.
        /// </summary>
        public void RequestToken()
        {
            if (DateTime.UtcNow < expiredDateTime)
            {
                TokenReceivedEvent.Invoke(this.Token);

                return;
            }

            entitlement.GetUserEntitlementOwnershipTokenOnly(this.itemIds, this.appIds, this.skus,
                    result =>
                    {
                        if (result == null || result.IsError)
                        {
                            return;
                        }

                        this.Token = result.Value.ownershipToken;

                        UpdateExpiration(this.Token);

                        TokenReceivedEvent.Invoke(this.Token);
                    });
        }

        /// <summary>
        /// Check token expiration and token validation.
        /// </summary>
        /// <returns> Return true if token is not expired and token is valid. </returns>
        public bool IsValid()
        {
            return !string.IsNullOrEmpty(this.Token) && DateTime.UtcNow < this.expiredDateTime;
        }

        private void UpdateExpiration(string token)
        {
            this.lastSuccessDateTime = DateTime.UtcNow;

            if (JsonWebToken.TryDecodeExpiration(token, out var expired))
            {
                this.expiredDateTime = this.lastSuccessDateTime.AddSeconds(expired - 30f);
            }
        }
    }
}