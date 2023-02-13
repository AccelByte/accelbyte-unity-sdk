// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace AccelByte.Utils
{
    internal static class AccelByteFileUtils
    {
        internal static string ReadTextFileFromResource(string resourceFilePath)
        {
            string returnValue = null;
            var textFile = Resources.Load(resourceFilePath);
            if(textFile != null)
            {
                try
                {
                    returnValue = ((TextAsset)textFile).text;
                }
                catch (System.Exception)
                {
                    AccelByteDebug.LogWarning($"Failed to read resources file: {resourceFilePath}");
                }
            }

            return returnValue;
        }

        internal static JObject ReadJson(string jsonText)
        {
            if (!string.IsNullOrEmpty(jsonText))
            {
                return JObject.Parse(jsonText);
            }

            return new JObject();
        }
    }
}
