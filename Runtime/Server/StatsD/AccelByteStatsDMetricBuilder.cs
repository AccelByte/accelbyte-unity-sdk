// Copyright (c) 2023 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using System.Collections.Generic;

namespace AccelByte.Server
{
    public class AccelByteStatsDMetricBuilder
    {
        int tagIndex = 0;
        const int bufferSize = 1 << 16;
        internal StatsDMetric metric;
        private IDebugger logger;

        public AccelByteStatsDMetricBuilder(string inName, string inValue)
        {
            metric = new StatsDMetric()
            {
                Name = inName,
                Value = inValue
            };
        }

        public AccelByteStatsDMetricBuilder AddTag(string tagValue)
        {
            if (string.IsNullOrEmpty(tagValue))
            {
                logger?.LogError($"tagValue must not be empty or null!");
                return this;
            }

            List<string> tagList = new List<string>();
            if (metric.Tags != null)
            {
                tagList.AddRange(metric.Tags);
            }

            if (tagList.Contains(tagValue))
            {
                logger?.LogError($"tagValue already exists on metric!");
                return this;
            }

            tagList.Add(tagValue);
            metric.Tags = tagList.ToArray();

            return this;
        }

        public string Build()
        {
            string builtString = $"{metric.Name}:{metric.Value}|g";

            if (metric.Tags == null || metric.Tags.Length < 1)
            {
                return builtString;
            }

            builtString += "|#";
            foreach(string tag in metric.Tags)
            {
                string tagName = $"Tag{tagIndex}";
                builtString += $"{tagName}:{metric.Tags[tagIndex]}";

                tagIndex++;
                if (metric.Tags.Length < tagIndex)
                {
                    builtString += ",";
                }
            }

            return builtString;
        }

        public void SetLogger(IDebugger newLogger)
        {
            logger = newLogger;
        }
    }
}
