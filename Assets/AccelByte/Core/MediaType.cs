// Copyright (c) 2018-2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

namespace AccelByte.Core
{
    //Type-safe enum for content type / media type
    public sealed class MediaType
    {
        private readonly string name;

        public static readonly MediaType ApplicationForm =
            new MediaType("application/x-www-form-urlencoded; charset=utf-8");

        public static readonly MediaType ApplicationJson = new MediaType("application/json; charset=utf-8");
        public static readonly MediaType TextPlain = new MediaType("text/plain; charset=utf-8");
        public static readonly MediaType ApplicationOctetStream = new MediaType("application/octet-stream; charset=utf-8");

        private MediaType(string name) { this.name = name; }

        public override string ToString() { return this.name; }
    }
}