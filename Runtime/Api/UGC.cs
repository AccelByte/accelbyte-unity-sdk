// Copyright (c) 2021 - 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    /// <summary>
    /// Provide APIs to access User-Generated Content (UGC) service.
    /// </summary>
    public class UGC : WrapperBase
    {
        private readonly UGCApi api;
        private readonly UserSession session;
        private readonly CoroutineRunner coroutineRunner;

        [UnityEngine.Scripting.Preserve]
        internal UGC(UGCApi inApi
            , UserSession inSession
            , CoroutineRunner inCoroutineRunner)
        {
            Assert.IsNotNull(inApi, "inApi parameter can not be null. Construction is failed.");
            Assert.IsNotNull(inCoroutineRunner, "inCoroutineRunner parameter can not be null. Construction is failed.");

            api = inApi;
            session = inSession;
            coroutineRunner = inCoroutineRunner;
        }

        /// <summary>
        /// </summary>
        /// <param name="inApi"></param>
        /// <param name="inSession"></param>
        /// <param name="inNamespace">DEPRECATED - Now passed to Api from Config</param>
        /// <param name="inCoroutineRunner"></param>
        [Obsolete("namespace param is deprecated (now passed to Api from Config): Use the overload without it"), UnityEngine.Scripting.Preserve]
        internal UGC(UGCApi inApi
            , UserSession inSession
            , string inNamespace
            , CoroutineRunner inCoroutineRunner)
            : this(inApi, inSession, inCoroutineRunner) // Curry this obsolete data to the new overload ->
        {
        }

        /// <summary>
        /// Create a content with string preview and get the payload url to upload the content.
        /// </summary>
        /// <param name="channelId">The id of the content's channel.</param>
        /// <param name="createRequest">Detail information for the content request.</param>
        /// <param name="callback">This will be called when the operation succeeded. The result is UGCResponse Model.</param>
        public void CreateContent(string channelId
            , UGCRequest createRequest
            , ResultCallback<UGCResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(channelId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetChannelIdInvalidMessage(channelId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.CreateContent(session.UserId, channelId, createRequest, callback));
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
        /// <param name="callback">
        /// This will be called when the operation succeeded. The result is UGCResponse Model.
        /// </param>
        /// <param name="contentType">
        /// The specific type of the content's created. default value is "application/octet-stream"
        /// </param>
        public void CreateContent(string channelId
            , string name
            , string type
            , string subtype
            , string[] tags
            , byte[] preview
            , string fileExtension
            , ResultCallback<UGCResponse> callback
            , string contentType = "application/octet-stream")
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(channelId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetChannelIdInvalidMessage(channelId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.CreateContent(session.UserId, channelId, name, type, subtype,
                    tags, preview, fileExtension, callback, contentType));
        }

        /// <summary>
        /// Modify existing content to update some information with string preview.
        /// </summary>
        /// <param name="channelId">The id of the content's channel.</param>
        /// <param name="contentId">The id of the content that will be modified.</param>
        /// <param name="ModifyRequest">Detail information for the content request that will be modified.</param>
        /// <param name="callback">
        /// This will be called when the operation succeeded. The result is UGCResponse Model.
        /// </param>
        public void ModifyContent(string channelId
            , string contentId
            , UGCUpdateRequest ModifyRequest
            , ResultCallback<UGCResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(channelId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetChannelIdInvalidMessage(channelId), callback))
            {
                return;
            }

            if (!ValidateAccelByteId(contentId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetContenttIdInvalidMessage(contentId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.ModifyContent(session.UserId, channelId, contentId, ModifyRequest, callback));
        }

        /// <summary>
        /// Modify existing content to update some information with string preview.
        /// </summary>
        /// <param name="channelId">The id of the content's channel.</param>
        /// <param name="contentId">The id of the content that will be modified.</param>
        /// <param name="ModifyRequest">Detail information for the content request that will be modified.</param>
        /// <param name="callback">
        /// This will be called when the operation succeeded. The result is UGCResponse Model.
        /// </param>
        /// <param name="updateContent">This will be used to update the content too or only content information . Default value is false.</param>
        [Obsolete("This method will be deprecated in future, please use ModifyContent(string channelId, string contentId, UGCUpdateRequest ModifyRequest, ResultCallback<UGCResponse> callback)")]
        public void ModifyContent(string channelId
            , string contentId
            , UGCRequest ModifyRequest
            , ResultCallback<UGCResponse> callback
            , bool updateContent = false)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(channelId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetChannelIdInvalidMessage(channelId), callback))
            {
                return;
            }

            if (!ValidateAccelByteId(contentId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetContenttIdInvalidMessage(contentId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.ModifyContent(session.UserId, channelId, contentId, ModifyRequest, callback, updateContent));
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
        /// <param name="updateContent">This will be used to update the content too or only content information . Default value is false.</param>
        [Obsolete("This method will be deprecated in future, please use ModifyContent(string channelId, string contentId, UGCUpdateRequest ModifyRequest, ResultCallback<UGCResponse> callback)")]
        public void ModifyContent(string channelId
            , string contentId
            , string name
            , string type
            , string subtype
            , string[] tags
            , byte[] preview
            , string fileExtension
            , ResultCallback<UGCResponse> callback
            , string contentType = "application/octet-stream"
            , bool updateContent = false)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(channelId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetChannelIdInvalidMessage(channelId), callback))
            {
                return;
            }

            if (!ValidateAccelByteId(contentId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetContenttIdInvalidMessage(contentId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.ModifyContent(
                    session.UserId,
                    channelId,
                    contentId,
                    name,
                    type,
                    subtype,
                    tags,
                    preview,
                    fileExtension,
                    callback,
                    contentType,
                    updateContent));
        }

        /// <summary>
        /// Delete a content based on the its channel id and content id.
        /// </summary>
        /// <param name="channelId">The id of the content's channel.</param>
        /// <param name="contentId">The id of the content that will be deleted.</param>
        /// <param name="callback">This will be called when the operation succeeded.</param>
        public void DeleteContent(string channelId
            , string contentId
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(channelId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetChannelIdInvalidMessage(channelId), callback))
            {
                return;
            }

            if (!ValidateAccelByteId(contentId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetContenttIdInvalidMessage(contentId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.DeleteContent(session.UserId, channelId, contentId, callback));
        }

        /// <summary>
        /// Get a content information by its content id
        /// </summary>
        /// <param name="contentId">The id of the content that will be fetched.</param>
        /// <param name="callback">This will be called when the operation succeeded. The result is UGCContentResponse Model.</param>
        public void GetContentByContentId(string contentId
            , ResultCallback<UGCContentResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(contentId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetContenttIdInvalidMessage(contentId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetContentByContentId(session.UserId, contentId, callback));
        }

        /// <summary>
        /// Get a content information by its share code.
        /// </summary>
        /// <param name="shareCode">The share code of the content that will be fetched.</param>
        /// <param name="callback">This will be called when the operation succeeded. The result is UGCContentResponse Model.</param>
        public void GetContentByShareCode(string shareCode
            , ResultCallback<UGCContentResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetContentByShareCode(shareCode, callback));
        }

        /// <summary>
        /// Get content preview as UGCPreview Model.
        /// </summary>
        /// <param name="contentId">The id of the Preview's content that will be fetched.</param>
        /// <param name="callback">This will be called when the operation succeeded. The result is UGCPreview Model.</param>
        public void GetContentPreview(string contentId
            , ResultCallback<UGCPreview> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(contentId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetContenttIdInvalidMessage(contentId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetContentPreview(session.UserId, contentId, callback));
        }

        /// <summary>
        /// Get content preview as byte array.
        /// </summary>
        /// <param name="contentId">The id of the Preview's content that will be fetched.</param>
        /// <param name="callback">This will be called when the operation succeeded. The result is byte array</param>
        public void GetContentPreview(string contentId
            , ResultCallback<byte[]> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(contentId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetContenttIdInvalidMessage(contentId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetContentPreview(session.UserId, contentId, callback));
        }

        /// <summary>
        /// Get all tags.
        /// </summary>
        /// <param name="callback">This will be called when the operation succeeded. The result is UGCTagsPagingResponse Model.</param>
        /// <param name="offset">The offset of the tags results. Default value is 0.</param>
        /// <param name="limit">The limit of the tags results. Default value is 1000.</param>
        public void GetTags(ResultCallback<UGCTagsPagingResponse> callback
            , int offset = 0
            , int limit = 1000)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetTags(callback, offset, limit));
        }

        /// <summary>
        /// Get all of player's types and subtypes.
        /// </summary>
        /// <param name="callback">This will be called when the operation succeeded. The result is UGCTypesPagingResponse Model.</param>
        /// <param name="offset">The offset of the types and/or subtypes paging data result. Default value is 0.</param>
        /// <param name="limit">The limit of the types and subtypes results. Default value is 1000.</param>
        public void GetTypes(ResultCallback<UGCTypesPagingResponse> callback
            , int offset = 0
            , int limit = 1000)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetTypes(callback, offset, limit));
        }

        /// <summary>
        /// Create player's channel with specific channel name.
        /// </summary>
        /// <param name="channelName">The name of the channel.</param>
        /// <param name="callback">This will be called when the operation succeeded. The result is UGCChannelResponse Model.</param>
        public void CreateChannel(string channelName
            , ResultCallback<UGCChannelResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.CreateChannel(session.UserId, channelName, callback));
        }

        /// <summary>
        /// Get all of the player's channels.
        /// </summary>
        /// <param name="userId">The userId.</param>
        /// <param name="callback">This will be called when the operation succeeded. The result is UGCChannelsPagingResponse Model.</param>
        /// <param name="offset">The offset of the channel results. Default value is 0.</param>
        /// <param name="limit">The limit of the channel results. Default value is 1000.</param>
        /// <param name="channelName">The name of the channel you want to query.</param>
        public void GetChannels(string userId
            , ResultCallback<UGCChannelPagingResponse> callback
            , int offset = 0
            , int limit = 1000
            , string channelName = "")
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(userId, Utils.AccelByteIdValidator.HypensRule.NoHypens, Utils.AccelByteIdValidator.GetUserIdInvalidMessage(userId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetChannels(userId, callback, offset, limit, channelName));
        }

        /// <summary>
        /// Get all of the player's channels.
        /// </summary>
        /// <param name="callback">This will be called when the operation succeeded. The result is UGCChannelsPagingResponse Model.</param>
        /// <param name="offset">The offset of the channel results. Default value is 0.</param>
        /// <param name="limit">The limit of the channel results. Default value is 1000.</param>
        /// <param name="channelName">The name of the channel you want to query.</param>
        public void GetChannels(ResultCallback<UGCChannelPagingResponse> callback
            , int offset = 0
            , int limit = 1000
            , string channelName = "")
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetChannels(session.UserId, callback, offset, limit, channelName));
        }

        /// <summary>
        /// Delete player's channel based on the its channel id.
        /// </summary>
        /// <param name="channelId">The id of the channel that will be deleted.</param>
        /// <param name="callback">This will be called when the operation succeeded.</param>
        public void DeleteChannel(string channelId
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(channelId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetChannelIdInvalidMessage(channelId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.DeleteChannel(session.UserId, channelId, callback));
        }

        /// <summary>
        /// Search Content player's channel based on the its channel id.
        /// </summary>
        /// <param name="searchContentRequest ">Detail information for the search content request..</param>
        /// <param name="callback">This will be called when the operation succeeded.</param>
        public void SearchContent(SearchContentRequest searchContentRequest
            , ResultCallback<UGCSearchContentsPagingResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }
            coroutineRunner.Run(
                api.SearchContent(searchContentRequest, session.UserId, callback));
        }

        /// <summary>
        /// Search Content player's channel based on the its channel id.
        /// </summary>
        /// <param name="channelId ">The id of the content's channel.</param>
        /// <param name="searchContentRequest ">Detail information for the content request..</param>
        /// <param name="callback">This will be called when the operation succeeded.</param>
        public void SearchContentsSpesificToChannel(string channelId
            , SearchContentRequest searchContentRequest
            , ResultCallback<UGCSearchContentsPagingResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(channelId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetChannelIdInvalidMessage(channelId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }
            coroutineRunner.Run(
                api.SearchContentsSpesificToChannel(channelId, searchContentRequest, session.UserId, callback));
        }

        /// </summary>
        /// Update like/unlike status to a content.
        /// </summary>
        /// <param name="contentId ">The content id that will be updated.</param>
        /// <param name="likeStatus ">New like Status value.</param>
        /// <param name="callback">This will be called when the operation succeeded.</param>
        public void UpdateLikeStatusToContent(string contentId
            , bool likeStatus
            , ResultCallback<UGCUpdateLikeStatusToContentResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(contentId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetContenttIdInvalidMessage(contentId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            var requestModel = new UpdateLikeStatusToContentRequest()
            {
                LikeStatus = likeStatus
            };

            var requestParameter = new UpdateLikeStatusToContentParameter()
            {
                ContentId = contentId
            };

            coroutineRunner.Run(api.UpdateLikeStatusToContent(requestModel, requestParameter, callback));
        }

        /// <summary>
        /// Get List Followers.
        /// </summary>
        /// <param name="callback">This will be called when the operation succeeded.</param>
        /// <param name="limit">The limit Number of user per page. Default value is 1000.</param>
        /// <param name="offset">The offset number to retrieve. Default value is 0.</param>
        public void GetListFollowers(
            ResultCallback<UGCGetListFollowersPagingResponse> callback
            , int limit = 1000
            , int offset = 0)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetListFollowers(session.UserId, callback, limit, offset));
        }

        /// <summary>
        /// Update follow/unfollow status to user.
        /// </summary>
        /// <param name="followStatus">The new follow status value.</param>
        /// <param name="callback">This will be called when the operation succeeded.</param>
        public void UpdateFollowStatus(bool followStatus
            , ResultCallback<UGCUpdateFollowStatusToUserResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            var requestModel = new UpdateFollowStatusRequest()
            {
                FollowStatus = followStatus
            };

            var requestParameter = new UpdateFollowStatusParameter()
            {
                UserId = session.UserId
            };

            coroutineRunner.Run(api.UpdateFollowStatus(requestModel, requestParameter, callback));
        }

        /// <summary>
        /// Update follow/unfollow status to user.
        /// </summary>
        /// <param name="followStatus">The new follow status value.</param>
        /// <param name="userId">The userId.</param>
        /// <param name="callback">This will be called when the operation succeeded.</param>
        public void UpdateFollowStatus(bool followStatus
            , string userId
            , ResultCallback<UGCUpdateFollowStatusToUserResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(userId, Utils.AccelByteIdValidator.HypensRule.NoHypens, Utils.AccelByteIdValidator.GetUserIdInvalidMessage(userId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            var requestModel = new UpdateFollowStatusRequest()
            {
                FollowStatus = followStatus
            };

            var requestParameter = new UpdateFollowStatusParameter()
            {
                UserId = userId
            };

            coroutineRunner.Run(api.UpdateFollowStatus(requestModel, requestParameter, callback));
        }

        /// <summary>
        /// Get Bulk ContentId.
        /// </summary>
        /// <param name="contentId">The content Ids.</param>
        /// <param name="callback">This will be called when the operation succeeded.</param>
        public void GetBulkContentId(string[] contentIds
            , ResultCallback<UGCModelsContentsResponse[]> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            var requestModel = new GetBulkContentIdRequest()
            {
                ContentId = contentIds
            };

            coroutineRunner.Run(api.GetBulkContentId(requestModel, callback));
        }

        /// <summary>
        /// Get User Contents.
        /// </summary>
        /// <param name="userId">The userId.</param>
        /// <param name="callback">This will be called when the operation succeeded.</param>
        /// <param name="limit">The limit Number of user per page. Default value is 1000.</param>
        /// <param name="offset">The offset number to retrieve. Default value is 0.</param>
        public void GetUserContents(string userId
            , ResultCallback<UGCContentsPagingResponse> callback
            , int limit = 1000
            , int offset = 0)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(userId, Utils.AccelByteIdValidator.HypensRule.NoHypens, Utils.AccelByteIdValidator.GetUserIdInvalidMessage(userId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetUserContents(userId, callback, limit, offset));
        }

        /// <summary>
        /// Get User Contents.
        /// </summary>
        /// <param name="contentId">The contentId.</param>
        /// <param name="userId">The userId.</param>
        /// <param name="screenshotsRequest">Supported file extensions: pjp, jpg, jpeg, jfif, bmp, png.
        /// Maximum description length: 1024..</param>
        /// <param name="callback">This will be called when the operation succeeded.</param>
        public void UploadScreenshotContent(string contentId
            , string userId
            , ScreenshotsRequest screenshotsRequest
            , ResultCallback<ScreenshotsResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(contentId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetContenttIdInvalidMessage(contentId), callback))
            {
                return;
            }

            if (!ValidateAccelByteId(userId, Utils.AccelByteIdValidator.HypensRule.NoHypens, Utils.AccelByteIdValidator.GetUserIdInvalidMessage(userId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.UploadScreenshotContent(contentId, userId, screenshotsRequest, callback));
        }

        /// <summary>
        /// Get Content Followed.
        /// </summary>
        /// <param name="callback">This will be called when the operation succeeded.</param>
        /// <param name="limit">The limit Number of user per page. Default value is 1000.</param>
        /// <param name="offset">The offset number to retrieve. Default value is 0.</param>
        public void GetContentFollowed(
            ResultCallback<UGCContentsPagingResponse> callback
            , int limit = 1000
            , int offset = 0)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetContentFollowed(callback, limit, offset));
        }

        /// <summary>
        /// Get Followed Creators.
        /// </summary>
        /// <param name="callback">This will be called when the operation succeeded.</param>
        /// <param name="limit">The limit Number of user per page. Default value is 1000.</param>
        /// <param name="offset">The offset number to retrieve. Default value is 0.</param>
        public void GetFollowedCreators(
            ResultCallback<UGCGetListFollowersPagingResponse> callback
            , int limit = 1000
            , int offset = 0)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetFollowedCreators(callback, limit, offset));
        }

        /// <summary>
        /// Get List Following.
        /// </summary>
        /// <param name="userId">The userId.</param>
        /// <param name="callback">This will be called when the operation succeeded.</param>
        /// <param name="limit">The limit Number of user per page. Default value is 1000.</param>
        /// <param name="offset">The offset number to retrieve. Default value is 0.</param>
        public void GetListFollowing(string userId
            , ResultCallback<UGCGetListFollowersPagingResponse> callback
            , int limit = 1000
            , int offset = 0)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(userId, Utils.AccelByteIdValidator.HypensRule.NoHypens, Utils.AccelByteIdValidator.GetUserIdInvalidMessage(userId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetListFollowing(userId, callback, limit, offset));
        }

        /// <summary>
        /// Get Liked Contents.
        /// </summary>
        /// <param name="getLikedContentRequest">Detail information for the content request.</param>
        /// <param name="callback">This will be called when the operation succeeded.</param>
        [Obsolete("This method will be deprecated in future, please use " +
            "GetLikedContents( GetAllLikedContentRequest getLikedContentRequest ResultCallback<UGCContentsPagingResponse> callback)")]
        public void GetLikedContents(
            GetLikedContentRequest getLikedContentRequest
            , ResultCallback<UGCContentsPagingResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetLikedContents(getLikedContentRequest, callback));
        }

        /// <summary>
        /// Get Liked Contents.
        /// </summary>
        /// <param name="getLikedContentRequest">Detail information for the content request.</param>
        /// <param name="callback">This will be called when the operation succeeded.</param>
        public void GetLikedContents(
            GetAllLikedContentRequest getLikedContentRequest
            , ResultCallback<UGCContentsPagingResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetLikedContents(getLikedContentRequest, callback));
        }

        /// <summary>
        /// Get Creator Stats.
        /// </summary>
        /// <param name="userId">The userId.</param>
        /// <param name="callback">This will be called when the operation succeeded.</param>
        public void GetCreatorStats(string userId
            , ResultCallback<UGCGetCreatorStatsResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(userId, Utils.AccelByteIdValidator.HypensRule.NoHypens, Utils.AccelByteIdValidator.GetUserIdInvalidMessage(userId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetCreatorStats(userId, callback));
        }

        /// <summary>
        /// Get User Groups.
        /// </summary>
        /// <param name="userId">The userId.</param>
        /// <param name="callback">This will be called when the operation succeeded.</param>
        /// <param name="limit">The limit Number of user per page. Default value is 1000.</param>
        /// <param name="offset">The offset number to retrieve. Default value is 0.</param>
        public void GetUserGroups(string userId
            , ResultCallback<UGCGetUserGroupsPagingResponse> callback
            , int limit = 1000
            , int offset = 0)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(userId, Utils.AccelByteIdValidator.HypensRule.NoHypens, Utils.AccelByteIdValidator.GetUserIdInvalidMessage(userId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetUserGroups(userId, callback, limit, offset));
        }

        /// <summary>
        /// Update channel.
        /// </summary>
        /// <param name="channelId">The channelId value.</param>
        /// <param name="name">The new channel's name value.</param>
        /// <param name="callback">This will be called when the operation succeeded.</param>
        public void UpdateChannel(string channelId
            , string name
            , ResultCallback<UGCChannelResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(channelId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetChannelIdInvalidMessage(channelId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            var requestModel = new UpdateChannelRequest()
            {
                Name = name
            };

            var requestParameter = new UpdateChannelParameter()
            {
                UserId = session.UserId,
                ChannelId = channelId
            };

            coroutineRunner.Run(api.UpdateChannel(requestModel, requestParameter, callback));
        }
    }
}
