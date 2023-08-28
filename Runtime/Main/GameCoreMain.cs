// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

namespace AccelByte.Core
{
    internal class GameCoreMain : IPlatformMain
    {
        internal static System.Func<Utils.Infoware.XboxGameCoreInfoware> InfowareGetter;

        public static Utils.Infoware.XboxGameCoreInfoware Infoware
        {
            get
            {
                Utils.Infoware.XboxGameCoreInfoware retval = null;
                if (InfowareGetter == null)
                {
                    if(defaultInfoware == null)
                    {
                        defaultInfoware = new Utils.Infoware.XboxGameCoreInfoware();
                    }
                    retval = defaultInfoware;
                }
                else
                {
                    retval = InfowareGetter.Invoke();
                }
                return retval;
            }
        }

        private static Utils.Infoware.XboxGameCoreInfoware defaultInfoware;

        public void Run()
        {
        }

        public void Stop()
        {
        }
    }
}