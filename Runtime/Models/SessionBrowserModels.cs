// Copyright (c) 2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace AccelByte.Models
{

    [DataContract]
    public enum SessionType
    {
        [EnumMember] none,
        [EnumMember] p2p,
        [EnumMember] dedicated
    }

    [DataContract]
    public class SessionBrowserStatusHistory
    {
        [DataMember] public string status { get; set; }
        [DataMember] public System.DateTime time_stamp { get; set; }

    }

    [DataContract]
    public class SessionBrowserServer
    {
        [DataMember] public string allocation_id { get; set; }
        [DataMember] public string[] alternate_ips { get; set; }
        [DataMember] public int cpu_limit { get; set; }
        [DataMember] public string cpu_request { get; set; }
        [DataMember] public string deployment { get; set; }
        [DataMember] public string game_version { get; set; }
        [DataMember] public string image_version { get; set; }
        [DataMember] public string ip { get; set; }
        [DataMember] public bool is_override_game_version { get; set; }
        [DataMember] public string last_update { get; set; }
        [DataMember] public int mem_limit { get; set; }
        [DataMember] public string mem_request { get; set; }
        [DataMember(Name = "namespace")] public string Namespace { get; set; }
        [DataMember(Name = "params")] public string Params { get; set; }
        [DataMember] public string pod_name { get; set; }
        [DataMember] public int port { get; set; }
        [DataMember] public int[] ports { get; set; }
        [DataMember] public string provider { get; set; }
        [DataMember] public string region { get; set; }
        [DataMember] public string session_id { get; set; }
        [DataMember] public string status { get; set; }
        [DataMember] public SessionBrowserStatusHistory[] status_history { get; set; }
    }

    [DataContract]
    public class SessionBrowserGameSetting
    {
        [DataMember] public string mode { get; set; } = "";
        [DataMember] public string map_name { get; set; } = "";
        [DataMember] public int num_bot { get; set; } = 0;
        [DataMember] public int max_player { get; set; } = 1;
        [DataMember] public int current_player { get; set; } = 0;
        [DataMember] public int max_internal_player { get; set; } = 1;
        [DataMember] public int current_internal_player { get; set; } = 0;
        [DataMember] public bool allow_join_in_progress { get; set; } = true;
        [DataMember] public string password { get; set; } = "";
        [DataMember] public Newtonsoft.Json.Linq.JObject settings { get; set; }
    }

    [DataContract]
    public class SessionBrowserCreateRequest
    {
        [DataMember(Name = "namespace")] public string Namespace { get; set; }
        [DataMember] public SessionType session_type { get; set; }
        [DataMember] public string username { get; set; }
        [DataMember] public string game_version { get; set; }
        [DataMember] public SessionBrowserGameSetting game_session_setting { get; set; }
    }

    [DataContract] //apidocs: SessionResponse
    public class SessionBrowserData
    {
        /// <summary>
        /// All players including player that leave the session.
        /// </summary>
        [DataMember] public string[] all_players { get; set; }
        [DataMember] public System.DateTime created_at { get; set; }
        [DataMember] public SessionBrowserGameSetting game_session_setting { get; set; }
        [DataMember] public string game_version { get; set; }
        [DataMember] public bool joinable { get; set; }
        [DataMember] public AccelByte.Models.MatchmakingResult match { get; set; }
        [DataMember(Name = "namespace")] public string Namespace { get; set; }
        /// <summary>
        /// Current active players
        /// </summary>
        [DataMember] public string[] players { get; set; }
        [DataMember] public SessionBrowserServer server { get; set; }
        [DataMember] public string session_id { get; set; }
        [DataMember] public SessionType session_type { get; set; }
        [DataMember] public string[] spectators { get; set; }
        [DataMember] public string user_id { get; set; }
        [DataMember] public string username { get; set; }
    }

    [DataContract]
    public class SessionBrowserUpdateSessionRequest
    {
        [DataMember] public int game_current_player { get; set; }
        [DataMember] public int game_max_player { get; set; }
    }

    public class SessionBrowserQuerySessionFilter
    {
        public SessionType? sessionType = SessionType.none;
        public string gameMode = "";
        public int? offset = null;
        public int? limit = null;
        public bool? matchExist = null;
    }

    [DataContract] //apidcos: SessionQueryResponse{
    public class SessionBrowserGetResult
    {
        [DataMember] public AccelByte.Models.PagingCursor pagination { get; set; }
        [DataMember] public SessionBrowserData[] sessions { get; set; }
    }
    
    [DataContract]
    public class SessionBrowserGetByUserIdsResult
    {
        [DataMember] public SessionBrowserData[] data { get; set; }
    }

    [DataContract]
    public class SessionBrowserAddPlayerRequest
    {
        [DataMember] public string user_id { get; set; }
        [DataMember] public bool as_spectator { get; set; }
    }

    [DataContract]
    public class SessionBrowserAddPlayerResponse
    {
        [DataMember] public bool status { get; set; }
    }

    [DataContract]
    public class SessionBrowserRemovePlayerResponse : SessionBrowserAddPlayerResponse {}

    [DataContract] //apidocs: RecentPlayerHistory
    public class SessionBrowserRecentPlayerData
    {
        [DataMember] public System.DateTime created_at { get; set; }
        [DataMember(Name = "namespace")] public string Namespace { get; set; }
        [DataMember] public string other_display_name { get; set; }
        [DataMember] public string other_id { get; set; }
        [DataMember] public System.DateTime updated_at { get; set; }
        [DataMember] public string user_id { get; set; }
    }

    [DataContract] //apidocs: RecentPlayerQueryResponse
    public class SessionBrowserRecentPlayerGetResponse
    {
        [DataMember] public SessionBrowserRecentPlayerData[] data { get; set; }
    }

    [DataContract] //apidocs: JoinGameSessionRequest
    public class SessionBrowserJoinSessionRequest
    {
        [DataMember] public string password { get; set; }
    }
}
