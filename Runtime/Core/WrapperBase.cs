// Copyright (c) 2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.


namespace AccelByte.Core
{
    /// <summary>
    /// Inherit from this class to set common Client Core functionality.
    /// - Used for Core modules, such as "Group".
    /// </summary>
    public class WrapperBase
    {
        private ApiSharedMemory sharedMemory;
        internal ApiSharedMemory SharedMemory
        {
            get
            {
                if(sharedMemory == null)
                {
                    sharedMemory = new ApiSharedMemory();
                }
                return sharedMemory;
            }
            set
            {
                sharedMemory = value;
            }
        }

        protected Utils.AccelByteIdValidator IdValidator
        {
            get
            {
                var retval = SharedMemory.IdValidator;
                return retval;
            }
        }

        internal void SetAccelByteIdValidator(Utils.AccelByteIdValidator idValidator)
        {
            SharedMemory.IdValidator = idValidator;
        }

        internal virtual void SetSharedMemory(ApiSharedMemory newSharedMemory)
        {
            SharedMemory = newSharedMemory;
        }

        /// <summary>
        /// Set predefined event scheduler to the wrapper
        /// </summary>
        /// <param name="predefinedEventController">Predefined event scheduler object reference</param>
        internal virtual void SetPredefinedEventScheduler(ref PredefinedEventScheduler predefinedEventController)
        {
            SharedMemory.PredefinedEventScheduler = predefinedEventController;
        }

        internal virtual bool ValidateAccelByteId(string accelByteId, Utils.AccelByteIdValidator.HypensRule hypensRule, string errorMessage, ResultCallback callback)
        {
            if (!IdValidator.IsAccelByteIdValid(accelByteId, hypensRule))
            {
                Error error = new Error(ErrorCode.InvalidRequest, errorMessage);
                callback?.TryError(error);
                return false;
            }
            return true;
        }

        internal virtual bool ValidateAccelByteId<T>(string accelByteId, Utils.AccelByteIdValidator.HypensRule hypensRule, string errorMessage, ResultCallback<T> callback)
        {
            if (!IdValidator.IsAccelByteIdValid(accelByteId, hypensRule))
            {
                Error error = new Error(ErrorCode.InvalidRequest, errorMessage);
                callback?.TryError(error);
                return false;
            }
            return true;
        }
    }
}
