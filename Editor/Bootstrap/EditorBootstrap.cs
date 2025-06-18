// Copyright (c) 2023 - 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AccelByte.Editor
{
    [InitializeOnLoad]
    public static class EditorBootstrap
    {
        const string writeVersionFileFlag = "AccelByteWriteVersionFlag";
        
        static EditorBootstrap()
        {
            var destinationPath = Api.AccelByteSettingsV2.SDKVersionFullPath();
            bool fileExists = System.IO.File.Exists(destinationPath);
            
            if (!SessionState.GetBool(writeVersionFileFlag, false) || !fileExists)
            {
                try
                {
                    var updateVersionCommands = GetSDKUpdateVersionCommand();
                    WriteVersionFile(updateVersionCommands, destinationPath, onDone: success =>
                    {
                        if (success)
                        {
                            SessionState.SetBool(writeVersionFileFlag, true);
                        }
                    });
                }
                catch (System.Exception)
                {
                }
            }
        }

        public static List<UpdateVersionFileCommand> GetSDKUpdateVersionCommand()
        {
            var retval = new List<UpdateVersionFileCommand>();
            
            var findPackageCommands = new List<FindPackageCommand>()
            {
                new FindPackageCommand("Packages/com.accelbyte.unitysdk", "version") , 
                new FindPackageCommand("Packages/com.accelbyte.networking"), 
                new FindPackageCommand("Packages/com.accelbyte.unitysdkapple"), 
                new FindPackageCommand("Packages/com.accelbyte.unitysdkgoogle"), 
                new FindPackageCommand("Packages/com.accelbyte.unitysdkgamecore"),
                new FindPackageCommand("Packages/com.accelbyte.unitysdkps4"), 
                new FindPackageCommand("Packages/com.accelbyte.unitysdkps5"), 
                new FindPackageCommand("Packages/com.accelbyte.unitysdksteam"),
                new FindPackageCommand("Packages/com.accelbyte.unitysdkswitch")
            };

            foreach (var findPackageCommand in findPackageCommands)
            {
                var accelBytePackageInfo = UnityEditor.PackageManager.PackageInfo.FindForAssetPath(findPackageCommand.PackagePath);
                if (accelBytePackageInfo != null)
                {
                    var sdkUpdateVersionCommand = new UpdateVersionFileCommand()
                    {
                        OriginalPackageFile = accelBytePackageInfo.assetPath,
                        PackageName = accelBytePackageInfo.name,
                        OverrideVersionPackageId = findPackageCommand.OverridePackageVersionId
                    };
                    retval.Add(sdkUpdateVersionCommand);
                }
            }

            return retval;
        }

        public static void WriteVersionFile(List<UpdateVersionFileCommand> commands, string destinationPath, System.Action<bool> onDone)
        {
            if (commands != null && commands.Count > 0)
            {
                SDKVersionUpdater.UpdateVersionFile(commands, destinationPath, onDone);
            }
            else
            {
                onDone?.Invoke(false);
            }
        }

        private class FindPackageCommand
        {
            public readonly string PackagePath;
            public readonly string OverridePackageVersionId;

            public FindPackageCommand(string packagePath)
            {
                PackagePath = packagePath;
            }

            public FindPackageCommand(string packagePath, string overridePackageVersionId)
            {
                PackagePath = packagePath;
                OverridePackageVersionId = overridePackageVersionId;
            }
        }
    }
}
