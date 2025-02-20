// Copyright (c) 2024 - 2025 AccelByte Inc.All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Utils;
using System;

namespace AccelByte.Api
{
    internal class InventoryApi : ApiBase
    {
        [UnityEngine.Scripting.Preserve]
        internal InventoryApi(
            IHttpClient inHttpClient
            , Config inConfig
            , ISession inSession)
            : base(inHttpClient, inConfig, inConfig.InventoryServerUrl, inSession)
        {
        }

        public void BulkDeleteInventoryItems(
            string inventoryId
            , DeleteUserInventoryItemRequest[] deletedItemsRequest
            , ResultCallback<DeleteUserInventoryItemResponse[]> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken, Namespace_, inventoryId, deletedItemsRequest);
            if (error != null)
            {
                callback.TryError(error);
                return;
            }

            var builder = HttpRequestBuilder.CreateDelete(BaseUrl + "/v1/public/namespaces/{namespace}/users/me/" +
                "inventories/{inventoryId}/items")
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .WithContentType(MediaType.ApplicationJson)
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("inventoryId", inventoryId)
                .WithBody(deletedItemsRequest.ToUtf8Json());

            var request = builder.GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<DeleteUserInventoryItemResponse[]>();

                if (result.IsError)
                {
                    callback?.TryError(result.Error);
                    return;
                }

