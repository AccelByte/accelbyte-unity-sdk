// Copyright (c) 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

namespace AccelByte.Models
{
    [UnityEngine.Scripting.Preserve]
    public class UploadToOptionalParameters : OptionalParametersBase
    {
        public string ContentType = "application/octet-stream";
    }
    
    [UnityEngine.Scripting.Preserve]
    public class UploadBinaryOptionalParameters : OptionalParametersBase
    {
        public string ContentType;
    }
        
    [UnityEngine.Scripting.Preserve]
    public class DownloadFromOptionalParameters : OptionalParametersBase
    {
    }
        
    [UnityEngine.Scripting.Preserve]
    public class DownloadBinaryFromOptionalParameters : OptionalParametersBase
    {
    }
    
    [UnityEngine.Scripting.Preserve]
    internal class UploadFileOptionalParameters : OptionalParametersBase
    {
    }
    
    [UnityEngine.Scripting.Preserve]
    internal class DownloadFileOptionalParameters : OptionalParametersBase
    {
        public bool IsBinary;
    }
}