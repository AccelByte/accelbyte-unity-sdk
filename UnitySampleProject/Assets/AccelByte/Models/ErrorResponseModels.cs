using System.Runtime.Serialization;

namespace AccelByte.Models
{
    [DataContract]
    public class ServiceError
    {
        [DataMember] public int numericErrorCode { get; set; }
        [DataMember] public string errorMessage { get; set; }
    }

    [DataContract]
    public class OAuthError
    {
        [DataMember] public string error { get; set; }
        [DataMember] public string error_description { get; set; }
        [DataMember] public string error_uri { get; set; }
    }
}
