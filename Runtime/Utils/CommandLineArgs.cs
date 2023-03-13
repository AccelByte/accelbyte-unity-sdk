// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

namespace AccelByte.Utils
{
    internal static class CommandLineArgs
    {
        /// <summary>
        /// Parses Argument Value if parameter exist in Arguments
        /// </summary>
        /// <param name="argName"></param>
        /// <returns>The value</returns>
        public static string GetArg(string argName)
        {
            string[] args = System.Environment.GetCommandLineArgs();
            for (int index = 0; index < args.Length; index++)
            {
                if (args[index] == argName && args.Length > index + 1)
                {
                    return args[index + 1];
                }
            }
            return null;
        }
    }
}
