using System.Runtime.Serialization;

namespace AccelByte.Models
{
    [DataContract]
    public class PublicIp
    {
        [DataMember] public string ip;
    }
}
