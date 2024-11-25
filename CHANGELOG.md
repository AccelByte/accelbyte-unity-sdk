# Changelog

All notable changes to this project will be documented in this file. See [standard-version] for commit guidelines.

### [17.0.1] (2024-10-31)


### Bug Fixes

* fix null GetTurnServers result on webgl platform 
* fix server challenge response model 
* fix webgl ping method 


### Refactors

* bring back websocket changes 

## [17.0.0] (2024-10-21)


### ⚠ BREAKING CHANGES

* drop support sort by quantity for get all items of inventory service

### Features

* add apple iap subscription interface 
* add google iap subscription interface 
* add IAP query subscription interface 
* add message system v2 
* add optional param to include mac address 
* add turn manager get latency 
* **entitlement:** add sync steam inventory 
* exposed chat websocket endpoints to get and set user chat configuration 
* exposed get user entitlement history endpoint 
* implemented admin agreement endpoint interfaces, updated response model for QueryLegalEligibilities 
* implemented matchmakingV2 canceled lobby notification 


### Bug Fixes

* add default latency calculator when calculating turn server's latency 
* fixed issue on matchmaking session polling 
* fixed issue when unity persistent path is null 
* webgl platform improvements 


### Refactors

* drop support sort by quantity for get all items of inventory service 
* expose some private api fields to internal 
* fixed game common info persistent path name 
* refactor qosm to support call GetAllServerLatencies on AMS 
* remove deprecated param from inventory and server inventory 


### Documentations

* pushed back presence event api and get country groups deprecation to AGS 3.81 

### [16.25.4] (2024-10-07)


### Bug Fixes

* fix unathorized http request null exception 

### [16.25.3] (2024-10-01)


### Bug Fixes

* support http client clear cookies on multi-thread 

### [16.25.2] (2024-09-24)


### Bug Fixes

* fix some field types of TurnManager model 

### [16.25.1] (2024-09-19)


### Bug Fixes

* add various http response fields 
* added missing parameter to set upload binary content type 
* **BinaryCloudSave:** incorrect request presigned url callback model 
* **BinaryCloudSave:** invalid callback model 


### Refactors

* change save binary and presigned url type 

## [16.25.0] (2024-09-18)


### Features

* add change policy preferences in agreement 
* add disconnect endpoint 
* add overload function for sync apple purchase 
* added additional header to ds hub connection request 
* added option to have randomized device id on development build and editor 
* **entitlement:** add overload function for google sync 
* exposed endpoint to set and reset session timeout for ams ds 
* exposed new challenge service endpoints 
* Implemented fallback ticket polling and ws notif spoofing for matchmaking 
* update turn manager model 


### Bug Fixes

* corrected sequenceId data type, changed buffer primary sort param from SentAt to SequenceId 
* incorrect data type for GoogleReceiptResolveResult.NeedConsume 
* preserve inventory api 
* set missing oauth api shared memory 


### Refactors

* add defensive code for sync apple iap 
* refactor sdk heartbeat 
* refactor sdk logging 
* refactor telemetry flightId payload field to camel case 
* sdk refresh session improvement 

## [16.24.0] (2024-08-12)


### Features

* add AppleExtension as InternalsVisibleTo 
* add flight id and device type into telemetry data 
* add friend related error code 
* add login with apple 
* add optional service label param 
* add server misc service 
* add StatisticCodePredicate and Code field for purchasing requirement 
* add support for GooglePlayGames authentication 
* add unity switch token expired time 
* implement BulkGetUserByOtherPlatformUserIdsV4 
* set telemetry client timestamp with server time reference 


### Bug Fixes

* fixed incorrect chat and server user account endpoints 


### Refactors

* refactor qos get latencies implementation 

### [16.23.1] (2024-07-31)


### Bug Fixes

* **QoS:** unable parse DNS address to get latency 

## [16.23.0] (2024-07-01)


### Features

* implemented network maintainer 


### Bug Fixes

* add editor dedicated server macro to linux server bootstrap 
* add missing storeId param and fix false serverEcommerce endpoint 
* broken endpoint name in UserApi 

### [16.22.2] (2024-06-13)


### Bug Fixes

* skip check null value for optional parameter in Entitlement 

### [16.22.1] (2024-05-28)


### Refactors

* deprecate few nintendo switch interfaces 

## [16.22.0] (2024-05-22)


### Features

* add connect failed warning callback 
* add get dlc mapping endpoint 
* add more error codes 
* **BinaryCloudSave:** add server binary cloudsave interface 
* **Order:** discount code redemption order preview order price with discount code API 
* server inventory service and api implementation 


### Bug Fixes

* broken revoke token endpoint 
* fix websocket implementation on webgl build 

## [16.21.0] (2024-05-13)


### Features

* add dshub websocket url 
* add impacted user information 

### [16.20.2] (2024-05-08)


### Features

* **session:** expose Session Endpoints to query active session 


### Refactors

* modify native websocket lib to return any ws close code 

### [16.20.1] (2024-04-30)


### Features

* added config for custom reconnect timeout for dshub and ams 


### Bug Fixes

* added missing leading/trailing slashes to config urls 
* remove nunit assert 
* **WebSocket:** missing close code on websocket retry 

## [16.20.0] (2024-04-26)


### Features

* implemented inventory service wrapper 


### Bug Fixes

* rename editor variable into namespace id 


### Refactors

* add null check on get refresh token info 

### [16.19.2] (2024-04-24)


### Features

* exposed get all server session endpoint for server session service 

### [16.19.1] (2024-04-08)

## [16.19.0] (2024-04-05)


### Features

* add a feature to immediately sending telemetry data in the queue 
* add additional config types: Sandbox, Integration, QA 
* add get oauth config interface 
* add google play wrapper 
* add switch save cache and handle game exit API 
* implement challenge api 
* **Item:** add autoCalcEstimatedPrice param to some item endpoint 

## [16.18.0] (2024-03-24)


### Features

* add get config interfaces 
* add implementation of nintendo switch platform 
* add server config editor 
* **login:** add login queue API 


