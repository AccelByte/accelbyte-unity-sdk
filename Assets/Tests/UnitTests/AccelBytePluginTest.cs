using AccelByte.Api;
using AccelByte.Core;
using NUnit.Framework;
using Tests.IntegrationTests;

namespace Tests.UnitTests
{
    public class AccelbytePluginTest
    {
        class EmptyHttpApi : HttpApiBase { }

        [Test, TestLog, Order(1)]
        public void Configure_Http_Api_Success()
        {
            Assert.DoesNotThrow(() => AccelBytePlugin.ConfigureHttpApi<EmptyHttpApi>());
        }

        [Test, TestLog, Order(2)]
        public void Get_Http_Api_After_Configure_Success()
        {
            Assert.DoesNotThrow(() =>
            {
                AccelBytePlugin.GetHttpApi<EmptyHttpApi>();
            });
        }

        [Test, TestLog, Order(3)]
        public void Initialize_No_Config_Passed_As_Parameter()
        {
            var configFile = UnityEngine.Resources.Load("AccelByteSDKConfig");
            var wholeJsonText = ((UnityEngine.TextAsset)configFile).text;
            var config = wholeJsonText.ToObject<AccelByte.Models.Config>();

            config.CheckRequiredField();
            config.Expand();
            AccelBytePlugin.Initialize();

            Assert.True(config.BaseUrl == AccelBytePlugin.Config.BaseUrl);
            Assert.True(config.ClientId == AccelBytePlugin.Config.ClientId);
            Assert.True(config.ClientSecret == AccelBytePlugin.Config.ClientSecret);
            Assert.True(config.Namespace == AccelBytePlugin.Config.Namespace);
            Assert.True(config.PublisherNamespace == AccelBytePlugin.Config.PublisherNamespace);

        }

        [Test, TestLog, Order(4)]
        public void Initialize_Config_Passed_As_Parameter()
        {
            var config = new AccelByte.Models.Config
            {
                BaseUrl = "http://testurl.com",
                ClientId = "ClientIdTest",
                ClientSecret = "ClientSecretTest",
                Namespace = "NameSpaceTest",
                PublisherNamespace = "PublisherNamespaceTest",
                RedirectUri = "http://127.0.0.1"
            };

            config.CheckRequiredField();
            config.Expand();
            AccelBytePlugin.Initialize(config);

            AccelByteDebug.Log(AccelBytePlugin.Config.BaseUrl);

            Assert.True(config == AccelBytePlugin.Config);
            Assert.True(config.BaseUrl == AccelBytePlugin.Config.BaseUrl);
            Assert.True(config.ClientId == AccelBytePlugin.Config.ClientId);
            Assert.True(config.ClientSecret == AccelBytePlugin.Config.ClientSecret);
            Assert.True(config.Namespace == AccelBytePlugin.Config.Namespace);
            Assert.True(config.PublisherNamespace == AccelBytePlugin.Config.PublisherNamespace);
            Assert.True(config.RedirectUri == AccelBytePlugin.Config.RedirectUri);
        }
    }
}