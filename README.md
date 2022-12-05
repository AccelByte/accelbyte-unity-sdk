# AccelByte Unity SDK
The Unity SDK acts as a bridge between your game and AccelByte Cloud services, making those services easy to access.

## Getting Started

### Install the SDK via Unity Package Manager
 1. Create or open a Project
 2. Go to **Window > Package Manager > + > Add package from git URL**, paste https://github.com/AccelByte/accelbyte-unity-sdk.git into popup and click **Add**
    - If you are using Assembly Definitions in your project, you may need to add the com.accelbyte.UnitySDK assembly as a reference to your relevant definitions.
    - You must have Git installed and configured on your computer in orderfor Unity to use a UPM Package

### Project Settings
Now that the SDK has been installed in the project, you will need to configure your project settings in order for the SDK to work on your project’s build standalone.
 1. In Unity Editor, go to **File > Build Settings** and click **Player Settings.**
 2. Select **Other Settings** to expand the options.
 3. In **Configurations** section, change the API Compatibility Level to **.NET 4.x** in the drop-down menu.

## AccelByte Configuration
 1. Go to **AccelByte > Edit Settings** in the menu bar.
 2. Fill in the AccelByte Configuration field using the information based on your game and click **Save**.
 3. Alternatively, you can create a file called `AccelByteSDKConfig.json` in your **Assets/Resources** folder and fill it with the appropriate information in the following format:

```json
{
	"Default": {
		"Namespace": "Game Namespace",
		"UsePlayerPrefs": true,
		"EnableDebugLog": true,
		"DebugLogFilter": "Log",
		"BaseUrl": "<baseURL>",
		"RedirectUri": "http://127.0.0.1",
		"AppId": "",
		"PublisherNamespace": "",
		"CustomerName": ""
	}
}
```

### Multi Environment Configuration
AccelByte enables you to use different environments such as **Production**, **Certification**, **Default**, and **Development**, within one single project, meaning that you only need to build your game once. By using our SDK, you will be able to switch environments even when your build is running. For example, you can run and test your build in the **Certification** environment and then publish your game to the **Production** environment, all without having to rebuild your game for each separate environment

To configure multi environment, go to **AccelByte > Edit Settings** in the menu bar, then change the **Environment** to your desired config and fill in the AccelByte Configuration field using the information based on your game and click **Save**.

### Multi Platform Configuration
AccelByte’s Multi-platform Credentials allow you to build your game with different AccelByte credentials for each platform, eliminating the need to change credentials every time the build setting is changed to other platforms. Our supported platforms include:
 - Steam (Windows/Linux build)
 - Epic Games (Windows/Linux build) 
 - Apple
 - iOS
 - Android
 - PS4
 - PS5
 - Live (Xbox build)
 - Nintendo
 - Stadia

To configure multi-platform, go to **AccelByte > Edit Settings** in the menu bar, then change the **Platform** to your desired config and fill in Client Id, Client Secret for the selected platform and click **Save**.