// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager
namespace AccelByte.ThirdParties.NintendoSwitch
{
    public interface ISwitchImp
    {
        public Models.AccelByteResult<Core.Error> Initialize(string mountName);
        public Models.AccelByteResult<Core.Error> MountStorage(string mountName);
        public Models.AccelByteResult<Core.Error> UnmountStorage(string mountName);
        public Models.AccelByteResult<GetSwitchTokenResult, Core.Error> GetNsaToken();
    }
}