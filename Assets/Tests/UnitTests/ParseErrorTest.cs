// Copyright (c) 2021 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using NUnit.Framework;

namespace Tests.UnitTests
{
    [TestFixture]
    public class ParseErrorTest
    {
        class MockBody
        {
            public string userId;
        }

        [Test]
        public void TryParse_NoResponse_IsErrorTrue()
        {
            Assert.True(((MockHttpResponse)null).TryParse().IsError);
            Assert.True(((MockHttpResponse)null).TryParseJson<MockBody>().IsError);
        }

        [Test]
        public void SuccessResponse_NoBody_IsErrorFalse([ValueSource("httpOks")] int httpOk)
        {
            this.AssertTryParseOk<object>(httpOk, null);
            this.AssertTryParseJsonOk<object>(httpOk, null);
        }

        [Test]
        public void SuccessResponse_WithBody_IsErrorFalse([ValueSource("httpOks")] int httpOk)
        {
            this.AssertTryParseOk(httpOk, new { userId = "abc12345" });
            this.AssertTryParseJsonOk(httpOk, new { userId = "abc12345" });
        }

        [Test]
        public void ErrorResponse_NoBody_ErrorIsHttpStatus([ValueSource("httpErrors")] int httpError)
        {
            this.AssertTryParseError<object>(new Error((ErrorCode)httpError), httpError, null);
            this.AssertTryParseJsonError<object>(new Error((ErrorCode)httpError), httpError, null);
        }

        [Test]
        public void ErrorResponse_OAuthErrorBody_ErrorIsHttpStatus([ValueSource("httpErrors")] int httpError)
        {
            this.AssertTryParseError(
                new Error((ErrorCode)httpError, "invalid_request"),
                httpError,
                new { error = "invalid_request" });
            this.AssertTryParseJsonError(
                new Error((ErrorCode)httpError, "invalid_request"),
                httpError,
                new { error = "invalid_request" });
        }

        [Test]
        public void ErrorResponse_OAuthErrorBody_ErrorIsHttpStatusAndDescriptionAppended([ValueSource("httpErrors")] int httpError)
        {
            this.AssertTryParseError(
                new Error((ErrorCode)httpError, "invalid_request: description"),
                httpError,
                new { error = "invalid_request", error_description = "description" });
            this.AssertTryParseJsonError(
                new Error((ErrorCode)httpError, "invalid_request: description"),
                httpError,
                new { error = "invalid_request", error_description = "description" });
        }

        [Test]
        public void ErrorResponse_GenericErrorBody_ErrorIsBodyErrorCode([ValueSource("httpErrors")] int httpError)
        {
            this.AssertTryParseError(
                new Error((ErrorCode)12345, "msg 1"),
                httpError,
                new { errorCode = 12345, errorMessage = "msg 1" });
            this.AssertTryParseError(
                new Error((ErrorCode)54321, "msg 1"),
                httpError,
                new { errorCode = 54321, errorMessage = "msg 1" });
            this.AssertTryParseJsonError(
                new Error((ErrorCode)12345, "msg 1"),
                httpError,
                new { errorCode = 12345, errorMessage = "msg 1" });
            this.AssertTryParseJsonError(
                new Error((ErrorCode)54321, "msg 1"),
                httpError,
                new { errorCode = 54321, errorMessage = "msg 1" });
        }

        [Test]
        public void ErrorResponse_PlatformErrorBody_ErrorIsBodyErrorCode([ValueSource("httpErrors")] int httpError)
        {
            this.AssertTryParseError(
                new Error((ErrorCode)12345, "msg 1"),
                httpError,
                new { numericErrorCode = 12345, errorMessage = "msg 1" });
            this.AssertTryParseError(
                new Error((ErrorCode)54321, "msg 1"),
                httpError,
                new { numericErrorCode = 54321, errorMessage = "msg 1" });
            this.AssertTryParseJsonError(
                new Error((ErrorCode)12345, "msg 1"),
                httpError,
                new { numericErrorCode = 12345, errorMessage = "msg 1" });
            this.AssertTryParseJsonError(
                new Error((ErrorCode)54321, "msg 1"),
                httpError,
                new { numericErrorCode = 54321, errorMessage = "msg 1" });
        }

