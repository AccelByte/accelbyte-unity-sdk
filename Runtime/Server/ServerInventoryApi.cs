// Copyright (c) 2024 - 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Server.Interface;
using AccelByte.Utils;
using System;

namespace AccelByte.Server
{
    /// <summary>
    /// Provide function to let Inventory service wrapper to connect to endpoint.
    /// </summary>
    internal class ServerInventoryApi : ServerApiBase
    {
        [UnityEngine.Scripting.Preserve]
        internal ServerInventoryApi(IHttpClient inHttpClient
            , ServerConfig inServerConfig
            , ISession inSession) : base(inHttpClient, inServerConfig, inServerConfig.InventoryServerUrl, inSession)
        {
        }

        public void BulkDeleteUserInventoryItems(string inventoryId
            , string userId
            , BulkDeleteUserInventoryItemsPayload[] payload
            , ResultCallback<DeleteUserInventoryItemResponse[]> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken, Namespace_, inventoryId, userId, payload);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var builder = HttpRequestBuilder.CreateDelete(BaseUrl + "/v1/admin/namespaces/{namespace}/users/" +
                "{userId}/inventories/{inventoryId}/items")
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .WithContentType(MediaType.ApplicationJson)
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithPathParam("inventoryId", inventoryId)
                .WithBody(payload.ToUtf8Json());

            var httpRequest = builder.GetResult();

            HttpOperator.SendRequest(httpRequest, response =>
            {
                var result = response.TryParseJson<DeleteUserInventoryItemResponse[]>();

                if (result.IsError)
                {
                    callback?.TryError(result.Error);
                    return;
                }
                callback?.Invoke(result);
            });
        }

        public void BulkUpdateUserInventoryItems(string inventoryId
            , string userId
            , ServerUpdateUserInventoryItemPayload[] payload
            , ResultCallback<UpdateUserInventoryItemResponse[]> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken, Namespace_, inventoryId, userId, payload);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var builder = HttpRequestBuilder.CreatePut(BaseUrl + "/v1/admin/namespaces/{namespace}/users/" +
                "{userId}/inventories/{inventoryId}/items")
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .WithContentType(MediaType.ApplicationJson)
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithPathParam("inventoryId", inventoryId)
                .WithBody(payload.ToUtf8Json());

            var httpRequest = builder.GetResult();