### Bug Fixes

* fixed dshub error on empty websocket message topic 


### Refactors

* add new steam handler format 


### Tests

* set internal visible to e2e test 

### [16.17.1] (2024-03-11)


### Bug Fixes

* **utility:** missmatch data on download 

## [16.17.0] (2024-03-08)


### Features

* add cloudsave endpoint 
* add default null filestream for switch platform. 
* **user:** assign uniqueDisplayName with displayName on Register 


### Bug Fixes

* fixed roslyn compiler warnings triggering on other attributes with the preview keyword 
* remove additionalKey requirement from UpdateUserStatItemsValue 


### Refactors

* add server log settings, log only active after configuration loaded 
* change config injection command 
* refactor config injection to avoid unnecessary warning 
* refactor SDK filesystem for switch support 
* update game telemetry cache save & load 

## [16.16.0] (2024-02-21)


### Features

* **account:** account profile revamp 
* **User:** deprecate upgrade account username param 


### Bug Fixes

* add preserve attributes on some models 
* missing sanitation stage 


### Refactors

* deprecate authentication platform link with username 

## [16.15.0] (2024-02-05)


### Features

* add GetCountryGroupV3 


### Bug Fixes

* prevent login error exception if an user hasn't complying legal agreement 


### Refactors

* add 429 response code description 
* change custom_attribute type based on backend 

## [16.14.0] (2024-01-18)


### Features

* add ams port in server config 
* add config injection v2 
* add new client registry interface 
* add new server registry interface 
* add override reload config option 
* added network conditioner to simulate dropped packets for websocket 
* implemented sdk adjustments to accomodate new field on userdata, new user search overloads, and getting other user platform basic info 
* **lobby:** add change user region 
* **lobby:** automatically change region on lobby connected 
* **UGC:** expose public staging content endpoint 


### Bug Fixes

* fix linux dedicated server error build 
* readd namepsace on predefined event added 


### Refactors

* redirect all interface into api registry 
* refactored null param handlers for game client services, fixed errors with incorrect uri and headers 
* update various service 

## [16.13.0] (2023-12-14)


### Features

* add byte converter utils 
* Added missing predefined events, refactored predefined event namespace 
* adjust concurrent record endpoint behavior 
* expose public bulk get content by sharecode 
* Expose UGC ModifyContent and DeleteContent by shareCode 
* **MPv2:** add promote game session leader API 
* sync epic games inventory/IAP 


### Bug Fixes

* change websocket-sharp to native-websocket 
* fix websocket send logic and add retry backoff mechanism 

### [16.12.1] (2023-11-30)


### Bug Fixes

* add missing override config in editor 
* fix presence broadcast event is never initiated 
* fix unable to login with latest token 


### Refactors

* remove flight id hypens 

## [16.12.0] (2023-11-28)


### Features

* add sdk version and game version info on multiplayer connect 
* Added api endpoint to serversession for sending ds session ready state 
* Added flag in client config to toggle using AMS QoS url 
* added new search hidden content parameters 
* implemented statsD / server metrics 


### Bug Fixes

* change websocket-sharp to native-websocket 
* fixed OnSDKStop race condition resulting in service clean up not triggering 
* move main run to first scene load before other scripts are run, api typo 


### Refactors

* refactor session cache implementation 
* store linux command line information to override config 

## [16.11.0] (2023-11-13)


### Features

* Added custom roslyn analyzer for preview attribute warning emission 
* added game standard events analytics implementations 
* added public binary cloud save APIs 
* **Cloudsave:** Expose Admin Game and Player Record endpoints 
* **ecommerce:** flexible bundle pricing 
* integrated flight id to api calls 
* Make the error description clearer for CheckUserAccountAvailability. 
* remove game standard customizable event namespace 
* **UGC:** Add new V2 endpoints for UGC Service 


### Bug Fixes

* add payment failed predefined event 
* fix client session not update after environment updated 
* fix empty device id on desktop platform 
* fix file cache webgl implementation 
* fix predefined event on monetization category 


### Refactors

* transfer previous event name to payload param, implemented generic event name 
* update game standard payload model 

## [16.10.0] (2023-10-30)


### Features

* add config injection for windows dev build and linux server platform 
* added predefined events analytics implementations 
* Expose CheckUserAvailability 
* **lobby:** add S2S friend sync API 
* Make the error description clearer for CheckUserAccountAvailability. 
* update config injector interface 


### Bug Fixes

* AccelbyteDebug config initialization and callbacks 
* change presence broadcast event namespace from publisher namespace to session namespace 
* remove unused metafile 
* **ugc:** sync the variables for UGC response 


### Refactors

* add leaderboard get rankings v3 
* lobby event send platform info on sending connect request 

### [16.9.1] (2023-10-16)


### Refactors

* fix webscheduler process keep looping after unity editor closed 

## [16.9.0] (2023-10-12)


### Features

* **DSHub:** add session member changed notification handler 
* **mpv2:** add persistent payload in SessionV2PublicConfiguration 
* oidc unlink features 


### Bug Fixes

* add webrequest duplication prevention 
* **mpv2:** add missing notification handler for OnSessionEnded and refactor warning message for unrecognized topic 


### Refactors

* add id input validation 

## [16.8.0] (2023-09-25)


### Features

* add StatusV2 for session member and DS status 
* add turn manager send metric endpoint 
* change client and server environment pointing into a same target 
* parity and add missing Oauth2Token params 

## [16.7.0] (2023-09-11)


### Features

* [JSC-1311] add game core and access model 
* [JSC-1311] add ITelemetryEvent interface and constructor 
* [JSC-1311] add play and social models 
* [JSC-1311] add predefined event and config 
* [JSC-1311] add storage and monetization 
* [JSC-1557] add presence broadcast event runtime config 
* add auto send login telemetry 
* add server-side predefined event analytics 
* Added version copy flow on engine initialization 
* apply sdk initialize and game launch event 
* **MPv2:** add API for session storage and lobby notification for storage changed 
* Refactored debug and default log classes to allow for scalable abstraction, multiple logger types running simultaneously 
* toggleable auto generate service url on saving configuration 


