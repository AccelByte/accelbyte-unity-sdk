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
        Utils.AccelByteIdValidator accelByteIdValidator;
        protected Utils.AccelByteIdValidator IdValidator
        {
            get
            {
                if(accelByteIdValidator == null)
                {
                    accelByteIdValidator = new Utils.AccelByteIdValidator();
                }
                return accelByteIdValidator;
            }
        }

        internal void SetAccelByteIdValidator(Utils.AccelByteIdValidator idValidator)
        {
            accelByteIdValidator = idValidator;
        }

        internal virtual bool ValidateAccelByteId(string accelByteId, Utils.AccelByteIdValidator.HypensRule hypensRule, string errorMessage, ResultCallback callback)
        {
            if (!IdValidator.IsAccelByteIdValid(accelByteId, hypensRule))
            {
                Error error = new Error(ErrorCode.InvalidRequest, errorMessage);
                callback.TryError(error);
                return false;
            }
            return true;
        }

        internal virtual bool ValidateAccelByteId<T>(string accelByteId, Utils.AccelByteIdValidator.HypensRule hypensRule, string errorMessage, ResultCallback<T> callback)
        {
            if (!IdValidator.IsAccelByteIdValid(accelByteId, hypensRule))
            {
                Error error = new Error(ErrorCode.InvalidRequest, errorMessage);
                callback.TryError(error);
                return false;
            }
            return true;
        }
    }
}
