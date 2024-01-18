// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
#if UNITY_STANDALONE_LINUX || UNITY_EDITOR_LINUX
using System.Runtime.InteropServices;

namespace AccelByte.Core
{
    internal class AccelByteSignalHandler
    {
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate void SignalHandlerDelegate(int signalCode);

        [DllImport("lib_sigterm_handler", EntryPoint = "on_term", CallingConvention = CallingConvention.StdCall)]
        protected static extern void OnReceivedSignalTerm([MarshalAs(UnmanagedType.FunctionPtr)] SignalHandlerDelegate handler);

        bool enableSignalTerm = true;

        public AccelByteSignalHandler()
        {
#if UNITY_EDITOR_WIN //To avoid running sigterm in linux dedicated server but with windows editor
            enableSignalTerm = false;
#endif
            if(enableSignalTerm)
            {
                OnReceivedSignalTerm(null);
            }
        }

        ~AccelByteSignalHandler()
        {
            ShutDown();
        }

        public void ShutDown()
        {
            if (enableSignalTerm)
            {
                OnReceivedSignalTerm(null);
            }
        }

        public virtual void SetSignalHandlerAction(SignalHandlerDelegate newHandlerAction)
        {
            if (enableSignalTerm)
            {
                OnReceivedSignalTerm(newHandlerAction);
            }
        }
    }
}
#endif