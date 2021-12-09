// Copyright (c) 2021 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections.Generic;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    /// <summary>
    /// Provide APIs to access UGC service.
    /// </summary>
    public class UGC
    {
        private readonly UGCApi api;
        private readonly ISession session;
        private readonly string @namespace;
        private readonly CoroutineRunner coroutineRunner;

        internal UGC(UGCApi api, ISession session, string @namespace, CoroutineRunner coroutineRunner)
        {
            Assert.IsNotNull(api, "api parameter can not be null. Construction is failed.");
            Assert.IsNotNull(session, "session parameter can not be null. Construction is failed.");
            Assert.IsFalse(string.IsNullOrEmpty(@namespace), "namespace paramater can not be empty. Construction is failed.");
            Assert.IsNotNull(coroutineRunner, "coroutineRunner parameter can not be null. Construction is failed.");

            this.api = api;
            this.session = session;
            this.@namespace = @namespace;
            this.coroutineRunner = coroutineRunner;
        }

        /// <summary>
        /// Create a content with string preview and get the payload url to upload the content.
        /// </summary>
        /// <param name="channelId">The id of the content's channel.</param>
        /// <param name="createRequest">Detail information for the content request.</param>
        /// <param name="callback">This will be called when the operation succeeded. The result is UGCResponse Model.</param>
        public void CreateContent(string channelId, UGCRequest createRequest, ResultCallback<UGCResponse> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.CreateContent(this.@namespace, this.session.UserId, this.session.AuthorizationToken, channelId, createRequest, callback));
        }

        /// <summary>
        /// Create a content with byte array Preview paramater and get the payload url to upload the content.
        /// </summary>
        /// <param name="channelId">The id of the content's channel.</param>
        /// <param name="name">Name of the content.</param>
        /// <param name="type">Type of the content.</param>
        /// <param name="subtype">SubType of the content.</param>
        /// <param name="tags">Tags of the content.</param>
        /// <param name="preview">Preview the byte array of the content's preview.</param>
        /// <param name="fileExtension">FileExtension of the content</param>
        /// <param name="callback">This will be called when the operation succeeded. The result is UGCResponse Model.</param>
        /// <param name="contentType">The specific type of the content's created. default value is "application/octet-stream"</param>
        public void CreateContent(string channelId, string name, string type, string subtype, string[] tags, byte[] preview, string fileExtension,
            ResultCallback<UGCResponse> callback, string contentType = "application/octet-stream")
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.CreateContent(this.@namespace, this.session.UserId, this.session.AuthorizationToken, channelId, name, type, subtype,
                    tags, preview, fileExtension, callback, contentType));
        }

        /// <summary>
        /// Modify existing content to update some information with string preview.
        /// </summary>
        /// <param name="channelId">The id of the content's channel.</param>
        /// <param name="contentId">The id of the content that will be modified.</param>
        /// <param name="ModifyRequest">Detail information for the content request that will be modified.</param>
        /// <param name="callback">This will be called when the operation succeeded. The result is UGCResponse Model.</param>
        public void ModifyContent(string channelId, string contentId, UGCRequest ModifyRequest, ResultCallback<UGCResponse> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.ModifyContent(this.@namespace, this.session.UserId, this.session.AuthorizationToken, channelId, contentId, ModifyRequest, callback));
        }

        /// <summary>
        /// Modify existing content to update some information with byte array Preview paramater.
        /// </summary>
        /// <param name="channelId">The id of the content's channel.</param>
        /// <param name="contentId">The id of the content that will be modified.</param>
        /// <param name="name">Name of the content</param>
        /// <param name="type">Type of the content</param>
        /// <param name="subtype">SubType of the content.</param>
        /// <param name="tags">Tags of the content.</param>
        /// <param name="preview">The byte array of the content's Preview</param>
        /// <param name="fileExtension">FileExtension of the content</param>
        /// <param name="callback">This will be called when the operation succeeded. The result is UGCResponse Model.</param>
        /// <param name="contentType">The specific type of the content's modified. Default value is "application/octet-stream"</param>
        public void ModifyContent(string channelId, string contentId, string name, string type, string subtype, string[] tags, byte[] preview, string fileExtension,
            ResultCallback<UGCResponse> callback, string contentType = "application/octet-stream")
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.ModifyContent(this.@namespace, this.session.UserId, this.session.AuthorizationToken, channelId, contentId, name, type, subtype,
                    tags, preview, fileExtension, callback, contentType));
        }

        /// <summary>
        /// Delete a content based on the its channel id and content id.
        /// </summary>
        /// <param name="channelId">The id of the content's channel.</param>
        /// <param name="contentId">The id of the content that will be deleted.</param>
        /// <param name="callback">This will be called when the operation succeeded.</param>
        public void DeleteContent(string channelId, string contentId, ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.DeleteContent(this.@namespace, this.session.UserId, this.session.AuthorizationToken, channelId, contentId, callback));
        }

        /// <summary>
        /// Get a content information by its content id
        /// </summary>
        /// <param name="contentId">The id of the content that will be fetched.</param>
        /// <param name="callback">This will be called when the operation succeeded. The result is UGCContentResponse Model.</param>
        public void GetContentByContentId(string contentId, ResultCallback<UGCContentResponse> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.GetContentByContentId(this.@namespace, this.session.UserId, this.session.AuthorizationToken, contentId, callback));
        }

        /// <summary>
        /// Get a content information by its share code.
        /// </summary>
        /// <param name="shareCode">The share code of the content that will be fetched.</param>
        /// <param name="callback">This will be called when the operation succeeded. The result is UGCContentResponse Model.</param>
        public void GetContentByShareCode(string shareCode, ResultCallback<UGCContentResponse> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.GetContentByShareCode(this.@namespace, this.session.AuthorizationToken, shareCode, callback));
        }

        /// <summary>
        /// Get content preview as UGCPreview Model.
        /// </summary>
        /// <param name="contentId">The id of the Preview's content that will be fetched.</param>
        /// <param name="callback">This will be called when the operation succeeded. The result is UGCPreview Model.</param>
        public void GetContentPreview(string contentId, ResultCallback<UGCPreview> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.GetContentPreview(this.@namespace, this.session.UserId, this.session.AuthorizationToken, contentId, callback));
        }

        /// <summary>
        /// Get content preview as byte array.
        /// </summary>
        /// <param name="contentId">The id of the Preview's content that will be fetched.</param>
        /// <param name="callback">This will be called when the operation succeeded. The result is byte array</param>
        public void GetContentPreview(string contentId, ResultCallback<byte[]> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.GetContentPreview(this.@namespace, this.session.UserId, this.session.AuthorizationToken, contentId, callback));
        }

        /// <summary>
        /// Get all tags.
        /// </summary>
        /// <param name="callback">This will be called when the operation succeeded. The result is UGCTagsPagingResponse Model.</param>
        /// <param name="offset">The offset of the tags results. Default value is 0.</param>
        /// <param name="limit">The limit of the tags results. Default value is 1000.</param>
        public void GetTags(ResultCallback<UGCTagsPagingResponse> callback, int offset = 0, int limit = 1000)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.GetTags(this.@namespace, this.session.AuthorizationToken, callback, offset, limit));
        }

        /// <summary>
        /// Get all of player's types and subtypes.
        /// </summary>
        /// <param name="callback">This will be called when the operation succeeded. The result is UGCTypesPagingResponse Model.</param>
        /// <param name="offset">The offset of the types and/or subtypes paging data result. Default value is 0.</param>
        /// <param name="limit">The limit of the types and subtypes results. Default value is 1000.</param>
        public void GetTypes(ResultCallback<UGCTypesPagingResponse> callback, int offset = 0, int limit = 1000)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.GetTypes(this.@namespace, this.session.AuthorizationToken, callback, offset, limit));
        }

        /// <summary>
        /// Create player's channel with specific channel name.
        /// </summary>
        /// <param name="channelName">The name of the channel.</param>
        /// <param name="callback">This will be called when the operation succeeded. The result is UGCChannelResponse Model.</param>
        public void CreateChannel(string channelName, ResultCallback<UGCChannelResponse> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.CreateChannel(this.@namespace, this.session.UserId, this.session.AuthorizationToken, channelName, callback));
        }

        /// <summary>
        /// Get all of the player's channels.
        /// </summary>
        /// <param name="callback">This will be called when the operation succeeded. The result is UGCChannelsPagingResponse Model.</param>
        /// <param name="offset">The offset of the channel results. Default value is 0.</param>
        /// <param name="limit">The limit of the channel results. Default value is 1000.</param>
        public void GetChannels(ResultCallback<UGCChannelPagingResponse> callback, int offset = 0, int limit = 1000)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.GetChannels(this.@namespace, this.session.UserId, this.session.AuthorizationToken, callback, offset, limit));
        }

        /// <summary>
        /// Delete player's channel based on the its channel id.
        /// </summary>
        /// <param name="channelId">The id of the channel that will be deleted.</param>
        /// <param name="callback">This will be called when the operation succeeded.</param>
        public void DeleteChannel(string channelId, ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.DeleteChannel(this.@namespace, this.session.UserId, this.session.AuthorizationToken, channelId, callback));
        }
    }
}
