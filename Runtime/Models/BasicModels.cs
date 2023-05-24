// Copyright (c) 2018 - 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using UnityEngine.Scripting;

namespace AccelByte.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum FileType
    {
        [Description("jpeg")]
        JPEG,
        [Description("jpg")]
        JPG,
        [Description("png")]
        PNG,
        [Description("bmp")]
        BMP,
        [Description("gif")]
        GIF,
        [Description("mp3")]
        MP3,
        [Description("bin")]
        BIN,
        [Description("webp")]
        WEBP
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum UploadCategory
    {
        [Description("Default")]
        DEFAULT,
        [Description("Reporting")]
        REPORTING,
    }

    [DataContract, Preserve]
    public class Time
    {
        [DataMember] public System.DateTime currentTime;
    }

    [DataContract, Preserve]
    public class GenerateUploadURLResult
    {
        [DataMember] public string url;
        [DataMember] public string accessUrl;
        [DataMember] public string method;
        [DataMember] public string contentType;
    }
}