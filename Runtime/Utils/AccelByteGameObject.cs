using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AccelByte.Utils
{
    internal static class AccelByteGameObject
    {
        private static GameObject sdkGameObject;

        private static string sdkGameObjectName = "AccelByteDummyGameObject";
        static internal GameObject GetOrCreateGameObject()
        {
            if(sdkGameObject == null)
            {
                sdkGameObject = GameObject.Find(sdkGameObjectName);
            }

            if (sdkGameObject == null)
            {
                sdkGameObject = new GameObject(sdkGameObjectName);
            }

            return sdkGameObject;
        }
    }
}
