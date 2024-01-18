// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections.Generic;

namespace AccelByte.Utils
{
    internal static class CommandLineArgs
    {
        /// <summary>
        /// Parses Argument Value if parameter exist in Arguments
        /// </summary>
        /// <param name="argName">Parameter to filter the argument value</param>
        /// <returns>The argument value of parameter if exist</returns>
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

        /// <summary>
        /// Parses Argument Value if parameter exist in specific Arguments
        /// </summary>
        /// <param name="args">Array of specific arguments</param>
        /// <param name="argName">Parameter to filter the argument value</param>
        /// <returns>The argument value of parameter from specific array of arguments if exist</returns>
        public static string GetArg(string[] args, string argName)
        {
            if (args == null)
            {
                return null;
            }

            for (int index = 0; index < args.Length; index++)
            {
                if (args[index] == argName && args.Length > index + 1)
                {
                    return args[index + 1];
                }
            }
            return null;
        }

        /// <summary>
        /// Parses Argument Values if parameter exist in Arguments
        /// </summary>
        /// <param name="argName">Parameter to filter the argument value</param>
        /// <returns>The argument values of parameter from specific array of arguments if exist</returns>
        public static string[] GetArgs(string[] args, string argName) 
        {
            List<string> argValues = new List<string>();

            if (args == null)
            {
                return null;
            }

            for (int index = 0; index < args.Length; index++)
            {
                if (args[index] == argName && args.Length > index + 1)
                {
                    argValues.Add(args[index + 1]);
                }
            }
            return argValues.ToArray();
        }
    }
}
