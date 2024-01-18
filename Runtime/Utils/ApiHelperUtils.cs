// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

namespace AccelByte.Utils
{
    internal static class ApiHelperUtils
    {
        public static Error CheckForNullOrEmpty(params object[] inObjects)
        {
            if (inObjects == null || inObjects.Length == 0)
            {
                return new Error(ErrorCode.InvalidRequest, $"[API-Error] Parameter cannot be null.");
            }

            for(int i = 0; i < inObjects.Length; i++)
            {
                if (inObjects[i] == null)
                {
                    return new Error(ErrorCode.InvalidRequest, $"[API-Error] Parameter index[{i}] " +
                        $"cannot be null.");
                }

                if (inObjects[i].GetType() == typeof(string))
                {
                    if (string.IsNullOrEmpty((string)inObjects[i]))
                    {
                        return new Error(ErrorCode.InvalidRequest, $"[API-Error] Parameter index[{i}] " +
                            $"of type {inObjects[i].GetType()} cannot be null or empty.");
                    }
                }
            }

            return null;
        }
    }
}
