# AccelByte Unity SDK

## Dependencies

AccelByte SDK v5.0.0 is compatible with these AccelByte Services version 3.3.1:

| Service                                             | Version        |
|-----------------------------------------------------|----------------|
| justice-iam-service                                 | 4.3.0          |
| justice-legal-service                               | 1.12.1         |
| justice-basic-service                               | 1.21.1         |
| justice-platform-service                            | 3.32.0         |
| justice-social-service                              | 1.17.1         |
| justice-leaderboard-service                         | 2.8.0          |
| justice-achievement-service                         | 2.5.4          |
| justice-cloudsave-service                           | 1.6.4          |
| justice-ugc-service                                 | 1.4.0          |
| justice-lobby-server                                | 1.34.0         |
| justice-group-service                               | 2.7.3          |
| Justice QoS Manager Service                         | 1.7.6          |
| justice-dedicated-server-manager-controller-service | 2.4.1          |
| amalgam_game_telemetry                              | 1.1.2-392420e1 |          

## Getting Started

### Preparation

1. Download the whole repository.
2. Create a file named **AccelByteSDKConfig.json** and add it to *Assets\Resources* directory. Fill the _PublisherNamespace_, _ClientId_, _ClientSecret_, _Namespace_ and _AppId_(app namespace), _BaseUrl_, and _RedirectUri_ according to your own. Here's the content of the json file:

```json
{
    "ClientId":"<GAME_CLIENT_ID>",
    "ClientSecret":"<GAME_CLIENT_SECRET>",
    "PublisherNamespace":"<PUBLISHER_NAMESPACE>",
    "Namespace":"<GAME_CLIENT_NAMESPACE>",
    "BaseUrl": "https://api-preview.accelbyte.io",
    "RedirectUri":"http://localhost",
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

# Commit Message Standardization
We are following Conventional Commits as a standard to follow for writing commit messages. The details of the rules can be found in the [conventional commits website](https://www.conventionalcommits.org/en/v1.0.0/). 

We optionally provide a git commit-hook that will be triggered when you do a commit through the terminal that will execute an interactive cli that can guide you to write commit message that follows the standard, to enable this, run `make SetupCommitHook`