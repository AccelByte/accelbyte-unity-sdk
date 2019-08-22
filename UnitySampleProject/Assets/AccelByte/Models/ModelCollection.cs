using System.Runtime.Serialization;

namespace AccelByte.Models
{
    [DataContract]
    public class Collection
    {
        [DataMember] public Slot[] slots;
        [DataMember] public UserGameProfiles[] userGameProfiles;
        [DataMember] public GameProfile[] gameProfiles;
        [DataMember] public OrderHistoryInfo[] orderHistories;
        [DataMember] public PlatformLink[] platformLinks;
        [DataMember] public PublicUserProfile[] publicUserProfiles;
    }
}
