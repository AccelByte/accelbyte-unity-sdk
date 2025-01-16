// Copyright (c) 2023 - 2024 AccelByte Inc. All Rights Reserved.
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
        public AccelByteMessagingSystem MessagingSystem;
        public AccelByteNotificationSender NotificationSender;
        public AccelByteTimeManager TimeManager;
        public AccelBytePastSessionRecordManager PastSessionRecordManager;
        public Models.DeviceIdGeneratorConfig DeviceIdGeneratorConfig;
        public IDebugger Logger;
        public CoreHeartBeat CoreHeartBeat;

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
        public AccelByte.Utils.AccelByteServiceTracker ServiceTracker;
    }
}
