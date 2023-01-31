using AccelByte.Api;
using System.Diagnostics;
using System.IO;
using System;
using System.Text;
using UnityEditor;
using UnityEngine;

public class ProjectFilePostprocessor : AssetPostprocessor
{
    public static string OnGeneratedSlnSolution(string path, string content)
    {
        //process solution content
        return content;
    }
    
    private static String MakeRelativePath(String fromPath, String toPath)
    {
        if (String.IsNullOrEmpty(fromPath)) throw new ArgumentNullException("fromPath");
        if (String.IsNullOrEmpty(toPath))   throw new ArgumentNullException("toPath");

        StringBuilder fromPathBuilder = new StringBuilder();
        fromPathBuilder.Append("file:///");
        fromPathBuilder.Append(fromPath);

        StringBuilder toPathBuilder = new StringBuilder();
        toPathBuilder.Append("file:///");
        toPathBuilder.Append(toPath);
        
        Uri fromUri = new Uri(fromPathBuilder.ToString());
        Uri toUri = new Uri(toPathBuilder.ToString());
        
        if (fromUri.Scheme != toUri.Scheme) { return toPath; } // path can't be made relative.

        Uri relativeUri = fromUri.MakeRelativeUri(toUri);
        String relativePath = Uri.UnescapeDataString(relativeUri.ToString());

        if (toUri.Scheme.Equals("file", StringComparison.InvariantCultureIgnoreCase))
        {
            relativePath = relativePath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
        }

        return relativePath;
    }

    public static string OnGeneratedCSProject(string path, string content)
    {
        var projectPath = Directory.GetCurrentDirectory();
        var AccelBytePackageInfo = UnityEditor.PackageManager.PackageInfo.FindForAssembly(typeof(AccelByteSettingsV2).Assembly);
        var folderName = MakeRelativePath(projectPath, AccelBytePackageInfo.assetPath);
        

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