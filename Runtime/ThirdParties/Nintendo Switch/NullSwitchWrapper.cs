// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
namespace AccelByte.ThirdParties.NintendoSwitch
{
    public class NullSwitchWrapper : ISwitchWrapper
    {
        public void Initialize(string mountName)
        {
            
        }

        public bool MountStorage(string mountName)
        {
            return false;
        }

        public bool UnmountStorage(string mountName)
        {
            return false;
        }
    }
}