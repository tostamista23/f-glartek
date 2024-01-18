namespace API.Model;

public sealed record TimeZoneDTO
{
    public string value { get; set; }
    public string abbr { get; set; }
    public double offset { get; set; }
    public bool isdst { get; set; }
    public string text { get; set; }
}
