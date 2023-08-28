// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
namespace AccelByte.Core
{
    internal class PS4Main : IPlatformMain
    {
        internal static System.Func<Utils.Infoware.PlayStation4Infoware> InfowareGetter;

        public static Utils.Infoware.PlayStation4Infoware Infoware
        {
            get
            {
                Utils.Infoware.PlayStation4Infoware retval = null;
                if (InfowareGetter == null)
                {
                    if(defaultInfoware == null)
                    {
                        defaultInfoware = new Utils.Infoware.PlayStation4Infoware();
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

        private static Utils.Infoware.PlayStation4Infoware defaultInfoware;

        public void Run()
        {
        }

        public void Stop()
        {
        }
    }
}