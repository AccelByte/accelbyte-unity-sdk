// Copyright (c) 2023 - 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using UnityEngine.Scripting;
using System.Collections.Generic;

namespace AccelByte.Models
{
    [JsonConverter(typeof(StringEnumConverter)), System.Serializable]
    public enum PresenceBroadcastEventGameState
    {
        [EnumMember(Value = "OUT_OF_GAMEPLAY")]
        OutOfGameplay,
        [EnumMember(Value = "IN_GAMEPLAY")]
        InGameplay,
        [EnumMember(Value = "STORE")]
        Store
    }

    [DataContract, Preserve]
    public class PresenceBroadcastEventPayload
    {
        [DataMember(Name = "flight_id")] public string FlightId;
        [DataMember(Name = "platform_info")] public string PlatformInfo;
        [DataMember(Name = "game_state")] public string GameState;
        [DataMember(Name = "game_context")] public string GameContext;
        [DataMember(Name = "additional_data")] public Dictionary<string, object> AdditionalData;
    }
}