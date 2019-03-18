using System.Runtime.InteropServices;
using UnityEngine;

namespace Oculus.Platform
{
  public class CallbackRunner : MonoBehaviour
  {
    [DllImport(CAPI.DLL_NAME)]
    static extern void ovr_UnityResetTestPlatform();

    public bool IsPersistantBetweenSceneLoads = true;

    void Awake()
    {
      var existingCallbackRunner = Object.FindObjectOfType<CallbackRunner>();
      if (existingCallbackRunner != this)
      {
        Debug.LogWarning("You only need one instance of CallbackRunner");
      }
      if (this.IsPersistantBetweenSceneLoads)
      {
        Object.DontDestroyOnLoad(this.gameObject);
      }
    }

    void Update()
    {
      Request.RunCallbacks();
    }

    void OnDestroy()
    {
#if UNITY_EDITOR
      CallbackRunner.ovr_UnityResetTestPlatform();
#endif
    }
  }
}
