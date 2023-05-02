// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using UnityEngine;
using UnityEngine.Networking;

namespace AccelByte.Core
{
    [ExecuteInEditMode]
    public class AccelByteGameThreadSignaller : MonoBehaviour
    {
        public static AccelByteGameThreadSignaller Instance
        {
            get
            {
                if(instance == null)
                {
                    GameObject signallerGameObject = Utils.AccelByteGameObject.GetOrCreateGameObject();

                    instance = signallerGameObject.GetComponent<AccelByteGameThreadSignaller>();
                    if (instance == null)
                    {
                        instance = signallerGameObject.AddComponent<AccelByteGameThreadSignaller>();
                    }
                }

                return instance;
            }
        }

        public static AccelByteGameThreadSignaller instance;

        public System.Action<float> MainThreadSignal;

        private void Start()
        {
            if(Instance != null && Instance != this)
            {
                Destroy(this);
            }
        }

        private void Update()
        {
            MainThreadSignal?.Invoke(Time.deltaTime);
        }
    };
}