        [Test]
        public void ErrorResponse_CloudSaveErrorBody_ErrorIsBodyErrorCode([ValueSource("httpErrors")] int httpError)
        {
            this.AssertTryParseError(
                new Error((ErrorCode)12345, "msg 1"),
                httpError,
                new { code = 12345, message = "msg 1" });
            this.AssertTryParseError(
                new Error((ErrorCode)54321, "msg 1"),
                httpError,
                new { code = 54321, message = "msg 1" });
            this.AssertTryParseJsonError(
                new Error((ErrorCode)12345, "msg 1"),
                httpError,
                new { code = 12345, message = "msg 1" });
            this.AssertTryParseJsonError(
                new Error((ErrorCode)54321, "msg 1"),
                httpError,
                new { code = 54321, message = "msg 1" });
        }

        [Test]
        public void ErrorResponse_MixedError_NumericErrorCodeBeforeErrorCode([ValueSource("httpErrors")] int httpError)
        {
            this.AssertTryParseError(
                new Error((ErrorCode)12345, "msg 1"),
                httpError,
                new { numericErrorCode = 12345, errorCode = 54321, errorMessage = "msg 1" });
            this.AssertTryParseError(
                new Error((ErrorCode)12345, "msg 1"),
                httpError,
                new { numericErrorCode = 12345, code = 54321, errorMessage = "msg 1" });
            this.AssertTryParseError(
                new Error((ErrorCode)12345, "msg 1"),
                httpError,
                new { errorCode = 12345, code = 54321, errorMessage = "msg 1" });
            this.AssertTryParseJsonError(
                new Error((ErrorCode)12345, "msg 1"),
                httpError,
                new { numericErrorCode = 12345, errorCode = 54321, errorMessage = "msg 1" });
            this.AssertTryParseJsonError(
                new Error((ErrorCode)12345, "msg 1"),
                httpError,
                new { numericErrorCode = 12345, code = 54321, errorMessage = "msg 1" });
            this.AssertTryParseJsonError(
                new Error((ErrorCode)12345, "msg 1"),
                httpError,
                new { errorCode = 12345, code = 54321, errorMessage = "msg 1" });
        }

        private static readonly int[] httpOks = { 200, 201, 204, 299 };

        private static readonly int[] httpErrors = { 400, 401, 403, 409, 422, 499, 500, 577, 599 };

        private void AssertTryParseOk<T>(int status, T body) where T : class
        {
            var response = new MockHttpResponse { Code = status, BodyBytes = body?.ToUtf8Json() };
            Result result = response.TryParse();

            Assert.False(result.IsError);
        }

        private void AssertTryParseJsonOk<T>(int status, T body) where T : class
        {
            var response = new MockHttpResponse { Code = status, BodyBytes = body?.ToUtf8Json() };
            Result<T> result = response.TryParseJson<T>();

            Assert.False(result.IsError);
        }

        private void AssertTryParseError<E>(Error error, int status, E errorBody) where E : class
        {
            var response = new MockHttpResponse { Code = status, BodyBytes = errorBody?.ToUtf8Json() };
            Result result = response.TryParse();

            Assert.AreEqual(error.Code, result.Error.Code);
            Assert.AreEqual(error.Message, result.Error.Message);
        }

        private void AssertTryParseJsonError<E>(Error error, int status, E errorBody) where E : class
        {
            var response = new MockHttpResponse { Code = status, BodyBytes = errorBody?.ToUtf8Json() };
            Result<MockBody> result = response.TryParseJson<MockBody>();

            Assert.AreEqual(error.Code, result.Error.Code);
            Assert.AreEqual(error.Message, result.Error.Message);
        }
    }
}