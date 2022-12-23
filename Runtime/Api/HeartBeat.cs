// Copyright (c) 2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

#if !UNITY_SERVER
using System;
using System.Collections;
using System.Collections.Generic;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    /// <summary>
    /// Send heart beat event.
    /// </summary>
    public class HeartBeat : WrapperBase
    {
        private readonly HeartBeatApi api;
        private readonly CoroutineRunner coroutineRunner;

        private readonly TimeSpan heartBeatInterval = TimeSpan.FromMinutes(1);

        private Coroutine heartBeatCoroutine = null;
        private bool isHeartBeatJobStarted = false;
        private bool isHeartBeatEnabled = false;
        private ResultCallback OnHeartBeatResponse = null;

        private Dictionary<string, object> heartBeatData = new Dictionary<string, object>();

        internal HeartBeat(HeartBeatApi inApi
            , CoroutineRunner inCoroutineRunner)
        {
            Assert.IsNotNull(inApi, "inApi parameter can not be null.");
            Assert.IsNotNull(inCoroutineRunner, "inCoroutineRunner parameter can not be null");

            api = inApi;
            coroutineRunner = inCoroutineRunner;
            InitializeHeartBeatData();
            if(isHeartBeatEnabled)
            {
                StartHeartBeatScheduler();
            }
        }

        /// <summary>
        /// Enables or disables heart beat
        /// </summary>
        public void SetHeartBeatEnabled(bool enable)
        {
            isHeartBeatEnabled = enable;
            if(isHeartBeatEnabled)
            {
                StartHeartBeatScheduler();
            }
            else
            {
                ResetHeartBeatScheduler();
            }
        }

        /// <summary>
        /// set heart beat response callback
        /// </summary>
        public void SetHeartBeatCallback(ResultCallback callback)
        {
            OnHeartBeatResponse = callback;
        }

        private IEnumerator RunPeriodicHeartBeat()
        {
            while (isHeartBeatEnabled)
            {
                coroutineRunner.Run(
                    api.SendHeartBeatEvent(heartBeatData, OnHeartBeatResponse));
                yield return new WaitForSeconds((float)heartBeatInterval.TotalSeconds);
            }

            ResetHeartBeatScheduler();
        }

        private void StartHeartBeatScheduler()
        {
            heartBeatCoroutine = coroutineRunner.Run(RunPeriodicHeartBeat());
            isHeartBeatJobStarted = true;
        }

        private void ResetHeartBeatScheduler()
        {
            isHeartBeatJobStarted = false;
            heartBeatCoroutine = null;
        }

        private void InitializeHeartBeatData()
        {
            string publisherNamespace = AccelBytePlugin.Config.PublisherNamespace;
            string customerName = AccelBytePlugin.Config.CustomerName;
            if (customerName == "")
            {
                customerName = AccelBytePlugin.Config.PublisherNamespace;
            }
            SettingsEnvironment env = AccelByteSettings.Instance.GetEditedEnvironment();
            string envString = "";
            switch(env)
            {
                case SettingsEnvironment.Development:
                    envString = "dev";
                    break;
                case SettingsEnvironment.Certification:
                    envString = "cert";
                    break;
                case SettingsEnvironment.Production:
                    envString = "prod";
                    break;
                case SettingsEnvironment.Default:
                    envString = "default";
                    break;
            }
            heartBeatData.Add("customer", customerName);
            heartBeatData.Add("pn", publisherNamespace);
            heartBeatData.Add("environment", envString);
            heartBeatData.Add("game", AccelBytePlugin.Config.Namespace);
        }
    }
}
#endif