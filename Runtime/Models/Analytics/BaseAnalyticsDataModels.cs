// Copyright (c) 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Utils;
using System;

namespace AccelByte.Models
{
    public class AccelByteId : BaseAnalyticsData
    {
        private readonly AccelByteIdValidator idValidator = new AccelByteIdValidator();

        public AccelByteId(string id)
        {
            if (idValidator.IsAccelByteIdValid(id))
            {
                data = id;
            }
        }
    }

    public class FreeFormData : BaseAnalyticsData
    {
        public FreeFormData(string id)
        {
            data = id;
        }
    }

    public class NumberData : BaseAnalyticsData
    {
        public NumberData(string val)
        {
            if (bool.TryParse(val, out var booleanValue))
            {
                data = booleanValue;
            }
            else if (long.TryParse(val, out var longValue))
            {
                data = longValue;
            }
            else if (double.TryParse(val, out var doubleValue))
            {
                data = doubleValue;
            }
        }

        public NumberData(bool val)
        {
            data = val;
        }

        public NumberData(int val)
        {
            data = val;
        }

        public NumberData(long val)
        {
            data = val;
        }

        public NumberData(float val)
        {
            data = val;
        }

        public NumberData(double val)
        {
            data = val;
        }
    }

    public class StringNumberData : BaseAnalyticsData
    {
        private const string formatSpecifier = "n2";

        public StringNumberData(string val)
        {
            if (long.TryParse(val, out long _))
            {
                data = val;
            }
            else if (double.TryParse(val, out double doubleValue))
            {
                data = doubleValue.ToString(formatSpecifier);
            }
        }

        public StringNumberData(int val)
        {
            data = val.ToString();
        }

        public StringNumberData(long val)
        {
            data = val.ToString();
        }

        public StringNumberData(double val)
        {
            data = val.ToString(formatSpecifier);
        }
    }
    
    public class DateTimeData : BaseAnalyticsData
    {
        public DateTimeData(string val)
        {
            if (DateTime.TryParse(val, out DateTime dateTimeValue))
            {
                data = dateTimeValue;
            }
        }

        public DateTimeData(DateTime val)
        {
            data = val;
        }
    }

    public class BaseAnalyticsData
    {
        protected object data;

        public virtual bool IsValid()
        {
            return data != null;
        }

        public override string ToString()
        {
            return IsValid() ? data.ToString() : string.Empty;
        }

        protected bool ToBoolean()
        {
            return Convert.ToBoolean(data);
        }

        protected int ToInt()
        {
            return Convert.ToInt32(data);
        }

        protected long ToLong()
        {
            return Convert.ToInt64(data);
        }

        protected double ToDouble()
        {
            return Convert.ToDouble(data);
        }

        protected DateTime ToDateTime()
        {
            return Convert.ToDateTime(data);
        }
    }
}