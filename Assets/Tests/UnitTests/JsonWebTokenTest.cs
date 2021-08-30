using System;
using System.Collections.Generic;
using AccelByte.Core;
using NUnit.Framework;

namespace Tests.UnitTests
{
    [TestFixture]
    public class JsonWebTokenTest
    {
        string publicKey = @"-----BEGIN PUBLIC KEY-----
                                MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAnzyis1ZjfNB0bBgKFMSv
                                vkTtwlvBsaJq7S5wA+kzeVOVpVWwkWdVha4s38XM/pa/yr47av7+z3VTmvDRyAHc
                                aT92whREFpLv9cj5lTeJSibyr/Mrm/YtjCZVWgaOYIhwrXwKLqPr/11inWsAkfIy
                                tvHWTxZYEcXLgAXFuUuaS3uF9gEiNQwzGTU1v0FqkqTBr4B8nW3HCN47XUu0t8Y0
                                e+lf4s4OxQawWD79J9/5d3Ry0vbV3Am1FtGJiJvOwRsIfVChDpYStTcHTCMqtvWb
                                V6L11BWkpzGXSW4Hv43qa+GSYOD2QU68Mb59oSk2OB+BtOLpJofmbGEGgvmwyCI9
                                MwIDAQAB
                                -----END PUBLIC KEY-----";

        string invalidPublicKey = @"-----BEGIN PUBLIC KEY-----
                                MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAnzyis1ZjfNB0bBgKFMSv
                                vkTtwlvBsaJq7S5wA+kzeVOVpVWwkWdVha4s38XM/pa/yr47av7+z3VTmvDRyAHc
                                aT92whREFpLv9cj5lTeJSibyr/Mrm/YtjCZVWgaOYIhwrXwKLqPr/11inWsAkfIy
                                tvHWTxZYEcXLgAXFuUuaS3uF9gEiNQwzGTU1v0FqkqTBr4B8nW3HCN47XUu0t8Y0
                                e+lf4s4OxQawWD79J9/5d3Ry0vbV3Am1FtGJiJvOwRsIfVChDpYStTcHTCMqtvWb
                                V6L11BWkpzGXSW4Hv43qa+GSYOD2QU68Mb59aSk2OB+BtOLpJofmbGEGgvmwyCI9
                                MwIDAQAB
                                -----END PUBLIC KEY-----";

        [Test]
        public void DecodeJsonWebToken_ValidPublicKey_GetStringPayload()
        {
            // Arrange
            string token = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyLCJleHAiOjIwMTYyMzkwMjJ9.AET2_srfSOa2f9HajuRyVKW1xh6R0UwwZI1rqh4_jlkikIQPeY1FkGJ2x_RQXHpmw64051ndOTiHKF9-RTOWsvPxpDa5NUphSgd2ODZA28Y964aoaLI5jH97MOGloIhLsS979ckFN6ACS3GLyUY5kZU_jGfzH5D3PCMTJuW5vEMZvPFFY27XwSkGYYQJdagyDvXgBaj6sftzoSvf4JVkBeetALZ7Afbc8Y2_J43GpXXjClYK4pl2rLPmKqfnMzizhlSgHuZvvs_dYiVxMdALl9pRWaX1HOhgYE4gxK6wD74KGG7OpeVnIIpim-SF9BviL-8rvKCRCN0r3VH45YOkeg";

            // ACT
            var isSuccessful = JsonWebToken.TryDecodeToken<Dictionary<string, object>>(publicKey, token, out var payload);

            // Assert
            DateTime exp = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt32(payload["exp"])).DateTime;
            DateTime iat = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt32(payload["iat"])).DateTime;
            DateTime now = DateTime.UtcNow;

