// Copyright (c) 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;

namespace AccelByte.Models
{
    public class OptionalParametersBase
    {
        /// <summary>
        /// Accelbyte logger instance to use logging functions within the interface.
        /// </summary>
        internal IDebugger Logger;
        internal HttpOperator OverrideHttpOperator;
        internal Utils.IApiTracker ApiTracker;
            
    }

    public struct AdditionalHttpParameters
    {
        /// <summary>
        /// Accelbyte logger instance to use logging functions within the http call process.
        /// </summary>
        internal IDebugger Logger;
        internal Utils.IApiTracker ApiTracker;

        static internal AdditionalHttpParameters CreateFromOptionalParameters(OptionalParametersBase optionalParameter)
        {
            var retval = new AdditionalHttpParameters();
            if (optionalParameter != null)
            {
                retval.Logger = optionalParameter.Logger;
                retval.ApiTracker = optionalParameter.ApiTracker;
            }
            return retval;
        }
    }
}
