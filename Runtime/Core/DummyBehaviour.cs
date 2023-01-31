// Copyright (c) 2019-2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using UnityEngine;
using Object = UnityEngine.Object;

namespace AccelByte.Core
{
    public class DummyBehaviour : MonoBehaviour
    {
        private void Awake()
        {
            Object.DontDestroyOnLoad(this.gameObject);
        }
    }
}