### Bug Fixes

* add additional info in login method 
* Added default logger to logging callbacks 
* change presence broadcast event interval unit 
* fix accelbyte plugin error 
* fix enable and disable config 
* fix presence broadcast event enabled logic 
* **Statistic:** Adjust newest flow to expose which statistic is being updated 


### Refactors

* add accelbyte telemetry event layer 
* add analytics api and wrapper 
* create new analytic event base 
* implement user login event 
* move and modify predefined event controller 
* refactor client after adding analytics wrapper and api 
* refactor game state event 
* refactor server after adding analytics wrapper and api 
* rename predefine event controller into predefine event scheduler 

### [16.6.5] (2023-09-07)


### Features

* add client analytics interval config 


### Bug Fixes

* reset sdk static object on game start 

### [16.6.4] (2023-09-06)


### Features

* add customizable event namespace to client analytics 


### Bug Fixes

* change Nunit import into UnityEngine 

### [16.6.3] (2023-09-06)


### Bug Fixes

* fix windows package and linux editor unable to find steam oAuth config 

### [16.6.2] (2023-09-04)


### Bug Fixes

* fix error on analytics dispose failure on unity close 

### [16.6.1] (2023-09-04)


### Features

* add analytic config editor 
* add client telemetry feature 
* add GetTokenWithClientCredentials oauth api 
* analytics core 
* change client and server environment pointing into a same target 


### Refactors

* refactor for testability 

## [16.6.0] (2023-08-28)


### Features

* **MPv2:** add data model for create match ticket error 
* **UGC:** add a new sort by value and devide liked content endpoint 


### Bug Fixes

* add error callback when the auth token is empty 
* itemIds param on BulkGetLocaleItems was modified to be a string joined separated with comma when used as a query param in an HTTP call. 


### Refactors

* add playstation 4 main 
* add playstation 5 main 
* add steam wrapper abstraction 
* add xbox game core main 
* refactor platform infoware 

## [16.5.0] (2023-08-14)


### Features

* [JSC-1519] add client timestamp as datamember 
* **lobby:** implement lobby API outgoing and incoming friend request with timestamp 
* **MPv1:** add region in MatchmakingNotif data model 
* **sessionV2:** add API for joining game session and party by code 


### Bug Fixes

* [none] set presence broadcast enabled after plugin is running 
* itemIds param on BulkGetLocaleItems was modified to be a string joined separated with comma when used as a query param in an HTTP call. 

### [16.4.1] (2023-08-03)


### Bug Fixes

* itemIds param on BulkGetLocaleItems was modified to be a string joined separated with comma when used as a query param in an HTTP call. 

## [16.4.0] (2023-07-31)


### Features

* Expose Oculus IAP Sync Endpoint 
* **MPv2:** add api to query active matchmaking ticket 
* **sessionV2:** add API for joining game session and party by code 


### Bug Fixes

* add missing field on user models 
* adjust typo in IAPOrderStatus enum 
* complete user service url error information 
* **UGC:** Expose the target user id so player didn't folloow themselves 

### [16.3.1] (2023-07-25)


### Bug Fixes

* use connect async to avoid blocking game thread 

## [16.3.0] (2023-07-17)


### Features

* exposed sync consumable entitlement 
* exposing missing psn sync entitlement multiple service 


### Bug Fixes

* adjust typo in IAPOrderStatus enum 


### Refactors

* set AccelByteSDKMain to public 

### [16.2.1] (2023-07-17)


### Refactors

* set AccelByteSDKMain to public 

## [16.2.0] (2023-07-03)


### Features

* [JSC-1199] add presence broadcast event UI and mechanism 
* added missing metafile for gdpr api 
* **chat:** add group chat moderator api 
* Provide methods for end users to request account deletion for own headless accounts in game 


### Bug Fixes

* [JSC-1278] remove unwanted warning while adding unity sdk through git 

## [16.1.0] (2023-06-19)


### Features

* [JSC-1125] Get device Id for Xbox 
* add new config member for P2P and assign reasonable default value 
* custom game thread signaller support 
* leaderboard v3 endpoints 
* new statistic endpoint 


### Refactors

* **AMS:** change ServerWatchdog to ServerAMS 


### Documentations

* redirect readme to doc portal 

## [16.0.0] (2023-06-05)


### ⚠ BREAKING CHANGES

* changed Attribute field in session models from Dictionary<string, string> to Dictionary<string, object>

### Bug Fixes

* preserve user API 
* **UGC:** change the content update boolean to false by default 


### Refactors

* cache generated device id 
* change attribute data type from Dictionary<string, string> to Dictionary<string, object> 

### [15.19.2] (2023-05-26)


### Features

* **DS:** add DSM heartbeat API 

### [15.19.1] (2023-05-24)


### Bug Fixes

* preserve SDK models and API 

## [15.19.0] (2023-05-22)


### Features

* Supporting Expiry Dates for Item Types 


### Bug Fixes

* [ASP-4655] add null IHttpResponse condition 

### [15.18.2] (2023-05-17)


### Bug Fixes

* add unityWebRequest dispose for better cleanup 
* exclude webgl in set platform login cache 

### [15.18.1] (2023-05-12)


### Bug Fixes

* fix webgl websocket missing implementation 

## [15.18.0] (2023-05-08)


### Features

* add custom attribute property 
* add statistic cycle endpoint along with the models 
* Section ID needs to be passed and stored when create order for items that are ‘Section Exclusive’. 


### Bug Fixes

* add config null check on setting editor 
* add multiple constructor in user and oauth api, set all api to public 
* fix config setting unable to redirect to default config 
* fix multiregistry issue 
* remove url config warning when the value is empty 


### Refactors

* add request task only at game main thread 
* remove xbox 360 from config 

## [15.17.0] (2023-04-26)


### Features

* Add itemName and currencyCode to response of RedeemCampaignCode API 
* add send verification to email 
* **cloudsave:** add get bulk user record keys and records 
* ecommerce enable item sellback store 
* **mmv2:** implement get metrics and add queueTime field in create ticket response 
* server api cloudsave get gamerecord 
* **UGC:** new get channels, fix sort by and order by 


