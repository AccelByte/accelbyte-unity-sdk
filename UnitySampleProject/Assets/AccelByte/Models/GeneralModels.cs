using System.Runtime.Serialization;

namespace AccelByte.Models
{
    [DataContract]
    public class Paging
    {
        [DataMember] public string first { get; set; }
        [DataMember] public string last { get; set; }
        [DataMember] public string next { get; set; }
        [DataMember] public string previous { get; set; }
    }
}