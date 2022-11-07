# Changelog

All notable changes to this project will be documented in this file. See [standard-version](https://github.com/conventional-changelog/standard-version) for commit guidelines.

## [15.5.0](https://github.com/AccelByte/accelbyte-unity-sdk/branches/compare/15.5.0%0D15.4.0) (2022-11-07)


### Features

* fixed subscription test that always fail ([499244f](https://github.com/AccelByte/accelbyte-unity-sdk/commits/499244f9f4b78e24cd01e18e4cb7888a602e5c62))
* OPTIONBOX on EAccelByteEntitlementClass ([4b3c67a](https://github.com/AccelByte/accelbyte-unity-sdk/commits/4b3c67a883811f7367a41bf6e8fc9d43ed8768a5))
* username field in the BaseUserInfo function ([9cf8eb5](https://github.com/AccelByte/accelbyte-unity-sdk/commits/9cf8eb5ccfb7c34a9d61b08a3e41fe90b75565ad))

## [15.4.0](https://github.com/AccelByte/accelbyte-unity-sdk/branches/compare/15.4.0%0D15.3.0) (2022-10-24)


### Features

* Bulk get user season progression API ([fb970e8](https://github.com/AccelByte/accelbyte-unity-sdk/commits/fb970e83dcfb39d83f26be259ef4366e32d06b04))
* deprecated endpoint /iam/oauth/token, added new end point /iam/v3/oauth/verify ([066de9a](https://github.com/AccelByte/accelbyte-unity-sdk/commits/066de9acd87bc9b4000411e9c990981cff436323))
* deprecated endpoint /iam/oauth/token, added new end point /iam/v3/oauth/verify ([ceedce0](https://github.com/AccelByte/accelbyte-unity-sdk/commits/ceedce00870b360671e8c8aa1146126627730c36))
* Expose update channel public endpoint ([1bf155b](https://github.com/AccelByte/accelbyte-unity-sdk/commits/1bf155bd5453fc661c630364e871c89cfd72bb5d))
* IAM Phase 3 Statement & Substitute Implementation ([9b680ee](https://github.com/AccelByte/accelbyte-unity-sdk/commits/9b680ee72b6e80e014212cf4995488874dffedee))
* IAM Phase 4 Statement & Substitute Implementation ([1ddf21f](https://github.com/AccelByte/accelbyte-unity-sdk/commits/1ddf21f72cea41d617a207db3092497c95d00c89))
* UnitTest/Sample Code SyncSteamDLC ([10a8470](https://github.com/AccelByte/accelbyte-unity-sdk/commits/10a847040e33fbacd72d845e30248557d0bf2dbc))


### Bug Fixes

* adopt the changes from lobby session refactoring ([881fd6d](https://github.com/AccelByte/accelbyte-unity-sdk/commits/881fd6d4ff1eaf84255bafb194b9c01d3aa173c2))
* client sdk config type misplaced ([228295c](https://github.com/AccelByte/accelbyte-unity-sdk/commits/228295c60b2fab59b160358dbb9809172612d190))
* comment out matchmaking code to avoid build test crashes ([c69679e](https://github.com/AccelByte/accelbyte-unity-sdk/commits/c69679e723574202ddd2d5c7db775c4f10bb1445))
* comment out protobuf messaage notification handling ([4889e85](https://github.com/AccelByte/accelbyte-unity-sdk/commits/4889e858643f51f448bbe06ffed457a682104c2e))
* fix wrong purchase condition model in item api BREAKING CHANGES: updated item info model ([a94338c](https://github.com/AccelByte/accelbyte-unity-sdk/commits/a94338c0152e19da5e3f1900b20c42f24d226829))
* **lobby:** fix presence message still uri escaped on FriendsStatusChanged and ListOnlineFriends ([71e9044](https://github.com/AccelByte/accelbyte-unity-sdk/commits/71e90447f5d670d98bc4b039bb3f0703fbd0ce0d))
* missing dependency after rebase ([02d8eec](https://github.com/AccelByte/accelbyte-unity-sdk/commits/02d8eeccb0e558799b86977f87fcf258d2d1fca3))
* missing request model for promote member to party leader ([b2765d0](https://github.com/AccelByte/accelbyte-unity-sdk/commits/b2765d07a73e770708df9e1c7d54cef29a484ec0))
* mistaken refactor of session model and server config ([7509b78](https://github.com/AccelByte/accelbyte-unity-sdk/commits/7509b78a24cf0d49c8ede65928eb2d47029f133c))
* rename typo, confusing namespace, remove UpdateGameSession ([6e511ba](https://github.com/AccelByte/accelbyte-unity-sdk/commits/6e511ba56997b46972a63f75cf51c82f23207b32))
* session info replace session member and ds status changed notif ([9d8dda4](https://github.com/AccelByte/accelbyte-unity-sdk/commits/9d8dda4ba2fc79f51f9e21e210eea06189bae05b))
* SessionV2DsInformation can has null requestedAt ([49f2000](https://github.com/AccelByte/accelbyte-unity-sdk/commits/49f2000b0510ac8cc6ebb7af95828f95669408ca))
* typo in Lobby.cs of session v2 notification ([e1a5249](https://github.com/AccelByte/accelbyte-unity-sdk/commits/e1a524939b56a4b0e89286ae6bf547f9ba38c310))
* typo in ServerMatchmakingApi.cs ([3870417](https://github.com/AccelByte/accelbyte-unity-sdk/commits/38704173545e296c9c2c83ea89455b3bbcab1892))


### Tests

* Refactor and improved party test coverage and quality ([4750084](https://github.com/AccelByte/accelbyte-unity-sdk/commits/4750084ce098e1a1d614d315e4810ff36f8ac58f))


### Refactors

* handle session notifications as json instead of protobuf ([f1cba09](https://github.com/AccelByte/accelbyte-unity-sdk/commits/f1cba09db43fbf959b744e3b0b3c352cdaccf0cb))
* **lobby:** move websocket api call into a new class ([8dfcf60](https://github.com/AccelByte/accelbyte-unity-sdk/commits/8dfcf60c495d0ae1b5679a69e3ab38af68dedfcc))
* MMv2 RejectBackfillProposal use backfill proposal notification directly ([7db3057](https://github.com/AccelByte/accelbyte-unity-sdk/commits/7db3057e530537a5d63c420666d8410218a85c56))
* remove protobuf library and generated class ([93ed4b7](https://github.com/AccelByte/accelbyte-unity-sdk/commits/93ed4b704659569a83ef25f47c804d7438f698d3))

## [15.3.0](https://github.com/AccelByte/accelbyte-unity-sdk/branches/compare/15.3.0%0D15.2.0) (2022-10-10)


### Features

*  Bruteforce protection upon game login ([f24e935](https://github.com/AccelByte/accelbyte-unity-sdk/commits/f24e9354684ff2ad9c609128ea98bf2b3ef537e2))
* adjustment for improve entitlement track log ([8a98deb](https://github.com/AccelByte/accelbyte-unity-sdk/commits/8a98debbf8dda2d38cabdb195a597a3f4af03951))
* banning by device id (PSN, XBOX) ([0dd98fc](https://github.com/AccelByte/accelbyte-unity-sdk/commits/0dd98fc0b9d72ca36a0659154d9f99b05e559aab))
* expose Get and Update PrivateCustomAttributes ([7adc696](https://github.com/AccelByte/accelbyte-unity-sdk/commits/7adc696c413a9737cfbc5646c5666fdf563b407f))
* **HTTP:** adding SDK version to the resource and add HTTP headers metadata [sdk version, game client version, namespace] ([fb041ee](https://github.com/AccelByte/accelbyte-unity-sdk/commits/fb041ee42190450100068f6664f96729c0c7625a))
* **lobby:** add send party notif ([a1f36ed](https://github.com/AccelByte/accelbyte-unity-sdk/commits/a1f36eded46e54ec02ffde8d4bef30071e5ea1f2))
* **lobby:** support for role based matchmaking ([835704f](https://github.com/AccelByte/accelbyte-unity-sdk/commits/835704f68e556c8e8272ada52baf7625a5481492))
* **lobby:** support rejected status on role based matchmaking ([2f30c17](https://github.com/AccelByte/accelbyte-unity-sdk/commits/2f30c1732ad7a87f53499aedfbd7bbacb3d291df))
* Temporarily cache telemetry data into the local disk storage ([7676ad8](https://github.com/AccelByte/accelbyte-unity-sdk/commits/7676ad85e33bf8ea8139ce2b4525a0d91df7210e))


### Bug Fixes

* **lobby:** fix presence message still uri escaped on FriendsStatusChanged and ListOnlineFriends ([d546342](https://github.com/AccelByte/accelbyte-unity-sdk/commits/d546342b3ad5cb68c923e22d10a34bd0c5aa2f74))
* support get wallet info old workflow ([e59db17](https://github.com/AccelByte/accelbyte-unity-sdk/commits/e59db17b72b7e4d53823ef4e76f55df4d70f7752))

## [15.2.0](https://github.com/AccelByte/accelbyte-unity-sdk/branches/compare/15.2.0%0D15.1.0) (2022-09-27)


### Features

* AccelByteUnitySdk Plug-in's GetWalletInfoByCurrencyCode returns a partial response ([bd3c2ce](https://github.com/AccelByte/accelbyte-unity-sdk/commits/bd3c2ce0d55401eaf4c967f796f5d2c7c29a74e6))
* Improve Store Category search results to display all items from its sub-categories ([29271da](https://github.com/AccelByte/accelbyte-unity-sdk/commits/29271dae27872f380376f57ab07fe2004c5e067f))
* providing mac address & platform name methods to fulfill client requirement ([5697687](https://github.com/AccelByte/accelbyte-unity-sdk/commits/5697687894b955522c7ee3f75fa3df2d6c7dd7b5))
* support old walletapi  walkflow and multiple platform ([2cca8ef](https://github.com/AccelByte/accelbyte-unity-sdk/commits/2cca8ef02baa73711291327550c608f0d7ff126b))


### Bug Fixes

* Fixed missing Dispose call when creating and assigning UnityWebRequest ([714c8cf](https://github.com/AccelByte/accelbyte-unity-sdk/commits/714c8cfc6fca799e1a771882e9476835aba8b599))
* Fixed missing Dispose call when creating and assigning UnityWebRequest on the possible issue ([2db0dab](https://github.com/AccelByte/accelbyte-unity-sdk/commits/2db0dab8a1b35b3fcb44fa2ad85f9c1f0f15c36c))
* player is unable to logout ([4314b02](https://github.com/AccelByte/accelbyte-unity-sdk/commits/4314b02bf7f1146bd0d71dcae6e3b1f5ef890e87))
* Set OnDispose = true on uploadHandler and downloadHandler ([a3b8da8](https://github.com/AccelByte/accelbyte-unity-sdk/commits/a3b8da8bdd30752db64f510a277d2f631bd3616f))
* support get wallet info old workflow ([42d2428](https://github.com/AccelByte/accelbyte-unity-sdk/commits/42d24285eee1db6268557307787a4f44813a97d7))

## [15.1.0](https://github.com/AccelByte/accelbyte-unity-sdk/branches/compare/15.1.0%0D15.0.0) (2022-09-12)


### Features

* ApiClient API Getter to ease the transition from singleton ([76e1487](https://github.com/AccelByte/accelbyte-unity-sdk/commits/76e14874ca8073d90e3026018e6385eb921b199f))
* change access modifier from internal to public ([d9a53a3](https://github.com/AccelByte/accelbyte-unity-sdk/commits/d9a53a34db541dcfd4cae7c4e881ffdb30b4ab3c))
* IAM Phase 4 Statement & Substitute Implementation ([af2c4d8](https://github.com/AccelByte/accelbyte-unity-sdk/commits/af2c4d8b746505b82020f972f44d383de436c3d0))
* SDK Implementation for UGC service Test ([b3f816b](https://github.com/AccelByte/accelbyte-unity-sdk/commits/b3f816bf5cf41b720b705308ef0268bfc5069a3b))


### Bug Fixes

* compile error on Unity 2019 ([c1af4b6](https://github.com/AccelByte/accelbyte-unity-sdk/commits/c1af4b67a05200ad5b14652439db41b3b4d31eaa))
* use proper LoginWithUsernameV3 in User.cs ([a127a33](https://github.com/AccelByte/accelbyte-unity-sdk/commits/a127a33292abff8d17f9625861a6e50babbe73da))

## [15.0.0](https://github.com/AccelByte/accelbyte-unity-sdk/branches/compare/15.0.0%0D14.2.0) (2022-08-29)


### ⚠ BREAKING CHANGES

* there is a rename LoginSession -> UserSession

### Features

* adjust function and simplify logic ([0a3c130](https://github.com/AccelByte/accelbyte-unity-sdk/commits/0a3c13052d3b18fd7d57b0bfab02e0c21f06b5f6))
* deprecated endpoint /iam/oauth/token, added new end point /iam/v3/oauth/verify ([ffde0f5](https://github.com/AccelByte/accelbyte-unity-sdk/commits/ffde0f58b2f64e0aee0bb1aa55b2cd7b18b065e9))
* fix merging conflict ([6f7574f](https://github.com/AccelByte/accelbyte-unity-sdk/commits/6f7574fa551dc21a0fa277bf14e903aa6aedfc99))
* IAM Phase 3 Statement & Substitute Implementation ([bde86d4](https://github.com/AccelByte/accelbyte-unity-sdk/commits/bde86d470847af4449009313d939c495f157573f))
* **qos:** add GetAllServerLatencies function ([940af73](https://github.com/AccelByte/accelbyte-unity-sdk/commits/940af737d41190085cb89c7492dbce202c701281))
* SDK Implementation for UGC service - get follower count & update Follow status ([c8b5c41](https://github.com/AccelByte/accelbyte-unity-sdk/commits/c8b5c41d36832c0f945374040aa0ed69dadf8a3d))
* SDK Implementation for UGC service - Get list content ([290f5b2](https://github.com/AccelByte/accelbyte-unity-sdk/commits/290f5b2e04a7eb9b54fc0ab39a4479d8899d8357))
* SDK Implementation for UGC service - like a content ([740ebc4](https://github.com/AccelByte/accelbyte-unity-sdk/commits/740ebc441ade46f224aefacbb4ab4b15db3280ca))
* SDK Implementation for UGC service - Query by Tags ([e30c5f9](https://github.com/AccelByte/accelbyte-unity-sdk/commits/e30c5f9bf55b9f9d89c1ffaa51277747e2920ce5))


### Bug Fixes

* Cannot remember "device_token" if comes from request header. rename header device_token to device-token ([2af20b1](https://github.com/AccelByte/accelbyte-unity-sdk/commits/2af20b1a9066acfd27a817b919154e9992cd1430))
* use proper LoginWithUsernameV3 in User.cs ([5df2273](https://github.com/AccelByte/accelbyte-unity-sdk/commits/5df22734702ef30eb14138c11573690a54fa3592))


### feature

* refactor LoginSession ([76b7e3a](https://github.com/AccelByte/accelbyte-unity-sdk/commits/76b7e3a92447825783adf6bf813ade2f5f2c48b9))


### Refactors

* **lobby:** move websocket api call into a new class ([e763547](https://github.com/AccelByte/accelbyte-unity-sdk/commits/e7635479cad6853edc38947cbabff7f9dc79490d))

## [14.2.0](https://github.com/AccelByte/accelbyte-unity-sdk/branches/compare/14.2.0%0D14.1.0) (2022-08-15)


### Features

* expose store Id and list public store ([1acf4ec](https://github.com/AccelByte/accelbyte-unity-sdk/commits/1acf4ec49d0d6e121a4a99baafa078779e77c185))
* exposing v2 statistics endpoint ([2a5b935](https://github.com/AccelByte/accelbyte-unity-sdk/commits/2a5b93519c30f4bc24fb832d193a6c1b8d7a95d1))
* searching entitlement by feature ([75f2b6e](https://github.com/AccelByte/accelbyte-unity-sdk/commits/75f2b6e74f681a0f2d280fa0f0aac179385ba91d))
* **sessionbrowser:** add get game sessions by user ids ([9949260](https://github.com/AccelByte/accelbyte-unity-sdk/commits/99492609f5afcb4cd3d8e704e5b5dccc53119c9c))

## [14.1.0](https://github.com/AccelByte/accelbyte-unity-sdk/branches/compare/14.1.0%0D14.0.1) (2022-08-01)


### Features

* **basic:** added category param in generate upload url for user content ([2e3ac30](https://github.com/AccelByte/accelbyte-unity-sdk/commits/2e3ac300a24760d4c7f023e6300a53b6a4d131cb))
* **iam:** EA Origin Authentication Integration ([ee83f59](https://github.com/AccelByte/accelbyte-unity-sdk/commits/ee83f599d5bef8a84d190cb2e1b4a5960b525fea))
* Lobby function signature change to follow the rest of WrapperBase class ([394f8d3](https://github.com/AccelByte/accelbyte-unity-sdk/commits/394f8d3a430843c419314d52a69755163350f129))
* Purchasing Requirements ([9517cdc](https://github.com/AccelByte/accelbyte-unity-sdk/commits/9517cdca58ab1415fe614d7ecd10a46fd144cee1))
* unity new item type optionbox ([fa324dd](https://github.com/AccelByte/accelbyte-unity-sdk/commits/fa324ddb73e18c90db5c557de455147362b711cc))


### Bug Fixes

* **apiclient:** older version of Unity doesn't support  TryAdd function ([3d44eeb](https://github.com/AccelByte/accelbyte-unity-sdk/commits/3d44eebd2ce726e4ff6fc63711047917eec8b51d))

## [13.1.0](https://bitbucket.org/accelbyte/justice-unity-sdk/branches/compare/13.1.0%0D13.0.0) (2022-06-20)


### Features

* **iam:** Support netflix platform ID for login through Unity SDK ([80f8ac8](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/80f8ac826c8a0a9446bce7510de6ceaeead3e69f))
* **lobby:** implement add friend by public id ([671f1f7](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/671f1f7cecc5c4871d729a2a0e392aa5a6e03be3))
* **lobby:** implement set party size limit ([7d0884e](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/7d0884e90937f4748046288223b8d9cd7e5ee38f))
* **seasonpass:** Granted Season Pass EXP Tracking ([c9b7e89](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/c9b7e89275126e30b52ee9490067fd5334a74a8d))
* **seasonpass:** make new params (source & tags) as optional. ([bd53296](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/bd53296032eef6e9d4f07da82a5273450af6ad3b))

## [13.0.0](https://bitbucket.org/accelbyte/justice-unity-sdk/branches/compare/13.0.0%0D12.0.1) (2022-06-06)


### ⚠ BREAKING CHANGES

* **basic:** no email field in UserProfile server response

### Features

* **basic:** Implement Public Code/Friend Code in SDK ([dfe6a47](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/dfe6a4725a37cfe8f85c9a400433872b57b5c5d4))
* **iam:**  Include platformUserIds field when bulk gets users' basic info ([60f99e6](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/60f99e6b2173829b1cde0887fe95bcda2808c95e))
* **iam:** account linking handling ([81cb8c2](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/81cb8c2fae96ee609aa4ff9f3fd6c9b066899555))
* **iam:** Adjusting Search User to the updated Public Search User V3 API, Adding Params Limit and Offset ([41ee254](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/41ee2547e1242949ebf46b20a4d5868bd12bb0bf))


### Bug Fixes

* **test:** add delay after create game mode for lobby to refresh game mode cache from matchmaking ([f1d6b31](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/f1d6b31dad0367d2f45d8927cdcca34a5aef307d))


### Tests

* **matchmaking:** fix cancel matchmaking using wrong value for game mode parameter ([9a0b13e](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/9a0b13ec84486dff2e851f7aaf5aa74aeca0a1ac))

### [12.0.1](https://bitbucket.org/accelbyte/justice-unity-sdk/branches/compare/12.0.1%0D12.0.0) (2022-05-23)


### ⚠ BREAKING CHANGES

* **config:** this changes delete ClientId and ClientSecret variable from Config model and move it to a new model

### Features

* **AccelbytePlugin:** change the platform list from RuntimePlatform to PlatformType ([0d1bbec](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/0d1bbec6304307c8e614f7ee39b65dfdb109e4e0))
* **config:** separate oauth config from config ([c44df5d](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/c44df5d856892f4c07ebdbbe4aace8f703270ed3))

## [12.0.0](https://bitbucket.org/accelbyte/justice-unity-sdk/branches/compare/12.0.0%0D11.0.0) (2022-05-23)


### ⚠ BREAKING CHANGES

* Band User End Point Call is moved to ServerUserAccountApi
* **iam:** Band User End Point Call is moved to ServerUserAccountApi
* **multiReg:** This refactors a great number of signatures.

### Features

* **iam:** Expose the Admin Ban User endpoint ([e6c4404](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/e6c44048e28cb813917e88aff6cb790954d13dfe))
* **iam:** Expose the Admin Ban User endpoint ([cd55afd](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/cd55afd1eb624b395c8e7dbed18e3bb3bc5c8713))
* **multiReg:** Implement multiReg (multiPlugins) ([65271a7](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/65271a7be3a25cb6faf82e5cca101d4593346a12))
* **Plugins:** add multiple environment credential switch ([fb68bb7](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/fb68bb778040a0984d2700ee54ba0afb8c79cbf3))


* Merge branch 'feature/OS-6354-Expose-Admin-Ban-User-endpoint' of bitbucket.org:accelbyte/justice-unity-sdk into feature/OS-6354-Expose-Admin-Ban-User-endpoint ([7052f5f](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/7052f5fcbba353eeabf5275f68f2c026439d6ea1))

## [11.0.0](https://bitbucket.org/accelbyte/justice-unity-sdk/branches/compare/11.0.0%0D10.0.0) (2022-05-09)


### ⚠ BREAKING CHANGES

* **ecommerce:** To get total  balance could not be retrieved from credit wallet response (call via admin end point call) anymore because it’s be a current wallet balance. User should call again get use wallet balance public interface to get total balance.
* some unused argument are removed
* some unused argument are removed

### Features

* **ecommerce:** Update Unit Test Code for New Behavior Cross Platform Wallet ([39141bb](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/39141bbdcfe981d3b5b4cc39c8ee08b6175e494c))
* **jsonconverter:** add json serializer settings for enum string, array object and dictionary ([8473923](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/84739237f120a12cd3148545765a4f14a744f315))
* **lobby:** add custom attribute in register server & DSNotif ([6948396](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/6948396a4e9929e5f99b63f3bcbc7d191ca452dc))
* **lobby:** add reconnect on connection close code 4000 ([2757d29](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/2757d29f84fca04bb2af31e06ce4882107612404))


### Bug Fixes

* clean up UTF8 Json resolver remnants ([5963290](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/5963290dc2894fe4454ff09af461b8afa7cc9e6b))
* **converter:** add attribute decoration to be able to convert enums into the string values ([8e81602](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/8e816023343b5ecb9e0e1d2b9b6eb9e7bb9ede05))
* **editor:** Fix save settings to the config file ([53599d4](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/53599d492e38d64b2e11430436e03d2b989a5187))
* **lobby:** fixed empty session when reconnecting ([ba5562c](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/ba5562c9f829a6fc3fca2fc176c43fdddaa78428))


* Due to limited permission of Game Record needs to be adjust some unit tests to make it passes. ([3ce9612](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/3ce9612f17b34599e5a035057896f1c91f80039f))
* Due to limited permission of Game Record needs to be adjust some unit tests to make it passes. ([111d07d](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/111d07d60af7353472093d2c305d2077219ac6e5))


### Tests

* **Agreement:** Make sure test setup fetch correct base policies that have expected contry code ([d30c47a](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/d30c47a3dd67578771cad6c7487d9e8fda2ff4cf))

## [10.0.0](https://bitbucket.org/accelbyte/justice-unity-sdk/branches/compare/10.0.0%0D9.2.0) (2022-04-25)


### ⚠ BREAKING CHANGES

* renamed enum for arg couldsave method, reorder cloudsave method arg used to call cloudsave end point

### Features

* metadata change field name ([9f1b9f0](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/9f1b9f0aedc304e2309e2ef05be8d6fb80b375ad))


### Bug Fixes

* Fixed missing Dispose call when using UnityWebRequest ([1050a3b](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/1050a3b42dbc6df629499caff56a3d85280f10b6))


### Tests

* change hardcoded namespace to using namespace from config ([8082d8d](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/8082d8d3e4b384a9a16c5872316f64ffcd16cfe0))
* **subscription:** add stage env public key ([b47c9e0](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/b47c9e05c3e0d88604eda1aa0ec7a294c202bce3))


* Merge branch 'master' into feature/OS-6138-Change-Metadata-RecordRequest-Field ([2d24e54](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/2d24e5402397f61c24b2edcad0430b4136fccd99))

## [9.2.0](https://bitbucket.org/accelbyte/justice-unity-sdk/branches/compare/9.2.0%0D9.1.0) (2022-04-11)


### Features

* **2FA:** Handle in-game login when 2FA enabled ([3d523a7](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/3d523a73aa3e3009e185b6e8e49cf64170df9987))
* **2FA:** Implement IAM Input validation to our SDK ([c20bfee](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/c20bfee3bc42e9b2d93ff22a0083bab798cc65a0))
* Bring back old login flow with old callback delegate. ([67ce7e9](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/67ce7e935f7e23a0151ff09998854c4c8070d3e9))
* cookie in 2FA in game login ([ad0f088](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/ad0f088ed72452939ae881490b3ec511a22c3135))
* **lobby:** combine signaling message and notif model ([b8babbb](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/b8babbb3f0d5744cfb748d26b133255ebca04a91))

## [9.1.0](https://bitbucket.org/accelbyte/justice-unity-sdk/branches/compare/9.1.0%0D9.0.0) (2022-03-28)


### Features

* **cloudsave:** support server validation by using additional META field ([59efcee](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/59efcee5d04bafc2b245de20410eef6f8787c7a8))
* **lobby:** add send signaling message and notification handler ([1cdf367](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/1cdf367fce0075d107cb036541616d2b60ee9ace))
* unity google IAP ([9798629](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/97986295f074bd7cec3913a6ebfd5ce3298d3ddb))

## [9.0.0](https://bitbucket.org/accelbyte/justice-unity-sdk/branches/compare/9.0.0%0D8.7.0) (2022-03-14)


### ⚠ BREAKING CHANGES

* **user:** VerificationContext.UpgradeHeadlessAccount to VerificationContext.upgradeHeadlessAccount

### Features

* **user:** upgrade and verify headless account ([4a7dbe4](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/4a7dbe4b1902eb4c2354105e2bd295ffa05e002c))


### Tests

* **matchmaking:** refactor matchmaking test to create game mode in setup step. ([643ce10](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/643ce106bb1a7d9f6a51bbdd6468d3f802a33f39))

## [8.7.0](https://bitbucket.org/accelbyte/justice-unity-sdk/branches/compare/8.7.0%0D8.6.0) (2022-03-01)


### Features

* **entitlement:** Sync item entitlement with Twitch Drop item ([57fdcc8](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/57fdcc85abaa27e7b7cb1f1d4b5c4bf641ec9e23))


### Tests

* **test:** make test stable in dev IaC env ([60a381e](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/60a381e4903880fbd56b41458b19c823ff643452))


### Refactors

* **lobby:** Separate websocket management logic from lobby class. ([a0bce24](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/a0bce245b39d71ec4c9c7151747b51de92c68a15))

## [8.6.0](https://bitbucket.org/accelbyte/justice-unity-sdk/branches/compare/8.6.0%0D8.5.0) (2022-02-14)


### Features

* **entitlement:** bringback sync DLC item ([91987f7](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/91987f736676e356857a536c40bc2b97ff5530d3))
* **user:** Login with password grant V3 ([68772f8](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/68772f81a7f04c172ec43148b7035e82bf8162ea))

## [8.5.0](https://bitbucket.org/accelbyte/justice-unity-sdk/branches/compare/8.5.0%0D8.4.1) (2022-01-31)


### Features

* **lobby:** add more fields in MatchmakingNotif ([a6aafb5](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/a6aafb50e0a7359ab6d5e8f63e5322591ee1fb64))
* **lobby:** add userID in lobby invite & kick response ([a189f06](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/a189f0653e2bbc9e12fb493f843bf80bc1456a7f))

### [8.4.1](https://bitbucket.org/accelbyte/justice-unity-sdk/branches/compare/8.4.1%0D8.4.0) (2022-01-17)


### Bug Fixes

* **session:** fix session doesn't maintain access token after relogging ([392e89f](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/392e89f21d7a84ce62c3f8a065e321b825a5a0ff))
* **utf8json:** update Utf8JsonGenerated ([2d1c34f](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/2d1c34f5dce1996c95ec0816dbd2e7c820137ea0))


### Tests

* **lobby:** add proper cleanup for RefreshToken test. ([e7fa401](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/e7fa401b65f705dd184d17cd21bfa994a816a7e6))
* **lobby:** fix wrong error codes in unit test check ([dc5b45a](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/dc5b45a03438b0e3d9f7092f66cc6c399bdbc809))

## [8.4.0](https://bitbucket.org/accelbyte/justice-unity-sdk/branches/compare/8.4.0%0D8.3.0) (2022-01-04)


### Features

* **lobby:** add startmatchmaking with optional struct as parameter. ([2337aba](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/2337abaa46367b2a4a0df62ba77c01140c1667ad))


### Tests

* **matchmaking:** add timeout duration for rematchmaking_ReturnOk case. ([2d314d2](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/2d314d2c68d09def0d352226082b5e5a6885f0cb))

## [8.3.0](https://bitbucket.org/accelbyte/justice-unity-sdk/branches/compare/8.3.0%0D8.2.0) (2021-12-20)


### Features

* **ServerUser:** API to provide DS searchUserOtherPlatform ([c84b6d0](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/c84b6d0d8165a5f3324a10d977406469ed3882df))
* **SyncGoogleandApple:** sync purchased item from mobile platform ([1ce5732](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/1ce5732d9ccdbeacda70810c4564896bba6fab42))

## [8.2.0](https://bitbucket.org/accelbyte/justice-unity-sdk/branches/compare/8.2.0%0D8.1.0) (2021-12-06)


### Features

* **lobby:** add RefreshToken command, and sdk auto call RefreshToken when Token is refreshed. ([b0325d0](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/b0325d0e35543ab85d002284c6b63aa09b637a38))
* **user:** get bulk userInfo ([2958ede](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/2958ede14acd91879796e326a596eff0dce238e6))


### Bug Fixes

* **UGC:** fix adjustment for backend behavior changes ([bf536d3](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/bf536d3e96b4a097f268781575b09b88c0777b76))

## [8.1.0](https://bitbucket.org/accelbyte/justice-unity-sdk/branches/compare/8.1.0%0D8.0.0) (2021-11-22)


### Features

* **accelbyteplugin:** add new internal function to register utf8json resolver. test included ([802995d](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/802995dc6786ad88c8b5902cf87bab3f50e47d88))
* Added a function to download the UserAvatar of a given UserID UserProfiles.GetUserAvatar(userID,callback) ([b2d4a53](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/b2d4a53f89af227601ddb15163a1e0ecfc895773))
* Added a function to download the UserAvatar of a given UserID UserProfiles.GetUserAvatar(userID,callback) ([aac220f](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/aac220ff4d70e43107c6a393b830c3a22e62c7a7))
* Added a function to download the UserAvatar of a given UserID UserProfiles.GetUserAvatar(userID,callback) ([0bc96db](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/0bc96dbba6305fb04103525d9d5db1cdb1972b5f))
* Added ability to get valid Countries, TimeZones and Languages in the SDK through the Miscellaneous Class ([4666829](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/4666829a045e62a4e2209ea72e8270834b665dd5))
* Added ability to get valid Countries, TimeZones and Languages in the SDK through the Miscellaneous Class ([547323d](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/547323d209b049c5105ccfbd586a978fbef20b72))
* Added GetPublicUserProfile to a client accessible function ([6032810](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/603281033caab73bbb4e4abf3988fed02429cc96))
* Added GetPublicUserProfile to a client accessible function ([b1cb686](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/b1cb686e60b61baa15c4e42e1ac6612ef271a3b3))
* Added GetPublicUserProfile to a client accessible function ([41be517](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/41be5178b1c23b2ecc2a4ef1b39d480d6ae9b698))
* **DSM:** Add support for DSMC Multi Allocation Server Registration. ([b26ad98](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/b26ad98fe831eb4d8128cbc9e6726c7b972c41c5))
* **ecommerce:** Add Store Media Item Type ([ed3f176](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/ed3f17687d09351d5897722f04a557f97e432af5))
* **GetCurrentUserProgression:** add GetCurrentUserProgression ([2421494](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/2421494aa075e9be7cc2b1ce460bdbe2be603482))
* **ihttpclient:** add post request without body parameter and make sure that then content type is json media type ([60668f7](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/60668f7349d470c5712369bfcb13a7e34c207f57))


### Refactors

* **DSM): Revert "feat(DSM:** Add support for DSMC Multi Allocation Server Registration." ([43b00a0](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/43b00a03a727705ffe9aac2b4fafa195a8b2989b))

## [8.0.0](https://bitbucket.org/accelbyte/justice-unity-sdk/branches/compare/8.0.0%0D7.1.1) (2021-11-08)


### ⚠ BREAKING CHANGES

* **LobbyServer:** remove GetActiveParties method.

### Features

* **AccelBytePlugin:** changes required for multicredential ([8547780](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/8547780bff7e89433a5da1bcf363f3a9754392a5))
* **ban:** add change ban status, get ban list, and update error code ([9c94372](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/9c94372eb3c60a4f3011ca63c58544ab8b7fc7ca))
* **reward:** Adding GetRewardByRewardCode, GetRewardByRewardId, and QueryRewards . also setup for the integration tests. ([631f6f6](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/631f6f6f2b366e0ac6d993bb9cbb58ffdf11986f))
* **session:** add is comply ([0a13c57](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/0a13c57692658f5814c4456e0af1adcc1800eadd))
* **shadowban:** Add auto refresh token when bearer auth rejected, and try reconnect when in lobby ([543f9f9](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/543f9f955554acbb878b21a565c82cb0ed46eb6c))


### Bug Fixes

* **AccelByteHttpClient:** remove ClientSecret is empty checker ([03826d1](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/03826d1bd2b675d48f5ada235130d5c8fbbf8b60))
* **AccelBytePlugin:** remove CheckPlugin preprocessor so that every platform could manually initialize plugin if the plugin has not been initialized ([c5d7513](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/c5d751344ec929703cc4474c07080914c1aba486))
* **login:** fix null reference when login with incorrect email ([da2ad38](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/da2ad381d275e6546bc47ad6ccf2360209fdbb05))


### Refactors

* **LobbyServer:** remove GetActiveParties method. ([e891cb3](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/e891cb3582cecfd62296074e111b8dfbad9fa7ed))

### [7.1.1](https://bitbucket.org/accelbyte/justice-unity-sdk/branches/compare/7.1.1%0D7.1.0) (2021-10-27)


### Refactors

* **DSM:** Removed apify dependencies ([69608fa](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/69608fac4e169ef378bea9a67dfeffeed2267250))

## [7.1.0](https://bitbucket.org/accelbyte/justice-unity-sdk/branches/compare/7.1.0%0D7.0.0) (2021-09-27)


### Features

* **httperrorparser:** handle empty response body bytes ([838050b](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/838050b28ff3a960e398a2e5903da1d0c5e3715c))


### Bug Fixes

* **ecommerce:** fix ecommerce setup teardown dependencies ([d3d48bb](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/d3d48bb245d57358daf220a8b5109c302a764e0a))
* **jenkins:** slack notification + add modules.groovy ([2f57192](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/2f5719249d972caa01b06540f39a0193070c8130))
* **jenkins:** workaround for unity test stuck at clean up mono ([bfc63f9](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/bfc63f9e0a84bc302d6618fbe3b9173ba244a382))
* **lobby:** make Dictionary<string,int> and <string,string> parseable with ws parser. ([f7133bc](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/f7133bcdc354e8a6cd0e24cad437ff7fba0df7b5))
* **test:** do not shutdown unity run tests on first test failure ([6dfaf2f](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/6dfaf2fd25a15e6eeff97ace93cd46a760dbbe8d))


### Tests

* **TestHelper:** add Wait* test unit ([af1435e](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/af1435ed81c8f4fe45079e014908eda3b8ec9523))
* **TestHelper:** change Wait* methods timeout to 1 minute ([ee6204d](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/ee6204d0baeaf0bdb1eab09cea3dbbc7abaa25f0))
* **TestHelper:** remove WaitWhile & WaitEqual ([8cf0d0f](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/8cf0d0f122689cad27dadc75e0f74c1f494cbfd1))

## [7.0.0](https://bitbucket.org/accelbyte/justice-unity-sdk/branches/compare/7.0.0%0D6.0.0) (2021-09-13)


### ⚠ BREAKING CHANGES

* **User:** Change the name of CountryInfo members of UserModel into using camelCase.

### Features

* **GameClient:** add extensible client API ([e189f43](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/e189f438f3cb9a76e852d0cdd5f5c5e0b190040a))
* **reporting:** Add unity reporting SDK ([a0524f9](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/a0524f94d4eb6e19dc776ccfb32a30292fe4c8fd))
* **SeasonPass:** initial support for Season Pass service ([35be892](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/35be8921ed1857c1a14eb14ff6b06b1ae5348adf))
* **User:** Reimplement Logout api ([d171c13](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/d171c13919f24b4477616c5097f82aca08b164f9))


### Bug Fixes

* **User:** Change the name of CountryInfo members of UserModel into using camelCase. ([0107e59](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/0107e59c1a6e63fe66c98936da3713c1945562d4))


### Reverts

* **accelbyteplugin:** revert accidentally deleted http api configure and getter ([13bebc4](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/13bebc469162e13ebb1a36708ab075585a37e4bc))

## [6.0.0](https://bitbucket.org/accelbyte/justice-unity-sdk/branches/compare/6.0.0%0D5.0.0) (2021-08-30)


### ⚠ BREAKING CHANGES

* **Achievement:** Change the type of goalValue of PublicAchievement and MultiLanguageAchievement and model from int into float.

### Features

* **accelbyteplugin:** provide basic api url to miscelaneus ([a0737ed](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/a0737edca56c10b82ec2cf144a5ba31dddfdb1a2))
* **ECommerceModel:** new model for fulfilluserItem api  ([b9a7c27](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/b9a7c272517837b5c4b3c73a7061c566a2f1ef39))
* **ECommerceModels:** add itemid property ([ce716a9](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/ce716a95f43827998e07ecd785647fedaa967cc3))
* **fulfillment:** add new fulfillmentapi, add new fulfillment ([9ae1c2d](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/9ae1c2dbbe1098b169b6d2124c19b5e1a1c82aad))
* **HttpRequestBuilder:** Refactor how http api works to simplify http request creation and make it more testable ([3af039a](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/3af039a744b4a0c7ef9058a8d4d22cf4148bd27e))
* **miscellaneous:** miscellaneous api,http request and model ([d71ecd5](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/d71ecd5abcfad75d0c8dd57c9c58041c68047161))
* **model:** add var in the leaderboard model ([5a122e1](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/5a122e138c2e3f334a409399a6d6f73d314cde5f))
* **OrderWalletEntitlementTest:** add fulfilluseritem test for invalid item id and negative amount number ([23576f0](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/23576f022f3dccd4808bdc3e379506ef843eeeff))
* **ServerEcommerceApi:** new fulfill user item method ([8a76ca0](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/8a76ca0e6cbe7b1f08d0028ac35c1f88782e6b7d))
* **ServerEcommerce:** fulfill user item ([1f3c6cd](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/1f3c6cd515c0fdcc274476f7bef933a3ebda48a7))


### Bug Fixes

* **Achievement:** Fix the Achievement Api's Public Achievement and Multi Language Achievement model difference with the Backend response. Fix the Test Helper's Achievement Request and Achievement Response model difference with the Backend response. ([745b4a7](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/745b4a7e2ec70ec0be33431bb034b37055ba36a2))
* **Subscription:** Adjusting from single draft store change ([69b2a98](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/69b2a988ab0d3509dd5aebbdeca406ec4d815b05))
* **testhelper:** revert unintended config change ([de74844](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/de74844fdf54982bd0051390732a1f27c19217e5))


### Refactors

* **accelbyteplugin:** use base url instead of platform url for fulfillment api ([e0d219c](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/e0d219c73480ede82485f7874b17b5cc71b149f8))
* **fulfillmentapi:** user PlatformUrl instead of BaseUrl ([36069c0](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/36069c0ad4242752738b9bcffdc1bd25b743162b))


### Tests

* **fulfillmentTest:** add test for fulfillment api ([09fdc55](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/09fdc55daa1fc7604bc001adee6833f26ec8d5da))
* **fulfillmentTest:** complete test ([f562834](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/f56283480adae8dd346f40735d1ee92ad74283a4))
* **miscellaneous:** add miscellaneous test ([9fcbd75](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/9fcbd7557aee1a12ab2646f43a52842e630e223b))
* **OrderWalletEntitlementTest:** add server fulfill in-game item success test ([ab78e41](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/ab78e41da0a191fe4257e114c7933487b9419118))

## [5.0.0](https://bitbucket.org/accelbyte/justice-unity-sdk/branches/compare/5.0.0%0D4.1.0) (2021-08-16)


### ⚠ BREAKING CHANGES

* **Achievement:** Change the type of latestValue of UserAchievement model from int into float.

### Features

* **UGC:** add UGC into Unity SDK ([26d2ab0](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/26d2ab0088da06063cc60a5c37e60ecd2cde49f5))


### Bug Fixes

* **Achievement:** Fix the User Achievement response model ([8a2fcdb](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/8a2fcdb16c9a1fe28bb18abfda295f6b77ac25c3))
* **LoginSession:** Fix the LoginWithAuthorizationCode() using v1 endpoint ([2192b48](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/2192b48c1d6587c3737b02fb7cdacd4c9a693484))

## [4.1.0](https://bitbucket.org/accelbyte/justice-unity-sdk/branches/compare/4.1.0%0D4.0.0) (2021-08-02)


### Features

* **lobby:** add get & getAll session attribute ws command ([b3aae1f](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/b3aae1fdef6c25a10afb5f1a5c7c2cedcee3f327))
* **server lobby:** add get, getAll and set user session attribute ([b8feece](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/b8feece56da58c4a33d4d521c11409bc5d054158))


### Bug Fixes

* **utf8json:** update utf8json ([728e000](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/728e00012f100caaa63d18d39f3bd744d15ca6ee))


### Tests

* **jenkins:** add quotes to block secrets and passwords ([9f8c9bc](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/9f8c9bc47681d9da3cf804de9ff9bfd993da40a7))
* **joinable:** increase timeout because of mm delay logic changes ([14958c4](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/14958c424c3f620f96d715aba4ceeacf599d50f6))
* **lobby:** fixed some lobby tests ([8d65e6d](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/8d65e6daea7a759a9951e7223be9405e5687ecf6))
* **server lobby:** rename UnityTearDown function to CleanupLobbyConnections ([524fee3](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/524fee3b4b33b74cb439d38a66b18fd6fcc79c77))

## [4.0.0](https://bitbucket.org/accelbyte/justice-unity-sdk/branches/compare/4.0.0%0D3.0.0) (2021-06-23)


### ⚠ BREAKING CHANGES

* **Remove APIGW:** Remove APIGW support from Unity SDK

### Features

* **Remove APIGW:** Remove APIGW support from Unity SDK ([e1a8ccd](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/e1a8ccd9d35082a806d5d7cb0455060e1d4565cb))

## [3.0.0](https://bitbucket.org/accelbyte/justice-unity-sdk/branches/compare/3.0.0%0D2.32.0) (2021-06-09)


### ⚠ BREAKING CHANGES

* **User:** remove sensitive user information

### Features

* **User:** remove sensitive user information ([1f47176](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/1f47176ce5c45470765ee7cb7a8bc34bae3b729d))


### Tests

* **leaderboard:** rewrite leadearboard test to be more readable ([33547da](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/33547da673437335efff5e3e630f1d5439689865))

## [2.32.0](https://bitbucket.org/accelbyte/justice-unity-sdk/branches/compare/2.32.0%0D2.31.0) (2021-05-25)


### Features

* **dsm:** add RegisterLocalServer with public ip address & moved GetPublicIp to AccelByteNetUtilites ([539566f](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/539566ff2b910f8f2d4ee0b455cbc154013cfee4)), closes [#263](https://accelbyte.atlassian.net/browse/263)

## [2.31.0](https://bitbucket.org/accelbyte/justice-unity-sdk/branches/compare/2.31.0%0D2.30.0) (2021-04-28)


### Features

* **lobby:** add custom ports field in DSNotif ([eb5b329](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/eb5b329815326b1da50c55189c62c0be91c0cebd)), closes [#258](https://accelbyte.atlassian.net/browse/258)

## [2.30.0](https://bitbucket.org/accelbyte/justice-unity-sdk/branches/compare/2.30.0%0D2.29.0) (2021-04-14)


### Features

* **server:** remove agones ([2aec3b6](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/2aec3b6e60a1d43b2bb585a721b35974a94c99a0))

## [2.29.0](https://bitbucket.org/accelbyte/justice-unity-sdk/branches/compare/2.29.0%0D2.28.3) (2021-03-17)


### Features

* **config:** make config url optional ([3c716e0](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/3c716e062e0cd27a05d178ad7e4e262ede6894ec))
* **config:** make config url optional ([6e5bc2e](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/6e5bc2e3e909904b87dc95d3a81dd90e644d66cf))
* **config:** remove protocol remover ([47eaf86](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/47eaf86eb5f525c9c465d0f692b92e9a66874893))
* **config:** revert field place ([5caa94a](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/5caa94a2a5084ea69ca0aeba1919afee51fb11b0))
* **jsonParser:** add resolver settings to exclude null value ([7cab47a](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/7cab47acf11b48bf48af23a87798f8b223d2de4b))
* **jsonParser:** add resolver settings to exclude null value ([33293cb](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/33293cbdd3898ddde64680713a3aefbb6b49da5d))


### Bug Fixes

* **config:** fix client lobby url ([7ebaaac](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/7ebaaac9b5075f0b12945be23b4620cc54e0bb7d))
* **config:** fix config JSON ([b8ccf72](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/b8ccf7206f331b286fed338db223783ac6a6a7ca))
* **config:** fix statistic url ([4184744](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/4184744aa5dc920da30ffd774c1fe179f9ae356f))
* **config:** incorrect url ([396e3c4](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/396e3c4d1290f6409dd24290bef3fa7d089e9475))
* **user:** fix UnlinkOtherPlatform got 406 ([b0e451e](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/b0e451ecd8090289c1602173f9191f9b67f7875c))
* **user:** fix UnlinkOtherPlatform got 406 ([97bfd42](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/97bfd4261bf44bb3d1d0452cba96b04631479fd8))

### [2.28.3](https://bitbucket.org/accelbyte/justice-unity-sdk/branches/compare/2.28.3%0D2.28.2) (2021-03-03)


### Bug Fixes

* **dsm:** refactor parse function ([68a1add](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/68a1addaa19d0c73497f7877d2dc89710dd3c1b4))
* **dsm:** replace Provider enum with string ([848fdde](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/848fdde60164e5379cf95f6ac4a84a8ba922ee11))
* **dsm:** replace Provider enum with string ([602a2e7](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/602a2e78764b12a966f3c2c42f459ef593694a8c))
* **dsm:** set variable from parse result ([366a078](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/366a078354917ff36e048bfd44df6e1b814f418a))

### [2.28.2](https://bitbucket.org/accelbyte/justice-unity-sdk/branches/compare/2.28.2%0D2.28.1) (2021-02-17)

### [2.28.1](https://bitbucket.org/accelbyte/justice-unity-sdk/branches/compare/2.28.1%0D2.28.0) (2021-02-03)


### Bug Fixes

* **lobby:** increase timeout and rename function ([44a21e3](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/44a21e3c875366affed3499c46a43830a97e30de))
* **plugin:** fix error AccelByteSettings.Save() called in game build. ([c08f8a9](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/c08f8a9591cd506ea957ffe783a83c5229d5d554)), closes [#240](https://accelbyte.atlassian.net/browse/240)

## [2.28.0](https://bitbucket.org/accelbyte/justice-unity-sdk/branches/compare/2.28.0%0D2.27.0) (2021-01-20)


### Features

* **Automation Metric:** SDET-1300 Integrate SDK Test Result to Automation Metrics ([731e761](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/731e7617cdeb34d968f3c0808a240a1bae476c48)), closes [#235](https://accelbyte.atlassian.net/browse/235)
* **lobby:** add matchmaking with extra attribute parameter ([f8e3c9b](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/f8e3c9b30267f11df4388663d226c2acc537606e)), closes [#236](https://accelbyte.atlassian.net/browse/236)
* **server:** add GetPartyDataByUserId in server ([739a76d](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/739a76d53525079a06601355d1810b8dbcb102e2)), closes [#238](https://accelbyte.atlassian.net/browse/238)


### Bug Fixes

* **age:** update age to comply new requisite ([f084a73](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/f084a7366df578ac6c85603510cb25c3fa8aa025))
* **config:** check required field on init ([ad9be26](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/ad9be2607c7e80404328e1964ff90e45cae7a829))

## [2.27.0](https://bitbucket.org/accelbyte/justice-unity-sdk/branches/compare/2.27.0%0D2.26.0) (2021-01-06)


### Features

* **lobby:** add client update party storage. ([7e143a2](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/7e143a24c84610563a89e8a9a11baf222e2ff119))
* **lobby:** add client update partyStorage. ([8e1323b](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/8e1323b40dd6bb1a8edccbc66675beee21dfb05a))
* **server:** implement joinable session ([e2577b4](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/e2577b47c8e9f2a6e69d2582eef397435d6dfcd8))
* **server:** implement joinable session ([926ceb8](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/926ceb8d4da0e0ab28aec91539eef40b983b302f))


### Bug Fixes

* **config:** assign default value when ClientSecret is null ([a63843f](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/a63843fed55b7d472a2fe3e65a482c47551d2b47))
* **dsm:** register server shouldn't query QoS but take region from launch params and add provider field ([31a47b1](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/31a47b10b437acadad61f968d73dd32ff3ef814b))
* **dsm:** register server shouldn't query QoS but take region from launch params and add provider field ([bc440a1](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/bc440a16f58a979d41b83505102812bd53d7aff0))

## [2.26.0](https://bitbucket.org/accelbyte/justice-unity-sdk/branches/compare/2.26.0%0D2.25.0) (2020-12-23)


### Features

* **lobby:** add matchmaking with temporary party ([71e23f7](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/71e23f7aa54de1a5f0a0337d5bef58085bc4486a))
* **lobby:** add matchmaking with temporary party ([28e804d](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/28e804d61cb3cb5ec1311009d9ec08e2e4f78fee))
* **log:** make toggle config to enable/disable debug log in editor ([954a715](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/954a715dbd0ae882043ebe49c0269e770eac58ae))
* **userLogin:** add EpicGames enum ([852aa9c](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/852aa9c76faf00c3db2be5c4f3377c9913c2018c))

## [2.25.0](https://bitbucket.org/accelbyte/justice-unity-sdk/branches/compare/2.25.0%0D2.24.0) (2020-12-10)


### Features

* **lobby:** bulk presence count ([04320e8](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/04320e8f3de3eb7f022b3dfbac5fedc2cb60a662))
* **lobby:** support parse lobby message object value ([b974900](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/b9749005649db5784c42de57772e6f2a3fd147a9))


### Bug Fixes

* login with launcher, calls error callback when the game is not launched from launcher instead of throw assertion ([cdc8c31](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/cdc8c31f0481cdb87acacacd53603879e044f5cc))

## [2.24.0](https://bitbucket.org/accelbyte/justice-unity-sdk/branches/compare/2.24.0%0D2.23.0) (2020-11-25)


### Features

* **cloudsave:** replace record concurrently ([c5b788c](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/c5b788cb77572dd5b8456776b443512c067340a9))
* **group:** bringback group api from VE ([1084f09](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/1084f09a709afedbac303e0fe21abea113681ded))
* **profanityFilter:** add new WS command setSessionAttribute in lobby, add profanity filter test ([7fbdbf9](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/7fbdbf9a4e39ca67157d372292eea0144bf36102))


### Refactors

* **agreement:** add bulk accept legal to support ismandatory in apigateway ([c453047](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/c4530477cc33a70eb2e482e5d07c4d7e43fba3b8))
* **userId:** get user id by calling refresh data ([e9071af](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/e9071af26be6b60d8cbe2f8ce686bd1659ed3b62))

## [2.23.0](https://bitbucket.org/accelbyte/justice-unity-sdk/branches/compare/2.23.0%0D2.22.0) (2020-11-11)


### Features

* **Lobby:** bulk user presence ([57a6b2a](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/57a6b2a558b5d1363ea31ecee41a40e4aaf67b87))
* **user:** stadia login and account link ([800a558](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/800a558c1a10c9af30a122879bdef6f2fff5ec49))

## [2.22.0](https://bitbucket.org/accelbyte/justice-unity-sdk/branches/compare/2.22.0%0D2.21.0) (2020-10-28)


### Features

* **leaderboard:** get leaderboard list ([384495f](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/384495f666fa22dde2be1e8f8367b9cec42a6714))


### Bug Fixes

* **dsm:** temporary disabling dsm-related test ([99485e6](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/99485e668558e876e6e11e65fee6bc1558048faa))


### Tests

* **Daily Test:** SDET-1166 Run SDK Automation Test Daily and Send report to slack channel ([acb8033](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/acb80338e38dd0961af50f5d6240a8835d83d1ec))

## [2.21.0](https://bitbucket.org/accelbyte/justice-unity-sdk/branches/compare/2.21.0%0D2.20.0) (2020-10-14)


### Features

* **lobby:** block and unblock player functions ([1be94dc](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/1be94dc8d1b4e278f64d38a1fb4c8a6ac5cfd3cb))
* **lobby:** reject party invitation ([c4c9bae](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/c4c9bae3ec9eb09f9283ad6b60aaec9b2df0f313))

## [2.20.0](https://bitbucket.org/accelbyte/justice-unity-sdk/branches/compare/2.20.0%0D2.19.0) (2020-10-01)


### Features

* add cloudsave API to Server SDK ([739b2c9](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/739b2c97d6d2e88c82a4d9588a2d94b935a599e4))
* **leaderboard:** add additionalKey and additionalData for character leaderboard ([7a25033](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/7a25033b70dc74c8560c82682a488739c01e4ac2))
* **lobby:** party update notif & its REST APIs ([defdd4b](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/defdd4b81b983e39386c613ab1ddd330a5bde3cc))
* **serverLobby:** change WritePartyStorage signature to prevent overwrite ([fae5501](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/fae550184a89ff28bbde5b65a869c8be362063be))


### Bug Fixes

* **plugin:** hotifx add CheckPlugin() on GetConfig() and set the order of bool setter in the CheckPlugin ([f496158](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/f49615833140a45f324280015f1e2c1bc818663e))
* **plugin:** plugin failure on Unity Editor disable domain/scene reload ([d18bb42](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/d18bb426b4e334728ebb83ae3b03e7c1cd195d3f))


### Tests

* **sub:** fixed sub test by changing user ([59eb188](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/59eb18818c74c85db59da9f292e2c2fe431d47d0))


### Refactors

* **search:** add filter param in search users ([72bd941](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/72bd941945f8a40f73739bee37ef2d24caf9073f))

## [2.19.0](https://bitbucket.org/accelbyte/justice-unity-sdk/branches/compare/2.19.0%0D2.18.0) (2020-09-16)


### Features

* **lobby:** add global chat implementation ([e9fe2fc](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/e9fe2fc331fcb57ecf5a8946335ab9041f16df96))
* **statistic:** add additionalKey and additionalData to update statItem endpoint ([0e86053](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/0e860539fb41c165b2b95d95974bf2eadcde25c7))
* **statistic:** add reset and update statistic ([c09c301](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/c09c30132205d14c32cffeb7873dfa88284f8942))
* **subscription:** check subscription in SKD changed flow to not check sub endpoints. ([bb84530](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/bb845304b40af4be7bcf67b74d108639be7dc5bf))
* **Subscription:** update using the new flow & API endpoint ([95af021](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/95af02132b8122b4ef65957b2e3e05bb6b84ce12))
* **subscriptionTest:** integration test ([b3475b0](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/b3475b0c75feca18ff4495427ea8f1d5bc7ef344))
* **user:** register and upgrade with username param ([38d3ddd](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/38d3ddde989545e8bc1f71adb19e4009bc0b7ea7))


### Bug Fixes

* **subscription:** added flag when final result is found before the end to make it more consistent ([5b7bb0d](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/5b7bb0d7f3ad84e081b1b7457250d8a57b0f0ab2))
* **subscription:** changed API endpoint so it can check publisher namespace using game namespace token ([2c97d5c](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/2c97d5ce5f75a610e3209002af7d995904eb23f8))
* **Subscription:** added Publisher Namespace and Changed label "Store App ID" to "App Id" in accelbytePlatrofmSettingEditor ([6f83873](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/6f838734a6091052da05b2121463a8f9792fa0ac))
* **Subscription:** added publisher namespace in AccelbyteSettings ([b29392f](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/b29392f76fb11823aab9b46e4d83b0d0060124b4))
* **Subscription:** changed accessing config from AccelbyteSettings to AccelbytePlugin.Config ([0a60df3](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/0a60df3c0230cf313637f856d00abd984ee5cd7a))
* **Subscription:** changed sub test name to SubscriptionTest ([fb69c30](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/fb69c30b07af2f0b4aa732e778f57196f8a47bf7))
* **Subscription:** some endpoints need to be accessed with publisher namespace (get entitlements by appId, sku and getItems by appid) ([7677110](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/7677110e43deaf4e37216de3e1825f02ccb28217))


### Code Style Fixes

* **Subscription:** added newline before callback parameter to keep style inline with other functions. ([4c697da](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/4c697da9312a9ea50b19bea8c90ea59921523948))


### Tests

* **subcription:** changed appId to static to comply to Jenkins Test ([375153f](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/375153fadd6d2a950259d2177bf21dedca07eefb))

## [2.18.0](https://bitbucket.org/accelbyte/justice-unity-sdk/branches/compare/2.18.0%0D2.17.0) (2020-09-02)


### Features

* **Lobby:** add partyAttributes for matchmaking ([68a1603](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/68a16039a78ba947fe07f9dc70e51e51a9a09efd))
* **User:** add ForcedLinkOtherPlatform function ([08ebe3a](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/08ebe3a8f1c9af9c8219c8a56bc30e7c6910fac2))
* **User:** fix refreshed token not distributed correctly to other services ([009e1b6](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/009e1b6a341725eb1a1232c11c78ee0a53721595))

## [2.17.0](https://bitbucket.org/accelbyte/justice-unity-sdk/branches/compare/2.17.0%0D2.16.0) (2020-08-19)


### Features

* **Achievement:** bring this API back from customer Versus Evil Unity SDK repository. ([fcd5ed6](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/fcd5ed6371c0bd80452e3b58d6178e23c8ad2403))


### Bug Fixes

* **test:** additional incremental Achievement test ([2809e5a](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/2809e5a624c6e814b662d194e4c3c0182bfc3138))

## [2.16.0](https://bitbucket.org/accelbyte/justice-unity-sdk/branches/compare/2.16.0%0D2.15.3) (2020-08-05)


### Features

* **editor:** AccelBytePlugin static class need to be forcefully initialize on Unity Editor entering the play mode ([afa43bf](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/afa43bfa70b8376511b36c34b6d2973a1db3cfa1))
* **gameTelemetry:** APIs are added for both client & server ([fc03e03](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/fc03e036cd79f51a270aec7c52bf1581ca253151))


### Bug Fixes

* **cloudstorage:** Fix UpdateSlotMetadata function ([bdf465a](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/bdf465ac5b0dc714b0875648113f1c0799ed9b1a))
* **dsm:** only select healthy DSM url ([5cf6118](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/5cf6118b12cedee60510f95aa5a42b069e74f16e))
* **statistic:** Fix statistic error codes. ([035ff92](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/035ff92b60f44ad8df74934bf7110b17bd264aab))


### CI

* **jenkins:** archive test log file ([e42fbb1](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/e42fbb117495c2faa5fcf6fa99f27746afbb7302))
* **jenkins:** temporary social service env flag ([a746962](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/a746962bfc07c6a2088b6c4020cb6addcf57b7a5))


### Tests

* add leaderboard log result ([abbb461](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/abbb4614cb8d0c1d3b46d29a9a6d59185873f809))
* log the test name at beginning test run ([160246b](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/160246b3ad4fc811d8acd8f5657cdeb3ebe56adf))


### Refactors

* **Agremeent:** add missing function from bring back ([c7c5d7e](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/c7c5d7eb8fa4fd34132f1daa024fe71f44427154))

### [2.15.3](https://bitbucket.org/accelbyte/justice-unity-sdk/branches/compare/2.15.3%0D2.15.2) (2020-07-08)


### Bug Fixes

* **serverDSM:** agones heartbeat delegate return should not be null ([4026c5c](https://bitbucket.org/accelbyte/justice-unity-sdk/commits/4026c5c1718440f677f651c89ba2c300483ea76f))

## 2.15.2 (2020-07-01)
### Changed
- ServerDSM
    - ENABLE_AGONES_PLUGIN definition added
    - Required to compile Agones by AccelByte SDK

## 2.15.1 (2020-06-26)
### Changed
- Agreement
    - GetLegalPolicies
        - Add policyType and tags param
    - GetLegalPoliciesByCountry
        - Add policyType and tags param

## 2.15.0 (2020-06-25)
### Added
- CloudSave
    - Add SaveUserRecord
    - Add ReplaceUserRecord
    - Add GetUserRecord
    - Add GetPublicUserRecord
    - Add DeleteUserRecord
    - Add SaveGameRecord
    - Add ReplaceGameRecord
    - Add GetGameRecord
    - Add DeleteGameRecord
- CloudSaveModels
    - Add UserRecord
    - Add GameRecord

## 2.14.0 (2020-06-24)
### Added
- Leaderboard
    - Add GetRankings
    - Add GetUserRanking

## 2.13.0 (2020-06-23)
### Added
- Agones SDK
- Dedicated Server Manager Api
    - Agones provider support
### Fixed
- Pretty print AccelByteConfig.json

## 2.12.0 (2020-06-18)
### Added
- Agreement
    - Add GetLegalPolicies
    - Add GetLegalPoliciesByCountry
    - Add BulkAcceptPolicyVersions
    - Add AcceptPolicyVersion

## 2.11.0 (2020-06-08)
### Added
- User Account
    - Add UpgradeWithPlayerPortal 
    - Add GetCountryFromIP
- Entitlement
    - Add CreateDistributionReceiver
    - Add GetDistributionReceiver
    - Add DeleteDistributionReceiver
    - Add UpdateDistributionReceiver
- Config
    - Add ApiBaseUrl for GetCountryFromIP feature
    - Add NonApiBaseUrl for UpgradeWithPlayerPortal feature
- User Models
    - Add CountryInfo
    - Add UpgradeUserRequest 
- Ecommerce Models
    - Add DistributesAttributes
    - Add Attributes
    - Add Distribution Receiver

## 2.10.2 (2020-06-03)
### Changed
- Entitlement
    - GetEntitlements() -> GetEntitlement()
    
## 2.10.1 (2020-05-28)
### Added
- User
    - add `ps4` and xbox `live` enum for 3rd party login
- Plugin
    - add `UNITY_PS4`, `UNITY_XBOXONE` and `UNITY_SWITCH` macro to support development on 3rd platform

## 2.10.0 (2020-05-26)
### Added
- User
    - Bulk Get User Id by Third Party Platform
- Lobby
    - Add LobbyApi
    - Add Bulk Friend Request

## 2.9.0 (2020-04-14)
### Added
- UserProfile Api
    - GetCustomAttributes
    - UpdateCustomAttributes

## 2.8.0 (2020-04-09)
### Added
- Session Management
    - Add automatic refresh session coroutine
- DedicatedServerManager
    - Implement register static ds
    - implement get regional dsm url
    - Add errorRetry param on ConfigureHeartBeat
    - Will stop sending heartbeat after errorRetry number reached
- ServerQos
    - Get latencies from game server

### Changed
- Cloudstorage model dateAccessed, dateCreated, dateModified type from float to DateTime

### Fixed
- Lobby
    - Latencies format when startmatchmaking
- ServerModels
    - MatchingAlly
        - string partyId
        - string[] partyMember -> MatchParty[] matching_parties
- Jenkins pipeline
    - Fix tests

## 2.7.0 (2020-03-26)
### Added
- Session Management
    - Add automatic refresh session coroutine

## 2.6.0 (2020-03-03)
### Added
- Statistic
  - CreateUserStatItems
  - GetUserStatItems
  - IncrementUserStatItems
- Entitlement
    - Get user entitlement by its entitlement id.
    - Consume user entitlement.
- ServerEcommerce
	- Credit user wallet.
	- Grant user entitlement.
    - Get user entitlement by its entitlement id.
- Ecommerce Model:
    - Enum CreditUserWalletSource
    - ConsumeUserEntitlementRequest
    - CreditUserWalletRequest
    - GrantUserEntitlementRequest
    - StackableEntitlementInfo
- QosManager
  - Get Qos Servers
- Qos
  - Get Server Latencies

### Fixed
- Lobby
  - Stop reconnecting when lobby server disconnects client because token revoked, expired or another session for the same user connecting
- Integration Test
  - Fix user test

### Changed
- Statistic API Standardization
  - Model:
    - StatItemInfo into StatItem
    - BulkUserStatItemInc into UserStatItemIncrement
    - StatInfo into StatConfig
    - StatItemIncResult into CreateStatItemRequest
      - Data Member delete: currentValue
      - Data Member add: statCode
    - StatItemPagingSlicedResult into PagedStatItems
    - BulkStatItemInc into StatItemIncrement
    - BulkStatItemOperationResult into StatItemOperationResult
      - Data Member change: detail -> details
    - StatItemInfo
      - Add member public string[] tags
- User
  - GetUserByEmailAddress into SearchUsers
    - parameter: string emailadress -> string emailOrDisplayName
  - UnlinkOtherPlatform
    - parameter: string platformId -> PlatformType platformType
  - LinkOtherPlatform
    - parameter: string platformId -> PlatformType platformType
- Entitlement
  - Change class name from entitlements into entitlement

## 2.5.0 (2019-11-25)
### Fixed
- Platform
  - Models
    - Change sealed class to enum
    - Added enum:
      - OrderStatus
      - EntitlementSource
    - Refactor some class name and fix some class members:
      - WalletTransaction -> WalletTransactionInfo
      - PagedWalletTransactions -> WalletTransactionPagingSlicedResult
      - WalletInfo
        - string status -> ItemStatus status
      - Category -> CategoryInfo
        - childCategories -> Deleted
      - RegionData -> RegionDataItem
        - int totalNum -> Deleted
        - int totalNumPerAccount -> Deleted
        - int discountTotalNum -> Deleted
        - int discountTotalNumPerAccount -> Deleted
      - ItemCriteria
        - EntitlementAppType AppType -> Added
        - string Region -> Added
        - string Language -> Added
        - string BaseAppId -> Added
        - string[] Tags -> Added
        - ItemStatus ItemStatus -> Deleted
        - int? Page -> int? Offset
        - int? Size -> int? Limit
      - Item -> ItemInfo
        - Image thumbnailImage -> Deleted
        - string thumbnailUrl -> Added
        - bool stackable -> Added
        - string status -> ItemStatus status
        - string baseAppId -> Added
        - string name -> Added
        - string targetNamespace -> Added
        - string targetItemId -> Added
        - int maxCountPerUser -> Added
        - int maxCount -> Added
      - ItemSnapshot
        - string baseAppId -> Added
        - EntitlementType entitlementType -> Added
        - bool stackable -> Added
        - string thumbnailUrl -> Added
        - Image thumbnailImage -> Deleted
        - string targetItemId -> Added
        - string boothName -> Added
        - string clazz -> Added
        - string boothName -> Added
        - int displayOrder -> Added
        - string ext -> Added
        - string region -> Added
        - string language -> Added
        - string localExt -> Added
      - Image
        - string As -> Added
        - string caption -> Added
      - PagedItems -> ItemPagingSlicedResult
      - PagedOrderInfo -> OrderPagingSlicedResult
      - Entitlement -> EntitlementInfo
        - EntitlementSource source -> Added
      - PagedEntitlements -> EntitlementPagingSlicedResult
      - OrderHistoryInfo
        - string Namespace -> Added
      - OrderInfo
        - string paymentOrderNo -> Added
        - string paymentProvider -> Added
        - PaymentUrl -> paymentUrl -> Deleted
        - ItemSnapshot itemSnapshot -> Added
        - OrderTransaction[] transactions -> Deleted
        - string entitlementIds -> Deleted
        - string region -> Added
        - string language -> Added
        - string status -> OrderStatus status
        - DateTime chargebackTime -> Added
        - DateTime chargebackReversedTime -> Added
        - DateTime createdAt -> Added
        - DateTime updatedAt -> Added
      - OrderRequest
        - string language -> Added
    - Added class:
      - PopulatedItemInfo
### Changed
- Rearrange Folder
  - UnitySampleProject folder removed
  - All files and folders in UnitySampleProject has been moved to root folder
- Statistic API Standardization
  - Model:
    - StatItemInfo
      - Data Member change: profileId -> userId
    - BulkUserStatItemInc
      - Data Member change: profileId -> userId
- Delete Jenkinsfile function to create doxygen document
### Added
- Log
  - Add HTTP Request/Response Log
  - Add Current Function Log
- Release
  - Add Release folder for Unity SDK Package (in GitHub)

## 2.4.0 (2019-11-11)
### Changed
- Statistic API Standardization
  - API:
    - GetAllStatItems() & GetStatItemsByStatCodes() -> GetUserStatItems()
      - URL change: `GET` "/public/namespaces/{namespace}/users/{userId}/profiles/{profileId}/statitems" -> `GET` "/v1/public/namespaces/{namespace}/users/{userId}/statitems"
    - BulkAddStatItemValue()
      - URL change: `POST` "/public/namespaces/{namespace}/statitems/bulk/inc" -> `PUT` "/v1/public/namespaces/{namespace}/statitems/value/bulk"
    - BulkAddUserStatItemValue()
      - URL change: `POST` "/public/namespaces/{namespace}/users/{userId}/profiles/{profileId}/statitems/bulk/inc" -> `PUT` "/v1/public/namespaces/{namespace}/users/{userId}/statitems/value/bulk"
    - AddUserStatItemValue()
      - URL change: `POST` "/public/namespaces/{namespace}/users/{userId}/profiles/{profileId}/stats/{statCode}/statitems/inc" -> `PUT` "/v1/public/namespaces/{namespace}/users/{userId}/stats/{statCode}/statitems/value"
  - Model:
    - StatItemInfo
      - Add member public string[] tags
#Added
- Statistic
  - Add GetUserStatItemsByTags()
    - URL: `GET` "/v1/public/namespaces/{namespace}/users/{userId}/statitems"
    - Query param: "tags"
#Fixed
- Basic
  - Follow API Standardization
    - Endpoint Url:
        - UserProfilesApi.CreateUserProfile() "/basic/public/namespaces/{namespace}/users/me/profiles" -> "/basic/v1/public/namespaces/{namespace}/users/me/profiles"
        - UserProfilesApi.GetUserProfilePublicInfo() "/basic/public/namespaces/{namespace}/users/{userId}/profiles/public" -> "/basic/v1/public/namespaces/{namespace}/users/{userId}/profiles/public"
        - UserProfilesApi.UpdateUserProfile() "/basic/public/namespaces/{namespace}/users/me/profiles" -> "/basic/v1/public/namespaces/{namespace}/users/me/profiles"
        - UserProfilesApi.GetUserProfile() "/basic/public/namespaces/{namespace}/users/me/profiles" -> "/basic/v1/public/namespaces/{namespace}/users/me/profiles"
        - UserProfilesApi.GetUserProfilePublicInfosByIds "/basic/public/namespaces/{namespace}/profiles/public" -> "/basic/v1/public/namespaces/{namespace}/profiles/public"
        - UserProfilesApi.GetTimeZones "/basic/public/namespaces/{namespace}/misc/timezones" -> "/basic/v1/public/namespaces/{namespace}/misc/timezones"
  - Cleanup user profile on integration test

## 2.3.0 (2019-10-15)
### Added

- Enum members for IAM error handling

### Fixed

- Jenkins Automation
  - Adding Order param on Ecommerce Test
  - Fix set proxy on jenkins file
  - Add delay for steam to startup on jenkins file
  - Ignore some test on RetryTest because can't be done on automated machine
  - Change virtual coinCode from "SdkCoin" to "SDKC"
  - Adding "sortBy" param for GetItemByCriteria endpoint
  - Adding set proxy on Lobby WebSocket test

### Changed

- Remove intermediate conversion from UTF8 to .NET string before converting to object

### Breaking changes

- IAM API Standardization
  - UserAccount.Register():
    - Result type change: `UserData` -> `RegisterUserResponse`, incompatible fields
  - UserAccount.GetData():
    - URL change: `GET /v2/public/namespaces/{namespace}/users/{userId}` -> `GET /v3/public/namespaces/{namespace}/users/me`
  - UserAccount.Update():
    - URL change: `PATCH /v2/public/namespaces/{namespace}/users/{userId}` -> `PATCH /v3/public/namespaces/{namespace}/users/me`
  - UserAccount.Upgrade():
    - URL change: `POST /v3/public/namespaces/{namespace}/users/{userId}/headless/verify` -> `POST /v3/public/namespaces/{namespace}/users/me/headless/verify`
  - UserAccount.SendVerificationCode()
    - URL change: `POST /v3/public/namespaces/{namespace}/users/{userId}/code/request` -> `POST /v3/public/namespaces/{namespace}/users/me/code/request`
  - UserAccount.Verify()
    - URL change: `POST /v3/public/namespaces/{namespace}/users/{userId}/code/verify` -> `POST /v3/public/namespaces/{namespace}/users/me/code/verify`
  - UserAccount.SendPasswordResetCode()
    - URL change: `POST /v2/public/namespaces/{namespace}/users/forgotPassword` -> `POST /v3/public/namespaces/{namespace}/users/forgot"`
    - Request Body change: JSON structure changed incompatibly
  - UserAccount.ResetPassword()
    - URL change: `POST /v2/public/namespaces/{namespace}/users/resetPassword` -> `POST /v3/public/namespaces/{namespace}/users/reset`
  - UserAccount.LinkOtherPlatform()
    - URL change: `POST /v2/public/namespaces/{namespace}/users/{userId}/platforms/{platformId}/link` -> `POST /v3/public/namespaces/{namespace}/users/me/platforms/{platformId}`
  - UserAccount.UnlinkOtherPlatform()
    - URL change: `DELETE /v2/public/namespaces/{namespace}/users/{userId}/platforms/{platformId}/link` -> `DELETE /v3/public/namespaces/{namespace}/users/me/platforms/{platformId}`
  - UserAccount.GetPlatformLinks()
    - Result type change: `PlatformLink[]` -> `PagedPlatformLinks`, add paging and change fields naming to camelCase
  - UserAccount.GetUserByLoginId()
    - Signature change: `GetUserByLoginId(string loginId, ResultCallback<UserData> callback)` -> `GetUserByEmailAddress(string emailAdress, ResultCallback<PagedPublicUsersInfo> callback)`
    - URL change: `GET /namespaces/{namespace}/users/byLoginId` -> `GET /v3/public/namespaces/{namespace}/users`
    - Request Body change: add query param `query` with value = email address
    - Result type change: `UserData` -> `PagedPublicUsersInfo`, incompatible fields with paging
  - UserAccount.GetUserByUserId()
    - URL change: `GET /namespaces/{namespace}/users/{userId}` -> `GET /v3/public/namespaces/{namespace}/users/{userId}`

## v2.3.0 (2019-10-15)

### Added

- Enum members for IAM error handling

### Fixed

- Jenkins Automation
  - Adding Order param on Ecommerce Test
  - Fix set proxy on jenkins file
  - Add delay for steam to startup on jenkins file
  - Ignore some test on RetryTest because can't be done on automated machine
  - Change virtual coinCode from "SdkCoin" to "SDKC"
  - Adding "sortBy" param for GetItemByCriteria endpoint
  - Adding set proxy on Lobby WebSocket test

### Changed

- Remove intermediate conversion from UTF8 to .NET string before converting to object

### Breaking changes

- IAM API Standardization
  - UserAccount.Register():
    - Result type change: `UserData` -> `RegisterUserResponse`, incompatible fields
  - UserAccount.GetData():
    - URL change: `GET /v2/public/namespaces/{namespace}/users/{userId}` -> `GET /v3/public/namespaces/{namespace}/users/me`
  - UserAccount.Update():
    - URL change: `PATCH /v2/public/namespaces/{namespace}/users/{userId}` -> `PATCH /v3/public/namespaces/{namespace}/users/me`
  - UserAccount.Upgrade():
    - URL change: `POST /v3/public/namespaces/{namespace}/users/{userId}/headless/verify` -> `POST /v3/public/namespaces/{namespace}/users/me/headless/verify`
  - UserAccount.SendVerificationCode()
    - URL change: `POST /v3/public/namespaces/{namespace}/users/{userId}/code/request` -> `POST /v3/public/namespaces/{namespace}/users/me/code/request`
  - UserAccount.Verify()
    - URL change: `POST /v3/public/namespaces/{namespace}/users/{userId}/code/verify` -> `POST /v3/public/namespaces/{namespace}/users/me/code/verify`
  - UserAccount.SendPasswordResetCode()
    - URL change: `POST /v2/public/namespaces/{namespace}/users/forgotPassword` -> `POST /v3/public/namespaces/{namespace}/users/forgot"`
    - Request Body change: JSON structure changed incompatibly
  - UserAccount.ResetPassword()
    - URL change: `POST /v2/public/namespaces/{namespace}/users/resetPassword` -> `POST /v3/public/namespaces/{namespace}/users/reset`
  - UserAccount.LinkOtherPlatform()
    - URL change: `POST /v2/public/namespaces/{namespace}/users/{userId}/platforms/{platformId}/link` -> `POST /v3/public/namespaces/{namespace}/users/me/platforms/{platformId}`
  - UserAccount.UnlinkOtherPlatform()
    - URL change: `DELETE /v2/public/namespaces/{namespace}/users/{userId}/platforms/{platformId}/link` -> `DELETE /v3/public/namespaces/{namespace}/users/me/platforms/{platformId}`
  - UserAccount.GetPlatformLinks()
    - Result type change: `PlatformLink[]` -> `PagedPlatformLinks`, add paging and change fields naming to camelCase
  - UserAccount.GetUserByLoginId()
    - Signature change: `GetUserByLoginId(string loginId, ResultCallback<UserData> callback)` -> `GetUserByEmailAddress(string emailAdress, ResultCallback<PagedPublicUsersInfo> callback)`
    - URL change: `GET /namespaces/{namespace}/users/byLoginId` -> `GET /v3/public/namespaces/{namespace}/users`
    - Request Body change: add query param `query` with value = email address
    - Result type change: `UserData` -> `PagedPublicUsersInfo`, incompatible fields with paging
  - UserAccount.GetUserByUserId()
    - URL change: `GET /namespaces/{namespace}/users/{userId}` -> `GET /v3/public/namespaces/{namespace}/users/{userId}`

## v2.0.0 (2019-08-19)
### Added
- Session Management
  - Implementing API Gateway
  - Add Switchable Session Management
- Supported Game Profile Service Features
  - Create Game Profile
  - Get Game Profile
  - Update Game Profile
  - Delete Game Profile
- Supported Statistic Service Features
  - Get StatItem
  - Update StatItem from Client
- Supported Lobby Service Features
  - Ready Consent
  - DS Notif 

### Changed
- Coroutine
  - Change from Custom Coroutine to Unity Coroutine
- Updated to v2.0.0 for Justice release v2.0.0

## v1.0.0-beta.1 (2019-03-13)
### Added
- Supported IAM Service Features:
  - Login With Username and Password
  - Login With Other Platforms (Google, Facebook, and Steam)
  - Login With AccelByte Launcher
  - Login With Device ID
  - User Registration
  - User Verification (via email)
  - Upgrade device account and other platform account to proper AccelByte account with username and password
  - Update User Data
- Supported Basic Service Features
  - Create User Profile
  - Get User Profile
  - Update User Profile
  - Delete User Profile
- Supported Platform and E-Commerce Service Features:
  - Get Wallet Info
  - Create Order
  - Get Entitlements
  - Browse catalog (categories and items)
- Supported Lobby Service Features
  - Private Chat
  - Party
  - Friends
  - Matchmaking
  - Notification