### Refactors

* add webrequest scheduler 
* change logger abstraction 
* refactor user and oauth api structure 
* remove sdk warnings 
* revert back user api function names, remove coroutine usage within user interface 
* user api using c# async await 

### [15.16.2] (2023-04-19)


### Bug Fixes

* [ASP-4200] Android null device id 
* add static marshal linking in linux sigterm listener 
* fix config setting unable to redirect to default config 


### Refactors

* add API http client getter 
* remove client secret as requirement 

### [15.16.1] (2023-04-18)


### Features

* server api cloudsave get gamerecord 


### Bug Fixes

* sigterm library exposed to all platforms 

## [15.16.0] (2023-04-10)


### Features

* **lobby:** ErrorNotif with associated Request will trigger Respond instead 
* platform relogin 


### Bug Fixes

* [JSC-1125] fix missing device ID on PSN 
* fix base url sanitize when url is null 
* fix linux server error 
* **turnManager:** change UdpClient instantiation so it doesn't bind to a port 
* upload crypto medatada 


### Refactors

* add file cache implementation 

## [15.15.0] (2023-03-27)


### Features

* added sigterm listener to linux server 
* admin get bulk user by user id 
* **chat:** add notif handler and api (query, update, delete, stats) for system messages 
* create connection to Watchdog in SDK Linux Server main 
* expose Account Linking and Proggresion 
* mark deprecated since will give security hole issue for other player/user use GetUserProfilePublicInfo instead. function now just only have error message 


### Refactors

* add log when SDK start 

## [15.14.0] (2023-03-13)


### Features

* **DS:** add watchdog api 


### Documentations

* remove documentation of directly creating config files 


### Refactors

* add http send request with async method 
* change heartbeat with async 
* change websocket maintainer with async method 
* create async method of heartbeat, websocket, http request 
* seperate device identifier and device unique id 
* set config editor default environment to default 

## [15.13.0] (2023-02-28)


### Features

* adding global achievements endpoints 


### Bug Fixes

* fix unsupported code in lower version csharp 


### Refactors

* set app id as non mandatory config field 

## [15.12.0] (2023-02-13)


### Features

* [JSC-1032] add compatibility matrix check 
* add ugc custom attributes 
* cache config 
* get game token by publisher token 
* http cache 
* http cache 
* **lobby:** add reject match 
* LRU Memory implementation 
* new v2 group api 
* new v2 group api 
* Remove some Stadia enumeration variable, because Google is shutting down Stadia. 
* **turnmanager:** add api to get turn server credential 
* user ban error response 
* user ban error response 


### Bug Fixes

* change default current_player and current_internal_player to 0 
* fix unsupported code in lower version csharp 
* generate sdk version metadata file 
* revert default config directory to resource root 
* set server flag on save OAuth file 


### Refactors

* add changeable http sender 

### [15.11.2] (2023-02-01)


### Bug Fixes

* missing NUnit package causes compile error 

### [15.11.1] (2023-01-31)


### Bug Fixes

