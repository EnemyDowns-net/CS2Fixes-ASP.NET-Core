using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CS2Fixes_ASP_DOTNET_Core.Entities;

public class Userpref
{
    public long Steamid { get; set; }
    public string Key { get; set; }
    public string Value { get; set; }
}
