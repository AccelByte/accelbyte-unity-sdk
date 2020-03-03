## Social Services

### Lobby

![lobby-api](http://www.plantuml.com/plantuml/png/TP3DgeCm44RtynI3Uz_WSj5xIw7q1y6bk8ZZL8CnAN5i4V7TAxLGA6GvdCF7S991b9UnDo1Q3EF9LTK4GCQDDSxmEl4dhjt3nbXagpjXplwkYSjl-jyg2SBCyy2ME2ZilMPR3q5-SQAlcfHePuFIMGcrlUgqRJxE0d1JrAc6CKg9sFnDmfZlZX7EK6mBQNOeSLlvVKTO85aVMjCy0G00)

Lobby is for chatting and party management. Unlike other servers which use HTTP, Lobby server uses WebSocket (RFC 6455).

Lobby is a collection of services that are connected together through a websocket connection. Those services are:

1. Party Service
2. Chat Service
3. Friends Service
4. Presence Service
5. Notification Service
6. Matchmaking Service

We have to connect to Lobby first

```csharp
string userId
var lobby = AccelBytePlugin.GetLobby();
lobby.Connect();
```

before we start interfacing with those services

### Lobby Protocol

AccelByte Lobby Protocol closely follows RPC model and is described by its message format, which is a subset of YAML. The message is divided into two parts: header and payload. Header fields are type, id, and code (optional), while payload fields depends on the type.

Request Example:

```text
type: someRequest\n
id: id123\n
payloadFieldBool1: true\n
payloadFieldDouble2: 2.0\n
payloadFieldInt3: 3\n
payloadFieldStr4: some text message
```

Response code 0 means the request returned an OK response, while other code means the request returned an error response. Request and Response come in pair, so that a pair of request and response have the same id, while an Notification doesn't have an id.

Response OK Example:

```text
type: someResponse\n
id: id123\n
code: 0\n
payloadFieldBool1: true\n
payloadFieldDouble2: 2.0\n
payloadFieldInt3: 3\n
payloadFieldStr4: some text message\n
payloadFieldStrArray5: [item1,item2,item3,item4]
```

Response Error Example:

```text
type: someResponse\n
id: id123\n
code: 14777
```

Notification/Event Example:

```text
type: someNotif\n
payloadFieldBool1: true\n
payloadFieldStrArray5: [item1]
```

You don't need to worry about this protocol. AccelByte SDK abstracted Request-Response as method call, while Notification will look like a C# event.

### Party

A party can be created by any user, but a user can only create a single party. Party creator is the leader of the party, which means he/she can invite or kick another user. There is no limit on how many members can be in a party.

```csharp
[DataContract]
public class PartyInfo
{
    [DataMember] public string partyID;
    [DataMember] public string leaderID;
    [DataMember] public string[] members;
    [DataMember] public string[] invitees;
    [DataMember] public string invitationToken;
}
```

#### Get Party Info

Get an already created party info

Usage:

```csharp
public static void OnGetPartyInfo(Result<PartyInfo> result)
{
    if (!result.IsError)
    {
        Debug.Log(result.Value.partyID);
    }
}

public static void Main(string[] args)
{
    var lobby = AccelBytePlugin.GetLobby();
    lobby.Connect();
    lobby.GetPartyInfo(OnGetPartyInfo);
}
```

#### Creating a Party

A user can only create one party. An attempt to create a second party will fail.

Usage:

```csharp
public static void OnCreateParty(Result<PartyInfo> result)
{
    if (!result.IsError)
    {
        Debug.Log(result.Value.partyID);
    }
}

public static void Main(string[] args)
{
    var lobby = AccelBytePlugin.GetLobby();
    lobby.Connect();
    lobby.CreateParty(OnCreateParty);
}
```

#### Inviting Another User to a Party

Party leader can invite another user to join his party.

Usage:


```csharp
public static void OnInvited(Result result)
{
    if (!result.IsError)
    {
        Debug.Log("Invited a user to your party");
    }
}

public static void Main(string[] args)
{
    var lobby = AccelBytePlugin.GetLobby();
    lobby.Connect();
    lobby.InviteToParty("anotherUserId", OnInvited);
}
```

#### Kick Another User from a Party

Party leader can kick a member out of his party

Usage:

```csharp
public static void OnMemberKicked(Result result)
{
    if (!result.IsError)
    {
        Debug.Log("Kicked a user from your party");
    }
}

public static void Main(string[] args)
{
    var lobby = AccelBytePlugin.GetLobby();
    lobby.Connect();
    lobby.KickPartyMember("anotherUserId", OnMemberKicked);
}
```

#### Leaving a Party

Party member (including party leader) can leave a party he is in.

Usage:

```csharp
public static void OnLeaveParty(Result result)
{
    if (!result.IsError)
    {
        Debug.Log("Leaved a party");
    }
}

public static void Main(string[] args)
{
    var lobby = AccelBytePlugin.GetLobby();
    lobby.Connect();
    lobby.LeaveParty(OnLeaveParty);
}
```

#### Joining a Party

A user must be invited before he can join a party. He / she can only join a single party.

Usage:

```csharp
public static PartyInvitation partyInvitation;

public static void OnInvitedToParty(Result<PartyInvitation> result)
{
    if (!result.IsError)
    {
        partyInvitation = result.Value;
    }
}

public static void OnJoinPartyClicked()
{
    var lobby = AccelBytePlugin.GetLobby();

    if (partyInvitation != null)
    {
        lobby.JoinParty(partyInvitation.partyId, partyInvitaion.invitationToken, OnJoinedParty);
    }
}

public static void OnJoinedParty(Result<PartyInfo> result)
{
    if (!result.IsError)
    {
        Debug.Log("Joined party " + result.partyID);
    }
}

public static void Main(string[] args)
{
    var lobby = AccelBytePlugin.GetLobby();
    lobby.InvitedToParty += OnInvitedToParty;
    lobby.Connect();
}
```

### Chat

#### Personal Chat

A user can chat directly another user with personal chat.

Usage:

```csharp
public static void OnSendChatClicked()
{
    var lobby = AccelBytePlugin.GetLobby();

    lobby.SendPersonalChat("anotherUserId", "Hi, how are you?", OnChatSent);
}

public static void OnChatSent(Result result)
{
    Debug.Log(result.IsError);
}

public static void OnChatReceived(Result<ChatMessage> result)
{
    if (!result.IsError)
    {
        Debug.Log(result.Value.from + " : " + result.Value.payload);
    }
}

public static void Main(string[] args)
{
    var lobby = AccelBytePlugin.GetLobby();
    lobby.PersonalChatReceived += OnChatReceived;
    lobby.Connect();
}
```

#### Party Chat

A user can send chat to all other user on his party.

Usage:

```csharp
public static void OnSendPartyChatClicked()
{
    var lobby = AccelBytePlugin.GetLobby();

    lobby.SendPartyChat("Hi party members!!!", OnPartyChatSent);
}

public static void OnPartyChatSent(Result result)
{
    Debug.Log(result.IsError);
}

public static void OnPartyChatReceived(Result<ChatMessage> result)
{
    if (!result.IsError)
    {
        Debug.Log(result.Value.from + " : " + result.Value.payload);
    }
}

public static void Main(string[] args)
{
    var lobby = AccelBytePlugin.GetLobby();
    lobby.PartyChatReceived += OnPartyChatReceived;
    lobby.Connect();
}
```

### Notifications

Lobby can also be used to send notification to all user. To make user receive system notification, subscribe to OnNotification event. Use **PullAsyncNotifications** to get all pending notification(s) that have been sent to the user while the user was not connected to lobby. Please call this function after the user connected to lobby.

```csharp
public static void OnReceiveNotification(Result<Notification> result)
{
    Debug.Log(result.IsError);
}

public static void Main(string[] args)
{
    var lobby = AccelBytePlugin.GetLobby();
    lobby.OnNotification += OnReceiveNotification;
    lobby.Connect();
    lobby.PullAsyncNotifications(result => {
            Debug.Log(result.IsError);
        });
}
```

### Friends

For a user to make friends with other users, he has to know other user id.

#### Requesting other user to be friend

The first step in making friend is to request other user to be friend.

Usage:

```csharp
public static void OnRequestFriend(Result result)
{
    Debug.Log(result.IsError);
}

public static void Main(string[] args)
{
    var lobby = AccelBytePlugin.GetLobby();
    lobby.Connect();
    lobby.RequestFriend("otherUserId", OnRequestFriend);
}
```

#### Accepting / rejecting a friend request

To complete a friend request, the other should accept / reject the request.

Usage:

 ```csharp
public static void CompleteFriendRequest(bool accept, string userId)
{
    var lobby = AccelBytePlugin.GetLobby();

    if (accept)
    {
        lobby.AcceptFriend(userId, OnAcceptFriend);
    }
    else
    {
        lobby.RejectFriend(userId, OnRejectFriend);
    }
}

public static void OnAcceptFriend(Result result)
{
    Debug.Log("Friend accepted");
}

public static void OnAcceptFriend(Result result)
{
    Debug.Log("Friend rejected");
}

public static void Main(string[] args)
{
    var lobby = AccelBytePlugin.GetLobby();
    lobby.Connect();
}
```

#### Getting list of friends

User can get list of other users who are already friend.

Usage:

```csharp
public static void OnLoadFriendsList(Result<Friends> result)
{
    if (!result.IsError)
    {
        foreach (string userId in result.Value.friendsId)
        {
            Debug.Log(userId);
        }
    }
}

public static void Main(string[] args)
{
    var lobby = AccelBytePlugin.GetLobby();
    lobby.Connect();
    lobby.LoadFriendsList(OnLoadFriendsList);
}
```

#### Getting list of incoming friends

User can get list of incoming friends (other users who has sent friend request to him/her)

Usage:

```csharp
public static void OnListIncomingFriends(Result<Friends> result)
{
    if (!result.IsError)
    {
        foreach (string userId in result.Value.friendsId)
        {
            Debug.Log(userId);
        }
    }
}

public static void Main(string[] args)
{
    var lobby = AccelBytePlugin.GetLobby();
    lobby.Connect();
    lobby.ListIncomingFriends(OnListIncomingFriends);
}
```

#### Getting list of outgoing friends

User can  get list of outgoing friends (other users who has not accepted his/her friend request)

Usage:

```csharp
public static void OnListOutgoingFriends(Result<Friends> result)
{
    if (!result.IsError)
    {
        foreach (string userId in result.Value.friendsId)
        {
            Debug.Log(userId);
        }
    }
}

public static void Main(string[] args)
{
    var lobby = AccelBytePlugin.GetLobby();
    lobby.Connect();
    lobby.ListOutgoingFriends(OnListOutgoingFriends);
}
```

#### Friends status

User can set his status and also can see his friends status.

Usage:

```csharp
public static void OnListFriendsStatus(Result<FriendsStatus> result)
{
    if (!result.IsError)
    {
        for (int i = 0; i < result.Value.friendsId.Length; i++)
        {
            Debug.Log(resut.Value.friendsId[i]);
            Debug.Log(resut.Value.availability[i]);
            Debug.Log(resut.Value.activity[i]);
        }

    }
}

public static void OnChangeStatus(UserStatus status, string activity)
{
    var lobby = AccelBytePlugin.GetLobby();
    lobby.SetUserStatus(status, activity, OnSetUserStatus);
}

public static void OnSetUserStatus(Result result)
{
    Debug.Log("Status changed");
}

public static void OnFriendStatusNotification(Result<FriendsStatusNotif> result)
{
    Debug.Log(resut.Value.userID);
    Debug.Log(resut.Value.availability);
    Debug.Log(resut.Value.activity);
}

public static void Main(string[] args)
{
    var lobby = AccelBytePlugin.GetLobby();
    lobby.FriendsStatusChanged += OnFriendStatusNotification;
    lobby.Connect();
    lobby.ListFriendsStatus(OnListFriendsStatus);
}
```

#### Unfriend a user

User can remove another user from their friends list by unfriending him / her

Usage:

```csharp
public static void OnUnfriend(Result result)
{
    Debug.Log(result.IsError);
}

public static void Main(string[] args)
{
    var lobby = AccelBytePlugin.GetLobby();
    lobby.Connect();
    lobby.Unfriend("otherUserId", OnUnfriend);
}
```

### Matchmaking

Party leader can start matchmaking. While waiting for matchmaking process, he/she can also cancel matchmaking. If match is found, a notification sent to all party members that matchmaking is done.

Usage:

```csharp
public static void OnReadyForMatchResponse(Result result){
    Debug.Log("Ready for match!");
}

public static void OnMatchMakingNotification(Result<MatchmakingNotif> result)
{
    if (!result.IsError)
    {
        if (result.Value.status == "done")
        {
            Debug.Log("Matchmaking done. Match found");
            var lobby = AccelBytePlugin.GetLobby();
            lobby.ConfirmReadyForMatch(result.Value.matchId, OnReadyForMatchResponse)
        }
        else if (result.Value.status == "cancel")
        {
            Debug.Log("Matchmaking cancelled.");
        }
    }
}

public static void OnReadyForMatchNotification(Result<ReadyForMatchConfirmation> result){
    Debug.Log(string.Format("User {0} confirmed ready for match.", result.Value.userId));
}

public static void OnRematchmakingNotification(Result<RematchmakingNotification> result){
    Debug.Log(string.Format("You're banned for {0} secs", result.Value.banDuration));
}

public static void OnDSNotification(Result<DSNotif> result){
    Debug.Log(string.Format(@"You've got a DS Notification: 
                                Status:     {0} |
                                Match ID:   {1} |
                                PodName:    {2} |
                                IP:         {3} |
                                Port:       {4} |",
                            result.Value.status,
                            result.Value.matchId,
                            result.Value.podName,
                            result.Value.ip,
                            result.Value.port));
}

public static void StartMatchMaking()
{
    var lobby = AccelBytePlugin.GetLobby();
    lobby.StartMatchmaking("game-mode", OnStartMatchMaking);
}

public static void OnStartMatchMaking(Result<MatchmakingCode> result)
{
    Debug.Log(result.IsError)
}

public static void CancelMatchMaking()
{
    var lobby = AccelBytePlugin.GetLobby();
    lobby.CancelMatchmaking("game-mode", OnCancelMatchmaking);
}

public static void OnCancelMatchmaking(Result<MatchmakingCode> result)
{
    Debug.Log(result.IsError)
}

public static void Main(string[] args)
{
    var lobby = AccelBytePlugin.GetLobby();
    lobby.MatchmakingCompleted += OnMatchMakingNotification;
    lobby.ReadyForMatchConfirmed += OnReadyForMatchNotification;
    lobby.RematchmakingNotif += OnRematchmakingNotification;
    lobby.DSUpdated += OnDSNotification;
    lobby.Connect();
}
```

## Game Profiles

User must be logged in before he can use game profile services.

### Get Public Game Profiles

User can get the other users game profiles information.

Usage:

```csharp
public static void Main(string[] args)
{
    var gameProfiles = AccelBytePlugin.GetGameProfiles();
    string[] userIds = new string[] 
    {
        this.user.Session.UserId,
        "some_random_user_id",
        "not_exist_user_id"
    };
    gameProfiles.BatchGetGameProfile(userIds, result => {
        // result type is Result<UserGameProfiles[]>
        // result.Value type is UserGameProfiles[]

        // showing some users game profiles
        Debug.Log(result.Value.Length);
    });
}
```

### Get All Game Profiles

User can get all their game profiles information.

Usage:

```csharp
public static void Main(string[] args)
{
    var gameProfiles = AccelBytePlugin.GetGameProfiles();
    gameProfiles.GetAllGameProfiles(result => {
        // result type is Result<GameProfile[]>
        // result.Value type is GameProfile[]

        // showing user's game profiles
        Debug.Log(result.Value.Length);
    });
}
```

### Get a Game Profile

User can get a game profile information of their.

Usage:

```csharp
public static void Main(string[] args)
{
    var gameProfiles = AccelBytePlugin.GetGameProfiles();
    gameProfiles.GetGameProfile("yourProfileId", result => {
        // result type is Result<GameProfile>
        // result.Value type is GameProfile

        // showing one of user's game profile
        Debug.Log(result.Value.profileId);
    });
} 
```

### Create a Game Profile

User can create a game profile.

Usage:

```csharp
public static void Main(string[] args)
{
    var gameProfiles = AccelBytePlugin.GetGameProfiles();
    var gameProfile = new GameProfileRequest
    {
        label = "GameProfile",
        profileName = "ProfileName",
        tags = new string[] {"tag1", "tag2", "tag3"},
        attributes = new Dictionary<string, string>() 
            {
                {"test", "test123"},
                {"name", "testName"}
            }
    }
    gameProfiles.CreateGameProfile(gameProfile,result => {
        // result type is Result<GameProfile>
        // result.Value type is GameProfile

        // showing one of user's game profile
        Debug.Log(result.Value.profileId);
    });
}
```

### Update a Game Profile

User can update their game profile information.

Usage:

```csharp
public static void Main(string[] args)
{
    var gameProfiles = AccelBytePlugin.GetGameProfiles();
    GameProfile gameProfile;
    gameProfiles.GetGameProfile("yourProfileId", result => {
        gameProfile = result.Value;
        Debug.Log(result.Value.profileId);
    });
    gameProfile.profileName = "changedProfileName";
    gameProfiles.UpdateGameProfile(gameProfile, result => {
        // result type is Result<GameProfile>
        // result.Value type is GameProfile

        // showing one of user's game profile
        Debug.Log(result.Value.profileId);
    });
}
```

### Delete a Game Profile

User can delete their game profile.

Usage:

```csharp
public static void Main(string[] args)
{
    var gameProfiles = AccelBytePlugin.GetGameProfiles();
    gameProfiles.DeleteGameProfile("yourProfileId", result => {
        if(!result.IsError())
        {
            Debug.Log("Delete Game Profile Success!");
        }
    });
}
```
---