* **chat:** change class initialization to support c[#8] and below 
* define AccelByteNetUtilities explicitly public 

## [15.11.0] (2023-01-30)


### Features

* **chat:** add chat apis 
* **turnmanager:** add api to get turn server credential 


### Bug Fixes

* set server flag on save OAuth file 

### [15.9.1] (2023-01-18)


### Features

* add required text on config input field 
* implement new endpoint bulk get user and game records 


### Bug Fixes

* add missing param title to reporting api 
* add safe guard and refactor with debugable code of http error parser 
* fix websocket not running coroutine in their events 
* missing Unity meta files 
* unsupported C# implementation in Unity 2019 


### Refactors

* refactor accelbyte setting into non singleton structure 
* repositioned config editor order and add foldout category 

### [15.10.1] (2023-01-18)


### Bug Fixes

* missing Unity meta files 
* unsupported C# implementation in Unity 2019 

## [15.10.0] (2023-01-16)


### Features

* [JSC-916] add device ID generator 
* ability to set logger 
* get global statistic item by stat code 


### Bug Fixes

* [JSC-1020] replace websocket-sharp.dll properly 


### Refactors

* rename websockket-sharp to accelbyte.websocket.sharp 

## [15.9.0] (2023-01-03)


### Features

* Displays & sections (Rotating Shop Items) 
* **lobby:** Add away availability in lobby presence 
* new get achievement public tag endpoint 
* renamed api class for rotating item feature 
* **Session:** add ENDED and UNKNOWN DS status 


### Bug Fixes

* GetItemsByCriteria parse Error 

### [15.8.1] (2022-12-23)


### Bug Fixes

* unsupported implicit instantiation when using .NET Framework 

## [15.8.0] (2022-12-19)


### Features

* Account linking with one-time code 
* add Snapchat & Discord to PlatformType Model 
* **doxygen:** add check envar before generate docs 
* **doxygen:** generate doxygen document when compilation triggerred 


### Bug Fixes

* fix sdk setting editor error and add scrollbar 
* GetItemsByCriteria parse Error 
* handle lobby received ban and unban 
* set dummy coroutine dont destroy called by gameobject 

## [15.7.0] (2022-12-05)


### Features

* add .editorconfig 
* add heartbeat service 
* Exposing Get Item By Criteria in DS 
* PSN sync endpoint to be able to receive multiple service labels 
* sanitize base url on load 
* tag query builder class for endpoint with search function with tags. 


### Bug Fixes

* disable HeartBeat in DS 
* http request namespace defined from parameter 
* misleading readme.md 


### Refactors

* set oAuth return the result 

## [15.6.0] (2022-11-21)


### Features

* add new item type ecommerce lootbox 
* **DSM:** add public getter for server name 
* expose get store list for ds 
* **misc:** add get back calculated server time 
* **refresh token:** change refresh token playerprefs save method with binary file save 
* **serverApiClient:** cache instantiated wrapper 
* **session:** add Disconnected and terminated member status enum 


### Bug Fixes

* add empty DsHubNotificationTopic enum 
* cache server name when register local server 
* fix bearer unathorized loop stuck 
* update access token failed authentication flow 
* Update SessionV2DsInformation and SessionV2MemberStatus 


### Tests

* add severity, time, and stacktraces to logger 

## [15.5.0] (2022-11-07)


### Features

* fixed subscription test that always fail 
* OPTIONBOX on EAccelByteEntitlementClass 
* username field in the BaseUserInfo function 

## [15.4.0] (2022-10-24)


### Features

* Bulk get user season progression API 
* deprecated endpoint /iam/oauth/token, added new end point /iam/v3/oauth/verify 
* deprecated endpoint /iam/oauth/token, added new end point /iam/v3/oauth/verify 
* Expose update channel public endpoint 
* IAM Phase 3 Statement & Substitute Implementation 
* IAM Phase 4 Statement & Substitute Implementation 
* UnitTest/Sample Code SyncSteamDLC 


### Bug Fixes

* adopt the changes from lobby session refactoring 
* client sdk config type misplaced 
* comment out matchmaking code to avoid build test crashes 
* comment out protobuf messaage notification handling 
* fix wrong purchase condition model in item api BREAKING CHANGES: updated item info model 
* **lobby:** fix presence message still uri escaped on FriendsStatusChanged and ListOnlineFriends 
* missing dependency after rebase 
* missing request model for promote member to party leader 
* mistaken refactor of session model and server config 
* rename typo, confusing namespace, remove UpdateGameSession 
* session info replace session member and ds status changed notif 
* SessionV2DsInformation can has null requestedAt 
* typo in Lobby.cs of session v2 notification 
* typo in ServerMatchmakingApi.cs 


### Tests

* Refactor and improved party test coverage and quality 


### Refactors

* handle session notifications as json instead of protobuf 
* **lobby:** move websocket api call into a new class 
* MMv2 RejectBackfillProposal use backfill proposal notification directly 
* remove protobuf library and generated class 

## [15.3.0] (2022-10-10)


### Features

*  Bruteforce protection upon game login 
* adjustment for improve entitlement track log 
* banning by device id (PSN, XBOX) 
* expose Get and Update PrivateCustomAttributes 
* **HTTP:** adding SDK version to the resource and add HTTP headers metadata [sdk version, game client version, namespace] 
* **lobby:** add send party notif 
* **lobby:** support for role based matchmaking 
* **lobby:** support rejected status on role based matchmaking 
* Temporarily cache telemetry data into the local disk storage 


### Bug Fixes

* **lobby:** fix presence message still uri escaped on FriendsStatusChanged and ListOnlineFriends 
* support get wallet info old workflow 

## [15.2.0] (2022-09-27)


### Features

* AccelByteUnitySdk Plug-in's GetWalletInfoByCurrencyCode returns a partial response 
* Improve Store Category search results to display all items from its sub-categories 
* providing mac address & platform name methods to fulfill client requirement 
* support old walletapi  walkflow and multiple platform 


### Bug Fixes

* Fixed missing Dispose call when creating and assigning UnityWebRequest 
* Fixed missing Dispose call when creating and assigning UnityWebRequest on the possible issue 
* player is unable to logout 
* Set OnDispose = true on uploadHandler and downloadHandler 
* support get wallet info old workflow 

## [15.1.0] (2022-09-12)


### Features

* ApiClient API Getter to ease the transition from singleton 
* change access modifier from internal to public 
* IAM Phase 4 Statement & Substitute Implementation 
* SDK Implementation for UGC service Test 


### Bug Fixes

* compile error on Unity 2019 
* use proper LoginWithUsernameV3 in User.cs 

## [15.0.0] (2022-08-29)


### ⚠ BREAKING CHANGES

* there is a rename LoginSession -> UserSession

### Features

* adjust function and simplify logic 
* deprecated endpoint /iam/oauth/token, added new end point /iam/v3/oauth/verify 
* fix merging conflict 
* IAM Phase 3 Statement & Substitute Implementation 
* **qos:** add GetAllServerLatencies function 
* SDK Implementation for UGC service - get follower count & update Follow status 
* SDK Implementation for UGC service - Get list content 
* SDK Implementation for UGC service - like a content 
* SDK Implementation for UGC service - Query by Tags 


### Bug Fixes

* Cannot remember "device_token" if comes from request header. rename header device_token to device-token 
* use proper LoginWithUsernameV3 in User.cs 


### feature

* refactor LoginSession 


### Refactors

* **lobby:** move websocket api call into a new class 

## [14.2.0] (2022-08-15)


### Features

* expose store Id and list public store 
* exposing v2 statistics endpoint 
* searching entitlement by feature 
* **sessionbrowser:** add get game sessions by user ids 

## [14.1.0] (2022-08-01)


### Features

* **basic:** added category param in generate upload url for user content 
* **iam:** EA Origin Authentication Integration 
* Lobby function signature change to follow the rest of WrapperBase class 
* Purchasing Requirements 
* unity new item type optionbox 


### Bug Fixes

* **apiclient:** older version of Unity doesn't support  TryAdd function 

## [13.1.0] (2022-06-20)


### Features

* **iam:** Support netflix platform ID for login through Unity SDK 
* **lobby:** implement add friend by public id 
* **lobby:** implement set party size limit 
* **seasonpass:** Granted Season Pass EXP Tracking 
* **seasonpass:** make new params (source & tags) as optional. 

## [13.0.0] (2022-06-06)


### ⚠ BREAKING CHANGES

* **basic:** no email field in UserProfile server response

### Features

* **basic:** Implement Public Code/Friend Code in SDK 
* **iam:**  Include platformUserIds field when bulk gets users' basic info 
* **iam:** account linking handling 
* **iam:** Adjusting Search User to the updated Public Search User V3 API, Adding Params Limit and Offset 


### Bug Fixes

* **test:** add delay after create game mode for lobby to refresh game mode cache from matchmaking 


### Tests

* **matchmaking:** fix cancel matchmaking using wrong value for game mode parameter 

### [12.0.1] (2022-05-23)


### ⚠ BREAKING CHANGES

* **config:** this changes delete ClientId and ClientSecret variable from Config model and move it to a new model

### Features

* **AccelbytePlugin:** change the platform list from RuntimePlatform to PlatformType 
* **config:** separate oauth config from config 

## [12.0.0] (2022-05-23)


### ⚠ BREAKING CHANGES

* Band User End Point Call is moved to ServerUserAccountApi
* **iam:** Band User End Point Call is moved to ServerUserAccountApi
* **multiReg:** This refactors a great number of signatures.

### Features

* **iam:** Expose the Admin Ban User endpoint 
* **iam:** Expose the Admin Ban User endpoint 
* **multiReg:** Implement multiReg (multiPlugins) 
* **Plugins:** add multiple environment credential switch 


* Merge branch 'feature/OS-6354-Expose-Admin-Ban-User-endpoint' of bitbucket.org:accelbyte/justice-unity-sdk into feature/OS-6354-Expose-Admin-Ban-User-endpoint 

## [11.0.0] (2022-05-09)


### ⚠ BREAKING CHANGES

* **ecommerce:** To get total  balance could not be retrieved from credit wallet response (call via admin end point call) anymore because it’s be a current wallet balance. User should call again get use wallet balance public interface to get total balance.
* some unused argument are removed
* some unused argument are removed

### Features

* **ecommerce:** Update Unit Test Code for New Behavior Cross Platform Wallet 
* **jsonconverter:** add json serializer settings for enum string, array object and dictionary 
* **lobby:** add custom attribute in register server & DSNotif 
* **lobby:** add reconnect on connection close code 4000 


### Bug Fixes

* clean up UTF8 Json resolver remnants 
* **converter:** add attribute decoration to be able to convert enums into the string values 
* **editor:** Fix save settings to the config file 
* **lobby:** fixed empty session when reconnecting 


* Due to limited permission of Game Record needs to be adjust some unit tests to make it passes. 
* Due to limited permission of Game Record needs to be adjust some unit tests to make it passes. 


### Tests

* **Agreement:** Make sure test setup fetch correct base policies that have expected contry code 

## [10.0.0] (2022-04-25)


### ⚠ BREAKING CHANGES

* renamed enum for arg couldsave method, reorder cloudsave method arg used to call cloudsave end point

### Features

* metadata change field name 


### Bug Fixes

* Fixed missing Dispose call when using UnityWebRequest 


### Tests

* change hardcoded namespace to using namespace from config 
* **subscription:** add stage env public key 


* Merge branch 'master' into feature/OS-6138-Change-Metadata-RecordRequest-Field 

## [9.2.0] (2022-04-11)


### Features

* **2FA:** Handle in-game login when 2FA enabled 
* **2FA:** Implement IAM Input validation to our SDK 
* Bring back old login flow with old callback delegate. 
* cookie in 2FA in game login 
* **lobby:** combine signaling message and notif model 

## [9.1.0] (2022-03-28)


### Features

* **cloudsave:** support server validation by using additional META field 
* **lobby:** add send signaling message and notification handler 
* unity google IAP 

## [9.0.0] (2022-03-14)


### ⚠ BREAKING CHANGES

* **user:** VerificationContext.UpgradeHeadlessAccount to VerificationContext.upgradeHeadlessAccount

### Features

* **user:** upgrade and verify headless account 


### Tests

* **matchmaking:** refactor matchmaking test to create game mode in setup step. 

## [8.7.0] (2022-03-01)


### Features

* **entitlement:** Sync item entitlement with Twitch Drop item 


### Tests

* **test:** make test stable in dev IaC env 


### Refactors

* **lobby:** Separate websocket management logic from lobby class. 

## [8.6.0] (2022-02-14)


### Features

* **entitlement:** bringback sync DLC item 
* **user:** Login with password grant V3 

## [8.5.0] (2022-01-31)


### Features

* **lobby:** add more fields in MatchmakingNotif 
* **lobby:** add userID in lobby invite & kick response 

### [8.4.1] (2022-01-17)


### Bug Fixes

* **session:** fix session doesn't maintain access token after relogging 
* **utf8json:** update Utf8JsonGenerated 


### Tests

* **lobby:** add proper cleanup for RefreshToken test. 
* **lobby:** fix wrong error codes in unit test check 

## [8.4.0] (2022-01-04)


### Features

* **lobby:** add startmatchmaking with optional struct as parameter. 


### Tests

* **matchmaking:** add timeout duration for rematchmaking_ReturnOk case. 

## [8.3.0] (2021-12-20)


### Features

* **ServerUser:** API to provide DS searchUserOtherPlatform 
* **SyncGoogleandApple:** sync purchased item from mobile platform 

## [8.2.0] (2021-12-06)


### Features

* **lobby:** add RefreshToken command, and sdk auto call RefreshToken when Token is refreshed. 
* **user:** get bulk userInfo 


### Bug Fixes

* **UGC:** fix adjustment for backend behavior changes 

## [8.1.0] (2021-11-22)


### Features

* **accelbyteplugin:** add new internal function to register utf8json resolver. test included 
* Added a function to download the UserAvatar of a given UserID UserProfiles.GetUserAvatar(userID,callback) 
* Added a function to download the UserAvatar of a given UserID UserProfiles.GetUserAvatar(userID,callback) 
* Added a function to download the UserAvatar of a given UserID UserProfiles.GetUserAvatar(userID,callback) 
* Added ability to get valid Countries, TimeZones and Languages in the SDK through the Miscellaneous Class 
* Added ability to get valid Countries, TimeZones and Languages in the SDK through the Miscellaneous Class 
* Added GetPublicUserProfile to a client accessible function 
* Added GetPublicUserProfile to a client accessible function 
* Added GetPublicUserProfile to a client accessible function 
* **DSM:** Add support for DSMC Multi Allocation Server Registration. 
* **ecommerce:** Add Store Media Item Type 
* **GetCurrentUserProgression:** add GetCurrentUserProgression 
* **ihttpclient:** add post request without body parameter and make sure that then content type is json media type 


### Refactors

* **DSM): Revert "feat(DSM:** Add support for DSMC Multi Allocation Server Registration." 

