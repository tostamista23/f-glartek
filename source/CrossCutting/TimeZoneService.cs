using API.Model;
using DotNetCore.Results;
using System.Net;
using System.Text.Json;

namespace API.CrossCutting;

public static class TimeZoneService
{
    public static List<TimeZoneDTO> TimeZones { get; set; } = new();

    public static Result<List<TimeZoneDTO>> GetTimeZones()
    {
        if (TimeZones.Count == 0)
        {
            using (StreamReader r = new StreamReader("TimeZone.file.json"))
            {
                TimeZones = JsonSerializer.Deserialize<List<TimeZoneDTO>>(r.ReadToEnd()) ?? new ();
            }
        }

        return new Result<List<TimeZoneDTO>>(HttpStatusCode.OK, TimeZones);
    }

    public static TimeZoneDTO? GetTimeZone(string id)
    {
        if (TimeZones.Count == 0)
        {
            using (StreamReader r = new StreamReader("TimeZone.file.json"))
            {
                TimeZones = JsonSerializer.Deserialize<List<TimeZoneDTO>>(r.ReadToEnd()) ?? new ();
            }
        }

        return TimeZones.FirstOrDefault(x => x.text == id);
    }
}
