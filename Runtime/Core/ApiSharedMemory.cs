// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;

namespace AccelByte.Core
{
	internal class ApiSharedMemory
    {
        public Action<string> OnClosestRegionChanged;
        public Action OnLobbyConnected;
        public PredefinedEventScheduler PredefinedEventScheduler;
        public AccelByteNetworkConditioner NetworkConditioner;

        public string ClosestRegion
        {
            get
            {
                return closestRegion;
            }
            set
            {
                if (string.IsNullOrEmpty(value) || closestRegion == value)
                {
                    return;
                }
                closestRegion = value;
                OnClosestRegionChanged?.Invoke(closestRegion);
            }
        }

        public bool LobbyConnected
        {
            get
            {
                return lobbyConnected;
            }
            set
            {
                lobbyConnected = value;
                if(lobbyConnected)
                {
                    OnLobbyConnected?.Invoke();
                }
            }
        }

        public Utils.AccelByteIdValidator IdValidator
        {
            get
            {
                if (accelByteIdValidator == null)
                {
                    accelByteIdValidator = new Utils.AccelByteIdValidator();
                }
                return accelByteIdValidator;
            }
            set
            {
                accelByteIdValidator = value;
            }
        }
        Utils.AccelByteIdValidator accelByteIdValidator;
        private string closestRegion;
        private bool lobbyConnected;
    }
}
