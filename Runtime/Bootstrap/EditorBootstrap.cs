// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Api;
using System.IO;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace AccelByte.Core
{
#if UNITY_EDITOR
    [InitializeOnLoad]
#endif
    public static class EditorBootstrap
    {
        static EditorBootstrap()
        {
            UpdateSDKVersionEditorBootstrap.Execute();
        }
    }
}
