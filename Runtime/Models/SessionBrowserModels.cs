// Copyright (c) 2022 - 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using UnityEngine.Scripting;

namespace AccelByte.Models
{

    [DataContract, Preserve]
    public enum SessionType
    {
        [EnumMember] none,
        [EnumMember] p2p,
        [EnumMember] dedicated
    }

    [DataContract, Preserve]
    public class SessionBrowserStatusHistory
    {
        [DataMember] public string status;
        [DataMember] public System.DateTime time_stamp;

    }

    [DataContract, Preserve]
    public class SessionBrowserServer
    {
        [DataMember] public string allocation_id;
        [DataMember] public string[] alternate_ips;
        [DataMember] public int cpu_limit;
        [DataMember] public string cpu_request;
        [DataMember] public string deployment;
        [DataMember] public string game_version;
        [DataMember] public string image_version;
        [DataMember] public string ip;
        [DataMember] public bool is_override_game_version;
        [DataMember] public string last_update;
        [DataMember] public int mem_limit;
        [DataMember] public string mem_request;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember(Name = "params")] public string Params;
        [DataMember] public string pod_name;
        [DataMember] public int port;
        [DataMember] public int[] ports;
        [DataMember] public string provider;
        [DataMember] public string region;
        [DataMember] public string session_id;
        [DataMember] public string status;
        [DataMember] public SessionBrowserStatusHistory[] status_history;
    }

    [DataContract, Preserve]
    public class SessionBrowserGameSetting
    {
        [DataMember] public string mode = "";
        [DataMember] public string map_name = "";
        [DataMember] public int num_bot = 0;
        [DataMember] public int max_player = 1;
        [DataMember] public int current_player = 0;
        [DataMember] public int max_internal_player = 1;
        [DataMember] public int current_internal_player = 0;
        [DataMember] public bool allow_join_in_progress = true;
        [DataMember] public string password = "";
        [DataMember] public Newtonsoft.Json.Linq.JObject settings;
    }

    [DataContract, Preserve]
    public class SessionBrowserCreateRequest
    {
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember] public SessionType session_type;
        [DataMember] public string username;
        [DataMember] public string game_version;
        [DataMember] public SessionBrowserGameSetting game_session_setting;
    }

    [DataContract, Preserve] //apidocs: SessionResponse
    public class SessionBrowserData
    {
        /// <summary>
        /// All players including player that leave the session.
        /// </summary>
        [DataMember] public string[] all_players;
        [DataMember] public System.DateTime created_at;
        [DataMember] public SessionBrowserGameSetting game_session_setting;
        [DataMember] public string game_version;
        [DataMember] public bool joinable;
        [DataMember] public AccelByte.Models.MatchmakingResult match;
        [DataMember(Name = "namespace")] public string Namespace;
        /// <summary>
        /// Current active players
        /// </summary>
        [DataMember] public string[] players;
        [DataMember] public SessionBrowserServer server;
        [DataMember] public string session_id;
        [DataMember] public SessionType session_type;
        [DataMember] public string[] spectators;
        [DataMember] public string user_id;
        [DataMember] public string username;
    }

    [DataContract, Preserve]
    public class SessionBrowserUpdateSessionRequest
    {
        [DataMember] public int game_current_player;
        [DataMember] public int game_max_player;
    }

    public class SessionBrowserQuerySessionFilter
    {
        public SessionType? sessionType = SessionType.none;
        public string gameMode = "";
        public int? offset = null;
        public int? limit = null;
        public bool? matchExist = null;
    }

    [DataContract, Preserve] //apidcos: SessionQueryResponse{
    public class SessionBrowserGetResult
    {
        [DataMember] public AccelByte.Models.PagingCursor pagination;
        [DataMember] public SessionBrowserData[] sessions;
    }
    
    [DataContract, Preserve]
    public class SessionBrowserGetByUserIdsResult
    {
        [DataMember] public SessionBrowserData[] data;
    }

    [DataContract, Preserve]
    public class SessionBrowserAddPlayerRequest
    {
        [DataMember] public string user_id;
        [DataMember] public bool as_spectator;
    }

    [DataContract, Preserve]
    public class SessionBrowserAddPlayerResponse
    {
        [DataMember] public bool status;
    }

    [DataContract, Preserve]
    public class SessionBrowserRemovePlayerResponse : SessionBrowserAddPlayerResponse {}

    [DataContract, Preserve] //apidocs: RecentPlayerHistory
    public class SessionBrowserRecentPlayerData
    {
        [DataMember] public System.DateTime created_at;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember] public string other_display_name;
        [DataMember] public string other_id;
        [DataMember] public System.DateTime updated_at;
        [DataMember] public string user_id;
    }

    [DataContract, Preserve] //apidocs: RecentPlayerQueryResponse
    public class SessionBrowserRecentPlayerGetResponse
    {
        [DataMember] public SessionBrowserRecentPlayerData[] data;
    }

    [DataContract, Preserve] //apidocs: JoinGameSessionRequest
    public class SessionBrowserJoinSessionRequest
    {
        [DataMember] public string password;
    }
}
