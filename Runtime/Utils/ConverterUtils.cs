// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.ComponentModel;

namespace AccelByte.Utils
{
    internal static class ConverterUtils
    {
        internal static string ByteArrayToHexString(byte[] bytes, int count, int offset = 0, bool seperateHexWithDash = false)
        {
            string output = BitConverter.ToString(bytes, offset, count);
            if (!seperateHexWithDash)
            {
                output = output.Replace("-", string.Empty);
            }
            return output;
        }

        internal static byte[] HexStringToByteArray(string hexString)
        {
            if (hexString == null)
            {
                throw new System.InvalidOperationException($"Hex string is null");
            }

            if (hexString.Length == 0 || hexString.Length % 2 != 0)
            {
                throw new System.InvalidOperationException($"Hex string length invalid: {hexString.Length}");
            }

            int length = hexString.Length;
            byte[] byteArray = new byte[length / 2];

            for (int i = 0; i < length; i += 2)
            {
                string hexValue = hexString.Substring(i, 2);
                byteArray[i / 2] = Convert.ToByte(hexValue, 16);
            }

            return byteArray;
        }

        internal static string EnumToDescription(Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            return attribute == null ? value.ToString() : attribute.Description;
        }
    }
}