            Assert.That(isSuccessful);
            Assert.IsFalse(string.IsNullOrEmpty((string)payload["sub"]));
            Assert.IsFalse(string.IsNullOrEmpty((string)payload["name"]));
            Assert.That(now <= exp);
            Assert.That((now - iat).TotalSeconds > -10);
        }

        [Test]
        public void DecodeJsonWebToken_ValidPublicKey_GetObjectPayload()
        {
            // Arrange
            string token = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwib2JqIjp7InN0cmluZ1ZhbHVlIjoiYWxwaGEiLCJhbm90aGVyU3RyaW5nVmFsdWUiOiJiZXRhIiwiaW50VmFsdWUiOjF9LCJpYXQiOjE1MTYyMzkwMjIsImV4cCI6MjAxNjIzOTAyMn0.PrqM8pQivNB8EJTBWmsGd4Gjo5-nYcThsyIR-iicNQFzwJRujUjF3Yaf0GCEp8g5fIs3wdspKMTTA_P650g0ojg5jUAumGaSdlYtFHvzr3MnCKwC4pjSU9uNQS-tpi5M41z8-vaQirEGjGKCApjhv_kJBktw8ZdpVaRcfzxhs06IJkR9hduoeL6BcVs5Ha3zhUwPs29rNF3x-9ZDzyKvmBh-ETI5dtLj8LqUN4DWVv9Kw3eSStUg5LrFhQXST1WNc5LcdSh5wIH1hQtPt7rD5Qxgz-aluBhYYgkYbpn5IiFey5SxstYhLMa9l0LGzf9BZrVV1wzdxKI74TseJoTq3w";

            // ACT
            var isSuccessful = JsonWebToken.TryDecodeToken<Dictionary<string, object>>(publicKey, token, out var payload);

            // Assert
            Dictionary<string, object> obj = JsonExtension.ToObject<Dictionary<string, object>>(payload["obj"].ToJsonString());

            DateTime exp = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt32(payload["exp"])).DateTime;
            DateTime iat = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt32(payload["iat"])).DateTime;
            DateTime now = DateTime.UtcNow;

            Assert.That(isSuccessful);
            Assert.IsFalse(string.IsNullOrEmpty((string)payload["sub"]));
            Assert.IsFalse(string.IsNullOrEmpty((string)obj["stringValue"]));
            Assert.IsFalse(string.IsNullOrEmpty((string)obj["anotherStringValue"]));
            Assert.That(Convert.ToInt32(obj["intValue"]) != 0);
            Assert.That(now <= exp);
            Assert.That((now - iat).TotalSeconds > -10);
        }

        [Test]
        public void DecodeJsonWebToken_InvalidPublicKey_GetNullPayload()
        {
            // Arrange
            string token = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwib2JqIjp7InN0cmluZ1ZhbHVlIjoiYWxwaGEiLCJhbm90aGVyU3RyaW5nVmFsdWUiOiJiZXRhIiwiaW50VmFsdWUiOjF9LCJpYXQiOjE1MTYyMzkwMjIsImV4cCI6MjAxNjIzOTAyMn0.PrqM8pQivNB8EJTBWmsGd4Gjo5-nYcThsyIR-iicNQFzwJRujUjF3Yaf0GCEp8g5fIs3wdspKMTTA_P650g0ojg5jUAumGaSdlYtFHvzr3MnCKwC4pjSU9uNQS-tpi5M41z8-vaQirEGjGKCApjhv_kJBktw8ZdpVaRcfzxhs06IJkR9hduoeL6BcVs5Ha3zhUwPs29rNF3x-9ZDzyKvmBh-ETI5dtLj8LqUN4DWVv9Kw3eSStUg5LrFhQXST1WNc5LcdSh5wIH1hQtPt7rD5Qxgz-aluBhYYgkYbpn5IiFey5SxstYhLMa9l0LGzf9BZrVV1wzdxKI74TseJoTq3w";

            // ACT
            var isSuccessful = JsonWebToken.TryDecodeToken<Dictionary<string, object>>(invalidPublicKey, token, out var payload);

            // Assert
            Assert.IsFalse(isSuccessful);
            Assert.That(payload == null);
        }

        [Test]
        public void DecodeJsonWebToken_InvalidPublicKeyWithoutPublicKeyVerification_GetObjectPayload()
        {
            // Arrange
            string token = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwib2JqIjp7InN0cmluZ1ZhbHVlIjoiYWxwaGEiLCJhbm90aGVyU3RyaW5nVmFsdWUiOiJiZXRhIiwiaW50VmFsdWUiOjF9LCJpYXQiOjE1MTYyMzkwMjIsImV4cCI6MjAxNjIzOTAyMn0.PrqM8pQivNB8EJTBWmsGd4Gjo5-nYcThsyIR-iicNQFzwJRujUjF3Yaf0GCEp8g5fIs3wdspKMTTA_P650g0ojg5jUAumGaSdlYtFHvzr3MnCKwC4pjSU9uNQS-tpi5M41z8-vaQirEGjGKCApjhv_kJBktw8ZdpVaRcfzxhs06IJkR9hduoeL6BcVs5Ha3zhUwPs29rNF3x-9ZDzyKvmBh-ETI5dtLj8LqUN4DWVv9Kw3eSStUg5LrFhQXST1WNc5LcdSh5wIH1hQtPt7rD5Qxgz-aluBhYYgkYbpn5IiFey5SxstYhLMa9l0LGzf9BZrVV1wzdxKI74TseJoTq3w";

            // ACT
            var isSuccessful = JsonWebToken.TryDecodeToken<Dictionary<string, object>>(invalidPublicKey, token, out var payload, verifyPublicKey: false);

            // Assert
            Dictionary<string, object> obj = JsonExtension.ToObject<Dictionary<string, object>>(payload["obj"].ToJsonString());

            Assert.That(isSuccessful);
            Assert.IsFalse(string.IsNullOrEmpty((string)payload["sub"]));
            Assert.IsFalse(string.IsNullOrEmpty((string)obj["stringValue"]));
            Assert.IsFalse(string.IsNullOrEmpty((string)obj["anotherStringValue"]));
            Assert.That(Convert.ToInt32(obj["intValue"]) != 0);
        }

        [Test]
        public void DecodeJsonWebToken_TokenAlreadyExpired_GetNullPayload()
        {
            // Arrange
            string token = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwib2JqIjp7InN0cmluZ1ZhbHVlIjoiYWxwaGEiLCJhbm90aGVyU3RyaW5nVmFsdWUiOiJiZXRhIiwiaW50VmFsdWUiOjF9LCJpYXQiOjE1MTYyMzkwMjIsImV4cCI6MTUxNjIzOTAyM30.Avwb5oe8KoERr39Uv29ySsaz4cTAl0qjqhEDwM9UwcV3xEjutTtBQhV8egAnG63WT5FJw3eHKchzoI7fe_yoc7KxjRlqOCffvvbDR7bo_0wB62ySN1OaCbYl3ZTf_fkkoJbJWHMZdX5e1pU7a18fAhBXsyaf2AeorT51PYbXwxcXC4gBsL_tdMk-b8weblV1oiTf8SOLnbVr45nJurqOoSgwnIQyP6ZtWKm3tlk7RZECINL0SA3XilEor5_f7-9sUjm4WOWpDbhnVYZXTG6HMrsg4LkWHsekp2G0ieX2NpJ5onLky6gUIk-WeJK8W37Vbv4NSMCVloLN4YYj6UP1kA";

            // ACT
            var isSuccessful = JsonWebToken.TryDecodeToken<Dictionary<string, object>>(publicKey, token, out var payload);

            // Assert
            Assert.IsFalse(isSuccessful);
            Assert.That(payload == null);
        }

        [Test]
        public void DecodeJsonWebToken_TokenIssuedAtFuture_GetNullPayload()
        {
            // Arrange
            string token = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwib2JqIjp7InN0cmluZ1ZhbHVlIjoiYWxwaGEiLCJhbm90aGVyU3RyaW5nVmFsdWUiOiJiZXRhIiwiaW50VmFsdWUiOjF9LCJpYXQiOjIwMTYyMzkwMjIsImV4cCI6MjAxNjIzOTAyM30.Qc1aaIGQC7TXGtSSPt4Jw5jNBt788D6I6_rs3olBVXGZpMa8Qfj6W2xPeLBKiDJREt6W_92iRJUbfgLt4FZfWZSqRnoYnlmdY7LmDNIgG29DsjM4u_GyNbaYjs9tKZum6vkLttcUXVDImjPt7xJ68zq5KoteX3bbgBxSeqGj0aNl1TLgryULsXhNLCu12XYKz70XnMrnNHGfi9YL6a5kqrT268y5BS6--cVhFIeSsnPEiUXUjlXryu6S2tNHtIjTxDEDCfmLixOMa6r-Kd4SUCkD_FgJAQTcKQIQ5RQLqqD5wvSi24l9u6BD66DWbjzfpolYUbxeKaO5ElskDUmfdA";

            // ACT
            var isSuccessful = JsonWebToken.TryDecodeToken<Dictionary<string, object>>(publicKey, token, out var payload);

            // Assert
            Assert.IsFalse(isSuccessful);
            Assert.That(payload == null);
        }

        [Test]
        public void DecodeJsonWebToken_TokenAlreadyExpiredWithoutExpiredVerification_GetObjectPayload()
        {
            // Arrange
            string token = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwib2JqIjp7InN0cmluZ1ZhbHVlIjoiYWxwaGEiLCJhbm90aGVyU3RyaW5nVmFsdWUiOiJiZXRhIiwiaW50VmFsdWUiOjF9LCJpYXQiOjE1MTYyMzkwMjIsImV4cCI6MTUxNjIzOTAyM30.Avwb5oe8KoERr39Uv29ySsaz4cTAl0qjqhEDwM9UwcV3xEjutTtBQhV8egAnG63WT5FJw3eHKchzoI7fe_yoc7KxjRlqOCffvvbDR7bo_0wB62ySN1OaCbYl3ZTf_fkkoJbJWHMZdX5e1pU7a18fAhBXsyaf2AeorT51PYbXwxcXC4gBsL_tdMk-b8weblV1oiTf8SOLnbVr45nJurqOoSgwnIQyP6ZtWKm3tlk7RZECINL0SA3XilEor5_f7-9sUjm4WOWpDbhnVYZXTG6HMrsg4LkWHsekp2G0ieX2NpJ5onLky6gUIk-WeJK8W37Vbv4NSMCVloLN4YYj6UP1kA";

            // ACT
            var isSuccessful = JsonWebToken.TryDecodeToken<Dictionary<string, object>>(publicKey, token, out var payload, verifyExpiration: false);

            // Assert
            Dictionary<string, object> obj = JsonExtension.ToObject<Dictionary<string, object>>(payload["obj"].ToJsonString());

            Assert.That(isSuccessful);
            Assert.IsFalse(string.IsNullOrEmpty((string)payload["sub"]));
            Assert.IsFalse(string.IsNullOrEmpty((string)obj["stringValue"]));
            Assert.IsFalse(string.IsNullOrEmpty((string)obj["anotherStringValue"]));
            Assert.That(Convert.ToInt32(obj["intValue"]) != 0);
        }

        [Test]
        public void DecodeJsonWebToken_TokenIssuedAtFutureWithoutExpiredVerification_GetObjectPayload()
        {
            // Arrange
            string token = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwib2JqIjp7InN0cmluZ1ZhbHVlIjoiYWxwaGEiLCJhbm90aGVyU3RyaW5nVmFsdWUiOiJiZXRhIiwiaW50VmFsdWUiOjF9LCJpYXQiOjIwMTYyMzkwMjIsImV4cCI6MjAxNjIzOTAyM30.Qc1aaIGQC7TXGtSSPt4Jw5jNBt788D6I6_rs3olBVXGZpMa8Qfj6W2xPeLBKiDJREt6W_92iRJUbfgLt4FZfWZSqRnoYnlmdY7LmDNIgG29DsjM4u_GyNbaYjs9tKZum6vkLttcUXVDImjPt7xJ68zq5KoteX3bbgBxSeqGj0aNl1TLgryULsXhNLCu12XYKz70XnMrnNHGfi9YL6a5kqrT268y5BS6--cVhFIeSsnPEiUXUjlXryu6S2tNHtIjTxDEDCfmLixOMa6r-Kd4SUCkD_FgJAQTcKQIQ5RQLqqD5wvSi24l9u6BD66DWbjzfpolYUbxeKaO5ElskDUmfdA";

            // ACT
            var isSuccessful = JsonWebToken.TryDecodeToken<Dictionary<string, object>>(publicKey, token, out var payload, verifyExpiration: false);

            // Assert
            Dictionary<string, object> obj = JsonExtension.ToObject<Dictionary<string, object>>(payload["obj"].ToJsonString());

            Assert.That(isSuccessful);
            Assert.IsFalse(string.IsNullOrEmpty((string)payload["sub"]));
            Assert.IsFalse(string.IsNullOrEmpty((string)obj["stringValue"]));
            Assert.IsFalse(string.IsNullOrEmpty((string)obj["anotherStringValue"]));
            Assert.That(Convert.ToInt32(obj["intValue"]) != 0);
        }

        [Test]
        public void DecodeExpiration_ValidToken_GetExpirationValue()
        {
            // Arrange
            string token = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoyMDE2MjM4NzIyLCJleHAiOjIwMTYyMzkwMjJ9.KT3xP7eWq2rjqR1NHuCrglbDmkATO2j-k8jg7UUXx5o40KIKkB2Nvsgdqr5J18XXRycY4cwGwYNtxuIIoGzQBd2nPdkCcVs39327WmmyxIPUFc42J1UcDZkjsOqZAKq3wX6gnzCep9oCqCgzIb5QwPWLnBlwFHaZc7J95eaiukuGuEbXVSoLaaiKUetOczas9Wc033AONobJdEY_3UwUyId-sUgsShfUZYvysPFM27KzXKwmZf4VeVJby4kYRkgLeC2kv9C9WsPLgSM8tg1poxgvtBMu4GLzMchoQ7DYd07z-GiCF9vkmnQ8RUD2jGpjFZ3hLxC5auUciowdl5_IJA";

            // ACT
            var isSuccessful = JsonWebToken.TryDecodeExpiration(token, out var expired);

            // Assert
            Assert.That(isSuccessful);
            Assert.That(expired == 300f);
        }

        [Test]
        public void DecodeExpiration_EmptyToken_GetZeroValue()
        {
            // Arrange
            string token = "";

            // ACT
            var isSuccessful = JsonWebToken.TryDecodeExpiration(token, out var expired);

            // Assert
            Assert.IsFalse(isSuccessful);
            Assert.That(expired == 0f);
        }

        [Test]
        public void DecodeExpiration_InvalidTokenFormat_GetZeroValue()
        {
            // Arrange
            string token = "eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoyMDE2MjM4NzIyLCJleHAiOjIwMTYyMzkwMjJ9.KT3xP7eWq2rjqR1NHuCrglbDmkATO2j-k8jg7UUXx5o40KIKkB2Nvsgdqr5J18XXRycY4cwGwYNtxuIIoGzQBd2nPdkCcVs39327WmmyxIPUFc42J1UcDZkjsOqZAKq3wX6gnzCep9oCqCgzIb5QwPWLnBlwFHaZc7J95eaiukuGuEbXVSoLaaiKUetOczas9Wc033AONobJdEY_3UwUyId-sUgsShfUZYvysPFM27KzXKwmZf4VeVJby4kYRkgLeC2kv9C9WsPLgSM8tg1poxgvtBMu4GLzMchoQ7DYd07z-GiCF9vkmnQ8RUD2jGpjFZ3hLxC5auUciowdl5_IJA";

            // ACT
            var isSuccessful = JsonWebToken.TryDecodeExpiration(token, out var expired);

            // Assert
            Assert.IsFalse(isSuccessful);
            Assert.That(expired == 0f);
        }
    }

}
