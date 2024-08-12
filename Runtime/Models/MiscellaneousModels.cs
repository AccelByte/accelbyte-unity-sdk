// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System.Runtime.Serialization;
using UnityEngine.Scripting;

namespace AccelByte.Models
{
    [DataContract, Preserve]
    public class CountryGroup
    {
        [DataMember(Name = "countryGroupCode")] public string CountryGroupCode;
        [DataMember(Name = "countryGroupName")] public string CountryGroupName;
        [DataMember(Name = "countries")] public CountryGroupInfo[] Countries;
    }

    [DataContract, Preserve]
    public class CountryGroupInfo
    {
        [DataMember(Name = "code")] public string Code;
        [DataMember(Name = "name")] public string Name;
    }
}
