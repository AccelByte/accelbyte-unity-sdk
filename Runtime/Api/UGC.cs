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

        #region UGC V1

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
        
        /// <summary>
        /// Modify existing content to update some information by share code.
        /// </summary>
        /// <param name="channelId">The id of the content's channel.</param>
        /// <param name="shareCode">The share code of the content that will be fetched.</param>
        /// <param name="modifyRequest">Detail information for the content request that will be modified.</param>
        /// <param name="callback">
        /// This will be called when the operation succeeded. The result is UGCResponse Model.
        /// </param>
        public void ModifyContentByShareCode(string channelId
            , string shareCode
            , UGCUpdateRequest modifyRequest
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
                api.ModifyContentByShareCode(session.UserId, channelId, shareCode, modifyRequest, callback));
        }
        
        /// <summary>
        /// Delete a content based on  its channel id and share code.
        /// </summary>
        /// <param name="channelId">The id of the content's channel.</param>
        /// <param name="shareCode">The share code of the content that will be fetched.</param>
        /// <param name="callback">This will be called when the operation succeeded.</param>
        public void DeleteContentByShareCode(string channelId
            , string shareCode
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
                api.DeleteContentByShareCode(session.UserId, channelId, shareCode, callback));
        }
        
        /// <summary>
        /// Get contents by share codes
        /// </summary>
        /// <param name="shareCodes">Content ShareCodes Array</param>
        /// <param name="callback">This will be called when the operation succeeded.</param>
        public void BulkGetContentByShareCode(string[] shareCodes
            , ResultCallback<UGCModelsContentsResponse[]> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.BulkGetContentByShareCode(shareCodes, callback));
        }

        #endregion

        #region UGC V2 (Content)

        /// <summary>
        /// Search contents specific to a channel.
        /// </summary>
        /// <param name="channelId">Channel Id.</param>
        /// <param name="callback">This will be called when the operation succeeded.</param>
        /// <param name="limit">The limit Number of user per page. Default value is 20.</param>
        /// <param name="offset">The offset number to retrieve. Default value is 0.</param>
        /// <param name="sortBy">Sorting criteria: created time with asc or desc. Default= created time and desc.</param>
        public void SearchContentsSpecificToChannelV2(string channelId
            , ResultCallback<UGCSearchContentsPagingResponseV2> callback
            , int limit = 20
            , int offset = 0
            , UGCContentDownloaderSortBy sortBy = UGCContentDownloaderSortBy.CreatedTimeDesc)
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
                api.SearchContentsSpecificToChannelV2(channelId, callback, limit, offset, sortBy));
        }

        /// <summary>
        /// Get all contents in current namespace
        /// </summary>
        /// <param name="filterRequest">To filter the returned UGC contets.</param>
        /// <param name="callback">This will be called when the operation succeeded.</param>
        /// <param name="limit">The limit Number of user per page. Default value is 20.</param>
        /// <param name="offset">The offset number to retrieve. Default value is 0.</param>
        /// <param name="sortBy">Sorting criteria: name, download, like, created time with asc or desc. Default= created time and asc.</param>
        public void SearchContentsV2(UGCGetContentFilterRequestV2 filterRequest
            , ResultCallback<UGCSearchContentsPagingResponseV2> callback
            , int limit = 20
            , int offset = 0
            , UGCContentSortBy sortBy = UGCContentSortBy.CreatedTimeDesc)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.SearchContentsV2(filterRequest, callback, limit, offset, sortBy));
        }

        /// <summary>
        /// Get contents by content Ids
        /// </summary>
        /// <param name="contentId">Content Ids Array</param>
        /// <param name="callback">This will be called when the operation succeeded.</param>
        public void GetContentBulkByIdsV2(string[] contentIds
            , ResultCallback<UGCModelsContentsResponseV2[]> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetContentBulkByIdsV2(contentIds, callback));
        }

        /// <summary>
        /// Get a content information by its share code.
        /// </summary>
        /// <param name="shareCode">The share code of the content that will be fetched.</param>
        /// <param name="callback">This will be called when the operation succeeded.</param>
        public void GetContentByShareCodeV2(string shareCode
            , ResultCallback<UGCModelsContentsResponseV2> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetContentByShareCodeV2(shareCode, callback));
        }

        /// <summary>
        /// Get a content information by its content id
        /// </summary>
        /// <param name="contentId">The id of the content that will be fetched.</param>
        /// <param name="callback">This will be called when the operation succeeded.</param>
        public void GetContentByContentIdV2(string contentId
            , ResultCallback<UGCModelsContentsResponseV2> callback)
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
                api.GetContentByContentIdV2(contentId, callback));
        }

        /// <summary>
        /// Create a UGC content with create content request.
        /// </summary>
        /// <param name="channelId ">The id of the content's channel.</param>
        /// <param name="createRequest">Detail information for the content request.</param>
        /// <param name="callback">This will be called when the operation succeeded.</param>
        public void CreateContentV2(string channelId
            , CreateUGCRequestV2 createRequest
            , ResultCallback<UGCModelsCreateUGCResponseV2> callback)
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
                api.CreateContentV2(session.UserId, channelId, createRequest, callback));
        }

        /// <summary>
        /// Delete a content based on the its channel id and content id.
        /// </summary>
        /// <param name="channelId">The id of the content's channel.</param>
        /// <param name="contentId">The id of the content that will be deleted.</param>
        /// <param name="callback">This will be called when the operation succeeded.</param>
        public void DeleteContentV2(string channelId
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
                api.DeleteContentV2(session.UserId, channelId, contentId, callback));
        }

        /// <summary>
        /// Modify existing content to update some information.
        /// </summary>
        /// <param name="channelId ">The id of the content's channel.</param>
        /// <param name="contentId">The contentId.</param>
        /// <param name="modifyRequest">Detail information for the content request that will be modified.</param>
        /// <param name="callback">This will be called when the operation succeeded.</param>
        public void ModifyContentV2(string channelId
            , string contentId
            , ModifyUGCRequestV2 modifyRequest
            , ResultCallback<UGCModelsModifyUGCResponseV2> callback)
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
                api.ModifyContentV2(session.UserId, channelId, contentId, modifyRequest, callback));
        }

        /// <summary>
        /// Generate Upload URL and Conten File Location.
        /// </summary>
        /// <param name="channelId ">The id of the content's channel.</param>
        /// <param name="contentId">The contentId.</param>
        /// <param name="uploadRequest">Detail information for the upload request.</param>
        /// <param name="callback">This will be called when the operation succeeded.</param>
        public void GenerateUploadContentURLV2(string channelId
            , string contentId
            , UGCUploadContentURLRequestV2 uploadRequest
            , ResultCallback<UGCModelsUploadContentURLResponseV2> callback)
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
                api.GenerateUploadContentURLV2(session.UserId, channelId, contentId, uploadRequest, callback));
        }

        /// <summary>
        /// Update Content File Location in S3.
        /// </summary>
        /// <param name="channelId">The id of the content's channel.</param>
        /// <param name="contentId">The id of the content.</param>
        /// <param name="fileExtension">FileExtension of the content.</param>
        /// <param name="s3Key">Detail information about the file location in S3.</param>
        /// <param name="callback">This will be called when the operation succeeded.</param>
        public void UpdateContentFileLocationV2(string channelId
            , string contentId
            , string fileExtension
            , string s3Key
            , ResultCallback<UpdateContentFileLocationResponseV2> callback)
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
                api.UpdateContentFileLocationV2(session.UserId, channelId, contentId, fileExtension, s3Key, callback));
        }

        /// <summary>
        /// Get user's generated contents
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="callback">This will be called when the operation succeeded.</param>
        /// <param name="limit">The limit Number of user per page. Default value is 20.</param>
        /// <param name="offset">The offset number to retrieve. Default value is 0.</param>
        /// <param name="sortBy">Sorting criteria: created time with asc or desc. Default= created time and desc.</param>
        public void GetUserContentsV2(string userId
            , ResultCallback<UGCSearchContentsPagingResponseV2> callback
            , int limit = 20
            , int offset = 0
            , UGCContentDownloaderSortBy sortBy = UGCContentDownloaderSortBy.CreatedTimeDesc)
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
                api.GetUserContentsV2(userId, callback, limit, offset, sortBy));
        }

        /// <summary>
        /// Update screenshots for content.
        /// </summary>
        /// <param name="contentId">Content Id.</param>
        /// <param name="userId">The userId.</param>
        /// <param name="screenshotsRequest">Supported file extensions: pjp, jpg, jpeg, jfif, bmp, png.
        /// Maximum description length: 1024.</param>
        /// <param name="callback">This will be called when the operation succeeded.</param
        public void UpdateContentScreenshotV2(string contentId
            , ScreenshotsUpdatesV2 updateSreenshotsRequest
            , ResultCallback<ScreenshotsUpdatesV2> callback)
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
                api.UpdateContentScreenshotV2(session.UserId, contentId, updateSreenshotsRequest, callback));
        }

        /// <summary>
        /// Upload screenshots for content.
        /// </summary>
        /// <param name="contentId">Content Id.</param>
        /// <param name="userId">The userId.</param>
        /// <param name="screenshotsRequest">Supported file extensions: pjp, jpg, jpeg, jfif, bmp, png.
        /// Maximum description length: 1024.</param>
        /// <param name="callback">This will be called when the operation succeeded.</param
        public void UploadContentScreenshotV2(string contentId
            , ScreenshotsRequest screenshotsRequest
            , ResultCallback<ScreenshotsResponseV2> callback)
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
                api.UploadContentScreenshotV2(session.UserId, contentId, screenshotsRequest, callback));
        }

        /// <summary>
        /// Delete screenshots for content.
        /// </summary>
        /// <param name="contentId">Content Id.</param>
        /// <param name="screenshotId">Screenshot Id.</param>
        /// <param name="callback">This will be called when the operation succeeded.</param
        public void DeleteContentScreenshotV2(string contentId
            , string screenshotId
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(contentId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetContenttIdInvalidMessage(contentId), callback))
            {
                return;
            }

            if (!ValidateAccelByteId(screenshotId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetScreenshotIdInvalidMessage(screenshotId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.DeleteContentScreenshotV2(session.UserId, contentId, screenshotId, callback));
        }
        
        /// <summary>
        /// Get contents by share codes
        /// </summary>
        /// <param name="shareCodes">Content ShareCodes Array</param>
        /// <param name="callback">This will be called when the operation succeeded.</param>
        public void BulkGetContentByShareCodeV2(string[] shareCodes
            , ResultCallback<UGCModelsContentsResponseV2[]> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.BulkGetContentByShareCodeV2(shareCodes, callback));
        }

        #endregion

        #region UGC V2 (Download Count)

        /// <summary>
        /// Add download count for a content.
        /// </summary>
        /// <param name="contentId">The contentId.</param>
        /// <param name="callback">This will be called when the operation succeeded.</param>
        public void AddDownloadContentCountV2(string contentId
            , ResultCallback<UGCModelsAddDownloadContentCountResponseV2> callback)
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
                api.AddDownloadContentCountV2(contentId, callback));
        }

        /// <summary>
        /// Get list of UGC content downloader
        /// </summary>
        /// <param name="contentId">The contentId.</param>
        /// <param name="callback">This will be called when the operation succeeded.</param>
        /// <param name="userId">The userId.</param>
        /// <param name="limit">The limit Number of user per page. Default value is 20.</param>
        /// <param name="offset">The offset number to retrieve. Default value is 0.</param>
        /// <param name="sortBy">Sorting criteria: created time with asc or desc. Default= created time and desc.</param>
        public void GetListContentDownloaderV2(string contentId
            , ResultCallback<UGCModelsPaginatedContentDownloaderResponseV2> callback
            , string userId = ""
            , int limit = 20
            , int offset = 0
            , UGCContentDownloaderSortBy sortBy = UGCContentDownloaderSortBy.CreatedTimeDesc)
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
                api.GetListContentDownloaderV2(contentId, callback, userId, limit, offset, sortBy));
        }

        #endregion

        #region UGC V2 (Like)

        /// <summary>
        /// Get a list of users who like the content.
        /// </summary>
        /// <param name="contentId">The contentId.</param>
        /// <param name="callback">This will be called when the operation succeeded.</param>
        /// <param name="limit">The limit Number of user per page. Default value is 20.</param>
        /// <param name="offset">The offset number to retrieve. Default value is 0.</param>
        /// <param name="sortBy">Sorting criteria: created time with asc or desc. Default= created time and desc.</param>
        public void GetListContentLikerV2(string contentId
            , ResultCallback<UGCModelsPaginatedContentLikerResponseV2> callback
            , int limit = 20
            , int offset = 0
            , UGCContentDownloaderSortBy sortBy = UGCContentDownloaderSortBy.CreatedTimeDesc)
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
                api.GetListContentLikerV2(contentId, callback, limit, offset, sortBy));
        }

        /// </summary>
        /// Update like/unlike status to a content.
        /// </summary>
        /// <param name="contentId ">The content id that will be updated.</param>
        /// <param name="likeStatus ">New like Status value.</param>
        /// <param name="callback">This will be called when the operation succeeded.</param>
        public void UpdateLikeStatusToContentV2(string contentId
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

            coroutineRunner.Run(
                api.UpdateLikeStatusToContentV2(contentId, likeStatus, callback));
        }

        #endregion UGC V2 (Like)

        #region UGC V2 (Staging Content)

        /// </summary>
        /// Get user staging content by Id.
        /// </summary>
        /// <param name="contentId ">The content id that will be fetched.</param>
        /// <param name="callback">This will be called when the operation succeeded.</param>
        public void GetUserStagingContentByContentId(string userId
            , string contentId
            , ResultCallback<UGCModelStagingContent> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(userId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetUserIdInvalidMessage(userId), callback))
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
                api.GetUserStagingContentByContentId(userId, contentId, callback));
        }

        /// </summary>
        /// Get user staging content by Id.
        /// </summary>
        /// <param name="userId ">The User id.</param>
        /// <param name="status ">Status of staging content that will be fetched</param>
        /// <param name="offset">The offset of the channel results.</param>
        /// <param name="limit">The limit of the channel results.</param>
        /// <param name="callback">This will be called when the operation succeeded.</param>
        public void GetListStagingContent(string userId
            , UGCStagingContentStatus status
            , int limit
            , int offset
            , UGCListStagingContentSortBy sortBy
            , ResultCallback<UGCModelsPaginatedStagingContentResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(userId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetUserIdInvalidMessage(userId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetListStagingContent(userId, status, limit, offset, sortBy, callback));
        }

        /// <summary>
        /// Modify staging content to update some information based on content id.
        /// </summary>
        /// <param name="userId ">The User di.</param>
        /// <param name="contentId">The content Id.</param>
        /// <param name="modifyRequest">Detail information for the content request that will be modified.</param>
        /// <param name="callback">This will be called when the operation succeeded.</param>
        public void ModifyStagingContentByContentId(string userId
            , string contentId
            , UGCUpdateStagingContentRequest modifyRequest
            , ResultCallback<UGCModelStagingContent> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(userId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetUserIdInvalidMessage(userId), callback))
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
                api.ModifyStagingContentByContentId(userId, contentId, modifyRequest, callback));
        }

        /// </summary>
        /// Delete a content based on content id.
        /// </summary
        /// <param name="userId ">The User id.</param>
        /// <param name="contentId ">The content id that will be fetched.</param>
        /// <param name="callback">This will be called when the operation succeeded.</param>
        public void DeleteStagingContentByContentId(string userId
            , string contentId
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(userId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetUserIdInvalidMessage(userId), callback))
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
                api.DeleteStagingContentByContentId(userId, contentId, callback));
        }

        #endregion
    }
}
 