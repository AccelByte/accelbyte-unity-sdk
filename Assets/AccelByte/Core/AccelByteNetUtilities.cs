using System.Collections;
using AccelByte.Models;

namespace AccelByte.Core
{
    static class AccelByteNetUtilities
    {
        private static readonly CoroutineRunner coroutineRunner = new CoroutineRunner();
        private static readonly IHttpWorker httpWorker = new UnityHttpWorker();

        public static void GetPublicIp(ResultCallback<PublicIp> callback)
        {
            coroutineRunner.Run(GetPublicIpAsync(callback));
        }

        private static IEnumerator GetPublicIpAsync(ResultCallback<PublicIp> callback)
        {
            var getPubIpRequest = HttpRequestBuilder.CreateGet("https://api.ipify.org?format=json")
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return httpWorker.SendRequest(getPubIpRequest, rsp => response = rsp);

            var result = response.TryParseJson<PublicIp>();
            callback.Try(result);
        }
    }
}
