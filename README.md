# AccelByte Unity SDK v2.1.0

## Dependencies

AccelByte SDK is compatible with these AccelByte backend services:

| Service              | Version              |
|----------------------|----------------------|
| IAM                  | v3.1                 |
| Platform/E-Commerce  | v1.8                 |
| Basic                | v1.0                 |
| cloudstorage-service | v0.3.4               |
| lobby-server         | v1.0.13              |
| telemetry-service    | v1.0.4               |
| soc-profile-service  | v0.1.5               |
| statistic-service    | v0.8.2               |

## Getting Started

### Preparation

1. Download the whole repository.
2. Create a file named **AccelByteSDKConfig.json** and add it to *Assets\Resources* directory. Fill the _PublisherNamespace_, _ClientId_, _ClientSecret_, _Namespace_ and _AppId_(app namespace), _BaseUrl_, and _RedirectUri_ according to your own. Here's the content of the json file:

```json
{
    "ClientId":"<GAME_CLIENT_ID>",
    "ClientSecret":"<GAME_CLIENT_SECRET>",
    "UseSessionManagement": true,
    "PublisherNamespace":"<PUBLISHER_NAMESPACE>",
    "Namespace":"<GAME_CLIENT_NAMESPACE>",
    "BaseUrl": "https://api-preview.accelbyte.io",
    "LoginServerUrl":"https://api-preview.accelbyte.io",
    "IamServerUrl":"https://api-preview.accelbyte.io/iam",
    "PlatformServerUrl":"https://api-preview.accelbyte.io/platform",
    "BasicServerUrl":"https://api-preview.accelbyte.io/basic",
    "LobbyServerUrl":"wss://preview.accelbyte.io/lobby/",
    "TelemetryServerUrl":"https://api-preview.accelbyte.io/telemetry",
    "RedirectUri":"http://localhost",
    "CloudStorageServerUrl":"https://api-preview.accelbyte.io/binary-store",
    "GameProfileServerUrl":"https://api-preview.accelbyte.io/soc-profile",
    "StatisticServerUrl": "https://api-preview.accelbyte.io/statistic"
}
```

3. Copy whole *Assets*/**AccelByte** folder from the repository that has been downloaded and add it to your project in the *Assets* directory.
4. If you need other functionalities, e.g. Steamworks, Xsolla, Oculus, embedded browser; you can include this whole repository to your project because we already provide those plugin/package here.
5. For cross platform (primarily iOS and WebGL) and performance reason (Utf8Json is fast), AccelByte Unity SDK uses Utf8Json for serializing objects to JSON. The serialization code is pre-generated, so you need to run RecreateJsonResolver.bat if you somehow change any of AccelByte models class inside Assets/AccelByte/Models.

### Using The SDK

At first, you need to import AccelByte namespaces:

```csharp
using AccelByte.Api;
using AccelByte.Models;
using AccelByte.Core;
```

After that, you can freely uses AccelByte SDK. Please see [tutorial](docs/tutorial.md) on how to use AccelByte SDK.
