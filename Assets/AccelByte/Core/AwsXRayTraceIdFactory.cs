// Copyright (c) 2018 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;

namespace AccelByte.Core
{
    internal static class AwsXRayTraceIdFactory
    {
        internal static string GetNewXRayTraceId()
        {
            const int version = 1;
            uint unixTimestamp = (uint) (DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            string guidString = Guid.NewGuid().ToString("N").Substring(0, 24);
            return version + "-" + unixTimestamp.ToString("X") + "-" + guidString;
        }
    }
}