// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections.Generic;

namespace AccelByte.Core
{
    internal class CoreHeartBeat
    {
        private bool acceptCommand;
        private List<IWaitCommand> newCommands;
        private List<IWaitCommand> waitCommands;
        private List<IWaitCommand> removedCommands;
        
        public CoreHeartBeat()
        {
            newCommands = new List<IWaitCommand>();
            waitCommands = new List<IWaitCommand>();
            removedCommands = new List<IWaitCommand>();
            acceptCommand = true;
            AccelByteSDKMain.AddGameUpdateListener(OnUpdate);
        }

        ~CoreHeartBeat()
        {
            Reset();
        }

        public void Reset()
        {
            acceptCommand = false;
            AccelByteSDKMain.RemoveGameUpdateListener(OnUpdate);
            lock (waitCommands)
            {
                waitCommands.Clear();
            }
        }
        
        public void Wait(IWaitCommand newCommand)
        {
            if (acceptCommand)
            {
                lock (newCommands)
                {
                    newCommands.Add(newCommand);
                }
            }
        }

        void OnUpdate(float dt)
        {
            removedCommands.Clear();
            lock (waitCommands)
            {
                lock (newCommands)
                {
                    if (newCommands.Count > 0)
                    {
                        waitCommands.AddRange(newCommands);
                        newCommands.Clear();
                    }
                }
                
                for (int i = 0; i < waitCommands.Count; i++)
                {
                    if (waitCommands[i].IsCancelled())
                    {
                        removedCommands.Add(waitCommands[i]);
                        continue;
                    }

                    if (waitCommands[i].Update(dt))
                    {
                        removedCommands.Add(waitCommands[i]);
                    }
                }

                foreach (IWaitCommand removedCommand in removedCommands)
                {
                    waitCommands.Remove(removedCommand);
                    if (!removedCommand.IsCancelled())
                    {
                        removedCommand?.TriggerDone();
                    }
                }
            }
        }
    }
}