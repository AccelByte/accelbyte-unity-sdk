// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections.Generic;
using System.Linq;
using UnityEngine.Rendering;

namespace AccelByte.Core
{
    public class AccelBytePastSessionRecordManager
    {
        internal readonly IAccelByteCacheImplementation<HashSet<string>> Records;

        private int maxStoredSessionIdCount = 5;

        public int MaxStoredSessionIdCount
        {
            get
            {
                return maxStoredSessionIdCount;
            }
            set
            {
                if (value > 0)
                {
                    maxStoredSessionIdCount = value;
                }
            }
        }

        public AccelBytePastSessionRecordManager()
        {
            Records = new AccelByteDictionaryCacheImplementation<HashSet<string>>();
        }

        public IEnumerable<string> GetPastSessionIds(string userId)
        {
            return Records.Retrieve(userId);
        }
        
	    public void InsertPastSessionId(string userId, params string[] sessionId)
        {
            var newSessionId = sessionId.ToHashSet();

            var cache = Records.Retrieve(userId);
            if (cache != null)
            {
                foreach (var item in newSessionId)
                {
                    cache.Add(item);
                }

                newSessionId = cache;
            }

            if (newSessionId.Count > maxStoredSessionIdCount)
            {
                newSessionId.TakeLast(maxStoredSessionIdCount);
            }

            Records.Emplace(userId, newSessionId);
        }

        public void RemoveSpecificCachedPastSessionIds(string userId, string sessionId)
        {
            var oldCache = Records.Retrieve(userId);
            var updatedCache = oldCache.Where(item => item != sessionId).ToHashSet();
            Records.Emplace(userId, updatedCache);
        }

	    public void ResetCachedPastSessionIds(string userId)
        {
            Records.Remove(userId);
        }

        public void ResetAllCachedPastSessionIds()
        {
            Records.Empty();
        }
    }
}
