// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
namespace AccelByte.ThirdParties.NintendoSwitch
{
    public class NullSwitchImp : ISwitchImp
    {
        public Models.AccelByteResult<GetSwitchTokenResult, Core.Error> GetNsaToken()
        {
            var result = new Models.AccelByteResult<GetSwitchTokenResult, Core.Error>();
            result.Reject(new Core.Error(Core.ErrorCode.NotImplemented, "AccelByte Switch Extension is not installed. Please contact AccelByte support."));
            return result;
        }

        public Models.AccelByteResult<Core.Error> HandleGameExit()
        {
            var result = new Models.AccelByteResult<Core.Error>();
            result.Reject(new Core.Error(Core.ErrorCode.NotImplemented, "AccelByte Switch Extension is not installed. Please contact AccelByte support."));
            return result;
        }

        public Models.AccelByteResult<Core.Error> Initialize(string mountName)
        {
            return Initialize(mountName, false);
        }

        public Models.AccelByteResult<Core.Error> Initialize(string mountName, bool autoHandleOnExit)
        {
            var result = new Models.AccelByteResult<Core.Error>();
            result.Reject(new Core.Error(Core.ErrorCode.NotImplemented, "AccelByte Switch Extension is not installed. Please contact AccelByte support."));
            return result;
        }

        public Models.AccelByteResult<Core.Error> MountStorage(string mountName)
        {
            var result = new Models.AccelByteResult<Core.Error>();
            result.Reject(new Core.Error(Core.ErrorCode.NotImplemented, "AccelByte Switch Extension is not installed. Please contact AccelByte support."));
            return result;
        }

        public Models.AccelByteResult<Core.Error> SaveCacheFiles()
        {
            var result = new Models.AccelByteResult<Core.Error>();
            result.Reject(new Core.Error(Core.ErrorCode.NotImplemented, "AccelByte Switch Extension is not installed. Please contact AccelByte support."));
            return result;
        }

        public Models.AccelByteResult<Core.Error> UnmountStorage(string mountName)
        {
            var result = new Models.AccelByteResult<Core.Error>();
            result.Reject(new Core.Error(Core.ErrorCode.NotImplemented, "AccelByte Switch Extension is not installed. Please contact AccelByte support."));
            return result;
        }
    }
}