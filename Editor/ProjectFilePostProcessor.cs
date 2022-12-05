using AccelByte.Api;
using System.Diagnostics;
using System.IO;
using System;
using UnityEditor;
using UnityEngine;

public class ProjectFilePostprocessor : AssetPostprocessor
{
    public static string OnGeneratedSlnSolution(string path, string content)
    {
        //process solution content
        return content;
    }

    public static string OnGeneratedCSProject(string path, string content)
    {
        var projectPath = Directory.GetCurrentDirectory();
        var AccelBytePackageInfo = UnityEditor.PackageManager.PackageInfo.FindForAssembly(typeof(AccelByteSettings).Assembly);
        var folderName = Path.GetRelativePath(projectPath, AccelBytePackageInfo.assetPath);

        string editorProjectConfigFindStr = string.Format("<None Include=\"{0}\\Editor\\com.accelbyte.UnitySDKEditor.asmdef\" />", folderName);
        string editorProjectConfigReplaceStr = string.Format("<None Include=\"{0}\\.editorconfig\" />\r\n    <None Include=\"{0}\\Editor\\com.accelbyte.UnitySDKEditor.asmdef\" />", folderName);
        string runtimeProjectConfigFindStr = string.Format("<None Include=\"{0}\\Runtime\\com.accelbyte.UnitySDK.asmdef\" />", folderName);
        string runtimeProjectConfigReplaceStr = string.Format("<None Include=\"{0}\\.editorconfig\" />\r\n    <None Include=\"{0}\\Runtime\\com.accelbyte.UnitySDK.asmdef\" />", folderName);

        if (path.IndexOf("com.accelbyte.UnitySDKEditor") > 0)
        {
           content = content.Replace(editorProjectConfigFindStr, editorProjectConfigReplaceStr);
        }
        else
        {
           content = content.Replace(runtimeProjectConfigFindStr, runtimeProjectConfigReplaceStr);
        }
        return content;
    }
}