﻿// Copyright (c) 2023 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager

namespace AccelByte.ThirdParties.Steam
{
	public interface ISteamImp : ISteamWrapper
    {
    }
    
    internal static class SteamWrapperInfo
    {
        internal const string AccelByteServiceIdentity = "accelbyteiam";
    }
}