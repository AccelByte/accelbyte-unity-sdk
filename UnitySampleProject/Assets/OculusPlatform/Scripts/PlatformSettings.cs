namespace Oculus.Platform
{
  using UnityEngine;
  using System.Collections;

#if UNITY_EDITOR
  [UnityEditor.InitializeOnLoad]
#endif
  public sealed class PlatformSettings : ScriptableObject
  {
    public static string AppID
    {
      get { return PlatformSettings.Instance.ovrAppID; }
      set { PlatformSettings.Instance.ovrAppID = value; }
    }

    public static string MobileAppID
    {
      get { return PlatformSettings.Instance.ovrMobileAppID; }
      set { PlatformSettings.Instance.ovrMobileAppID = value; }
    }

    public static bool UseStandalonePlatform
    {
      get { return PlatformSettings.Instance.ovrUseStandalonePlatform; }
      set { PlatformSettings.Instance.ovrUseStandalonePlatform = value; }
    }

    [SerializeField]
    private string ovrAppID = "";

    [SerializeField]
    private string ovrMobileAppID = "";

#if UNITY_EDITOR_WIN
    [SerializeField]
    private bool ovrUseStandalonePlatform = false;
#else
    [SerializeField]
    private bool ovrUseStandalonePlatform = true;
#endif

    private static PlatformSettings instance;
    public static PlatformSettings Instance
    {
      get
      {
        if (PlatformSettings.instance == null)
        {
          PlatformSettings.instance = Resources.Load<PlatformSettings>("OculusPlatformSettings");

          // This can happen if the developer never input their App Id into the Unity Editor
          // and therefore never created the OculusPlatformSettings.asset file
          // Use a dummy object with defaults for the getters so we don't have a null pointer exception
          if (PlatformSettings.instance == null)
          {
            PlatformSettings.instance = ScriptableObject.CreateInstance<PlatformSettings>();

#if UNITY_EDITOR
            // Only in the editor should we save it to disk
            string properPath = System.IO.Path.Combine(UnityEngine.Application.dataPath, "Resources");
            if (!System.IO.Directory.Exists(properPath))
            {
              UnityEditor.AssetDatabase.CreateFolder("Assets", "Resources");
            }

            string fullPath = System.IO.Path.Combine(
              System.IO.Path.Combine("Assets", "Resources"),
              "OculusPlatformSettings.asset"
            );
            UnityEditor.AssetDatabase.CreateAsset(PlatformSettings.instance, fullPath);
#endif
          }
        }
        return PlatformSettings.instance;
      }

      set
      {
        PlatformSettings.instance = value;
      }
    }
  }
}
