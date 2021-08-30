using AccelByte.Api;
using AccelByte.Core;
using NUnit.Framework;
using Tests.IntegrationTests;

namespace Tests.UnitTests
{
    public class AccelbytePluginTest
    {
        class EmptyHttpApi : HttpApiBase { }
        
        [Test,TestLog,Order(1)]
        public void Configure_Http_Api_Success()
        {
            Assert.DoesNotThrow(() => AccelBytePlugin.ConfigureHttpApi<EmptyHttpApi>());
        }

        [Test,TestLog,Order(2)]
        public void Get_Http_Api_After_Configure_Success()
        {
            Assert.DoesNotThrow(() => 
            {
                AccelBytePlugin.GetHttpApi<EmptyHttpApi>();
            });
        }

    }
}