            HttpOperator.SendRequest(httpRequest, response =>
            {
                var result = response.TryParseJson<UpdateUserInventoryItemResponse[]>();

                if (result.IsError)
                {
                    callback?.TryError(result.Error);
                    return;
                }
                callback?.Invoke(result);
            });
        }

        public void ConsumeUserInventoryItem(string inventoryId, string userId, ConsumeUserItemsRequest request, ResultCallback<UserItem> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken, Namespace_, inventoryId, userId, request);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var builder = HttpRequestBuilder.CreatePost(BaseUrl + "/v1/admin/namespaces/{namespace}/users/" +
                "{userId}/inventories/{inventoryId}/consume")
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .WithContentType(MediaType.ApplicationJson)
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithPathParam("inventoryId", inventoryId)
                .WithBody(request.ToUtf8Json());

            var httpRequest = builder.GetResult();

            HttpOperator.SendRequest(httpRequest, response =>
            {
                var result = response.TryParseJson<UserItem>();

                if (result.IsError)
                {
                    callback?.TryError(result.Error);
                    return;
                }
                callback?.Invoke(result);
            });
        }

        public void CreateIntegrationConfiguration(ServerCreateIntegrationConfigurationRequest request, ResultCallback<ServerIntegrationConfiguration> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken, Namespace_, request);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var builder = HttpRequestBuilder.CreatePost(BaseUrl + "/v1/admin/namespaces/{namespace}/integrationConfigurations")
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .WithContentType(MediaType.ApplicationJson)
                .WithPathParam("namespace", Namespace_)
                .WithBody(request.ToUtf8Json());

            var httpRequest = builder.GetResult();

            HttpOperator.SendRequest(httpRequest, response =>
            {
                var result = response.TryParseJson<ServerIntegrationConfiguration>();

                if (result.IsError)
                {
                    callback?.TryError(result.Error);
                    return;
                }
                callback?.Invoke(result);
            });
        }

        public void CreateInventoryConfiguration(ServerCreateInventoryConfigurationRequest request, ResultCallback<InventoryConfiguration> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken, Namespace_, request, request.Code);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var builder = HttpRequestBuilder.CreatePost(BaseUrl + "/v1/admin/namespaces/{namespace}/inventoryConfigurations")
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .WithContentType(MediaType.ApplicationJson)
                .WithPathParam("namespace", Namespace_)
                .WithBody(request.ToUtf8Json());

            var httpRequest = builder.GetResult();

            HttpOperator.SendRequest(httpRequest, response =>
            {
                var result = response.TryParseJson<InventoryConfiguration>();

                if (result.IsError)
                {
                    callback?.TryError(result.Error);
                    return;
                }
                callback?.Invoke(result);
            });
        }

        public void CreateInventoryTag(ServerCreateInventoryTagRequest request, ResultCallback<InventoryTag> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken, Namespace_, request, request.Name, request.Owner);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var builder = HttpRequestBuilder.CreatePost(BaseUrl + "/v1/admin/namespaces/{namespace}/tags")
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .WithContentType(MediaType.ApplicationJson)
                .WithPathParam("namespace", Namespace_)
                .WithBody(request.ToUtf8Json());

            var httpRequest = builder.GetResult();

            HttpOperator.SendRequest(httpRequest, response =>
            {
                var result = response.TryParseJson<InventoryTag>();

                if (result.IsError)
                {
                    callback?.TryError(result.Error);
                    return;
                }
                callback?.Invoke(result);
            });
        }

        public void CreateItemTypes(ServerCreateItemTypeRequest request, ResultCallback<InventoryItemType> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken, Namespace_, request, request.Name);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var builder = HttpRequestBuilder.CreatePost(BaseUrl + "/v1/admin/namespaces/{namespace}/itemtypes")
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .WithContentType(MediaType.ApplicationJson)
                .WithPathParam("namespace", Namespace_)
                .WithBody(request.ToUtf8Json());

            var httpRequest = builder.GetResult();

            HttpOperator.SendRequest(httpRequest, response =>
            {
                var result = response.TryParseJson<InventoryItemType>();

                if (result.IsError)
                {
                    callback?.TryError(result.Error);
                    return;
                }
                callback?.Invoke(result);
            });
        }

        public void CreateUserInventory(ServerCreateInventoryRequest request, ResultCallback<UserInventory> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken, Namespace_, request);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var builder = HttpRequestBuilder.CreatePost(BaseUrl + "/v1/admin/namespaces/{namespace}/inventories")
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .WithContentType(MediaType.ApplicationJson)
                .WithPathParam("namespace", Namespace_)
                .WithBody(request.ToUtf8Json());

            var httpRequest = builder.GetResult();

            HttpOperator.SendRequest(httpRequest, response =>
            {
                var result = response.TryParseJson<UserInventory>();

                if (result.IsError)
                {
                    callback?.TryError(result.Error);
                    return;
                }
                callback?.Invoke(result);
            });
        }

        public void DeleteInventoryConfiguration(string inventoryConfigurationId, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken, Namespace_, inventoryConfigurationId);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var builder = HttpRequestBuilder.CreateDelete(BaseUrl + "/v1/admin/namespaces/{namespace}/" +
                "inventoryConfigurations/{inventoryConfigurationId}")
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("inventoryConfigurationId", inventoryConfigurationId);

            var httpRequest = builder.GetResult();

            HttpOperator.SendRequest(httpRequest, response =>
            {
                var result = response.TryParse();

                if (result.IsError)
                {
                    callback?.TryError(result.Error);
                    return;
                }
                callback?.Invoke(result);
            });
        }

        public void DeleteInventoryTag(string tagName, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken, Namespace_, tagName);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var builder = HttpRequestBuilder.CreateDelete(BaseUrl + "/v1/admin/namespaces/{namespace}/" +
                "tags/{tagName}")
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("tagName", tagName);

            var httpRequest = builder.GetResult();

            HttpOperator.SendRequest(httpRequest, response =>
            {
                var result = response.TryParse();

                if (result.IsError)
                {
                    callback?.TryError(result.Error);
                    return;
                }
                callback?.Invoke(result);
            });
        }

        public void DeleteItemTypes(string itemTypeName, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken, Namespace_, itemTypeName);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var builder = HttpRequestBuilder.CreateDelete(BaseUrl + "/v1/admin/namespaces/{namespace}/itemtypes/{itemTypeName}")
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("itemTypeName", itemTypeName);

            var httpRequest = builder.GetResult();

            HttpOperator.SendRequest(httpRequest, response =>
            {
                var result = response.TryParse();

                if (result.IsError)
                {
                    callback?.TryError(result.Error);
                    return;
                }
                callback?.Invoke(result);
            });
        }

        public void DeleteUserInventory(string inventoryId, ServerDeleteInventoryRequest request, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken, Namespace_, inventoryId, request);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var builder = HttpRequestBuilder.CreateDelete(BaseUrl + "/v1/admin/namespaces/{namespace}/inventories/{inventoryId}")
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .WithContentType(MediaType.ApplicationJson)
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("inventoryId", inventoryId)
                .WithBody(request.ToUtf8Json());

            var httpRequest = builder.GetResult();

            HttpOperator.SendRequest(httpRequest, response =>
            {
                var result = response.TryParse();

                if (result.IsError)
                {
                    callback?.TryError(result.Error);
                    return;
                }
                callback?.Invoke(result);
            });
        }

        public void GetAllInventoryConfigurations(ResultCallback<InventoryConfigurationsPagingResponse> callback, InventoryConfigurationSortBy sortBy = InventoryConfigurationSortBy.CreatedAt, int limit = 25, int offset = 0, string inventoryConfigurationCode = "")
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken, Namespace_);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var builder = HttpRequestBuilder.CreateGet(BaseUrl + "/v1/admin/namespaces/{namespace}/inventoryConfigurations")
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .WithPathParam("namespace", Namespace_)
                .WithQueryParam("sortBy", ConverterUtils.EnumToDescription(sortBy))
                .WithQueryParam("limit", limit > 0 ? limit.ToString() : string.Empty)
                .WithQueryParam("offset", offset > 0 ? offset.ToString() : string.Empty)
                .WithQueryParam("code", inventoryConfigurationCode);

            var httpRequest = builder.GetResult();

            HttpOperator.SendRequest(httpRequest, response =>
            {
                var result = response.TryParseJson<InventoryConfigurationsPagingResponse>();

                if (result.IsError)
                {
                    callback?.TryError(result.Error);
                    return;
                }
                callback?.Invoke(result);
            });
        }

        public void GetIntegrationConfigurations(ResultCallback<ServerIntegrationConfigurationsPagingResponse> callback, ServerIntegrationConfigurationSortBy sortBy = ServerIntegrationConfigurationSortBy.CreatedAt, int limit = 25, int offset = 0)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken, Namespace_);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var builder = HttpRequestBuilder.CreateGet(BaseUrl + "/v1/admin/namespaces/{namespace}/integrationConfigurations")
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .WithPathParam("namespace", Namespace_)
                .WithQueryParam("sortBy", ConverterUtils.EnumToDescription(sortBy))
                .WithQueryParam("limit", limit > 0 ? limit.ToString() : string.Empty)
                .WithQueryParam("offset", offset > 0 ? offset.ToString() : string.Empty);

            var httpRequest = builder.GetResult();

            HttpOperator.SendRequest(httpRequest, response =>
            {
                var result = response.TryParseJson<ServerIntegrationConfigurationsPagingResponse>();

                if (result.IsError)
                {
                    callback?.TryError(result.Error);
                    return;
                }
                callback?.Invoke(result);
            });
        }

        public void GetInventoryConfiguration(string inventoryConfigurationId, ResultCallback<InventoryConfiguration> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken, Namespace_, inventoryConfigurationId);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var builder = HttpRequestBuilder.CreateGet(BaseUrl + "/v1/admin/namespaces/{namespace}/inventoryConfigurations/" +
                "{inventoryConfigurationId}")
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("inventoryConfigurationId", inventoryConfigurationId);

            var httpRequest = builder.GetResult();

            HttpOperator.SendRequest(httpRequest, response =>
            {
                var result = response.TryParseJson<InventoryConfiguration>();

                if (result.IsError)
                {
                    callback?.TryError(result.Error);
                    return;
                }
                callback?.Invoke(result);
            });
        }

        public void GetInventoryTags(ResultCallback<InventoryTagsPagingResponse> callback, ItemTypeSortBy sortBy = ItemTypeSortBy.CreatedAt, int limit = 25, int offset = 0)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken, Namespace_);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var builder = HttpRequestBuilder.CreateGet(BaseUrl + "/v1/admin/namespaces/{namespace}/tags")
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .WithPathParam("namespace", Namespace_)
                .WithQueryParam("sortBy", ConverterUtils.EnumToDescription(sortBy))
                .WithQueryParam("limit", limit > 0 ? limit.ToString() : string.Empty)
                .WithQueryParam("offset", offset > 0 ? offset.ToString() : string.Empty);

            var httpRequest = builder.GetResult();

            HttpOperator.SendRequest(httpRequest, response =>
            {
                var result = response.TryParseJson<InventoryTagsPagingResponse>();

                if (result.IsError)
                {
                    callback?.TryError(result.Error);
                    return;
                }
                callback?.Invoke(result);
            });
        }

        public void GetItemTypes(ResultCallback<ItemTypesPagingResponse> callback, ItemTypeSortBy sortBy = ItemTypeSortBy.CreatedAt, int limit = 25, int offset = 0)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken, Namespace_);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var builder = HttpRequestBuilder.CreateGet(BaseUrl + "/v1/admin/namespaces/{namespace}/itemtypes")
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .WithPathParam("namespace", Namespace_)
                .WithQueryParam("sortBy", ConverterUtils.EnumToDescription(sortBy))
                .WithQueryParam("limit", limit > 0 ? limit.ToString() : string.Empty)
                .WithQueryParam("offset", offset > 0 ? offset.ToString() : string.Empty);

            var httpRequest = builder.GetResult();

            HttpOperator.SendRequest(httpRequest, response =>
            {
                var result = response.TryParseJson<ItemTypesPagingResponse>();

                if (result.IsError)
                {
                    callback?.TryError(result.Error);
                    return;
                }
                callback?.Invoke(result);
            });
        }

        public void GetUserInventories(ResultCallback<UserInventoriesPagingResponse> callback, UserInventorySortBy sortBy = UserInventorySortBy.CreatedAt, int limit = 25, int offset = 0, string inventoryConfigurationCode = "", string userId = "")
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken, Namespace_);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var builder = HttpRequestBuilder.CreateGet(BaseUrl + "/v1/admin/namespaces/{namespace}/inventories")
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .WithPathParam("namespace", Namespace_)
                .WithQueryParam("userId", userId)
                .WithQueryParam("inventoryConfigurationCode", inventoryConfigurationCode)
                .WithQueryParam("limit", limit > 0 ? limit.ToString() : string.Empty)
                .WithQueryParam("offset", offset > 0 ? offset.ToString() : string.Empty)
                .WithQueryParam("sortBy", ConverterUtils.EnumToDescription(sortBy));

            var httpRequest = builder.GetResult();

            HttpOperator.SendRequest(httpRequest, response =>
            {
                var result = response.TryParseJson<UserInventoriesPagingResponse>();

                if (result.IsError)
                {
                    callback?.TryError(result.Error);
                    return;
                }
                callback?.Invoke(result);
            });
        }

        public void GetUserInventory(string inventoryId, ResultCallback<UserInventory> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken, Namespace_, inventoryId);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var builder = HttpRequestBuilder.CreateGet(BaseUrl + "/v1/admin/namespaces/{namespace}/inventories/{inventoryId}")
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("inventoryId", inventoryId);

            var httpRequest = builder.GetResult();

            HttpOperator.SendRequest(httpRequest, response =>
            {
                var result = response.TryParseJson<UserInventory>();

                if (result.IsError)
                {
                    callback?.TryError(result.Error);
                    return;
                }
                callback?.Invoke(result);
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

            var builder = HttpRequestBuilder.CreateGet(BaseUrl + "/v1/admin/namespaces/{namespace}/inventories/{inventoryId}/items")
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

            var httpRequest = builder.GetResult();

            HttpOperator.SendRequest(httpRequest, response =>
            {
                var result = response.TryParseJson<UserItemsPagingResponse>();

                if (result.IsError)
                {
                    callback?.TryError(result.Error);
                    return;
                }
                callback?.Invoke(result);
            });
        }

        public void GetUserInventoryItem(string inventoryId, string slotId, string sourceItemId, ResultCallback<UserItem> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken, Namespace_, inventoryId);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var builder = HttpRequestBuilder.CreateGet(BaseUrl + "/v1/admin/namespaces/{namespace}/inventories/" +
                "{inventoryId}/slots/{slotId}/sourceItems/{sourceItemId}")
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("inventoryId", inventoryId)
                .WithPathParam("slotId", slotId)
                .WithPathParam("sourceItemId", sourceItemId);

            var httpRequest = builder.GetResult();

            HttpOperator.SendRequest(httpRequest, response =>
            {
                var result = response.TryParseJson<UserItem>();

                if (result.IsError)
                {
                    callback?.TryError(result.Error);
                    return;
                }
                callback?.Invoke(result);
            });
        }

        public void RunChainingOperation(ServerInventoryChainingOperationRequest request, ResultCallback<InventoryChainingOperationResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken, Namespace_, request);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var builder = HttpRequestBuilder.CreatePost(BaseUrl + "/v1/admin/namespaces/{namespace}/chainingOperations")
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .WithContentType(MediaType.ApplicationJson)
                .WithPathParam("namespace", Namespace_)
                .WithBody(request.ToUtf8Json());

            var httpRequest = builder.GetResult();

            HttpOperator.SendRequest(httpRequest, response =>
            {
                var result = response.TryParseJson<InventoryChainingOperationResponse>();

                if (result.IsError)
                {
                    callback?.TryError(result.Error);
                    return;
                }
                callback?.Invoke(result);
            });
        }

        public void SaveUserInventoryItem(string userId
            , string inventoryConfigurationCode
            , string source
            , string sourceItemId
            , string type
            , uint quantity
            , SaveUserInventoryItemOptionalParameters optionalParameters
            , ResultCallback<UserItem> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken, Namespace_, userId, inventoryConfigurationCode, source, sourceItemId, type);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }
            
            var request = new ServerSaveUserInventoryItemToUserPayload
            {
                InventoryConfigurationCode = inventoryConfigurationCode,
                Source = source,
                SourceItemId = sourceItemId,
                Type = type,
                Quantity = (int)quantity
            };

            if (optionalParameters != null)
            {
                if (optionalParameters.Tags != null)
                {
                    request.Tags = optionalParameters.Tags;
                }

                if (!string.IsNullOrEmpty(optionalParameters.SlotId))
                {
                    request.SlotId = optionalParameters.SlotId;
                }

                if (optionalParameters.SlotUsed > 0)
                {
                    request.SlotUsed = optionalParameters.SlotUsed;
                }

                if (optionalParameters.CustomAttributes != null)
                {
                    request.CustomAttributes = optionalParameters.CustomAttributes;
                }

                if (optionalParameters.ServerCustomAttributes != null)
                {
                    request.ServerCustomAttributes = optionalParameters.ServerCustomAttributes;
                }
            }

            var builder = HttpRequestBuilder.CreatePost(BaseUrl + "/v1/admin/namespaces/{namespace}/users/{userId}/items")
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .WithContentType(MediaType.ApplicationJson)
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBody(request.ToUtf8Json());

            var httpRequest = builder.GetResult();

            HttpOperator.SendRequest(httpRequest, response =>
            {
                var result = response.TryParseJson<UserItem>();

                if (result.IsError)
                {
                    callback?.TryError(result.Error);
                    return;
                }
                callback?.Invoke(result);
            });
        }

        public void SaveUserInventoryItemToInventory(string inventoryId, string userId, ServerSaveUserInventoryItemRequest request, ResultCallback<UserItem> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken, Namespace_, inventoryId, userId, request);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var builder = HttpRequestBuilder.CreatePost(BaseUrl + "/v1/admin/namespaces/{namespace}/users/{userId}/" +
                "inventories/{inventoryId}/items")
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .WithContentType(MediaType.ApplicationJson)
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("inventoryId", inventoryId)
                .WithPathParam("userId", userId)
                .WithBody(request.ToUtf8Json());

            var httpRequest = builder.GetResult();

            HttpOperator.SendRequest(httpRequest, response =>
            {
                var result = response.TryParseJson<UserItem>();

                if (result.IsError)
                {
                    callback?.TryError(result.Error);
                    return;
                }
                callback?.Invoke(result);
            });
        }

        public void SyncUserEntitlements(string userId, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken, Namespace_, userId);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var builder = HttpRequestBuilder.CreatePut(BaseUrl + "/v1/admin/namespaces/{namespace}/users/{userId}/" +
                "items/entitlements/sync")
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId);

            var httpRequest = builder.GetResult();

            HttpOperator.SendRequest(httpRequest, response =>
            {
                var result = response.TryParse();

                if (result.IsError)
                {
                    callback?.TryError(result.Error);
                    return;
                }
                callback?.Invoke(result);
            });
        }

        public void UpdateIntegrationConfiguration(string integrationConfigurationId, ServerUpdateIntegrationConfigurationRequest request, ResultCallback<ServerIntegrationConfiguration> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken, Namespace_, integrationConfigurationId, request);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var builder = HttpRequestBuilder.CreatePut(BaseUrl + "/v1/admin/namespaces/{namespace}/integrationConfigurations/" +
                "{integrationConfigurationId}")
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .WithContentType(MediaType.ApplicationJson)
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("integrationConfigurationId", integrationConfigurationId)
                .WithBody(request.ToUtf8Json());

            var httpRequest = builder.GetResult();

            HttpOperator.SendRequest(httpRequest, response =>
            {
                var result = response.TryParseJson<ServerIntegrationConfiguration>();

                if (result.IsError)
                {
                    callback?.TryError(result.Error);
                    return;
                }
                callback?.Invoke(result);
            });
        }

        public void UpdateIntegrationConfigurationStatus(string integrationConfigurationId, ServerUpdateIntegrationConfigurationStatusRequest request, ResultCallback<ServerIntegrationConfiguration> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken, Namespace_, integrationConfigurationId, request);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var builder = HttpRequestBuilder.CreatePut(BaseUrl + "/v1/admin/namespaces/{namespace}/integrationConfigurations/" +
                "{integrationConfigurationId}/status")
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .WithContentType(MediaType.ApplicationJson)
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("integrationConfigurationId", integrationConfigurationId)
                .WithBody(request.ToUtf8Json());

            var httpRequest = builder.GetResult();

            HttpOperator.SendRequest(httpRequest, response =>
            {
                var result = response.TryParseJson<ServerIntegrationConfiguration>();

                if (result.IsError)
                {
                    callback?.TryError(result.Error);
                    return;
                }
                callback?.Invoke(result);
            });
        }

        public void UpdateInventoryConfiguration(string inventoryConfigurationId, ServerUpdateInventoryConfigurationRequest request, ResultCallback<InventoryConfiguration> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken, Namespace_, inventoryConfigurationId, request);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var builder = HttpRequestBuilder.CreatePut(BaseUrl + "/v1/admin/namespaces/{namespace}/inventoryConfigurations/" +
                "{inventoryConfigurationId}")
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .WithContentType(MediaType.ApplicationJson)
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("inventoryConfigurationId", inventoryConfigurationId)
                .WithBody(request.ToUtf8Json());

            var httpRequest = builder.GetResult();

            HttpOperator.SendRequest(httpRequest, response =>
            {
                var result = response.TryParseJson<InventoryConfiguration>();

                if (result.IsError)
                {
                    callback?.TryError(result.Error);
                    return;
                }
                callback?.Invoke(result);
            });
        }

        public void UpdateUserInventory(string inventoryId, ServerUpdateInventoryRequest request, ResultCallback<UserInventory> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken, Namespace_, inventoryId, request);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var builder = HttpRequestBuilder.CreatePut(BaseUrl + "/v1/admin/namespaces/{namespace}/inventories/{inventoryId}")
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .WithContentType(MediaType.ApplicationJson)
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("inventoryId", inventoryId)
                .WithBody(request.ToUtf8Json());

            var httpRequest = builder.GetResult();

            HttpOperator.SendRequest(httpRequest, response =>
            {
                var result = response.TryParseJson<UserInventory>();

                if (result.IsError)
                {
                    callback?.TryError(result.Error);
                    return;
                }
                callback?.Invoke(result);
            });
        }

        public void ValidateUserInventoryCapacity(string userId, ServerValidateUserInventoryCapacityRequest request, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken, Namespace_, userId, request);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var builder = HttpRequestBuilder.CreatePost(BaseUrl + "/v1/admin/namespaces/{namespace}/users/{userId}/purchaseable")
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .WithContentType(MediaType.ApplicationJson)
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBody(request.ToUtf8Json());

            var httpRequest = builder.GetResult();

            HttpOperator.SendRequest(httpRequest, response =>
            {
                var result = response.TryParse();

                if (result.IsError)
                {
                    callback?.TryError(result.Error);
                    return;
                }
                callback?.Invoke(result);
            });
        }
    }
}