## [8.0.0] (2021-11-08)


### ⚠ BREAKING CHANGES

* **LobbyServer:** remove GetActiveParties method.

### Features

* **AccelBytePlugin:** changes required for multicredential 
* **ban:** add change ban status, get ban list, and update error code 
* **reward:** Adding GetRewardByRewardCode, GetRewardByRewardId, and QueryRewards . also setup for the integration tests. 
* **session:** add is comply 
* **shadowban:** Add auto refresh token when bearer auth rejected, and try reconnect when in lobby 


### Bug Fixes

* **AccelByteHttpClient:** remove ClientSecret is empty checker 
* **AccelBytePlugin:** remove CheckPlugin preprocessor so that every platform could manually initialize plugin if the plugin has not been initialized 
* **login:** fix null reference when login with incorrect email 


### Refactors

* **LobbyServer:** remove GetActiveParties method. 

### [7.1.1] (2021-10-27)


### Refactors

* **DSM:** Removed apify dependencies 

## [7.1.0] (2021-09-27)


### Features

* **httperrorparser:** handle empty response body bytes 


### Bug Fixes

* **ecommerce:** fix ecommerce setup teardown dependencies 
* **jenkins:** slack notification + add modules.groovy 
* **jenkins:** workaround for unity test stuck at clean up mono 
* **lobby:** make Dictionary<string,int> and <string,string> parseable with ws parser. 
* **test:** do not shutdown unity run tests on first test failure 


