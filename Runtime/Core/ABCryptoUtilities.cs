// Copyright (c) 2022 - 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Text;

namespace AccelByte.Core
{
    public static class ABCryptoUtilities
    {
        public static string PrintBytesToString(byte[] inData)
        {
            return BitConverter.ToString(inData ?? new byte[0]).Replace("-", "");
        }

        public static byte[] StringToBytes(string inData)
        {
            // Check arguments.
            if (string.IsNullOrEmpty(inData))
            {
                return null;
            }

            // low style code(like unreal engine)
            byte[] outData = new byte[inData.Length];
            for (int index = 0; index < inData.Length; ++index)
            {
                outData[index] = (byte)(inData[index]);
            }

            return outData;
        }

        public static string BytesToString(byte[] inData)
        {
            // Check arguments.
            if ((inData is null) || (inData.Length <= 0))
            {
                return null;
            }

            // low style code(like unreal engine)
            StringBuilder strBuilder = new StringBuilder(inData.Length);
            strBuilder.Clear();

            for (int index = 0; index < inData.Length; ++index)
            {
                strBuilder.Append((char)(inData[index]));
            }

            return strBuilder.ToString();
        }

        public static byte[] StringToBytesWithoutNull(string inData)
        {
            // Check arguments.
            if (string.IsNullOrEmpty(inData))
            {
                return null;
            }

            // low style code(like unreal engine)
            byte[] outData = new byte[inData.Length];
            int count = 0;
            for (int index = 0; index < inData.Length; ++index)
            {
                if (inData[index] == '\0')
                {
                    continue;
                }
                outData[count++] = (byte)(inData[index]);
            }

            return outData;
        }

        public static string BytesToStringWithoutNull(byte[] inData)
        {
            // Check arguments.
            if ((inData is null) || (inData.Length <= 0))
            {
                return null;
            }

            // low style code(like unreal engine)
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Clear();

            for (int index = 0; index < inData.Length; ++index)
            {
                if (inData[index] == '\0')
                {
                    continue;
                }
                strBuilder.Append((char)(inData[index]));
            }

            return strBuilder.ToString();
        }

        public static byte[] BytesToBytesWithoutNull(byte[] inData)
        {
            // Check arguments.
            if ((inData is null) || (inData.Length <= 0))
            {
                return null;
            }

            // low style code(like unreal engine)
            int count = 0;
            byte[] newData = new byte[inData.Length];
            for (int index=0; index < inData.Length; ++index)
            {
                if (inData[index] == '\0')
                {
                    continue;
                }
                newData[count++] = (byte)(inData[index]);
            }

            byte[] newBytes = new byte[count];
            Buffer.BlockCopy(newData, 0, newBytes, 0, count);

            return newBytes;
        }
    }
}

