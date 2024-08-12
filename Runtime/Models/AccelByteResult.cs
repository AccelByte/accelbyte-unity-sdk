// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System;

namespace AccelByte.Models
{
    public class AccelByteResult<ResultType,ErrorType>
    {
        private Action<ResultType> successDelegates;
        private Action<ErrorType> errorDelegates;
        private Action finalDelegates;
        private ResultType queuedData;
        private ErrorType queuedError;

        private bool isSuccess;
        private bool isError;

        internal void Resolve(ResultType result)
        {
            queuedData = result;
            isSuccess = true;
            successDelegates?.Invoke(result);
            finalDelegates?.Invoke();
        }

        internal void Reject(ErrorType error)
        {
            queuedError = error;
            isError = true;
            errorDelegates?.Invoke(error);
            finalDelegates?.Invoke();
        }

        /// <summary>
        /// Add callback on success 
        /// </summary>
        /// <param name="onSuccess">Callback when AccelByte API is success</param>
        /// <returns>AccelByteResult<ResultType,ErrorType></returns>
        public AccelByteResult<ResultType,ErrorType> OnSuccess(Action<ResultType> onSuccess)
        {
            if (onSuccess != null)
            {
                successDelegates += onSuccess;
                if (isSuccess)
                {
                    onSuccess?.Invoke(queuedData);
                }
            }
            return this;
        }

        /// <summary>
        /// Add callback on failed
        /// </summary>
        /// <param name="onError">Callback when AccelByte API is failed</param>
        /// <returns>AccelByteResult<ResultType,ErrorType></returns>
        public AccelByteResult<ResultType,ErrorType> OnFailed(Action<ErrorType> onError)
        {
            if (onError != null)
            {
                errorDelegates += onError;
                // to avoid race conditions for early returns
                if (isError)
                {
                    onError.Invoke(queuedError);
                }
            }
            return this;
        }

        /// <summary>
        /// Add callback when the proccess is complete.
        /// </summary>
        /// <param name="onComplete">Callback when AccelByte API is either success or failed</param>
        /// <returns>AccelByteResult<ResultType,ErrorType></returns>
        public AccelByteResult<ResultType,ErrorType> OnComplete(Action onComplete)
        {
            if (onComplete != null)
            {
                finalDelegates += onComplete;
                // to avoid race conditions for early returns
                if (isSuccess || isError)
                {
                    onComplete?.Invoke();
                }
            }
            return this;
        }
    }

    public class AccelByteResult<ErrorType>
    {
        private Action successDelegates;
        private Action<ErrorType> errorDelegates;
        private Action finalDelegates;
        private bool isSuccess;
        private bool isError;
        
        private ErrorType queuedError;

        internal void Resolve()
        {
            isSuccess = true;
            successDelegates?.Invoke();
            finalDelegates?.Invoke();
        }

        internal void Reject(ErrorType error)
        {
            isError = true;
            queuedError = error;
            errorDelegates?.Invoke(error);
            finalDelegates?.Invoke();
        }

        /// <summary>
        /// Add callback on success 
        /// </summary>
        /// <param name="onSuccess">Callback when AccelByte API is success</param>
        /// <returns>AccelByteResult<ErrorType></returns>
        public AccelByteResult<ErrorType> OnSuccess(Action onSuccess)
        {
            if (onSuccess != null)
            {
                successDelegates += onSuccess;
                if (isSuccess)
                {
                    onSuccess?.Invoke();
                }
            }
            return this;
        }

        /// <summary>
        /// Add callback on failed
        /// </summary>
        /// <param name="onError">Callback when AccelByte API is failed</param>
        /// <returns>AccelByteResult<ErrorType></returns>
        public AccelByteResult<ErrorType> OnFailed(Action<ErrorType> onError)
        {
            if (onError != null)
            {
                errorDelegates += onError;
                // to avoid race conditions for early returns
                if (isError)
                {
                    onError.Invoke(queuedError);
                }
            }
            return this;
        }

        /// <summary>
        /// Add callback when the proccess is complete.
        /// </summary>
        /// <param name="onComplete">Callback when AccelByte API is either success or failed</param>
        /// <returns>AccelByteResult<ErrorType></returns>
        public AccelByteResult<ErrorType> OnComplete(Action onComplete)
        {
            if (onComplete != null)
            {
                finalDelegates += onComplete;
                // to avoid race conditions for early returns
                if (isError || isSuccess)
                {
                    onComplete?.Invoke();
                }
            }
            return this;
        }
    }
}
