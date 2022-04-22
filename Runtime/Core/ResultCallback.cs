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
            callback?.Invoke(param);
        }

        public static void TryOk<T>(this ResultCallback<T> callback, T value)
        {
            callback?.Invoke(Result<T>.CreateOk(value));
        }

        public static void TryError<T>(this ResultCallback<T> callback, ErrorCode errorCode)
        {
            callback?.Invoke(Result<T>.CreateError(errorCode));
        }

        public static void TryError<T>(this ResultCallback<T> callback, Error error)
        {
            callback?.Invoke(Result<T>.CreateError(error));
        }

        public static void Try(this ResultCallback callback, Result param)
        {
            callback?.Invoke(param);
        }

        public static void TryOk(this ResultCallback callback)
        {
            callback?.Invoke(Result.CreateOk());
        }

        public static void TryError(this ResultCallback callback, ErrorCode errorCode, string errorMessage = null)
        {
            callback?.Invoke(Result.CreateError(errorCode, errorMessage));
        }

        public static void TryError(this ResultCallback callback, Error error)
        {
            callback?.Invoke(Result.CreateError(error));
        }
    }
}