### Tests

* **TestHelper:** add Wait* test unit 
* **TestHelper:** change Wait* methods timeout to 1 minute 
* **TestHelper:** remove WaitWhile & WaitEqual 

## [7.0.0] (2021-09-13)


### ⚠ BREAKING CHANGES

* **User:** Change the name of CountryInfo members of UserModel into using camelCase.

### Features

* **GameClient:** add extensible client API 
* **reporting:** Add unity reporting SDK 
* **SeasonPass:** initial support for Season Pass service 
* **User:** Reimplement Logout api 


### Bug Fixes

* **User:** Change the name of CountryInfo members of UserModel into using camelCase. 


### Reverts

* **accelbyteplugin:** revert accidentally deleted http api configure and getter 

## [6.0.0] (2021-08-30)


### ⚠ BREAKING CHANGES

* **Achievement:** Change the type of goalValue of PublicAchievement and MultiLanguageAchievement and model from int into float.

### Features

* **accelbyteplugin:** provide basic api url to miscelaneus 
* **ECommerceModel:** new model for fulfilluserItem api  
* **ECommerceModels:** add itemid property 
* **fulfillment:** add new fulfillmentapi, add new fulfillment 
* **HttpRequestBuilder:** Refactor how http api works to simplify http request creation and make it more testable 
* **miscellaneous:** miscellaneous api,http request and model 
* **model:** add var in the leaderboard model 
* **OrderWalletEntitlementTest:** add fulfilluseritem test for invalid item id and negative amount number 
* **ServerEcommerceApi:** new fulfill user item method 
* **ServerEcommerce:** fulfill user item 


### Bug Fixes

* **Achievement:** Fix the Achievement Api's Public Achievement and Multi Language Achievement model difference with the Backend response. Fix the Test Helper's Achievement Request and Achievement Response model difference with the Backend response. 
* **Subscription:** Adjusting from single draft store change 
* **testhelper:** revert unintended config change 


### Refactors

* **accelbyteplugin:** use base url instead of platform url for fulfillment api 
* **fulfillmentapi:** user PlatformUrl instead of BaseUrl 


### Tests

* **fulfillmentTest:** add test for fulfillment api 
* **fulfillmentTest:** complete test 
* **miscellaneous:** add miscellaneous test 
* **OrderWalletEntitlementTest:** add server fulfill in-game item success test 

## [5.0.0] (2021-08-16)


### ⚠ BREAKING CHANGES

* **Achievement:** Change the type of latestValue of UserAchievement model from int into float.

### Features

* **UGC:** add UGC into Unity SDK 


### Bug Fixes

* **Achievement:** Fix the User Achievement response model 
* **LoginSession:** Fix the LoginWithAuthorizationCode() using v1 endpoint 

## [4.1.0] (2021-08-02)


### Features

* **lobby:** add get & getAll session attribute ws command 
* **server lobby:** add get, getAll and set user session attribute 


### Bug Fixes

* **utf8json:** update utf8json 


### Tests

* **jenkins:** add quotes to block secrets and passwords 
* **joinable:** increase timeout because of mm delay logic changes 
* **lobby:** fixed some lobby tests 
* **server lobby:** rename UnityTearDown function to CleanupLobbyConnections 

## [4.0.0] (2021-06-23)


### ⚠ BREAKING CHANGES

* **Remove APIGW:** Remove APIGW support from Unity SDK

### Features

* **Remove APIGW:** Remove APIGW support from Unity SDK 

## [3.0.0] (2021-06-09)


### ⚠ BREAKING CHANGES

* **User:** remove sensitive user information

### Features

* **User:** remove sensitive user information 


### Tests

* **leaderboard:** rewrite leadearboard test to be more readable 

## [2.32.0] (2021-05-25)


### Features

