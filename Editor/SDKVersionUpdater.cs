// Copyright (c) 2023 - 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Scripting;

namespace AccelByte.Editor
{
    public static class SDKVersionUpdater
    {
        public static System.Action OnWriteNewVersionFileSuccess;

        internal static void UpdateVersionFile(List<UpdateVersionFileCommand> parameters, string path, System.Action<bool> onDone = null)
        {
            if (parameters == null || string.IsNullOrEmpty(path))
            {
                onDone?.Invoke(false);
                return;
            }
            
            IFileStream fs = new AccelByteFileStream();
            var sdkVersionJson = new JObject();
            foreach (var parameter in parameters)
            {
                try
                {
                    var versionText =
                        UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>($"{parameter.OriginalPackageFile}/package.json");
                    var versionObject = versionText.text.ToObject<UnityPackage>();
                    string packageVersion = versionObject.Version;
                    string packageId = !string.IsNullOrEmpty(parameter.OverrideVersionPackageId)
                        ? parameter.OverrideVersionPackageId
                        : parameter.PackageName;
                    sdkVersionJson[packageId] = packageVersion;
                }
                catch (Exception)
                {
                }
            }
            
            string directoryName = System.IO.Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(directoryName) && !System.IO.Directory.Exists(directoryName))
            {
                System.IO.Directory.CreateDirectory(directoryName);
            }

            fs.WriteFile(formatter: null,  content: sdkVersionJson.ToString(), path: path, instantWrite: true, onDone: (isSuccess) =>
            {
                if (isSuccess)
                {
                    OnWriteNewVersionFileSuccess?.Invoke();
                    onDone?.Invoke(true);
                }
                else
                {
                    onDone?.Invoke(false);
                }
            });
        }

        [DataContract, Preserve]
        private class UnityPackage
        {
            [DataMember(Name = "version")] public string Version;
        }
    }
        
    public class UpdateVersionFileCommand
    {
        public string PackageName;
        public string OriginalPackageFile;
        public string OverrideVersionPackageId;
    }
}

