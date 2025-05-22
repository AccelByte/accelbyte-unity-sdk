// Copyright (c) 2018 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections;
using AccelByte.Core;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace AccelByte.Api
{
    public static class ABUtilities 
    {
        /// <summary>
        /// Take a URL and attempt to Download a <see cref="Texture2D"/> from it
        /// </summary>
        /// <param name="url">The URL to download the Image from</param>
        /// <param name="callback">Returns a result that contains a <see cref="Texture2D"/></param>
        /// <param name="logger">Override method logger</param>
        /// <returns></returns>
        public static IEnumerator DownloadTexture2D(string url, ResultCallback<Texture2D> callback, IDebugger logger = null)
        {
            if (string.IsNullOrEmpty(url))
            {
                callback?.Try(Result<Texture2D>.CreateError(ErrorCode.ErrorFromException, "Download url is empty"));
                yield break;
            }
            
            using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
            {
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
                {
                    logger?.LogWarning(request.error);
                    ErrorCode code;
                    if (request.result == UnityWebRequest.Result.ConnectionError)
                    {
                        code = ErrorCode.NetworkError;
                    }
                    else
                    {
                        code = (ErrorCode)request.responseCode;
                    }
                    callback?.Try(Result<Texture2D>.CreateError(code, request.error));
                }
                else
                {
                    Texture2D returnedTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                    callback?.Try(returnedTexture == null
                        ? Result<Texture2D>.CreateError(ErrorCode.NotFound, $"Could not find specified image file {request.url}")
                        : Result<Texture2D>.CreateOk(returnedTexture));
                }
            }
        }

        /// <summary>
        /// Take a URL and attempt to Download a <see cref="Texture2D"/> from it
        /// </summary>
        /// <param name="url">The URL to download the Image from</param>
        /// <param name="callback">Returns a result that contains a <see cref="Texture2D"/></param>
        /// <param name="logger">Override method logger</param>
        /// <returns></returns>
        public static async void DownloadTexture2DAsync(string url, ResultCallback<Texture2D> callback, IDebugger logger = null)
        {
            if (string.IsNullOrEmpty(url))
            {
                callback?.Try(Result<Texture2D>.CreateError(ErrorCode.ErrorFromException, "Download url is empty"));
                return;
            }
            
            using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
            {
                await request.SendWebRequest();

                try
                {
                    if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
                    {
                        logger?.LogWarning(request.error);
                        ErrorCode code;
                        if (request.result == UnityWebRequest.Result.ConnectionError)
                        {
                            code = ErrorCode.NetworkError;
                        }
                        else
                        {
                            code = (ErrorCode)request.responseCode;
                        }
                        callback?.Try(Result<Texture2D>.CreateError(code, request.error));
                    }
                    else
                    {
                        Texture2D returnedTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                        callback?.Try(returnedTexture == null
                            ? Result<Texture2D>.CreateError(ErrorCode.NotFound, $"Could not find specified image file {request.url}")
                            : Result<Texture2D>.CreateOk(returnedTexture));
                    }
                }
                catch (Exception ex)
                {
                    callback?.Try(Result<Texture2D>.CreateError(ErrorCode.ErrorFromException, $"{ex.Message}"));
                }
            }
        }
    }
}
