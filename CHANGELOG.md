# Changelog

All notable changes to this project will be documented in this file. See [standard-version](https://github.com/conventional-changelog/standard-version) for commit guidelines.

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
