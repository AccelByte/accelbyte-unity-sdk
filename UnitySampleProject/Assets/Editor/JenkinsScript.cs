// Copyright (c) 2018 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using UnityEditor;

class JenkinsScript
{
    static void PerformBuild()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = new[] {"Assets/SampleScene/TestScene.unity"};
        buildPlayerOptions.locationPathName = "../output/JusticeDemoGame.exe";
        buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
        buildPlayerOptions.options = BuildOptions.None;

#if !UNITY_2018_1_OR_NEWER
        string res = BuildPipeline.BuildPlayer(buildPlayerOptions);

        if (res.Length > 0)
        {
            throw new Exception("BuildPlayer failure: " + res);
        }
#else
        UnityEditor.Build.Reporting.BuildReport res = BuildPipeline.BuildPlayer(buildPlayerOptions);
        if (res.summary.totalErrors > 0)
        {
            throw new Exception(string.Format("BuildPlayer failure: " + res.summary.result));
        }
#endif
    }
}