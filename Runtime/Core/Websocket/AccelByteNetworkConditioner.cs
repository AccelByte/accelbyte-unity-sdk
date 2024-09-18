// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections.Generic;
using UnityEngine;

namespace AccelByte.Core
{
    public class AccelByteNetworkConditioner
    {
        internal event Action<bool, string> OnFailRateCalculated;

        private bool isEnabled = false;
        private int overallFailRate = 0;
        private readonly Dictionary<string, int> messageFailRate = new Dictionary<string, int>();

        private readonly int initialSeed;
        private int seed;
        System.Random random;

        private IDebugger logger;

        public AccelByteNetworkConditioner(IDebugger logger = null)
        {
            initialSeed = (int)System.DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            seed = initialSeed;
            random = new System.Random(seed);
            this.logger = logger;

            logger?.LogVerbose(
                $"[AccelByteNetworkConditioner] Initialized AccelByte network conditioner with seed {seed}."
            );
        }

        #region Public Methods

        /// <summary>
        /// Get enabled status of network conditioner.
        /// </summary>
        /// <returns>true if network conditioner is enabled.</returns>
        public bool IsEnabled => isEnabled;

        /// <summary>
        /// Set enabled status of network conditioner.
        /// </summary>
        /// <param name="inEnabled">Network conditioner enabled state.</param>
        public void SetEnabled(bool inEnabled)
        {
            isEnabled = inEnabled;
            logger?.Log(
                $"[AccelByteNetworkConditioner] {(isEnabled ? "Enabled" : "Disabled")} " +
                $"with overall fail rate {overallFailRate}%."
            );
        }

        /// <summary>
        /// Get overall fail rate percentage.
        /// </summary>
        /// <returns>overall fail rate value.</returns>
        public int GetOverallFailRate => overallFailRate;

        /// <summary>
        /// Set overall fail rate percentage
        /// </summary>
        /// <param name="failRate">Percentage fail rate to use, 
        /// valid value is between 0 (always pass) to 100 (always fail).</param>
        /// <returns>true if overall fail rate set successfully.</returns>
        public bool SetOverallFailRate(int failRate)
        {
            try
            {
                LimitCheckFailRate(failRate);

                overallFailRate = failRate;
                logger?.Log($"Overall percentage failure rate set: {failRate}");

                return true;
            }
            catch (Exception exception)
            {
                logger?.LogWarning(exception.Message);
                return false;
            }
        }

        /// <summary>
        /// Set fail rate percentage of a particular message type,
        /// override the overall fail rate value for this message type.
        /// </summary>
        /// <param name="messageType">The message type name to override</param>
        /// <param name="failRate">Percentage fail rate to use, valid value is between 
        /// 0 (always pass) to 100 (always fail).</param>
        /// <returns>true if fail rate set successfully</returns>
        public bool SetMessageFailRate(string messageType, int failRate)
        {
            try
            {
                LimitCheckFailRate(failRate);

                messageFailRate.Add(messageType, failRate);
                logger?.Log($"[AccelByteNetworkConditioner] {messageType} percentage failure rate set: {failRate}");

                return true;
            }
            catch (Exception exception)
            {
                logger?.LogWarning(exception.Message);
                return false;
            }
        }

        /// <summary>
        /// Get fail rate percentage of a particular message type.
        /// </summary>
        /// <param name="messageType">The message type name.</param>
        /// <returns>Fail rate percentage value for that message type, 0 if the message type is not set.</returns>
        public int GetMessageFailRate(string messageType)
        {
            if (!messageFailRate.ContainsKey(messageType))
            {
                logger?.LogWarning(
                    $"[AccelByteNetworkConditioner] Get message fail rate with message type {messageType} not found."
                );

                return 0;
            }

            return messageFailRate[messageType];
        }

        /// <summary>
        /// Get dictionary of fail rate percentage of all message type that is set
        /// </summary>
        /// <param name="outMessageFailRate">Out param for fail rate percentage.</param>
        public void GetAllMessageFailRate(out Dictionary<string, int> outMessageFailRate)
        {
            outMessageFailRate = messageFailRate;
        }

        /// <summary>
        /// Clear all message fail rate that was set.
        /// </summary>
        public void ClearMessageFailRate()
        {
            messageFailRate.Clear();

            logger?.Log("[AccelByteNetworkConditioner] All notification percentage failure rates cleared.");
        }

        /// <summary>
        /// Remove fail rate of a message type.
        /// </summary>
        /// <param name="messageType">Name of message type to remove.</param>
        /// <returns>true if fail rate removed successfully.</returns>
        public bool RemoveMessageFailRate(string messageType)
        {
            if (!messageFailRate.ContainsKey(messageType))
            {
                logger?.LogWarning(
                    $"[AccelByteNetworkConditioner] Failed to remove message fail rate " +
                    $"with message type {messageType} not found."
                );

                return false;
            }

            messageFailRate.Remove(messageType);
            return true;
        }

        /// <summary>
        /// Set random seed to be used in calculation.
        /// </summary>
        /// <param name="seed">Random seed to use.</param>
        public void SetRandomSeed(int seed)
        {
            this.seed = seed;
            random = new System.Random(seed);

            logger?.Log($"[AccelByteNetworkConditioner] Setting random seed as {seed}");
        }

        /// <summary>
        /// Get current random seed used.
        /// </summary>
        /// <returns>random seed used.</returns>
        public int GetCurrentRandomSeed => seed;

        /// <summary>
        /// Get initial random seed used. Initial seed 
        /// is taken from unix time from the moment an instance of this class is created.
        /// </summary>
        /// <returns>initial random seed used.</returns>
        public int GetInitialRandomSeed => initialSeed;

        #endregion

        #region Internal/Private Methods

        /// <summary>
        /// Calculate with random if a MessageType should be fail or pass.
        /// </summary>
        /// <param name="messageType">Name of message type.</param>
        /// <returns>true should fail, false is pass.</returns>
        internal bool CalculateFailRate(string messageType)
        {
#if !UNITY_EDITOR && !DEVELOPMENT_BUILD
            return false;
#endif
            if (!isEnabled)
            {
                return false;
            }

            int failRate = messageFailRate.ContainsKey(messageType) ? messageFailRate[messageType] : overallFailRate;
            int randomVal = random.Next(0, 100);
            bool isFail = failRate >= randomVal;

            if (isFail)
            {
                logger?.Log($"[AccelByteNetworkConditioner] Killed message {messageType}");
            }

            OnFailRateCalculated?.Invoke(isFail, messageType);
            return isFail;
        }

        private void LimitCheckFailRate(int failRate)
        {
            if (failRate < 0 || failRate > 100)
            {
                throw new Exception(
                    message: $"[AccelByteNetworkConditioner] Setting message fail rate with invalid value " +
                    $"{failRate}, value must be between 0-100."
                );
            }
        }

        #endregion
    }
}
