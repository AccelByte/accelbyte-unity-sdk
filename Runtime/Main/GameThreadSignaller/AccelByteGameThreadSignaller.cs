// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System;
using UnityEngine;

namespace AccelByte.Core
{
    [ExecuteInEditMode]
    public class AccelByteGameThreadSignaller : MonoBehaviour, IAccelByteGameThreadSignaller
    {
        public Action<float> GameThreadSignal
        {
            get;
            set;
        }

        private void Update()
        {
            GameThreadSignal?.Invoke(Time.unscaledDeltaTime);
        }
    }}
