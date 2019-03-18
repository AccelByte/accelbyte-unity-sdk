
# Getting Started

## Introduction
## Preparation

1. Download the whole repository.
2. Create a file named **AccelByteSDKConfig.json** and add it to *Assets\Resources* directory. Fill the _PublisherNamespace_, _ClientId_, _ClientSecret_, _Namespace_ and _AppId_(app namespace), _BaseUrl_, and _RedirectUri_ according to your own. Here's the content of the json file:

```json
{
    "PublisherNamespace":"<YOUR-PUBLISHER-NAMESPACE>",
    "AppId":"<YOUR-APP-ID>",
    "Namespace":"<YOUR-GAME-NAMESPACE>",
    "IamServerUrl":"<IAM-SERVER-URL>",
    "PlatformServerUrl":"<PLATFORM-SERVER-URL>",
    "BasicServerUrl":"<BASIC-SERVER-URL>",
    "LobbyServerUrl":"<LOBBY-SERVER-URL>",
    "TelemetryServerUrl":"<TELEMETRY-SERVER-URL>",
    "CloudStorageServerUrl":"<CLOUDSTORAGE-SERVER-URL>",
    "ClientId":"<YOUR-CLIENT-ID>",
    "ClientSecret":"<YOUR-CLIENT-SECRET>",
    "RedirectUri":"https://localhost/"
}
```

3. Copy whole *Assets*/**AccelByte** folder from the repository that has been downloaded and add it to your project in the *Assets* directory.
4. If you need other functionalities, e.g. Steamworks, Xsolla, Oculus, embedded browser; you can include this whole repository to your project because we already provide those plugin/package here.

## Using The SDK

At first, you need to import AccelByte namespaces:

```csharp
using AccelByte.Api;
using AccelByte.Models;
using AccelByte.Core;
```

After that, you can freely uses AccelByte SDK. Please see [tutorial](tutorial.md) on how to use AccelByte SDK.