                callback?.TryOk(result.Value);
            });
        }

        public void BulkUpdateInventoryItems(
            string inventoryId
            , UpdateUserInventoryItemRequest[] updatedItemsRequest
            , ResultCallback<UpdateUserInventoryItemResponse[]> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken, Namespace_, inventoryId, updatedItemsRequest);
            if (error != null)
            {
                callback.TryError(error);
                return;
            }

            var builder = HttpRequestBuilder.CreatePut(BaseUrl + "/v1/public/namespaces/{namespace}/users/me/" +
                "inventories/{inventoryId}/items")
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .WithContentType(MediaType.ApplicationJson)
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("inventoryId", inventoryId)
                .WithBody(updatedItemsRequest.ToUtf8Json());

            var request = builder.GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<UpdateUserInventoryItemResponse[]>();

                if (result.IsError)
                {
                    callback?.TryError(result.Error);
                    return;
                }

                callback?.TryOk(result.Value);
            });
        }

        public void ConsumeUserInventoryItem(
            string inventoryId
            , ConsumeUserItemsRequest consumedItemsRequest
            , ResultCallback<UserItem> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken, Namespace_, inventoryId, consumedItemsRequest);
            if (error != null)
            {
                callback.TryError(error);
                return;
            }

            var builder = HttpRequestBuilder.CreatePost(BaseUrl + "/v1/public/namespaces/{namespace}/users/me/" +
                "inventories/{inventoryId}/consume")
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .WithContentType(MediaType.ApplicationJson)
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("inventoryId", inventoryId)
                .WithBody(consumedItemsRequest.ToUtf8Json());

            var request = builder.GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<UserItem>();

                if (result.IsError)
                {
                    callback?.TryError(result.Error);
                    return;
                }

                callback?.TryOk(result.Value);
            });
        }

        public void GetInventoryConfigurations(ResultCallback<InventoryConfigurationsPagingResponse> callback
            , InventoryConfigurationSortBy sortBy = InventoryConfigurationSortBy.CreatedAt
            , int limit = 25
            , int offset = 0
            , string inventoryConfigurationCode = "")
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken, Namespace_);
            if (error != null)
            {
                callback.TryError(error);
                return;
            }

            var builder = HttpRequestBuilder.CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/inventoryConfigurations")
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .WithPathParam("namespace", Namespace_)
                .WithQueryParam("sortBy", ConverterUtils.EnumToDescription(sortBy))
                .WithQueryParam("limit", limit.ToString())
                .WithQueryParam("offset", offset.ToString())
                .WithQueryParam("code", inventoryConfigurationCode);

            var request = builder.GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<InventoryConfigurationsPagingResponse>();

                if (result.IsError)
                {
                    callback?.TryError(result.Error);
                    return;
                }

                callback?.TryOk(result.Value);
            });
        }

        public void GetInventoryTags(ResultCallback<InventoryTagsPagingResponse> callback
            , ItemTypeSortBy sortBy = ItemTypeSortBy.CreatedAt
            , int limit = 25
            , int offset = 0)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken, Namespace_);
            if (error != null)
            {
                callback.TryError(error);
                return;
            }

            var builder = HttpRequestBuilder.CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/tags")
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .WithPathParam("namespace", Namespace_)
                .WithQueryParam("sortBy", ConverterUtils.EnumToDescription(sortBy))
                .WithQueryParam("limit", limit.ToString())
                .WithQueryParam("offset", offset.ToString());

            var request = builder.GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<InventoryTagsPagingResponse>();

                if (result.IsError)
                {
                    callback?.TryError(result.Error);
                    return;
                }

                callback?.TryOk(result.Value);
            });
        }

        public void GetItemTypes(ResultCallback<ItemTypesPagingResponse> callback
            , ItemTypeSortBy sortBy = ItemTypeSortBy.CreatedAt
            , int limit = 25
            , int offset = 0)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken, Namespace_);
            if (error != null)
            {
                callback.TryError(error);
                return;
            }

            var builder = HttpRequestBuilder.CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/itemtypes")
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .WithPathParam("namespace", Namespace_)
                .WithQueryParam("sortBy", ConverterUtils.EnumToDescription(sortBy))
                .WithQueryParam("limit", limit.ToString())
                .WithQueryParam("offset", offset.ToString());

            var request = builder.GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<ItemTypesPagingResponse>();

                if (result.IsError)
                {
                    callback?.TryError(result.Error);
                    return;
                }

                callback?.TryOk(result.Value);
            });
        }

        public void GetUserInventories(ResultCallback<UserInventoriesPagingResponse> callback
            , UserInventorySortBy sortBy = UserInventorySortBy.CreatedAt
            , int limit = 25
            , int offset = 0
            , string inventoryConfigurationCode = "")
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken, Namespace_);
            if (error != null)
            {
                callback.TryError(error);
                return;
            }

            var builder = HttpRequestBuilder.CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/users/me/inventories")
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .WithPathParam("namespace", Namespace_)
                .WithQueryParam("sortBy", ConverterUtils.EnumToDescription(sortBy))
                .WithQueryParam("limit", limit.ToString())
                .WithQueryParam("offset", offset.ToString())
                .WithQueryParam("inventoryConfigurationCode", inventoryConfigurationCode);

            var request = builder.GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<UserInventoriesPagingResponse>();

                if (result.IsError)
                {
                    callback?.TryError(result.Error);
                    return;
                }

                callback?.TryOk(result.Value);
            });
        }

        public void GetUserInventoryAllItems(string inventoryId
            , GetUserInventoryAllItemsOptionalParameters optionalParameters
            , ResultCallback<UserItemsPagingResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken, Namespace_, inventoryId);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var builder = HttpRequestBuilder.CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/users/me/inventories/{inventoryId}/items")
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("inventoryId", inventoryId);

            if (optionalParameters != null)
            {
                UserItemSortBy sortBy = optionalParameters.SortBy;
                builder.WithQueryParam("sortBy", ConverterUtils.EnumToDescription(sortBy));
                
                if (optionalParameters.Limit > 0)
                {
                    builder.WithQueryParam("limit", optionalParameters.Limit.ToString());
                }
                
                builder.WithQueryParam("offset", optionalParameters.Offset.ToString());

                string collectedTags = null;
                if (!string.IsNullOrEmpty(optionalParameters.Tags))
                {
                    collectedTags += optionalParameters.Tags;
                }
                
                if (optionalParameters.TagQueryBuilder != null)
                {
                    collectedTags += optionalParameters.TagQueryBuilder.Build();
                }

                if (!string.IsNullOrEmpty(collectedTags))
                {
                    builder.WithQueryParam("tags", collectedTags);
                }

                if (!string.IsNullOrEmpty(optionalParameters.SourceItemId))
                {
                    builder.WithQueryParam("sourceItemId", optionalParameters.SourceItemId);
                }
            }

            var request = builder.GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<UserItemsPagingResponse>();

                if (result.IsError)
                {
                    callback?.TryError(result.Error);
                    return;
                }

                callback?.TryOk(result.Value);
            });
        }

        public void GetUserInventoryItem(string inventoryId
            , string slotId
            , string sourceItemId
            , ResultCallback<UserItem> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken, Namespace_, inventoryId, slotId, sourceItemId);
            if (error != null)
            {
                callback.TryError(error);
                return;
            }

            var builder = HttpRequestBuilder.CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/users/me/" +
                "inventories/{inventoryId}/slots/{slotId}/sourceItems/{sourceItemId}")
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("inventoryId", inventoryId)
                .WithPathParam("slotId", slotId)
                .WithPathParam("sourceItemId", sourceItemId);

            var request = builder.GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<UserItem>();

                if (result.IsError)
                {
                    callback?.TryError(result.Error);
                    return;
                }

                callback?.TryOk(result.Value);
            });
        }

        public void MoveItemsBetweenInventories(string targetInventoryId
            , MoveUserItemsBetweenInventoriesRequest moveItemsRequest
            , ResultCallback<MoveUserItemsBetweenInventoriesResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken, Namespace_, targetInventoryId, moveItemsRequest);
            if (error != null)
            {
                callback.TryError(error);
                return;
            }

            var builder = HttpRequestBuilder.CreatePost(BaseUrl + "/v1/public/namespaces/{namespace}/users/me/" +
                "inventories/{inventoryId}/items/movement")
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .WithContentType(MediaType.ApplicationJson)
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("inventoryId", targetInventoryId)
                .WithBody(moveItemsRequest.ToUtf8Json());

            var request = builder.GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<MoveUserItemsBetweenInventoriesResponse>();

                if (result.IsError)
                {
                    callback?.TryError(result.Error);
                    return;
                }

                callback?.TryOk(result.Value);
            });
        }
    }
}