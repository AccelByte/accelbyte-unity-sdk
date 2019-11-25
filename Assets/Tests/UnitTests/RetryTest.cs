// Copyright (c) 2018 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections;
using System.Linq;
using AccelByte.Core;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.UnitTests
{
    [TestFixture]
    public class RetryTest
    {
        private int numCalled;

        [UnityTest]
        public IEnumerator SendRequest_GotError400_RaiseHttpError()
        {
            var worker = new UnityHttpWorker();

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

        [UnityTest]
        public IEnumerator SendRequest_GotError500Twice_RetryTwice()
        {
            var worker = new UnityHttpWorker();
            int serverErrorCount = 0;
            worker.ServerErrorOccured += req => serverErrorCount++;

            //Mocky is free third party mock HTTP server. If some day, it disappears, we should use mock http request
            //Response is BadGateway 502
            var request = HttpRequestBuilder.CreateGet("http://www.mocky.io/v2/5cd45103350000de307a54b7")
                .WithContentType(MediaType.TextPlain)
                .Accepts(MediaType.TextPlain)
                .GetResult();

            IHttpResponse response = null;
            worker.SetRetryParameters(5000, 1000, 2000);

            yield return worker.SendRequest(request, req => response = req);

            TestHelper.Assert.That(serverErrorCount, Is.GreaterThan(0));
            TestHelper.Assert.That(response.Code, Is.EqualTo(502));
        }

        [UnityTest, Ignore("Can't be done on automated machine")]
        public IEnumerator SendRequest_NoConnection_RaiseNetworkErrorImmediately()
        {
            var worker = new UnityHttpWorker();
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

        [UnityTest]
        public IEnumerator SendRequest_NoResponse_RequestTimedOut()
        {
            var worker = new UnityHttpWorker();
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

        [UnityTest]
        public IEnumerator SendRequests_WithValidUrls_AllCompleted()
        {
            var worker = new UnityHttpWorker();
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

        [UnityTest, Ignore("Can't be done on automated machine")]
        public IEnumerator SendRequests_WithSomeTimeout_AllCompleted()
        {
            int serverErrorCount = 0;
            int networkErrorCount = 0;
            var worker = new UnityHttpWorker();
            worker.ServerErrorOccured += req => serverErrorCount++;
            worker.NetworkErrorOccured += req => networkErrorCount++;
            var otherWorker = new UnityHttpWorker();
            otherWorker.ServerErrorOccured += req => serverErrorCount++;
            otherWorker.NetworkErrorOccured += req => networkErrorCount++;
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