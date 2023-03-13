// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;

namespace AccelByte.Utils
{
    internal static class TimeUtils
    {
        internal static float SecondsToMilliseconds(float seconds)
        {
            return seconds * 1000;
        }

        internal static int SecondsToMilliseconds(int seconds)
        {
            int retval;
            try
            {
                float floatConversion = SecondsToMilliseconds((float)seconds);
                retval = Convert.ToInt32(floatConversion);
            }
            catch (OverflowException)
            {
                throw new OverflowException("Milliseconds value is too large for int type");
            }
            return retval;
        }

        internal static float MillisecondsToSeconds(float milliseconds)
        {
            return milliseconds / 1000;
        }

        internal static float MillisecondsToSeconds(int milliseconds)
        {
            return MillisecondsToSeconds((float)milliseconds);
        }
    }
}
