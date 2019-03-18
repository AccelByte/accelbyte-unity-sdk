# In-game Embedded Browser

You should add an embedded browser if:

1. You own a website that required to login the user.
1. You want to link user's account with another platform that need browser to login the user, example: Google Account, Twitch Account, Facebook Account, Twitter Account, etc. We give a support to let you implement it with two kind of browser, **ZenFulcrum**'s browser and free simple web browser (we already provide it in this repository too).

In our example/demo, we want to use browser to login the user from our [login website](https://justice.accelbyte.net/login/). It won't work with another website. 
If you want to use your own website, you need to create custom inter-process communication codes in your website. Here are the explanations for those browsers usage.

If you don't have a reason to use an embedded browser, you can remove it from the project. Basically, functionalities in our Justice login website already covered by our **AccelByte.Api.UserAccount** in this SDK (LoginWithEmail, LoginWithDeviceAccount, etc.).

### Zen Fulcrum's Embedded Browser

1. Download and open this repository.
1. Remove whole _SimpleWebBrowser_ directory in the _Assets/_ directory.
1. Please buy and download it from [Asset Store](https://assetstore.unity.com/packages/tools/gui/embedded-browser-55459).
1. Add the **EmbeddedBrowser.unitypackage** that has been bought from asset store.
1. Create a UI canvas in the scene and then rename it to *BrowserCanvas*.
1. Attach the **BrowserNavigator.cs** script from the *SampleScene* directory to that *BrowserCanvas*.
1. Uncomment preprocessor directive macro define (#define USE_ZF_BROWSER) in **BrowserNavigator.cs**.
1. Here's a bit explanation of the **BrowserNavigator.cs**
    * It'll automatically create a browser in the game scene.
    * It has public methods to `TurnOnBrowser()`, `TurnOffBrowser()`, and `OpenLink(string URL)`. 
    * It has a public event field, **BrowserMessageReceived**, that can be subscribed to listen an event. It'll pass an JSONNode parameter to the subscriber.
1. Your event handler/manager will communicate indirectly with the browser through the **BrowserNavigator** class.
1. To 'listen' a message that received by the browser, access the **BrowserNavigator**'s event and subscribe your method to the listener with += operator.

        this.browserNavigator.BrowserMessageReceived += OnBrowserNavigatorMessageReceived;

    `OnBrowserNavigatorMessageReceived(JSONNode args)` will be run when the browser receives a message (args) as the parameter .    

    💡 You can see this/implementation on the **EventsScripts.cs** inside the *Assets/SampleScene*. Or, simply load our **TestScene** to see how it works.

1. Snippet from `OnBrowserNavigatorMessageReceived(JSONNode args)`
    <!-- language: lang-cs -->

        public void OnBrowserNavigatorMessageReceived(JSONNode args)
        {
            var query = (JSON.Parse(args[0]));
            var q = JsonUtility.FromJson<LoginQuery>(query.ToString());

            /* 
            * When we hear a message with specific action, we'll give a respond or do something else.
            */
            if (q.action == "request-initial-data-injections")
            {
                ClientCreds clientCreds = new ClientCreds
                {
                    clientId = JusticePlugin.Config.ClientId,
                    clientAuthorizationHeader = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(JusticePlugin.Config.ClientId + ":" + JusticePlugin.Config.ClientSecret))
                };

                /*
                * We give a authorization header as response, to the browser.
                * __unity_onRespond is our custom-made function that provided and will be called in our login website.
                * Our site: https://justice.accelbyte.net/login/
                */
                browserNavigator.GetComponentInChildren<Browser>().CallFunction("__unity_onRespond", JsonUtility.ToJson(clientCreds))
                    .Then(ret => Debug.Log("Result: " + ret)).Done();
                Debug.Log(JsonUtility.ToJson(clientCreds));
            }
            /*
            * Here's another specific action.
            */
            else if (q.action == "success")
            {
                //We use the user's access token to login the user
                this.identityJustice.LoginWithWebsite(q.data, result =>
                    {
                        this.browserNavigator.TurnOffBrowser();
                        this.mainMenuCanvas.enabled = true;

                        this.userProfiles = JusticePlugin.GetUserProfiles();
                        this.userProfiles.GetUserProfile(OnUserProfileComplete);
                        this.purchases = JusticePlugin.GetPurchases();
                        this.purchases.GetWalletInfoByCurrencyCode("DOGECOIN", OnGetWalletInfo);
                    });
            }
        }

1. User can register from SDK but login from the embedded browser, and vice versa, user can register from Justice login website but login from the SDK.
1. To show and hide your browser, you can simply turn of the canvas for the browser.
    - You can use our `TurnOffBrowser()` method in **BrowserNavigator** to 'destroy' and unload the browser too, to prevent memory leakage and suppress the memory usage.
    - To turn it on again, you should use the `TurnOnBrowser()` method in **BrowserNavigator** to instantiate the browser again, because the previous browser that has been terminated, already gone from scene.
1. After the browser has been activated, you can redirect the browser to another page using `OpenLink(string URL)` method in **BrowserNavigator**.

### Simple Web Browser

1. Download and open this repository.
1. If you can't find the free embedded web browser, download it from [here](https://bitbucket.org/vitaly_chashin/simpleunitybrowser/downloads/SimpleWebBrowser-v0.1.2.unitypackage). Open your Unity project and then add the **SimpleWebBrowser-v0.1.2.unitypackage**.
1. Create a UI canvas in the scene and then rename it to *BrowserCanvas*.
1. Attach the **BrowserNavigator.cs** script from the *SampleScene* directory to that *BrowserCanvas*.
1. Add _Browser2D_ prefab from the package that has been imported. You can find it here: _Asset/SimpleWebBrowser/Prefabs/Browser2D.prefab_.
1. Here's a bit explanation of the **BrowserNavigator.cs**
    - It'll automatically find the browser that has been inserted into the game scene.
    - It has public methods to `TurnOnBrowser()`, `TurnOffBrowser()`, and `OpenLink(string URL)`.
    - It has a public event field, `OnBrowserLoadURL`, that can be subscribed to detect if the browser load a page.
1. To show and hide your browser, you can simply turn of the canvas for the browser.
1. This browser can listen a JSQuery response. You can see the implementation on the **EventScript.cs**. We only need to subscribe and then we can get the incoming queery, here's the code to subscribe:
    <!-- language: lang-cs -->

        //don't forget to import it!
        using SimpleWebBrowser;

        private WebBrowser2D browser;
        this.browser = GameObject.Find("Browser2D").GetComponent<WebBrowser2D>();
        this.browser.OnJSQuery += OnJSQuery;

    In the demo **EventsScript.cs**, we can see how to process the query on the `private void OnJSQuery(string query)` method after user login with Justice login website. Justice login website pass a query in JSON format that contains a `TokenResponse`. We can login the user with the `TokenResponse` that passed from Justice login website.
    <!-- language: lang-cs -->

        [Serializable]
        private class LoginQuery
        {
            public string action;
            public TokenResponse data;
        }

        private void OnJSQuery(string query)
        {  
            var q = JsonUtility.FromJson<LoginQuery>(query);

            Log(q.action);

            if (q.action == "request-initial-data-injections")
            {
                ClientCreds clientCreds = new ClientCreds
                {
                    clientId = JusticePlugin.Config.ClientId,
                    clientAuthorizationHeader = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(JusticePlugin.Config.ClientId + ":" + JusticePlugin.Config.ClientSecret))
                };

                this.browser.RespondToJSQuery(JsonUtility.ToJson(clientCreds));
            }
            else if (q.action == "success")
            {
                this.identityManager.LoginWithWebsite(q.data, result =>
                {
                        this.browserNavigator.TurnOffBrowser();
                            this.mainMenuCanvas.enabled = true;

                            this.userProfiles = JusticePlugin.GetUserProfiles();
                            this.userProfiles.GetUserProfile(OnUserProfileComplete);
                            this.purchases = JusticePlugin.GetPurchases();
                            this.purchases.GetWalletInfoByCurrencyCode(currencyCode, OnGetWalletInfo);
                });
            }
        }

1. After your script ready to capture the incoming query from the website, user can create account (register) and login from the Justice login website.
1. User can register from SDK but login from the embedded browser, and vice versa, user can register from Justice login website but login from the SDK.
