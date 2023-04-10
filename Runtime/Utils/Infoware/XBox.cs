// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Api;
using AccelByte.Core;

namespace AccelByte.Utils.Infoware
{

    //TODO : Research about Device Unique Identifier for Xbox
    internal class XB1 : InfowareUtils
    {
        internal override string GetDeviceID()
        {
            AccelByteDebug.LogWarning("Incomplete implementation for XboxOne platform");
            return "XBoxOne";
        }

        internal override string GetMacAddress()
        {
            return "";
        }
    }

    internal class XSX : InfowareUtils
    {
        internal override string GetDeviceID()
        {
            AccelByteDebug.LogWarning("Incomplete implementation for XbosSeriesX platform");
            return "XboxSeriesX";
        }

        internal override string GetMacAddress()
        {
            return "";
        }
    }
}