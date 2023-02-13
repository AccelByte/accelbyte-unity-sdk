// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

namespace AccelByte.Core
{
    internal class AccelByteHttpCacheData
    {
        readonly IHttpRequest request;
        readonly IHttpResponse response;
        readonly Error responseError;

        public AccelByteHttpCacheData(IHttpRequest request, IHttpResponse response, Error responseError)
        {
            this.request = request;
            this.response = response;
            this.responseError = responseError;
        }

        public IHttpRequest Request
        {
            get
            {
                return request;
            }
        }

        public IHttpResponse Response
        {
            get
            {
                return response;
            }
        }

        public Error ResponseError
        {
            get
            {
                return responseError;
            }
        }
    }
}
