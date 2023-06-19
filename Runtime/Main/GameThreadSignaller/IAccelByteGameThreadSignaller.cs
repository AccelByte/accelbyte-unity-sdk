// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
namespace AccelByte.Core
{
    public interface IAccelByteGameThreadSignaller
    {
        public System.Action<float> GameThreadSignal
        {
            get;
            set;
        }
    }
}
