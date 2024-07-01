// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AccelByte.Core
{
    public class AccelByteNotificationBuffer
    {
        private string lastSequenceId = null;
        private int lastSequenceNumber = default;
        private DateTime lastSentAt = default;

        private HashSet<UserNotification> buffer = new HashSet<UserNotification>();

        private bool isBuffering = false;
        public bool IsBuffering => isBuffering;

        public bool TryAddBuffer(UserNotification userNotification)
        {
            if (!HasValidSequence(userNotification))
            {
                return false;
            }

            if (IsDuplicateNotification(userNotification))
            {
                UpdateLastSequence(userNotification);
                return false;
            }

            if (IsNotificationMissing(userNotification) || isBuffering)
            {
                isBuffering = true;

                return AddToBuffer(userNotification);
            }

            UpdateLastSequence(userNotification);
            return false;
        }

        public bool TryAddMissingNotifications(UserNotification[] userNotifications)
        {
            if (!isBuffering)
            {
                return false;
            }

            foreach(var notif in userNotifications)
            {
                if (notif.SequenceId == lastSequenceId && notif.SequenceNumber <= lastSequenceNumber)
                {
                    continue;
                }

                AddToBuffer(notif);
            }

            return true;
        }

        public DateTime GetLastNotificationReceivedTime()
        {
            return lastSentAt;
        }

        public string GetLastNotificationSequenceId()
        {
            return lastSequenceId;
        }

        public int GetLastNotificationSequenceNumber()
        {
            return lastSequenceNumber;
        }

        public void Clear()
        {
            if (buffer.Count == 0)
            {
                return;
            }

            UpdateLastSequence(GetSortedBuffer().Last());
            buffer.Clear();

            isBuffering = false;
        }

        public UserNotification[] GetSortedBuffer()
        {
            return buffer
                .OrderBy(notif => notif.SentAt)
                .ThenBy(notif => notif.SequenceNumber).ToArray();
        }

        private void UpdateLastSequence(UserNotification userNotification)
        {
            lastSequenceId = userNotification.SequenceId;
            lastSequenceNumber = userNotification.SequenceNumber;
            lastSentAt = userNotification.SentAt;
        }

        private bool HasValidSequence(UserNotification userNotification)
        {
            if (userNotification.SequenceNumber <= 0 || string.IsNullOrEmpty(userNotification.SequenceId))
            {
                AccelByteDebug.LogVerbose(
                    $"Notification has no sequence identifiers, skipping.\n" +
                    $"sequenceId: {userNotification.SequenceId}\n" +
                    $"sequenceNumber: {userNotification.SequenceNumber}");
                return false;
            }
            return true;
        }

        private bool IsNotificationMissing(UserNotification userNotification)
        {
            // Default / initialized values
            if (string.IsNullOrEmpty(lastSequenceId) && lastSequenceNumber == 0 && lastSentAt == default)
            {
                return false;
            }

            // No reconnection occured (same SequenceId) and SequenceNumber incremented by one.
            if (lastSequenceId == userNotification.SequenceId 
                && lastSequenceNumber == userNotification.SequenceNumber - 1)
            {
                return false;
            }

            AccelByteDebug.LogWarning(
                $"Missing notification(s) detected. Last valid notification:\n" +
                $"SequenceId: {lastSequenceId}\n" +
                $"SequenceNumber: {lastSequenceNumber}\n" +
                $"LastSentAt: {lastSentAt}\n" +
                $"Incoming notification:\n" +
                $"SequenceId: {userNotification.SequenceId}\n" +
                $"SequenceNumber: {userNotification.SequenceNumber}\n" +
                $"LastSentAt: {userNotification.SentAt}");
            return true;
        }

        private bool IsDuplicateNotification(UserNotification userNotification)
        {
            if (lastSequenceId == userNotification.SequenceId && userNotification.SequenceNumber <= lastSequenceNumber)
            {
                AccelByteDebug.LogVerbose(
                    $"Duplicate notification detected:\n" +
                    $"{userNotification.ToJsonString()}");
                return true;
            }
            return false;
        }

        private bool AddToBuffer(UserNotification userNotification)
        {
            return buffer.Add(userNotification);
        }
    }
}
