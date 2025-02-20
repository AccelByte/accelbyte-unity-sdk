// Copyright (c) 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Reflection;

namespace AccelByte.Utils
{
    internal static class TypeUtils
    {
        public static void CopyField(this object to, object source)
        {
            if (to != null && source != null)
            {
                FieldInfo[] sourceFieldsInfo = source.GetType().GetFields();
                System.Type toType = to.GetType();
                foreach (FieldInfo fieldInfo in sourceFieldsInfo)
                {
                    FieldInfo toField = toType.GetField(fieldInfo.Name);
                    if (toField != null)
                    {
                        object sourceFieldValue = fieldInfo.GetValue(source);
                        toField.SetValue(to, sourceFieldValue);
                    }
                }
            }
        }
    }
}
