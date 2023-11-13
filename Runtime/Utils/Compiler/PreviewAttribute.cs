// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Runtime.InteropServices;

namespace AccelByte.Utils.Attributes
{
    /// <summary>
    /// This attribute is used to designate methods that are preview or early access.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    [ComVisible(true)]
    internal class PreviewAttribute : Attribute
    {
        
    }
}
