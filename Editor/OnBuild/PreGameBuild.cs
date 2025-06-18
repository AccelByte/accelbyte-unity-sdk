// Copyright (c) 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Editor;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace AccelByte.Build
{
    public class PreGameBuild : IPreprocessBuildWithReport
    {
        public int callbackOrder
        {
            get
            {
                return 0;
            }
        }

        public void OnPreprocessBuild(BuildReport report)
        {
            var commandList = EditorBootstrap.GetSDKUpdateVersionCommand();
            var destinationPath = Api.AccelByteSettingsV2.SDKVersionFullPath();
            EditorBootstrap.WriteVersionFile(commandList, destinationPath, onDone: (success) =>
            {
                    
            });
        }
    }
}