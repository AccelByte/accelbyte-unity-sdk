// Copyright (c) 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

namespace AccelByte.Editor
{
    public static class EditorCommand
    {
        public static void ExecuteCommand(AccelByteEditorCommand command)
        {
        }
    }

    public class AccelByteEditorCommand
    {
        public string CommandName;
        public string AttributeJson;
    }
}