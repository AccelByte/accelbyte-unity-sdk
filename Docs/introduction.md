# Introduction to AccelByte Unity SDK

## Introduction

The AccelByte SDK is a specialized set of libraries and examples that seamlessly integrate with AccelByte backend services. This service gives you a convenience way to wrap web requests in function calls, so that you will be able to save more time to focus on your game while the service is working for you in the backstage. In doing so, the AccelByte SDK provides you with these key features:

- Easy to Use API
- Automatically Retry Temporary Server Errors
- Access Token Management and Session Management
- Switchable Session Management

## Key Features

### Easy to Use API

AccelByte SDK APIs uses continuation passing style for calling HTTP REST API, which pass a callback that can be used to return the value from a HTTP response. Typically, an HTTP call is abstracted as

```csharp
public void CallHttpRestApi(TReqParam1 param1, TReqParam2 param2, ResultCallback<TResponse> callback)
```

ResultCallback\<T\> is a delegate that accepts Result\<T\> as its parameter, while ResultCallback accepts Result. We use Result\<T\> type because we want to avoid exception in our code base, so we combine error code and returned value. The class Result\<T\> is generic class of T and have 3 properties:

1. Result\<T\>.IsError indicates that the result of a request is normal or has an error.
2. Result\<T\>.Error have error code and error message
3. Result\<T\>.Value have the type of T and only meaningful if Result\<T\>.IsError is false.

Because of this, making HTTP requests is as easy as calling a method.

### Automatically Retry Temporary Server Errors

Temporary server errors (HTTP status 5xx) will be automatically retried if it failed, with exponential backoff and some randomization.

### Access Token Management and Session Management

AccelByte SDK will automatically handle user access tokens expiry, by calling refresh token to get new access token at about 70% - 90% near expiration time (randomly spread). However, if you choose to use our backend services with Session Management feature provided in API Gateway, AccelByte Unity SDK will not keep raw JWT access token and refresh token. The API Gateway will create and maintain session on server side, with the game client getting a hold for user session through SessionId. This will also enable possibility for a user to logout from other device.

### Switchable Session Management

You can easily switch between using Session Management provided in API Gateway, or just directly connected to the backend. All you need to do is just change the config on **AccelByteSDKConfig.json**, look for `UseSessionManagement` and change the value to `true` or `false`. Also you need to change all the Url value on config to Url of the API Gateway. Config with Session Management switched `true` will be something like this:

```json
    {
        "ClientId":"<GAME_CLIENT_ID>",
        "ClientSecret":"<GAME_CLIENT_SECRET>",
        "UseSessionManagement": true,
        "PublisherNamespace":"<YOUR-PUBLISHER-NAMESPACE>",
        "Namespace":"<YOUR-GAME-NAMESPACE>",
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

And when it switched to `false`, the config will look like this:

```json
    {
        "ClientId":"<GAME_CLIENT_ID>",
        "ClientSecret":"<GAME_CLIENT_SECRET>",
        "UseSessionManagement": false,
        "PublisherNamespace":"<YOUR-PUBLISHER-NAMESPACE>",
        "Namespace":"<YOUR-GAME-NAMESPACE>",
        "BaseUrl": "https://preview.accelbyte.io",
        "LoginServerUrl":"https://preview.accelbyte.io/iam",
        "IamServerUrl":"https://preview.accelbyte.io/iam",
        "PlatformServerUrl":"https://preview.accelbyte.io/platform",
        "BasicServerUrl":"https://preview.accelbyte.io/basic",
        "LobbyServerUrl":"wss://preview.accelbyte.io/lobby/",
        "TelemetryServerUrl":"https://preview.accelbyte.io/telemetry",
        "RedirectUri":"http://localhost",
        "CloudStorageServerUrl":"https://preview.accelbyte.io/binary-store",
        "GameProfileServerUrl":"https://preview.accelbyte.io/soc-profile",
        "StatisticServerUrl": "https://preview.accelbyte.io/statistic"
    }
```
---