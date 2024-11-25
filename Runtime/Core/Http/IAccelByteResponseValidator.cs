// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
namespace AccelByte.Core
{
    internal interface IAccelByteResponseValidator
    {
        public void Validate<T>(IHttpRequest request, IHttpResponse response);
        public void Validate(IHttpRequest request, IHttpResponse response);
    }
}