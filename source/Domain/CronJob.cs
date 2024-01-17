using API.Model.Enums;

namespace API.Domain
{
    public sealed class CronJob : Entity
    {

        public CronJob() { }

        public string Uri { get; set; }

        public HttpMethodEnum HttpMethod { get; set; }

        public string Body { get; set; }

        public string Schecule { get; set; }

        public string TimeZone { get; set; }
    }

}
