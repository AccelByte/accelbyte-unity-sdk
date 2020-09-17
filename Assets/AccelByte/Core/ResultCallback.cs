// Copyright (c) 2018 - 2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

namespace AccelByte.Core
{
    public delegate void ResultCallback<T>(Result<T> result);

    public delegate void ResultCallback(Result result);

    public static class ResultCallbackExtension
    {
        public static void Try<T>(this ResultCallback<T> callback, Result<T> param)
        {
            if (callback != null)
            {
                callback(param);
            }
        }

        public static void TryOk<T>(this ResultCallback<T> callback, T value)
        {
            if (callback != null)
            {
                callback(Result<T>.CreateOk(value));
            }
        }

        public static void TryError<T>(this ResultCallback<T> callback, ErrorCode errorCode)
        {
            if (callback != null)
            {
                callback(Result<T>.CreateError(errorCode));
            }
        }

        public static void TryError<T>(this ResultCallback<T> callback, Error error)
        {
            if (callback != null)
            {
                callback(Result<T>.CreateError(error));
            }
        }

        public static void Try(this ResultCallback callback, Result param)
        {
            if (callback != null)
            {
                callback(param);
            }
        }

        public static void TryOk(this ResultCallback callback)
        {
            if (callback != null)
            {
                callback(Result.CreateOk());
            }
        }

        public static void TryError(this ResultCallback callback, ErrorCode errorCode, string errorMessage = null)
        {
            if (callback != null)
            {
                callback(Result.CreateError(errorCode, errorMessage));
            }
        }

        public static void TryError(this ResultCallback callback, Error error)
        {
            if (callback != null)
            {
                callback(Result.CreateError(error));
            }
        }
    }
}