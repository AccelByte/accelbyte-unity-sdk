// Copyright (c) 2018 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections;
using System.Linq;
using AccelByte.Core;
using NUnit.Framework;
using Tests.IntegrationTests;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.UnitTests
{
    [TestFixture]
    public class RetryTest
    {
        private int numCalled;

        [UnityTest, TestLog]
        public IEnumerator SendRequest_GotError400_RaiseHttpError()
        {
            var worker = new AccelByteHttpClient();

            //Mocky is free third party mock HTTP server. If some day, it disappears, we should use mock http request
            //Response is BadRequest 400
            var request = HttpRequestBuilder.CreateGet("http://www.mocky.io/v2/5cd44dee350000aa2c7a54a4")
                .WithContentType(MediaType.TextPlain)
                .Accepts(MediaType.TextPlain)
                .GetResult();

            IHttpResponse response = null;

            yield return worker.SendRequest(request, req => response = req);

            Assert.That(response.Code, Is.EqualTo(400));
        }

        [UnityTest, TestLog]
        public IEnumerator SendRequest_GotError500Twice_RetryTwice()
        {
            var worker = new AccelByteHttpClient();
            int serverErrorCount = 0;
            worker.ServerErrorOccured += req => serverErrorCount++;

            //Mocky is free third party mock HTTP server. If some day, it disappears, we should use mock http request
            //Response is BadGateway 502
            var request = HttpRequestBuilder.CreateGet("http://www.mocky.io/v2/5cd45103350000de307a54b7")
                .WithContentType(MediaType.TextPlain)
                .Accepts(MediaType.TextPlain)
                .GetResult();

            IHttpResponse response = null;
            worker.SetRetryParameters(3000, 500, 5000);
            
            yield return worker.SendRequest(request, req => response = req);

            TestHelper.Assert.That(serverErrorCount, Is.EqualTo(3));
            TestHelper.Assert.That(response.Code, Is.EqualTo(502));
        }

        [UnityTest, TestLog, Ignore("Can't be done on automated machine")]
        public IEnumerator SendRequest_NoConnection_RaiseNetworkErrorImmediately()
        {
            var worker = new AccelByteHttpClient();
            int serverErrorCount = 0;
            bool isNetworkError = false;
            worker.ServerErrorOccured += req => serverErrorCount++;
            worker.NetworkErrorOccured += req => isNetworkError = true;

            var request = HttpRequestBuilder.CreateGet("http://accelbyte.example")
                .WithContentType(MediaType.TextPlain)
                .Accepts(MediaType.TextPlain)
                .GetResult();

            IHttpResponse response = null;
            worker.SetRetryParameters(2000, 2500, 1000);

            yield return worker.SendRequest(request, req => response = req);

            TestHelper.Assert.That(serverErrorCount, Is.EqualTo(0));
            TestHelper.Assert.IsTrue(isNetworkError);
        }

        [UnityTest, TestLog]
        public IEnumerator SendRequest_NoResponse_RequestTimedOut()
        {
            var worker = new AccelByteHttpClient();
            int serverErrorCount = 0;
            bool isNetworkError = false;
            worker.ServerErrorOccured += req => serverErrorCount++;
            worker.NetworkErrorOccured += req => isNetworkError = true;

            //Mocky is free third party mock HTTP server. If some day, it disappears, we should use mock http request and response
            //Sucess 200, delayed by 9s
            var request = HttpRequestBuilder.CreateGet("http://www.mocky.io/v2/5cd459f335000050407a54e6?mocky-delay=9s")
                .WithContentType(MediaType.TextPlain)
                .Accepts(MediaType.TextPlain)
                .GetResult();

            worker.SetRetryParameters(2000, 250, 500);
            
            yield return worker.SendRequest(request, null);

            TestHelper.Assert.That(serverErrorCount, Is.EqualTo(0));
            TestHelper.Assert.IsTrue(isNetworkError);
        }

        [UnityTest, TestLog]
        public IEnumerator SendRequests_WithValidUrls_AllCompleted()
        {
            var worker = new AccelByteHttpClient();
            int serverErrorCount = 0;
            int networkErrorCount = 0;
            worker.ServerErrorOccured += req => serverErrorCount++;
            worker.NetworkErrorOccured += req => networkErrorCount++;
            var runner = new CoroutineRunner();
            IHttpResponse[] responses = new IHttpResponse[15];

            for (int i = 0; i < responses.Length; i++)
            {
                int index = i;
                
                var request = HttpRequestBuilder.CreateGet(string.Format("http://www.example.com/?id={0}", i))
                    .GetResult();

                runner.Run(worker.SendRequest(request, rsp => responses[index] = rsp));
            }

            yield return new WaitUntil(() => responses.All(req => req != null));

            TestHelper.Assert.That(serverErrorCount, Is.EqualTo(0));
            TestHelper.Assert.That(networkErrorCount, Is.EqualTo(0));
            Assert.That(responses.Count(req => req != null), Is.EqualTo(15));
        }

        [UnityTest, TestLog, Ignore("Can't be done on automated machine")]
        public IEnumerator SendRequests_WithSomeTimeout_AllCompleted()
        {
            int serverErrorCount = 0;
            int networkErrorCount = 0;
            var worker = new AccelByteHttpClient();
            worker.ServerErrorOccured += req => serverErrorCount++;
            worker.NetworkErrorOccured += req => networkErrorCount++;
            var otherWorker = new AccelByteHttpClient();
            otherWorker.ServerErrorOccured += req => serverErrorCount++;
            worker.NetworkErrorOccured += req => networkErrorCount++;
            var runner = new CoroutineRunner();
            IHttpResponse[] responses = new IHttpResponse[15];

            for (int i = 0; i < 15; i++)
            {
                IHttpRequest request;
                int index = i;


                switch (i % 5)
                {
                case 0:
                    request = HttpRequestBuilder.CreateGet(string.Format("http://www.example.com/?id={0}", i)).GetResult();
                    runner.Run(worker.SendRequest(request, req => responses[index] = req));

                    break;
                case 1:
                    request = HttpRequestBuilder.CreateGet(string.Format("http://accelbyte.example/?id={0}", i)).GetResult();
                    runner.Run(worker.SendRequest(request, req => responses[index] = req));

                    break;
                case 2:
                    request = HttpRequestBuilder.CreateGet(
                        string.Format("http://www.mocky.io/v2/5c38bc153100006c00a991ed?mocky-delay=10s&id={0}", i)).GetResult();
                    runner.Run(worker.SendRequest(request, req => responses[index] = req));

                    break;
                case 3:
                    request = HttpRequestBuilder.CreateGet(
                        string.Format("http://www.mocky.io/v2/5c38bc153100006c00a991ed?mocky-delay=10s&id={0}", i)).GetResult();
                    otherWorker.SetRetryParameters(5000);
                    runner.Run(otherWorker.SendRequest(request, req => responses[index] = req));

                    break;
                case 4:
                    request = HttpRequestBuilder.CreateGet(
                        string.Format("http://www.mocky.io/v2/5c37fc0330000054001f659d?mocky-delay=15s&id={0}", i)).GetResult();
                    runner.Run(worker.SendRequest(request, req => responses[index] = req));

                    break;
                }

            }
            
            yield return new WaitForSeconds(3);
            
            Assert.That(responses.Count(req => req != null), Is.EqualTo(6));

            yield return new WaitForSeconds(5);

            Assert.That(responses.Count(req => req != null), Is.EqualTo(9));

            yield return new WaitForSeconds(5);

            Assert.That(responses.Count(req => req != null), Is.EqualTo(12));

            yield return new WaitForSeconds(5);

            Assert.That(responses.Count(req => req != null), Is.EqualTo(15));

            Assert.That(serverErrorCount, Is.EqualTo(0));
            Assert.That(networkErrorCount, Is.EqualTo(6));
        }
    }
}