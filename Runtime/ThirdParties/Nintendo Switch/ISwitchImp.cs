// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager

using System;

namespace AccelByte.ThirdParties.NintendoSwitch
{
    public interface ISwitchImp
    {
        public Models.AccelByteResult<Core.Error> Initialize(string mountName);
        [Obsolete("This interface will be removed on August release.")]
        public Models.AccelByteResult<Core.Error> Initialize(string mountName, bool autoHandleOnExit);
        [Obsolete("This interface not supported anymore and will be removed on August release.")]
        public Models.AccelByteResult<Core.Error> MountStorage(string mountName);
        [Obsolete("This interface not supported anymore and will be removed on August release.")]
        public Models.AccelByteResult<Core.Error> UnmountStorage(string mountName);
        [Obsolete("Please use GetNsaToken(string) instead. This interface will be removed on August release.")]
        public Models.AccelByteResult<GetSwitchTokenResult, Core.Error> GetNsaToken();
        public Models.AccelByteResult<GetSwitchTokenResult, Core.Error> GetNsaToken(string userHandle);
        public Models.AccelByteResult<Core.Error> SaveCacheFiles();
        public Models.AccelByteResult<Core.Error> HandleGameExit();
    }
}