* **dsm:** add RegisterLocalServer with public ip address & moved GetPublicIp to AccelByteNetUtilites , closes [#263]

## [2.31.0] (2021-04-28)


### Features

* **lobby:** add custom ports field in DSNotif , closes [#258]

## [2.30.0] (2021-04-14)


### Features

* **server:** remove agones 

## [2.29.0] (2021-03-17)


### Features

* **config:** make config url optional 
* **config:** make config url optional 
* **config:** remove protocol remover 
* **config:** revert field place 
* **jsonParser:** add resolver settings to exclude null value 
* **jsonParser:** add resolver settings to exclude null value 


### Bug Fixes

* **config:** fix client lobby url 
* **config:** fix config JSON 
* **config:** fix statistic url 
* **config:** incorrect url 
* **user:** fix UnlinkOtherPlatform got 406 
* **user:** fix UnlinkOtherPlatform got 406 

### [2.28.3] (2021-03-03)


### Bug Fixes

* **dsm:** refactor parse function 
* **dsm:** replace Provider enum with string 
* **dsm:** replace Provider enum with string 
* **dsm:** set variable from parse result 

### [2.28.2] (2021-02-17)

### [2.28.1] (2021-02-03)


### Bug Fixes

* **lobby:** increase timeout and rename function 
* **plugin:** fix error AccelByteSettings.Save() called in game build. , closes [#240]

## [2.28.0] (2021-01-20)


### Features

* **Automation Metric:** SDET-1300 Integrate SDK Test Result to Automation Metrics , closes [#235]
* **lobby:** add matchmaking with extra attribute parameter , closes [#236]
* **server:** add GetPartyDataByUserId in server , closes [#238]


### Bug Fixes

* **age:** update age to comply new requisite 
* **config:** check required field on init 

## [2.27.0] (2021-01-06)


### Features

* **lobby:** add client update party storage. 
* **lobby:** add client update partyStorage. 
* **server:** implement joinable session 
* **server:** implement joinable session 


### Bug Fixes

* **config:** assign default value when ClientSecret is null 
* **dsm:** register server shouldn't query QoS but take region from launch params and add provider field 
* **dsm:** register server shouldn't query QoS but take region from launch params and add provider field 

## [2.26.0] (2020-12-23)


### Features

* **lobby:** add matchmaking with temporary party 
* **lobby:** add matchmaking with temporary party 
* **log:** make toggle config to enable/disable debug log in editor 
* **userLogin:** add EpicGames enum 

## [2.25.0] (2020-12-10)


### Features

* **lobby:** bulk presence count 
* **lobby:** support parse lobby message object value 


### Bug Fixes

* login with launcher, calls error callback when the game is not launched from launcher instead of throw assertion 

## [2.24.0] (2020-11-25)


### Features

* **cloudsave:** replace record concurrently 
* **group:** bringback group api from VE 
* **profanityFilter:** add new WS command setSessionAttribute in lobby, add profanity filter test 


### Refactors

* **agreement:** add bulk accept legal to support ismandatory in apigateway 
* **userId:** get user id by calling refresh data 

## [2.23.0] (2020-11-11)


### Features

* **Lobby:** bulk user presence 
* **user:** stadia login and account link 

## [2.22.0] (2020-10-28)


### Features

* **leaderboard:** get leaderboard list 


### Bug Fixes

* **dsm:** temporary disabling dsm-related test 


### Tests

* **Daily Test:** SDET-1166 Run SDK Automation Test Daily and Send report to slack channel 

## [2.21.0] (2020-10-14)


### Features

* **lobby:** block and unblock player functions 
* **lobby:** reject party invitation 

## [2.20.0] (2020-10-01)


### Features

* add cloudsave API to Server SDK 
* **leaderboard:** add additionalKey and additionalData for character leaderboard 
* **lobby:** party update notif & its REST APIs 
* **serverLobby:** change WritePartyStorage signature to prevent overwrite 


### Bug Fixes

* **plugin:** hotifx add CheckPlugin() on GetConfig() and set the order of bool setter in the CheckPlugin 
* **plugin:** plugin failure on Unity Editor disable domain/scene reload 


### Tests

* **sub:** fixed sub test by changing user 


### Refactors

* **search:** add filter param in search users 

## [2.19.0] (2020-09-16)


### Features

* **lobby:** add global chat implementation 
* **statistic:** add additionalKey and additionalData to update statItem endpoint 
* **statistic:** add reset and update statistic 
* **subscription:** check subscription in SKD changed flow to not check sub endpoints. 
* **Subscription:** update using the new flow & API endpoint 
* **subscriptionTest:** integration test 
* **user:** register and upgrade with username param 


### Bug Fixes

* **subscription:** added flag when final result is found before the end to make it more consistent 
* **subscription:** changed API endpoint so it can check publisher namespace using game namespace token 
* **Subscription:** added Publisher Namespace and Changed label "Store App ID" to "App Id" in accelbytePlatrofmSettingEditor 
* **Subscription:** added publisher namespace in AccelbyteSettings 
* **Subscription:** changed accessing config from AccelbyteSettings to AccelbytePlugin.Config 
* **Subscription:** changed sub test name to SubscriptionTest 
* **Subscription:** some endpoints need to be accessed with publisher namespace (get entitlements by appId, sku and getItems by appid) 


### Code Style Fixes

* **Subscription:** added newline before callback parameter to keep style inline with other functions. 


### Tests

* **subcription:** changed appId to static to comply to Jenkins Test 

## [2.18.0] (2020-09-02)


### Features

* **Lobby:** add partyAttributes for matchmaking 
* **User:** add ForcedLinkOtherPlatform function 
* **User:** fix refreshed token not distributed correctly to other services 

## [2.17.0] (2020-08-19)


### Features

* **Achievement:** bring this API back from customer Versus Evil Unity SDK repository. 


### Bug Fixes

* **test:** additional incremental Achievement test 

## [2.16.0] (2020-08-05)


### Features

* **editor:** AccelBytePlugin static class need to be forcefully initialize on Unity Editor entering the play mode 
* **gameTelemetry:** APIs are added for both client & server 


### Bug Fixes

* **cloudstorage:** Fix UpdateSlotMetadata function 
* **dsm:** only select healthy DSM url 
* **statistic:** Fix statistic error codes. 


### CI

* **jenkins:** archive test log file 
* **jenkins:** temporary social service env flag 


### Tests

* add leaderboard log result 
* log the test name at beginning test run 


### Refactors

* **Agremeent:** add missing function from bring back 

### [2.15.3] (2020-07-08)


### Bug Fixes

* **serverDSM:** agones heartbeat delegate return should not be null 

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
