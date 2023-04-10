# AccelByte Unity SDK
The Unity SDK acts as a bridge between your game and AccelByte Cloud services, making those services easy to access.

## Getting Started

### Install the SDK via Unity Package Manager
 1. Create or open a Project
 2. Go to **Window > Package Manager > + > Add package from git URL**, paste https://github.com/AccelByte/accelbyte-unity-sdk.git into popup and click **Add**
    - If you are using Assembly Definitions in your project, you may need to add the com.accelbyte.UnitySDK assembly as a reference to your relevant definitions.
    - You must have Git installed and configured on your computer in orderfor Unity to use a UPM Package

## AccelByte Configuration
 1. Go to **AccelByte > Edit Settings** in the menu bar.
 2. Fill in the AccelByte Configuration required field using the information based on your game and click **Save**.

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
 - Nintendo

To configure multi-platform, go to **AccelByte > Edit Settings** in the menu bar, then change the **Platform** to your desired config and fill in Client Id, Client Secret for the selected platform and click **Save**.