using AccelByte.Core;

namespace Tests.UnitTests
{
    public class MockHttpResponse : IHttpResponse
    {
        public string Url { get; set; }
        public long Code { get; set; }
        public byte[] BodyBytes { get; set; }

        public string Body
        {
            get => System.Text.Encoding.UTF8.GetString(BodyBytes);
            set => this.BodyBytes = System.Text.Encoding.UTF8.GetBytes(value);
        }
    }
}